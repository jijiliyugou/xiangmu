using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.TenPay.V3;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.TencentCustom;
using Yaeher.Controllers;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using System.Xml;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar;
using Senparc.Weixin.MP.Entities;
using Yaeher.YaeherDoctors.Dto;
using Yaeher.Consultation;
using Yaeher.DoctorReport;
using Yaeher.DoctorReport.Dto;
using Yaeher.CompaniesReport;
using Yaeher.CompaniesReport.Dto;
using Yaeher.HangFire;
using Yaeher.EventBus;
using Yaeher.Common.HangfireJob;
using Yaeher.EventBus.Dto;
using Yaeher.Extensions;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class TencentPayController : YaeherControllerBase
    {
        private readonly IAbpSession _IabpSession;
        private readonly IServiceMoneyListService _serviceMoneyListService;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherUserService _yaeherUserService;
        private readonly ISystemConfigsService _systemConfigsService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IIncomeDevideService _incomeDevideService;
        private readonly IConsultationService _yaeherConsultationService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IOrderManageService _orderManageService;
        private readonly IDoctorIncomeService _doctorIncomeService;
        private readonly ICorporateIncomeTotalService _corporateIncomeTotalService;
        private readonly IIncomeDetailsService _incomeDetailsService;
        private readonly IHangFireJobService _hangFireJobService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IDoctorOnlineRecordService _doctorOnlineRecordService;
        /// <summary>
        /// Home 构造函数
        /// </summary>
        ///// <param name="abpSession"></param>
        ///// <param name="serviceMoneyListService"></param>
        ///// <param name="userManagerService"></param>
        public TencentPayController(
             IAbpSession abpSession
            , IServiceMoneyListService serviceMoneyListService
            , IUserManagerService userManagerService
            , IYaeherUserService yaeherUserService
            , ISystemConfigsService systemConfigsService
            , IYaeherDoctorService yaeherDoctorService
            , IIncomeDevideService incomeDevideService
            , IConsultationService yaeherConsultationService
            , IOrderManageService orderManageService
            , IOrderTradeRecordService orderTradeRecordService
            , IDoctorIncomeService doctorIncomeService
            , ICorporateIncomeTotalService corporateIncomeTotalService
            , IIncomeDetailsService incomeDetailsService
            , IHangFireJobService hangFireJobService
            , ISystemParameterService systemParameterService
            , IDoctorOnlineRecordService doctorOnlineRecordService
            )
        {
            _yaeherUserService = yaeherUserService;
            _IabpSession = abpSession;
            _serviceMoneyListService = serviceMoneyListService;
            _userManagerService = userManagerService;
            _systemConfigsService = systemConfigsService;
            _yaeherDoctorService = yaeherDoctorService;
            _incomeDevideService = incomeDevideService;
            _yaeherConsultationService = yaeherConsultationService;
            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _doctorIncomeService = doctorIncomeService;
            _corporateIncomeTotalService = corporateIncomeTotalService;
            _incomeDetailsService = incomeDetailsService;
            _hangFireJobService = hangFireJobService;
            _systemParameterService = systemParameterService;
            _doctorOnlineRecordService = doctorOnlineRecordService;
        }

        //退款申请请可参考Senparc.Weixin.MP.Sample中的退款demo
        /// <summary>
        /// 【异步方法】退款申请接口
        /// </summary>
        /// <param name="PackageRequestHandler"></param>
        /// <param name="url"></param>
        /// <param name="cert">证书绝对路径，如@"F:\apiclient_cert.p12"</param>
        /// <param name="certPassword">证书密码</param>
        /// <returns></returns>
        private static async Task<RefundResult> ShareAsync(RequestHandler PackageRequestHandler, string url, string cert, string certPassword)
        {
            var data = PackageRequestHandler.ParseXML();

            //var urlFormat = "https://api.mch.weixin.qq.com/secapi/pay/refund";

            //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
            //string cert = cert;// @"F:\apiclient_cert.p12";
            //私钥（在安装证书时设置）
            string responseContent = await CertPostAsync(cert, certPassword, data, url);

            var resxml = XDocument.Parse(responseContent);


            return new RefundResult(responseContent);
        }

        /// <summary>
        /// 【异步方法】带证书提交
        /// </summary>
        /// <param name="cert">证书绝对路径</param>
        /// <param name="certPassword">证书密码</param>
        /// <param name="data">数据</param>
        /// <param name="url">Url</param>
        /// <returns></returns>
        private static async Task<string> CertPostAsync(string cert, string certPassword, string data, string url, int timeOut = Config.TIME_OUT)
        {
            string password = certPassword;
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using (MemoryStream ms = new MemoryStream(dataBytes))
            {
                //调用证书
                X509Certificate2 cer = new X509Certificate2(cert, certPassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

                string responseContent = await RequestUtility.HttpPostAsync(
                    url,
                    postStream: ms,
                    cer: cer,
                    timeOut: timeOut);

                return responseContent;
            }
        }

        /// <summary>
        /// 微信pay信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/WXOAuthPay")]
        [AbpAuthorize]
        public async Task<ObjectResultModule> WXOAuthPay([FromBody]TencentPayModel model)
        {
            //Logger.Info("WXOAuthPay"+JsonHelper.ToJson(model));
            if (!Commons.CheckSecret(model.Secret))
            {
                return new ObjectResultModule("", 422, "Wrong Secret");
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var User = await _yaeherUserService.YaeherUserByID(userid);
            if (usermanager.MobileRoleName != "patient") { return new ObjectResultModule("", 400, "患者用户才能够提单！"); }
            SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
            systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
            var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
            var tencentparam = configs.FirstOrDefault();
            var secret = await CreateSecret();
            //获取产品信息
            var Content = "{\"ConsultNumber\":\"" + model.ConsultNumber + "\",\"secret\":\"" + secret + "\"}";
            var consulresult = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationById", Content);
            var consul = JsonHelper.FromJson<APIResult<ResultModule<YaeherConsultation>>>(consulresult);
            if (consul == null || consul.result.item == null) { return new ObjectResultModule("", 400, "查询订单失败,请联系管理员！"); }
            //新增处理中判断
            if (consul.result.item.ConsultState == "processing")
            { return new ObjectResultModule("", 400, "订单处理中,请稍后！"); }
            if (consul.result.item.ConsultState == "created")
            { return new ObjectResultModule("", 400, "订单已支付,请勿重复支付！");  }
            var product = await _serviceMoneyListService.ServiceMoneyListByID(consul.result.item.ServiceMoneyListId);
            if (product == null || product.ServiceState == false)
            {
                return new ObjectResultModule("", 400, "医生该服务已下线,不允许提交咨询！");
            }
            DoctorOnlineRecordIn doctorOnlineRecordIn = new DoctorOnlineRecordIn();
            doctorOnlineRecordIn.AndAlso(t => t.IsDelete == false && t.DoctorID == product.DoctorID);
            var onlinelist = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctorOnlineRecordIn);
            var online = onlinelist.FirstOrDefault();
            //Logger.Info("online"+JsonHelper.ToJson(online));
            if (online == null || online.OnlineState != "online")
            {
                return new ObjectResultModule("", 400, "医生已下线不允许提交咨询！");
            }
            var doctor = await _yaeherDoctorService.YaeherDoctorByID(product.DoctorID);

            var StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var EndTime = StartTime.AddDays(1);
            var consin = new ConsultationIn(); consin.AndAlso(a => a.IsDelete == false && a.DoctorID == product.DoctorID);
            consin.AndAlso(a => (a.CreatedOn >= StartTime));
            consin.AndAlso(a => (a.CreatedOn < EndTime));
            consin.AndAlso(a => a.ServiceMoneyListId == product.Id);
            consin.AndAlso(a => (a.RefundNumber == null || a.RefundNumber == ""));
            var allconsultation = await _yaeherConsultationService.YaeherConsultationList(consin);
            var allconsultationcount = allconsultation.Count();

            if (product.ServiceFrequency <= allconsultationcount)
            {
                return new ObjectResultModule("", 400, "医生该服务已满单,请明天再来！");
            }
            if (product == null)
            {
                return new ObjectResultModule("", 400, "商品不存在");
            }
            if (product.ServiceExpense < 0) { return new ObjectResultModule("", 422, "商品价格出错！"); }
            var openId = usermanager.WecharOpenID;
            if (string.IsNullOrEmpty(openId))
            {
                return new ObjectResultModule("", 422, "Wrong openid");
            }
           
            TencentWXPay tencentWXPay = new TencentWXPay();
            var spbillCreateIp = HttpContext.UserHostAddress()?.ToString();
            var payresult = await tencentWXPay.UnifiedorderAsync(doctor.IsSharing, spbillCreateIp, consul.result.item, User, product, tencentparam);
            //Logger.Info("PAYResult" + JsonHelper.ToJson(payresult));
            if (payresult.code == "SUCCESS")
            {
                //处理订单状态为处理中
                consul.result.item.ConsultState = "processing";
                consul.result.item.Secret = secret;
                await this.PostResponseAsync(Commons.PatientIp + "api/UpdateConsultationStatus/",  JsonHelper.ToJson(consul.result.item));
                return new ObjectResultModule(payresult, 200, "success");
            }
            else
            {
                return new ObjectResultModule("", 400, "获取支付信息失败,请联系管理员！");
            }
        }

       
        /// <summary>
        /// 结算统计
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeTotal([FromBody]HangFireJob hangFireJobInfo)
        {
            //Logger.Info("IncomeTotal:"+JsonHelper.ToJson(hangFireJobInfo));
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateHangFireJob = new HangFireJob();
            int hangfireid = 0;
            hangfireid = hangFireJobInfo.Id;
            string jobid = hangFireJobInfo.JobRunID;
            string JobSates = hangFireJobInfo.JobSates;
            if (!string.IsNullOrEmpty(jobid))
            {
                // UpdateHangFireJob = _hangFireJobService.HangFireJobSingleByID(hangfireid);
                UpdateHangFireJob = _hangFireJobService.HangFireJobSingleByJobID(jobid);
            }
            else if (hangfireid > 0)
            {
                UpdateHangFireJob = _hangFireJobService.HangFireJobSingleByID(hangfireid);
            }
            else
            {
                UpdateHangFireJob = _hangFireJobService.HangFireJobSingleFirstOrDefault();
            }
            if (UpdateHangFireJob == null)
            { return new ObjectResultModule("", 400, "error"); }
            #region cs
            if (JobSates == "Open")//新增状态过来则open状态转换成完成
            {
                UpdateHangFireJob.JobSates = "Complete"; // 执行状态
            }
            else
            {
                UpdateHangFireJob.JobSates = JobSates; // 执行状态
            }
            UpdateHangFireJob.ModifyOn = DateTime.Now;
            UpdateHangFireJob.ModifyBy = userid;

            var relruntime = UpdateHangFireJob.JobRunTime;

            var param = new SystemParameterIn() { SystemType = "ProfitSharingTime" };
            var SharingTimeList = await _systemParameterService.ParameterList(param);
            var SharingTime = SharingTimeList.FirstOrDefault();

            //DateTime runtime = relruntime.AddHours(-double.Parse(SharingTime.ItemValue));
            //DateTime runtime = Convert.ToDateTime("2018-12-19 20:06:14.252810");//开始时间
            //relruntime = Convert.ToDateTime("2018-12-19 23:59:59");//结束时间

            //分账配置信息
            SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
            systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
            var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
            var tencentparam = configs.FirstOrDefault();
            //#region 测试数据
            //TencentWXPay tencentWXPay1 = new TencentWXPay();
            //List<receivershare> receivers1 = new List<receivershare>();
            //var receivershare1 = new receivershare();
            //receivershare1.name = "黄彪";
            //receivershare1.type = "PERSONAL_OPENID";
            //receivershare1.account = "oq6TvvmMkMjbX3xuV7TVLpSNA4qc";
            //receivershare1.amount = Convert.ToInt32(80);
            //receivers1.Add(receivershare1);

            ////  var query = await tencentWXPay1.OrderQueryAsync("201901021030541520941790",tencentparam);
            ////  var shareresult1 = await tencentWXPay1.ProfitSharing(receivers1, tencentparam, "4200000255201901027259198855", "201901021030541520941790");

            ////Logger.Info("receivers1:" + JsonHelper.ToJson(receivers1));
            ////Logger.Info("tencentparam1:" + JsonHelper.ToJson(tencentparam));
            ////Logger.Info("4200000234201812188859300979:");
            ////Logger.Info("20181218210136133155139:");
            ////var shareresult1 = await tencentWXPay1.ProfitSharing(receivers1, tencentparam, "4200000234201812188859300979", "20181218210136133155139");
            ////var sharre = shareresult1.result_code;
            //#endregion
            #region 分账

            //日起止时间
            var sysdaytotal = 0.00;//公司收入
            var sysdayordertotal = 0.00;//公司订单总金额

            var Ipaddress = "";
            var enddate = Convert.ToDateTime(relruntime.ToString("yyyy-MM-dd")).AddDays(1);
            var startdate = Convert.ToDateTime(relruntime.ToString("yyyy-MM-dd"));

            //  var enddate = DateTime.Parse("2019-01-24 23:59:59.000001");
            //  var startdate = DateTime.Parse("2019-01-24 00:00:00.000001");

            var totaldate = startdate;
            // if (enddate.Day != startdate.Day)
            //{
            //    enddate = Convert.ToDateTime(startdate.AddDays(1).ToString("yyyy-MM-dd"));
            //   totaldate = enddate.AddSeconds(-1);
            // }

            // 检查今日是否执行过job 日起止时间
            var docincomestartdate = Convert.ToDateTime(startdate.ToString("yyyy-MM-dd"));
            var docincomeenddate = Convert.ToDateTime(startdate.AddDays(1).ToString("yyyy-MM-dd"));
            // var docincomestartdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //  var docincomeenddate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")).AddSeconds(-1);

            //月起止时间
            DateTime MonthStartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")).AddDays(1 - DateTime.Now.Day);
            DateTime MonthEndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 11:59:59")).AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1);
            //年起止时间
            DateTime YearStartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01 00:00:00"));
            DateTime YearEndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-12-31 11:59:59"));

            //医生 表
            YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn(); yaeherDoctorIn.AndAlso(t => t.IsSharing == true && t.IsDelete == false && t.CheckRes == "success");
            var doctor = new List<YaeherDoctor>();
            doctor = await _yaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);
            //用户 表
            YaeherUserIn yaeherUserIn = new YaeherUserIn(); yaeherUserIn.AndAlso(t => t.IsDelete == false && t.RoleName == "doctor");
            var userlist = await _yaeherUserService.YaeherUserList(yaeherUserIn);
            ////订单收入分配 表
            //IncomeDevideIn incomeDevideIn = new IncomeDevideIn(); incomeDevideIn.AndAlso(t => t.IsDelete == false&&t.DevideTime >= startdate && t.DevideTime < enddate);
            //var devideTotal = await _incomeDevideService.IncomeDevideList(incomeDevideIn);
            //var devide = devideTotal.Where(t => t.WXSharing == "Open").ToList();
            ////咨询 表
            //var consultationIn = new ConsultationIn(); consultationIn.AndAlso(t => t.IsDelete == false && t.ConsultState == "success");
            //var ConsultationList = await _yaeherConsultationService.YaeherConsultationList(consultationIn);
            ////订单 表
            //var orderManageIn = new OrderManageIn(); orderManageIn.AndAlso(t => t.IsDelete == false);
            //var OrderList = await _orderManageService.OrderManageList(orderManageIn);
            ////订单明细 表
            //OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn(); orderTradeRecordIn.AndAlso(t => t.IsDelete == false && t.PayType == "wxpay" && t.PaymentState == "paid" && t.PaymentSourceCode == "order");
            //var OrderTradeList = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);

            IncomeDevideIn incomeDevideIn = new IncomeDevideIn(); incomeDevideIn.AndAlso(t => t.IsDelete == false && t.DevideTime >= startdate && t.DevideTime < enddate);

            var devideTotalModelList = await _incomeDevideService.IncomeTotalModelList(incomeDevideIn);

            OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
            orderTradeRecordIn.AndAlso(t => t.IsDelete == false && t.PayMoney < 0 && t.CreatedOn >= docincomestartdate && t.CreatedOn < docincomeenddate);
            var returnordertrade = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);

            var devideTotal = devideTotalModelList.incomeDevides.ToList();
            var devide = devideTotalModelList.incomeDevides.Where(t => t.WXSharing == "Open").ToList();
            var ConsultationList = devideTotalModelList.yaeherConsultations.ToList();
            var OrderList = devideTotalModelList.orderManages.ToList();
            var OrderTradeList = devideTotalModelList.orderTradeRecords.ToList();

            //医生收入 表
            DoctorIncomeIn doctorIncomeIn = new DoctorIncomeIn(); doctorIncomeIn.AndAlso(t => t.IsDelete == false);
            var doctorIncome = await _doctorIncomeService.DoctorIncomeList(doctorIncomeIn);
            //收入 表
            CorporateIncomeTotalIn corporateIncomeTotalIn = new CorporateIncomeTotalIn(); corporateIncomeTotalIn.AndAlso(t => t.IsDelete == false);
            var sysIncome = await _corporateIncomeTotalService.CorporateIncomeTotalList(corporateIncomeTotalIn);

            ////医生收入
            var evedoctor = new YaeherDoctor();
            for (int i = 0; i < doctor.Count(); i++)
            {
                evedoctor = doctor[i];
                var doctoruserid = evedoctor.UserID; var doctorid = evedoctor.Id;
                var user = userlist.Where(t => t.Id == doctoruserid).FirstOrDefault();
                if (user == null) { continue; }
                var daydocincomelist = doctorIncome.Where(t => t.IsDelete == false && t.DoctorID == doctorid && t.IncomeTimeType == "day" && t.TotalDate >= docincomestartdate && t.TotalDate < docincomeenddate).ToList();
                var doctordaytotal = 0.00;
                var devidelist = devide.Where(t => t.DoctorID == doctorid).ToList();
                if (devidelist != null && devidelist.Count > 0)
                {
                    var devid = new IncomeDevide();
                    for (int j = 0; j < devidelist.Count(); j++)
                    {
                        TencentWXPay tencentWXPay = new TencentWXPay();
                        List<receivershare> receivers = new List<receivershare>();
                        var receivershare = new receivershare();

                        devid = devidelist[j];
                        var ConsultNumber = devid.ConsultNumber;
                        var yaeherConsultation = ConsultationList.Where(t => t.ConsultNumber == ConsultNumber).FirstOrDefault();
                        if (yaeherConsultation == null) { continue; }//必须为完成状态,没找到不分账
                        var order = OrderList.Where(t => t.ConsultNumber == ConsultNumber).FirstOrDefault();
                        var OrderNumber = order.OrderNumber;
                        var ordertrade = OrderTradeList.Where(t => t.OrderNumber == OrderNumber).FirstOrDefault();
                        if (ordertrade != null && ordertrade.Id > 0)
                        {
                            //分账开启
                            var Sahring = "false";
                            SharingResult shareresult = new SharingResult();
                            if (Ipaddress != "http://192.168.2.3:5002/" && evedoctor.IsSharing)
                            {
                                var WXTransactionId = ordertrade.WXTransactionId;
                                var WXPayBillno = ordertrade.WXPayBillno;
                                receivershare.name = evedoctor.DoctorName;
                                receivershare.type = "PERSONAL_OPENID";
                                receivershare.account = user.WecharOpenID;
                                receivershare.amount = Convert.ToInt32(devid.DevideMoney * 100);
                                receivers.Add(receivershare);
                                //  var query = await tencentWXPay.OrderQueryAsync(ordertrade.WXPayBillno,tencentparam);
                                System.Threading.Thread.Sleep(10);
                                shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, WXTransactionId, WXPayBillno);
                                Sahring = shareresult.result_code;
                            }
                            else
                            { Sahring = "SUCCESS"; }
                            if (Sahring == "SUCCESS")
                            {
                                devid.WXSharing = "Complete";
                                //医生收入
                                IncomeDetails incomeDetails = new IncomeDetails();
                                incomeDetails.DoctorName = yaeherConsultation.DoctorName;
                                incomeDetails.DoctorID = yaeherConsultation.DoctorID;//有doctorid为医生收入
                                incomeDetails.ConsultID = yaeherConsultation.Id;
                                incomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                //  incomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                incomeDetails.OrderNumber = order.OrderNumber;
                                incomeDetails.OrderCurrency = order.OrderCurrency;
                                incomeDetails.OrderMoney = Math.Round(Convert.ToDouble(order.OrderMoney), 2);
                                incomeDetails.ProportionMoney = Math.Round(Convert.ToDouble(devid.DevideMoney), 2);//医生收款
                                incomeDetails.CreatedOn = DateTime.Now;
                                incomeDetails.TotalDate = totaldate;
                                await _incomeDetailsService.CreateIncomeDetails(incomeDetails);
                                doctordaytotal += incomeDetails.ProportionMoney;

                                sysdayordertotal += incomeDetails.OrderMoney;

                                //系统收入(未过滤微信手续费,购物券等)没有doctorid为系统收入
                                IncomeDetails sysincomeDetails = new IncomeDetails();
                                sysincomeDetails.DoctorName = evedoctor.DoctorName;
                                sysincomeDetails.ConsultID = yaeherConsultation.Id;
                                sysincomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                //  sysincomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                sysincomeDetails.OrderNumber = order.OrderNumber;
                                sysincomeDetails.OrderCurrency = order.OrderCurrency;
                                sysincomeDetails.OrderMoney = Math.Round(Convert.ToDouble(order.OrderMoney), 2);
                                var propo = order.OrderMoney - devid.DevideMoney;
                                var propdouble = Math.Round(Convert.ToDouble(propo.ToString()), 2);
                                sysincomeDetails.ProportionMoney = propdouble;//公司收入
                                sysdaytotal += sysincomeDetails.ProportionMoney;
                                sysincomeDetails.CreatedOn = DateTime.Now;
                                sysincomeDetails.TotalDate = totaldate;
                                await _incomeDetailsService.CreateIncomeDetails(sysincomeDetails);
                            }
                            devid.WXSharingJson = JsonHelper.ToJson(shareresult);
                            await _incomeDevideService.UpdateIncomeDevide(devid);
                        }
                    }
                }
                if (daydocincomelist == null || daydocincomelist.Count < 1)//新增
                {
                    //新增医生统计日总表
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = evedoctor.Id;
                    DoctorIncome.DoctorName = evedoctor.DoctorName;
                    DoctorIncome.IncomeTimeType = "day";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _doctorIncomeService.CreateDoctorIncome(DoctorIncome);
                }
                else //修改
                {
                    var daydocincome = daydocincomelist.FirstOrDefault();

                    //新增医生统计日总表
                    daydocincome.Total += doctordaytotal;//金额
                    daydocincome.ModifyOn = DateTime.Now;
                    daydocincome.TotalDate = totaldate;

                    await _doctorIncomeService.UpdateDoctorIncome(daydocincome);

                }

                //月 医生收入
                var monthdocincomelist = doctorIncome.Where(t => t.DoctorID == doctorid && t.IncomeTimeType == "month" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();
                if (monthdocincomelist.Count > 0)//修改
                {
                    var monthincome = monthdocincomelist.FirstOrDefault();
                    var monthdaydocincomelist = doctorIncome.Where(t => t.DoctorID == doctorid && t.IncomeTimeType == "day" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();

                    var monthtotalIenum = from a in monthdaydocincomelist
                                          group a by a.DoctorID into g
                                          select new
                                          {
                                              g.Key,
                                              total = g.Sum(a => a.Total)
                                          };
                    var month = monthtotalIenum.FirstOrDefault();
                    monthincome.Total = month == null ? doctordaytotal : month.total;
                    monthincome.TotalDate = totaldate;
                    monthincome.ModifyOn = DateTime.Now;
                    await _doctorIncomeService.UpdateDoctorIncome(monthincome);
                }
                else//新增
                {
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = doctorid;
                    DoctorIncome.DoctorName = evedoctor.DoctorName;
                    DoctorIncome.IncomeTimeType = "month";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _doctorIncomeService.CreateDoctorIncome(DoctorIncome);
                }
                //年 医生收入
                var yeardocincomelist = doctorIncome.Where(t => t.IsDelete == false && t.DoctorID == doctorid && t.IncomeTimeType == "year" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();
                if (yeardocincomelist.Count > 0)//修改
                {
                    var yearincome = yeardocincomelist.FirstOrDefault();
                    var monthcountdocincomelist = doctorIncome.Where(t => t.DoctorID == doctorid && t.IncomeTimeType == "month" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();

                    var monthtotalIenum = from a in monthcountdocincomelist
                                          group a by a.DoctorID into g
                                          select new
                                          {
                                              g.Key,
                                              total = g.Sum(a => a.Total)
                                          };
                    var month = monthtotalIenum.FirstOrDefault();
                    yearincome.Total = month == null ? doctordaytotal : month.total;
                    yearincome.TotalDate = totaldate;
                    yearincome.ModifyOn = DateTime.Now;
                    await _doctorIncomeService.UpdateDoctorIncome(yearincome);
                }
                else//新增
                {
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = doctorid;
                    DoctorIncome.DoctorName = evedoctor.DoctorName;
                    DoctorIncome.IncomeTimeType = "year";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _doctorIncomeService.CreateDoctorIncome(DoctorIncome);
                }

            }
            #region 公司收入
            // 日 公司收入

            var sysdaydevide = devideTotal.ToList();

            var daysysIncomeList = sysIncome.Where(t => t.IncomeType == "day" && t.TotalDate >= docincomestartdate && t.TotalDate < docincomeenddate).ToList();

            if (daysysIncomeList == null || daysysIncomeList.Count < 1)
            {
                ////新增公司统计总表
                var CorporateIncomeTotal = new CorporateIncomeTotal();
                CorporateIncomeTotal.IncomeType = "day";
                CorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                CorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                CorporateIncomeTotal.RefundTotalMoney = returnordertrade.Sum(t => t.PayMoney);
                CorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                CorporateIncomeTotal.CreatedOn = DateTime.Now;
                CorporateIncomeTotal.TotalDate = totaldate;
                await _corporateIncomeTotalService.CreateCorporateIncomeTotal(CorporateIncomeTotal);
            }
            else
            {
                var daysysincome = daysysIncomeList.FirstOrDefault();
                daysysincome.IncomeTotal += Convert.ToDecimal(sysdaytotal);
                daysysincome.OrderTotalMoney += Convert.ToDecimal(sysdayordertotal);
                daysysincome.RefundTotalMoney += returnordertrade.Sum(t => t.PayMoney);
                daysysincome.OrderTotal += sysdaydevide.Count;
                daysysincome.ModifyOn = DateTime.Now;
                await _corporateIncomeTotalService.UpdateCorporateIncomeTotal(daysysincome);
            }
            //#region 新增月数据

            //////月 公司收入

            var monthsysIncomeList = sysIncome.Where(t => t.IncomeType == "month" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();
            if (monthsysIncomeList.Count > 0)
            {
                //  var sysConsultationmonthlist = ConsultationList.Where(t => t.CreatedOn >= MonthStartTime && t.CreatedOn <= MonthEndTime).ToList();

                var monthsysIncome = monthsysIncomeList.FirstOrDefault();

                var daydocincomelist = sysIncome.Where(t => t.IncomeType == "day" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();

                var daytotalIenum = from a in daydocincomelist
                                    group a by a.IncomeType into g
                                    select new
                                    {
                                        g.Key,
                                        totalmoney = g.Sum(a => a.OrderTotalMoney),
                                        incometotal = g.Sum(a => a.IncomeTotal),
                                        refundtotalmoney = g.Sum(a => a.RefundTotalMoney),
                                        total = g.Sum(a => a.OrderTotal)
                                    };
                var daytotal = daytotalIenum.FirstOrDefault();

                monthsysIncome.IncomeTotal = Convert.ToDecimal(daytotal.incometotal);
                monthsysIncome.OrderTotalMoney = Convert.ToDecimal(daytotal.totalmoney);
                monthsysIncome.OrderTotal = daytotal.total;
                monthsysIncome.RefundTotalMoney = daytotal.refundtotalmoney;
                monthsysIncome.TotalDate = totaldate;
                monthsysIncome.ModifyOn = DateTime.Now;

                await _corporateIncomeTotalService.UpdateCorporateIncomeTotal(monthsysIncome);
            }
            else
            {
                var monthCorporateIncomeTotal = new CorporateIncomeTotal();
                monthCorporateIncomeTotal.IncomeType = "month";
                monthCorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                monthCorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                monthCorporateIncomeTotal.RefundTotalMoney = returnordertrade.Sum(t => t.PayMoney);
                monthCorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                monthCorporateIncomeTotal.CreatedOn = DateTime.Now;
                monthCorporateIncomeTotal.TotalDate = totaldate;
                await _corporateIncomeTotalService.CreateCorporateIncomeTotal(monthCorporateIncomeTotal);

            }
            ////年 公司收入
            var yaersysIncomeList = sysIncome.Where(t => t.IncomeType == "year" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();
            if (yaersysIncomeList.Count > 0)
            {
                // var sysConsultationmonthlist = ConsultationList.Where(t => t.CreatedOn >= MonthStartTime && t.CreatedOn <= MonthEndTime).ToList();

                var yearsysIncome = yaersysIncomeList.FirstOrDefault();

                var daydocincomelist = sysIncome.Where(t => t.IncomeType == "month" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();

                var monthtotalIenum = from a in daydocincomelist
                                      group a by a.IncomeType into g
                                      select new
                                      {
                                          g.Key,
                                          totalmoney = g.Sum(a => a.OrderTotalMoney),
                                          incometotal = g.Sum(a => a.IncomeTotal),
                                          refundtotalmoney = g.Sum(a => a.RefundTotalMoney),
                                          total = g.Sum(a => a.OrderTotal)
                                      };
                var month = monthtotalIenum.FirstOrDefault();

                yearsysIncome.IncomeTotal = Convert.ToDecimal(month.incometotal);
                yearsysIncome.OrderTotalMoney = Convert.ToDecimal(month.totalmoney);
                yearsysIncome.RefundTotalMoney = month.refundtotalmoney;
                yearsysIncome.OrderTotal = month.total;
                yearsysIncome.TotalDate = totaldate;
                yearsysIncome.ModifyOn = DateTime.Now;

                await _corporateIncomeTotalService.UpdateCorporateIncomeTotal(yearsysIncome);
            }
            else
            {
                var monthCorporateIncomeTotal = new CorporateIncomeTotal();
                monthCorporateIncomeTotal.IncomeType = "year";
                monthCorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                monthCorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                monthCorporateIncomeTotal.RefundTotalMoney = returnordertrade.Sum(t => t.PayMoney);
                monthCorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                monthCorporateIncomeTotal.CreatedOn = DateTime.Now;
                monthCorporateIncomeTotal.TotalDate = totaldate;
                await _corporateIncomeTotalService.CreateCorporateIncomeTotal(monthCorporateIncomeTotal);

            }
            #endregion
            #endregion
            var result = await _hangFireJobService.UpdateHangFireJob(UpdateHangFireJob);
            
            HangFireJob ReturnhangFireJob = new HangFireJob();
            ReturnhangFireJob.JobName = "咨询分账统计";
            ReturnhangFireJob.JobCode = "IncomeTotal";
            //  ReturnhangFireJob.BusinessID = ConsultationInfo.Id;
            // ReturnhangFireJob.BusinessCode = "";
            //ReturnhangFireJob.JobRunTime = ConsultationInfo.Overtime;
            ReturnhangFireJob.JobRunTime = Convert.ToDateTime(relruntime.ToString("yyyy-MM-dd")).AddDays(1).AddHours(double.Parse(SharingTime.ItemValue));
            ReturnhangFireJob.JobSates = "Open";
            ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/IncomeTotal/";
            ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);

            HangfireScheduleJob job = new HangfireScheduleJob();
            JobModel model = new JobModel();
            model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
            model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
            model.queues = "totalqueue";
            model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间
            var returnjobid = job.Schedule(model);
            if (returnjobid.IndexOf("error") < 0)
            {
                ReturnhangFireJob.JobRunID = returnjobid;
                _hangFireJobService.CreateSingleHangFireJob(ReturnhangFireJob);
            }
            if (!string.IsNullOrEmpty(UpdateHangFireJob.JobRunID))
            {
                job.DeleteSchedule(UpdateHangFireJob.JobRunID);
            }
            #endregion
            return new ObjectResultModule("", 200, "success");
        }

        [Route("api/profitsharingfinish")]
        [HttpPost]
        [AbpAuthorize]

        public async Task<ObjectResultModule> profitsharingfinish([FromBody]OrderTradeRecord hangFireJobInfo)
        {
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }

            SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
            systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
            var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
            var tencentparam = configs.FirstOrDefault();
            TencentWXPay tencentWXPay = new TencentWXPay();
            var shareresult = await tencentWXPay.profitsharingfinish(tencentparam, hangFireJobInfo.OrderNumber, hangFireJobInfo.WXTransactionId, hangFireJobInfo.WXPayBillno);
            return new ObjectResultModule(shareresult, 200, "");
        }
        #region 测试微信支付
        ///// <summary>
        ///// 微信pay信息
        ///// </summary>
        ///// <param name="secret"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/WXOAuthPayTest")]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> WXOAuthPayTest([FromBody]SecretModel secret)
        //{


        //#endregion

        //return "SUCCESS";
        //SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
        //systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
        //var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
        //var tencentparam = configs.FirstOrDefault();

        //TencentWXPay tencentWXPay = new TencentWXPay();
        //List<receivershare> receivers = new List<receivershare>();
        //var receivershare = new receivershare();
        //receivershare.name = "黄彪";
        //receivershare.type = "PERSONAL_OPENID";
        //receivershare.account = "oOiwG1lvLGJeij_R8NdYmiHuOQXY";
        //receivershare.amount = Convert.ToInt32(80);
        //receivers.Add(receivershare);
        //var shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, "4200000234201812188859300979", "20181218210136133155139");
        //var Sahring = shareresult.result_code;

        //SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
        //systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
        //var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
        //var tencentparam = configs.FirstOrDefault();
        //TencentWXPay tencentWXPay = new TencentWXPay();

        //var doctor = await _yaeherDoctorService.YaeherDoctorByID(2);
        ////  doctor.DoctorName = "李义";
        //var user = await _yaeherUserService.YaeherUserByID(8);

        //var receiver = new receiver();
        //receiver.name = "李义";
        //receiver.type = "PERSONAL_OPENID";
        //receiver.account = "oOiwG1si-LaGiZcH_mhc7LFc2No8";
        //var addresult = await tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam);


        //receiver = new receiver();
        //receiver.name = "李义";
        //receiver.type = "PERSONAL_OPENID";
        //receiver.account = "oOiwG1si-LaGiZcH_mhc7LFc2No8";
        //var removeresult = await tencentWXPay.ProfitSharingRemoveReceiver(receiver, tencentparam);

        //List<receivershare> receivers = new List<receivershare>();
        //var receivershare = new receivershare();
        //receivershare.name = "李义";
        //receivershare.type = "PERSONAL_OPENID";
        //receivershare.account = "oOiwG1si-LaGiZcH_mhc7LFc2No8";
        //receivershare.amount = 1;
        //receivers.Add(receivershare);
        //var shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, "4200000220201812013668188467", "201812011538161882018");


        //string key = "8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm";
        //var nonce_str = TenPayV3Util.GetNoncestr();
        //#region  新增分账账户
        //TencentUserManage tencentUserManage = new TencentUserManage();
        //CreateReceiver create = new CreateReceiver();


        //var receiverstr = JsonHelper.ToJson(receiver);


        //var requestHandler2 = new RequestHandler();
        //requestHandler2.SetParameter("mch_id", "1516692491");
        //requestHandler2.SetParameter("appid", "wx6ab2890bc0b72574");
        //requestHandler2.SetParameter("nonce_str", nonce_str);

        //requestHandler2.SetParameter("sign_type", "HMAC-SHA256");
        //requestHandler2.SetParameter("receiver", "{\"type\":\"PERSONAL_OPENID\",\"account\":\"oOiwG1si-LaGiZcH_mhc7LFc2No8\",\"name\":\"李义\"}");
        //////HMAC-SHA256加密签名
        //var sha256Sign = requestHandler2.CreateSha256Sign("key", key);
        ////requestHandler2.SetParameter("sign", sha256Sign);
        ////Hashtable Parameters = new Hashtable(); ;
        ////Parameters.Add("mch_id", "1516692491");
        ////Parameters.Add("appid", "wxd930ea5d5a258f4f");
        ////Parameters.Add("nonce_str", nonce_str);
        ////Parameters.Add("sign_type", "HMAC-SHA256");
        ////Parameters.Add("receiver" ,"{\"type\":\"PERSONAL_OPENID\",\"account\":\"oOiwG1qmIfhnXwJVkgY8ns7aI-g0\",\"name\":\"\"}");
        ////var sha256Sign = CreateSha256Sign("key", Parameters, "8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm");
        //requestHandler2.SetParameter("sign", sha256Sign);

        //string urlFormat = "https://api.mch.weixin.qq.com/pay/profitsharingaddreceiver";

        ////var urlFormat = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}pay/orderquery");
        //var data = requestHandler2.ParseXML();//获取XML
        //var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
        //MemoryStream ms = new MemoryStream();
        //ms.Write(formDataBytes, 0, formDataBytes.Length);
        //ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //var resultXml = RequestUtility.HttpPost(urlFormat, null, ms);

        //var _resultXml = XDocument.Parse(resultXml);
        //var result_code = GetXmlValue(_resultXml,"result_code"); // res.Element("xml").Element

        //Logger.Info("resultXml" + resultXml);
        ////   return new OrderQueryResult(resultXml);

        //#endregion 单次分账
        // requestHandler2 = new RequestHandler();
        //requestHandler2.SetParameter("mch_id", "1516692491");
        //requestHandler2.SetParameter("appid", "wx6ab2890bc0b72574");
        //var profitsharingnonce_str = TenPayV3Util.GetNoncestr();
        //requestHandler2.SetParameter("nonce_str", profitsharingnonce_str);
        //requestHandler2.SetParameter("sign_type", "HMAC-SHA256");
        //requestHandler2.SetParameter("transaction_id", "4200000229201812017893268206");
        //requestHandler2.SetParameter("out_order_no", "201812011547135547348");

        //requestHandler2.SetParameter("receivers", "[{\"type\":\"PERSONAL_OPENID\",\"account\":\"oOiwG1si-LaGiZcH_mhc7LFc2No8\",\"amount\":1,\"description\":\"测试test\"}]");
        //////HMAC-SHA256加密签名
        //sha256Sign = requestHandler2.CreateSha256Sign("key", key);
        //requestHandler2.SetParameter("sign", sha256Sign);
        //urlFormat = "https://api.mch.weixin.qq.com/secapi/pay/profitsharing";

        //data = requestHandler2.ParseXML();//获取XML
        //formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
        //ms = new MemoryStream();
        //ms.Write(formDataBytes, 0, formDataBytes.Length);
        //ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //var cert = @"C:\cert\apiclient_cert.p12";//根据自己的证书位置修改
        //var password = "1516692491";//默认为商户号，建议修改
        //var result = await ShareAsync(requestHandler2, urlFormat, cert, password);

        #region 删除分账账户

        //TencentUserManage tencentUserManage1 = new TencentUserManage();
        //CreateReceiver create1 = new CreateReceiver();
        //var receiver1 = new receiver();
        //receiver1.account = "oOiwG1si-LaGiZcH_mhc7LFc2No8";//liyi
        //// receiver.account = "oOiwG1qmIfhnXwJVkgY8ns7aI-g0";
        //receiver1.name = "李义";
        //receiver1.type = "PERSONAL_OPENID";
        ////var receiverstr1 = JsonHelper.ToJson(receiver);
        //var nonce_str1 = TenPayV3Util.GetNoncestr();
        //var requestHandler3 = new RequestHandler();
        //requestHandler3.SetParameter("mch_id", "1516692491");
        //requestHandler3.SetParameter("appid", "wx6ab2890bc0b72574");
        //requestHandler3.SetParameter("nonce_str", nonce_str1);

        //requestHandler3.SetParameter("sign_type", "HMAC-SHA256");
        //requestHandler3.SetParameter("receiver", "{\"type\":\"PERSONAL_OPENID\",\"account\":\"oOiwG1si-LaGiZcH_mhc7LFc2No8\",\"name\":\"李义\"}");
        //////HMAC-SHA256加密签名
        //var sha256Sign1 = requestHandler3.CreateSha256Sign("key", key);
        ////requestHandler2.SetParameter("sign", sha256Sign);
        ////Hashtable Parameters = new Hashtable(); ;
        ////Parameters.Add("mch_id", "1516692491");
        ////Parameters.Add("appid", "wxd930ea5d5a258f4f");
        ////Parameters.Add("nonce_str", nonce_str);
        ////Parameters.Add("sign_type", "HMAC-SHA256");
        ////Parameters.Add("receiver" ,"{\"type\":\"PERSONAL_OPENID\",\"account\":\"oOiwG1qmIfhnXwJVkgY8ns7aI-g0\",\"name\":\"\"}");
        ////var sha256Sign = CreateSha256Sign("key", Parameters, "8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm");
        //requestHandler3.SetParameter("sign", sha256Sign1);
        //string urldelete = "https://api.mch.weixin.qq.com/pay/profitsharingremovereceiver";
        //var data1 = requestHandler3.ParseXML();//获取XML
        //var formDataBytes1 = data1 == null ? new byte[0] : Encoding.UTF8.GetBytes(data1);
        //MemoryStream ms1 = new MemoryStream();
        //ms1.Write(formDataBytes1, 0, formDataBytes1.Length);
        //ms1.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        //var resultXml1 = RequestUtility.HttpPost(urldelete, null, ms1);
        #endregion
        //  var reuslt = await tencentUserManage.Profitsharingaddreceiver(create);

        //    return new ObjectResultModule("", 200, "success");
        //}
        ///// <summary>
        ///// 测试微信pay信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/WXOAuthPayTest")]
        //[AbpAllowAnonymous]
        //public async Task<ObjectResultModule> WXOAuthPayTest([FromBody]TencentPayModel model)
        //{
        //    var WXPayBillno = "201811291445327145038";
        //    SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
        //    systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
        //    var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
        //    var tencentparam = configs.FirstOrDefault();
        //    Logger.Info("tencentparam" + tencentparam);
        //    //   WXPayBillno = tencentparam.TenPayMchId + ConsultationInfo.sp_billno;
        //    TencentWXPay tencentWXPay = new TencentWXPay();
        //    var queryresult = await tencentWXPay.OrderQueryAsync(WXPayBillno, tencentparam);
        //    Logger.Info("OrderQueryAsync:" + JsonHelper.ToJson(queryresult));
        //    if (queryresult.code != "SUCCESS"||int.Parse(queryresult.totalmoney) != (int)(0.01 * 100))
        //    {
        //        return new ObjectResultModule("", 400, "查询支付信息失败,请重新提交！");
        //    }

        //    string outTradeNo = WXPayBillno;
        //    string outRefundNo = "outnumber-1516692491"+DateTime.Now.ToString("yyyyMMddHHmmss");
        //    var totalFee = 1;//单位：分
        //    int refundFee = totalFee;
        //    TencentWXPay tencentWXPay1 = new TencentWXPay();
        //    var refundpayresult = await tencentWXPay1.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);
        //    if (refundpayresult.code != "SUCCESS")
        //    {
        //        return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
        //    }
        //    return new ObjectResultModule(refundpayresult, 200, "");
        //}
        #endregion

        #region 回调（暂不需要）
        //        //退单回调        http://admin/integraltel.com/api/RefundPayNotify
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        [HttpPost]
        //        [Route("api/PayNotify")]
        //        public async Task<ActionResult> PayNotify()
        //        {
        //            try
        //            {
        //                CreateWrite("C:\\回调.txt", "PayNotifyin" + DateTime.Now.ToString("yyyy-MM-ddTmm:hh:ss"));
        //                ResponseHandler resHandler = new ResponseHandler(HttpContext);
        //                CreateWrite("C:\\回调.txt", "model:" + JsonHelper.ToJson(resHandler));
        //                string return_code = resHandler.GetParameter("return_code");
        //                string return_msg = resHandler.GetParameter("return_msg");

        //                CreateWrite("C:\\回调.txt", "return_code:" + return_code);
        //                CreateWrite("C:\\回调.txt", "return_msg:" + return_msg);
        //                TenPayV3Info TenPayV3Info = new TenPayV3Info("wx6ab2890bc0b72574", "05eaa0c76a08eac353f4e4ce89a3702f", "1516692491", "8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm", "http://admin/integraltel.com/api/PayNotify", "http://admin/integraltel.com/api/PayNotify");

        //                string res = null;
        //                resHandler.SetKey(TenPayV3Info.Key);
        //                //验证请求是否从微信发过来（安全）
        //                if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
        //                {
        //                    res = "success";//正确的订单处理
        //                                    //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
        //                    var user = await _yaeherUserService.YaeherUserByID(2);
        //                    CreateWrite("C:\\回调.txt", "user:" + JsonHelper.ToJson(user));

        //                }
        //                else
        //                {
        //                    res = "wrong";//错误的订单处理
        //                }
        //                /* 这里可以进行订单处理的逻辑 */
        //                string xml = string.Format(@"<xml>
        //<return_code><![CDATA[{0}]]></return_code>
        //<return_msg><![CDATA[{1}]]></return_msg>
        //</xml>", return_code, return_msg);

        //                CreateWrite("C:\\回调.txt", "model:" + xml);

        //                return Content(xml, "text/xml");
        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        [HttpPost]
        //        [Route("api/RefundNotify")]
        //        /// <summary>
        //        /// 退款通知地址
        //        /// </summary>
        //        /// <returns></returns>
        //        public async Task<ActionResult> RefundNotify()
        //        {
        //            // WeixinTrace.SendCustomLog("RefundNotifyUrl被访问", "IP" + HttpContext.UserHostAddress()?.ToString());
        //            CreateWrite("C:\\回调.txt", "in" + DateTime.Now.ToString("yyyy-MM-ddTmm:hh:ss"));
        //            CreateWrite("C:\\回调.txt", "IP" + HttpContext.UserHostAddress()?.ToString());

        //            string responseCode = "FAIL";
        //            string responseMsg = "FAIL";
        //            try
        //            {
        //                TenPayV3Info TenPayV3Info = new TenPayV3Info("wx6ab2890bc0b72574", "05eaa0c76a08eac353f4e4ce89a3702f", "1516692491", "8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm", "http://admin/integraltel.com/api/RefundNotify", "http://admin/integraltel.com/api/PayNotify");

        //                ResponseHandler resHandler = new ResponseHandler(null);

        //                string return_code = resHandler.GetParameter("return_code");
        //                string return_msg = resHandler.GetParameter("return_msg");

        //                // WeixinTrace.SendCustomLog("跟踪RefundNotifyUrl信息", resHandler.ParseXML());
        //                CreateWrite("C:\\回调.txt", "跟踪RefundNotifyUrl信息" + resHandler.ParseXML());


        //                if (return_code == "SUCCESS")
        //                {
        //                    responseCode = "SUCCESS";
        //                    responseMsg = "OK";

        //                    string appId = resHandler.GetParameter("appid");
        //                    string mch_id = resHandler.GetParameter("mch_id");
        //                    string nonce_str = resHandler.GetParameter("nonce_str");
        //                    string req_info = resHandler.GetParameter("req_info");

        //                    var decodeReqInfo = TenPayV3Util.DecodeRefundReqInfo(req_info, TenPayV3Info.Key);
        //                    var decodeDoc = XDocument.Parse(decodeReqInfo);

        //                    //获取接口中需要用到的信息
        //                    string transaction_id = decodeDoc.Root.Element("transaction_id").Value;
        //                    string out_trade_no = decodeDoc.Root.Element("out_trade_no").Value;
        //                    string refund_id = decodeDoc.Root.Element("refund_id").Value;
        //                    string out_refund_no = decodeDoc.Root.Element("out_refund_no").Value;
        //                    int total_fee = int.Parse(decodeDoc.Root.Element("total_fee").Value);
        //                    int? settlement_total_fee = decodeDoc.Root.Element("settlement_total_fee") != null
        //                            ? int.Parse(decodeDoc.Root.Element("settlement_total_fee").Value)
        //                            : null as int?;
        //                    int refund_fee = int.Parse(decodeDoc.Root.Element("refund_fee").Value);
        //                    int tosettlement_refund_feetal_fee = int.Parse(decodeDoc.Root.Element("settlement_refund_fee").Value);
        //                    string refund_status = decodeDoc.Root.Element("refund_status").Value;
        //                    string success_time = decodeDoc.Root.Element("success_time").Value;
        //                    string refund_recv_accout = decodeDoc.Root.Element("refund_recv_accout").Value;
        //                    string refund_account = decodeDoc.Root.Element("refund_account").Value;
        //                    string refund_request_source = decodeDoc.Root.Element("refund_request_source").Value;

        //                    var user = await _yaeherUserService.YaeherUserByID(2);
        //                    CreateWrite("C:\\回调.txt", "user:" + JsonHelper.ToJson(user));

        //                    //进行业务处理
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                responseMsg = ex.Message;
        //                //    WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
        //            }

        //            string xml = string.Format(@"<xml>
        //<return_code><![CDATA[{0}]]></return_code>
        //<return_msg><![CDATA[{1}]]></return_msg>
        //</xml>", responseCode, responseMsg);
        //            return Content(xml, "text/xml");
        //        }
        #endregion
        /// <summary>
        /// 退单
        /// </summary>
        /// <param name="refund"></param>
        /// <returns></returns>
        [Route("api/RefundAsync")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundAsync([FromBody]RefundModel refund)
        {
            if (!Commons.CheckSecret(refund.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                throw new ArgumentNullException("secret");
            }
            SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
            systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
            var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
            var tencentparam = configs.FirstOrDefault();

            TenPayV3Info TenPayV3Info = new TenPayV3Info(tencentparam.AppID, tencentparam.AppSecret, tencentparam.TenPayMchId, tencentparam.TenPayKey, tencentparam.TenPayNotify, tencentparam.TenPayWxOpenNotify);
            string nonceStr = TenPayV3Util.GetNoncestr();
            string opUserId = TenPayV3Info.MchId;
            var notifyUrl = "http://admin/integraltel.com/api/RefundPayNotify";
            var dataInfo = new TenPayV3RefundRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, TenPayV3Info.Key,
                null, nonceStr, null, refund.outTradeNo, refund.outRefundNo, refund.totalFee, refund.refundFee, opUserId, null, refundDescription: refund.msg, notifyUrl: notifyUrl);
            var cert = @"C:\cert\apiclient_cert.p12";//根据自己的证书位置修改
            var password = TenPayV3Info.MchId;//默认为商户号，建议修改
            try
            {
                var wxpayresult = await TenPayV3.RefundAsync(dataInfo, cert, password);
                if (wxpayresult.result_code != "SUCCESS")
                {
                    Logger.Info("退款失败！"+JsonHelper.ToJson(refund)+JsonHelper.ToJson(wxpayresult));
                    if (wxpayresult.err_code_des != "订单已全额退款")
                    {
                        throw new ArgumentNullException(wxpayresult.result_code);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Info("退款失败！"+JsonHelper.ToJson(refund)+ex.Message.ToString());
                throw ex;
            }

            return new ObjectResultModule("", 200, "");
        }
    }
}