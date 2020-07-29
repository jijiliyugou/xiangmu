using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.EventHelper;
using Yaeher.Common.HangfireJob;
using Yaeher.Common.HttpHelpers;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.Controllers;
using Yaeher.EventBus;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;
using Yaeher.HangFire;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;

namespace YaeherDoctorAPI.Web.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HangfirejobController : YaeherControllerBase
    {
        private readonly IPublishsService _publishsService;
        private readonly IReceiveEventService _receiveEventService;
        private readonly ISubscribeService _subscribeService;
        private readonly ISubscribetionService _subscribetionService;
        private readonly IAbpSession _IabpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IHangFireJobService _hangFireJobService;
        private readonly ISystemParameterService _systemParameterService;

        private readonly IConsultationService _consultationService;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IRefundManageService _refundManageService;
        private readonly IConsultationEvaluationService _consultationEvaluationService;
        private readonly IConsultationReplyService _consultationReplyService;
        private readonly IPhoneReplyRecordService _phoneReplyRecordService;
        private readonly IAttachmentServices _attachmentServices;
        private readonly IWecharSendMessageService _wecharSendMessageService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishsService"></param>
        /// <param name="receiveEventService"></param>
        /// <param name="subscribeService"></param>
        /// <param name="subscribetionService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="consultationService"></param>
        /// <param name="orderManageService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="refundManageService"></param>
        /// <param name="consultationEvaluationService"></param>
        /// <param name="consultationReplyService"></param>
        /// <param name="phoneReplyRecordService"></param>
        /// <param name="hangFireJobService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="abpSession"></param>
        /// <param name="attachmentServices"></param>
        /// <param name="wecharSendMessageService"></param>
        public HangfirejobController(IPublishsService publishsService,
                               IReceiveEventService receiveEventService,
                               ISubscribeService subscribeService,
                               ISubscribetionService subscribetionService,
                               IUnitOfWorkManager unitOfWorkManager,
                               IConsultationService consultationService,
                               IOrderManageService orderManageService,
                               IOrderTradeRecordService orderTradeRecordService,
                               IRefundManageService refundManageService,
                               IConsultationEvaluationService consultationEvaluationService,
                               IConsultationReplyService consultationReplyService,
                               IPhoneReplyRecordService phoneReplyRecordService,
                               IHangFireJobService hangFireJobService,
                               ISystemParameterService systemParameterService,
                               IAbpSession abpSession,
                               IAttachmentServices attachmentServices,
                               IWecharSendMessageService wecharSendMessageService
            )
        {
            _publishsService = publishsService;
            _receiveEventService = receiveEventService;
            _subscribeService = subscribeService;
            _subscribetionService = subscribetionService;
            _IabpSession = abpSession;
            _unitOfWorkManager = unitOfWorkManager;
            _consultationService = consultationService;
            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _refundManageService = refundManageService;
            _consultationEvaluationService = consultationEvaluationService;
            _consultationReplyService = consultationReplyService;
            _phoneReplyRecordService = phoneReplyRecordService;
            _hangFireJobService = hangFireJobService;
            _systemParameterService = systemParameterService;
            _attachmentServices = attachmentServices;
            _wecharSendMessageService = wecharSendMessageService;
            //_yaeherOperListService = yaeherOperListService;
        }
        /// <summary>
        /// 咨询管理
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationManage([FromBody] Publishs PublishsInfo)
        {
            //Logger.Info("api/ConsultationManage:"+JsonHelper.ToJson(PublishsInfo));
            if (!Commons.CheckSecret(PublishsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (PublishsInfo != null)
            {
                try
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        Consultation consultation = JsonHelper.FromJson<Consultation>(PublishsInfo.BusinessJSON);
                        YaeherConsultation ConsultationInfo = new YaeherConsultation();
                        //Logger.Info("咨询开始");
                        #region 咨询主表
                        if (consultation.yaeherConsultation != null)
                        {
                            ConsultationIn consultationIn = new ConsultationIn();
                            consultationIn.AndAlso(a => a.ConsultNumber == consultation.yaeherConsultation.ConsultNumber);
                            var Consultation = await _consultationService.YaeherConsultationList(consultationIn);
                            if (Consultation.Count > 0)
                            {
                                #region 咨询主表
                                YaeherConsultation yaeherConsultation = Consultation.FirstOrDefault();
                                yaeherConsultation.CreatedBy = consultation.yaeherConsultation.CreatedBy;
                                yaeherConsultation.CreatedOn = consultation.yaeherConsultation.CreatedOn;
                                yaeherConsultation.ModifyBy = consultation.yaeherConsultation.ModifyBy;
                                yaeherConsultation.ModifyOn = consultation.yaeherConsultation.ModifyOn;
                                yaeherConsultation.IsDelete = consultation.yaeherConsultation.IsDelete;
                                yaeherConsultation.DeleteBy = consultation.yaeherConsultation.DeleteBy;
                                yaeherConsultation.DeleteTime = consultation.yaeherConsultation.DeleteTime;
                                yaeherConsultation.ConsultNumber = consultation.yaeherConsultation.ConsultNumber;
                                yaeherConsultation.ConsultantID = consultation.yaeherConsultation.ConsultantID;
                                yaeherConsultation.ConsultantName = consultation.yaeherConsultation.ConsultantName;
                                yaeherConsultation.ConsultantJSON = consultation.yaeherConsultation.ConsultantJSON;
                                yaeherConsultation.DoctorName = consultation.yaeherConsultation.DoctorName;
                                yaeherConsultation.DoctorID = consultation.yaeherConsultation.DoctorID;
                                yaeherConsultation.DoctorJSON = consultation.yaeherConsultation.DoctorJSON;
                                yaeherConsultation.PatientID = consultation.yaeherConsultation.PatientID;
                                yaeherConsultation.PatientName = consultation.yaeherConsultation.PatientName;
                                yaeherConsultation.PatientJSON = consultation.yaeherConsultation.PatientJSON;
                                yaeherConsultation.ConsultType = consultation.yaeherConsultation.ConsultType;
                                yaeherConsultation.IIInessType = consultation.yaeherConsultation.IIInessType;
                                yaeherConsultation.IIInessJSON = consultation.yaeherConsultation.IIInessJSON;
                                yaeherConsultation.PhoneNumber = consultation.yaeherConsultation.PhoneNumber;
                                yaeherConsultation.PatientCity = consultation.yaeherConsultation.PatientCity;
                                yaeherConsultation.IIInessDescription = consultation.yaeherConsultation.IIInessDescription;
                                yaeherConsultation.InquiryTimes = consultation.yaeherConsultation.InquiryTimes;
                                yaeherConsultation.ConsultState = consultation.yaeherConsultation.ConsultState;
                                yaeherConsultation.OvertimeRemindTimes = consultation.yaeherConsultation.OvertimeRemindTimes;
                                yaeherConsultation.Overtime = consultation.yaeherConsultation.Overtime;
                                yaeherConsultation.RefundBy = consultation.yaeherConsultation.RefundBy;
                                yaeherConsultation.RefundTime = consultation.yaeherConsultation.RefundTime;
                                yaeherConsultation.RefundType = consultation.yaeherConsultation.RefundType;
                                yaeherConsultation.RefundNumber = consultation.yaeherConsultation.RefundNumber;
                                yaeherConsultation.RefundState = consultation.yaeherConsultation.RefundState;
                                yaeherConsultation.RefundReason = consultation.yaeherConsultation.RefundReason;
                                yaeherConsultation.RefundRemarks = consultation.yaeherConsultation.RefundRemarks;
                                yaeherConsultation.RecommendDoctorID = consultation.yaeherConsultation.RecommendDoctorID;
                                yaeherConsultation.RecommendDoctorName = consultation.yaeherConsultation.RecommendDoctorName;
                                yaeherConsultation.Age = consultation.yaeherConsultation.Age;
                                yaeherConsultation.HasReply = consultation.yaeherConsultation.HasReply;
                                yaeherConsultation.ServiceMoneyListId = consultation.yaeherConsultation.ServiceMoneyListId;
                                yaeherConsultation.HasInquiryTimes = consultation.yaeherConsultation.HasInquiryTimes;
                                yaeherConsultation.IsEvaluate = consultation.yaeherConsultation.IsEvaluate;
                                yaeherConsultation.IsReturnVisit = consultation.yaeherConsultation.IsReturnVisit;
                                yaeherConsultation.ReturnVisit = consultation.yaeherConsultation.ReturnVisit;
                                yaeherConsultation.ReturnVisitTime = consultation.yaeherConsultation.ReturnVisitTime;
                                yaeherConsultation.Completetime = consultation.yaeherConsultation.Completetime;
                                #endregion
                                ConsultationInfo = await _consultationService.UpdateYaeherConsultation(yaeherConsultation);

                                #region 当咨询退单时 需要将所有job都关闭
                                if (yaeherConsultation.ConsultState == "return")
                                {
                                    //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                                    //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == ConsultationInfo.ConsultNumber&&a.JobSates== "Open").ToList();

                                    HangFireJobIn hangFireJobIn = new HangFireJobIn();
                                    hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == ConsultationInfo.ConsultNumber&&a.JobSates== "Open");
                                    var hangFireJobs = await _hangFireJobService.HangFireJobListAsync(hangFireJobIn);

                                    int CanelCount = 0;
                                    if (hangFireJobs.Count > 0)
                                    {
                                        foreach (var CancelhangFire in hangFireJobs)
                                        {
                                            CancelhangFire.JobSates = "Close";
                                            CancelhangFire.ModifyOn = DateTime.Now;
                                            CancelhangFire.IsDelete = true;
                                            var Canel = await _hangFireJobService.UpdateHangFireJob(CancelhangFire);
                                            CanelCount += 1;
                                            //使用hangfire更新已有job
                                            HangfireScheduleJob job = new HangfireScheduleJob();
                                            job.DeleteSchedule(CancelhangFire.JobRunID);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                consultation.yaeherConsultation.Id = 0;
                                consultation.yaeherConsultation.CreatedOn = DateTime.Now;
                                ConsultationInfo = await _consultationService.CreateYaeherConsultation(consultation.yaeherConsultation);
                                #region 当订单新增时系统增加 系统退单job 以及医生预警Job
                                HangFireJob ReturnhangFireJob = new HangFireJob();
                                ReturnhangFireJob.JobName = "咨询退单";
                                ReturnhangFireJob.JobCode = "ReturnConsultation";
                                ReturnhangFireJob.BusinessID = ConsultationInfo.Id;
                                ReturnhangFireJob.BusinessCode = ConsultationInfo.ConsultNumber;
                                ReturnhangFireJob.JobRunTime = ConsultationInfo.Overtime;
                                ReturnhangFireJob.JobSates = "Open";
                                ReturnhangFireJob.CallbackUrl = Commons.AdminIp + "api/DoHangFire/";
                                ReturnhangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
                                var ReturnConsult = await _hangFireJobService.CreateHangFireJob(ReturnhangFireJob);

                                #region  增加系统退单延迟job

                                //参数1  ReturnhangFireJob.CallbackUrl 回调URL
                                //参数2  ReturnhangFireJob.JobParameter  回调参数
                                // 参数3  ReturnhangFireJob.JobRunTime  运行时间
                                #endregion
                                HangfireScheduleJob job = new HangfireScheduleJob();
                                JobModel model = new JobModel();
                                model.CallbackUrl = ReturnhangFireJob.CallbackUrl;//回调URL
                                model.queues = "doctorqueue";
                                model.CallbackContent = JsonHelper.ToJson(ReturnhangFireJob);//回调参数
                                model.Timespan = ReturnhangFireJob.JobRunTime;//运行时间

                                var returnjobid = job.Schedule(model);
                                // var jobid1 = BackgroundJob.Schedule(() => PostJobResponse(ReturnhangFireJob.CallbackUrl, ReturnhangFireJob.JobParameter), ReturnhangFireJob.JobRunTime);
                                //  var jobid1 = BackgroundJob.Schedule(() => PostJobResponse(ReturnhangFireJob.CallbackUrl, JsonHelper.ToJson(ReturnConsult)), ReturnhangFireJob.JobRunTime);
                                if (returnjobid.IndexOf("error") < 0)
                                {
                                    ReturnConsult.JobRunID = returnjobid;
                                    await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
                                }
                                var param = new SystemParameterIn() { Type = "ConfigPar" };
                                param.AndAlso(t => !t.IsDelete && t.SystemCode == "WarningConsultation");
                                var WarningTimeList = await _systemParameterService.ParameterList(param);
                                if (WarningTimeList != null)
                                {
                                    foreach (var Warning in WarningTimeList)
                                    {
                                        HangFireJob WarninghangFireJob = new HangFireJob();
                                        WarninghangFireJob.JobName = "咨询预警";
                                        WarninghangFireJob.JobCode = "WarningConsultation";
                                        WarninghangFireJob.BusinessID = ConsultationInfo.Id;
                                        WarninghangFireJob.BusinessCode = ConsultationInfo.ConsultNumber;
                                        //   WarninghangFireJob.JobRunTime = ConsultationInfo.CreatedOn.AddHours(int.Parse(Warning.ItemValue));
                                        WarninghangFireJob.JobRunTime = ConsultationInfo.CreatedOn.AddHours(double.Parse(Warning.ItemValue));

                                        WarninghangFireJob.JobSates = "Open";
                                        WarninghangFireJob.CallbackUrl = Commons.AdminIp + "api/WarningConsultation/";
                                        WarninghangFireJob.JobParameter = JsonHelper.ToJson(ReturnhangFireJob);
                                        var WarninghangConsult = await _hangFireJobService.CreateHangFireJob(WarninghangFireJob);

                                        #region  增加系统退单延迟job
                                        //参数1  WarninghangFireJob.CallbackUrl 回调URL
                                        //参数2  WarninghangFireJob.JobParameter  回调参数
                                        // 参数3  WarninghangFireJob.JobRunTime  运行时间
                                        #endregion
                                        model.CallbackUrl = WarninghangFireJob.CallbackUrl;//回调URL

                                        model.CallbackContent = JsonHelper.ToJson(WarninghangConsult);//回调参数
                                        model.Timespan = WarninghangFireJob.JobRunTime;//运行时间
                                        var Warningjobid = job.Schedule(model);

                                        //   var jobid = BackgroundJob.Schedule(() => PostJobResponse(WarninghangFireJob.CallbackUrl, JsonHelper.ToJson(WarninghangConsult)), WarninghangFireJob.JobRunTime);

                                        if (returnjobid.IndexOf("error") < 0)
                                        {
                                            WarninghangConsult.JobRunID = Warningjobid;
                                            await _hangFireJobService.UpdateHangFireJob(WarninghangConsult);
                                        }
                                    }
                                }
                                #endregion

                            }
                        }
                        #endregion
                        //Logger.Info("咨询主表结束");
                        //Logger.Info("订单开始");
                        #region 订单表
                        OrderManage orderManageInfo = new OrderManage();
                        if (consultation.orderManage != null)
                        {
                            OrderManageIn orderManageIn = new OrderManageIn();
                            orderManageIn.AndAlso(a => a.SequenceNo == consultation.orderManage.SequenceNo);
                            var orderManages = await _orderManageService.OrderManageList(orderManageIn);
                            if (orderManages.Count > 0)
                            {
                                #region 订单表
                                OrderManage orderManage = orderManages.FirstOrDefault();
                                orderManage.CreatedBy = consultation.orderManage.CreatedBy;
                                orderManage.CreatedOn = consultation.orderManage.CreatedOn;
                                orderManage.ModifyBy = consultation.orderManage.ModifyBy;
                                orderManage.ModifyOn = consultation.orderManage.ModifyOn;
                                orderManage.IsDelete = consultation.orderManage.IsDelete;
                                orderManage.DeleteBy = consultation.orderManage.DeleteBy;
                                orderManage.DeleteTime = consultation.orderManage.DeleteTime;
                                orderManage.OrderNumber = consultation.orderManage.OrderNumber;
                                orderManage.ConsultNumber = consultation.orderManage.ConsultNumber;
                                orderManage.ConsultID = orderManages.FirstOrDefault().ConsultID;
                                orderManage.ConsultType = consultation.orderManage.ConsultType;
                                orderManage.ConsultantID = consultation.orderManage.ConsultantID;
                                orderManage.ConsultantName = consultation.orderManage.ConsultantName;
                                orderManage.PatientID = consultation.orderManage.PatientID;
                                orderManage.PatientName = consultation.orderManage.PatientName;
                                orderManage.DoctorName = consultation.orderManage.DoctorName;
                                orderManage.DoctorID = consultation.orderManage.DoctorID;
                                orderManage.OrderCurrency = consultation.orderManage.OrderCurrency;
                                orderManage.OrderMoney = consultation.orderManage.OrderMoney;
                                orderManage.ReceivablesType = consultation.orderManage.ReceivablesType;
                                orderManage.ReceivablesNumber = consultation.orderManage.ReceivablesNumber;
                                orderManage.ServiceID = consultation.orderManage.ServiceID;
                                orderManage.ServiceName = consultation.orderManage.ServiceName;
                                orderManage.SellerMoneyID = consultation.orderManage.SellerMoneyID;
                                orderManage.TradeType = consultation.orderManage.TradeType;
                                orderManage.SequenceNo = consultation.orderManage.SequenceNo;

                                orderManage.Id = orderManages.FirstOrDefault().Id;
                                #endregion
                                var result = await _orderManageService.UpdateOrderManage(orderManage);
                            }
                            else
                            {
                                consultation.orderManage.ConsultID = ConsultationInfo.Id;
                                consultation.orderManage.Id = 0;
                                orderManageInfo = await _orderManageService.CreateOrderManage(consultation.orderManage);
                            }
                        }
                        #endregion
                        //Logger.Info("订单结束");
                        //Logger.Info("退单开始");
                        #region 退单表
                        RefundManage refundManageInfo = new RefundManage();
                        if (consultation.refundManage != null)
                        {
                            RefundManageIn refundManageIn = new RefundManageIn();
                            refundManageIn.AndAlso(a => a.SequenceNo == consultation.refundManage.SequenceNo);
                            var refundManages = await _refundManageService.RefundManageList(refundManageIn);
                            if (refundManages.Count > 0)
                            {
                                #region 退单表
                                RefundManage refundManage = refundManages.FirstOrDefault();
                                refundManage.CreatedBy = consultation.refundManage.CreatedBy;
                                refundManage.CreatedOn = consultation.refundManage.CreatedOn;
                                refundManage.ModifyBy = consultation.refundManage.ModifyBy;
                                refundManage.ModifyOn = consultation.refundManage.ModifyOn;
                                refundManage.IsDelete = consultation.refundManage.IsDelete;
                                refundManage.DeleteBy = consultation.refundManage.DeleteBy;
                                refundManage.DeleteTime = consultation.refundManage.DeleteTime;
                                refundManage.RefundNumber = consultation.refundManage.RefundNumber;
                                refundManage.ConsultID = refundManages.FirstOrDefault().ConsultID;
                                refundManage.ConsultNumber = consultation.refundManage.ConsultNumber;
                                refundManage.OrderID = consultation.refundManage.OrderID;
                                refundManage.OrderNumber = consultation.refundManage.OrderNumber;
                                refundManage.ConsultantID = consultation.refundManage.ConsultantID;
                                refundManage.ConsultantName = consultation.refundManage.ConsultantName;
                                refundManage.PatientID = consultation.refundManage.PatientID;
                                refundManage.PatientName = consultation.refundManage.PatientName;
                                refundManage.DoctorID = consultation.refundManage.DoctorID;
                                refundManage.DoctorName = consultation.refundManage.DoctorName;
                                refundManage.OrderCurrency = consultation.refundManage.OrderCurrency;
                                refundManage.OrderMoney = consultation.refundManage.OrderMoney;
                                refundManage.ServiceID = consultation.refundManage.ServiceID;
                                refundManage.ServiceName = consultation.refundManage.ServiceName;
                                refundManage.SellerMoneyID = consultation.refundManage.SellerMoneyID;
                                refundManage.CheckRemark = consultation.refundManage.CheckRemark;
                                refundManage.CheckState = consultation.refundManage.CheckState;
                                refundManage.CheckTime = consultation.refundManage.CheckTime;
                                refundManage.Checker = consultation.refundManage.Checker;
                                refundManage.RefundReason = consultation.refundManage.RefundReason;
                                refundManage.RefundRemarks = consultation.refundManage.RefundRemarks;
                                refundManage.SequenceNo = consultation.refundManage.SequenceNo;
                                refundManage.Id = refundManages.FirstOrDefault().Id;
                                #endregion
                                var result = await _refundManageService.UpdateRefundManage(refundManage);
                            }
                            else
                            {
                                //Logger.Info("ConsultationInfo" + JsonHelper.ToJson(ConsultationInfo));
                                consultation.refundManage.ConsultID = ConsultationInfo.Id;
                                consultation.refundManage.Id = 0;
                                refundManageInfo = await _refundManageService.CreateRefundManage(consultation.refundManage);
                            }
                        }
                        #endregion
                        //Logger.Info("退单结束");
                        //Logger.Info("交易开始");
                        #region 交易表
                        if (consultation.orderTradeRecords != null)
                        {
                            OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
                            orderTradeRecordIn.AndAlso(a => a.SequenceNo == consultation.orderTradeRecords.SequenceNo);
                            var orderTradeRecords = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);
                            if (orderTradeRecords.Count > 0)
                            {
                                #region 交易表
                                OrderTradeRecord orderTradeRecord = orderTradeRecords.FirstOrDefault();
                                orderTradeRecord.CreatedBy = consultation.orderTradeRecords.CreatedBy;
                                orderTradeRecord.CreatedOn = consultation.orderTradeRecords.CreatedOn;
                                orderTradeRecord.ModifyBy = consultation.orderTradeRecords.ModifyBy;
                                orderTradeRecord.ModifyOn = consultation.orderTradeRecords.ModifyOn;
                                orderTradeRecord.IsDelete = consultation.orderTradeRecords.IsDelete;
                                orderTradeRecord.DeleteBy = consultation.orderTradeRecords.DeleteBy;
                                orderTradeRecord.DeleteTime = consultation.orderTradeRecords.DeleteTime;
                                orderTradeRecord.OrderID = orderTradeRecords.FirstOrDefault().OrderID;
                                orderTradeRecord.OrderNumber = consultation.orderTradeRecords.OrderNumber;
                                orderTradeRecord.PayType = consultation.orderTradeRecords.PayType;
                                orderTradeRecord.OrderCurrency = consultation.orderTradeRecords.OrderCurrency;
                                orderTradeRecord.TenpayNumber = consultation.orderTradeRecords.TenpayNumber;
                                orderTradeRecord.VoucherNumber = consultation.orderTradeRecords.VoucherNumber;
                                orderTradeRecord.VoucherJSON = consultation.orderTradeRecords.VoucherJSON;
                                orderTradeRecord.PayMoney = consultation.orderTradeRecords.PayMoney;
                                orderTradeRecord.PayAchiveTime = consultation.orderTradeRecords.PayAchiveTime;
                                orderTradeRecord.PaySerialNumber = consultation.orderTradeRecords.PaySerialNumber;
                                orderTradeRecord.PaymentState = consultation.orderTradeRecords.PaymentState;
                                orderTradeRecord.PaymentSource = consultation.orderTradeRecords.PaymentSource;
                                orderTradeRecord.PaymentSourceCode = consultation.orderTradeRecords.PaymentSourceCode;
                                orderTradeRecord.SequenceNo = consultation.orderTradeRecords.SequenceNo;
                                orderTradeRecord.WXPayBillno = consultation.orderTradeRecords.WXPayBillno;
                                orderTradeRecord.WXOrderQuery = consultation.orderTradeRecords.WXOrderQuery;
                                orderTradeRecord.WXTransactionId = consultation.orderTradeRecords.WXTransactionId;
                                orderTradeRecord.Id = orderTradeRecords.FirstOrDefault().Id;
                                #endregion
                                var result = await _orderTradeRecordService.UpdateOrderTradeRecord(orderTradeRecord);
                            }
                            else
                            {
                                consultation.orderTradeRecords.OrderID = orderManageInfo.Id;
                                if (consultation.orderTradeRecords.PaymentSourceCode == "order")
                                { consultation.orderTradeRecords.OrderID = orderManageInfo.Id; }
                                else
                                { consultation.orderTradeRecords.OrderID = refundManageInfo.Id; }
                                consultation.orderTradeRecords.Id = 0;
                                var result = await _orderTradeRecordService.CreateOrderTradeRecord(consultation.orderTradeRecords);
                            }
                        }
                        #endregion
                        //Logger.Info("交易结束");
                        //Logger.Info("评分开始");
                        #region 咨询评分
                        if (consultation.consultationEvaluation != null)
                        {
                            ConsultationEvaluationIn consultationEvaluationIn = new ConsultationEvaluationIn();
                            consultationEvaluationIn.AndAlso(a => a.SequenceNo == consultation.consultationEvaluation.SequenceNo);
                            var consultationEvaluations = await _consultationEvaluationService.ConsultationEvaluationList(consultationEvaluationIn);

                            if (consultationEvaluations.Count > 0)
                            {
                                #region 咨询评分
                                ConsultationEvaluation consultationEvaluation = consultationEvaluations.FirstOrDefault();
                                consultationEvaluation.CreatedBy = consultation.consultationEvaluation.CreatedBy;
                                consultationEvaluation.CreatedOn = consultation.consultationEvaluation.CreatedOn;
                                consultationEvaluation.ModifyBy = consultation.consultationEvaluation.ModifyBy;
                                consultationEvaluation.ModifyOn = consultation.consultationEvaluation.ModifyOn;
                                consultationEvaluation.IsDelete = consultation.consultationEvaluation.IsDelete;
                                consultationEvaluation.DeleteBy = consultation.consultationEvaluation.DeleteBy;
                                consultationEvaluation.DeleteTime = consultation.consultationEvaluation.DeleteTime;
                                consultationEvaluation.ConsultNumber = consultation.consultationEvaluation.ConsultNumber;
                                consultationEvaluation.ConsultID = consultationEvaluations.FirstOrDefault().ConsultID;
                                consultationEvaluation.ConsultantID = consultation.consultationEvaluation.ConsultantID;
                                consultationEvaluation.ConsultantName = consultation.consultationEvaluation.ConsultantName;
                                consultationEvaluation.PatientID = consultation.consultationEvaluation.PatientID;
                                consultationEvaluation.PatientName = consultation.consultationEvaluation.PatientName;
                                consultationEvaluation.DoctorName = consultation.consultationEvaluation.DoctorName;
                                consultationEvaluation.DoctorID = consultation.consultationEvaluation.DoctorID;
                                consultationEvaluation.EvaluateContent = consultation.consultationEvaluation.EvaluateContent;
                                consultationEvaluation.EvaluateReason = consultation.consultationEvaluation.EvaluateReason;
                                consultationEvaluation.EvaluateLevel = consultation.consultationEvaluation.EvaluateLevel;
                                consultationEvaluation.QualityLevel = consultation.consultationEvaluation.QualityLevel;
                                consultationEvaluation.IsQuality = consultation.consultationEvaluation.IsQuality;
                                consultationEvaluation.QualityReason = consultation.consultationEvaluation.QualityReason;
                                consultationEvaluation.StarTitle = consultation.consultationEvaluation.StarTitle;
                                consultationEvaluation.SequenceNo = consultation.consultationEvaluation.SequenceNo;
                                consultationEvaluation.Id = consultationEvaluations.FirstOrDefault().Id;
                                #endregion
                                var result = await _consultationEvaluationService.UpdateConsultationEvaluation(consultationEvaluation);
                            }
                            else
                            {
                                consultation.consultationEvaluation.ConsultID = ConsultationInfo.Id;
                                consultation.consultationEvaluation.Id = 0;
                                var result = await _consultationEvaluationService.CreateConsultationEvaluation(consultation.consultationEvaluation);
                            }
                        }
                        #endregion
                        //Logger.Info("评分结束");
                        //Logger.Info("回复 回答 追问开始");
                        #region  回复 回答 追问
                        if (consultation.consultationReply != null)
                        {
                            ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
                            consultationReplyIn.AndAlso(a => a.SequenceNo == consultation.consultationReply.SequenceNo);
                            var consultationReplys = await _consultationReplyService.ConsultationReplyList(consultationReplyIn);

                            if (consultationReplys.Count > 0)
                            {
                                #region 回复 回答 追问
                                ConsultationReply consultationReply = consultationReplys.FirstOrDefault();
                                consultationReply.CreatedBy = consultation.consultationReply.CreatedBy;
                                consultationReply.CreatedOn = consultation.consultationReply.CreatedOn;
                                consultationReply.ModifyBy = consultation.consultationReply.ModifyBy;
                                consultationReply.ModifyOn = consultation.consultationReply.ModifyOn;
                                consultationReply.IsDelete = consultation.consultationReply.IsDelete;
                                consultationReply.DeleteBy = consultation.consultationReply.DeleteBy;
                                consultationReply.DeleteTime = consultation.consultationReply.DeleteTime;
                                consultationReply.ConsultNumber = consultation.consultationReply.ConsultNumber;
                                consultationReply.ConsultID = consultationReplys.FirstOrDefault().ConsultID;
                                consultationReply.ConsultantID = consultation.consultationReply.ConsultantID;
                                consultationReply.ConsultantName = consultation.consultationReply.ConsultantName;
                                consultationReply.PatientID = consultation.consultationReply.PatientID;
                                consultationReply.PatientName = consultation.consultationReply.PatientName;
                                consultationReply.DoctorName = consultation.consultationReply.DoctorName;
                                consultationReply.DoctorID = consultation.consultationReply.DoctorID;
                                consultationReply.ConsultType = consultation.consultationReply.ConsultType;
                                consultationReply.PatientTelephone = consultation.consultationReply.PatientTelephone;
                                consultationReply.PatientCity = consultation.consultationReply.PatientCity;
                                consultationReply.IllnessDescription = consultation.consultationReply.IllnessDescription;
                                consultationReply.ReplyType = consultation.consultationReply.ReplyType;
                                consultationReply.RepayIllnessDescription = consultation.consultationReply.RepayIllnessDescription;
                                consultationReply.ReplyState = consultation.consultationReply.ReplyState;
                                consultationReply.SequenceNo = consultation.consultationReply.SequenceNo;
                                consultationReply.Id = consultationReplys.FirstOrDefault().Id;
                                #endregion
                                var result = await _consultationReplyService.UpdateConsultationReply(consultationReply);
                            }
                            else
                            {
                                //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                                //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == consultation.consultationReply.ConsultNumber && a.JobSates == "Open" && a.JobCode == "QuestionConsultation").ToList();

                                HangFireJobIn hangFireJobIn = new HangFireJobIn();
                                hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == consultation.consultationReply.ConsultNumber && a.JobSates == "Open" && a.JobCode == "QuestionConsultation");
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

                                consultation.consultationReply.ConsultID = ConsultationInfo.Id;
                                consultation.consultationReply.Id = 0;
                                var result = await _consultationReplyService.CreateConsultationReply(consultation.consultationReply);
                            }
                        }
                        #endregion
                        //Logger.Info("回复 回答 追问结束");
                        //Logger.Info("电话开始");
                        #region 电话回复
                        if (consultation.phoneReplyRecord != null)
                        {
                            PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
                            phoneReplyRecordIn.AndAlso(a => a.ConsultNumber == consultation.phoneReplyRecord.SequenceNo);
                            var phoneReplyRecords = await _phoneReplyRecordService.PhoneReplyRecordList(phoneReplyRecordIn);

                            if (phoneReplyRecords.Count > 0)
                            {
                                #region 电话回复
                                PhoneReplyRecord phoneReplyRecord = phoneReplyRecords.FirstOrDefault();
                                phoneReplyRecord.CreatedBy = consultation.phoneReplyRecord.CreatedBy;
                                phoneReplyRecord.CreatedOn = consultation.phoneReplyRecord.CreatedOn;
                                phoneReplyRecord.ModifyBy = consultation.phoneReplyRecord.ModifyBy;
                                phoneReplyRecord.ModifyOn = consultation.phoneReplyRecord.ModifyOn;
                                phoneReplyRecord.IsDelete = consultation.phoneReplyRecord.IsDelete;
                                phoneReplyRecord.DeleteBy = consultation.phoneReplyRecord.DeleteBy;
                                phoneReplyRecord.DeleteTime = consultation.phoneReplyRecord.DeleteTime;
                                phoneReplyRecord.ConsultNumber = consultation.phoneReplyRecord.ConsultNumber;
                                phoneReplyRecord.ConsultID = phoneReplyRecords.FirstOrDefault().ConsultID;
                                phoneReplyRecord.ConsultantID = consultation.phoneReplyRecord.ConsultantID;
                                phoneReplyRecord.ConsultantName = consultation.phoneReplyRecord.ConsultantName;
                                phoneReplyRecord.PatientID = consultation.phoneReplyRecord.PatientID;
                                phoneReplyRecord.PatientName = consultation.phoneReplyRecord.PatientName;
                                phoneReplyRecord.DoctorID = consultation.phoneReplyRecord.DoctorID;
                                phoneReplyRecord.DoctorName = consultation.phoneReplyRecord.DoctorName;
                                phoneReplyRecord.CallTime = consultation.phoneReplyRecord.CallTime;
                                phoneReplyRecord.CallDuration = consultation.phoneReplyRecord.CallDuration;
                                phoneReplyRecord.CallIntro = consultation.phoneReplyRecord.CallIntro;
                                phoneReplyRecord.RecordAddress = consultation.phoneReplyRecord.RecordAddress;
                                phoneReplyRecord.Callee = consultation.phoneReplyRecord.Callee;
                                phoneReplyRecord.Caller = consultation.phoneReplyRecord.Caller;
                                phoneReplyRecord.IsUpload = consultation.phoneReplyRecord.IsUpload;
                                phoneReplyRecord.StorageAddress = consultation.phoneReplyRecord.StorageAddress;
                                phoneReplyRecord.SequenceNo = consultation.phoneReplyRecord.SequenceNo;
                                phoneReplyRecord.Id = phoneReplyRecords.FirstOrDefault().Id;
                                #endregion
                                var result = await _phoneReplyRecordService.UpdatePhoneReplyRecord(phoneReplyRecord);
                            }
                            else
                            {
                                consultation.phoneReplyRecord.ConsultID = ConsultationInfo.Id;
                                consultation.phoneReplyRecord.Id = 0;
                                var result = await _phoneReplyRecordService.CreatePhoneReplyRecord(consultation.phoneReplyRecord);
                            }
                        }
                        #endregion
                        //Logger.Info("电话结束");
                        //Logger.Info("开始发送消息");
                        #region 发送消息提醒
                        SendMessageInfo sendMessageInfo = new SendMessageInfo();
                        sendMessageInfo.TemplateCode = PublishsInfo.TemplateCode;
                        sendMessageInfo.OperationType = PublishsInfo.OperationType;
                        sendMessageInfo.ConsultNumber = PublishsInfo.BusinessCode;
                        sendMessageInfo.MessageRemark = PublishsInfo.MessageRemark;
                        if (consultation.consultationEvaluation != null)
                        {
                            // 增加评分星级
                            sendMessageInfo.EvaluateLevel = consultation.consultationEvaluation.EvaluateLevel+"星";
                        }
                        try
                        {
                            var re = await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                        }
                        catch(Exception ex)
                        {
                             Logger.Info("sendMessage异常："+ ex.Message.ToString()+"strack:"+ex.StackTrace.ToString());
                        }
                        #endregion
                        //Logger.Info("结束发送消息");
                        unitOfWork.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Info("异常："+ ex.Message.ToString()+"strack:"+ex.StackTrace.ToString());
                    throw ex;
                }
            }
            if (PublishsInfo != null)
            {
                this.ObjectResultModule.Object = PublishsInfo;
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
        
    }
}
