using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using Yaeher.Controllers;
using Abp.BackgroundJobs;
using System;
using Hangfire;
using System.IO;
using Abp.Authorization;
using Senparc.Weixin.MP;
using Yaeher.Common.Constants;
using System.Xml.Linq;
using Senparc.Weixin.TenPay.V3;
using Yaeher.Common.TencentCustom;
using Yaeher.Common;
using Yaeher.SystemConfig;
using Yaeher;
using System.Collections.Generic;
using System.Linq;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;
using Abp.Runtime.Session;
using Abp.Domain.Uow;
using Yaeher.Consultation;
using Yaeher.EventBus;
using Yaeher.SystemManage;
using Yaeher.Common.HangfireJob;
using Yaeher.HangFire;

namespace YaeherPatientAPI.Web.Host.Controllers
{
    public class HomeController : YaeherControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IUserManagerService _userManagerService;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IRefundManageService _refundManageService;
        private readonly ILeaguerInfoService _leaguerservice;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _IabpSession;
        private readonly IAttachmentServices _attachmentService;
        private readonly IConsultationService _consultationService;
        private readonly IPublishsService _publishsService;
        private readonly IYaeherOperListService _YaeherOperListrepository;
        public HomeController(
                                      IConsultationService consultationService,
                                      IOrderManageService orderManageService,
                                      IOrderTradeRecordService orderTradeRecordService,
                                      IRefundManageService refundManageService,
                                      ILeaguerInfoService leaguerservice,
                                      IUnitOfWorkManager unitOfWorkManager,
                                      IAttachmentServices attachmentServices,
                                      IUserManagerService userManagerService,
                                      IPublishsService publishsService,
                                      IAbpSession session,
                                      IYaeherOperListService yaeherOperListService,
                                      INotificationPublisher notificationPublisher)
        {
            _consultationService = consultationService;
            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _refundManageService = refundManageService;
            _leaguerservice = leaguerservice;
            _unitOfWorkManager = unitOfWorkManager;
            _IabpSession = session;
            _attachmentService = attachmentServices;
            _userManagerService = userManagerService;
            _publishsService = publishsService;
            _YaeherOperListrepository = yaeherOperListService;
            _notificationPublisher = notificationPublisher;
        }

