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
using Microsoft.AspNetCore.Mvc;
using Myvas.AspNetCore.TencentCos;
using Senparc.Weixin.MP.AdvancedAPIs;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.Common;
using Yaeher.Common.CloudCallCenter;
using Yaeher.Common.Constants;
using Yaeher.Common.HangfireJob;
using Yaeher.Common.TencentCustom;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.Doctor;
using Yaeher.DoctorQuality;
using Yaeher.DoctorQuality.Dto;
using Yaeher.EventBus;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;
using Yaeher.Extensions;
using Yaeher.HangFire;
using Yaeher.MessageRemind;
using Yaeher.NumericalStatement;
using Yaeher.NumericalStatement.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherDoctorAPI.Web.Host.Controllers
{
    /// <summary>
    /// 咨询 以及订单管理
    /// </summary>
    public class ConsultationController : YaeherAppServiceBase
    {
        private readonly IConsultationEvaluationService _consultationEvaluationService;
        private readonly IConsultationReplyService _consultationReplyService;
        private readonly IConsultationService _consultationService;
        private readonly IPhoneReplyRecordService _phoneReplyRecordService;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IRefundManageService _refundManageService;
        private readonly IIncomeDevideService _incomeDevideService;
        private readonly ILeaguerInfoService _leaguerservice;
        private readonly IYaeherUserService _yaeherUser;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IServiceMoneyListService _serviceMoneyListService;
        private readonly IAttachmentServices _attachmentServices;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICollectConsultationService _collectConsultationService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IUserManagerService _userManagerService;
        private readonly IEvaluationTotalService _evaluationTotalService;
        private readonly IAbpSession _IabpSession;
        private readonly IQualityControlManageService _QualityControlManageService;
        private readonly IPublishsService _publishsService;
        private readonly IYaeherPhoneService _yaeherPhoneService;
        private readonly IHangFireJobService _hangFireJobService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly ISystemConfigsService _systemConfigsService;
        private readonly IYaeherLabelConfigService _yaeherLabelConfigService;
        private readonly IWecharSendMessageService _wecharSendMessageService;
        private readonly ITencentCosHandler _cosHandler;
        private readonly ISystemTokenService _systemTokenService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultationEvaluationService"></param>
        /// <param name="consultationReplyService"></param>
        /// <param name="consultationService"></param>
        /// <param name="phoneReplyRecordService"></param>
        /// <param name="orderManageService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="refundManageService"></param>
        /// <param name="incomeDevideService"></param>
        /// <param name="leaguerservice"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="ServiceMoneyListService"></param>
        /// <param name="attachmentServices"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="collectConsultationService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="evaluationTotalService"></param>
        /// <param name="session"></param>
        /// <param name="qualityControlManageService"></param>
        /// <param name="publishsService"></param>
        /// <param name="yaeherPhoneService"></param>
        /// <param name="hangFireJobService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="systemConfigsService"></param>
        /// <param name="yaeherLabelConfigService"></param>
        /// <param name="wecharSendMessageService"></param>
        /// <param name="cosHandler"></param>
        /// <param name="systemTokenService"></param>
        public ConsultationController(IConsultationEvaluationService consultationEvaluationService,
                                    IConsultationReplyService consultationReplyService,
                                    IConsultationService consultationService,
                                    IPhoneReplyRecordService phoneReplyRecordService,
                                    IOrderManageService orderManageService,
                                    IOrderTradeRecordService orderTradeRecordService,
                                    IRefundManageService refundManageService,
                                    IIncomeDevideService incomeDevideService,
                                    ILeaguerInfoService leaguerservice,
                                    IYaeherUserService yaeherUserService,
                                    IYaeherDoctorService yaeherDoctorService,
                                    IServiceMoneyListService ServiceMoneyListService,
                                    IAttachmentServices attachmentServices,
                                    IUnitOfWorkManager unitOfWorkManager,
                                    ICollectConsultationService collectConsultationService,
                                    ISystemParameterService systemParameterService,
                                    IUserManagerService userManagerService,
                                    IEvaluationTotalService evaluationTotalService,
                                    IAbpSession session,
                                    IQualityControlManageService qualityControlManageService,
                                    IPublishsService publishsService,
                                    IYaeherPhoneService yaeherPhoneService,
                                    IHangFireJobService hangFireJobService,
                                    IYaeherOperListService yaeherOperListService,
                                    ISystemConfigsService systemConfigsService,
                                    IYaeherLabelConfigService yaeherLabelConfigService,
                                    IWecharSendMessageService wecharSendMessageService,
                                     ITencentCosHandler cosHandler,
                                     ISystemTokenService systemTokenService)
        {
            _consultationEvaluationService = consultationEvaluationService;
            _consultationReplyService = consultationReplyService;
            _consultationService = consultationService;
            _phoneReplyRecordService = phoneReplyRecordService;
            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _refundManageService = refundManageService;
            _incomeDevideService = incomeDevideService;
            _leaguerservice = leaguerservice;
            _yaeherUser = yaeherUserService;
            _yaeherDoctorService = yaeherDoctorService;
            _serviceMoneyListService = ServiceMoneyListService;
            _attachmentServices = attachmentServices;
            _unitOfWorkManager = unitOfWorkManager;
            _collectConsultationService = collectConsultationService;
            _systemParameterService = systemParameterService;
            _userManagerService = userManagerService;
            _evaluationTotalService = evaluationTotalService;
            _IabpSession = session;
            _QualityControlManageService = qualityControlManageService;
            _publishsService = publishsService;
            _yaeherPhoneService = yaeherPhoneService;
            _hangFireJobService = hangFireJobService;
            _yaeherOperListService = yaeherOperListService;
            _systemConfigsService = systemConfigsService;
            _yaeherLabelConfigService = yaeherLabelConfigService;
            _wecharSendMessageService = wecharSendMessageService;
            _cosHandler = cosHandler ?? throw new ArgumentNullException(nameof(cosHandler));
            _systemTokenService = systemTokenService;
        }
        #region 咨询管理

        /// <summary>
        /// 质控已处理咨询列表 Page
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/QualityConsultationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityConsultationPage([FromBody] ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationInfo.EndTime))
                {
                    ConsultationInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                ConsultationInfo.AndAlso(a => a.CreatedOn >= StartTime);
                ConsultationInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }

            //咨询类型
            if (!string.IsNullOrEmpty(ConsultationInfo.ConsultType))
            {
                ConsultationInfo.AndAlso(a => a.ConsultType.Contains(ConsultationInfo.ConsultType));
            }
            // 问题描述
            if (!string.IsNullOrEmpty(ConsultationInfo.IIInessDescription))
            {
                ConsultationInfo.AndAlso(a => a.IIInessDescription.Contains(ConsultationInfo.IIInessDescription));
            }
            // 关键字
            if (!string.IsNullOrEmpty(ConsultationInfo.KeyWord))
            {
                ConsultationInfo.AndAlso(a => a.IIInessType.Contains(ConsultationInfo.KeyWord) ||
                                              a.ConsultantName.Contains(ConsultationInfo.KeyWord) ||
                                              a.DoctorName.Contains(ConsultationInfo.KeyWord) ||
                                              a.PatientName.Contains(ConsultationInfo.KeyWord));
            }
            ConsultationInfo.AndAlso(a => a.IsDelete == false);

            // 区分平台来源
            if (usermanager.IsQC || usermanager.MobileRoleName == "quality")
            {
                ConsultationInfo.CreatedBy = userid;
                var param = new SystemParameterIn() { Type = "ConfigPar" };
                param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultState");
                var paramlist = await _systemParameterService.ParameterList(param);
                if (ConsultationInfo.RefundState == "return")//质控退单列表
                {
                    var values = await _consultationService.QualityRefundYaeherConsultationPage(ConsultationInfo);
                    if (values.Items.Count() == 0)
                    {
                        this.ObjectResultModule.StatusCode = 200;
                        this.ObjectResultModule.Object = new List<ConsultationIn>();
                    }
                    else
                    {
                        this.ObjectResultModule.Object = new ConsultationOut(values, ConsultationInfo, paramlist);

                    }
                }
                else//质控处理列表
                {
                    var values = await _consultationService.QualityYaeherConsultationPage(ConsultationInfo);
                    if (values.Items.Count() == 0)
                    {
                        this.ObjectResultModule.StatusCode = 200;
                        this.ObjectResultModule.Object = new List<ConsultationIn>();
                    }
                    else
                    {

                        this.ObjectResultModule.Object = new ConsultationOut(values, ConsultationInfo, paramlist);

                    }
                }
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityConsultationPage",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "QualityConsultationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 咨询管理 Page
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationPage([FromBody] ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (usermanager.MobileRoleName == "patient")
            {
                return new ObjectResultModule("", 400, "不允许查看医生订单！");
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationInfo.EndTime))
                {
                    ConsultationInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                ConsultationInfo.AndAlso(a => a.CreatedOn >= StartTime );
                ConsultationInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            // 是否已回复 2，已回复，1未回复，3默认所有
            if (ConsultationInfo.HasReply == 2)
            {
                ConsultationInfo.AndAlso(a => (a.HasReply == true || a.ConsultState == "return"));
            }
            else if (ConsultationInfo.HasReply == 1)
            {
                ConsultationInfo.AndAlso(a => a.HasReply == false );
                ConsultationInfo.AndAlso(a =>a.ConsultState != "return");
            }
            //增加咨询状态
            if (!string.IsNullOrEmpty(ConsultationInfo.ConsultState))
            {
                ConsultationInfo.AndAlso(a => a.ConsultState == ConsultationInfo.ConsultState);
            }
            //咨询类型
            if (!string.IsNullOrEmpty(ConsultationInfo.ConsultType))
            {
                ConsultationInfo.AndAlso(a => a.ConsultType.Contains(ConsultationInfo.ConsultType));
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.RefundState))
            {
                ConsultationInfo.AndAlso(a => a.RefundState != "" || a.RefundState != null);
            }
            // 问题描述
            if (!string.IsNullOrEmpty(ConsultationInfo.IIInessDescription))
            {
                ConsultationInfo.AndAlso(a => a.IIInessDescription.Contains(ConsultationInfo.IIInessDescription));
            }
            // 关键字
            if (!string.IsNullOrEmpty(ConsultationInfo.KeyWord))
            {
                ConsultationInfo.AndAlso(a => a.IIInessDescription.Contains(ConsultationInfo.KeyWord) ||
                                              a.ConsultantName.Contains(ConsultationInfo.KeyWord) ||
                                              a.DoctorName.Contains(ConsultationInfo.KeyWord) ||
                                              a.PatientName.Contains(ConsultationInfo.KeyWord) ||
                                              a.ConsultNumber.Contains(ConsultationInfo.KeyWord));
            }
            ConsultationInfo.AndAlso(a => a.IsDelete == false);
            // 区分平台来源
            if (ConsultationInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    ConsultationInfo.AndAlso(a => a.DoctorID == doctor.Id);
                    if (ConsultationInfo.CreatedBy > 0)
                    {
                        ConsultationInfo.AndAlso(a => a.CreatedBy == ConsultationInfo.CreatedBy);
                    }
                }
                if (usermanager.MobileRoleName == "quality")
                {
                    ConsultationInfo.AndAlso(a => a.DoctorID == ConsultationInfo.DoctorID);
                }
            }
            else if (ConsultationInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    ConsultationInfo.AndAlso(a => a.DoctorID == doctor.Id);
                }
            }
            var values = await _consultationService.YaeherConsultationPage(ConsultationInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = new List<ConsultationIn>();
            }
            else
            {
                var param = new SystemParameterIn() { Type = "ConfigPar" };
                param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultState");
                var paramlist = await _systemParameterService.ParameterList(param);

                var dict = new Dictionary<int, Tuple<DateTime>>();
                foreach (var item in values.Items)
                {
                    if (item.ConsultState == "created")
                    {
                        dict.Add(item.Id, Tuple.Create(item.CreatedOn));
                    }
                    else if (item.ConsultState == "consulting")
                    {
                        if (item.HasReply == false)
                        {
                            ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
                            consultationReplyIn.AndAlso(t => t.IsDelete == false && t.ConsultNumber == item.ConsultNumber && t.ReplyType == "inquiries");
                            var replylist = await _consultationReplyService.ConsultationReplyList(consultationReplyIn);
                            if (replylist.Count > 0)
                            {
                                dict.Add(item.Id, Tuple.Create(replylist[0].CreatedOn));
                            }
                        }
                        else
                        {
                            dict.Add(item.Id, Tuple.Create(item.Completetime));
                        }
                    }
                    else if (item.ConsultState == "success" && item.HasReply == false)
                    {
                        ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
                        consultationReplyIn.AndAlso(t => t.IsDelete == false && t.ConsultNumber == item.ConsultNumber && t.ReplyType == "inquiries");
                        var replylist = await _consultationReplyService.ConsultationReplyList(consultationReplyIn);
                        if (replylist.Count > 0)
                        {
                            dict.Add(item.Id, Tuple.Create(replylist[0].CreatedOn));
                        }
                    }
                }

                this.ObjectResultModule.Object = new ConsultationOut(values, ConsultationInfo, dict, paramlist);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationPage",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "ConsultationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询管理 List 
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationList([FromBody]ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            YaeherDoctor doctor = new YaeherDoctor();
            if (ConsultationInfo.DoctorID < 0)
            {
                doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            }
            else
            {
                doctor = await _yaeherDoctorService.YaeherDoctorByID(ConsultationInfo.DoctorID);
            }

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationInfo.EndTime))
                {
                    ConsultationInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                ConsultationInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            // 是否已回复 2，已回复，1未回复，3默认所有
            if (ConsultationInfo.HasReply == 2)
            {
                ConsultationInfo.AndAlso(t => t.HasReply == true);
            }
            else if (ConsultationInfo.HasReply == 2)
            {
                ConsultationInfo.AndAlso(t => t.HasReply == false);
            }
            //咨询类型
            if (!string.IsNullOrEmpty(ConsultationInfo.ConsultType))
            {
                ConsultationInfo.AndAlso(a => a.ConsultType.Contains(ConsultationInfo.ConsultType));
            }
            //服务id
            if (ConsultationInfo.ServiceMoneyListId > 0)
            {
                ConsultationInfo.AndAlso(a => a.ServiceMoneyListId == ConsultationInfo.ServiceMoneyListId);
            }

            // 问题描述
            if (!string.IsNullOrEmpty(ConsultationInfo.IIInessDescription))
            {
                ConsultationInfo.AndAlso(a => a.IIInessDescription.Contains(ConsultationInfo.IIInessDescription));
            }
            // 关键字
            if (!string.IsNullOrEmpty(ConsultationInfo.KeyWord))
            {
                ConsultationInfo.AndAlso(a => a.IIInessDescription.Contains(ConsultationInfo.KeyWord) ||
                                              a.ConsultantName.Contains(ConsultationInfo.KeyWord) ||
                                              a.DoctorName.Contains(ConsultationInfo.KeyWord) ||
                                              a.PatientName.Contains(ConsultationInfo.KeyWord));
            }
            ConsultationInfo.AndAlso(t => t.IsDelete == false);
            // 区分平台来源
            if (ConsultationInfo.Platform == "Mobile")
            {
                ConsultationInfo.AndAlso(t => t.DoctorID == doctor.Id);
            }
            else if (ConsultationInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    ConsultationInfo.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            var values = await _consultationService.YaeherConsultationList(ConsultationInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationList",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "ConsultationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询详情
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationDetail")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationDetail([FromBody]ConsultationIn ConsultationInfo)
        {
            //Logger.Info("开始：" + DateTime.Now + "：" + JsonHelper.ToJson(ConsultationInfo.ConsultNumber));
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            YaeherConsultation values = new YaeherConsultation();
            // 当传入ID时 已ID查询  当已咨询单号查询时查询
            if (ConsultationInfo.Id > 0)
            {
                values = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            }
            else
            {
                values = await _consultationService.YaeherConsultationByNumber(ConsultationInfo.ConsultNumber);
            }
            if (values == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (usermanager.MobileRoleName == "patient" && values.CreatedBy != userid)
            {
                return new ObjectResultModule("", 400, "不允许查看其他患者的订单！");
            }
            if (usermanager.MobileRoleName == "doctor")
            {
                if (ConsultationInfo.Platform == "PC")
                {
                    if (values.DoctorID != usermanager.DoctorID)
                    {
                        return new ObjectResultModule("", 400, "不允许查看其他医生的订单！");
                    }
                }
                else if (ConsultationInfo.Platform == "Mobile")
                {
                    var userdoctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    if (values.DoctorID != userdoctor.Id)
                    {
                        return new ObjectResultModule("", 400, "不允许查看其他医生的订单！");
                    }
                }
                
            }
            var EvaluationIn = new ConsultationEvaluationIn();
            EvaluationIn.AndAlso(t => !t.IsDelete && t.ConsultNumber == values.ConsultNumber);
            // 评分
            var eva = await _consultationEvaluationService.ConsultationEvaluationList(EvaluationIn);
            ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
            consultationReplyIn.ConsultNumber = values.ConsultNumber;
            // 回答与追问
            var replys = await _consultationReplyService.ReplyDetailList(consultationReplyIn);

            PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
            phoneReplyRecordIn.ConsultNumber = values.ConsultNumber;
            // 电话回复
            var phonereplys = await _phoneReplyRecordService.ReplyDetailList(phoneReplyRecordIn);

            var AttachmentInfo = new AttachmentIn() { ConsultNumber = values.ConsultNumber };
            // 附件
            var Attachmentreply = await _attachmentServices.ReplyDetailList(AttachmentInfo);

            var doctor = await _yaeherDoctorService.YaeherDoctorByID(values.DoctorID);
            values.DoctorJSON = JsonHelper.ToJson(doctor);
            var order = await _orderManageService.OrderManageByconsultNumber(values.ConsultNumber);
            var serverid = order.ServiceID;
            var UserResult = await _yaeherUser.YaeherUserByID(doctor.UserID);

            var collect = await _collectConsultationService.CollectConsultationByExpression(t => !t.IsDelete && t.ConsultID == values.Id && t.CreatedBy == userid);

            var questionatt = new List<ReplyDetail>();
            if (Attachmentreply != null && Attachmentreply.Count > 0)
            {
                foreach (var item in replys)
                {
                    var query = from a in Attachmentreply
                                where a.ReplyNumber == item.ReplyNumber
                                select new ConsultationFile
                                {
                                    FileUrl = a.Message,
                                    FileName = a.FileName,
                                    FileSize = a.FileSize,
                                    MediaType = a.Mediatype,
                                    FileContentUrl = a.FileContentAddress,
                                    FileTotalTime = a.FileTotalTime,
                                    Id = a.Id,
                                };
                    item.ConsultationFile = query.ToList();
                }
                questionatt = Attachmentreply.Where(t => t.ConsultNumber == values.ConsultNumber && t.ServiceType == "consultation").ToList();
            }

            if (phonereplys.Count > 0)
            {
                replys = replys.Union(phonereplys).ToList();
            }
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultState");
            var paramlist = await _systemParameterService.ParameterList(param);

            var param1 = new SystemParameterIn() { SystemType = "InquiryMaxCount" };
            var paramlist1 = await _systemParameterService.ParameterList(param1);

            var IIInesslabel = JsonHelper.FromJson<LableManage>(values.IIInessJSON);

            var Refund = await _refundManageService.RefundManageByConsulID(values.Id);

            var Recommenddoctoruser = new YaeherDoctorUser();
            if (values.RecommendDoctorID > 0)
            {
                var doctorin = new YaeherDoctorIn(); doctorin.AndAlso(t => !t.IsDelete && t.Id == values.RecommendDoctorID);
                Recommenddoctoruser = await _yaeherDoctorService.YaeherDoctorUser(doctorin);
            }
            if (ConsultationInfo.Platform == "Mobile" && usermanager.MobileRoleName == "quality")
            {
                var info = new QualityControlManageIn(); info.AndAlso(t => t.ConsultID == values.Id);
                var qualityControlManage = await _QualityControlManageService.QualityControlManageList(info);

                this.ObjectResultModule.Object = new ConsultationDetailOut(values, Recommenddoctoruser, qualityControlManage, Refund, collect == null ? false : true, questionatt, replys, UserResult, eva, serverid, IIInesslabel.Id, paramlist, paramlist1);
            }
            else
            {
                var Canhargeback = false;//创建和咨询中才医生才可以退单
                if (values.ConsultState == "consulting" || values.ConsultState == "created") { Canhargeback = true; }
                if (values.ConsultState == "success")
                {
                    IncomeDevideIn incomein = new IncomeDevideIn();
                    incomein.AndAlso(t => t.IsDelete == false && t.ConsultNumber == values.ConsultNumber);
                    var incomelist = await _incomeDevideService.IncomeDevideList(incomein);
                    var income = incomelist.FirstOrDefault();
                    if (income != null && income.Id > 0)
                    {
                        TimeSpan sp = income.DevideTime.Subtract(DateTime.Now);
                        if (sp.TotalDays > 1)
                        {
                            Canhargeback = true;
                        }
                    }
                }
                this.ObjectResultModule.Object = new ConsultationDetailOut(values, Recommenddoctoruser, Refund, collect == null ? false : true, questionatt, replys, UserResult, eva, serverid, IIInesslabel.Id, Canhargeback, paramlist, paramlist1);
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationDetail",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "ConsultationDetail",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            //检查返回信息
            //Logger.Info("结束：" + DateTime.Now + "：" + JsonHelper.ToJson(ConsultationInfo.ConsultNumber));
            return ObjectResultModule;

        }
        /// <summary>
        /// 咨询管理 删除
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteConsultation([FromBody] YaeherConsultation ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _consultationService.DeleteYaeherConsultation(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteConsultation",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "DeleteConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 咨询管理 查询
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/Consultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> Consultation([FromBody] YaeherConsultation ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (ConsultationInfo.Id > 0)
            {
                var UpdateConsultation = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
                ObjectResultModule.Object = UpdateConsultation;
            }
            else if (!string.IsNullOrEmpty(ConsultationInfo.ConsultNumber))
            {
                var UpdateConsultation = await _consultationService.YaeherConsultationByNumber(ConsultationInfo.ConsultNumber);
                ObjectResultModule.Object = UpdateConsultation;
            }
            ObjectResultModule.StatusCode = 200;
            return ObjectResultModule;
        }
        #endregion

        #region 咨询回答回复
        /// <summary>
        /// 咨询回答 新增
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/CreateConsultationReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateConsultationReply([FromBody] ConsultationReplyAdd ConsultationReplyInfo)
        {
            //Logger.Info("CreateConsultationReply:"+JsonHelper.ToJson(ConsultationReplyInfo));
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var consultation = await _consultationService.YaeherConsultationByID(ConsultationReplyInfo.ConsultID);
            if (consultation == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (consultation.ConsultState == "return") { return new ObjectResultModule("", 400, "退单数据不能回复!"); }

            var param = new SystemParameterIn() { SystemType = "ConsultationSucessTime" };
            var CompleteTimeList = await _systemParameterService.ParameterList(param);
            var CompleteTime = CompleteTimeList.FirstOrDefault();

            param = new SystemParameterIn() { SystemType = "RemindInquiry" };
            var QuestionTimeList = await _systemParameterService.ParameterList(param);
            var QuestionTime = QuestionTimeList.FirstOrDefault();

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateConsultationReply = new ConsultationReply()
                {
                    SequenceNo = ConsultationReplyInfo.SequenceNo == null ? DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6) : ConsultationReplyInfo.SequenceNo,
                    ConsultNumber = consultation.ConsultNumber,
                    ConsultID = consultation.Id,
                    ConsultantID = consultation.ConsultantID,
                    ConsultantName = consultation.ConsultantName,
                    PatientID = consultation.PatientID,
                    PatientName = consultation.PatientName,
                    DoctorName = consultation.DoctorName,
                    DoctorID = consultation.DoctorID,
                    ConsultType = consultation.ConsultType,
                    PatientTelephone = consultation.PhoneNumber,
                    PatientCity = consultation.PatientCity,
                    IllnessDescription = consultation.IIInessDescription,
                    ReplyType = ConsultationReplyInfo.ReplyTypeCode,
                    RepayIllnessDescription = ConsultationReplyInfo.RepayIllnessDescription,
                    ReplyState = ConsultationReplyInfo.ReplyStateCode,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                var result = await _consultationReplyService.CreateConsultationReply(CreateConsultationReply);
                consultation.HasReply = true;

                if (consultation.ConsultState == "created")
                { consultation.ConsultState = "consulting"; }

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
                var paramse = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
                var paramlist = await _systemParameterService.ParameterList(paramse);
                if (ConsultationReplyInfo.Attach != null && ConsultationReplyInfo.Attach.Count > 0)
                {
                    foreach (var AttachmentInfo in ConsultationReplyInfo.Attach)
                    {
                        var att = new AttachmentService();
                        if (!string.IsNullOrEmpty(AttachmentInfo.Filename))
                        {
                            if (AttachmentInfo.MediaType == "voice")
                            {
                                var filedir = Commons.AMRHelperFile;
                                var timestamp = DateTime.Now.ToString("yyyyMMdd");
                                TencentToken tencentoken = new TencentToken();
                                #region 获取WecharToken
                                //  SystemToken systemToken = new SystemToken();
                                // systemToken.TokenType = "Wechar";
                                var Tokens = await _systemTokenService.SystemTokenList("Wechar");
                                #endregion
                                MemoryStream ms = new MemoryStream();
                                var dir = filedir + timestamp + "\\" + AttachmentInfo.Filename + ".amr";
                                try
                                {
                                    var getresult = await MediaApi.GetAsync(Tokens.access_token, AttachmentInfo.Filename, dir);
                                    var filePath = dir;
                                    if (!File.Exists(dir))
                                    {
                                        return new ObjectResultModule("", 400, "录音文件不存在");
                                    }
                                    AMRFileHelper aMRFileHelper = new AMRFileHelper();
                                    aMRFileHelper.ConvertMP3(filedir, timestamp, AttachmentInfo.Filename + ".amr");
                                    var res = aMRFileHelper.result;
                                    string duration = aMRFileHelper.MatchDuration(res.ToString());
                                    TimeSpan ts = TimeSpan.Parse(duration);

                                    var contentkey = paramlist[0].ItemValue + "/answer/voice/" + timestamp + AttachmentInfo.Filename + ".mp3";
                                    var sourcekey = filePath.Replace("amr", "mp3");
                                    using (FileStream fsRead = new FileStream(sourcekey, FileMode.Open))
                                    {
                                        att.FileSize = fsRead.Length;
                                        var putobjectresult = await _cosHandler.PutObjectAsync(contentkey, fsRead);
                                    }
                                    att.FileContentAddress = contentkey;
                                    att.FileAddress = contentkey;
                                    att.FileTotalTime = Math.Round(ts.TotalSeconds, 0);
                                }
                                catch (Exception ex)
                                {
                                    //Logger.Info("音频文件转码失败！"+ex.Message.ToString());
                                    return new ObjectResultModule("", 400, "音频文件转码失败！" + ex.Message.ToString());
                                }
                                att.Filename = AttachmentInfo.Filename;
                            }
                            else
                            {
                                att.Filename = AttachmentInfo.Filename;
                                att.FileAddress = paramlist[0].ItemValue + "/answer/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.Filename;
                            }
                        }
                        if (AttachmentInfo.FileSize > 0) { att.FileSize = AttachmentInfo.FileSize; }
                        att.ConsultID = consultation.Id;
                        att.ServiceID = result.Id;
                        att.ConsultNumber = consultation.ConsultNumber;
                        att.ServiceNumber = result.SequenceNo;
                        att.FileFrom = "answer";
                        att.CreatedBy = userid;
                        att.FileType = AttachmentInfo.MediaType;
                        var resul1 = await _attachmentServices.CreateAttachment(att);
                    }
                }
                #region 当医生回复时系统增加  系统完成Job 并关闭退单job 预警job 添加追问job
                //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == result.ConsultNumber && a.JobSates != "Close" && (a.JobCode == "WarningConsultation" || a.JobCode == "ReturnConsultation")).ToList();

                HangFireJobIn hangFireJobIn = new HangFireJobIn();
                hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == result.ConsultNumber && a.JobSates != "Close" && (a.JobCode == "WarningConsultation" || a.JobCode == "ReturnConsultation"));
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
                if (CanelCount > 0)
                {
                    consultation.Completetime = result.CreatedOn.AddDays(double.Parse(CompleteTime.ItemValue));
                    HangFireJob CompletehangFireJob = new HangFireJob();
                    CompletehangFireJob.JobName = "咨询完成";
                    CompletehangFireJob.JobCode = "CompleteConsultation";
                    CompletehangFireJob.BusinessID = result.Id;
                    CompletehangFireJob.BusinessCode = result.ConsultNumber;
                    CompletehangFireJob.JobRunTime = result.CreatedOn.AddDays(double.Parse(CompleteTime.ItemValue));
                    CompletehangFireJob.JobSates = "Open";
                    CompletehangFireJob.CallbackUrl = Commons.AdminIp + "api/DoHangFire/";
                    CompletehangFireJob.JobParameter = JsonHelper.ToJson(CompletehangFireJob);

                    var ReturnConsult = await _hangFireJobService.CreateHangFireJob(CompletehangFireJob);

                    HangfireScheduleJob job = new HangfireScheduleJob();
                    JobModel model = new JobModel();
                    model.CallbackUrl = CompletehangFireJob.CallbackUrl;//回调URL
                    model.CallbackContent = JsonHelper.ToJson(ReturnConsult);//回调参数

                    model.Timespan = CompletehangFireJob.JobRunTime;//运行时间
                    var returnjobid = job.Schedule(model);
                    if (returnjobid.IndexOf("error") < 0)
                    {
                        ReturnConsult.JobRunID = returnjobid;
                        await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
                    }
                    HangFireJob QuestionFireJob = new HangFireJob();
                    QuestionFireJob.JobName = "咨询追问";
                    QuestionFireJob.JobCode = "RemindInquiry";
                    QuestionFireJob.BusinessID = result.Id;
                    QuestionFireJob.BusinessCode = result.ConsultNumber;
                    QuestionFireJob.JobRunTime = result.CreatedOn.AddDays(double.Parse(CompleteTime.ItemValue)).AddHours(-double.Parse(QuestionTime.ItemValue));
                    QuestionFireJob.JobSates = "Open";
                    QuestionFireJob.CallbackUrl = Commons.AdminIp + "api/DoHangFire/";
                    QuestionFireJob.JobParameter = JsonHelper.ToJson(QuestionFireJob);
                    var questionConsult = await _hangFireJobService.CreateHangFireJob(QuestionFireJob);
                    model.CallbackUrl = QuestionFireJob.CallbackUrl;//回调URL
                    model.CallbackContent = JsonHelper.ToJson(QuestionFireJob);//回调参数

                    model.Timespan = QuestionFireJob.JobRunTime;//运行时间
                    var jobid = job.Schedule(model);
                    if (jobid.IndexOf("error") < 0)
                    {
                        questionConsult.JobRunID = jobid;
                        await _hangFireJobService.UpdateHangFireJob(questionConsult);
                    }
                }
                await _consultationService.UpdateYaeherConsultation(consultation);
                #endregion

                #region  发布咨询 
                Consultation consultations = new Consultation();
                consultations.yaeherConsultation = consultation;
                consultations.consultationReply = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "PatientReply",  // 医生回复消息
                    OperationType = "ReplayConsultant",  //    咨询评分
                    MessageRemark = ConsultationReplyInfo.RepayIllnessDescription,   // 回复消息
                    Publisher = "Doctor",
                    PublishUrl = "Doctor",
                    EventName = "医生回复消息",
                    EventCode = "Reply",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(consultations),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion

                #region 发送消息提醒  医生回复消息 咨询者接受微信提醒 PatientNotice ReplayConsultant  ok
                SendMessageInfo sendMessageInfo = new SendMessageInfo();
                sendMessageInfo.TemplateCode = "PatientReply";
                sendMessageInfo.OperationType = "ReplayConsultant";
                sendMessageInfo.ConsultNumber = Consultationpublishs.BusinessCode;
                await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                #endregion

                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateConsultationReply",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "CreateConsultationReply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            try
            {
                var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回答 修改
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateConsultationReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateConsultationReply([FromBody] ConsultationReply ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var UpdateConsultationReply = await _consultationReplyService.ConsultationReplyByID(ConsultationReplyInfo.Id);
            var consultation = await _consultationService.YaeherConsultationByID(ConsultationReplyInfo.ConsultID);
            if (UpdateConsultationReply != null)
            {
                UpdateConsultationReply.ConsultNumber = ConsultationReplyInfo.ConsultNumber;
                UpdateConsultationReply.ConsultID = ConsultationReplyInfo.ConsultID;
                UpdateConsultationReply.ConsultantID = ConsultationReplyInfo.ConsultantID;
                UpdateConsultationReply.ConsultantName = ConsultationReplyInfo.ConsultantName;
                UpdateConsultationReply.PatientID = ConsultationReplyInfo.PatientID;
                UpdateConsultationReply.PatientName = ConsultationReplyInfo.PatientName;
                UpdateConsultationReply.DoctorName = ConsultationReplyInfo.DoctorName;
                UpdateConsultationReply.DoctorID = ConsultationReplyInfo.DoctorID;
                UpdateConsultationReply.ConsultType = ConsultationReplyInfo.ConsultType;
                UpdateConsultationReply.PatientTelephone = ConsultationReplyInfo.PatientTelephone;
                UpdateConsultationReply.PatientCity = ConsultationReplyInfo.PatientCity;
                UpdateConsultationReply.IllnessDescription = ConsultationReplyInfo.IllnessDescription;
                UpdateConsultationReply.ReplyType = ConsultationReplyInfo.ReplyType;
                UpdateConsultationReply.RepayIllnessDescription = ConsultationReplyInfo.RepayIllnessDescription;
                UpdateConsultationReply.ReplyState = ConsultationReplyInfo.ReplyState;
                UpdateConsultationReply.ModifyOn = DateTime.Now;
                UpdateConsultationReply.ModifyBy = userid;

                var result = await _consultationReplyService.UpdateConsultationReply(UpdateConsultationReply);
                #region  咨询回答回复
                Consultation consultations = new Consultation();
                consultations.yaeherConsultation = consultation;
                consultations.consultationReply = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "PatientReply",  // 医生回复消息
                    OperationType = "ReplayConsultant",  //    咨询评分
                    MessageRemark = UpdateConsultationReply.RepayIllnessDescription,   // 回复消息
                    Publisher = "Doctor",
                    PublishUrl = "Doctor",
                    EventName = "咨询回答回复",
                    EventCode = "Reply",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(consultations),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateConsultationReply",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "UpdateConsultationReply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回答 删除
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteConsultationReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteConsultationReply([FromBody] ConsultationReply ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var query = await _consultationReplyService.ConsultationReplyByID(ConsultationReplyInfo.Id);
            var consultation = await _consultationService.YaeherConsultationByID(ConsultationReplyInfo.ConsultID);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _consultationReplyService.DeleteConsultationReply(query);
                #region  咨询回答回复
                Consultation consultations = new Consultation();
                consultations.yaeherConsultation = consultation;
                consultations.consultationReply = res;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "PatientReply",  // 医生回复消息
                    OperationType = "ReplayConsultant",  //    咨询评分
                    Publisher = "Doctor",
                    PublishUrl = "Doctor",
                    EventName = "咨询回答回复",
                    EventCode = "Reply",
                    BusinessID = res.Id.ToString(),
                    BusinessCode = res.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(consultations),
                    PublishedTime = res.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteConsultationReply",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "DeleteConsultationReply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询回答 Page
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyPage([FromBody]ConsultationReplyIn ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationReplyInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationReplyService.ConsultationReplyPage(ConsultationReplyInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ConsultationReplyOut(values, ConsultationReplyInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationReplyPage",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "ConsultationReplyPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询回答 List 
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyList([FromBody]ConsultationReplyIn ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationReplyInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationReplyService.ConsultationReplyList(ConsultationReplyInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationReplyList",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "ConsultationReplyList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回答 Byid
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyById([FromBody]ConsultationReplyIn ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _consultationReplyService.ConsultationReplyByID(ConsultationReplyInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationReplyById",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "ConsultationReplyById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 咨询评分
        /// <summary>
        /// 咨询评分 新增
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/CreateConsultationEvaluation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateConsultationEvaluation([FromBody] ConsultationEvaluation ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateConsultationEvaluation = new ConsultationEvaluation()
            {
                SequenceNo = ConsultationEvaluationInfo.SequenceNo == null ? DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6) : ConsultationEvaluationInfo.SequenceNo,
                ConsultNumber = ConsultationEvaluationInfo.ConsultNumber,
                ConsultID = ConsultationEvaluationInfo.ConsultID,
                ConsultantID = ConsultationEvaluationInfo.ConsultantID,
                ConsultantName = ConsultationEvaluationInfo.ConsultantName,
                PatientID = ConsultationEvaluationInfo.PatientID,
                PatientName = ConsultationEvaluationInfo.PatientName,
                DoctorName = ConsultationEvaluationInfo.DoctorName,
                DoctorID = ConsultationEvaluationInfo.DoctorID,
                EvaluateContent = ConsultationEvaluationInfo.EvaluateContent,
                EvaluateReason = ConsultationEvaluationInfo.EvaluateReason,
                EvaluateLevel = ConsultationEvaluationInfo.EvaluateLevel,
                QualityLevel = ConsultationEvaluationInfo.QualityLevel,
                IsQuality = ConsultationEvaluationInfo.IsQuality,
                QualityReason = ConsultationEvaluationInfo.QualityReason,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
            };
            var result = await _consultationEvaluationService.CreateConsultationEvaluation(CreateConsultationEvaluation);
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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateConsultationEvaluation",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "CreateConsultationEvaluation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 质控打分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateConsultationEvaluation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateConsultationEvaluation([FromBody] ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var UpdateConsultationEvaluation = await _consultationEvaluationService.ConsultationEvaluationByID(ConsultationEvaluationInfo.Id);
            if (usermanager.MobileRoleName == "quality" || usermanager.IsQC)
            {
                var comtrol = await _QualityControlManageService.QualityControlManageByConsultStateID(UpdateConsultationEvaluation.ConsultID);
                if (comtrol != null) { return new ObjectResultModule("", 400, "质控委员未处理完不允许质控重新分发质控委员！"); }
                if (ConsultationEvaluationInfo.QualityLevel > 0)
                {
                    UpdateConsultationEvaluation.QualityLevel = ConsultationEvaluationInfo.QualityLevel;
                }
                if (!string.IsNullOrEmpty(ConsultationEvaluationInfo.QualityReason))
                {
                    UpdateConsultationEvaluation.QualityReason = ConsultationEvaluationInfo.QualityReason;
                }
                UpdateConsultationEvaluation.IsQuality = true;
                var result = await _consultationEvaluationService.UpdateConsultationEvaluation(UpdateConsultationEvaluation);

                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";

            }
            else
            {
                return new ObjectResultModule("", 400, "只有质控可以修改评分，请联系质控！");
            }
            //else
            //{
            //    if (UpdateConsultationEvaluation != null)
            //    {
            //        UpdateConsultationEvaluation.EvaluateContent = ConsultationEvaluationInfo.EvaluateContent;
            //        UpdateConsultationEvaluation.EvaluateReason = ConsultationEvaluationInfo.EvaluateReason;
            //        UpdateConsultationEvaluation.EvaluateLevel = ConsultationEvaluationInfo.EvaluateLevel;
            //        UpdateConsultationEvaluation.QualityLevel = ConsultationEvaluationInfo.QualityLevel;
            //        UpdateConsultationEvaluation.IsQuality = ConsultationEvaluationInfo.IsQuality;
            //        UpdateConsultationEvaluation.QualityReason = ConsultationEvaluationInfo.QualityReason;
            //        UpdateConsultationEvaluation.ModifyOn = DateTime.Now;
            //        UpdateConsultationEvaluation.ModifyBy = userid;

            //        var result = await _consultationEvaluationService.UpdateConsultationEvaluation(UpdateConsultationEvaluation);

            //        this.ObjectResultModule.Object = result;
            //        this.ObjectResultModule.StatusCode = 200;
            //        this.ObjectResultModule.Message = "success";
            //    }
            //    else
            //    {
            //        this.ObjectResultModule.Object = "";
            //        this.ObjectResultModule.StatusCode = 404;
            //        this.ObjectResultModule.Message = "NotFound";
            //    }
            //}
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateConsultationEvaluation",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "UpdateConsultationEvaluation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询评分 删除
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteConsultationEvaluation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteConsultationEvaluation([FromBody] ConsultationEvaluation ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _consultationEvaluationService.ConsultationEvaluationByID(ConsultationEvaluationInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _consultationEvaluationService.DeleteConsultationEvaluation(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteConsultationEvaluation",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "DeleteConsultationEvaluation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 我的咨询评分 Page
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationPage([FromBody]ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationEvaluationInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationEvaluationInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationEvaluationInfo.EndTime))
                {
                    ConsultationEvaluationInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationEvaluationInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationEvaluationInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationEvaluationInfo.StartTime))
            {
                ConsultationEvaluationInfo.AndAlso(t => t.CreatedOn >= StartTime);
                ConsultationEvaluationInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            ConsultationEvaluationInfo.AndAlso(t => t.DoctorID == doctor.Id);
            ConsultationEvaluationInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationEvaluationService.ConsultationEvaluationPage(ConsultationEvaluationInfo);

            this.ObjectResultModule.Object = new ConsultationEvaluationOut(values, ConsultationEvaluationInfo);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationEvaluationPage",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 咨询评分个人分数接单数 Page
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationDetail")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationDetail([FromBody]SecretModel ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            var doctorView = new doctorCountView();
            // 区分平台来源
            if (usermanager.MobileRoleName == "doctor" || usermanager.IsDoctor)
            {
                var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                var eval = new EvaluationTotalIn();

                DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime EndTime = StartTime.AddDays(1);
                eval.AndAlso(t => !t.IsDelete);
                eval.AndAlso(t => t.DoctorID == doctor.Id);
                eval.AndAlso(a => (a.CreatedOn >= StartTime));
                eval.AndAlso(a => (a.CreatedOn < EndTime));
                var eva = await _evaluationTotalService.EvaluationTotalList(eval);
                var evaluationTotal = eva.FirstOrDefault();
                doctorView.ReceiptNumBer = eva.Count < 1 ? 0 : eva[0].OrderTotal;//接单数
                if (evaluationTotal != null)
                {
                    doctorView.ReceiptNumBer = evaluationTotal.CompleteTotal;//接单数
                    doctorView.AverageTime = evaluationTotal.AverageAnswer.ToString();//平均时长                    //  item.DoctorLevel = evaluationTotal == null ? "0" : evaluationTotal.AverageEvaluate.ToString();//星级
                    var EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
                    doctorView.AverageEvaluate = 0;
                    if (EvaluationCount >= 15)
                    { doctorView.AverageEvaluate = evaluationTotal.AverageEvaluate; }//星级
                }

            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            this.ObjectResultModule.Object = doctorView;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationEvaluationDetail",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationDetail",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询评分 List 
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationList([FromBody]ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationEvaluationInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationEvaluationService.ConsultationEvaluationList(ConsultationEvaluationInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationEvaluationList",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询评分详情
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationById([FromBody]ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            ConsultationEvaluation values = new ConsultationEvaluation();
            if (ConsultationEvaluationInfo.Id > 0)
            {
                values = await _consultationEvaluationService.ConsultationEvaluationByID(ConsultationEvaluationInfo.Id);
            }
            else
            {
                values = await _consultationEvaluationService.ConsultationEvaluationByNumber(ConsultationEvaluationInfo.ConsultNumber);
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var idarr = new YaeherUserIDArray() { IDArray = values.DoctorID.ToString() };
            var user = await _yaeherUser.YaeherUserListByArray(idarr.IDArray);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var result = new ConsultationEvaluationDetail(values, user[0]);
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationEvaluationById",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 电话回复记录回答
        /// <summary>
        /// 电话回复记录 新增
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/CreatePhoneReplyRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreatePhoneReplyRecord([FromBody] PhoneReplyRecord PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var User = await _yaeherUser.YaeherUserByID(userid);
            var secret = await CreateSecret();
            var consultation = await _consultationService.YaeherConsultationByID(PhoneReplyRecordInfo.ConsultID);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var CreatePhoneReplyRecord = new PhoneReplyRecord()
            {
                SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                ConsultNumber = consultation.ConsultNumber,
                ConsultID = consultation.Id,
                ConsultantID = consultation.ConsultantID,
                ConsultantName = consultation.ConsultantName,
                PatientID = consultation.PatientID,
                PatientName = consultation.PatientName,
                DoctorID = consultation.DoctorID,
                DoctorName = consultation.DoctorName,
                CallTime = DateTime.Now,
                CallDuration = PhoneReplyRecordInfo.CallDuration,
                CallIntro = "",
                RecordAddress = "",
                Callee = PhoneReplyRecordInfo.Callee,
                Caller = PhoneReplyRecordInfo.Caller,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            #region  拨打电话
            AliCallCenter aliCallCenter = new AliCallCenter();
            PhoneReplyRecordInfo.Caller = PhoneReplyRecordInfo.Caller.Replace(" ", "");
            PhoneReplyRecordInfo.Callee = PhoneReplyRecordInfo.Callee.Replace(" ", "");
            if (PhoneReplyRecordInfo.Caller.Length != 11)
            {
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "请使用正确的电话号码拨打。";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (PhoneReplyRecordInfo.Callee.Length != 11)
            {
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "咨询者电话号码不正确，请联系客服。";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var yaeherPhone = new YaeherPhone()
            {
                UserID = userid,
                UserName =User.LoginName,
                Caller = PhoneReplyRecordInfo.Caller,
                Callee = PhoneReplyRecordInfo.Callee,
                CallCenterNumber = "075526788591",   // 公司呼叫电话
                StatusCode = "",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var StatusCode = await aliCallCenter.StartBack2BackCall(yaeherPhone);
            #endregion
            if (StatusCode == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
                return ObjectResultModule;
            }
            else
            {
                yaeherPhone.StatusCode = StatusCode;
                yaeherPhone.CallCenterNumber = "075526788591";
                var Phoneresult = await _yaeherPhoneService.CreateYaeherPhone(yaeherPhone);
                #region 电话回复修改主表
                consultation.HasReply = true;
                if (consultation.ConsultState == "created")
                { consultation.ConsultState = "consulting"; }
                await _consultationService.UpdateYaeherConsultation(consultation);
                #endregion
                var result = await _phoneReplyRecordService.CreatePhoneReplyRecord(CreatePhoneReplyRecord);
                if (result.Id > 0)
                {
                    #region  咨询回答电话回复
                    Consultation consultations = new Consultation();
                    consultations.yaeherConsultation = consultation;
                    consultations.phoneReplyRecord = result;
                    Publishs Consultationpublishs = new Publishs()
                    {
                        TemplateCode = "ConsultationNotice",
                        OperationType = "PhoneReplay",  //    咨询评分
                        MessageRemark = consultation.DoctorName + "医生 :电话回复时间" + DateTime.Now.ToString("yyyy-MM-dd hh:MM"),   // 回复消息
                        Publisher = "Doctor",
                        PublishUrl = "Doctor",
                        EventName = "咨询回答电话回复",
                        EventCode = "Reply",
                        BusinessID = result.Id.ToString(),
                        BusinessCode = result.ConsultNumber,
                        BusinessJSON = JsonHelper.ToJson(consultations),
                        PublishedTime = result.CreatedOn,
                        PublishStatus = true,
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        Secret = secret,
                    };
                    var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                    var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                    if (ConsultationJson != null)
                    {
                        Consultationpublishs.PublishStatus = true;
                    }
                    else
                    {
                        Consultationpublishs.PublishStatus = false;
                    }
                    Consultationpublishs.ServerClient = "Client";
                    var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                    #endregion

                    #region 当医生回复时系统增加  系统完成Job 并关闭退单job 预警job 
                    //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                    //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == result.ConsultNumber).ToList();

                    HangFireJobIn hangFireJobIn = new HangFireJobIn();
                    hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == result.ConsultNumber && a.JobSates != "Close" && (a.JobCode == "WarningConsultation" || a.JobCode == "ReturnConsultation"));
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
                        }
                    }
                    if (CanelCount > 0)
                    {
                        var param = new SystemParameterIn() { SystemCode = "ConsultationSucessTime", SystemType = "ConsultationSucessTime" };
                        var CompleteTimeList = await _systemParameterService.ParameterList(param);
                        var CompleteTime = CompleteTimeList.FirstOrDefault();
                        HangFireJob CompletehangFireJob = new HangFireJob();
                        CompletehangFireJob.JobName = "咨询完成";
                        CompletehangFireJob.JobCode = "CompleteConsultation";
                        CompletehangFireJob.BusinessID = result.Id;
                        CompletehangFireJob.BusinessCode = result.ConsultNumber;
                        CompletehangFireJob.JobRunTime = result.CreatedOn.AddDays(double.Parse(CompleteTime.ItemValue));
                        CompletehangFireJob.JobSates = "Open";
                        CompletehangFireJob.CallbackUrl = Commons.AdminIp + "api/DoHangFire/";
                        CompletehangFireJob.JobParameter = JsonHelper.ToJson(CompletehangFireJob);
                        var ReturnConsult = await _hangFireJobService.CreateHangFireJob(CompletehangFireJob);
                        //使用hangfire更新已有job
                        HangfireScheduleJob job = new HangfireScheduleJob();
                        JobModel model = new JobModel();
                        model.CallbackUrl = CompletehangFireJob.CallbackUrl;//回调URL
                        model.CallbackContent = JsonHelper.ToJson(ReturnConsult);//回调参数
                        model.Timespan = CompletehangFireJob.JobRunTime;//运行时间
                        var returnjobid = job.Schedule(model);
                        if (returnjobid.IndexOf("error") < 0)
                        {
                            ReturnConsult.JobRunID = returnjobid;
                            await _hangFireJobService.UpdateHangFireJob(ReturnConsult);
                        }
                    }
                    #endregion
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
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreatePhoneReplyRecord",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "CreatePhoneReplyRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 电话回复记录 修改
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/UpdatePhoneReplyRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdatePhoneReplyRecord([FromBody] PhoneReplyRecord PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdatePhoneReplyRecord = await _phoneReplyRecordService.PhoneReplyRecordByID(PhoneReplyRecordInfo.Id);
            var consultation = await _consultationService.YaeherConsultationByID(PhoneReplyRecordInfo.ConsultID);
            if (UpdatePhoneReplyRecord != null)
            {
                UpdatePhoneReplyRecord.ConsultNumber = PhoneReplyRecordInfo.ConsultNumber;
                UpdatePhoneReplyRecord.ConsultID = PhoneReplyRecordInfo.ConsultID;
                UpdatePhoneReplyRecord.ConsultantID = PhoneReplyRecordInfo.ConsultantID;
                UpdatePhoneReplyRecord.ConsultantName = PhoneReplyRecordInfo.ConsultantName;
                UpdatePhoneReplyRecord.PatientID = PhoneReplyRecordInfo.PatientID;
                UpdatePhoneReplyRecord.PatientName = PhoneReplyRecordInfo.PatientName;
                UpdatePhoneReplyRecord.DoctorID = PhoneReplyRecordInfo.DoctorID;
                UpdatePhoneReplyRecord.DoctorName = PhoneReplyRecordInfo.DoctorName;
                UpdatePhoneReplyRecord.CallTime = PhoneReplyRecordInfo.CallTime;
                UpdatePhoneReplyRecord.CallDuration = PhoneReplyRecordInfo.CallDuration;
                UpdatePhoneReplyRecord.CallIntro = PhoneReplyRecordInfo.CallIntro;
                UpdatePhoneReplyRecord.RecordAddress = PhoneReplyRecordInfo.RecordAddress;
                UpdatePhoneReplyRecord.CreatedBy = PhoneReplyRecordInfo.CreatedBy;
                UpdatePhoneReplyRecord.CreatedOn = PhoneReplyRecordInfo.CreatedOn;
                UpdatePhoneReplyRecord.ModifyOn = DateTime.Now;
                UpdatePhoneReplyRecord.ModifyBy = userid;

                var result = await _phoneReplyRecordService.UpdatePhoneReplyRecord(UpdatePhoneReplyRecord);

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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdatePhoneReplyRecord",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "UpdatePhoneReplyRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 电话回复记录 删除
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DeletePhoneReplyRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeletePhoneReplyRecord([FromBody] PhoneReplyRecord PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _phoneReplyRecordService.PhoneReplyRecordByID(PhoneReplyRecordInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _phoneReplyRecordService.DeletePhoneReplyRecord(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeletePhoneReplyRecord",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "DeletePhoneReplyRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 电话回复记录 Page
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/PhoneReplyRecordPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PhoneReplyRecordPage([FromBody]PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PhoneReplyRecordInfo.AndAlso(t => !t.IsDelete);
            var values = await _phoneReplyRecordService.PhoneReplyRecordPage(PhoneReplyRecordInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new PhoneReplyRecordOut(values, PhoneReplyRecordInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "PhoneReplyRecordPage",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "PhoneReplyRecordPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 电话回复记录 List 
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/PhoneReplyRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PhoneReplyRecordList([FromBody]PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PhoneReplyRecordInfo.AndAlso(t => t.ConsultNumber == PhoneReplyRecordInfo.ConsultNumber);
            PhoneReplyRecordInfo.AndAlso(t => !t.IsDelete);
            var values = await _phoneReplyRecordService.PhoneReplyRecordList(PhoneReplyRecordInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "PhoneReplyRecordList",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "PhoneReplyRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 电话回复记录 Byid
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/PhoneReplyRecordById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PhoneReplyRecordById([FromBody]PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _phoneReplyRecordService.PhoneReplyRecordByID(PhoneReplyRecordInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "PhoneReplyRecordById",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "PhoneReplyRecordById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单管理
        /// <summary>
        /// 订单管理 新增
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateOrderManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateOrderManage([FromBody] OrderManage OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateOrderManage = new OrderManage()
            {
                SequenceNo = OrderManageInfo.SequenceNo == null ? DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6) : OrderManageInfo.SequenceNo,
                OrderNumber = OrderManageInfo.OrderNumber,
                ConsultNumber = OrderManageInfo.ConsultNumber,
                ConsultID = OrderManageInfo.ConsultID,
                ConsultType = OrderManageInfo.ConsultType,
                ConsultantID = OrderManageInfo.ConsultantID,
                ConsultantName = OrderManageInfo.ConsultantName,
                PatientID = OrderManageInfo.PatientID,
                PatientName = OrderManageInfo.PatientName,
                DoctorName = OrderManageInfo.DoctorName,
                DoctorID = OrderManageInfo.DoctorID,
                OrderCurrency = OrderManageInfo.OrderCurrency,
                OrderMoney = OrderManageInfo.OrderMoney,
                ReceivablesType = OrderManageInfo.ReceivablesType,
                ReceivablesNumber = OrderManageInfo.ReceivablesNumber,
                ServiceID = OrderManageInfo.ServiceID,
                ServiceName = OrderManageInfo.ServiceName,
                SellerMoneyID = OrderManageInfo.SellerMoneyID,
                TradeType = OrderManageInfo.TradeType,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _orderManageService.CreateOrderManage(CreateOrderManage);
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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateOrderManage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "CreateOrderManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 修改
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateOrderManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateOrderManage([FromBody] OrderManage OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateOrderManage = await _orderManageService.OrderManageByID(OrderManageInfo.Id);
            if (UpdateOrderManage != null)
            {
                UpdateOrderManage.OrderNumber = OrderManageInfo.OrderNumber;
                UpdateOrderManage.ConsultNumber = OrderManageInfo.ConsultNumber;
                UpdateOrderManage.ConsultID = OrderManageInfo.ConsultID;
                UpdateOrderManage.ConsultType = OrderManageInfo.ConsultType;
                UpdateOrderManage.ConsultantID = OrderManageInfo.ConsultantID;
                UpdateOrderManage.ConsultantName = OrderManageInfo.ConsultantName;
                UpdateOrderManage.PatientID = OrderManageInfo.PatientID;
                UpdateOrderManage.PatientName = OrderManageInfo.PatientName;
                UpdateOrderManage.DoctorName = OrderManageInfo.DoctorName;
                UpdateOrderManage.DoctorID = OrderManageInfo.DoctorID;
                UpdateOrderManage.OrderCurrency = OrderManageInfo.OrderCurrency;
                UpdateOrderManage.OrderMoney = OrderManageInfo.OrderMoney;
                UpdateOrderManage.ReceivablesType = OrderManageInfo.ReceivablesType;
                UpdateOrderManage.ReceivablesNumber = OrderManageInfo.ReceivablesNumber;
                UpdateOrderManage.ServiceID = OrderManageInfo.ServiceID;
                UpdateOrderManage.ServiceName = OrderManageInfo.ServiceName;
                UpdateOrderManage.SellerMoneyID = OrderManageInfo.SellerMoneyID;
                UpdateOrderManage.TradeType = OrderManageInfo.TradeType;
                UpdateOrderManage.ModifyOn = DateTime.Now;
                UpdateOrderManage.ModifyBy = userid;

                var result = await _orderManageService.UpdateOrderManage(UpdateOrderManage);

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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateOrderManage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "UpdateOrderManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 删除
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteOrderManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteOrderManage([FromBody] OrderManage OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _orderManageService.OrderManageByID(OrderManageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _orderManageService.DeleteOrderManage(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteOrderManage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "DeleteOrderManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单管理 Page
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/OrderManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderManagePage([FromBody]OrderManageIn OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderManageInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderManageInfo.StartTime);
                if (string.IsNullOrEmpty(OrderManageInfo.EndTime))
                {
                    OrderManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderManageInfo.StartTime))
            {
                OrderManageInfo.AndAlso(t => t.CreatedOn >= StartTime);
                OrderManageInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            //咨询类型
            if (!string.IsNullOrEmpty(OrderManageInfo.ConsultType))
            {
                OrderManageInfo.AndAlso(a => a.ConsultType.Contains(OrderManageInfo.ConsultType));
            }
            // 问题描述
            if (!string.IsNullOrEmpty(OrderManageInfo.OrderNumber))
            {
                OrderManageInfo.AndAlso(a => a.OrderNumber.Contains(OrderManageInfo.OrderNumber));
            }
            // 关键字
            if (!string.IsNullOrEmpty(OrderManageInfo.KeyWord))
            {
                OrderManageInfo.AndAlso(a => a.ConsultantName.Contains(OrderManageInfo.KeyWord) ||
                                              a.DoctorName.Contains(OrderManageInfo.KeyWord) ||
                                              a.PatientName.Contains(OrderManageInfo.KeyWord) ||
                                              a.ConsultNumber.Contains(OrderManageInfo.KeyWord));
            }
            // 区分平台来源
            if (OrderManageInfo.Platform == "Mobile")
            {
                OrderManageInfo.AndAlso(t => t.DoctorID == doctor.Id);
            }
            else if (OrderManageInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    OrderManageInfo.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            var values = await _orderManageService.OrderManagePage(OrderManageInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new OrderManageOut(values, OrderManageInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderManagePage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "OrderManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 订单统计管理 Page（带上退单）医生端
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/TotalOrderManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> TotalOrderManagePage([FromBody]ConsultationIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderTradeRecordInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (usermanager.MobileRoleName != "doctor") { return new ObjectResultModule("", 400, "医生角色才允许查看收入列表,请联系管理员！"); }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderTradeRecordInfo.StartTime);
                if (string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
                {
                    OrderTradeRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderTradeRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                OrderTradeRecordInfo.AndAlso(t => t.Completetime >= StartTime);
                OrderTradeRecordInfo.AndAlso(t => t.Completetime < EndTime.AddDays(+1));
            }
            //// 区分平台来源
            //if (OrderTradeRecordInfo.Platform == "Mobile")
            //{
                OrderTradeRecordInfo.DoctorID = doctor.Id;
            //}
            OrderTradeRecordInfo.AndAlso(t=>t.ConsultState=="success");
            var values = await _orderManageService.TotalOrderManagePage(OrderTradeRecordInfo);
            this.ObjectResultModule.Object = new OrderManageTotalOut(values, OrderTradeRecordInfo);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderManagePage",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 List 
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/OrderManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderManageList([FromBody]OrderManageIn OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderManageInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderManageInfo.StartTime);
                if (string.IsNullOrEmpty(OrderManageInfo.EndTime))
                {
                    OrderManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderManageInfo.StartTime))
            {
                OrderManageInfo.AndAlso(a => a.CreatedOn >= StartTime);
                OrderManageInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            //咨询类型
            if (!string.IsNullOrEmpty(OrderManageInfo.ConsultType))
            {
                OrderManageInfo.AndAlso(a => a.ConsultType.Contains(OrderManageInfo.ConsultType));
            }
            // 问题描述
            if (!string.IsNullOrEmpty(OrderManageInfo.OrderNumber))
            {
                OrderManageInfo.AndAlso(a => a.OrderNumber.Contains(OrderManageInfo.OrderNumber));
            }
            // 关键字
            if (!string.IsNullOrEmpty(OrderManageInfo.KeyWord))
            {
                OrderManageInfo.AndAlso(a => a.ConsultantName.Contains(OrderManageInfo.KeyWord) ||
                                              a.DoctorName.Contains(OrderManageInfo.KeyWord) ||
                                              a.PatientName.Contains(OrderManageInfo.KeyWord));
            }
            if (OrderManageInfo.DoctorID > 0)
            {
                OrderManageInfo.AndAlso(a => a.DoctorID == OrderManageInfo.DoctorID);
            }
            // 区分平台来源
            if (OrderManageInfo.Platform == "Mobile" && usermanager.MobileRoleName == "doctor")
            {
                OrderManageInfo.AndAlso(a => a.DoctorID == doctor.Id);
            }
            else if (OrderManageInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    OrderManageInfo.AndAlso(a => a.DoctorID == doctor.Id);
                }
            }
            var values = await _orderManageService.OrderManageList(OrderManageInfo);

            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderManageList",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "OrderManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 Byid
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/OrderManageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderManageById([FromBody]OrderManageIn OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _orderManageService.OrderManageByID(OrderManageInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderManageById",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "OrderManageById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单交易记录
        /// <summary>
        /// 订单交易记录 新增
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/CreateOrderTradeRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateOrderTradeRecord([FromBody] OrderTradeRecord OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateOrderTradeRecord = new OrderTradeRecord()
            {
                SequenceNo = OrderTradeRecordInfo.SequenceNo == null ? DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6) : OrderTradeRecordInfo.SequenceNo,
                OrderID = OrderTradeRecordInfo.OrderID,
                OrderNumber = OrderTradeRecordInfo.OrderNumber,
                PayType = OrderTradeRecordInfo.PayType,
                OrderCurrency = OrderTradeRecordInfo.OrderCurrency,
                TenpayNumber = OrderTradeRecordInfo.TenpayNumber,
                VoucherNumber = OrderTradeRecordInfo.VoucherNumber,
                VoucherJSON = OrderTradeRecordInfo.VoucherJSON,
                PayMoney = OrderTradeRecordInfo.PayMoney,
                PayAchiveTime = OrderTradeRecordInfo.PayAchiveTime,
                PaySerialNumber = OrderTradeRecordInfo.PaySerialNumber,
                PaymentState = OrderTradeRecordInfo.PaymentState,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _orderTradeRecordService.CreateOrderTradeRecord(CreateOrderTradeRecord);
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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateOrderTradeRecord",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "CreateOrderTradeRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单交易记录 修改
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateOrderTradeRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateOrderTradeRecord([FromBody] OrderTradeRecord OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var UpdateOrderTradeRecord = await _orderTradeRecordService.OrderTradeRecordByID(OrderTradeRecordInfo.Id);
            if (UpdateOrderTradeRecord != null)
            {
                UpdateOrderTradeRecord.OrderID = OrderTradeRecordInfo.OrderID;
                UpdateOrderTradeRecord.OrderNumber = OrderTradeRecordInfo.OrderNumber;
                UpdateOrderTradeRecord.PayType = OrderTradeRecordInfo.PayType;
                UpdateOrderTradeRecord.OrderCurrency = OrderTradeRecordInfo.OrderCurrency;
                UpdateOrderTradeRecord.TenpayNumber = OrderTradeRecordInfo.TenpayNumber;
                UpdateOrderTradeRecord.VoucherNumber = OrderTradeRecordInfo.VoucherNumber;
                UpdateOrderTradeRecord.VoucherJSON = OrderTradeRecordInfo.VoucherJSON;
                UpdateOrderTradeRecord.PayMoney = OrderTradeRecordInfo.PayMoney;
                UpdateOrderTradeRecord.PayAchiveTime = OrderTradeRecordInfo.PayAchiveTime;
                UpdateOrderTradeRecord.PaySerialNumber = OrderTradeRecordInfo.PaySerialNumber;
                UpdateOrderTradeRecord.PaymentState = OrderTradeRecordInfo.PaymentState;
                UpdateOrderTradeRecord.ModifyOn = DateTime.Now;
                UpdateOrderTradeRecord.ModifyBy = userid;

                var result = await _orderTradeRecordService.UpdateOrderTradeRecord(UpdateOrderTradeRecord);

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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateOrderTradeRecord",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "UpdateOrderTradeRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单交易记录 删除
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteOrderTradeRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteOrderTradeRecord([FromBody] OrderTradeRecord OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _orderTradeRecordService.OrderTradeRecordByID(OrderTradeRecordInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _orderTradeRecordService.DeleteOrderTradeRecord(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteOrderTradeRecord",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "DeleteOrderTradeRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单交易记录 Page
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTradeRecordPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTradeRecordPage([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderTradeRecordInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderTradeRecordInfo.StartTime);
                if (string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
                {
                    OrderTradeRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderTradeRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                OrderTradeRecordInfo.AndAlso(a => a.CreatedOn >= StartTime);
                OrderTradeRecordInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            //订单编号
            //if (!string.IsNullOrEmpty(OrderTradeRecordInfo.KeyWord))
            //{
            //    OrderTradeRecordInfo.AndAlso(a => a.OrderNumber.Contains(OrderTradeRecordInfo.KeyWord) || 
            //                                      a.WXPayBillno.Contains(OrderTradeRecordInfo.KeyWord) ||
            //                                      a.WXTransactionId.Contains(OrderTradeRecordInfo.KeyWord) );
            //}
            var values = await _orderTradeRecordService.PCOrderTradeRecordPage(OrderTradeRecordInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new OrderTradeRecordPCModuleOut(values, OrderTradeRecordInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTradeRecordPage",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单交易记录 List 
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTradeRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTradeRecordList([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderTradeRecordInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderTradeRecordInfo.StartTime);
                if (string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
                {
                    OrderTradeRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderTradeRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                OrderTradeRecordInfo.AndAlso(a => a.CreatedOn >= StartTime && a.CreatedOn < EndTime.AddDays(+1));
            }
            var values = await _orderTradeRecordService.OrderTradeRecordList(OrderTradeRecordInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTradeRecordList",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单交易记录 Byid
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTradeRecordById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTradeRecordById([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _orderTradeRecordService.OrderTradeRecordByID(OrderTradeRecordInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTradeRecordById",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单退单管理
        /// <summary>
        /// 获取退单理由
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorRefundManageType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRefundManageType([FromBody] RefundManageType RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorRefundManageType");
            var paramlist = await _systemParameterService.ParameterList(param);
            var typelist = new List<RefundManageType>();
            foreach (var item in paramlist)
            {
                var newcode = new RefundManageType() { Code = item.Code, Type = item.Name };
                typelist.Add(newcode);
            }
            //var typelist = new List<RefundManageType>()
            //{
            //    new RefundManageType(){Id=1,Code="others",Type="其他" },
            //    new RefundManageType(){Id=2,Code="notmatch",Type="病历与标签不对应" },
            //};
            this.ObjectResultModule.Object = typelist;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorRefundManageType",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "DoctorRefundManageType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 订单退单管理 新增
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateRefundManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateRefundManage([FromBody] RefundManageDoctorInfo RefundManageInfo)
        {
            // Logger.Info("RefundManageInfo:" + JsonHelper.ToJson(RefundManageInfo));
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            // 查询咨询单
            var yaeherConsultation = await _consultationService.YaeherConsultationByID(RefundManageInfo.ConsultID);
            // 查询订单
            var Order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);
            if (Order == null) { return new ObjectResultModule("", 204, "NoContent"); }
            // 查询交易记录
            var ordertrade = await _orderTradeRecordService.OrderTradeRecordExpress(t => !t.IsDelete && t.OrderID == Order.Id && t.PaymentSourceCode == "order");
            if (ordertrade == null) { return new ObjectResultModule("", 204, "NoContent"); }
            // 查询咨询用户
            var Consultant = await _yaeherUser.YaeherUserByID(yaeherConsultation.CreatedBy);
            if (Consultant == null) { return new ObjectResultModule("", 204, "NoContent"); }

            // 查询患者信息 
            var Patient = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(yaeherConsultation.PatientJSON);
            if (Patient == null) { return new ObjectResultModule("", 204, "NoContent"); }
            // 查询医生服务信息
            var ServiceResult = await _serviceMoneyListService.ServiceMoneyListByID(Order.ServiceID);
            if (ServiceResult == null) { return new ObjectResultModule("", 204, "NoContent"); }
            // 查询微信支付商家信息
            var param1 = new SystemParameterIn() { SystemType = "WxPayBusinessId" };
            var wxparamlist = await _systemParameterService.ParameterList(param1);


            if (usermanager.MobileRoleName == "doctor")//医生
            {
                TencentWXPay tencentWXPay = new TencentWXPay();
                // 剔除重复查询
                var DocResult = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                if (DocResult == null) { return new ObjectResultModule("", 204, "NoContent"); }
                // 校对医生医生咨询订单状态
                if (yaeherConsultation == null || yaeherConsultation.DoctorID != DocResult.Id) { return new ObjectResultModule("", 204, "NoContent"); }
                if (yaeherConsultation.ConsultState == "return") { return new ObjectResultModule("", 400, "退单状态不允许重复提交！"); }
                // 查询患者端咨询订单状态
                var content = "{\"ConsultNumber\":\"" + yaeherConsultation.ConsultNumber.ToString() + "\",\"secret\":\"" + secret + "\",\"RefundType\":\"return\"}";
                var consulrequet = await this.PostResponseAsync(Commons.PatientIp + "api/Consultation/", content);
                var PatientConsul = JsonHelper.FromJson<APIResult<ResultModule<YaeherConsultation>>>(consulrequet);
                if (PatientConsul == null || PatientConsul.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
                if (PatientConsul.result.item.ConsultState == "return") { return new ObjectResultModule("", 400, "患者已退单,请不要退单"); }
                var income = new IncomeDevide();
                if (yaeherConsultation.ConsultState == "success")
                {
                    // return new ObjectResultModule("", 400, "订单已经进入完成状态,请不要退单！");
                    IncomeDevideIn incomein = new IncomeDevideIn();
                    incomein.AndAlso(t => t.IsDelete == false && t.ConsultNumber == yaeherConsultation.ConsultNumber);
                    var incomelist = await _incomeDevideService.IncomeDevideList(incomein);
                    income = incomelist.FirstOrDefault();
                    if (income != null && income.Id > 0)
                    {
                        TimeSpan sp = income.DevideTime.Subtract(DateTime.Now);
                        if (sp.TotalDays < 1)
                        {
                            return new ObjectResultModule("", 400, "订单已进入结算程序，不允许退单！");
                        }
                    }

                }
                if (string.IsNullOrEmpty(RefundManageInfo.RefundRemarks))
                {
                    return new ObjectResultModule("", 400, "退单原因必填,不允许退单！");
                }
                var label = new YaeherLabelConfig();
                if (RefundManageInfo.LabelId > 0)
                {
                    label = await _yaeherLabelConfigService.YaeherLabelConfigByID(RefundManageInfo.LabelId);
                    if (label == null) { return new ObjectResultModule("", 204, "NoContent"); }
                }
                var order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);

                OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
                orderTradeRecordIn.AndAlso(t => !t.IsDelete && t.PayType == "wxpay" && t.OrderID == order.Id && t.PaymentState == "paid" && t.PaymentSourceCode == "order");
                var refundordertradelist = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);

                var refundordertrade = refundordertradelist.FirstOrDefault();
                var refundnumber = "RN-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4);
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var CreateRefundManage = new RefundManage()
                    {
                        RefundNumber = refundnumber,
                        SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                        ConsultID = RefundManageInfo.ConsultID,
                        ConsultNumber = yaeherConsultation.ConsultNumber,
                        OrderID = Order.Id,
                        OrderNumber = Order.OrderNumber,
                        ConsultantID = yaeherConsultation.CreatedBy,
                        ConsultantName = Consultant.FullName,
                        PatientID = yaeherConsultation.PatientID,
                        PatientName = Patient.LeaguerName,
                        DoctorID = yaeherConsultation.DoctorID,
                        DoctorName = DocResult.DoctorName,
                        OrderCurrency = "rmb",
                        OrderMoney = Convert.ToDecimal(Order.OrderMoney),//订单总金额就是退单金额
                        ServiceID = Order.ServiceID,
                        ServiceName = ServiceResult.DoctorName + ServiceResult.ServiceType,
                        SellerMoneyID = wxparamlist[0].ItemValue,
                        CheckState = "success",//医生退单自动审核通过
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        RefundRemarks = RefundManageInfo.RefundRemarks
                    };
                    if (RefundManageInfo.LabelId > 0)
                    {
                        CreateRefundManage.RefundReason = JsonHelper.ToJson(label);
                    }
                    var result = await _refundManageService.CreateRefundManage(CreateRefundManage);

                    yaeherConsultation.RefundState = "return";
                    yaeherConsultation.ConsultState = "return";
                    yaeherConsultation.RefundBy = userid;
                    yaeherConsultation.RefundTime = DateTime.Now;
                    yaeherConsultation.RefundType = "doctorreturn";
                    yaeherConsultation.RefundNumber = result.RefundNumber;
                    yaeherConsultation.RefundReason = JsonHelper.ToJson(label);
                    yaeherConsultation.RefundRemarks = RefundManageInfo.RefundRemarks;

                    yaeherConsultation.RecommendDoctorID = RefundManageInfo.RecommendDoctorID;//推荐医生ID
                    if (RefundManageInfo.RecommendDoctorID > 0)
                    {
                        var Refunddoctor = await _yaeherDoctorService.YaeherDoctorByID(RefundManageInfo.RecommendDoctorID);
                        if (Refunddoctor != null)
                        {
                            yaeherConsultation.RecommendDoctorID = RefundManageInfo.RecommendDoctorID;//推荐医生ID
                            yaeherConsultation.RecommendDoctorName = Refunddoctor.DoctorName;//推荐医生
                        }
                    }
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
                        PaymentSourceCode = "doctorreturn",
                        PaymentSource = "医生退单",
                        CreatedBy = userid,
                        WXPayBillno = refundordertrade.WXPayBillno,
                        WXOrderQuery = refundordertrade.WXOrderQuery,
                        WXTransactionId = refundordertrade.WXTransactionId,
                        CreatedOn = DateTime.Now
                    };
                    var result2 = await _orderTradeRecordService.CreateOrderTradeRecord(record);
                    if (income != null && income.Id > 0)
                    {
                        income.IsDelete = true;
                        await _incomeDevideService.UpdateIncomeDevide(income);
                    }


                    #region 当咨询退单时 需要将所有job都关闭
                    //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                    //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == result.ConsultNumber && a.JobSates == "Open").ToList();

                    HangFireJobIn hangFireJobIn = new HangFireJobIn();
                    hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == result.ConsultNumber && a.JobSates == "Open");
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

                    #region  咨询退单
                    Consultation evaluation = new Consultation();
                    evaluation.yaeherConsultation = yaeherConsultation;
                    evaluation.refundManage = result;
                    evaluation.orderTradeRecords = result2;
                    Publishs Consultationpublishs = new Publishs()
                    {
                        TemplateCode = "PatientReturn",
                        OperationType = "DoctorReturn",  //    咨询评分
                        MessageRemark = yaeherConsultation.RefundRemarks,   // 退单原因
                        Publisher = "Doctor",
                        PublishUrl = "Doctor",
                        EventName = "咨询退单医生",
                        EventCode = "Reply",
                        BusinessID = res.Id.ToString(),
                        BusinessCode = res.ConsultNumber,
                        BusinessJSON = JsonHelper.ToJson(evaluation),
                        PublishedTime = res.CreatedOn,
                        PublishStatus = true,
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        Secret = secret,
                    };
                    var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                    var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                    if (ConsultationJson != null)
                    {
                        Consultationpublishs.PublishStatus = true;
                    }
                    else
                    {
                        Consultationpublishs.PublishStatus = false;
                    }
                    Consultationpublishs.ServerClient = "Client";
                    var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                    #endregion

                    #region 发送消息提醒  医生退单 PatientNotice DoctorReturn
                    SendMessageInfo sendMessageInfo = new SendMessageInfo();
                    sendMessageInfo.TemplateCode = "PatientReturn";
                    sendMessageInfo.OperationType = "DoctorReturn";
                    sendMessageInfo.ConsultNumber = Consultationpublishs.BusinessCode;
                    await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                    #endregion

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
                    string outRefundNo = refundnumber;
                    var totalFee = (int)(ordertrade.PayMoney * 100);//单位：分
                    int refundFee = totalFee;

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
            if (usermanager.MobileRoleName == "quality")//质控
            {
                var DocResult = await _yaeherDoctorService.YaeherDoctorByID(yaeherConsultation.DoctorID);
                if (DocResult == null) { return new ObjectResultModule("", 204, "NoContent"); }
                //退单标签
                var label = await _yaeherLabelConfigService.YaeherLabelConfigByID(RefundManageInfo.LabelId);
                if (label == null) { return new ObjectResultModule("", 204, "NoContent"); }
                //var param = new SystemParameterIn() { Type = "ConfigPar" };
                //param.AndAlso(t => !t.IsDelete && t.SystemCode == "QualityRefundManageType");
                //var paramlist = await _systemParameterService.ParameterList(param);

                var param3 = new SystemParameterIn() { SystemType = "QualityRefundTime" };
                var paramlist1 = await _systemParameterService.ParameterList(param3);
                if (yaeherConsultation.CreatedOn.AddDays(double.Parse(paramlist1[0].ItemValue)) < DateTime.Now)
                {
                    return new ObjectResultModule("", 400, "超过三十天咨询单不允许退回！");
                }
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var CreateRefundManage = new RefundManage()
                    {
                        RefundNumber = "RN-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4),
                        SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                        ConsultID = RefundManageInfo.ConsultID,
                        ConsultNumber = yaeherConsultation.ConsultNumber,
                        OrderID = Order.Id,
                        OrderNumber = Order.OrderNumber,
                        ConsultantID = yaeherConsultation.CreatedBy,
                        ConsultantName = Consultant.FullName,
                        PatientID = yaeherConsultation.PatientID,
                        PatientName = Patient.LeaguerName,
                        DoctorID = yaeherConsultation.DoctorID,
                        DoctorName = DocResult.DoctorName,
                        OrderCurrency = "rmb",
                        OrderMoney = Convert.ToDecimal(ServiceResult.ServiceExpense),
                        ServiceID = Order.ServiceID,
                        ServiceName = ServiceResult.DoctorName + ServiceResult.ServiceType,
                        SellerMoneyID = wxparamlist[0].ItemValue,
                        CheckState = "checking",
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        RefundReason = JsonHelper.ToJson(label),
                        RefundRemarks = RefundManageInfo.RefundRemarks,
                    };
                    yaeherConsultation.RefundType = "qualityreturn";
                    var res = await _consultationService.UpdateYaeherConsultation(yaeherConsultation);
                    var result = await _refundManageService.CreateRefundManage(CreateRefundManage);

                    #region  咨询退单
                    Consultation evaluation = new Consultation();
                    evaluation.yaeherConsultation = yaeherConsultation;
                    evaluation.refundManage = result;
                    Publishs Consultationpublishs = new Publishs()
                    {
                        TemplateCode = "ConsultationCancel",
                        OperationType = "QualityReturn",  //    咨询评分
                        MessageRemark = yaeherConsultation.RefundRemarks,   // 退单原因
                        Publisher = "QualityReturn",
                        PublishUrl = "QualityReturn",
                        EventName = "质控退单",
                        EventCode = "QualityReturn",
                        BusinessID = res.Id.ToString(),
                        BusinessCode = res.ConsultNumber,
                        BusinessJSON = JsonHelper.ToJson(evaluation),
                        PublishedTime = res.CreatedOn,
                        PublishStatus = true,
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        Secret = secret,
                    };
                    var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                    var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                    if (ConsultationJson != null)
                    {
                        Consultationpublishs.PublishStatus = true;
                    }
                    else
                    {
                        Consultationpublishs.PublishStatus = false;
                    }
                    Consultationpublishs.ServerClient = "Client";
                    var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                    #endregion
                    unitOfWork.Complete();
                }
            }
            this.ObjectResultModule.Object = "";
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateRefundManage",
                OperContent = yaeherConsultation.ConsultNumber,
                OperType = "CreateRefundManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion


            return ObjectResultModule;
        }
        /// <summary>
        /// 订单退单管理 修改
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateRefundManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateRefundManage([FromBody] RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var UpdateRefundManage = await _refundManageService.RefundManageByID(RefundManageInfo.Id);
            if (UpdateRefundManage != null)
            {
                if (usermanager.MobileRoleName == "admin")
                {
                    UpdateRefundManage.Checker = userid;
                    UpdateRefundManage.CheckRemark = RefundManageInfo.CheckRemark;
                    UpdateRefundManage.CheckState = RefundManageInfo.CheckState;
                    UpdateRefundManage.CheckTime = DateTime.Now;

                    if (RefundManageInfo.CheckState == "success")
                    {
                        var secret = await CreateSecret();
                        var yaeherConsultation = await _consultationService.YaeherConsultationByID(UpdateRefundManage.ConsultID);

                        yaeherConsultation.RefundState = "return";
                        yaeherConsultation.ConsultState = "return";
                        yaeherConsultation.RefundBy = userid;
                        yaeherConsultation.RefundTime = DateTime.Now;
                        yaeherConsultation.RefundType = "qualityreturn";
                        yaeherConsultation.RefundNumber = UpdateRefundManage.RefundNumber;
                        yaeherConsultation.RefundReason = UpdateRefundManage.RefundReason;
                        yaeherConsultation.RefundRemarks = UpdateRefundManage.RefundRemarks;


                        var order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);
                        var ordertrade = await _orderTradeRecordService.OrderTradeRecordExpress(t => !t.IsDelete && t.OrderID == order.Id && t.PaymentSourceCode == "order");

                        OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
                        orderTradeRecordIn.AndAlso(t => !t.IsDelete && t.PayType == "wxpay" && t.OrderID == order.Id && t.PaymentState == "paid" && t.PaymentSourceCode == "order");
                        var refundordertradelist = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);
                        var refundordertrade = refundordertradelist.FirstOrDefault();
                        using (var unitOfWork = _unitOfWorkManager.Begin())
                        {
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
                                PayMoney = -ordertrade.PayMoney,
                                PaymentState = "paid",
                                PaymentSourceCode = "qualityreturn",
                                PaymentSource = "质控退单",
                                CreatedBy = userid,
                                WXPayBillno = refundordertrade.WXPayBillno,
                                WXOrderQuery = refundordertrade.WXOrderQuery,
                                WXTransactionId = refundordertrade.WXTransactionId,
                                CreatedOn = DateTime.Now
                            };
                            var result2 = await _orderTradeRecordService.CreateOrderTradeRecord(record);

                           
                            #region 当咨询退单时 需要将所有job都关闭
                            //IList<HangFireJob> hangFireJobs = await _hangFireJobService.HangFireJobList();
                            //hangFireJobs = hangFireJobs.Where(a => a.BusinessCode == yaeherConsultation.ConsultNumber && a.JobSates == "Open").ToList();

                            HangFireJobIn hangFireJobIn = new HangFireJobIn();
                            hangFireJobIn.AndAlso(a => a.IsDelete == false && a.BusinessCode == yaeherConsultation.ConsultNumber && a.JobSates == "Open");
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
                            UpdateRefundManage.ModifyOn = DateTime.Now;
                            UpdateRefundManage.ModifyBy = userid;
                            var result = await _refundManageService.UpdateRefundManage(UpdateRefundManage);
                            this.ObjectResultModule.Object = result;

                            #region  咨询退单质控退单处理 接受人为患者 DoctorNotice QualityReturn
                            Consultation evaluation = new Consultation();
                            evaluation.yaeherConsultation = yaeherConsultation;
                            evaluation.refundManage = UpdateRefundManage;
                            evaluation.orderTradeRecords = result2;
                            Publishs Consultationpublishs = new Publishs()
                            {
                                TemplateCode = "DoctorReturn",
                                OperationType = "QualityReturn",  //    咨询评分
                                MessageRemark = yaeherConsultation.RefundRemarks,   // 退单原因
                                Publisher = "Admin",
                                PublishUrl = "Admin",
                                EventName = "管理审核退单",
                                EventCode = "Admin",
                                BusinessID = res.Id.ToString(),
                                BusinessCode = res.ConsultNumber,
                                BusinessJSON = JsonHelper.ToJson(evaluation),
                                PublishedTime = res.CreatedOn,
                                PublishStatus = true,
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                                Secret = secret,
                            };
                            var ConsultationParma = await this.PostResponseAsync(Commons.PatientIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                            var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                            if (ConsultationJson != null)
                            {
                                Consultationpublishs.PublishStatus = true;
                            }
                            else
                            {
                                Consultationpublishs.PublishStatus = false;
                            }
                            Consultationpublishs.ServerClient = "Client";
                            var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                            #endregion

                            #region 咨询退单质控退单处理 接受人为医生 DoctorReturn QualityReturn
                            SendMessageInfo sendMessageInfo = new SendMessageInfo();
                            sendMessageInfo.TemplateCode = "DoctorReturn";
                            sendMessageInfo.OperationType = "QualityReturn";
                            sendMessageInfo.ConsultNumber = Consultationpublishs.BusinessCode;
                            await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
                            #endregion

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
                            string outRefundNo = UpdateRefundManage.RefundNumber;
                            var totalFee = (int)(ordertrade.PayMoney * 100);//单位：分
                            int refundFee = totalFee;
                            TencentWXPay tencentWXPay = new TencentWXPay();
                            var refundpayresult = await tencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);
                            if (refundpayresult.code != "SUCCESS")
                            {
                                return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
                            }
                        }
                        #endregion
                    }
                }


                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateRefundManage",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "UpdateRefundManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单退单管理 删除
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteRefundManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteRefundManage([FromBody] RefundManage RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _refundManageService.RefundManageByID(RefundManageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _refundManageService.DeleteRefundManage(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteRefundManage",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "DeleteRefundManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单退单管理 Page
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManagePage([FromBody]RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            RefundManageInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(RefundManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(RefundManageInfo.StartTime);
                if (string.IsNullOrEmpty(RefundManageInfo.EndTime))
                {
                    RefundManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(RefundManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(RefundManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(RefundManageInfo.StartTime))
            {
                RefundManageInfo.AndAlso(a => a.CreatedOn >= StartTime);
                RefundManageInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(RefundManageInfo.KeyWord))
            {
                RefundManageInfo.AndAlso(a => a.ConsultantName.Contains(RefundManageInfo.KeyWord)
                                            || a.PatientName.Contains(RefundManageInfo.KeyWord)
                                            || a.DoctorName.Contains(RefundManageInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(RefundManageInfo.ConsultNumber))
            {
                RefundManageInfo.AndAlso(a => a.ConsultNumber.Contains(RefundManageInfo.ConsultNumber));
            }

            // 区分平台来源
            if (RefundManageInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    RefundManageInfo.RefundType = "doctorreturn";
                    RefundManageInfo.AndAlso(a => a.DoctorID == doctor.Id);
                }
                if (usermanager.MobileRoleName == "admin")
                {//管理审核指控退单的数据
                    RefundManageInfo.RefundType = "qualityreturn";
                }
            }
            else if (RefundManageInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    RefundManageInfo.RefundType = "doctorreturn";

                    RefundManageInfo.AndAlso(a => a.DoctorID == doctor.Id);
                }
                if (usermanager.IsAdmin)
                {//管理审核指控退单的数据
                    RefundManageInfo.RefundType = "qualityreturn";
                }
            }
            var values = await _refundManageService.RefundManagePage(RefundManageInfo);

            this.ObjectResultModule.Object = new RefundManageOut(values, RefundManageInfo);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "RefundManagePage",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "RefundManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单退单管理 List 
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManageList([FromBody]RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            RefundManageInfo.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(RefundManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(RefundManageInfo.StartTime);
                if (string.IsNullOrEmpty(RefundManageInfo.EndTime))
                {
                    RefundManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(RefundManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(RefundManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(RefundManageInfo.StartTime))
            {
                RefundManageInfo.AndAlso(a => a.CreatedOn >= StartTime && a.CreatedOn < EndTime.AddDays(+1));
            }
            // 区分平台来源
            if (RefundManageInfo.Platform == "Mobile")
            {
                RefundManageInfo.AndAlso(a => a.DoctorID == doctor.Id);
            }
            else if (RefundManageInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    RefundManageInfo.AndAlso(a => a.DoctorID == doctor.Id);
                }
            }
            var values = await _refundManageService.RefundManageList(RefundManageInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "RefundManageList",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "RefundManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单退单管理 Byid
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManageById([FromBody]RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = new RefundManage();
            if (RefundManageInfo.Id > 0)
            {
                values = await _refundManageService.RefundManageByID(RefundManageInfo.Id);
            }
            else
            {
                values = await _refundManageService.RefundManageByNumber(RefundManageInfo.RefundNumber);
            }
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "RefundManageById",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "RefundManageById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单分配
        /// <summary>
        /// 订单分配 新增
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/CreateIncomeDevide")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateIncomeDevide([FromBody]  IncomeDevide IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateIncomeDevide = new IncomeDevide()
            {
                ConsultNumber = IncomeDevideInfo.ConsultNumber,
                ConsultID = IncomeDevideInfo.ConsultID,
                DoctorID = IncomeDevideInfo.DoctorID,
                DoctorName = IncomeDevideInfo.DoctorName,
                OrderID = IncomeDevideInfo.OrderID,
                OrderNumber = IncomeDevideInfo.OrderNumber,
                OrderMoney = IncomeDevideInfo.OrderMoney,
                OrderCurrency = IncomeDevideInfo.OrderCurrency,
                DevideMoney = IncomeDevideInfo.DevideMoney,
                DevideRatio = IncomeDevideInfo.DevideRatio,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _incomeDevideService.CreateIncomeDevide(CreateIncomeDevide);
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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateIncomeDevide",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "CreateIncomeDevide",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单分配 修改
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateIncomeDevide")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateIncomeDevide([FromBody]  IncomeDevide IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateIncomeDevide = await _incomeDevideService.IncomeDevideByID(IncomeDevideInfo.Id);
            if (UpdateIncomeDevide != null)
            {
                UpdateIncomeDevide.ConsultNumber = IncomeDevideInfo.ConsultNumber;
                UpdateIncomeDevide.ConsultID = IncomeDevideInfo.ConsultID;
                UpdateIncomeDevide.DoctorID = IncomeDevideInfo.DoctorID;
                UpdateIncomeDevide.DoctorName = IncomeDevideInfo.DoctorName;
                UpdateIncomeDevide.OrderID = IncomeDevideInfo.OrderID;
                UpdateIncomeDevide.OrderNumber = IncomeDevideInfo.OrderNumber;
                UpdateIncomeDevide.OrderMoney = IncomeDevideInfo.OrderMoney;
                UpdateIncomeDevide.OrderCurrency = IncomeDevideInfo.OrderCurrency;
                UpdateIncomeDevide.DevideMoney = IncomeDevideInfo.DevideMoney;
                UpdateIncomeDevide.DevideRatio = IncomeDevideInfo.DevideRatio;

                UpdateIncomeDevide.ModifyOn = DateTime.Now;
                UpdateIncomeDevide.ModifyBy = userid;

                var result = await _incomeDevideService.UpdateIncomeDevide(UpdateIncomeDevide);

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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateIncomeDevide",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "UpdateIncomeDevide",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单分配 删除
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteIncomeDevide")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteIncomeDevide([FromBody]  IncomeDevide IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _incomeDevideService.IncomeDevideByID(IncomeDevideInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _incomeDevideService.DeleteIncomeDevide(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteIncomeDevide",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "DeleteIncomeDevide",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单分配 Page
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeDevidePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDevidePage([FromBody] IncomeDevideIn IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            IncomeDevideInfo.AndAlso(t => !t.IsDelete);
            var values = await _incomeDevideService.IncomeDevidePage(IncomeDevideInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new IncomeDevideOut(values, IncomeDevideInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "IncomeDevidePage",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "IncomeDevidePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单分配 List 
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeDevideList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDevideList([FromBody]IncomeDevideIn IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            IncomeDevideInfo.AndAlso(t => !t.IsDelete);
            var values = await _incomeDevideService.IncomeDevideList(IncomeDevideInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "IncomeDevideList",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "IncomeDevideList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单分配 Byid
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeDevideById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDevideById([FromBody]IncomeDevideIn IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _incomeDevideService.IncomeDevideByID(IncomeDevideInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "IncomeDevideById",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "IncomeDevideById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        /// <summary>
        /// 查看咨询明细
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherConsultationDetail")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherConsultationDetail([FromBody]ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            // 咨询主表
            var YaeherConsultationDetail = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            if (YaeherConsultationDetail == null) { return new ObjectResultModule("", 204, "NoContent"); }

            // 咨询文件附件地址
            AttachmentIn attachmentIn = new AttachmentIn();
            attachmentIn.AndAlso(a => a.IsDelete == false);
            attachmentIn.AndAlso(a => a.ConsultNumber == YaeherConsultationDetail.ConsultNumber);
            var ConsultationFileList = await _attachmentServices.AttachmentList(attachmentIn);

            // 回答 追问
            ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
            consultationReplyIn.AndAlso(a => a.IsDelete == false);
            consultationReplyIn.AndAlso(a => a.ConsultNumber == YaeherConsultationDetail.ConsultNumber);
            var ReplyList = await _consultationReplyService.ConsultationReplyList(consultationReplyIn);

            // 电话回复
            PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
            phoneReplyRecordIn.AndAlso(a => a.IsDelete == false);
            phoneReplyRecordIn.AndAlso(a => a.ConsultNumber == YaeherConsultationDetail.ConsultNumber);
            var PhoneReplyList = await _phoneReplyRecordService.PhoneReplyRecordList(phoneReplyRecordIn);

            // 订单评分
            ConsultationEvaluationIn consultationEvaluationIn = new ConsultationEvaluationIn();
            consultationEvaluationIn.AndAlso(a => a.IsDelete == false);
            consultationEvaluationIn.AndAlso(a => a.ConsultNumber == YaeherConsultationDetail.ConsultNumber);
            var EvaluationList = await _consultationEvaluationService.ConsultationEvaluationList(consultationEvaluationIn);

            // 订单管理
            OrderManageIn orderManageIn = new OrderManageIn();
            orderManageIn.AndAlso(a => a.IsDelete == false);
            orderManageIn.AndAlso(a => a.ConsultNumber == YaeherConsultationDetail.ConsultNumber);
            var orderManageList = await _orderManageService.OrderManageList(orderManageIn);

            // 订单交易记录
            OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
            orderTradeRecordIn.AndAlso(a => a.IsDelete == false);
            //orderTradeRecordIn.AndAlso(a => a.OrderNumber == ConsultationInfo);
            var OrderTradeList = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);

            // 订单退单记录
            RefundManageIn refundManageIn = new RefundManageIn();
            refundManageIn.AndAlso(a => a.IsDelete == false);
            refundManageIn.AndAlso(a => a.ConsultNumber == YaeherConsultationDetail.ConsultNumber);
            var refundManageList = await _refundManageService.RefundManageList(refundManageIn);

            ConsultationManageEntity consultationManageEntity = new ConsultationManageEntity();
            consultationManageEntity.ConsultationInfo = YaeherConsultationDetail;
            consultationManageEntity.ConsultationFileList = ConsultationFileList.ToList();

            List<ConsultationReplyInfo> consultationReplyInfos = new List<ConsultationReplyInfo>();
            if (ReplyList.Count > 0)
            {
                foreach (var Reply in ReplyList)
                {
                    ConsultationReplyInfo consultationReplyInfo = new ConsultationReplyInfo();
                    consultationReplyInfo.ReplyInfo = Reply;
                    var ReplyFileList = from a in ReplyList
                                        join b in ConsultationFileList on a.ConsultantID equals b.ServiceID
                                        select b;
                    consultationReplyInfo.ReplyAttachmentFile = ReplyFileList.ToList();
                    consultationReplyInfos.Add(consultationReplyInfo);
                }
            }
            consultationManageEntity.ReplyList = consultationReplyInfos.ToList();
            consultationManageEntity.PhoneReplyList = PhoneReplyList.ToList();
            consultationManageEntity.EvaluationList = EvaluationList.ToList();
            consultationManageEntity.orderManageList = orderManageList.ToList();
            if (OrderTradeList.Count > 0)
            {
                var orderTradeRecords = from a in orderManageList
                                        join b in OrderTradeList on a.OrderNumber equals b.OrderNumber
                                        select b;
                if (orderTradeRecords.Count() > 0)
                {
                    consultationManageEntity.OrderTradeList = orderTradeRecords.ToList();
                }
            }
            consultationManageEntity.refundManageList = refundManageList.ToList();

            if (consultationManageEntity == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = consultationManageEntity;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherConsultationDetail",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "YaeherConsultationDetail",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 退单查询
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/RefunOrderPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefunOrderPage([FromBody]ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            ConsultationInfo.AndAlso(a => !a.IsDelete);
            ConsultationInfo.AndAlso(a => a.RefundNumber != "");  // 不等于null为退单
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationInfo.EndTime))
                {
                    ConsultationInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.StartTime))
            {
                ConsultationInfo.AndAlso(a => a.RefundTime >= StartTime);
                ConsultationInfo.AndAlso(a => a.RefundTime < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.KeyWord))
            {
                ConsultationInfo.AndAlso(a => a.ConsultantName.Contains(ConsultationInfo.KeyWord)
                                            || a.PatientName.Contains(ConsultationInfo.KeyWord)
                                            || a.DoctorName.Contains(ConsultationInfo.KeyWord)
                                            || a.ConsultNumber.Contains(ConsultationInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.ConsultNumber))
            {
                ConsultationInfo.AndAlso(a => a.ConsultNumber.Contains(ConsultationInfo.ConsultNumber));
            }
            if (!string.IsNullOrEmpty(ConsultationInfo.RefundState))
            {
                ConsultationInfo.AndAlso(a => a.RefundState.Contains(ConsultationInfo.RefundState));
            }
            // 区分平台来源
            if (ConsultationInfo.Platform == "Mobile")
            {
                ConsultationInfo.AndAlso(a => a.DoctorID == doctor.Id);
            }
            else if (ConsultationInfo.Platform == "PC")
            {
                // 当为PC登陆 是医生角色使用医生ID查询
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    ConsultationInfo.AndAlso(a => a.DoctorID == doctor.Id);
                }
            }
            var YaeherConsultationList = await _consultationService.YaeherConsultationPage(ConsultationInfo);
            if (YaeherConsultationList.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var param = new SystemParameterIn() { Type = "ConfigPar" };
                param.AndAlso(a => !a.IsDelete && a.SystemCode == "ConsultState");
                var paramlist = await _systemParameterService.ParameterList(param);
                this.ObjectResultModule.Object = new ConsultationOut(YaeherConsultationList, ConsultationInfo, paramlist);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "RefunOrderPage",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "RefunOrderPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }


    }
}