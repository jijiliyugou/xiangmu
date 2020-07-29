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
using Yaeher.Common.EventHelper;
using Yaeher.Common.HttpHelpers;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.EventBus;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;

namespace YaeherPatientAPI.Web.Host.Controllers
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
        private readonly IAbpSession _IabpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        #region
        private readonly IConsultationService _consultationService;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IRefundManageService _refundManageService;
        private readonly IConsultationEvaluationService _consultationEvaluationService;
        private readonly IConsultationReplyService _consultationReplyService;
        private readonly IPhoneReplyRecordService _phoneReplyRecordService;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishsService"></param>
        /// <param name="receiveEventService"></param>
        /// <param name="subscribeService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="subscribetionService"></param>
        /// <param name="consultationService"></param>
        /// <param name="orderManageService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="refundManageService"></param>
        /// <param name="consultationEvaluationService"></param>
        /// <param name="consultationReplyService"></param>
        /// <param name="phoneReplyRecordService"></param>
        /// <param name="abpSession"></param>
        public EvnetController(IPublishsService publishsService,
                               IReceiveEventService receiveEventService,
                               ISubscribeService subscribeService,
                               IUnitOfWorkManager unitOfWorkManager,
                               ISubscribetionService subscribetionService,
                               IConsultationService consultationService,
                               IOrderManageService orderManageService,
                               IOrderTradeRecordService orderTradeRecordService,
                               IRefundManageService refundManageService,
                               IConsultationEvaluationService consultationEvaluationService,
                               IConsultationReplyService consultationReplyService,
                               IPhoneReplyRecordService phoneReplyRecordService,
                               IAbpSession abpSession)
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
        }
        /// <summary>
        /// 发布任务
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [Route("api/CreatePublishs")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreatePublishs([FromBody] Publishs PublishsInfo)
        {
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
                PublishStatus = PublishsInfo.PublishStatus,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
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
                ActionStatus = SubscribeInfo.ActionStatus,  // 默认true
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
        /// 执行未执行的发布 
        /// </summary>
        /// <returns></returns>
        public async Task<string> DoPublishs()
        {
            var publishsList = await _publishsService.PublishsList();
            List<Publishs> publishs = publishsList.Where(a => a.PublishStatus == false).ToList();
            if (publishs.Count > 0)
            {
                string uri = string.Empty;
                foreach (var PublishsInfo in publishs)
                {
                    var Result = string.Empty;
                    try
                    {
                        string PublishsJSON = JsonHelper.ToJson(PublishsInfo);
                        Result = await new HttpHelper().PostResponseAsync(uri, PublishsJSON);
                        if (Result.Contains("Success"))
                        {
                            PublishsInfo.PublishStatus = true;
                            PublishsInfo.ModifyOn = DateTime.Now;
                            await _publishsService.UpdatePublishs(PublishsInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                }
                return "";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 执行未订阅成功的信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> DoSubscribe()
        {
            var subscribeList = await _subscribeService.SubscribeList();
            List<Subscribe> subscribes = subscribeList.Where(a => a.ActionStatus == false).ToList();
            if (subscribes.Count > 0)
            {
                string uri = string.Empty;
                foreach (var subscribeInfo in subscribes)
                {
                    var Result = string.Empty;
                    try
                    {
                        string subscribeJson = JsonHelper.ToJson(subscribeInfo);
                        Result = await new HttpHelper().PostResponseAsync(uri, subscribeJson);
                        if (Result.Contains("Success"))
                        {
                            subscribeInfo.ActionStatus = true;
                            subscribeInfo.ModifyOn = DateTime.Now;
                            await _subscribeService.UpdateSubscribe(subscribeInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                }
                return "";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <returns></returns>
        public async Task<string> CancelSubscribe(Subscribe SubscribeInfo)
        {
            var subscribeList = await _subscribeService.SubscribeList();
            Subscribe subscribes = subscribeList.Where(a => a.ActionStatus == false && a.Subscriber == SubscribeInfo.Subscriber).FirstOrDefault();
            if (subscribes != null)
            {
                string uri = string.Empty;
                string subscribeJson = JsonHelper.ToJson(subscribes);
                var Result = await new HttpHelper().PostResponseAsync(uri, subscribeJson);
                if (Result.Contains("Success"))
                {
                    subscribes.ActionStatus = SubscribeInfo.ActionStatus;
                    subscribes.ModifyOn = DateTime.Now;
                    await _subscribeService.UpdateSubscribe(subscribes);
                }
                return "";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 阅读订阅信息
        /// </summary>
        /// <returns></returns>
        [Route("api/ClientSubscribetion")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClientSubscribetion([FromBody]Subscribetion subscribetion)
        {
            if (subscribetion != null)
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var Result = await _subscribetionService.CreateSubscribetion(subscribetion);
                    if (Result.Id > 0)
                    {
                        Consultation consultation = JsonHelper.FromJson<Consultation>(subscribetion.BusinessJSON);
                        try
                        {
                            // 判断咨询主表
                            if (consultation.yaeherConsultation != null)
                            {
                                ConsultationIn consultationIn = new ConsultationIn();
                                consultationIn.AndAlso(a => a.ConsultNumber == consultation.yaeherConsultation.ConsultNumber);
                                var Consultation = await _consultationService.YaeherConsultationList(consultationIn);
                                #region 咨询表
                                YaeherConsultation yaeherConsultation = new YaeherConsultation();
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
                                if (Consultation.Count > 0)
                                {
                                    yaeherConsultation.Id = Consultation.FirstOrDefault().Id;
                                    consultation.yaeherConsultation = await _consultationService.UpdateYaeherConsultation(yaeherConsultation);
                                }
                                else
                                {
                                    consultation.yaeherConsultation = await _consultationService.CreateYaeherConsultation(yaeherConsultation);
                                }
                            }
                            // 订单表
                            if (consultation.orderManage != null)
                            {
                                OrderManageIn orderManageIn = new OrderManageIn();
                                orderManageIn.AndAlso(a => a.SequenceNo == consultation.orderManage.SequenceNo);
                                var orderManages = await _orderManageService.OrderManageList(orderManageIn);
                                #region 订单表
                                OrderManage orderManage = new OrderManage();
                                orderManage.CreatedBy = consultation.orderManage.CreatedBy;
                                orderManage.CreatedOn = consultation.orderManage.CreatedOn;
                                orderManage.ModifyBy = consultation.orderManage.ModifyBy;
                                orderManage.ModifyOn = consultation.orderManage.ModifyOn;
                                orderManage.IsDelete = consultation.orderManage.IsDelete;
                                orderManage.DeleteBy = consultation.orderManage.DeleteBy;
                                orderManage.DeleteTime = consultation.orderManage.DeleteTime;
                                orderManage.OrderNumber = consultation.orderManage.OrderNumber;
                                orderManage.ConsultNumber = consultation.orderManage.ConsultNumber;
                                orderManage.ConsultID = consultation.orderManage.ConsultID;
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
                                #endregion
                                if (orderManages.Count > 0)
                                {
                                    orderManage.Id = orderManages.FirstOrDefault().Id;
                                    consultation.orderManage = await _orderManageService.UpdateOrderManage(orderManage);
                                }
                                else
                                {
                                    consultation.orderManage = await _orderManageService.CreateOrderManage(orderManage);
                                }
                            }
                            // 订单交易记录
                            if (consultation.orderTradeRecords != null)
                            {
                                OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
                                orderTradeRecordIn.AndAlso(a => a.SequenceNo == consultation.orderTradeRecords.SequenceNo);
                                var orderTradeRecords = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);
                                #region 订单交易记录
                                OrderTradeRecord orderTradeRecord = new OrderTradeRecord();
                                orderTradeRecord.CreatedBy = consultation.orderTradeRecords.CreatedBy;
                                orderTradeRecord.CreatedOn = consultation.orderTradeRecords.CreatedOn;
                                orderTradeRecord.ModifyBy = consultation.orderTradeRecords.ModifyBy;
                                orderTradeRecord.ModifyOn = consultation.orderTradeRecords.ModifyOn;
                                orderTradeRecord.IsDelete = consultation.orderTradeRecords.IsDelete;
                                orderTradeRecord.DeleteBy = consultation.orderTradeRecords.DeleteBy;
                                orderTradeRecord.DeleteTime = consultation.orderTradeRecords.DeleteTime;
                                orderTradeRecord.OrderID = consultation.orderTradeRecords.OrderID;
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
                                #endregion
                                if (orderTradeRecords.Count > 0)
                                {
                                    orderTradeRecord.Id = orderTradeRecords.FirstOrDefault().Id;
                                    consultation.orderTradeRecords = await _orderTradeRecordService.UpdateOrderTradeRecord(orderTradeRecord);
                                }
                                else
                                {
                                    consultation.orderTradeRecords = await _orderTradeRecordService.CreateOrderTradeRecord(orderTradeRecord);
                                }
                            }
                            // 退单表
                            if (consultation.refundManage != null)
                            {
                                RefundManageIn refundManageIn = new RefundManageIn();
                                refundManageIn.AndAlso(a => a.SequenceNo == consultation.refundManage.SequenceNo);
                                var refundManages = await _refundManageService.RefundManageList(refundManageIn);
                                #region 退单表
                                RefundManage refundManage = new RefundManage();
                                refundManage.CreatedBy = consultation.refundManage.CreatedBy;
                                refundManage.CreatedOn = consultation.refundManage.CreatedOn;
                                refundManage.ModifyBy = consultation.refundManage.ModifyBy;
                                refundManage.ModifyOn = consultation.refundManage.ModifyOn;
                                refundManage.IsDelete = consultation.refundManage.IsDelete;
                                refundManage.DeleteBy = consultation.refundManage.DeleteBy;
                                refundManage.DeleteTime = consultation.refundManage.DeleteTime;
                                refundManage.RefundNumber = consultation.refundManage.RefundNumber;
                                refundManage.ConsultID = consultation.refundManage.ConsultID;
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

                                #endregion
                                if (refundManages.Count > 0)
                                {
                                    refundManage.Id = refundManages.FirstOrDefault().Id;
                                    consultation.refundManage = await _refundManageService.UpdateRefundManage(refundManage);
                                }
                                else
                                {
                                    consultation.refundManage = await _refundManageService.CreateRefundManage(refundManage);
                                }
                            }
                            // 咨询评分
                            if (consultation.consultationEvaluation != null)
                            {
                                ConsultationEvaluationIn consultationEvaluationIn = new ConsultationEvaluationIn();
                                consultationEvaluationIn.AndAlso(a => a.SequenceNo == consultation.consultationEvaluation.SequenceNo);
                                var consultationEvaluations = await _consultationEvaluationService.ConsultationEvaluationList(consultationEvaluationIn);
                                #region 咨询评分
                                ConsultationEvaluation consultationEvaluation = new ConsultationEvaluation();
                                consultationEvaluation.CreatedBy = consultation.consultationEvaluation.CreatedBy;
                                consultationEvaluation.CreatedOn = consultation.consultationEvaluation.CreatedOn;
                                consultationEvaluation.ModifyBy = consultation.consultationEvaluation.ModifyBy;
                                consultationEvaluation.ModifyOn = consultation.consultationEvaluation.ModifyOn;
                                consultationEvaluation.IsDelete = consultation.consultationEvaluation.IsDelete;
                                consultationEvaluation.DeleteBy = consultation.consultationEvaluation.DeleteBy;
                                consultationEvaluation.DeleteTime = consultation.consultationEvaluation.DeleteTime;
                                consultationEvaluation.ConsultNumber = consultation.consultationEvaluation.ConsultNumber;
                                consultationEvaluation.ConsultID = consultation.consultationEvaluation.ConsultID;
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

                                #endregion
                                if (consultationEvaluations.Count > 0)
                                {
                                    consultationEvaluation.Id = consultationEvaluations.FirstOrDefault().Id;
                                    consultation.consultationEvaluation = await _consultationEvaluationService.UpdateConsultationEvaluation(consultationEvaluation);
                                }
                                else
                                {
                                    consultation.consultationEvaluation = await _consultationEvaluationService.CreateConsultationEvaluation(consultationEvaluation);
                                }
                            }
                            // 回复 回答 追问
                            if (consultation.consultationReply != null)
                            {
                                ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
                                consultationReplyIn.AndAlso(a => a.ConsultNumber == consultation.consultationReply.SequenceNo);
                                var consultationReplys = await _consultationReplyService.ConsultationReplyList(consultationReplyIn);
                                #region 回复 回答 追问
                                ConsultationReply consultationReply = new ConsultationReply();
                                consultationReply.CreatedBy = consultation.consultationReply.CreatedBy;
                                consultationReply.CreatedOn = consultation.consultationReply.CreatedOn;
                                consultationReply.ModifyBy = consultation.consultationReply.ModifyBy;
                                consultationReply.ModifyOn = consultation.consultationReply.ModifyOn;
                                consultationReply.IsDelete = consultation.consultationReply.IsDelete;
                                consultationReply.DeleteBy = consultation.consultationReply.DeleteBy;
                                consultationReply.DeleteTime = consultation.consultationReply.DeleteTime;
                                consultationReply.ConsultNumber = consultation.consultationReply.ConsultNumber;
                                consultationReply.ConsultID = consultation.consultationReply.ConsultID;
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
                                #endregion
                                if (consultationReplys.Count > 0)
                                {
                                    consultationReply.Id = consultationReplys.FirstOrDefault().Id;
                                    consultation.consultationReply = await _consultationReplyService.UpdateConsultationReply(consultationReply);
                                }
                                else
                                {
                                    consultation.consultationReply = await _consultationReplyService.CreateConsultationReply(consultationReply);
                                }
                            }
                            // 电话回复
                            if (consultation.phoneReplyRecord != null)
                            {
                                PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
                                phoneReplyRecordIn.AndAlso(a => a.ConsultNumber == consultation.phoneReplyRecord.SequenceNo);
                                var phoneReplyRecords = await _phoneReplyRecordService.PhoneReplyRecordList(phoneReplyRecordIn);
                                #region 电话回复
                                PhoneReplyRecord phoneReplyRecord = new PhoneReplyRecord();
                                phoneReplyRecord.CreatedBy = consultation.phoneReplyRecord.CreatedBy;
                                phoneReplyRecord.CreatedOn = consultation.phoneReplyRecord.CreatedOn;
                                phoneReplyRecord.ModifyBy = consultation.phoneReplyRecord.ModifyBy;
                                phoneReplyRecord.ModifyOn = consultation.phoneReplyRecord.ModifyOn;
                                phoneReplyRecord.IsDelete = consultation.phoneReplyRecord.IsDelete;
                                phoneReplyRecord.DeleteBy = consultation.phoneReplyRecord.DeleteBy;
                                phoneReplyRecord.DeleteTime = consultation.phoneReplyRecord.DeleteTime;
                                phoneReplyRecord.ConsultNumber = consultation.phoneReplyRecord.ConsultNumber;
                                phoneReplyRecord.ConsultID = consultation.phoneReplyRecord.ConsultID;
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
                                #endregion
                                if (phoneReplyRecords.Count > 0)
                                {
                                    phoneReplyRecord.Id = phoneReplyRecords.FirstOrDefault().Id;
                                    consultation.phoneReplyRecord = await _phoneReplyRecordService.UpdatePhoneReplyRecord(phoneReplyRecord);
                                }
                                else
                                {
                                    consultation.phoneReplyRecord = await _phoneReplyRecordService.CreatePhoneReplyRecord(phoneReplyRecord);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        this.ObjectResultModule.Object = Result;
                        this.ObjectResultModule.StatusCode = 200;
                        this.ObjectResultModule.Message = "success";
                    }
                    else
                    {
                        this.ObjectResultModule.Object = "";
                        this.ObjectResultModule.StatusCode = 400;
                        this.ObjectResultModule.Message = "error!";
                    }
                    unitOfWork.Complete();
                }
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
        /// 咨询管理
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationManage")]
        [HttpPost]
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
                    Consultation consultation = JsonHelper.FromJson<Consultation>(PublishsInfo.BusinessJSON);
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
                            var result = await _consultationService.UpdateYaeherConsultation(yaeherConsultation);
                        }
                        else
                        {
                            consultation.yaeherConsultation.Id = 0;
                            var result = await _consultationService.CreateYaeherConsultation(consultation.yaeherConsultation);
                        }
                    }
                    #endregion
                    #region 订单表
                    if (consultation.orderManage != null)
                    {
                        OrderManageIn orderManageIn = new OrderManageIn();
                        orderManageIn.AndAlso(a => a.SequenceNo == consultation.orderManage.SequenceNo);
                        var orderManages = await _orderManageService.OrderManageList(orderManageIn);
                        if (orderManages.Count > 0)
                        {
                            #region 订单表
                            OrderManage orderManage = new OrderManage();
                            orderManage.CreatedBy = consultation.orderManage.CreatedBy;
                            orderManage.CreatedOn = consultation.orderManage.CreatedOn;
                            orderManage.ModifyBy = consultation.orderManage.ModifyBy;
                            orderManage.ModifyOn = consultation.orderManage.ModifyOn;
                            orderManage.IsDelete = consultation.orderManage.IsDelete;
                            orderManage.DeleteBy = consultation.orderManage.DeleteBy;
                            orderManage.DeleteTime = consultation.orderManage.DeleteTime;
                            orderManage.OrderNumber = consultation.orderManage.OrderNumber;
                            orderManage.ConsultNumber = consultation.orderManage.ConsultNumber;
                            orderManage.ConsultID = consultation.orderManage.ConsultID;
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
                            consultation.orderManage.Id = 0;
                            var result = await _orderManageService.CreateOrderManage(consultation.orderManage);
                        }
                    }
                    #endregion
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
                            orderTradeRecord.OrderID = consultation.orderTradeRecords.OrderID;
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
                            //orderTradeRecord.Id = orderTradeRecords.FirstOrDefault().Id;
                            #endregion
                            var result = await _orderTradeRecordService.UpdateOrderTradeRecord(orderTradeRecord);
                        }
                        else
                        {
                            consultation.orderTradeRecords.Id = 0;
                            var result = await _orderTradeRecordService.CreateOrderTradeRecord(consultation.orderTradeRecords);
                        }
                    }
                    #endregion
                    #region 退单表
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
                            refundManage.ConsultID = consultation.refundManage.ConsultID;
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
                            //refundManage.Id=refundManages.FirstOrDefault().Id;
                            #endregion
                            var result = await _refundManageService.UpdateRefundManage(refundManage);
                        }
                        else
                        {
                            consultation.refundManage.Id = 0;
                            var result = await _refundManageService.CreateRefundManage(consultation.refundManage);
                        }
                    }
                    #endregion
                    #region 咨询评分
                    if (consultation.consultationEvaluation != null)
                    {
                        ConsultationEvaluationIn consultationEvaluationIn = new ConsultationEvaluationIn();
                        consultationEvaluationIn.AndAlso(a => a.SequenceNo == consultation.consultationEvaluation.SequenceNo);
                        var consultationEvaluations = await _consultationEvaluationService.ConsultationEvaluationList(consultationEvaluationIn);

                        if (consultationEvaluations.Count > 0)
                        {
                            #region 咨询评分
                            ConsultationEvaluation consultationEvaluation = new ConsultationEvaluation();
                            consultationEvaluation.CreatedBy = consultation.consultationEvaluation.CreatedBy;
                            consultationEvaluation.CreatedOn = consultation.consultationEvaluation.CreatedOn;
                            consultationEvaluation.ModifyBy = consultation.consultationEvaluation.ModifyBy;
                            consultationEvaluation.ModifyOn = consultation.consultationEvaluation.ModifyOn;
                            consultationEvaluation.IsDelete = consultation.consultationEvaluation.IsDelete;
                            consultationEvaluation.DeleteBy = consultation.consultationEvaluation.DeleteBy;
                            consultationEvaluation.DeleteTime = consultation.consultationEvaluation.DeleteTime;
                            consultationEvaluation.ConsultNumber = consultation.consultationEvaluation.ConsultNumber;
                            consultationEvaluation.ConsultID = consultation.consultationEvaluation.ConsultID;
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
                            consultation.consultationEvaluation.Id = 0;
                            var result = await _consultationEvaluationService.CreateConsultationEvaluation(consultation.consultationEvaluation);
                        }
                    }
                    #endregion
                    #region  回复 回答 追问
                    if (consultation.consultationReply != null)
                    {
                        ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
                        consultationReplyIn.AndAlso(a => a.ConsultNumber == consultation.consultationReply.SequenceNo);
                        var consultationReplys = await _consultationReplyService.ConsultationReplyList(consultationReplyIn);

                        if (consultationReplys.Count > 0)
                        {
                            #region 回复 回答 追问
                            ConsultationReply consultationReply = new ConsultationReply();
                            consultationReply.CreatedBy = consultation.consultationReply.CreatedBy;
                            consultationReply.CreatedOn = consultation.consultationReply.CreatedOn;
                            consultationReply.ModifyBy = consultation.consultationReply.ModifyBy;
                            consultationReply.ModifyOn = consultation.consultationReply.ModifyOn;
                            consultationReply.IsDelete = consultation.consultationReply.IsDelete;
                            consultationReply.DeleteBy = consultation.consultationReply.DeleteBy;
                            consultationReply.DeleteTime = consultation.consultationReply.DeleteTime;
                            consultationReply.ConsultNumber = consultation.consultationReply.ConsultNumber;
                            consultationReply.ConsultID = consultation.consultationReply.ConsultID;
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
                            consultation.consultationReply.Id = 0;
                            var result = await _consultationReplyService.CreateConsultationReply(consultation.consultationReply);
                        }
                    }
                    #endregion
                    #region 电话回复
                    if (consultation.phoneReplyRecord != null)
                    {
                        PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
                        phoneReplyRecordIn.AndAlso(a => a.ConsultNumber == consultation.phoneReplyRecord.SequenceNo);
                        var phoneReplyRecords = await _phoneReplyRecordService.PhoneReplyRecordList(phoneReplyRecordIn);

                        if (phoneReplyRecords.Count > 0)
                        {
                            #region 电话回复
                            PhoneReplyRecord phoneReplyRecord = new PhoneReplyRecord();
                            phoneReplyRecord.CreatedBy = consultation.phoneReplyRecord.CreatedBy;
                            phoneReplyRecord.CreatedOn = consultation.phoneReplyRecord.CreatedOn;
                            phoneReplyRecord.ModifyBy = consultation.phoneReplyRecord.ModifyBy;
                            phoneReplyRecord.ModifyOn = consultation.phoneReplyRecord.ModifyOn;
                            phoneReplyRecord.IsDelete = consultation.phoneReplyRecord.IsDelete;
                            phoneReplyRecord.DeleteBy = consultation.phoneReplyRecord.DeleteBy;
                            phoneReplyRecord.DeleteTime = consultation.phoneReplyRecord.DeleteTime;
                            phoneReplyRecord.ConsultNumber = consultation.phoneReplyRecord.ConsultNumber;
                            phoneReplyRecord.ConsultID = consultation.phoneReplyRecord.ConsultID;
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
                            phoneReplyRecord.Id= phoneReplyRecords.FirstOrDefault().Id;
                            #endregion
                            var result = await _phoneReplyRecordService.UpdatePhoneReplyRecord(phoneReplyRecord);
                        }
                        else
                        {
                            consultation.phoneReplyRecord.Id = 0;
                            var result = await _phoneReplyRecordService.CreatePhoneReplyRecord(consultation.phoneReplyRecord);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
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