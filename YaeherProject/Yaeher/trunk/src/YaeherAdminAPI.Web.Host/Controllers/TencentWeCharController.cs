using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Mvc;
using Senparc.NeuChar;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.TenPay.V2;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.SystemHelper;
using Yaeher.Common.TencentCustom;
using Yaeher.Controllers;
using Yaeher.Release;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TencentWeCharController : YaeherControllerBase
    {
        private readonly IYaeherUserService _yaeherUserService;
        private readonly ISystemConfigsService _systemConfigsService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherUserPaymentService _yaeherUserPaymentService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAcceptTencentWecharService _acceptTencentWecharService;
        private readonly IAcceptWecharStateService _acceptWecharStateService;
        private readonly ISystemTokenService _systemTokenService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
       /// <summary>
       /// Home 构造函数
       /// </summary>
       /// <param name="yaeherUserService"></param>
       /// <param name="systemConfigsService"></param>
       /// <param name="systemParameterService"></param>
       /// <param name="yaeherUserPaymentService"></param>
       /// <param name="unitOfWorkManager"></param>
       /// <param name="acceptTencentWecharService"></param>
       /// <param name="acceptWecharStateService"></param>
       /// <param name="systemTokenService"></param>
       /// <param name="yaeherDoctorService"></param>
        public TencentWeCharController(IYaeherUserService yaeherUserService,
                                       ISystemConfigsService systemConfigsService,
                                       ISystemParameterService systemParameterService,
                                       IYaeherUserPaymentService yaeherUserPaymentService,
                                       IUnitOfWorkManager unitOfWorkManager,
                                       IAcceptTencentWecharService acceptTencentWecharService,
                                       IAcceptWecharStateService acceptWecharStateService,
                                       ISystemTokenService systemTokenService,
                                       IYaeherDoctorService yaeherDoctorService)
        {
            _yaeherUserService = yaeherUserService;
            _systemConfigsService = systemConfigsService;
            _systemParameterService = systemParameterService;
            _yaeherUserPaymentService = yaeherUserPaymentService;
            _unitOfWorkManager = unitOfWorkManager;
            _acceptTencentWecharService = acceptTencentWecharService;
            _acceptWecharStateService = acceptWecharStateService;
            _systemTokenService = systemTokenService;
            _yaeherDoctorService = yaeherDoctorService;
        }

        #region 微信绑定urltoken接口
        /// <summary>
        /// 微信绑定urltoken接口
        /// </summary>
        /// <returns></returns>
        [Route("api/WXMSG")]
        [HttpGet]
        [AbpAllowAnonymous]
        public ActionResult wxmsg(string signature, string timestamp, string nonce, string echostr)
        {
            try
            {
                if (CheckSignature.Check(signature, timestamp, nonce, Commons.WXToken ))
                {
                    return Content(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    return Content("failed:" + signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(timestamp, nonce, Commons.WXToken) + "。如果您在浏览器中看到这条信息，表明此Url可以填入微信后台。");
                }
            }
            catch (Exception ex)
            {
                return Content("failed:" + signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(timestamp, nonce, Commons.WXToken) + "。如果您在浏览器中看到这条信息，表明此Url可以填入微信后台。");
            }
        }
        #endregion


        /// <summary>
        /// 微信消息事件接口
        /// </summary>
        /// <returns></returns>
        [Route("api/WXMSG")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ActionResult> WXMSG(string signature, string timestamp, string nonce, string echostr)
        {
            if (!CheckSignature.Check(signature, timestamp, nonce, Commons.WXToken))
            {
                return Content("参数错误！");
            }
            try
            {
                XDocument requestDoc = null;
                requestDoc = XDocument.Load(Request.Body);
                var requestMessage = RequestMessageFactory.GetRequestEntity(requestDoc);
                // 关注 取消 事件
                if (requestMessage.MsgType == RequestMsgType.Event)
                {
                    RequestMessageEventBase msg = (RequestMessageEventBase)requestMessage;
                    if (msg.Event == Event.subscribe)
                    {
                        await FocusOn(requestDoc);  //关注

                    }
                    if (msg.Event == Event.unsubscribe)
                    {

                       await UnFocusOn(requestDoc); //取消关注
                    }
                }
                else
                {
                    await AcceptWechar(requestDoc);// 发送的消息
                }
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }


        #region 公众号关注事件
        /// <summary>
        /// 公众号关注事件
        /// </summary>
        /// <param name="requestDoc"></param>
        /// <returns></returns>
        public async Task<ObjectResultModule> FocusOn(XDocument requestDoc)
        {
            SystemToken systemToken = new SystemToken();
            TencentUserManage usermanage = new TencentUserManage();
            TencentWXPay tencentWXPay = new TencentWXPay();
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                try
                {
                    //关注
                    var openid = JsonHelper.GetXmlValue(requestDoc, "FromUserName");
                    if (openid == null)
                    {
                        return new ObjectResultModule("", 401, "");
                    }
                   // systemToken.TokenType = "Wechar";
                    var Tokens = _systemTokenService.SystemTokenList("Wechar").Result;
                    string access_token = Tokens.access_token;
                    var usermsg = usermanage.WeiXinUserInfoUtils(openid, Tokens.access_token).Result;
                    if (usermsg.subscribe != 0)
                    {
                        #region  同步操作
                        //var UserInfo = _yaeherUserService.YaeherUserInfo(openid, Tokens.access_token);
                        //if (UserInfo.Id > 0)
                        //{
                        //    UserInfo= await usermanage.YaeherUserLable(usermsg,UserInfo,Tokens.access_token);
                        //    if (!UserInfo.IsPay)
                        //    {
                        //        var payment = await _yaeherUserPaymentService.YaeherUserPaymentByUserID(UserInfo.Id);
                        //        if (payment == null || payment.Id < 1)
                        //        {
                        //            //http请求微信信息，获取账户的信息 新增用户payment
                        //            var CreateUserPayment = new YaeherUserPayment()
                        //            {
                        //                UserID = UserInfo.Id,
                        //                FullName = UserInfo.FullName,
                        //                PayMethod = "wxpay",
                        //                PayMethodName = "微信支付",
                        //                PaymentAccout = UserInfo.WecharName,
                        //                BankName = "wx",
                        //                Subbranch = "wx",
                        //                BandAdd = "wx",
                        //                BankNo = "wx",
                        //                CreatedOn = DateTime.Now,
                        //                IsDefault = true,
                        //            };
                        //            CreateUserPayment = await _yaeherUserPaymentService.CreateYaeherUserPayment(CreateUserPayment);
                        //        }
                        //        UserInfo.IsPay = true;
                        //    }
                        //    if (!UserInfo.IsUpdate)
                        //    {
                        //        var DoctorInfo = await _yaeherDoctorService.YaeherDoctorByUserID(UserInfo.Id);
                        //        if (DoctorInfo != null && DoctorInfo.IsSharing && UserInfo.IsProfitSharing == false)   //医生角色切没有生成分账账号
                        //        {
                        //            //查询分账配置
                        //            SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
                        //            systemConfigsIn.AndAlso(a => a.IsDelete == false);
                        //            systemConfigsIn.AndAlso(a => a.SystemType == "TencentWechar");
                        //            var configs = _systemConfigsService.SystemConfigsList(systemConfigsIn).Result;
                        //            // 查询医生信息
                        //            var tencentparam = configs.ToList().FirstOrDefault();
                        //            var receiver = new receiver();
                        //            receiver.name = DoctorInfo.DoctorName;
                        //            receiver.type = "PERSONAL_OPENID";
                        //            receiver.account = openid;
                        //            var addresult = tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam).Result;
                        //            if (addresult.result_code == "SUCCESS")  //插入成功后更新状态
                        //            {
                        //                UserInfo.IsProfitSharing = true;
                        //            }
                        //        }
                        //    }
                        //    UserInfo = await _yaeherUserService.UpdateYaeherUser(UserInfo);
                        //}
                        #endregion
                        string OperType = "用户关注";
                        var UserInfo =await _yaeherUserService.YaeherUserInfo(usermsg,access_token,OperType);
                    }
                    StringBuilder Contentsb = new StringBuilder();
                    // 增加问医生
                    Contentsb.Append("欢迎关注怡禾健康！");
                    Contentsb.Append("\n");
                    Contentsb.Append("\n");
                    // 增加问医生
                    Contentsb.Append("<a href=\"" + Commons.WecharWeb + "?link=index-patient\">问医生</a>\n");
                    // 增加听课程
                    Contentsb.Append("\n");
                    Contentsb.Append("<a href=\"https://m.qlchat.com/wechat/page/live/210000155031160\">听课程</a>\n");
                    var textSingleMessage = new TextSingleMessage()
                    {
                        ToUser = openid,
                        TextContent = Contentsb.ToString() == null ? "欢迎关注怡禾健康！" : Contentsb.ToString()
                    };
                    textSingleMessage.Send(access_token).ToString();
                    unitOfWork.Complete();
                }
                catch(Exception ex)
                {
                    Logger.Info("我的一个关注的错误：" + ex.ToString() + "DateTime:" + DateTime.Now);
                }
            }
            return new ObjectResultModule("", 200, "");
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="requestDoc"></param>
        /// <returns></returns>
        public async Task<ObjectResultModule> UnFocusOn(XDocument requestDoc)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                //取消关注
                var openid = JsonHelper.GetXmlValue(requestDoc, "FromUserName");
                if (openid== null)
                {
                     return new ObjectResultModule("", 401, "");
                }
               // SystemToken systemToken = new SystemToken();
               // systemToken.TokenType = "Wechar";
                var Tokens = _systemTokenService.SystemTokenList("Wechar").Result;
                string access_token = Tokens.access_token;
                var UserInfo =await _yaeherUserService.YaeherUserByExpress(a=>a.WecharOpenID==openid && a.IsDelete==false);
                if (UserInfo != null)
                {
                    YaeherUser yaeherUser = UserInfo;
                    yaeherUser.IsLabel = false;
                    yaeherUser.IsPay = false;
                    yaeherUser.IsUpdate = false;
                    yaeherUser.IsProfitSharing = false;
                    UserInfo =await _yaeherUserService.UpdateYaeherUser(yaeherUser);
                }
                unitOfWork.Complete();
            }
            return new ObjectResultModule("", 200, "");
        }
        #endregion

        #region 接受微信发送消息
        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="requestDoc"></param>
        /// <returns></returns>
        public async Task<ObjectResultModule> AcceptWechar(XDocument requestDoc)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                // 将接收到的消息存储
                AcceptTencentWechar acceptTencentWechar = new AcceptTencentWechar();
                acceptTencentWechar.ToUserName = JsonHelper.GetXmlValue(requestDoc, "ToUserName");
                acceptTencentWechar.FromUserName = JsonHelper.GetXmlValue(requestDoc, "FromUserName");
                acceptTencentWechar.CreateTime = JsonHelper.GetXmlValue(requestDoc, "CreateTime");
                acceptTencentWechar.MsgType = JsonHelper.GetXmlValue(requestDoc, "MsgType");
                acceptTencentWechar.Content = JsonHelper.GetXmlValue(requestDoc, "Content");
                acceptTencentWechar.PicUrl = JsonHelper.GetXmlValue(requestDoc, "PicUrl");
                acceptTencentWechar.MediaId = JsonHelper.GetXmlValue(requestDoc, "MediaId");
                acceptTencentWechar.ThumbMediaId = JsonHelper.GetXmlValue(requestDoc, "ThumbMediaId");
                acceptTencentWechar.Format = JsonHelper.GetXmlValue(requestDoc, "Format");
                acceptTencentWechar.Recognition = JsonHelper.GetXmlValue(requestDoc, "Recognition");
                acceptTencentWechar.Title = JsonHelper.GetXmlValue(requestDoc, "Title");
                acceptTencentWechar.Description = JsonHelper.GetXmlValue(requestDoc, "Description");
                acceptTencentWechar.Url = JsonHelper.GetXmlValue(requestDoc, "Url");
                acceptTencentWechar.Location_X = JsonHelper.GetXmlValue(requestDoc, "Location_X");
                acceptTencentWechar.Location_Y = JsonHelper.GetXmlValue(requestDoc, "Location_Y");
                acceptTencentWechar.Scale = JsonHelper.GetXmlValue(requestDoc, "Scale");
                acceptTencentWechar.Label = JsonHelper.GetXmlValue(requestDoc, "Label");
                acceptTencentWechar.MsgId = JsonHelper.GetXmlValue(requestDoc, "MsgId");
                acceptTencentWechar.MessageFrom = "Consultant";
                if (acceptTencentWechar.FromUserName==null)
                {
                    return new ObjectResultModule("", 401, "");
                }
               // SystemToken systemToken = new SystemToken();
              //  systemToken.TokenType = "Wechar";
                var Tokens = _systemTokenService.SystemTokenList("Wechar").Result;
                string access_token = Tokens.access_token;
                // 检测用户信息
                YaeherUser YaerherUser = new YaeherUser();
                try
                {
                    TencentUserManage usermanage = new TencentUserManage();
                     var usermsg = usermanage.WeiXinUserInfoUtils(acceptTencentWechar.FromUserName, Tokens.access_token).Result;
                     if (usermsg.subscribe != 0)
                     {
                        string OperType = "用户咨询客服";
                        YaerherUser =await _yaeherUserService.YaeherUserInfo(usermsg,access_token,OperType);
                        #region 检测发送消息状态
                        acceptTencentWechar.FullName = YaerherUser.FullName;// 用户全称
                        acceptTencentWechar.CreatedBy = YaerherUser.Id;   // 用户ID
                        acceptTencentWechar.UserImage = YaerherUser.UserImage;  // 增加用户图像
                        acceptTencentWechar.NickName = YaerherUser.NickName;   // 增加用户昵称
                        //检测是否客服跟进
                        AcceptWecharStateIn acceptWecharStateIn = new AcceptWecharStateIn();
                        acceptWecharStateIn.AndAlso(a => a.ConsultantOpenID == acceptTencentWechar.FromUserName);
                        var StateList = await _acceptWecharStateService.WecharStateList(acceptWecharStateIn);
                        AcceptWecharState acceptWecharState = StateList.FirstOrDefault();
                        bool OverTime = false;
                        string AcceptState = string.Empty;
                        if (acceptWecharState!=null )
                        {
                            OverTime =acceptWecharState.AcceptTime.AddHours(2) < DateTime.Now;
                            AcceptState = acceptWecharState.AcceptState;
                            if (OverTime == true || acceptWecharState.AcceptState == "3")
                            {
                                // 需要根据当前客服排班记录查询
                                acceptWecharState.CustomerServiceID = 0;
                                acceptWecharState.CustomerServiceName = "客服";
                                acceptWecharState.CustomerServiceJson = "客服";
                            }
                            acceptWecharState.AcceptState = "2";
                            acceptWecharState.UserImage = YaerherUser.UserImage;  // 增加用户图像
                            acceptWecharState.NickName = YaerherUser.NickName;   // 增加用户昵称
                            acceptWecharState.ConsultantID = YaerherUser.Id;
                            acceptWecharState.ConsultantName = YaerherUser.FullName;
                            acceptWecharState.ConsultantJSON = JsonHelper.ToJson(YaerherUser);
                            acceptWecharState.IsReady = false;
                            acceptWecharState.AcceptTime = DateTime.Now;
                            var UpdateState = await _acceptWecharStateService.UpdateAcceptWecharState(acceptWecharState);
                        }
                        else
                        {
                            AcceptWecharState WecharState = new AcceptWecharState();
                            // 需要根据当前客服排班记录查询
                            WecharState.CustomerServiceID = 0;
                            WecharState.CustomerServiceName = "客服";
                            WecharState.CustomerServiceJson = "客服";
                            WecharState.UserImage = YaerherUser.UserImage;  // 增加用户图像
                            WecharState.NickName = YaerherUser.NickName;    // 增加用户昵称
                            WecharState.ConsultantID = YaerherUser.Id;
                            WecharState.ConsultantName = YaerherUser.FullName;
                            WecharState.ConsultantJSON = JsonHelper.ToJson(YaerherUser);
                            WecharState.ConsultantOpenID = acceptTencentWechar.FromUserName;
                            WecharState.AcceptState = "2";
                            WecharState.IsReady = false;
                            WecharState.AcceptTime = DateTime.Now;
                            //插入回复状态
                            var CreateState = await _acceptWecharStateService.CreateAcceptWecharState(WecharState);
                        }
                        #endregion
                        // 插入接受消息
                        var Result = await _acceptTencentWecharService.CreateAcceptTencent(acceptTencentWechar);
                        #region 关键字检索
                        string Content = string.Empty;
                        // 将特殊符过滤掉
                        string KeyWord = new StringHepler().FilterString(acceptTencentWechar.Content).Trim();
                        if (string.IsNullOrEmpty(KeyWord))
                        {
                            Content = null;
                        }
                        else
                        {
                            Content = _acceptTencentWecharService.SendWecharMesaage(KeyWord).Result;
                        }
                        if (string.IsNullOrEmpty(Content))
                        {
                            Content = null;
                        }
                        #endregion
                        // 当没客服跟进时先发送一条短信
                        if (acceptWecharState==null || AcceptState=="3"|| OverTime || Content!=null)
                        {
                            StringBuilder Contentsb = new StringBuilder();
                            Contentsb.Append("<a href=\"https://mp.weixin.qq.com/s/20iAOwP8Gzrq3zCdwMASng\">客服常见问题</a>\n");
                            Contentsb.Append("\n");
                            Contentsb.Append("客服工作时间： \n");
                            Contentsb.Append("周一到周五 9:00-17:30 \n");
                            Contentsb.Append("收到消息后我们会尽快反馈。\n");
                            Contentsb.Append("非工作时间请留言。\n");
                            // 增加问医生
                            Contentsb.Append("\n");
                            Contentsb.Append("<a href=\"" + Commons.WecharWeb + "?link=index-patient\">查看所有医生</a>    ");
                            if(Content!=null)
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                stringBuilder.Append(Content);
                                // 增加问医生
                                stringBuilder.Append("\n");
                                stringBuilder.Append("<a href=\"" + Commons.WecharWeb + "?link=index-patient\">查看所有医生</a>    ");

                                Content = stringBuilder.ToString();
                            }
                    
                            // 回复消息
                            var textSingleMessage = new TextSingleMessage()
                            {
                                ToUser = acceptTencentWechar.FromUserName,
                                TextContent = Content == null ? Contentsb.ToString() : Content
                            };
                            textSingleMessage.Send(Tokens.access_token).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Info("我是一个发送消息的错误：" + ex.ToString() + "DateTime:" + DateTime.Now);
                }
                unitOfWork.Complete();
            }
            return new ObjectResultModule("", 200, "");
        }
        #endregion

        #region 获取微信js-sdk授权
        /// <summary>
        /// 获取微信js-sdk授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/WXJSTicket")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> WXJSTicket([FromBody]TencentTicletModel input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            #region 获取微信系统参数
            //SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
            //systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
            //var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
            //var tencentparam = configs.FirstOrDefault();

            //TencentWeCharEntity tencentWeCharEntity = new TencentWeCharEntity();
            //tencentWeCharEntity.grant_type = "authorization_code";
            //tencentWeCharEntity.appid = tencentparam.AppID;
            //tencentWeCharEntity.secret = tencentparam.AppSecret;

           // SystemToken systemToken = new SystemToken();
          //  systemToken.TokenType = "Wechar";
            var Tokens = await _systemTokenService.SystemTokenList("Wechar");
            #endregion
            SystemTokenIn Token = new SystemTokenIn();
            Token.access_token = Tokens.access_token;
            Token.Appid = Tokens.Appid;
            Token.AppSecret = Tokens.AppSecret;
            Token.YaeherPlatform = Tokens.YaeherPlatform;
            var JSTicketTokens = await _systemTokenService.JSWecharTokenList("JSWechar", Token);
            //var result = await tencentoken.TencentTicket("jsapi",Tokens.access_token);

            var nonce_str = TenPayUtil.GetNoncestr();
            string timeStamp = TenPayUtil.GetTimestamp();
            var requestHandler2 = new RequestHandler(null);
            requestHandler2.SetParameter("noncestr", nonce_str);
            requestHandler2.SetParameter("jsapi_ticket", JSTicketTokens.access_token);
            requestHandler2.SetParameter("timestamp", timeStamp);
            requestHandler2.SetParameter("url", input.url);
            ////SHA1加密签名
            var sha1Sign = requestHandler2.CreateSHA1Sign();
            TencentTicletResponseModel model = new TencentTicletResponseModel();
            model.appId = Tokens.Appid;
            model.nonceStr = nonce_str;
            model.signature = sha1Sign;
            model.timestamp = timeStamp;
            return new ObjectResultModule(model, 200, "success");

        }
        #endregion

    }
}