        public IActionResult Index()
        {
            //BackgroundJob.Schedule(() => NewJob(), TimeSpan.FromMinutes(3));
            //RecurringJob.AddOrUpdate(() => NewJob(), Cron.Minutely(), null, "jobs");
            return Redirect("/swagger/index.html");
        }
        private static object ojb = new object();
        public static bool CreateWrite(string path, string context)
        {
            bool b = false;
            try
            {
                lock (ojb)
                {
                    context += DateTime.UtcNow;
                    FileStream fs = new FileStream(path, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    //开始写入
                    sw.Write(context);
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                    return b;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return b;
            }
        }
        public async Task NewJob()
        {

            CreateWrite("D:\\学习文档.txt", "ASP .NET CORE HangFire");
        }
        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }
        /// <summary>
        /// 支付完成调用接口
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationPaid")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationPaid([FromBody] YaeherConsultationAdd ConsultationInfo)
        {
            //var secret = await CreateSecret();
            //var Content1 = "{\"SystemType\":\"TencentWechar\",\"secret\":\"" + secret + "\"}";
            //var tencentmparam = await this.PostResponseAsync(Commons.AdminIp + "api/SystemConfigsList/", Content1);
            //var tencentmparamlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemConfigs>>>>(tencentmparam);
            //if (tencentmparamlist == null || tencentmparamlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            //var tencentparam = tencentmparamlist.result.item.FirstOrDefault();
            //  TencentWXPay tencentWXPay = new TencentWXPay();
            //var queryresult = await tencentWXPay.OrderQueryAsync("201901180408ZIJ5191219", tencentparam);
            return new ObjectResultModule("", 200, "success");
            //Logger.Info("ConsultationPaid:"+JsonHelper.ToJson(ConsultationInfo));
            //if (!Commons.CheckSecret(ConsultationInfo.Secret))
            //{
            //    this.ObjectResultModule.StatusCode = 422;
            //    this.ObjectResultModule.Message = "Wrong Secret";
            //    this.ObjectResultModule.Object = "";
            //    return this.ObjectResultModule;
            //}
            //var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //TencentWXPay tencentWXPay = new TencentWXPay();
            //string message = "";
            //string WXPayBillno = "";
            //string WXTransactionId = "";
            //string WXOrderQuery = "";
            //var consul = await _consultationService.YaeherConsultationByNumber(ConsultationInfo.ConsultNumber);
            //if (consul == null || consul.Id < 1 || consul.CreatedBy != userid) { return new ObjectResultModule("", 400, "信息查询错误，请联系官方客服！"); }
            //WXPayBillno = "CN-" + ConsultationInfo.sp_billno;
            //var secret = await CreateSecret();
            //var Content1 = "{\"SystemType\":\"TencentWechar\",\"secret\":\"" + secret + "\"}";
            //var tencentmparam = await this.PostResponseAsync(Commons.AdminIp + "api/SystemConfigsList/", Content1);
            //var tencentmparamlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemConfigs>>>>(tencentmparam);
            //if (tencentmparamlist == null || tencentmparamlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            //var tencentparam = tencentmparamlist.result.item.FirstOrDefault();

            //var Content = "{\"Id\":" + consul.DoctorID + ",\"secret\":\"" + secret + "\"}";
            //var doctoruser = await this.PostResponseAsync(Commons.AdminIp + "api/DoctorInformation", Content);
            //var doctorUserResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherDoctorInfo>>>(doctoruser);
            //if (doctorUserResult == null || doctorUserResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            //var ServiceResult = doctorUserResult.result.item.ServiceMoneyLists.Find(t => t.Id == consul.ServiceMoneyListId);
            //var queryresult = await tencentWXPay.OrderQueryAsync(WXPayBillno, tencentparam);
            //if (ServiceResult == null || ServiceResult.Id < 1 || ServiceResult.ServiceState == false)
            //{
            //    message = "医生该服务已下线,资金会原路返回您的账户，请稍后！";
            //    //Logger.Info("message:" + message);
            //    string outTradeNo = WXPayBillno;
            //    string outRefundNo = consul.ConsultNumber + "-" + DateTime.Now.ToString("yyyMMddhhmmss");
            //    var totalFee = int.Parse(queryresult.total_fee);//单位：分
            //    int refundFee = totalFee;
            //    var refundpayresult = await tencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);

            //    if (refundpayresult.code != "SUCCESS")
            //    {
            //        return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
            //    }
            //    return new ObjectResultModule("", 400, message);
            //}
            //OrderTradeRecordIn refundManageIncheck = new OrderTradeRecordIn();
            //refundManageIncheck.AndAlso(t => t.IsDelete == false);
            //refundManageIncheck.AndAlso(t => t.WXPayBillno == WXPayBillno);
            //var ordertradeCheck = await _orderTradeRecordService.DoctorOrderTradeRecordList(refundManageIncheck);
            //if (ordertradeCheck.Count > 0) { return new ObjectResultModule("", 400, "支付检查失败,请联系管理员！"); }
            //OrderTradeRecordIn refundManageIn = new OrderTradeRecordIn();
            //var StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //refundManageIn.DoctorId = consul.DoctorID;
            //refundManageIn.ServiceID = consul.ServiceMoneyListId;
            //refundManageIn.AndAlso(t => t.IsDelete == false && t.CreatedOn >= StartTime && t.CreatedOn < StartTime.AddDays(1));
            //var ordertradelist = await _orderTradeRecordService.DoctorOrderTradeRecordList(refundManageIn);
            //var paidtrade = ordertradelist.Where(t => t.PayMoney > 0).ToList();
            //var returntrade = ordertradelist.Where(t => t.PayMoney < 0);
            //var allconsultation = paidtrade.Count() - returntrade.Count();
            //if (ServiceResult.ServiceFrequency <= allconsultation)
            //{
            //    message = "医生该服务已满单,资金会原路返回您的账户，请稍后！";
            //    //Logger.Info("message:" + message);
            //    string outTradeNo = WXPayBillno;
            //    string outRefundNo = consul.ConsultNumber + "-" + DateTime.Now.ToString("yyyMMddhhmmss");
            //    var totalFee = int.Parse(queryresult.total_fee);//单位：分
            //    int refundFee = totalFee;
            //    //Logger.Info("outTradeNo:" + outTradeNo + "outRefundNo:" + outRefundNo + "totalFee:" + totalFee + "refundFee：" + refundFee);
            //    var refundpayresult = await tencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);
            //    //Logger.Info("refundpayresult:" + JsonHelper.ToJson(refundpayresult));
            //    if (refundpayresult.code != "SUCCESS")
            //    {
            //        return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
            //    }
            //    return new ObjectResultModule("", 400, message);
            //}
            //var YaeherUserPaymentResult = doctorUserResult.result.item.YaeherUserPayment.Find(t => t.IsDefault);
            //if (YaeherUserPaymentResult == null) { YaeherUserPaymentResult = doctorUserResult.result.item.YaeherUserPayment.Find(t => t.PayMethod == "wxpay"); }

            //if (!string.IsNullOrEmpty(ConsultationInfo.sp_billno))
            //{
            //    if (queryresult.result_code != "SUCCESS")
            //    {
            //        return new ObjectResultModule("", 400, "查询支付信息失败,请重新提交！");
            //    }

            //    if (int.Parse(queryresult.total_fee) != (int)(ServiceResult.ServiceExpense * 100))
            //    {
            //        message = "医生该服务已修改价格，资金会原路返回您的账户，请稍后！";
            //        //Logger.Info("message:" + message);
            //        string outTradeNo = WXPayBillno;
            //        string outRefundNo = consul.ConsultNumber + "-" + DateTime.Now.ToString("yyyMMddhhmmss");
            //        var totalFee = int.Parse(queryresult.total_fee);//单位：分
            //        int refundFee = totalFee;
            //        var refundpayresult = await tencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);

            //        if (refundpayresult.code != "SUCCESS")
            //        {
            //            return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
            //        }
            //        return new ObjectResultModule("", 400, message);
            //    }
            //    WXTransactionId = queryresult.transaction_id;
            //    WXOrderQuery = JsonHelper.ToJson(queryresult);
            //}
            //Content = "{\"SystemCode\":\"SystemOverTime\",\"secret\":\"" + secret + "\"}";
            //var replymparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", Content);
            //var replylistparam = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(replymparam);
            //if (replylistparam == null || replylistparam.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            //using (var unitOfWork = _unitOfWorkManager.Begin())
            //{
            //    consul.ConsultState = "created";
            //    consul.Overtime = DateTime.Now.AddDays(double.Parse(replylistparam.result.item[0].ItemValue));
            //    var result = await _consultationService.UpdateYaeherConsultation(consul);
            //    var OrderManage = new OrderManage();
            //    OrderManage.SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6);
            //    OrderManage.OrderNumber = "ON-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4);
            //    OrderManage.ConsultNumber = result.ConsultNumber;
            //    OrderManage.ConsultID = result.Id;
            //    OrderManage.ConsultType = ServiceResult.ServiceType;
            //    OrderManage.ConsultantID = userid;
            //    OrderManage.ConsultantName = result.ConsultantName;
            //    OrderManage.PatientID = result.PatientID;
            //    OrderManage.PatientName = result.PatientName;
            //    OrderManage.DoctorID = consul.DoctorID;
            //    OrderManage.DoctorName = doctorUserResult.result.item.DoctorInfo.DoctorName;
            //    OrderManage.OrderCurrency = "rmb";
            //    OrderManage.OrderMoney = Convert.ToDecimal(ServiceResult.ServiceExpense);
            //    OrderManage.ReceivablesType = YaeherUserPaymentResult.PayMethod;//医生收款类型
            //    OrderManage.ReceivablesNumber = YaeherUserPaymentResult.PaymentAccout;//医生收款账号
            //    OrderManage.ServiceID = ServiceResult.Id;//产品ID
            //    OrderManage.ServiceName = ServiceResult.DoctorName + ServiceResult.ServiceType;//产品名称
            //    OrderManage.SellerMoneyID = doctorUserResult.result.item.WxPayBusinessId.ItemValue;//微信支付分配的商户号
            //    OrderManage.TradeType = ServiceResult.ServiceType;//交易类型
            //    OrderManage.CreatedBy = userid;
            //    OrderManage.CreatedOn = DateTime.Now;
            //    var result1 = await _orderManageService.CreateOrderManage(OrderManage);
            //    var record = new OrderTradeRecord()
            //    {
            //        SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
            //        OrderID = result1.Id,
            //        OrderNumber = result1.OrderNumber,
            //        PayType = "wxpay",
            //        OrderCurrency = "rmb",
            //        TenpayNumber = "",//支付账号
            //        VoucherNumber = "",//代金券编号
            //        VoucherJSON = "",//代金券Json
            //        PayMoney = Convert.ToDecimal(ServiceResult.ServiceExpense),
            //        PaymentState = "paid",
            //        PaymentSourceCode = "order",
            //        PaymentSource = "订单",
            //        WXPayBillno = WXPayBillno,
            //        WXTransactionId = WXTransactionId,
            //        WXOrderQuery = WXOrderQuery,
            //        CreatedBy = userid,
            //        CreatedOn = DateTime.Now
            //    };
            //    var result2 = await _orderTradeRecordService.CreateOrderTradeRecord(record);


            //    #region  发布咨询 消息接受者为医生  DoctorNotice AddConsultation
            //    // 发布咨询 
            //    Consultation consultation = new Consultation();
            //    consultation.yaeherConsultation = result;       // 咨询主表
            //    consultation.orderManage = result1;             // 订单管理表    
            //    consultation.orderTradeRecords = result2;       // 交易记录表
            //    Publishs Consultationpublishs = new Publishs()
            //    {
            //        TemplateCode = "DoctorNotice",
            //        OperationType = "AddConsultation",  // 新增咨询
            //        MessageRemark = result.IIInessDescription,  // 发起咨询
            //        Publisher = "Patient",
            //        PublishUrl = "Patient",
            //        EventName = "发布 新增咨询",
            //        EventCode = "Consultation",
            //        BusinessID = result.Id.ToString(),
            //        BusinessCode = result.ConsultNumber,
            //        BusinessJSON = JsonHelper.ToJson(consultation),
            //        PublishedTime = result.CreatedOn,
            //        PublishStatus = true,
            //        CreatedBy = userid,
            //        CreatedOn = DateTime.Now,
            //        Secret = secret,
            //    };
            //    var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
            //    var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
            //    if (ConsultationJson != null)
            //    {
            //        Consultationpublishs.PublishStatus = true;
            //    }
            //    else
            //    {
            //        Consultationpublishs.PublishStatus = false;
            //    }
            //    Consultationpublishs.ServerClient = "Client";
            //    try
            //    {
            //        var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    #endregion

            //    unitOfWork.Complete();
            //}
            //return new ObjectResultModule("", 200, "success");
        }

        /// <summary>
        /// 撤销咨询单处理中状态 --> 未支付 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/WXOAuthPayProcessingRelease")]
        [AbpAuthorize]
        public async Task<ObjectResultModule> WXOAuthPayProcessingRelease([FromBody]TencentPayModel model)
        {
            if (!Commons.CheckSecret(model.Secret))
            {
                return new ObjectResultModule("", 422, "Wrong Secret");
            }
            var secret = await CreateSecret();
            //获取产品信息
            var consul = await _consultationService.YaeherConsultationByNumber(model.ConsultNumber);
            if (consul == null) { return new ObjectResultModule("", 400, "查询咨询单失败,请联系管理员！"); }

            if (consul.ConsultState == "processing")
            {
                consul.ConsultState = "unpaid";
                var result = await _consultationService.UpdateYaeherConsultation(consul);
                if (result.ConsultState == "unpaid")
                {
                    return new ObjectResultModule("", 200, "success");
                }
            }
            return new ObjectResultModule("", 400, "咨询单状态更新失败");
        }

        /// <summary>
        /// 支付回调接口
        /// </summary>
        /// <returns></returns>
        [Route("api/PayNotify")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ActionResult> PayNotify()
        {
            try
            {
                ResponseHandler resHandler = new ResponseHandler(HttpContext);
                string return_code = resHandler.GetParameter("return_code");
                string return_msg = resHandler.GetParameter("return_msg");

                string xml = string.Format(@"<xml>
                                                <return_code><![CDATA[{0}]]></return_code>
                                                <return_msg><![CDATA[{1}]]></return_msg>
                                                </xml>", return_code, return_msg);

                string res = null;
                resHandler.SetKey("8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm");//微信支付密室
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
                {
                    res = "success";//正确的订单处理
                                    //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
                }
                else
                {
                    res = "wrong";//错误的订单处理
                }
                /* 这里可以进行订单处理的逻辑 */
                try
                {
                    if (res == "success")
                    {
                        string out_trade_no = resHandler.GetParameter("out_trade_no");
                        YaeherConsultationAdd yaeherConsultationAdd = new YaeherConsultationAdd();
                        yaeherConsultationAdd.ConsultNumber = "CN-" + out_trade_no.Substring(0, out_trade_no.Length - 6);
                        yaeherConsultationAdd.sp_billno = out_trade_no;
                        yaeherConsultationAdd.Secret = await CreateSecret();

                        HangfireScheduleJob job = new HangfireScheduleJob();
                        var JobModel = new JobModel();
                        JobModel.CallbackUrl = Commons.PatientIp + "api/PayNotifyJob";
                        JobModel.CallbackContent = JsonHelper.ToJson(yaeherConsultationAdd);
                        job.NotifyEnqueue(JobModel);

                    }
                    //发送支付成功的模板消息
                    #region 消息模板
                    // string appId = TenPayV3_AppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
                    // string openId = resHandler.GetParameter("openid");
                    // var templateData = new WeixinTemplate_PaySuccess("https://weixin.senparc.com", "购买商品", "状态：" + return_code);

                    //  Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息参数", appId + " , " + openId);

                    //  var result = AdvancedAPIs.TemplateApi.SendTemplateMessage(appId, openId, templateData);
                    #endregion
                }
                catch (Exception ex)
                {
                    YaeherOperList yaeherOperList = new YaeherOperList();
                    yaeherOperList.CreatedOn = DateTime.Now;
                    yaeherOperList.OperExplain = "Message:" + ex.Message.ToString();
                    yaeherOperList.OperContent = "StackTrace:" + ex.StackTrace.ToString();
                    yaeherOperList.OperType = "支付回调";
                    await _YaeherOperListrepository.PatientYaeherOperList(yaeherOperList);
                }

                return Content(xml, "text/xml");

            }
            catch (Exception ex)
            {
                YaeherOperList yaeherOperList = new YaeherOperList();
                yaeherOperList.CreatedOn = DateTime.Now;
                yaeherOperList.OperExplain = "Message:" + ex.Message.ToString();
                yaeherOperList.OperContent = "StackTrace:" + ex.StackTrace.ToString();
                yaeherOperList.OperType = "支付回调";
                await _YaeherOperListrepository.PatientYaeherOperList(yaeherOperList);
                throw;
            }


        }
        /// <summary>
        /// 支付回调接口
        /// </summary>
        /// <returns></returns>
        [Route("api/PayNotifyJob")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ActionResult> PayNotifyJob([FromBody]YaeherConsultationAdd yaeherConsultationAdd)
        {
            if (!Commons.CheckSecret(yaeherConsultationAdd.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                Logger.Info("secret 错误!");
                return Content("", "text/xml");
            }
            string xml = "";
            TencentWXPay OrdertencentWXPay = new TencentWXPay();
            TencentWXPay RefundtencentWXPay = new TencentWXPay();

            string message = "";
            string WXPayBillno = "";
            string WXTransactionId = "";
            string WXOrderQuery = "";
            WXPayBillno = yaeherConsultationAdd.sp_billno;
            var secret = await CreateSecret();
            //患者端咨询单 
            var consul = await _consultationService.YaeherConsultationByNumber(yaeherConsultationAdd.ConsultNumber);
            
            //if (consul.ConsultState != "unpaid"&&consul.ConsultState != "created") { return Content(xml, "text/xml"); }
            //新增处理中条件判断
             if (consul.ConsultState != "unpaid" && consul.ConsultState !="processing") { return Content(xml, "text/xml"); }
            //配置文件查询
            var Content1 = "{\"SystemType\":\"TencentWechar\",\"secret\":\"" + secret + "\"}";
            var tencentmparam = await this.PostResponseAsync(Commons.AdminIp + "api/SystemConfigsList/", Content1);
            var tencentmparamlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemConfigs>>>>(tencentmparam);
            if (tencentmparamlist == null || tencentmparamlist.result.item == null) { return Content("", "text/xml"); }
            var tencentparam = tencentmparamlist.result.item.FirstOrDefault();
            //医生端医生信息查询
            Content1 = "{\"Id\":" + consul.DoctorID + ",\"secret\":\"" + secret + "\"}";
            var doctoruser = await this.PostResponseAsync(Commons.AdminIp + "api/DoctorInformation", Content1);
            var doctorUserResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherDoctorInfo>>>(doctoruser);
            if (doctorUserResult == null || doctorUserResult.result.item == null) { return Content("", "text/xml"); }
            var ServiceResult = doctorUserResult.result.item.ServiceMoneyLists.Find(t => t.Id == consul.ServiceMoneyListId);
            var queryresult = await OrdertencentWXPay.OrderQueryAsync(WXPayBillno, tencentparam);
            if (ServiceResult == null || ServiceResult.Id < 1 || ServiceResult.ServiceState == false)
            {
                message = "医生该服务已下线,资金会原路返回您的账户，请稍后！";
            }
            if (queryresult.trade_state != "SUCCESS")
            {
                return Content(queryresult.trade_state, "text/xml");
            }
            #region 重复支付处理
            //if (consul.ConsultState == "created")
            //{
            //    var ordertradeold = await _orderTradeRecordService.OrderTradeRecordByConsultNumber(consul.ConsultNumber);
            //    if (ordertradeold != null && ordertradeold.WXPayBillno != WXPayBillno)
            //    {
            //        message = "您的支付已经收到，请不要重复支付，程序正在处理，请稍后！";
            //    }
            //}
            #endregion
            var YaeherUserPaymentResult = doctorUserResult.result.item.YaeherUserPayment.Find(t => t.IsDefault);
            if (YaeherUserPaymentResult == null) { YaeherUserPaymentResult = doctorUserResult.result.item.YaeherUserPayment.Find(t => t.PayMethod == "wxpay"); }

            if (string.IsNullOrEmpty(message))
            {
                //患者端满单查询
                OrderTradeRecordIn refundManageIn = new OrderTradeRecordIn();
                var StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                refundManageIn.DoctorId = consul.DoctorID;
                refundManageIn.ServiceID = consul.ServiceMoneyListId;
                refundManageIn.AndAlso(t => t.IsDelete == false);
                refundManageIn.AndAlso(t => t.PayMoney > 0);
                refundManageIn.AndAlso(t => t.CreatedOn >= StartTime);
                refundManageIn.AndAlso(t => t.CreatedOn < StartTime.AddDays(1));

                var ordertradelist = await _orderTradeRecordService.PatientOrderTradeRecordList(refundManageIn);

                var allconsultation = ordertradelist.Count();
                if (ServiceResult.ServiceFrequency <= allconsultation)
                {
                    message = "医生该服务已满单,资金会原路返回您的账户，请稍后！";
                }
               
                if (!string.IsNullOrEmpty(yaeherConsultationAdd.sp_billno))
                {
                    if (queryresult.result_code != "SUCCESS")
                    {
                        return Content("查询支付信息失败,请重新提交！", "text/xml");
                    }

                    if (int.Parse(queryresult.total_fee) != (int)(ServiceResult.ServiceExpense * 100))
                    {
                        message = "医生该服务已修改价格，资金会原路返回您的账户，请稍后！";

                    }
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                string outTradeNo = WXPayBillno;
                string outRefundNo = consul.ConsultNumber + "-" + DateTime.Now.ToString("yyyMMddhhmmss");
                var totalFee = int.Parse(queryresult.total_fee);//单位：分
                int refundFee = totalFee;
                var refundpayresult = await RefundtencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam, message);

                consul.ConsultState = "unpaid";
                await _consultationService.UpdateYaeherConsultation(consul);

                if (refundpayresult.code != "SUCCESS")
                {
                    Logger.Info("outTradeNo:" + outTradeNo + "outRefundNo:" + outRefundNo + "totalFee:" + totalFee + "refundFee:" + refundFee + "message:" + message + "+refundpayresult:" + JsonHelper.ToJson(refundpayresult));

                    HangfireScheduleJob job = new HangfireScheduleJob();
                    RefundModel refundModel = new RefundModel();
                    refundModel.Secret = await CreateSecret();
                    refundModel.outTradeNo = outTradeNo;
                    refundModel.outRefundNo = outRefundNo;
                    refundModel.totalFee = totalFee;
                    refundModel.refundFee = refundFee;
                    refundModel.msg = message;
                    JobModel model = new JobModel();
                    model.CallbackUrl = Commons.AdminIp + "api/RefundAsync";
                    model.CallbackContent = JsonHelper.ToJson(refundModel);

                    job.Enqueue(model);

                    return Content("退款支付失败,请联系管理员！", "text/xml");
                    //return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
                }
                return Content(message, "text/xml");
            }

            WXTransactionId = queryresult.transaction_id;
            WXOrderQuery = JsonHelper.ToJson(queryresult);

            Content1 = "{\"SystemCode\":\"SystemOverTime\",\"secret\":\"" + secret + "\"}";
            var replymparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", Content1);
            var replylistparam = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(replymparam);
            if (replylistparam == null || replylistparam.result.item == null) { return Content("", "text/xml"); }

            Consultation consultation = new Consultation();
            Publishs Consultationpublishs = new Publishs();
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                consul.ConsultState = "created";
                consul.Overtime = DateTime.Now.AddDays(double.Parse(replylistparam.result.item[0].ItemValue));
                var result = await _consultationService.UpdateYaeherConsultation(consul);
                var OrderManage = new OrderManage();
                OrderManage.SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6);
                OrderManage.OrderNumber = "ON-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4);
                OrderManage.ConsultNumber = result.ConsultNumber;
                OrderManage.ConsultID = result.Id;
                OrderManage.ConsultType = ServiceResult.ServiceType;
                OrderManage.ConsultantID = consul.CreatedBy;
                OrderManage.ConsultantName = result.ConsultantName;
                OrderManage.PatientID = result.PatientID;
                OrderManage.PatientName = result.PatientName;
                OrderManage.DoctorID = consul.DoctorID;
                OrderManage.DoctorName = doctorUserResult.result.item.DoctorInfo.DoctorName;
                OrderManage.OrderCurrency = "rmb";
                OrderManage.OrderMoney = Convert.ToDecimal(ServiceResult.ServiceExpense);
                OrderManage.ReceivablesType = YaeherUserPaymentResult.PayMethod;//医生收款类型
                OrderManage.ReceivablesNumber = YaeherUserPaymentResult.PaymentAccout;//医生收款账号
                OrderManage.ServiceID = ServiceResult.Id;//产品ID
                OrderManage.ServiceName = ServiceResult.DoctorName + ServiceResult.ServiceType;//产品名称
                OrderManage.SellerMoneyID = doctorUserResult.result.item.WxPayBusinessId.ItemValue;//微信支付分配的商户号
                OrderManage.TradeType = ServiceResult.ServiceType;//交易类型
                OrderManage.CreatedBy = consul.CreatedBy;
                OrderManage.CreatedOn = DateTime.Now;
                var result1 = await _orderManageService.CreateOrderManage(OrderManage);
                var record = new OrderTradeRecord()
                {
                    SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                    OrderID = result1.Id,
                    OrderNumber = result1.OrderNumber,
                    PayType = "wxpay",
                    OrderCurrency = "rmb",
                    TenpayNumber = "",//支付账号
                    VoucherNumber = "",//代金券编号
                    VoucherJSON = "",//代金券Json
                    PayMoney = Convert.ToDecimal(ServiceResult.ServiceExpense),
                    PaymentState = "paid",
                    PaymentSourceCode = "order",
                    PaymentSource = "订单",
                    WXPayBillno = WXPayBillno,
                    WXTransactionId = WXTransactionId,
                    WXOrderQuery = WXOrderQuery,
                    CreatedBy = consul.CreatedBy,
                    CreatedOn = DateTime.Now
                };
                var result2 = await _orderTradeRecordService.CreateOrderTradeRecord(record);
                #region  发布咨询 消息接受者为医生  DoctorNotice AddConsultation
                // 发布咨询 
                consultation.yaeherConsultation = result;       // 咨询主表
                consultation.orderManage = result1;             // 订单管理表    
                consultation.orderTradeRecords = result2;       // 交易记录表
                Consultationpublishs.TemplateCode = "DoctorNotice";
                Consultationpublishs.OperationType = "AddConsultation";  // 新增咨询
                Consultationpublishs.MessageRemark = result.IIInessDescription;  // 发起咨询
                Consultationpublishs.Publisher = "Patient";
                Consultationpublishs.PublishUrl = "Patient";
                Consultationpublishs.EventName = "发布 新增咨询";
                Consultationpublishs.EventCode = "Consultation";
                Consultationpublishs.BusinessID = result.Id.ToString();
                Consultationpublishs.BusinessCode = result.ConsultNumber;
                Consultationpublishs.BusinessJSON = JsonHelper.ToJson(consultation);
                Consultationpublishs.PublishedTime = result.CreatedOn;
                Consultationpublishs.PublishStatus = true;
                Consultationpublishs.CreatedBy = consul.CreatedBy;
                Consultationpublishs.CreatedOn = DateTime.Now;
                Consultationpublishs.Secret = secret;

                Consultationpublishs.ServerClient = "Client";
                try
                {
                    var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                }
                catch (Exception ex)
                {
                    YaeherOperList yaeherOperList = new YaeherOperList();
                    yaeherOperList.CreatedOn = DateTime.Now;
                    yaeherOperList.OperExplain = "Message:" + ex.Message.ToString();
                    yaeherOperList.OperContent = "StackTrace:" + ex.StackTrace.ToString();
                    yaeherOperList.OperType = "支付回调";
                    await _YaeherOperListrepository.PatientYaeherOperList(yaeherOperList);

                    throw ex;
                }
                #endregion
                unitOfWork.Complete();
            }
            var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
           // var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);

            return Content("success", "text/xml");
        }

    }
}
