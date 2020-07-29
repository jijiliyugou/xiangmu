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
using Yaeher.Common.HttpHelpers;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.Controllers;
using Yaeher.EventBus;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;
using Yaeher.HangFire;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;

namespace YaeherDoctorAPI.Web.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EvnetController : YaeherAppServiceBase
    {
        #region
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
        #endregion
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
        public EvnetController(IPublishsService publishsService,
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
            _hangFireJobService = hangFireJobService;
            _systemParameterService = systemParameterService;
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
        #region
        ///// <summary>
        ///// 阅读订阅信息
        ///// </summary>
        ///// <returns></returns>
        //[Route("api/ClientSubscribetion")]
        //[HttpPost]
        //[AbpAllowAnonymous]
        //public async Task<ObjectResultModule> ClientSubscribetion([FromBody]Subscribetion subscribetion)
        //{
        //    if (subscribetion != null)
        //    {
        //        using (var unitOfWork = _unitOfWorkManager.Begin())
        //        {
        //            var Result = await _subscribetionService.CreateSubscribetion(subscribetion);
        //            if (Result.Id > 0)
        //            {
        //                this.ObjectResultModule.Object = Result;
        //                this.ObjectResultModule.StatusCode = 200;
        //                this.ObjectResultModule.Message = "success";
        //            }
        //            else
        //            {
        //                this.ObjectResultModule.Object = "";
        //                this.ObjectResultModule.StatusCode = 400;
        //                this.ObjectResultModule.Message = "error!";
        //            }
        //            unitOfWork.Complete();
        //        }
        //    }
        //    else
        //    {
        //        this.ObjectResultModule.Object = "";
        //        this.ObjectResultModule.StatusCode = 400;
        //        this.ObjectResultModule.Message = "error!";
        //    }
        //    return ObjectResultModule;
        //}
        #endregion
      
       
    }
}