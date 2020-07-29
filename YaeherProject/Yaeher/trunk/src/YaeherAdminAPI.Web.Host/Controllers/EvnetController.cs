using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.HangfireJob;
using Yaeher.Common.TencentCustom;
using Yaeher.Consultation;
using Yaeher.DoctorReport;
using Yaeher.EventBus;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;
using Yaeher.HangFire;
using Yaeher.NumericalStatement;
using Yaeher.NumericalStatement.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;
using Yaeher.EventBus.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EvnetController : YaeherAppServiceBase
    {
        private IPublishsService _publishsService;
        private IReceiveEventService _receiveEventService;
        private ISubscribeService _subscribeService;
        private ISubscribetionService _subscribetionService;
        private IHangFireJobService _hangFireJobService;
        private IConsultationService _consultationService;
        private IConsultationOrderTotalService _consultationOrderTotalService;
        private IEvaluationTotalService _evaluationTotalService;
        private readonly IAbpSession _IabpSession;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IYaeherUserService _yaeherUserService;
        private readonly IServiceMoneyListService _serviceMoneyListService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IRefundManageService _refundManageService;
        private readonly ISystemConfigsService _systemConfigsService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IncomeDevideService _incomeDevideService;
        private readonly DoctorOnlineRecordService _doctorOnlineRecordService;
        private readonly IIncomeDetailsService _incomeDetailsService;
        private readonly IWecharSendMessageService _wecharSendMessageService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishsService"></param>
        /// <param name="receiveEventService"></param>
        /// <param name="subscribeService"></param>
        /// <param name="subscribetionService"></param>
        /// <param name="hangFireJobService"></param>
        /// <param name="consultationService"></param>
        /// <param name="consultationOrderTotalService"></param>
        /// <param name="evaluationTotalService"></param>
        /// <param name="abpSession"></param>
        /// <param name="orderManageService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="serviceMoneyListService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="refundManageService"></param>
        /// <param name="systemConfigsService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="incomeDevideService"></param>
        /// <param name="doctorOnlineRecordService"></param>
        /// <param name="incomeDetailsService"></param>
        /// <param name="wecharSendMessageService"></param>
        public EvnetController(IPublishsService publishsService,
                               IReceiveEventService receiveEventService,
                               ISubscribeService subscribeService,
                               ISubscribetionService subscribetionService,
                               IHangFireJobService hangFireJobService,
                               IConsultationService consultationService,
                               IConsultationOrderTotalService consultationOrderTotalService,
                               IEvaluationTotalService evaluationTotalService,
                               IAbpSession abpSession,
                               IOrderManageService orderManageService,
                               IOrderTradeRecordService orderTradeRecordService,
                               IYaeherUserService yaeherUserService,
                               IServiceMoneyListService serviceMoneyListService,
                               ISystemParameterService systemParameterService,
                               IYaeherDoctorService yaeherDoctorService,
                               IRefundManageService refundManageService,
                               ISystemConfigsService systemConfigsService,
                               IUnitOfWorkManager unitOfWorkManager,
                               IncomeDevideService incomeDevideService,
                               DoctorOnlineRecordService doctorOnlineRecordService,
                               IIncomeDetailsService incomeDetailsService,
                               IWecharSendMessageService wecharSendMessageService)
        {
            _publishsService = publishsService;
            _receiveEventService = receiveEventService;
            _subscribeService = subscribeService;
            _subscribetionService = subscribetionService;
            _hangFireJobService = hangFireJobService;
            _IabpSession = abpSession;
            _consultationService = consultationService;
            _consultationOrderTotalService = consultationOrderTotalService;
            _evaluationTotalService = evaluationTotalService;
            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _yaeherUserService = yaeherUserService;
            _serviceMoneyListService = serviceMoneyListService;
            _systemParameterService = systemParameterService;
            _yaeherDoctorService = yaeherDoctorService;
            _refundManageService = refundManageService;
            _systemConfigsService = systemConfigsService;
            _unitOfWorkManager = unitOfWorkManager;
            _incomeDevideService = incomeDevideService;
            _doctorOnlineRecordService = doctorOnlineRecordService;
            _incomeDetailsService = incomeDetailsService;
            _wecharSendMessageService = wecharSendMessageService;
        }


        /// <summary>
        /// 发布任务
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [Route("api/CreatePublishs")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> CreatePublishs([FromBody] Publishs PublishsInfo)
        {
            var PublishsInfos = await _publishsService.PublishsList();
            PublishsInfo = PublishsInfos.Where(a => a.Id == 2).FirstOrDefault();
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreatePublishs = new Publishs()
            {
                Publisher = PublishsInfo.Publisher,
                PublishUrl = PublishsInfo.PublishUrl,
                EventName = PublishsInfo.EventName,
                EventCode = PublishsInfo.EventCode,
                BusinessID = PublishsInfo.BusinessID,
                BusinessCode = PublishsInfo.BusinessCode,
                BusinessJSON = PublishsInfo.BusinessJSON,
                PublishedTime = PublishsInfo.PublishedTime,
                PublishStatus = true,
                ServerClient = "Server",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            #region 先将发布的内容传递一次给订阅者
            // 查询订阅者
            var SubscribeList = await _subscribeService.SubscribeList();
            SubscribeList = SubscribeList.Where(a => a.EventCode == PublishsInfo.EventCode && a.ActionStatus == true).ToList();
            if (SubscribeList.Count > 0)
            {
                foreach (var Subscribe in SubscribeList)
                {
                    PendingEvent pendingEvent = new PendingEvent();
                    pendingEvent.SubscribeID = Subscribe.Id;
                    pendingEvent.Subscriber = Subscribe.Subscriber;
                    pendingEvent.CallbackUrl = Subscribe.CallbackUrl;
                    pendingEvent.EventCode = Subscribe.EventCode;
                    pendingEvent.EventName = Subscribe.EventName;
                    pendingEvent.BusinessID = PublishsInfo.BusinessID;
                    pendingEvent.BusinessJSON = PublishsInfo.BusinessJSON;
                    pendingEvent.BusinessCode = PublishsInfo.BusinessCode;
                    pendingEvent.PublishedTime = PublishsInfo.PublishedTime;
                    await RelayPublishs(pendingEvent);
                }
            }
            #endregion
            var result = await _publishsService.CreatePublishs(CreatePublishs);
            if (result.Id > 0)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 注册订阅者
        /// </summary>
        /// <param name="SubscribeInfo"></param>
        /// <returns></returns>
        [Route("api/CreateSubscribe")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateSubscribe([FromBody] Subscribe SubscribeInfo)
        {
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateSubscribe = new Subscribe()
            {
                Subscriber = SubscribeInfo.Subscriber,
                CallbackUrl = SubscribeInfo.CallbackUrl,
                EventName = SubscribeInfo.EventName,
                EventCode = SubscribeInfo.EventCode,
                RegisterTime = SubscribeInfo.RegisterTime,
                ServerClient = "Client",
                ActionStatus = true,  // 默认true
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _subscribeService.CreateSubscribe(CreateSubscribe);
            if (result.Id > 0)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 取消订阅者
        /// </summary>
        /// <param name="SubscribeInfo"></param>
        /// <returns></returns>
        [Route("api/CancelSubscribe")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CancelSubscribe([FromBody] Subscribe SubscribeInfo)
        {
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateSubscribe = new Subscribe()
            {
                Subscriber = SubscribeInfo.Subscriber,
                CallbackUrl = SubscribeInfo.CallbackUrl,
                EventName = SubscribeInfo.EventName,
                EventCode = SubscribeInfo.EventCode,
                RegisterTime = SubscribeInfo.RegisterTime,
                ActionStatus = false,  // false
                IsDelete = true,
                CreatedBy = userid,
                ModifyOn = DateTime.Now
            };
            var result = await _subscribeService.DeleteSubscribe(CreateSubscribe);
            if (result.Id > 0)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 执行订阅机制  将消息发布到订阅者
        /// </summary>
        [Route("api/DoSubscribeTask")]
        [HttpPost]
        [AbpAuthorize]
        public async void DoSubscribeTask()
        {
            //获取订阅者Subscribe
            var subscribesList = await _subscribeService.SubscribeList();
            subscribesList = subscribesList.Where(a => a.ServerClient == "Server").ToList();
            //获取发布者消息Publishs
            var PublishsList = await _publishsService.PublishsList();
            PublishsList = PublishsList.Where(a => a.ServerClient == "Server").ToList();
            //获取执行记录 ReceiveEvent
            var ReceiveEventsList = await _receiveEventService.ReceiveEventList();
            var ReceiveEvents = ReceiveEventsList.Select(c => new PendingEvent { Id = c.PublishId, SubscribeID = c.SubscribeID }).ToList();
            try
            {
                var List = (from a in subscribesList
                            join b in PublishsList on a.EventName equals b.EventName into joinlist
                            from leftjoin in joinlist.DefaultIfEmpty()
                            select new PendingEvent
                            {
                                SubscribeID = a.Id,
                                Subscriber = a.Subscriber,
                                CallbackUrl = a.CallbackUrl,
                                EventName = a.EventName,
                                EventCode = a.EventCode,
                                CreatedOn = a.CreatedOn,
                                CreatedBy = a.CreatedBy,
                                Id = leftjoin.Id,//发布者发布的消息id
                                PublishUrl = leftjoin.PublishUrl,
                                BusinessID = leftjoin.BusinessID,
                                BusinessCode = leftjoin.BusinessCode,
                                BusinessJSON = leftjoin.BusinessJSON,
                                PublishedTime = leftjoin.PublishedTime,
                            }).ToList();
                //查询出不在ReceiveEvent表格的发送数据
                var sendEvent = (from sendList in List
                                 where !(from publishlist in List
                                         join b in ReceiveEvents on new { pid = publishlist.Id, sid = publishlist.SubscribeID } equals new { pid = b.Id, sid = b.SubscribeID }
                                         select publishlist.Id).Contains(sendList.Id)
                                 select sendList);
                foreach (var Item in sendEvent)
                {
                    await RelayPublishs(Item);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 跟据筛选数据进行发布消息
        /// </summary>
        /// <param name="PublishsInfo"></param>
        [Route("api/RelayPublishs")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<string> RelayPublishs(PendingEvent PublishsInfo)
        {
            #region 转发消息到订阅者
            Subscribetion SubscribetionInfo = new Subscribetion();
            SubscribetionInfo.Id = PublishsInfo.SubscribeID;
            SubscribetionInfo.Subscriber = PublishsInfo.Subscriber;
            SubscribetionInfo.CallbackUrl = PublishsInfo.CallbackUrl;
            SubscribetionInfo.EventName = PublishsInfo.EventName;
            SubscribetionInfo.EventCode = PublishsInfo.EventCode;//发布者发布的消息id
            SubscribetionInfo.BusinessID = PublishsInfo.BusinessID;
            SubscribetionInfo.BusinessCode = PublishsInfo.BusinessCode;
            SubscribetionInfo.BusinessJSON = PublishsInfo.BusinessJSON;
            SubscribetionInfo.PublishedTime = PublishsInfo.PublishedTime;
            SubscribetionInfo.ExecuteStatus = true;
            SubscribetionInfo.ExecuteDate = DateTime.Parse(PublishsInfo.PublishedTime.ToString("yyyy-MM-dd HH:mm:ss"));
            SubscribetionInfo.IsDelete = false;
            #endregion
            //如果消息返回成功则往ReceiveEvent插入数据
            bool issuccess = false;
            try
            {
                var SubscribetionJSON = JsonHelper.ToJson(SubscribetionInfo);
                var result = await PostResponseAsync(SubscribetionInfo.CallbackUrl, SubscribetionJSON);
                if (result == null)
                {
                    issuccess = true;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            if (issuccess)
            {
                //这里要添加判断client success
                ReceiveEvent ReceiveEventinfo = new ReceiveEvent();
                ReceiveEventinfo.PublishId = PublishsInfo.Id;       //监听者ID
                ReceiveEventinfo.SubscribeID = PublishsInfo.SubscribeID; // 订阅者ID
                ReceiveEventinfo.ReceptionMessage = "";
                ReceiveEventinfo.ReceptionDate = DateTime.Now;
                await _receiveEventService.CreateReceiveEvent(ReceiveEventinfo);  // 同时将转发记录存储到监听服务
                return "success";
            }
            else
            {
                return "fail";
            }
        }

        /// <summary>
        /// 回传执行咨询完成  咨询退单
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/DoHangFire")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoHangFire(HangFireJob hangFireJobInfo)
        {
            //Logger.Info("hangfirejob" + JsonHelper.ToJson(hangFireJobInfo));
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }

            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateHangFireJob = await _hangFireJobService.HangFireJobByID(hangFireJobInfo.Id);
            if (UpdateHangFireJob != null)
            {
                if (hangFireJobInfo.JobSates == "Open")//新增状态过来则open状态转换成完成
                {
                    UpdateHangFireJob.JobSates = "Complete"; // 执行状态
                }
                else
                {
                    UpdateHangFireJob.JobSates = hangFireJobInfo.JobSates; // 执行状态
                }
                UpdateHangFireJob.ModifyOn = DateTime.Now;
                UpdateHangFireJob.ModifyBy = userid;
                var result = await _hangFireJobService.UpdateHangFireJob(UpdateHangFireJob);
                Consultation consultation = new Consultation();
                var yaeherConsultation = await _consultationService.YaeherConsultationByNumber(hangFireJobInfo.BusinessCode);

                //switch (hangFireJobInfo.JobCode)
                //{
                //    case "CompleteConsultation":  // 咨询完成
                //        yaeherConsultation.ConsultState = "success";
                //        break;
                //    case "ReturnConsultation":  // 超时退单
                //        yaeherConsultation.ConsultState = "return";
                //        break;
                //}
                // 系统退单
                if (hangFireJobInfo.JobCode == "ReturnConsultation")
                {
                    if (yaeherConsultation.ConsultState == "created")//防重复 创建状态才可以系统退单
                    {
                        var secret = await CreateSecret();
                        //系统退单
                        var Order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);
                        if (Order == null) { return new ObjectResultModule("", 204, "NoContent"); }

                        var ordertrade = await _orderTradeRecordService.OrderTradeRecordExpress(t => !t.IsDelete && t.OrderNumber == Order.OrderNumber && t.PaymentSourceCode == "order");
                        if (ordertrade == null) { return new ObjectResultModule("", 204, "NoContent"); }

                        var Consultant = await _yaeherUserService.YaeherUserByID(yaeherConsultation.CreatedBy);
                        if (Consultant == null) { return new ObjectResultModule("", 204, "NoContent"); }


                        var ServiceResult = await _serviceMoneyListService.ServiceMoneyListByID(Order.ServiceID);
                        if (ServiceResult == null) { return new ObjectResultModule("", 204, "NoContent"); }

                        var param1 = new SystemParameterIn() { SystemType = "WxPayBusinessId" };
                        var wxparamlist = await _systemParameterService.ParameterList(param1);

                        var DocResult = await _yaeherDoctorService.YaeherDoctorByID(yaeherConsultation.DoctorID);
                        if (DocResult == null) { return new ObjectResultModule("", 204, "NoContent"); }
                        var order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);


                        OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
                        orderTradeRecordIn.AndAlso(t => !t.IsDelete && t.PayType == "wxpay" && t.OrderNumber == order.OrderNumber && t.PaymentState == "paid" && t.PaymentSourceCode == "order");
                        var refundordertradelist = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);
                        var refundordertrade = refundordertradelist.FirstOrDefault();
                        var RefundNumber = "RN-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4);
                        using (var unitOfWork = _unitOfWorkManager.Begin())
                        {
                            var CreateRefundManage = new RefundManage()
                            {
                                RefundNumber = RefundNumber,
                                SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                                ConsultID = yaeherConsultation.Id,
                                ConsultNumber = yaeherConsultation.ConsultNumber,
                                OrderID = Order.Id,
                                OrderNumber = Order.OrderNumber,
                                ConsultantID = yaeherConsultation.CreatedBy,
                                ConsultantName = Consultant.FullName,
                                PatientID = yaeherConsultation.PatientID,
                                PatientName = yaeherConsultation.PatientName,
                                DoctorID = yaeherConsultation.DoctorID,
                                DoctorName = DocResult.DoctorName,
                                OrderCurrency = "rmb",
                                OrderMoney = Convert.ToDecimal(Order.OrderMoney),//订单总金额就是退单金额
                                ServiceID = Order.ServiceID,
                                ServiceName = ServiceResult.DoctorName + ServiceResult.ServiceType,
                                SellerMoneyID = wxparamlist[0].ItemValue,
                                CheckState = "success",//系统退单自动审核通过
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                            };
                            var create = await _refundManageService.CreateRefundManage(CreateRefundManage);
                            yaeherConsultation.ConsultState = "return";
                            yaeherConsultation.RefundState = "return";
                            yaeherConsultation.ConsultState = "return";
                            yaeherConsultation.RefundBy = userid;
                            yaeherConsultation.RefundTime = DateTime.Now;
                            yaeherConsultation.RefundType = "systemreturn";
                            yaeherConsultation.RefundNumber = create.RefundNumber;
                            yaeherConsultation.RefundRemarks = "系统退单";
                            var res = await _consultationService.UpdateYaeherConsultation(yaeherConsultation);




                            var record = new OrderTradeRecord()
                            {
                                SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                                OrderID = order.Id,
                                OrderNumber = order.OrderNumber,
                                PayType = "wxpay",
                                OrderCurrency = "rmb",
                                TenpayNumber = "",//支付账号
                                VoucherNumber = "",//代金券编号
                                VoucherJSON = "",//代金券Json
                                PayMoney = -Convert.ToDecimal(ordertrade.PayMoney),//订单总金额就是退单金额
                                PaymentState = "paid",
                                PaymentSourceCode = "systemreturn",
                                PaymentSource = "系统退单",
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                                WXPayBillno = refundordertrade.WXPayBillno,
                                WXOrderQuery = refundordertrade.WXOrderQuery,
                                WXTransactionId = refundordertrade.WXTransactionId
                            };
                            var result2 = await _orderTradeRecordService.CreateOrderTradeRecord(record);

                            #region 当系统退单  系统完成Job 并关闭预警job 完成job 
                            HangFireJobIn hangFireJobIn = new HangFireJobIn();
                            hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == UpdateHangFireJob.BusinessCode && a.JobSates == "Open");
                            var hangFireJobs = await _hangFireJobService.HangFireJobListAsync(hangFireJobIn);
                            if (hangFireJobs.Count > 0)
                            {
                                foreach (var CancelhangFire in hangFireJobs)
                                {
                                    CancelhangFire.JobSates = "Close";
                                    CancelhangFire.ModifyOn = DateTime.Now;
                                    CancelhangFire.IsDelete = true;
                                    var Canel = await _hangFireJobService.UpdateHangFireJob(CancelhangFire);

                                    //使用hangfire更新已有job
                                    HangfireScheduleJob job = new HangfireScheduleJob();
                                    job.DeleteSchedule(CancelhangFire.JobRunID);
                                }
                            }
                            #endregion

                            #region 超时退单 咨询者接受消息 PatientReturn Systemreturn
                            SendMessageInfo sendMessageInfo = new SendMessageInfo();
                            sendMessageInfo.TemplateCode = "PatientReturn";
                            sendMessageInfo.OperationType = "Systemreturn";
                            sendMessageInfo.ConsultNumber = hangFireJobInfo.BusinessCode;
                            await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                            #endregion

                            consultation.refundManage = create;
                            consultation.orderTradeRecords = result2;
                            consultation.yaeherConsultation = yaeherConsultation;
                            Publishs Consultationpublishs = new Publishs()
                            {
                                Publisher = "System",
                                PublishUrl = "System",
                                EventName = "系统" + hangFireJobInfo.JobName,
                                EventCode = hangFireJobInfo.JobCode,
                                BusinessID = result.Id.ToString(),
                                BusinessCode = yaeherConsultation.ConsultNumber,
                                BusinessJSON = JsonHelper.ToJson(consultation),
                                PublishedTime = result.CreatedOn,
                                PublishStatus = true,
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                                Secret = secret,
                            };
                            var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                            var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                            this.ObjectResultModule.Object = result;
                            unitOfWork.Complete();
                        }
                        #region 发起微信支付退款流程
                        if (!string.IsNullOrEmpty(refundordertrade.WXPayBillno))//过滤前期的未支付数据
                        {
                            SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
                            systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
                            var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
                            var tencentparam = configs.FirstOrDefault();
                            string outTradeNo = refundordertrade.WXPayBillno;
                            string outRefundNo = RefundNumber;
                            var totalFee = (int)(ordertrade.PayMoney * 100);//单位：分
                            int refundFee = totalFee;
                            TencentWXPay tencentWXPay = new TencentWXPay();
                            var refundpayresult = await tencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);
                            if (refundpayresult.code != "SUCCESS")
                            {
                                HangfireScheduleJob job = new HangfireScheduleJob();
                                RefundModel refundModel = new RefundModel();
                                refundModel.Secret = await CreateSecret();
                                refundModel.outTradeNo = outTradeNo;
                                refundModel.outRefundNo = outRefundNo;
                                refundModel.totalFee = totalFee;
                                refundModel.refundFee = refundFee;
                                refundModel.msg = "";
                                JobModel model = new JobModel();
                                model.CallbackUrl = Commons.AdminIp + "api/RefundAsync";
                                model.CallbackContent = JsonHelper.ToJson(refundModel);
                                job.Enqueue(model);
                                //  return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
                            }
                        }
                        #endregion
                    }
                }
                // 系统完成
                else if (hangFireJobInfo.JobCode == "CompleteConsultation")
                {
                    if (yaeherConsultation.ConsultState == "consulting")//防重复 咨询中状态才可以系统自动完成job
                    {
                        var param = new SystemParameterIn() { SystemType = "RemindEvaluation" };
                        var EvaluationTimeList = await _systemParameterService.ParameterList(param);
                        var EvaluationTime = EvaluationTimeList.FirstOrDefault();

                        var secret = await CreateSecret();
                        using (var unitOfWork = _unitOfWorkManager.Begin())
                        {
                            var order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);
                            DoctorOnlineRecordIn doctorOnlineRecordIn = new DoctorOnlineRecordIn();
                            doctorOnlineRecordIn.AndAlso(t => !t.IsDelete && t.DoctorID == yaeherConsultation.DoctorID);
                            var doctoronlinelist = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctorOnlineRecordIn);
                            var doctoronline = doctoronlinelist.FirstOrDefault();

                            IncomeDevide create = new IncomeDevide();
                            create.ConsultNumber = yaeherConsultation.ConsultNumber;
                            create.ConsultID = yaeherConsultation.Id;
                            create.DoctorName = yaeherConsultation.DoctorName;
                            create.OrderID = order.Id;
                            create.DoctorID = yaeherConsultation.DoctorID;
                            create.OrderNumber = order.OrderNumber;
                            create.OrderMoney = order.OrderMoney;
                            create.OrderCurrency = order.OrderCurrency;
                            create.DevideMoney = order.OrderMoney * Convert.ToDecimal(doctoronline.DivideInto / 100);
                            create.DevideRatio = doctoronline.DivideInto;
                            create.DevideTime = DateTime.Now.AddDays(doctoronline.IncomeDay);
                            create.WXSharing = "Open";
                            create.CreatedOn = DateTime.Now;
                            create.CreatedBy = 0;
                            await _incomeDevideService.CreateIncomeDevide(create);
                            yaeherConsultation.ModifyOn = DateTime.Now;
                            yaeherConsultation.Completetime = DateTime.Now;
                            yaeherConsultation.ConsultState = "success";
                            await _consultationService.UpdateYaeherConsultation(yaeherConsultation);

                            #region 当系统完成  系统退单Job 并关闭预警job 完成job 追问job
                            //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                            //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == UpdateHangFireJob.BusinessCode && a.JobSates != "Close" && (a.JobCode == "WarningConsultation" || a.JobCode == "RemindInquiry" || a.JobCode == "ReturnConsultation")).ToList();

                            HangFireJobIn hangFireJobIn = new HangFireJobIn();
                            hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == UpdateHangFireJob.BusinessCode && a.JobSates != "Close" && (a.JobCode == "WarningConsultation" || a.JobCode == "RemindInquiry" || a.JobCode == "ReturnConsultation"));
                            var hangFireJobs = await _hangFireJobService.HangFireJobListAsync(hangFireJobIn);

                            HangfireScheduleJob job = new HangfireScheduleJob();
                            if (hangFireJobs.Count > 0)
                            {
                                foreach (var CancelhangFire in hangFireJobs)
                                {
                                    CancelhangFire.JobSates = "Close";
                                    CancelhangFire.ModifyOn = DateTime.Now;
                                    CancelhangFire.IsDelete = true;
                                    var Canel = await _hangFireJobService.UpdateHangFireJob(CancelhangFire);

                                    //使用hangfire更新已有job

                                    job.DeleteSchedule(CancelhangFire.JobRunID);
                                }
                            }
                            #endregion
                            #region 完成之后 新增评价提醒job
                            HangFireJob EvaluationFireJob = new HangFireJob();
                            EvaluationFireJob.JobName = "咨询评价";
                            EvaluationFireJob.JobCode = "RemindEvaluation";
                            EvaluationFireJob.BusinessID = yaeherConsultation.Id;
                            EvaluationFireJob.BusinessCode = yaeherConsultation.ConsultNumber;
                            EvaluationFireJob.JobRunTime = UpdateHangFireJob.JobRunTime.AddHours(double.Parse(EvaluationTime.ItemValue));
                            EvaluationFireJob.JobSates = "Open";
                            EvaluationFireJob.CallbackUrl = Commons.AdminIp + "api/DoHangFire/";
                            EvaluationFireJob.JobParameter = JsonHelper.ToJson(EvaluationFireJob);
                            var EvaluationConsult = await _hangFireJobService.CreateHangFireJob(EvaluationFireJob);
                            JobModel model = new JobModel();
                            model.CallbackUrl = EvaluationFireJob.CallbackUrl;//回调URL
                            model.CallbackContent = JsonHelper.ToJson(EvaluationFireJob);//回调参数

                            model.Timespan = EvaluationFireJob.JobRunTime;//运行时间
                            var jobid = job.Schedule(model);
                            if (jobid.IndexOf("error") < 0)
                            {
                                EvaluationConsult.JobRunID = jobid;
                                await _hangFireJobService.UpdateHangFireJob(EvaluationConsult);
                            }
                            #endregion
                            #region 订单完成提醒 关闭
                            //SendMessageInfo sendMessageInfo = new SendMessageInfo();
                            //sendMessageInfo.TemplateCode = "ConsultationComplete";
                            //sendMessageInfo.OperationType = "ConsultationComplete";
                            //sendMessageInfo.ConsultNumber = hangFireJobInfo.BusinessCode;
                            //sendMessageInfo.MessageRemark = "咨询订单已完成，请查看！";
                            //await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                            #endregion

                            consultation.yaeherConsultation = yaeherConsultation;
                            Publishs Consultationpublishs = new Publishs()
                            {
                                Publisher = "System",
                                PublishUrl = "System",
                                EventName = "系统" + hangFireJobInfo.JobName,
                                EventCode = hangFireJobInfo.JobCode,
                                BusinessID = result.Id.ToString(),
                                BusinessCode = yaeherConsultation.ConsultNumber,
                                BusinessJSON = JsonHelper.ToJson(consultation),
                                PublishedTime = result.CreatedOn,
                                PublishStatus = true,
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                                Secret = secret,
                            };
                            var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                            var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                            this.ObjectResultModule.Object = result;
                            unitOfWork.Complete();

                        }
                    }
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                // 追问倒计时提醒
                else if (hangFireJobInfo.JobCode == "RemindInquiry")
                {
                    #region 追问倒计时提醒 咨询者接受消息 PatientReply RemindInquiry
                    SendMessageInfo sendMessageInfo = new SendMessageInfo();
                    var RemindInquiry = new SystemParameterIn() { SystemType = "RemindInquiry" };
                    var RemindInquiryLsit = await _systemParameterService.ParameterList(RemindInquiry);
                    if (RemindInquiryLsit != null)
                    {
                        SystemParameter systemParameter = RemindInquiryLsit.FirstOrDefault();
                        sendMessageInfo.Inquiry = systemParameter.ItemValue + "小时";
                    }
                    else
                    {
                        sendMessageInfo.Inquiry = null;
                    }
                    sendMessageInfo.TemplateCode = "PatientReply";
                    sendMessageInfo.OperationType = "RemindInquiry";
                    sendMessageInfo.ConsultNumber = hangFireJobInfo.BusinessCode;

                    await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                    #endregion
                }
                // 评价提醒
                else if (hangFireJobInfo.JobCode == "RemindEvaluation")
                {
                    #region 评价提醒 咨询者接受消息 PatientEvaluate RemindEvaluation
                    SendMessageInfo sendMessageInfo = new SendMessageInfo();
                    sendMessageInfo.TemplateCode = "PatientEvaluate";
                    sendMessageInfo.OperationType = "RemindEvaluation";
                    sendMessageInfo.ConsultNumber = hangFireJobInfo.BusinessCode;
                    await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                    #endregion
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 404;
                    this.ObjectResultModule.Message = "NotFound";
                }
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTotalTest")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTotalTest(HangFireJob hangFireJobInfo)
        {
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            // DateTime runtime = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyy-MM-dd")).AddSeconds(1);
            HangFireJob ReturnhangFireJob = new HangFireJob();
            ReturnhangFireJob.JobName = "咨询订单统计";
            ReturnhangFireJob.JobCode = "OrderTotal";
            //  ReturnhangFireJob.BusinessID = ConsultationInfo.Id;
            //  ReturnhangFireJob.BusinessCode = ConsultationInfo.ConsultNumber;
            //  ReturnhangFireJob.JobRunTime = ConsultationInfo.Overtime;
            //  ReturnhangFireJob.JobRunTime = runtime;
            ReturnhangFireJob.JobRunTime = Convert.ToDateTime("2019-01-02T00:00:01");
            ReturnhangFireJob.JobSates = "Open";
            ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/OrderTotal/";
            ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
            var ReturnConsult = await _hangFireJobService.CreateHangFireJob(ReturnhangFireJob);

            #region  增加系统退单延迟job
            ////  参数1  ReturnhangFireJob.CallbackUrl 回调URL
            //// 参数2  ReturnhangFireJob.JobParameter  回调参数
            ////  参数3  ReturnhangFireJob.JobRunTime  运行时间
            #endregion

            HangfireScheduleJob job = new HangfireScheduleJob();
            JobModel model = new JobModel();
            model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
            model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
            model.queues = "totalqueue";
            model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间

            var returnjobid = job.Schedule(model);
            // var jobid1 = BackgroundJob.Schedule(() => PostJobResponse(ReturnhangFireJob.CallbackUrl, ReturnhangFireJob.JobParameter), ReturnhangFireJob.JobRunTime);
            //  var jobid1 = BackgroundJob.Schedule(() => PostJobResponse(ReturnhangFireJob.CallbackUrl, JsonHelper.ToJson(ReturnConsult)), ReturnhangFireJob.JobRunTime);
            if (returnjobid.IndexOf("error") < 0)
            {
                ReturnConsult.JobRunID = returnjobid;
                await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
            }
            return new ObjectResultModule(ReturnConsult, 400, "");
        }
        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/EvaluationTotalTest")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> EvaluationTotalTest(HangFireJob hangFireJobInfo)
        {
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            HangFireJob ReturnhangFireJob = new HangFireJob();
            ReturnhangFireJob.JobName = "评分统计";
            ReturnhangFireJob.JobCode = "EvaluationTotal";
            //  ReturnhangFireJob.BusinessID = ConsultationInfo.Id;
            //   ReturnhangFireJob.BusinessCode = ConsultationInfo.ConsultNumber;
            //ReturnhangFireJob.JobRunTime = ConsultationInfo.Overtime;
            ReturnhangFireJob.JobRunTime = DateTime.Now.AddMinutes(3);
            ReturnhangFireJob.JobSates = "Open";
            ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/EvaluationTotal/";
            ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
            var ReturnConsult = await _hangFireJobService.CreateHangFireJob(ReturnhangFireJob);

            //#region  增加系统退单延迟job

            ////参数1  ReturnhangFireJob.CallbackUrl 回调URL
            ////参数2  ReturnhangFireJob.JobParameter  回调参数
            //// 参数3  ReturnhangFireJob.JobRunTime  运行时间
            //#endregion
            HangfireScheduleJob job = new HangfireScheduleJob();
            JobModel model = new JobModel();
            model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
            model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
            model.queues = "totalqueue";
            model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间

            var returnjobid = job.Schedule(model);
            // var jobid1 = BackgroundJob.Schedule(() => PostJobResponse(ReturnhangFireJob.CallbackUrl, ReturnhangFireJob.JobParameter), ReturnhangFireJob.JobRunTime);
            //  var jobid1 = BackgroundJob.Schedule(() => PostJobResponse(ReturnhangFireJob.CallbackUrl, JsonHelper.ToJson(ReturnConsult)), ReturnhangFireJob.JobRunTime);
            if (returnjobid.IndexOf("error") < 0)
            {
                ReturnConsult.JobRunID = returnjobid;
                await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
            }
            return new ObjectResultModule(ReturnConsult, 400, "");
        }
        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTotal(HangFireJob hangFireJobInfo)
        {
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateHangFireJob = await _hangFireJobService.HangFireJobByID(hangFireJobInfo.Id);
            if (UpdateHangFireJob != null)
            {
                //UpdateHangFireJob.JobRunID = hangFireJobInfo.JobRunID;  // 回写ID
                if (hangFireJobInfo.JobSates == "Open")//新增状态过来则open状态转换成完成
                {
                    UpdateHangFireJob.JobSates = "Complete"; // 执行状态
                }
                else
                {
                    UpdateHangFireJob.JobSates = hangFireJobInfo.JobSates; // 执行状态
                }
                UpdateHangFireJob.ModifyOn = DateTime.Now;
                UpdateHangFireJob.ModifyBy = userid;
                OrderTotalIn orderTotalIn = new OrderTotalIn();
                // orderTotalIn.StartTime = DateTime.Parse("2019-01-6");
                //  orderTotalIn.EndTime = DateTime.Parse("2019-01-07");
                orderTotalIn.StartTime = DateTime.Parse(UpdateHangFireJob.JobRunTime.AddDays(-1).ToString("yyyy-MM-dd"));
                orderTotalIn.EndTime = DateTime.Parse(UpdateHangFireJob.JobRunTime.ToString("yyyy-MM-dd"));
                // 计算医生当天咨询订单统计
                var ConsultationTotal = await _consultationOrderTotalService.OrderTotal(orderTotalIn);
                if (ConsultationTotal == "success")
                {
                    var result = await _hangFireJobService.UpdateHangFireJob(UpdateHangFireJob);
                    #region 当操作完成后 将当前callback事件再次提交   
                    DateTime runtime = Convert.ToDateTime(UpdateHangFireJob.JobRunTime.AddDays(1).ToString("yyy-MM-dd")).AddSeconds(1);
                    HangFireJob ReturnhangFireJob = new HangFireJob();
                    ReturnhangFireJob.JobName = "咨询订单统计";
                    ReturnhangFireJob.JobCode = "OrderTotal";
                    ReturnhangFireJob.JobRunTime = runtime;
                    ReturnhangFireJob.JobSates = "Open";
                    ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/OrderTotal/";
                    ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
                    var ReturnConsult = await _hangFireJobService.CreateHangFireJob(ReturnhangFireJob);
                    HangfireScheduleJob job = new HangfireScheduleJob();
                    JobModel model = new JobModel();
                    model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
                    model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
                    model.queues = "totalqueue";
                    model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间
                    var returnjobid = job.Schedule(model);
                    if (returnjobid.IndexOf("error") < 0)
                    {
                        ReturnConsult.JobRunID = returnjobid;
                        await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
                    }
                    if (!string.IsNullOrEmpty(UpdateHangFireJob.JobRunID))
                    {
                        job.DeleteSchedule(UpdateHangFireJob.JobRunID);
                    }
                    #endregion

                    this.ObjectResultModule.Object = result;
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 404;
                    this.ObjectResultModule.Message = "NotFound";
                }
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 评分统计  
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/EvaluationTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> EvaluationTotal(HangFireJob hangFireJobInfo)
        {
            //Logger.Info("hangFireJobInfo+" + JsonHelper.ToJson(hangFireJobInfo.BusinessCode));
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateHangFireJob = await _hangFireJobService.HangFireJobByID(hangFireJobInfo.Id);
            if (UpdateHangFireJob != null)
            {

                if (hangFireJobInfo.JobSates == "Open")//新增状态过来则open状态转换成完成
                {
                    UpdateHangFireJob.JobSates = "Complete"; // 执行状态
                }
                else
                {
                    UpdateHangFireJob.JobSates = hangFireJobInfo.JobSates; // 执行状态
                }
                UpdateHangFireJob.ModifyOn = DateTime.Now;
                UpdateHangFireJob.ModifyBy = userid;
                // 统计当前所有医生的咨询评分
                var EvaluationAll = await _evaluationTotalService.EvaluationTotalAll();
                if (EvaluationAll == "success")
                {
                    var result = await _hangFireJobService.UpdateHangFireJob(UpdateHangFireJob);

                    #region 再次提交callback事件 
                    DateTime runtime = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyy-MM-dd")).AddSeconds(2);
                    HangFireJob ReturnhangFireJob = new HangFireJob();
                    ReturnhangFireJob.JobName = "评分统计";
                    ReturnhangFireJob.JobCode = "EvaluationTotal";
                    ReturnhangFireJob.JobRunTime = runtime;
                    ReturnhangFireJob.JobSates = "Open";
                    ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/EvaluationTotal/";
                    ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
                    var ReturnConsult = await _hangFireJobService.CreateHangFireJob(ReturnhangFireJob);
                    HangfireScheduleJob job = new HangfireScheduleJob();
                    JobModel model = new JobModel();
                    model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
                    model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
                    model.queues = "totalqueue";
                    model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间

                    var returnjobid = job.Schedule(model);
                    if (returnjobid.IndexOf("error") < 0)
                    {
                        ReturnConsult.JobRunID = returnjobid;
                        await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
                    }
                    if (!string.IsNullOrEmpty(UpdateHangFireJob.JobRunID))
                    {
                        job.DeleteSchedule(UpdateHangFireJob.JobRunID);
                    }
                    #endregion

                    this.ObjectResultModule.Object = result;
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 404;
                    this.ObjectResultModule.Message = "NotFound";
                }
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 预警提醒
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/WarningConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> WarningConsultation(HangFireJob hangFireJobInfo)
        {
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //Logger.Info("hangFireJobInfo" + JsonHelper.ToJson(hangFireJobInfo.BusinessCode));
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateHangFireJob = await _hangFireJobService.HangFireJobByID(hangFireJobInfo.Id);
            if (UpdateHangFireJob != null)
            {
                if (hangFireJobInfo.JobSates == "Open")
                {
                    UpdateHangFireJob.JobSates = "Complete";
                }
                else
                {
                    UpdateHangFireJob.JobSates = hangFireJobInfo.JobSates;  // 执行状态
                }
                UpdateHangFireJob.ModifyOn = DateTime.Now;
                UpdateHangFireJob.ModifyBy = userid;
                #region 预警时间
                var yaeherConsultation = await _consultationService.YaeherConsultationByNumber(hangFireJobInfo.BusinessCode);
                TimeSpan ts = UpdateHangFireJob.JobRunTime - yaeherConsultation.CreatedOn;
                string msg = "";
                if (ts.Days > 0)
                {
                    msg = ts.Days + "天";
                }
                else if (ts.Hours > 0)
                {
                    msg = ts.Hours + "小时";
                }
                else if (ts.Minutes > 0)
                {
                    msg = ts.Minutes + "分钟";
                }
                #endregion
                #region 微信提醒医生预警通知  提醒医生 DoctorTimeOut WarningRemind
                SendMessageInfo sendMessageInfo = new SendMessageInfo();
                sendMessageInfo.TemplateCode = "DoctorTimeOut";
                sendMessageInfo.OperationType = "WarningRemind";
                sendMessageInfo.ConsultNumber = hangFireJobInfo.BusinessCode;
                sendMessageInfo.WarningTime = msg;
                await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                #endregion

                var result = await _hangFireJobService.UpdateHangFireJob(UpdateHangFireJob);
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 结算统计
        /// </summary>
        /// <param name="hangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeTotalTest")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeTotalTest(HangFireJobIn hangFireJobInfo)
        {
            if (!Commons.CheckSecret(hangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            #region 测试分账
            //SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
            //systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
            //var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
            //var tencentparam = configs.FirstOrDefault();

            //TencentWXPay tencentWXPay = new TencentWXPay();
            //List<receivershare> receivers = new List<receivershare>();
            //var receivershare = new receivershare();
            //receivershare.name = "周俊";
            //receivershare.type = "PERSONAL_OPENID";
            //receivershare.account ="oOiwG1pMMnair4qvjO8AtCnkN0kM";
            //receivershare.amount = Convert.ToInt32(80);
            //receivers.Add(receivershare);
            //var shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, "4200000231201812153264431851","20181215221326633662138");
            //var Sahring = shareresult.result_code;
            #endregion
            //var doctor = await _yaeherDoctorService.YaeherDoctorByID(6);
            ////  doctor.DoctorName = "周俊";
            //var user = await _yaeherUserService.YaeherUserByID(doctor.UserID);


            //TencentWXPay tencentWXPay = new TencentWXPay();

            //var receiver = new receiver();
            //receiver.name = "周俊";
            //receiver.type = "PERSONAL_OPENID";
            //receiver.account = "oOiwG1pMMnair4qvjO8AtCnkN0kM";
            //var addresult = await tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam);

            //var jobruntime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddSeconds(-1);
            //  var jobruntime =Convert.ToDateTime(hangFireJobInfo.EndTime);
            var param = new SystemParameterIn() { SystemType = "ProfitSharingTime" };
            var SharingTimeList = await _systemParameterService.ParameterList(param);
            var SharingTime = SharingTimeList.FirstOrDefault();

            //  DateTime runtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-1);
            //  var runtime = Convert.ToDateTime(hangFireJobInfo.StartTime);
            // 统计当前所有收入
            //  var IncomeAll = await _incomeDetailsService.IncomeTotalTestServerAll(runtime, jobruntime,"111");

            DateTime nestruntime = Convert.ToDateTime("2019-02-20T14:00:01");
            HangFireJob ReturnhangFireJob = new HangFireJob();
            ReturnhangFireJob.JobName = "咨询分账统计";
            ReturnhangFireJob.JobCode = "IncomeTotal";
            //  ReturnhangFireJob.BusinessID = ConsultationInfo.Id;
            //   ReturnhangFireJob.BusinessCode = ConsultationInfo.ConsultNumber;
            //ReturnhangFireJob.JobRunTime = ConsultationInfo.Overtime;
            ReturnhangFireJob.JobRunTime = nestruntime;
            ReturnhangFireJob.JobSates = "Open";
            ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/IncomeTotal/";
            ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
            var ReturnConsult = await _hangFireJobService.CreateHangFireJob(ReturnhangFireJob);

            //#region  增加系统退单延迟job

            ////参数1  ReturnhangFireJob.CallbackUrl 回调URL
            ////参数2  ReturnhangFireJob.JobParameter  回调参数
            //// 参数3  ReturnhangFireJob.JobRunTime  运行时间
            //#endregion
            HangfireScheduleJob job = new HangfireScheduleJob();
            JobModel model = new JobModel();
            model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
            model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
            model.queues = "totalqueue";
            model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间

            var returnjobid = job.Schedule(model);
            if (returnjobid.IndexOf("error") < 0)
            {
                ReturnConsult.JobRunID = returnjobid;
                await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
            }

            return ObjectResultModule;
        }

    }
}