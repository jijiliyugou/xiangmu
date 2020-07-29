using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.CloudCallCenter;
using Yaeher.Common.Constants;
using Yaeher.Common.HttpHelpers;
using Yaeher.Common.SendMsm;
using Yaeher.Common.TencentCustom;
using Yaeher.EventBus;
using Yaeher.Extensions;
using Yaeher.HangFire;
using Yaeher.MessageRemind;
using Yaeher.MessageRemind.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemManageController : YaeherAppServiceBase
    {
        private readonly IAreaManageService _areaManageService;
        private readonly IDoctorCheckService _doctorCheckService;
        private readonly IDoctorOnlineRecordService _doctorOnlineRecordService;
        private readonly IDoctorOnlineSetLogService _doctorOnlineSetLogService;
        private readonly IDoctorWithdrawRecordService _doctorWithdrawRecordService;
        private readonly IInterfaceSetService _interfaceSetService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IDoctorParaSetService _doctorParaSetService;
        private readonly IYaeherMessageRemindService _yaeherMessageRemindService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IUserManagerService _userManagerService;
        private readonly ISystemConfigsService _systemConfigsService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IYaeherPhoneService _yaeherPhoneService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherBannerService _yaeherBannerService;
        private readonly IYaeherLabelConfigService _yaeherLabelConfigService;
        private readonly IYaeherConditionalMenuService _yaeherConditionalMenuService;
        private readonly IYaeherMessageTemplateService _yaeherMessageTemplateService;
        private readonly IWecharSendMessageService _wecharSendMessageService;
        private readonly ISendMessageService _sendMessageService;
        private readonly IYaeherUserService _yaeherUserService;
        private readonly IAcceptTencentWecharService _acceptTencentWecharService;
        private readonly IAcceptWecharStateService _acceptWecharStateService;
        private readonly IRelationLabelGroupService _relationLabelGroupService;
        private readonly ISystemTokenService _systemTokenService;
        private readonly IQuickReplyService _quickReplyService;
        private readonly IHangFireJobService _hangFireJobService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaManageService"></param>
        /// <param name="doctorCheckService"></param>
        /// <param name="doctorOnlineRecordService"></param>
        /// <param name="doctorOnlineSetLogService"></param>
        /// <param name="doctorWithdrawRecordService"></param>
        /// <param name="interfaceSetService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="doctorParaSetService"></param>
        /// <param name="yaeherMessageRemindService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="userManagerService"></param>
        /// <param name="systemConfigsService"></param>
        /// <param name="yaeherPhoneService"></param>
        /// <param name="yaeherBannerService"></param>
        /// <param name="yaeherLabelConfigService"></param>
        /// <param name="yaeherConditionalMenuService"></param>
        /// <param name="yaeherMessageTemplateService"></param>
        /// <param name="wecharSendMessageService"></param>
        /// <param name="sendMessageService"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="acceptTencentWecharService"></param>
        /// <param name="acceptWecharStateService"></param>
        /// <param name="session"></param>
        /// <param name="relationLabelGroupService"></param>
        /// <param name="systemTokenService"></param>
        /// <param name="quickReplyService"></param>
        /// <param name="hangFireJobService"></param>
        public SystemManageController(IAreaManageService areaManageService,
                                      IDoctorCheckService doctorCheckService,
                                      IDoctorOnlineRecordService doctorOnlineRecordService,
                                      IDoctorOnlineSetLogService doctorOnlineSetLogService,
                                      IDoctorWithdrawRecordService doctorWithdrawRecordService,
                                      IInterfaceSetService interfaceSetService,
                                      ISystemParameterService systemParameterService,
                                      IYaeherOperListService yaeherOperListService,
                                      IDoctorParaSetService doctorParaSetService,
                                      IYaeherMessageRemindService yaeherMessageRemindService,
                                      IYaeherDoctorService yaeherDoctorService,
                                      IUnitOfWorkManager unitOfWorkManager,
                                      IUserManagerService userManagerService,
                                      ISystemConfigsService systemConfigsService,
                                      IYaeherPhoneService yaeherPhoneService,
                                      IYaeherBannerService yaeherBannerService,
                                      IYaeherLabelConfigService yaeherLabelConfigService,
                                      IYaeherConditionalMenuService yaeherConditionalMenuService,
                                      IYaeherMessageTemplateService yaeherMessageTemplateService,
                                      IWecharSendMessageService wecharSendMessageService,
                                      ISendMessageService sendMessageService,
                                      IYaeherUserService yaeherUserService,
                                      IAcceptTencentWecharService acceptTencentWecharService,
                                      IAcceptWecharStateService acceptWecharStateService,
                                      IAbpSession session,
                                      IRelationLabelGroupService relationLabelGroupService,
                                      ISystemTokenService systemTokenService,
                                      IQuickReplyService quickReplyService,
                                      IHangFireJobService hangFireJobService)
        {
            _areaManageService = areaManageService;
            _doctorCheckService = doctorCheckService;
            _doctorOnlineRecordService = doctorOnlineRecordService;
            _doctorOnlineSetLogService = doctorOnlineSetLogService;
            _doctorWithdrawRecordService = doctorWithdrawRecordService;
            _interfaceSetService = interfaceSetService;
            _systemParameterService = systemParameterService;
            _yaeherOperListService = yaeherOperListService;
            _doctorParaSetService = doctorParaSetService;
            _yaeherMessageRemindService = yaeherMessageRemindService;
            _yaeherDoctorService = yaeherDoctorService;
            _unitOfWorkManager = unitOfWorkManager;
            _userManagerService = userManagerService;
            _systemConfigsService = systemConfigsService;
            _yaeherPhoneService = yaeherPhoneService;
            _IabpSession = session;
            _yaeherBannerService = yaeherBannerService;
            _yaeherLabelConfigService = yaeherLabelConfigService;
            _yaeherConditionalMenuService = yaeherConditionalMenuService;
            _yaeherMessageTemplateService = yaeherMessageTemplateService;
            _wecharSendMessageService = wecharSendMessageService;
            _sendMessageService = sendMessageService;
            _yaeherUserService = yaeherUserService;
            _acceptTencentWecharService = acceptTencentWecharService;
            _acceptWecharStateService = acceptWecharStateService;
            _relationLabelGroupService = relationLabelGroupService;
            _systemTokenService = systemTokenService;
            _quickReplyService = quickReplyService;
            _hangFireJobService = hangFireJobService;
        }

        #region 医生上下线设置记录
        /// <summary>
        /// 医生上下线设置记录 新增
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorOnlineRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorOnlineRecord([FromBody] DoctorOnlineRecord DoctorOnlineRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var doctor = await _yaeherDoctorService.YaeherDoctorByID(DoctorOnlineRecordInfo.DoctorID);
            var doctoronlineIN = new DoctorOnlineRecordIn();
            doctoronlineIN.AndAlso(t => !t.IsDelete && t.DoctorID == DoctorOnlineRecordInfo.DoctorID);
            var doctoronline = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctoronlineIN);
            if (doctoronline.Count > 0) { return new ObjectResultModule("", 400, "已创建上线记录医生不允许创建新纪录!"); }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateDoctorOnlineRecord = new DoctorOnlineRecord()
                {
                    DoctorName = doctor.DoctorName,
                    DoctorID = DoctorOnlineRecordInfo.DoctorID,
                    DoctorJSON = JsonHelper.ToJson(doctor),
                    OnlineState = DoctorOnlineRecordInfo.OnlineState,//上线
                    DivideInto = DoctorOnlineRecordInfo.DivideInto,//NewDoctorPayProportions code对应的值
                    IncomeDay = DoctorOnlineRecordInfo.IncomeDay,//DoctorReceivablesTime 医生收款
                    DoctorMoneyExchange = DoctorOnlineRecordInfo.DoctorMoneyExchange,//
                    DoctorMoneyexTime = DoctorOnlineRecordInfo.DoctorMoneyexTime,//
                    Remark = DoctorOnlineRecordInfo.Remark,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                var result = await _doctorOnlineRecordService.CreateDoctorOnlineRecord(CreateDoctorOnlineRecord);
                #region 插入log日志表
                var CreateDoctorOnlineSetLog = new DoctorOnlineSetLog()
                {
                    DoctorName = DoctorOnlineRecordInfo.DoctorName,
                    DoctorID = DoctorOnlineRecordInfo.DoctorID,
                    DoctorJSON = JsonHelper.ToJson(doctor),
                    OnlineState = DoctorOnlineRecordInfo.OnlineState,
                    DivideInto = DoctorOnlineRecordInfo.DivideInto,
                    IncomeDay = DoctorOnlineRecordInfo.IncomeDay,
                    Remark = DoctorOnlineRecordInfo.Remark,
                    CheckState = DoctorOnlineRecordInfo.CheckState,
                    CheckRemark = DoctorOnlineRecordInfo.CheckRemark,
                    CheckTime = DoctorOnlineRecordInfo.CheckTime,
                    Checker = DoctorOnlineRecordInfo.CreatedBy,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    DoctorMoneyExchange = DoctorOnlineRecordInfo.DoctorMoneyExchange,
                    DoctorMoneyexTime = DoctorOnlineRecordInfo.DoctorMoneyexTime,
                };
                await _doctorOnlineSetLogService.CreateDoctorOnlineSetLog(CreateDoctorOnlineSetLog);
                #endregion
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
                #region 操作日志
                var CreateYaeherOperList = new YaeherOperList()
                {
                    OperExplain = "CreateDoctorOnlineRecord",
                    OperContent = JsonHelper.ToJson(DoctorOnlineRecordInfo),
                    OperType = "CreateDoctorOnlineRecord",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
                #endregion
                unitOfWork.Complete();

            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生上下线设置记录 修改
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorOnlineRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorOnlineRecord([FromBody]  DoctorOnlineRecord DoctorOnlineRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateDoctorOnlineRecord = await _doctorOnlineRecordService.DoctorOnlineRecordByID(DoctorOnlineRecordInfo.Id);
            if (UpdateDoctorOnlineRecord != null)
            {
                if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.OnlineState))
                {
                    UpdateDoctorOnlineRecord.OnlineState = DoctorOnlineRecordInfo.OnlineState;
                }
                if (DoctorOnlineRecordInfo.DivideInto > 0)
                {
                    UpdateDoctorOnlineRecord.DivideInto = DoctorOnlineRecordInfo.DivideInto;
                }
                if (DoctorOnlineRecordInfo.IncomeDay > 0)
                {
                    UpdateDoctorOnlineRecord.IncomeDay = DoctorOnlineRecordInfo.IncomeDay;
                }
                if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.Remark))
                {
                    UpdateDoctorOnlineRecord.Remark = DoctorOnlineRecordInfo.Remark;
                }
                if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.CheckState))
                {
                    UpdateDoctorOnlineRecord.CheckState = DoctorOnlineRecordInfo.CheckState;
                    UpdateDoctorOnlineRecord.CheckRemark = DoctorOnlineRecordInfo.CheckRemark;
                    UpdateDoctorOnlineRecord.CheckTime = DateTime.Now;
                    UpdateDoctorOnlineRecord.Checker = userid;
                }
                UpdateDoctorOnlineRecord.ModifyOn = DateTime.Now;
                UpdateDoctorOnlineRecord.ModifyBy = userid;
                if (DoctorOnlineRecordInfo.DoctorMoneyExchange > 0)
                {
                    UpdateDoctorOnlineRecord.DoctorMoneyExchange = DoctorOnlineRecordInfo.DoctorMoneyExchange;
                }
                if (DoctorOnlineRecordInfo.DoctorMoneyexTime > 0)
                {
                    UpdateDoctorOnlineRecord.DoctorMoneyexTime = DoctorOnlineRecordInfo.DoctorMoneyexTime;
                }
                var result = await _doctorOnlineRecordService.UpdateDoctorOnlineRecord(UpdateDoctorOnlineRecord);

                #region 插入log日志表
                var CreateDoctorOnlineSetLog = new DoctorOnlineSetLog()
                {
                    DoctorName = UpdateDoctorOnlineRecord.DoctorName,
                    DoctorID = UpdateDoctorOnlineRecord.DoctorID,
                    DoctorJSON = UpdateDoctorOnlineRecord.DoctorJSON,
                    OnlineState = DoctorOnlineRecordInfo.OnlineState,
                    DivideInto = DoctorOnlineRecordInfo.DivideInto,
                    IncomeDay = DoctorOnlineRecordInfo.IncomeDay,
                    Remark = DoctorOnlineRecordInfo.Remark,
                    CheckState = DoctorOnlineRecordInfo.CheckState,
                    CheckRemark = DoctorOnlineRecordInfo.CheckRemark,
                    CheckTime = DoctorOnlineRecordInfo.CheckTime,
                    Checker = DoctorOnlineRecordInfo.CreatedBy,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    DoctorMoneyExchange = DoctorOnlineRecordInfo.DoctorMoneyExchange,
                    DoctorMoneyexTime = DoctorOnlineRecordInfo.DoctorMoneyexTime,
                };
                await _doctorOnlineSetLogService.CreateDoctorOnlineSetLog(CreateDoctorOnlineSetLog);
                #endregion
                #region 操作日志
                var CreateYaeherOperList = new YaeherOperList()
                {
                    OperExplain = "UpdateDoctorOnlineRecord",
                    OperContent = JsonHelper.ToJson(DoctorOnlineRecordInfo),
                    OperType = "UpdateDoctorOnlineRecord",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生上下线设置记录 删除
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorOnlineRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorOnlineRecord([FromBody]  DoctorOnlineRecord DoctorOnlineRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorOnlineRecordService.DoctorOnlineRecordByID(DoctorOnlineRecordInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _doctorOnlineRecordService.DeleteDoctorOnlineRecord(query);

                #region 插入log日志表
                var CreateDoctorOnlineSetLog = new DoctorOnlineSetLog()
                {
                    DoctorName = DoctorOnlineRecordInfo.DoctorName,
                    DoctorID = DoctorOnlineRecordInfo.DoctorID,
                    DoctorJSON = DoctorOnlineRecordInfo.DoctorJSON,
                    OnlineState = DoctorOnlineRecordInfo.OnlineState,
                    DivideInto = DoctorOnlineRecordInfo.DivideInto,
                    IncomeDay = DoctorOnlineRecordInfo.IncomeDay,
                    Remark = DoctorOnlineRecordInfo.Remark,
                    CheckState = DoctorOnlineRecordInfo.CheckState,
                    CheckRemark = DoctorOnlineRecordInfo.CheckRemark,
                    CheckTime = DoctorOnlineRecordInfo.CheckTime,
                    Checker = DoctorOnlineRecordInfo.CreatedBy,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    DoctorMoneyExchange = DoctorOnlineRecordInfo.DoctorMoneyExchange,
                    DoctorMoneyexTime = DoctorOnlineRecordInfo.DoctorMoneyexTime,
                };
                await _doctorOnlineSetLogService.CreateDoctorOnlineSetLog(CreateDoctorOnlineSetLog);
                #endregion
                #region 操作日志
                var CreateYaeherOperList = new YaeherOperList()
                {
                    OperExplain = "DeleteDoctorOnlineRecord",
                    OperContent = JsonHelper.ToJson(DoctorOnlineRecordInfo),
                    OperType = "DeleteDoctorOnlineRecord",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
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
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生上下线设置记录 Page
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOnlineRecordPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOnlineRecordPage([FromBody] DoctorOnlineRecordIn DoctorOnlineRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorOnlineRecordInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorOnlineRecordInfo.EndTime))
                {
                    DoctorOnlineRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorOnlineRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.StartTime))
            {
                DoctorOnlineRecordInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.KeyWord))
            {
                DoctorOnlineRecordInfo.AndAlso(t => t.DoctorName.Contains(DoctorOnlineRecordInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.OnlineState))
            {
                DoctorOnlineRecordInfo.AndAlso(t => t.OnlineState.Contains(DoctorOnlineRecordInfo.OnlineState));
            }
            DoctorOnlineRecordInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorOnlineRecordService.DoctorOnlineRecordPage(DoctorOnlineRecordInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorOnlineRecordOut(values, DoctorOnlineRecordInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorOnlineRecordPage",
                OperContent = JsonHelper.ToJson(DoctorOnlineRecordInfo),
                OperType = "DoctorOnlineRecordPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生上下线设置记录 List 
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOnlineRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOnlineRecordList([FromBody]DoctorOnlineRecordIn DoctorOnlineRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorOnlineRecordInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorOnlineRecordInfo.EndTime))
                {
                    DoctorOnlineRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorOnlineRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.StartTime))
            {
                DoctorOnlineRecordInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.KeyWord))
            {
                DoctorOnlineRecordInfo.AndAlso(t => t.DoctorName.Contains(DoctorOnlineRecordInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(DoctorOnlineRecordInfo.OnlineState))
            {
                DoctorOnlineRecordInfo.AndAlso(t => t.OnlineState.Contains(DoctorOnlineRecordInfo.OnlineState));
            }
            DoctorOnlineRecordInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorOnlineRecordService.DoctorOnlineRecordList(DoctorOnlineRecordInfo);
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
                OperExplain = "DoctorOnlineRecordList",
                OperContent = JsonHelper.ToJson(DoctorOnlineRecordInfo),
                OperType = "DoctorOnlineRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生上下线设置记录 Byid
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOnlineRecordById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOnlineRecordById([FromBody]DoctorOnlineRecordIn DoctorOnlineRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _doctorOnlineRecordService.DoctorOnlineRecordByID(DoctorOnlineRecordInfo.Id);
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
                OperExplain = "DoctorOnlineRecordById",
                OperContent = JsonHelper.ToJson(DoctorOnlineRecordInfo),
                OperType = "DoctorOnlineRecordById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 医生上下线设置记录log
        #region 关闭log日志的操作
        ///// <summary>
        ///// 医生上下线设置记录log 新增
        ///// </summary>
        ///// <param name="DoctorOnlineSetLogInfo"></param>
        ///// <returns></returns>
        //[Route("api/CreateDoctorOnlineSetLog")]
        //[HttpPost]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> CreateDoctorOnlineSetLog([FromBody] DoctorOnlineSetLog DoctorOnlineSetLogInfo)
        //{
        //    if (!Commons.CheckSecret(DoctorOnlineSetLogInfo.Secret))
        //    {
        //        this.ObjectResultModule.StatusCode = 422;
        //        this.ObjectResultModule.Message = "Wrong Secret";
        //        this.ObjectResultModule.Object = "";
        //        return this.ObjectResultModule;
        //    }
        //    var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
        //    var CreateDoctorOnlineSetLog = new DoctorOnlineSetLog()
        //    {
        //        DoctorName = DoctorOnlineSetLogInfo.DoctorName,
        //        DoctorID = DoctorOnlineSetLogInfo.DoctorID,
        //        DoctorJSON = DoctorOnlineSetLogInfo.DoctorJSON,
        //        OnlineState = DoctorOnlineSetLogInfo.OnlineState,
        //        DivideInto = DoctorOnlineSetLogInfo.DivideInto,
        //        IncomeDay = DoctorOnlineSetLogInfo.IncomeDay,
        //        Remark = DoctorOnlineSetLogInfo.Remark,
        //        CheckState = DoctorOnlineSetLogInfo.CheckState,
        //        CheckRemark = DoctorOnlineSetLogInfo.CheckRemark,
        //        CheckTime = DoctorOnlineSetLogInfo.CheckTime,
        //        Checker = DoctorOnlineSetLogInfo.CreatedBy,
        //        CreatedBy = userid,
        //        CreatedOn = DateTime.Now,
        //    };
        //    var result = await _doctorOnlineSetLogService.CreateDoctorOnlineSetLog(CreateDoctorOnlineSetLog);
        //    if (result.Id > 0)
        //    {
        //        this.ObjectResultModule.Object = result;
        //        this.ObjectResultModule.StatusCode = 200;
        //        this.ObjectResultModule.Message = "success";
        //    }
        //    else
        //    {
        //        this.ObjectResultModule.Object = "";
        //        this.ObjectResultModule.StatusCode = 400;
        //        this.ObjectResultModule.Message = "error!";
        //    }
        //    return ObjectResultModule;
        //}
        ///// <summary>
        ///// 医生上下线设置记录log 修改
        ///// </summary>
        ///// <param name="DoctorOnlineSetLogInfo"></param>
        ///// <returns></returns>
        //[Route("api/UpdateDoctorOnlineSetLog")]
        //[HttpPost]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> UpdateDoctorOnlineSetLog([FromBody]  DoctorOnlineSetLog DoctorOnlineSetLogInfo)
        //{
        //    if (!Commons.CheckSecret(DoctorOnlineSetLogInfo.Secret))
        //    {
        //        this.ObjectResultModule.StatusCode = 422;
        //        this.ObjectResultModule.Message = "Wrong Secret";
        //        this.ObjectResultModule.Object = "";
        //        return this.ObjectResultModule;
        //    }
        //    var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
        //    var UpdateDoctorOnlineSetLog = await _doctorOnlineSetLogService.DoctorOnlineSetLogByID(DoctorOnlineSetLogInfo.Id);
        //    if (UpdateDoctorOnlineSetLog != null)
        //    {
        //        UpdateDoctorOnlineSetLog.DoctorName = DoctorOnlineSetLogInfo.DoctorName;
        //        UpdateDoctorOnlineSetLog.DoctorID = DoctorOnlineSetLogInfo.DoctorID;
        //        UpdateDoctorOnlineSetLog.DoctorJSON = DoctorOnlineSetLogInfo.DoctorJSON;
        //        UpdateDoctorOnlineSetLog.OnlineState = DoctorOnlineSetLogInfo.OnlineState;
        //        UpdateDoctorOnlineSetLog.DivideInto = DoctorOnlineSetLogInfo.DivideInto;
        //        UpdateDoctorOnlineSetLog.IncomeDay = DoctorOnlineSetLogInfo.IncomeDay;
        //        UpdateDoctorOnlineSetLog.Remark = DoctorOnlineSetLogInfo.Remark;
        //        UpdateDoctorOnlineSetLog.CheckState = DoctorOnlineSetLogInfo.CheckState;
        //        UpdateDoctorOnlineSetLog.CheckRemark = DoctorOnlineSetLogInfo.CheckRemark;
        //        UpdateDoctorOnlineSetLog.CheckTime = DoctorOnlineSetLogInfo.CheckTime;
        //        UpdateDoctorOnlineSetLog.Checker = DoctorOnlineSetLogInfo.CreatedBy;
        //        UpdateDoctorOnlineSetLog.ModifyOn = DateTime.Now;
        //        UpdateDoctorOnlineSetLog.ModifyBy = userid;
        //        var result = await _doctorOnlineSetLogService.UpdateDoctorOnlineSetLog(UpdateDoctorOnlineSetLog);

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
        //    return ObjectResultModule;
        //}
        ///// <summary>
        ///// 医生上下线设置记录log 删除
        ///// </summary>
        ///// <param name="DoctorOnlineSetLogInfo"></param>
        ///// <returns></returns>
        //[Route("api/DeleteDoctorOnlineSetLog")]
        //[HttpPost]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> DeleteDoctorOnlineSetLog([FromBody]  DoctorOnlineSetLog DoctorOnlineSetLogInfo)
        //{
        //    if (!Commons.CheckSecret(DoctorOnlineSetLogInfo.Secret))
        //    {
        //        this.ObjectResultModule.StatusCode = 422;
        //        this.ObjectResultModule.Message = "Wrong Secret";
        //        this.ObjectResultModule.Object = "";
        //        return this.ObjectResultModule;
        //    }
        //    var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
        //    var query = await _doctorOnlineSetLogService.DoctorOnlineSetLogByID(DoctorOnlineSetLogInfo.Id);
        //    if (query != null)
        //    {
        //        query.DeleteBy = userid;
        //        query.DeleteTime = DateTime.Now;
        //        query.IsDelete = true;
        //        var res = await _doctorOnlineSetLogService.DeleteDoctorOnlineSetLog(query);

        //        this.ObjectResultModule.Object = res;
        //        this.ObjectResultModule.Message = "sucess";
        //        this.ObjectResultModule.StatusCode = 200;
        //    }
        //    else
        //    {
        //        this.ObjectResultModule.Message = "NotFound";
        //        this.ObjectResultModule.StatusCode = 404;
        //        this.ObjectResultModule.Object = "";
        //    }
        //    return this.ObjectResultModule;
        //}
        #endregion

        /// <summary>
        /// 医生上下线设置记录log Page
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOnlineSetLogPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOnlineSetLogPage([FromBody] DoctorOnlineSetLogIn DoctorOnlineSetLogInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineSetLogInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DoctorOnlineSetLogInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(DoctorOnlineSetLogInfo.KeyWord))
            {
                DoctorOnlineSetLogInfo.AndAlso(a => a.DoctorName.Contains(DoctorOnlineSetLogInfo.KeyWord));
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorOnlineSetLogInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorOnlineSetLogInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorOnlineSetLogInfo.EndTime))
                {
                    DoctorOnlineSetLogInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorOnlineSetLogInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorOnlineSetLogInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorOnlineSetLogInfo.StartTime))
            {
                DoctorOnlineSetLogInfo.AndAlso(t => t.CreatedOn >= StartTime);
                DoctorOnlineSetLogInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            var values = await _doctorOnlineSetLogService.DoctorOnlineSetLogPage(DoctorOnlineSetLogInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorOnlineSetLogOut(values, DoctorOnlineSetLogInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生上下线设置记录log List 
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOnlineSetLogList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOnlineSetLogList([FromBody]DoctorOnlineSetLogIn DoctorOnlineSetLogInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineSetLogInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DoctorOnlineSetLogInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(DoctorOnlineSetLogInfo.KeyWord))
            {
                DoctorOnlineSetLogInfo.AndAlso(a => a.DoctorName.Contains(DoctorOnlineSetLogInfo.KeyWord));
            }
            var values = await _doctorOnlineSetLogService.DoctorOnlineSetLogList(DoctorOnlineSetLogInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生上下线设置记录log Byid
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOnlineSetLogById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOnlineSetLogById([FromBody]DoctorOnlineSetLogIn DoctorOnlineSetLogInfo)
        {
            if (!Commons.CheckSecret(DoctorOnlineSetLogInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _doctorOnlineSetLogService.DoctorOnlineSetLogByID(DoctorOnlineSetLogInfo.Id);
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
            return ObjectResultModule;
        }
        #endregion

        #region 医生审核
        /// <summary>
        /// 医生审核 新增
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorCheck")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorCheck([FromBody] DoctorCheck DoctorCheckInfo)
        {
            if (!Commons.CheckSecret(DoctorCheckInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateDoctorCheck = new DoctorCheck()
            {
                DoctorName = DoctorCheckInfo.DoctorName,
                DoctorID = DoctorCheckInfo.DoctorID,
                DoctorJSon = DoctorCheckInfo.DoctorJSon,
                CheckType = DoctorCheckInfo.CheckType,
                CheckState = DoctorCheckInfo.CheckState,
                CheckRemark = DoctorCheckInfo.CheckRemark,
                CheckTime = DoctorCheckInfo.CheckTime,
                Checker = DoctorCheckInfo.Checker,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _doctorCheckService.CreateDoctorCheck(CreateDoctorCheck);
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
                OperExplain = "CreateDoctorCheck",
                OperContent = JsonHelper.ToJson(DoctorCheckInfo),
                OperType = "CreateDoctorCheck",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生审核 修改
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorCheck")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorCheck([FromBody]  DoctorCheck DoctorCheckInfo)
        {
            if (!Commons.CheckSecret(DoctorCheckInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateDoctorCheck = await _doctorCheckService.DoctorCheckByID(DoctorCheckInfo.Id);
            if (UpdateDoctorCheck != null)
            {
                UpdateDoctorCheck.DoctorName = DoctorCheckInfo.DoctorName;
                UpdateDoctorCheck.DoctorID = DoctorCheckInfo.DoctorID;
                UpdateDoctorCheck.DoctorJSon = DoctorCheckInfo.DoctorJSon;
                UpdateDoctorCheck.CheckType = DoctorCheckInfo.CheckType;
                UpdateDoctorCheck.CheckState = DoctorCheckInfo.CheckState;
                UpdateDoctorCheck.CheckRemark = DoctorCheckInfo.CheckRemark;
                UpdateDoctorCheck.CheckTime = DoctorCheckInfo.CheckTime;
                UpdateDoctorCheck.Checker = DoctorCheckInfo.Checker;
                UpdateDoctorCheck.CreatedBy = DoctorCheckInfo.CreatedBy;
                UpdateDoctorCheck.CreatedOn = DoctorCheckInfo.CreatedOn;
                UpdateDoctorCheck.ModifyOn = DateTime.Now;
                UpdateDoctorCheck.ModifyBy = userid;
                var result = await _doctorCheckService.UpdateDoctorCheck(UpdateDoctorCheck);
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
                OperExplain = "UpdateDoctorCheck",
                OperContent = JsonHelper.ToJson(DoctorCheckInfo),
                OperType = "UpdateDoctorCheck",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生审核 删除
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorCheck")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorCheck([FromBody]  DoctorCheck DoctorCheckInfo)
        {
            if (!Commons.CheckSecret(DoctorCheckInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorCheckService.DoctorCheckByID(DoctorCheckInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _doctorCheckService.DeleteDoctorCheck(query);

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
                OperExplain = "DeleteDoctorCheck",
                OperContent = JsonHelper.ToJson(DoctorCheckInfo),
                OperType = "DeleteDoctorCheck",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生审核 Page
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorCheckPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorCheckPage([FromBody] DoctorCheckIn DoctorCheckInfo)
        {
            if (!Commons.CheckSecret(DoctorCheckInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorCheckInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorCheckInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorCheckInfo.EndTime))
                {
                    DoctorCheckInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorCheckInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.StartTime))
            {
                DoctorCheckInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.KeyWord))
            {
                DoctorCheckInfo.AndAlso(t => t.DoctorName.Contains(DoctorCheckInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.CheckType))
            {
                DoctorCheckInfo.AndAlso(t => t.CheckType.Contains(DoctorCheckInfo.CheckType));
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.CheckState))
            {
                DoctorCheckInfo.AndAlso(t => t.CheckState.Contains(DoctorCheckInfo.CheckState));
            }
            DoctorCheckInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorCheckService.DoctorCheckPage(DoctorCheckInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorCheckOut(values, DoctorCheckInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorCheckPage",
                OperContent = JsonHelper.ToJson(DoctorCheckInfo),
                OperType = "DoctorCheckPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生审核 List 
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorCheckList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorCheckList([FromBody]DoctorCheckIn DoctorCheckInfo)
        {
            if (!Commons.CheckSecret(DoctorCheckInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorCheckInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorCheckInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorCheckInfo.EndTime))
                {
                    DoctorCheckInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorCheckInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.StartTime))
            {
                DoctorCheckInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.KeyWord))
            {
                DoctorCheckInfo.AndAlso(t => t.DoctorName.Contains(DoctorCheckInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.CheckType))
            {
                DoctorCheckInfo.AndAlso(t => t.CheckType.Contains(DoctorCheckInfo.CheckType));
            }
            if (!string.IsNullOrEmpty(DoctorCheckInfo.CheckState))
            {
                DoctorCheckInfo.AndAlso(t => t.CheckState.Contains(DoctorCheckInfo.CheckState));
            }
            DoctorCheckInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorCheckService.DoctorCheckList(DoctorCheckInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生审核 Byid
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorCheckById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorCheckById([FromBody]DoctorCheckIn DoctorCheckInfo)
        {
            if (!Commons.CheckSecret(DoctorCheckInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _doctorCheckService.DoctorCheckByID(DoctorCheckInfo.Id);
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
            return ObjectResultModule;
        }
        #endregion

        #region 地区管理
        /// <summary>
        /// 地区管理 新增
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateAreaManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateAreaManage([FromBody] AreaManage AreaManageInfo)
        {
            if (!Commons.CheckSecret(AreaManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateAreaManage = new AreaManage()
            {
                Code = AreaManageInfo.Code,
                Name = AreaManageInfo.Name,
                Remark = AreaManageInfo.Remark,
                Postcode = AreaManageInfo.Postcode,
                CreatedBy = userid,
                CreatedOn = DateTime.Now

            };
            var result = await _areaManageService.CreateAreaManage(CreateAreaManage);
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
        /// 地区管理 修改
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateAreaManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateAreaManage([FromBody]  AreaManage AreaManageInfo)
        {
            if (!Commons.CheckSecret(AreaManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            TencentUserManage usermanage = new TencentUserManage();
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateAreaManage = await _areaManageService.AreaManageByID(AreaManageInfo.Id);
            if (UpdateAreaManage != null)
            {
                UpdateAreaManage.Code = AreaManageInfo.Code;
                UpdateAreaManage.Name = AreaManageInfo.Name;
                UpdateAreaManage.Remark = AreaManageInfo.Remark;
                UpdateAreaManage.Postcode = AreaManageInfo.Postcode;
                UpdateAreaManage.ModifyOn = DateTime.Now;
                UpdateAreaManage.ModifyBy = userid;
                var result = await _areaManageService.UpdateAreaManage(UpdateAreaManage);

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
        /// 地区管理 删除
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteAreaManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteAreaManage([FromBody]  AreaManage AreaManageInfo)
        {
            if (!Commons.CheckSecret(AreaManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _areaManageService.AreaManageByID(AreaManageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _areaManageService.DeleteAreaManage(query);

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
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 地区管理 Page
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [Route("api/AreaManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AreaManagePage([FromBody] AreaManageIn AreaManageInfo)
        {
            if (!Commons.CheckSecret(AreaManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            AreaManageInfo.AndAlso(t => t.IsDelete == false);
            var values = await _areaManageService.AreaManagePage(AreaManageInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new AreaManageOut(values, AreaManageInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 地区管理 List 
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [Route("api/AreaManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AreaManageList([FromBody]AreaManageIn AreaManageInfo)
        {
            if (!Commons.CheckSecret(AreaManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            AreaManageInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(AreaManageInfo.KeyWord))
            {
                AreaManageInfo.AndAlso(a => a.Code.Contains(AreaManageInfo.KeyWord) || a.Name.Contains(AreaManageInfo.KeyWord) || a.Postcode.Contains(AreaManageInfo.KeyWord));
            }
            var values = await _areaManageService.AreaManageList(AreaManageInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 地区管理 Byid
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [Route("api/AreaManageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AreaManageById([FromBody]AreaManageIn AreaManageInfo)
        {
            if (!Commons.CheckSecret(AreaManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _areaManageService.AreaManageByID(AreaManageInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        #endregion

        #region 医生提现记录表
        /// <summary>
        /// 医生提现记录表 新增
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorWithdrawRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorWithdrawRecord([FromBody] DoctorWithdrawRecord DoctorWithdrawRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorWithdrawRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateDoctorWithdrawRecord = new DoctorWithdrawRecord()
            {
                SequenceNo = DoctorWithdrawRecordInfo.SequenceNo,
                DoctorName = DoctorWithdrawRecordInfo.DoctorName,
                DoctorID = DoctorWithdrawRecordInfo.DoctorID,
                DoctorJSON = DoctorWithdrawRecordInfo.DoctorJSON,
                WithdrawMoney = DoctorWithdrawRecordInfo.WithdrawMoney,
                WithdrawTime = DoctorWithdrawRecordInfo.WithdrawTime,
                CheckState = DoctorWithdrawRecordInfo.CheckState,
                CheckRemark = DoctorWithdrawRecordInfo.CheckRemark,
                Checker = DoctorWithdrawRecordInfo.CreatedBy,
                CheckTime = DoctorWithdrawRecordInfo.CheckTime,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
            };
            var result = await _doctorWithdrawRecordService.CreateDoctorWithdrawRecord(CreateDoctorWithdrawRecord);
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
                OperExplain = "CreateDoctorWithdrawRecord",
                OperContent = JsonHelper.ToJson(DoctorWithdrawRecordInfo),
                OperType = "CreateDoctorWithdrawRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生提现记录表 修改
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorWithdrawRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorWithdrawRecord([FromBody]  DoctorWithdrawRecord DoctorWithdrawRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorWithdrawRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateDoctorWithdrawRecord = await _doctorWithdrawRecordService.DoctorWithdrawRecordByID(DoctorWithdrawRecordInfo.Id);
            if (UpdateDoctorWithdrawRecord != null)
            {
                UpdateDoctorWithdrawRecord.SequenceNo = DoctorWithdrawRecordInfo.SequenceNo;
                UpdateDoctorWithdrawRecord.DoctorName = DoctorWithdrawRecordInfo.DoctorName;
                UpdateDoctorWithdrawRecord.DoctorID = DoctorWithdrawRecordInfo.DoctorID;
                UpdateDoctorWithdrawRecord.DoctorJSON = DoctorWithdrawRecordInfo.DoctorJSON;
                UpdateDoctorWithdrawRecord.WithdrawMoney = DoctorWithdrawRecordInfo.WithdrawMoney;
                UpdateDoctorWithdrawRecord.WithdrawTime = DoctorWithdrawRecordInfo.WithdrawTime;
                UpdateDoctorWithdrawRecord.CheckState = DoctorWithdrawRecordInfo.CheckState;
                UpdateDoctorWithdrawRecord.CheckRemark = DoctorWithdrawRecordInfo.CheckRemark;
                UpdateDoctorWithdrawRecord.Checker = DoctorWithdrawRecordInfo.CreatedBy;
                UpdateDoctorWithdrawRecord.CheckTime = DoctorWithdrawRecordInfo.CheckTime;
                UpdateDoctorWithdrawRecord.ModifyOn = DateTime.Now;
                UpdateDoctorWithdrawRecord.ModifyBy = userid;
                var result = await _doctorWithdrawRecordService.UpdateDoctorWithdrawRecord(UpdateDoctorWithdrawRecord);

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
                OperExplain = "UpdateDoctorWithdrawRecord",
                OperContent = JsonHelper.ToJson(DoctorWithdrawRecordInfo),
                OperType = "UpdateDoctorWithdrawRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生提现记录表 删除
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorWithdrawRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorWithdrawRecord([FromBody]  DoctorWithdrawRecord DoctorWithdrawRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorWithdrawRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorWithdrawRecordService.DoctorWithdrawRecordByID(DoctorWithdrawRecordInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _doctorWithdrawRecordService.DeleteDoctorWithdrawRecord(query);

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
                OperExplain = "DeleteDoctorWithdrawRecord",
                OperContent = JsonHelper.ToJson(DoctorWithdrawRecordInfo),
                OperType = "DeleteDoctorWithdrawRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生提现记录表 Page
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorWithdrawRecordPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorWithdrawRecordPage([FromBody] DoctorWithdrawRecordIn DoctorWithdrawRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorWithdrawRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (DoctorWithdrawRecordInfo.Platform == "PC")
            {
                // 判断当角色为医生角色时
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    DoctorWithdrawRecordInfo.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
            }
            else if (DoctorWithdrawRecordInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    DoctorWithdrawRecordInfo.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorWithdrawRecordInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorWithdrawRecordInfo.EndTime))
                {
                    DoctorWithdrawRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorWithdrawRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.StartTime))
            {
                DoctorWithdrawRecordInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.KeyWord))
            {
                DoctorWithdrawRecordInfo.AndAlso(t => t.DoctorName.Contains(DoctorWithdrawRecordInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.SequenceNo))
            {
                DoctorWithdrawRecordInfo.AndAlso(t => t.SequenceNo.Contains(DoctorWithdrawRecordInfo.SequenceNo));
            }
            DoctorWithdrawRecordInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorWithdrawRecordService.DoctorWithdrawRecordPage(DoctorWithdrawRecordInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorWithdrawRecordOut(values, DoctorWithdrawRecordInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorWithdrawRecordPage",
                OperContent = JsonHelper.ToJson(DoctorWithdrawRecordInfo),
                OperType = "DoctorWithdrawRecordPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生提现记录表 List 
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorWithdrawRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorWithdrawRecordList([FromBody]DoctorWithdrawRecordIn DoctorWithdrawRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorWithdrawRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorWithdrawRecordInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorWithdrawRecordInfo.EndTime))
                {
                    DoctorWithdrawRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorWithdrawRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.StartTime))
            {
                DoctorWithdrawRecordInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.KeyWord))
            {
                DoctorWithdrawRecordInfo.AndAlso(t => t.DoctorName.Contains(DoctorWithdrawRecordInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(DoctorWithdrawRecordInfo.SequenceNo))
            {
                DoctorWithdrawRecordInfo.AndAlso(t => t.SequenceNo.Contains(DoctorWithdrawRecordInfo.SequenceNo));
            }
            DoctorWithdrawRecordInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorWithdrawRecordService.DoctorWithdrawRecordList(DoctorWithdrawRecordInfo);
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
                OperExplain = "DoctorWithdrawRecordList",
                OperContent = JsonHelper.ToJson(DoctorWithdrawRecordInfo),
                OperType = "DoctorWithdrawRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生提现记录表 Byid
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorWithdrawRecordById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorWithdrawRecordById([FromBody]DoctorWithdrawRecordIn DoctorWithdrawRecordInfo)
        {
            if (!Commons.CheckSecret(DoctorWithdrawRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _doctorWithdrawRecordService.DoctorWithdrawRecordByID(DoctorWithdrawRecordInfo.Id);
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
                OperExplain = "DoctorWithdrawRecordById",
                OperContent = JsonHelper.ToJson(DoctorWithdrawRecordInfo),
                OperType = "DoctorWithdrawRecordById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 系统参数设置
        /// <summary>
        /// 系统参数设置 新增
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/CreateSystemParameter")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateSystemParameter([FromBody] SystemParameter SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var systemparaterin = new SystemParameterIn(); systemparaterin.AndAlso(t => !t.IsDelete && t.SystemCode == SystemParameterInfo.SystemCode && t.Code == SystemParameterInfo.Code);
            var check = await _systemParameterService.PatientParameterList(systemparaterin);
            if (check != null && check.Count() > 0)
            {
                return new ObjectResultModule("", 100, "重复数据,不允许重新添加！");
            }
            var CreateSystemParameter = new SystemParameter()
            {
                SystemType = SystemParameterInfo.SystemType,
                SystemCode = SystemParameterInfo.SystemCode,
                Code = SystemParameterInfo.Code,
                Name = SystemParameterInfo.Name,
                Remark = SystemParameterInfo.Remark,
                ItemValue = SystemParameterInfo.ItemValue,
                Parameter = SystemParameterInfo.Parameter,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _systemParameterService.CreateSystemParameter(CreateSystemParameter);

            if (SystemParameterInfo.SystemCode == "WXRole")
            {   //微信端 标签新增
                #region 获取WecharToken
                // SystemToken systemToken = new SystemToken();
                // systemToken.TokenType = "Wechar";
                var Tokens = await _systemTokenService.SystemTokenList("Wechar");
                #endregion
                TencentUserManage Usermanage = new TencentUserManage();
                var tag = new TagDetail(); tag.name = SystemParameterInfo.Code;
                CreateTagDetail tagDetail = await Usermanage.CreateWeiXinTag(tag, Tokens.access_token);

                result.ItemValue = JsonHelper.ToJson(tagDetail.tag);


                await _systemParameterService.UpdateSystemParameter(result);


            }
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
                OperExplain = "CreateSystemParameter",
                OperContent = JsonHelper.ToJson(SystemParameterInfo),
                OperType = "CreateSystemParameter",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 系统参数设置 修改
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateSystemParameter")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateSystemParameter([FromBody]  SystemParameter SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateSystemParameter = await _systemParameterService.SystemParameterByID(SystemParameterInfo.Id);
            if (UpdateSystemParameter != null)
            {
                UpdateSystemParameter.Name = SystemParameterInfo.Name;
                UpdateSystemParameter.Remark = SystemParameterInfo.Remark;
                UpdateSystemParameter.Parameter = SystemParameterInfo.Parameter;
                UpdateSystemParameter.ItemValue = SystemParameterInfo.ItemValue;
                UpdateSystemParameter.ModifyOn = DateTime.Now;
                UpdateSystemParameter.ModifyBy = userid;
                var result = await _systemParameterService.UpdateSystemParameter(UpdateSystemParameter);

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
                OperExplain = "UpdateSystemParameter",
                OperContent = JsonHelper.ToJson(SystemParameterInfo),
                OperType = "UpdateSystemParameter",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 系统参数设置 删除
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteSystemParameter")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteSystemParameter([FromBody]  SystemParameter SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (SystemParameterInfo.SystemCode == "WXRole")
            { return new ObjectResultModule("", 400, "移动端角色不允许删除,若要删除请联系管理员！"); }
            var query = await _systemParameterService.SystemParameterByID(SystemParameterInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _systemParameterService.DeleteSystemParameter(query);

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
                OperExplain = "DeleteSystemParameter",
                OperContent = JsonHelper.ToJson(SystemParameterInfo),
                OperType = "DeleteSystemParameter",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 系统参数设置 Page
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/SystemParameterPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SystemParameterPage([FromBody] SystemParameterIn SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SystemParameterInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(SystemParameterInfo.KeyWord))
            {
                SystemParameterInfo.AndAlso(t => t.SystemCode.Contains(SystemParameterInfo.KeyWord)
                                              || t.SystemType.Contains(SystemParameterInfo.KeyWord)
                                              || t.ItemValue.Contains(SystemParameterInfo.KeyWord)
                                              || t.Code.Contains(SystemParameterInfo.KeyWord)
                                              || t.Name.Contains(SystemParameterInfo.KeyWord));
            }
            SystemParameterInfo.AndAlso(t => t.IsDelete == false);
            var values = await _systemParameterService.SystemParameterPage(SystemParameterInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new SystemParameterOut(values, SystemParameterInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "SystemParameterPage",
                OperContent = JsonHelper.ToJson(SystemParameterInfo),
                OperType = "SystemParameterPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 系统参数设置 List 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/SystemParameterList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SystemParameterList([FromBody]SystemParameterIn SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SystemParameterInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(SystemParameterInfo.KeyWord))
            {
                SystemParameterInfo.AndAlso(t => t.SystemCode.Contains(SystemParameterInfo.KeyWord)
                                              || t.SystemType.Contains(SystemParameterInfo.KeyWord)
                                              || t.ItemValue.Contains(SystemParameterInfo.KeyWord)
                                              || t.Code.Contains(SystemParameterInfo.KeyWord)
                                              || t.Name.Contains(SystemParameterInfo.KeyWord));
            }
            SystemParameterInfo.AndAlso(t => t.IsDelete == false);
            var values = await _systemParameterService.SystemParameterList(SystemParameterInfo);
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
                OperExplain = "SystemParameterList",
                OperContent = JsonHelper.ToJson(SystemParameterInfo),
                OperType = "SystemParameterList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 系统参数设置 Byid
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/SystemParameterById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SystemParameterById([FromBody]SystemParameterIn SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _systemParameterService.SystemParameterByID(SystemParameterInfo.Id);
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
                OperExplain = "SystemParameterById",
                OperContent = JsonHelper.ToJson(SystemParameterInfo),
                OperType = "SystemParameterById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 医生参数设置
        /// <summary>
        /// 医生参数设置 新增
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorParaSet")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorParaSet([FromBody] DoctorParaSet DoctorParaSetInfo)
        {
            if (!Commons.CheckSecret(DoctorParaSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateDoctorParaSet = new DoctorParaSet()
            {
                DoctorParaSetCode = DoctorParaSetInfo.DoctorParaSetCode,
                DoctorParaSetName = DoctorParaSetInfo.DoctorParaSetName,
                ItemValue = DoctorParaSetInfo.ItemValue,
                CreatedBy = userid,
                CreatedOn = DateTime.Now

            };
            var result = await _doctorParaSetService.CreateDoctorParaSet(CreateDoctorParaSet);
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
                OperExplain = "CreateDoctorParaSet",
                OperContent = JsonHelper.ToJson(DoctorParaSetInfo),
                OperType = "CreateDoctorParaSet",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生参数设置 修改
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorParaSet")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorParaSet([FromBody]  DoctorParaSet DoctorParaSetInfo)
        {
            if (!Commons.CheckSecret(DoctorParaSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateDoctorParaSet = await _doctorParaSetService.DoctorParaSetByID(DoctorParaSetInfo.Id);
            if (UpdateDoctorParaSet != null)
            {
                UpdateDoctorParaSet.DoctorParaSetName = DoctorParaSetInfo.DoctorParaSetName;
                UpdateDoctorParaSet.ItemValue = DoctorParaSetInfo.ItemValue;
                UpdateDoctorParaSet.ModifyOn = DateTime.Now;
                UpdateDoctorParaSet.ModifyBy = userid;
                var result = await _doctorParaSetService.UpdateDoctorParaSet(UpdateDoctorParaSet);

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
                OperExplain = "UpdateDoctorParaSet",
                OperContent = JsonHelper.ToJson(DoctorParaSetInfo),
                OperType = "UpdateDoctorParaSet",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生参数设置 删除
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorParaSet")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorParaSet([FromBody]  DoctorParaSet DoctorParaSetInfo)
        {
            if (!Commons.CheckSecret(DoctorParaSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorParaSetService.DoctorParaSetByID(DoctorParaSetInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _doctorParaSetService.DeleteDoctorParaSet(query);

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
                OperExplain = "DeleteDoctorParaSet",
                OperContent = JsonHelper.ToJson(DoctorParaSetInfo),
                OperType = "DeleteDoctorParaSet",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生参数设置 Page
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorParaSetPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorParaSetPage([FromBody] DoctorParaSetIn DoctorParaSetInfo)
        {
            if (!Commons.CheckSecret(DoctorParaSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorParaSetInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(DoctorParaSetInfo.KeyWord))
            {
                DoctorParaSetInfo.AndAlso(t => t.DoctorParaSetCode.Contains(DoctorParaSetInfo.KeyWord)
                                              || t.DoctorParaSetName.Contains(DoctorParaSetInfo.KeyWord)
                                              || t.ItemValue.Contains(DoctorParaSetInfo.KeyWord));
            }
            DoctorParaSetInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorParaSetService.DoctorParaSetPage(DoctorParaSetInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorParaSetOut(values, DoctorParaSetInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorParaSetPage",
                OperContent = JsonHelper.ToJson(DoctorParaSetInfo),
                OperType = "DoctorParaSetPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生参数设置 List 
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorParaSetList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorParaSetList([FromBody]DoctorParaSetIn DoctorParaSetInfo)
        {
            if (!Commons.CheckSecret(DoctorParaSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorParaSetInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(DoctorParaSetInfo.KeyWord))
            {
                DoctorParaSetInfo.AndAlso(t => t.DoctorParaSetCode.Contains(DoctorParaSetInfo.KeyWord)
                                              || t.DoctorParaSetName.Contains(DoctorParaSetInfo.KeyWord)
                                              || t.ItemValue.Contains(DoctorParaSetInfo.KeyWord));
            }
            DoctorParaSetInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorParaSetService.DoctorParaSetList(DoctorParaSetInfo);
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
                OperExplain = "DoctorParaSetList",
                OperContent = JsonHelper.ToJson(DoctorParaSetInfo),
                OperType = "DoctorParaSetList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 医生参数设置 Byid
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorParaSetById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorParaSetById([FromBody]DoctorParaSetIn DoctorParaSetInfo)
        {
            if (!Commons.CheckSecret(DoctorParaSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _doctorParaSetService.DoctorParaSetByID(DoctorParaSetInfo.Id);
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
                OperExplain = "DoctorParaSetById",
                OperContent = JsonHelper.ToJson(DoctorParaSetInfo),
                OperType = "DoctorParaSetById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 对接接口设置
        /// <summary>
        /// 对接接口设置 新增
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [Route("api/CreateInterfaceSet")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateInterfaceSet([FromBody] InterfaceSet InterfaceSetInfo)
        {
            if (!Commons.CheckSecret(InterfaceSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateInterfaceSet = new InterfaceSet()
            {
                InterfaceName = InterfaceSetInfo.InterfaceName,
                InterfaceIntro = InterfaceSetInfo.InterfaceIntro,
                InterfaceAddress = InterfaceSetInfo.InterfaceAddress,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _interfaceSetService.CreateInterfaceSet(CreateInterfaceSet);
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
        /// 对接接口设置 修改
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateInterfaceSet")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateInterfaceSet([FromBody]  InterfaceSet InterfaceSetInfo)
        {
            if (!Commons.CheckSecret(InterfaceSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateInterfaceSet = await _interfaceSetService.InterfaceSetByID(InterfaceSetInfo.Id);
            if (UpdateInterfaceSet != null)
            {
                UpdateInterfaceSet.InterfaceName = InterfaceSetInfo.InterfaceName;
                UpdateInterfaceSet.InterfaceIntro = InterfaceSetInfo.InterfaceIntro;
                UpdateInterfaceSet.InterfaceAddress = InterfaceSetInfo.InterfaceAddress;

                UpdateInterfaceSet.ModifyOn = DateTime.Now;
                UpdateInterfaceSet.ModifyBy = userid;
                var result = await _interfaceSetService.UpdateInterfaceSet(UpdateInterfaceSet);

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
        /// 对接接口设置 删除
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteInterfaceSet")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteInterfaceSet([FromBody]  InterfaceSet InterfaceSetInfo)
        {
            if (!Commons.CheckSecret(InterfaceSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _interfaceSetService.InterfaceSetByID(InterfaceSetInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _interfaceSetService.DeleteInterfaceSet(query);

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
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 对接接口设置 Page
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [Route("api/InterfaceSetPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> InterfaceSetPage([FromBody] InterfaceSetIn InterfaceSetInfo)
        {
            if (!Commons.CheckSecret(InterfaceSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(InterfaceSetInfo.StartTime))
            {
                StartTime = DateTime.Parse(InterfaceSetInfo.StartTime);
                if (string.IsNullOrEmpty(InterfaceSetInfo.EndTime))
                {
                    InterfaceSetInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.EndTime))
            {
                EndTime = DateTime.Parse(InterfaceSetInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.StartTime))
            {
                InterfaceSetInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.KeyWord))
            {
                InterfaceSetInfo.AndAlso(t => t.InterfaceName.Contains(InterfaceSetInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.InterfaceName))
            {
                InterfaceSetInfo.AndAlso(t => t.InterfaceName.Contains(InterfaceSetInfo.InterfaceName));
            }
            InterfaceSetInfo.AndAlso(t => t.IsDelete == false);
            var values = await _interfaceSetService.InterfaceSetPage(InterfaceSetInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new InterfaceSetOut(values, InterfaceSetInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 对接接口设置 List 
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [Route("api/InterfaceSetList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> InterfaceSetList([FromBody]InterfaceSetIn InterfaceSetInfo)
        {
            if (!Commons.CheckSecret(InterfaceSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(InterfaceSetInfo.StartTime))
            {
                StartTime = DateTime.Parse(InterfaceSetInfo.StartTime);
                if (string.IsNullOrEmpty(InterfaceSetInfo.EndTime))
                {
                    InterfaceSetInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.EndTime))
            {
                EndTime = DateTime.Parse(InterfaceSetInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.StartTime))
            {
                InterfaceSetInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.KeyWord))
            {
                InterfaceSetInfo.AndAlso(t => t.InterfaceName.Contains(InterfaceSetInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(InterfaceSetInfo.InterfaceName))
            {
                InterfaceSetInfo.AndAlso(t => t.InterfaceName.Contains(InterfaceSetInfo.InterfaceName));
            }
            InterfaceSetInfo.AndAlso(t => t.IsDelete == false);
            var values = await _interfaceSetService.InterfaceSetList(InterfaceSetInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 对接接口设置 Byid
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [Route("api/InterfaceSetById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> InterfaceSetById([FromBody]InterfaceSetIn InterfaceSetInfo)
        {
            if (!Commons.CheckSecret(InterfaceSetInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _interfaceSetService.InterfaceSetByID(InterfaceSetInfo.Id);
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
            return ObjectResultModule;
        }
        #endregion

        #region 操作日志表
        /// <summary>
        /// 操作日志表 新增
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherOperList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherOperList([FromBody] YaeherOperList YaeherOperListInfo)
        {
            if (!Commons.CheckSecret(YaeherOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = YaeherOperListInfo.OperExplain,
                OperContent = YaeherOperListInfo.OperContent,
                OperType = YaeherOperListInfo.OperType,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
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
        /// 操作日志表 Page
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherOperListPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherOperListPage([FromBody] YaeherOperListIn YaeherOperListInfo)
        {
            if (!Commons.CheckSecret(YaeherOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherOperListInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherOperListInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherOperListInfo.EndTime))
                {
                    YaeherOperListInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherOperListInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.StartTime))
            {
                YaeherOperListInfo.AndAlso(t => t.CreatedOn >= StartTime);
                YaeherOperListInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.KeyWord))
            {
                YaeherOperListInfo.AndAlso(t => t.OperContent.Contains(YaeherOperListInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.OperType))
            {
                YaeherOperListInfo.AndAlso(t => t.OperType.Contains(YaeherOperListInfo.OperType));
            }
            YaeherOperListInfo.AndAlso(t => t.IsDelete == false);
            var values = await _yaeherOperListService.YaeherOperListPage(YaeherOperListInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new YaeherOperListOut(values, YaeherOperListInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 操作日志表 List 
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherOperLists")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherOperLists([FromBody]YaeherOperListIn YaeherOperListInfo)
        {
            if (!Commons.CheckSecret(YaeherOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherOperListInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherOperListInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherOperListInfo.EndTime))
                {
                    YaeherOperListInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherOperListInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.StartTime))
            {
                YaeherOperListInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.KeyWord))
            {
                YaeherOperListInfo.AndAlso(t => t.OperContent.Contains(YaeherOperListInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(YaeherOperListInfo.OperType))
            {
                YaeherOperListInfo.AndAlso(t => t.OperType.Contains(YaeherOperListInfo.OperType));
            }
            YaeherOperListInfo.AndAlso(t => t.IsDelete == false);
            var values = await _yaeherOperListService.YaeherOperListList(YaeherOperListInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 操作日志表 Byid
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherOperListById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherOperListById([FromBody]YaeherOperListIn YaeherOperListInfo)
        {
            if (!Commons.CheckSecret(YaeherOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _yaeherOperListService.YaeherOperListByID(YaeherOperListInfo.Id);
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
            return ObjectResultModule;
        }
        #endregion

        #region 短信记录表
        /// <summary>
        /// 短信记录表 新增
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherMessage([FromBody] YaeherMessageRemind YaeherMessageRemindInfo)
        {
            if (!Commons.CheckSecret(YaeherMessageRemindInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var User = await _yaeherUserService.YaeherUserByID(userid);
            YaeherUserIn yaeherUserIn = new YaeherUserIn();
            yaeherUserIn.AndAlso(a => a.IsDelete == false);
            yaeherUserIn.AndAlso(a => a.PhoneNumber == YaeherMessageRemindInfo.PhoneNumber);
            var UserInfos = await _yaeherUserService.YaeherUserList(yaeherUserIn);
            if (UserInfos.Count > 0)
            {
                var UserInfo = UserInfos.FirstOrDefault();
                if (UserInfo.Id != userid)
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "该电话号码已被使用，请更换其他号码！";
                    return ObjectResultModule;
                }
            }
            YaeherMessageRemind CreateYaeherMessageRemind = new YaeherMessageRemind();
            CreateYaeherMessageRemind.UserID = User.Id;
            CreateYaeherMessageRemind.UserName = User.LoginName;
            CreateYaeherMessageRemind.PhoneNumber = YaeherMessageRemindInfo.PhoneNumber;
            CreateYaeherMessageRemind.MessageType = YaeherMessageRemindInfo.MessageType;
            CreateYaeherMessageRemind.VerificationCode = new RandomCode().GenerateCheckCodeNum(6);
            CreateYaeherMessageRemind.Message = YaeherMessageRemindInfo.Message;
            CreateYaeherMessageRemind.CreatedBy = userid;
            CreateYaeherMessageRemind.CreatedOn = DateTime.Now;
            #region  发送短信
            SendMsmHelper sendMsmHelper = new SendMsmHelper();
            YaeherSendMsm yaeherSendMsm = new YaeherSendMsm();
            yaeherSendMsm.PhoneNumbers = YaeherMessageRemindInfo.PhoneNumber;
            yaeherSendMsm.MessageType = YaeherMessageRemindInfo.MessageType;
            switch (YaeherMessageRemindInfo.MessageType)
            {
                case "Verification":
                    yaeherSendMsm.TemplateParam = "{\"code\":\"" + CreateYaeherMessageRemind.VerificationCode + "\"}";
                    CreateYaeherMessageRemind.EffectiveLength = 90;  // 默认90秒
                    CreateYaeherMessageRemind.EffectiveTime = DateTime.Now.AddSeconds(90);
                    break;
                case "Authentication":
                    yaeherSendMsm.TemplateParam = "{\"code\":\"" + CreateYaeherMessageRemind.VerificationCode + "\"}";
                    CreateYaeherMessageRemind.EffectiveLength = 300;  // 默认300秒
                    CreateYaeherMessageRemind.EffectiveTime = DateTime.Now.AddSeconds(300);
                    break;
                case "Notice":
                    yaeherSendMsm.TemplateParam = "{\"remark\":\"" + CreateYaeherMessageRemind.Message + "\"}";
                    CreateYaeherMessageRemind.EffectiveLength = 0;  // 0
                    CreateYaeherMessageRemind.EffectiveTime = DateTime.Now.AddSeconds(0);
                    break;
                case "ChangePassword":
                    yaeherSendMsm.TemplateParam = "{\"code\":\"" + CreateYaeherMessageRemind.VerificationCode + "\"}";
                    CreateYaeherMessageRemind.EffectiveLength = 90;  // 默认90秒
                    CreateYaeherMessageRemind.EffectiveTime = DateTime.Now.AddSeconds(90);
                    break;
            }
            var SendMessage = sendMsmHelper.SendMsm(yaeherSendMsm);
            if (SendMessage == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
                return ObjectResultModule;
            }
            #endregion
            var result = await _yaeherMessageRemindService.CreateYaeherMessageRemind(CreateYaeherMessageRemind);
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
                OperExplain = "YaeherMessage",
                OperContent = JsonHelper.ToJson(YaeherMessageRemindInfo),
                OperType = "YaeherMessage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 短信记录表 Page
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherMessageRemindPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherMessageRemindPage([FromBody] YaeherMessageRemindIn YaeherMessageRemindInfo)
        {
            if (!Commons.CheckSecret(YaeherMessageRemindInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherMessageRemindInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherMessageRemindInfo.EndTime))
                {
                    YaeherMessageRemindInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherMessageRemindInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.StartTime))
            {
                YaeherMessageRemindInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.KeyWord))
            {
                YaeherMessageRemindInfo.AndAlso(t => t.Message.Contains(YaeherMessageRemindInfo.KeyWord) ||
                                                     t.PhoneNumber.Contains(YaeherMessageRemindInfo.KeyWord) ||
                                                     t.UserName.Contains(YaeherMessageRemindInfo.KeyWord) ||
                                                     t.VerificationCode.Contains(YaeherMessageRemindInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.MessageType))
            {
                YaeherMessageRemindInfo.AndAlso(t => t.MessageType.Contains(YaeherMessageRemindInfo.MessageType));
            }
            YaeherMessageRemindInfo.AndAlso(t => t.IsDelete == false);
            var values = await _yaeherMessageRemindService.YaeherMessageRemindPage(YaeherMessageRemindInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new YaeherMessageRemindOut(values, YaeherMessageRemindInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherMessageRemindPage",
                OperContent = JsonHelper.ToJson(YaeherMessageRemindInfo),
                OperType = "YaeherMessageRemindPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 短信记录表 List 
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherMessageRemindLists")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherMessageRemindLists([FromBody]YaeherMessageRemindIn YaeherMessageRemindInfo)
        {
            if (!Commons.CheckSecret(YaeherMessageRemindInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherMessageRemindInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherMessageRemindInfo.EndTime))
                {
                    YaeherMessageRemindInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherMessageRemindInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.StartTime))
            {
                YaeherMessageRemindInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.KeyWord))
            {
                YaeherMessageRemindInfo.AndAlso(t => t.Message.Contains(YaeherMessageRemindInfo.KeyWord)
                                                  || t.PhoneNumber.Contains(YaeherMessageRemindInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(YaeherMessageRemindInfo.MessageType))
            {
                YaeherMessageRemindInfo.AndAlso(t => t.MessageType.Contains(YaeherMessageRemindInfo.MessageType));
            }
            YaeherMessageRemindInfo.AndAlso(t => t.IsDelete == false);
            var values = await _yaeherMessageRemindService.YaeherMessageRemindList(YaeherMessageRemindInfo);
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
                OperExplain = "YaeherMessageRemindLists",
                OperContent = JsonHelper.ToJson(YaeherMessageRemindInfo),
                OperType = "YaeherMessageRemindLists",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 管理端基础参数
        /// <summary>
        /// 管理端基础参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/ParameterList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<IList<SystemParameter>> ParameterList(SystemParameterIn SystemParameterInfo)
        {
            //参数类型 ConfigPar配置参数 SetPar 系统参数
            if (SystemParameterInfo.Type == "ConfigPar")
            {
                SystemParameterInfo.AndAlso(a => a.IsDelete == false);
                if (!string.IsNullOrEmpty(SystemParameterInfo.SystemCode))
                {
                    SystemParameterInfo.AndAlso(a => a.SystemCode == SystemParameterInfo.SystemCode);
                }
                if (!string.IsNullOrEmpty(SystemParameterInfo.SystemType))
                {
                    SystemParameterInfo.AndAlso(a => a.SystemType == SystemParameterInfo.SystemType);
                }
                var SystemParameters = await _systemParameterService.SystemParameterList(SystemParameterInfo);
                return SystemParameters.ToList();
            }
            else
            {
                DoctorParaSetIn doctorParaSetIn = new DoctorParaSetIn();
                List<SystemParameter> SystemParameterList = new List<SystemParameter>();
                if (!string.IsNullOrEmpty(SystemParameterInfo.SystemCode))
                {
                    doctorParaSetIn.AndAlso(a => a.DoctorParaSetCode == SystemParameterInfo.SystemCode);
                }
                var doctorParaSets = await _doctorParaSetService.DoctorParaSetList(doctorParaSetIn);
                if (doctorParaSets.Count > 0)
                {
                    foreach (var doctorParaSet in doctorParaSets)
                    {
                        SystemParameter systemParameter = new SystemParameter();
                        systemParameter.SystemType = doctorParaSet.DoctorParaSetCode;
                        systemParameter.SystemCode = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Code = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Name = doctorParaSet.DoctorParaSetName;
                        systemParameter.ItemValue = doctorParaSet.ItemValue;
                        systemParameter.Remark = doctorParaSet.DoctorParaSetName;
                        SystemParameterList.Add(systemParameter);
                    }
                }
                return SystemParameterList.ToList();
            }
        }
        /// <summary>
        /// 供调用的基础参数 List 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherMobileParameterList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> YaeherMobileParameterList([FromBody]SystemParameterIn SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            SystemParameterInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(SystemParameterInfo.SystemCode))
            {
                SystemParameterInfo.AndAlso(a => a.SystemCode == SystemParameterInfo.SystemCode);
            }
            if (!string.IsNullOrEmpty(SystemParameterInfo.SystemType))
            {
                SystemParameterInfo.AndAlso(a => a.SystemType == SystemParameterInfo.SystemType);
            }
            var SystemParameters = await _systemParameterService.PatientParameterList(SystemParameterInfo);
            if (SystemParameters.Count == 0)
            {
                return null;
            }
            else
            {
                this.ObjectResultModule.Object = SystemParameters;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 管理端基础参数 List 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/AdminParameterList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> AdminParameterList([FromBody]SystemParameterIn SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            SystemParameterInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(SystemParameterInfo.SystemCode))
            {
                SystemParameterInfo.AndAlso(a => a.SystemCode == SystemParameterInfo.SystemCode);
            }
            if (!string.IsNullOrEmpty(SystemParameterInfo.SystemType))
            {
                SystemParameterInfo.AndAlso(a => a.SystemType == SystemParameterInfo.SystemType);
            }
            var SystemParameters = await _systemParameterService.PatientParameterList(SystemParameterInfo);
            if (SystemParameters.Count == 0)
            {
                return null;
            }
            else
            {
                this.ObjectResultModule.Object = SystemParameters;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 基础参数 List 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherParameterList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherParameterList([FromBody]SystemParameterIn SystemParameterInfo)
        {
            if (!Commons.CheckSecret(SystemParameterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //参数类型 ConfigPar配置参数 SetPar 医生系统参数
            List<SystemParameter> SystemParameterList = new List<SystemParameter>();
            if (SystemParameterInfo.Type == "ConfigPar")
            {
                SystemParameterInfo.AndAlso(a => a.IsDelete == false);
                if (!string.IsNullOrEmpty(SystemParameterInfo.SystemCode))
                {
                    SystemParameterInfo.AndAlso(a => a.SystemCode == SystemParameterInfo.SystemCode);
                }
                if (!string.IsNullOrEmpty(SystemParameterInfo.SystemType))
                {
                    SystemParameterInfo.AndAlso(a => a.SystemType == SystemParameterInfo.SystemType);
                }
                var SystemParameters = await _systemParameterService.SystemParameterList(SystemParameterInfo);
                SystemParameterList = SystemParameters.ToList();
            }
            else
            {
                DoctorParaSetIn doctorParaSetIn = new DoctorParaSetIn();
                doctorParaSetIn.AndAlso(t => !t.IsDelete);
                if (!string.IsNullOrEmpty(SystemParameterInfo.SystemCode))
                {
                    doctorParaSetIn.AndAlso(a => a.DoctorParaSetCode == SystemParameterInfo.SystemCode);
                }
                var doctorParaSets = await _doctorParaSetService.DoctorParaSetList(doctorParaSetIn);
                if (doctorParaSets.Count > 0)
                {
                    foreach (var doctorParaSet in doctorParaSets)
                    {
                        SystemParameter systemParameter = new SystemParameter();
                        systemParameter.SystemType = doctorParaSet.DoctorParaSetCode;
                        systemParameter.SystemCode = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Code = doctorParaSet.DoctorParaSetCode;
                        systemParameter.Name = doctorParaSet.DoctorParaSetName;
                        systemParameter.ItemValue = doctorParaSet.ItemValue;
                        systemParameter.Remark = doctorParaSet.DoctorParaSetName;
                        systemParameter.Id = doctorParaSet.Id;
                        SystemParameterList.Add(systemParameter);
                    }
                }
            }
            if (SystemParameterList.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = SystemParameterList;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        #endregion

        #region 用户基本信息
        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserInfo")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserInfo([FromBody]YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = _userManagerService.UserManager(YaeherUserInfo.Id);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorInformation")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> DoctorInformation([FromBody]YaeherDoctorIn YaeherDoctorInfo)
        {
            if (!Commons.CheckSecret(YaeherDoctorInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = _userManagerService.DoctorInformation(YaeherDoctorInfo.Id);
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
            return ObjectResultModule;
        }
        #endregion

        #region 系统配置
        /// <summary>
        /// 系统配置 新增
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [Route("api/SystemConfigsManager")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SystemConfigsManager([FromBody] SystemConfigs SystemConfigsInfo)
        {
            if (!Commons.CheckSecret(SystemConfigsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SystemConfigs systemConfigs = new SystemConfigs();
            // 当ID>0 更新
            if (SystemConfigsInfo.Id > 0)
            {
                systemConfigs = await _systemConfigsService.SystemConfigsByID(SystemConfigsInfo.Id);
                systemConfigs.Id = SystemConfigsInfo.Id;
                systemConfigs.ModifyBy = userid;
                systemConfigs.ModifyOn = DateTime.Now;
                systemConfigs.IsDelete = SystemConfigsInfo.IsDelete;
                systemConfigs.SystemType = SystemConfigsInfo.SystemType;
                systemConfigs.SystemName = SystemConfigsInfo.SystemName;
                systemConfigs.AppID = SystemConfigsInfo.AppID;
                systemConfigs.AppKey = SystemConfigsInfo.AppKey;
                systemConfigs.AppSecret = SystemConfigsInfo.AppSecret;
                systemConfigs.AccessKeyID = SystemConfigsInfo.AccessKeyID;
                systemConfigs.AccessSecret = SystemConfigsInfo.AccessSecret;
                systemConfigs.RefreshTokenCode = SystemConfigsInfo.RefreshTokenCode;
                systemConfigs.CallCenterNumber = SystemConfigsInfo.CallCenterNumber;
                systemConfigs.TenPayNotify = SystemConfigsInfo.TenPayNotify;
                systemConfigs.TenPayRefundNotify = SystemConfigsInfo.TenPayRefundNotify;
                systemConfigs.TenPayWxOpenNotify = SystemConfigsInfo.TenPayWxOpenNotify;
                systemConfigs.TenPayMchId = SystemConfigsInfo.TenPayMchId;
                systemConfigs.TenPayKey = SystemConfigsInfo.TenPayKey;
                systemConfigs = await _systemConfigsService.UpdateSystemConfigs(systemConfigs);
            }
            else
            {
                systemConfigs.SystemType = SystemConfigsInfo.SystemType;
                systemConfigs.SystemName = SystemConfigsInfo.SystemName;
                systemConfigs.AppID = SystemConfigsInfo.AppID;
                systemConfigs.AppKey = SystemConfigsInfo.AppKey;
                systemConfigs.AppSecret = SystemConfigsInfo.AppSecret;
                systemConfigs.AccessKeyID = SystemConfigsInfo.AccessKeyID;
                systemConfigs.AccessSecret = SystemConfigsInfo.AccessSecret;
                systemConfigs.RefreshTokenCode = SystemConfigsInfo.RefreshTokenCode;
                systemConfigs.CallCenterNumber = SystemConfigsInfo.CallCenterNumber;
                systemConfigs.TenPayNotify = SystemConfigsInfo.TenPayNotify;
                systemConfigs.TenPayRefundNotify = SystemConfigsInfo.TenPayRefundNotify;
                systemConfigs.TenPayWxOpenNotify = SystemConfigsInfo.TenPayWxOpenNotify;
                systemConfigs.TenPayMchId = SystemConfigsInfo.TenPayMchId;
                systemConfigs.TenPayKey = SystemConfigsInfo.TenPayKey;
                systemConfigs.CreatedBy = userid;
                systemConfigs.CreatedOn = DateTime.Now;
                systemConfigs.IsDelete = false;
                systemConfigs = await _systemConfigsService.CreateSystemConfigs(systemConfigs);
            }

            if (systemConfigs.Id > 0)
            {
                this.ObjectResultModule.Object = systemConfigs;
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
                OperExplain = "SystemConfigsManager",
                OperContent = JsonHelper.ToJson(SystemConfigsInfo),
                OperType = "SystemConfigsManager",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 对接接口设置 Page
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [Route("api/SystemConfigsPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SystemConfigsPage([FromBody] SystemConfigsIn SystemConfigsInfo)
        {
            if (!Commons.CheckSecret(SystemConfigsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (!string.IsNullOrEmpty(SystemConfigsInfo.SystemType))
            {
                SystemConfigsInfo.AndAlso(t => t.SystemType == SystemConfigsInfo.SystemType);
            }
            SystemConfigsInfo.AndAlso(t => t.IsDelete == false);
            var values = await _systemConfigsService.SystemConfigsPage(SystemConfigsInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new SystemConfigsOut(values, SystemConfigsInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "SystemConfigsPage",
                OperContent = JsonHelper.ToJson(SystemConfigsInfo),
                OperType = "SystemConfigsPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 对接接口设置 Page
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [Route("api/SystemConfigsList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> SystemConfigsList([FromBody] SystemConfigsIn SystemConfigsInfo)
        {
            if (!Commons.CheckSecret(SystemConfigsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (!string.IsNullOrEmpty(SystemConfigsInfo.SystemType))
            {
                SystemConfigsInfo.AndAlso(t => t.SystemType == SystemConfigsInfo.SystemType);
            }
            SystemConfigsInfo.AndAlso(t => t.IsDelete == false);
            var values = await _systemConfigsService.SystemConfigsList(SystemConfigsInfo);
            if (values.Count() == 0)
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
                OperExplain = "SystemConfigsPage",
                OperContent = JsonHelper.ToJson(SystemConfigsInfo),
                OperType = "SystemConfigsPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 对接接口设置 Page
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [Route("api/SystemConfigsBySystemType")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> SystemConfigsBySystemType([FromBody] SystemConfigsIn SystemConfigsInfo)
        {
            if (!Commons.CheckSecret(SystemConfigsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SystemConfigsInfo.AndAlso(t => t.IsDelete == false && t.SystemType == SystemConfigsInfo.SystemType);
            var values = await _systemConfigsService.SystemConfigsPage(SystemConfigsInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new SystemConfigsMobileOut(values.Items.FirstOrDefault());
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "SystemConfigsBySystemType",
                OperContent = JsonHelper.ToJson(SystemConfigsInfo),
                OperType = "SystemConfigsBySystemType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 对接接口设置 Byid
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [Route("api/SystemConfigsById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SystemConfigsById([FromBody]SystemConfigsIn SystemConfigsInfo)
        {
            if (!Commons.CheckSecret(SystemConfigsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _systemConfigsService.SystemConfigsByID(SystemConfigsInfo.Id);
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
                OperExplain = "SystemConfigsById",
                OperContent = JsonHelper.ToJson(SystemConfigsInfo),
                OperType = "SystemConfigsById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 电话接口
        /// <summary>
        /// 电话呼叫 新增
        /// </summary>
        /// <param name="YaeherPhoneInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherPhone")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherPhone([FromBody] YaeherPhone YaeherPhoneInfo)
        {
            if (!Commons.CheckSecret(YaeherPhoneInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var User = await _yaeherUserService.YaeherUserByID(userid);
            var yaeherPhone = new YaeherPhone()
            {
                UserID = User.Id,
                UserName = User.LoginName,
                Caller = YaeherPhoneInfo.Caller,
                Callee = YaeherPhoneInfo.Callee,
                CallCenterNumber = "075526788591",   // 公司呼叫电话
                StatusCode = YaeherPhoneInfo.StatusCode,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            #region  拨打电话
            AliCallCenter aliCallCenter = new AliCallCenter();
            var StatusCode = await aliCallCenter.StartBack2BackCall(yaeherPhone);
            if (StatusCode == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
                return ObjectResultModule;
            }
            else
            {
                YaeherPhoneInfo.StatusCode = StatusCode;
                YaeherPhoneInfo.CallCenterNumber = "075526788591";
            }
            #endregion
            var result = await _yaeherPhoneService.CreateYaeherPhone(YaeherPhoneInfo);
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
                OperExplain = "YaeherPhone",
                OperContent = JsonHelper.ToJson(YaeherPhoneInfo),
                OperType = "YaeherPhone",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 获取平台角色
        /// <summary>
        ///  获取平台角色
        /// </summary>
        /// <returns></returns>
        [Route("api/WXRoleList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> WXRoleList()
        {
            SystemParameterIn SystemParameterInfo = new SystemParameterIn();
            SystemParameterInfo.AndAlso(a => a.IsDelete == false);
            SystemParameterInfo.AndAlso(a => a.SystemCode == "WXRole");
            var SystemParameters = await _systemParameterService.SystemParameterList(SystemParameterInfo);
            if (SystemParameters.Count > 0)
            {
                SystemParameter systemParameter = new SystemParameter();
                systemParameter = SystemParameters.Where(a => a.Code == "doctor").FirstOrDefault();
                SystemParameters.Remove(systemParameter);
            }
            this.ObjectResultModule.Object = SystemParameters.ToList();
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return ObjectResultModule;
        }
        #endregion

        #region 轮播图
        /// <summary>
        /// 轮播图 新增
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherBanner")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherBanner([FromBody] YaeherBanner YaeherBannerInfo)
        {
            if (!Commons.CheckSecret(YaeherBannerInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherBannerIn yaeherBannerIn = new YaeherBannerIn();
            yaeherBannerIn.AndAlso(a => a.IsDelete == false);
            yaeherBannerIn.AndAlso(a => a.BannerName == YaeherBannerInfo.BannerName);
            yaeherBannerIn.AndAlso(a => a.BannerTypeCode == YaeherBannerInfo.BannerTypeCode);
            var YaeherBannerList = await _yaeherBannerService.YaeherBannerList(yaeherBannerIn);
            if (YaeherBannerList.Count > 0)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "数据已存在，请重新添加";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var CreateYaeherBanner = new YaeherBanner()
            {
                BannerTypeCode = YaeherBannerInfo.BannerTypeCode,
                BannerTypeName = YaeherBannerInfo.BannerTypeName,
                BannerName = YaeherBannerInfo.BannerName,
                BannerImageUrl = YaeherBannerInfo.BannerImageUrl,
                BannerUrl = YaeherBannerInfo.BannerUrl,
                PlayStartTime = YaeherBannerInfo.PlayStartTime,
                PlayEndTime = YaeherBannerInfo.PlayEndTime,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _yaeherBannerService.CreateYaeherBanner(CreateYaeherBanner);
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
                OperExplain = "CreateYaeherBanner",
                OperContent = JsonHelper.ToJson(YaeherBannerInfo),
                OperType = "CreateYaeherBanner",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 轮播图 修改
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherBanner")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherBanner([FromBody]  YaeherBanner YaeherBannerInfo)
        {
            if (!Commons.CheckSecret(YaeherBannerInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateYaeherBanner = await _yaeherBannerService.YaeherBannerByID(YaeherBannerInfo.Id);
            if (UpdateYaeherBanner != null)
            {
                UpdateYaeherBanner.BannerTypeCode = YaeherBannerInfo.BannerTypeCode;
                UpdateYaeherBanner.BannerTypeName = YaeherBannerInfo.BannerTypeName;
                UpdateYaeherBanner.BannerName = YaeherBannerInfo.BannerName;
                UpdateYaeherBanner.BannerImageUrl = YaeherBannerInfo.BannerImageUrl;
                UpdateYaeherBanner.BannerUrl = YaeherBannerInfo.BannerUrl;
                UpdateYaeherBanner.PlayStartTime = YaeherBannerInfo.PlayStartTime;
                UpdateYaeherBanner.PlayEndTime = YaeherBannerInfo.PlayEndTime;
                UpdateYaeherBanner.ModifyOn = DateTime.Now;
                UpdateYaeherBanner.ModifyBy = userid;
                var result = await _yaeherBannerService.UpdateYaeherBanner(UpdateYaeherBanner);
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
                OperExplain = "UpdateYaeherBanner",
                OperContent = JsonHelper.ToJson(YaeherBannerInfo),
                OperType = "UpdateYaeherBanner",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 轮播图 删除
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherBanner")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherBanner([FromBody]  YaeherBanner YaeherBannerInfo)
        {
            if (!Commons.CheckSecret(YaeherBannerInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherBannerService.YaeherBannerByID(YaeherBannerInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherBannerService.DeleteYaeherBanner(query);

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
                OperExplain = "DeleteYaeherBanner",
                OperContent = JsonHelper.ToJson(YaeherBannerInfo),
                OperType = "DeleteYaeherBanner",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 轮播图 Page
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherBannerPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherBannerPage([FromBody] YaeherBannerIn YaeherBannerInfo)
        {
            if (!Commons.CheckSecret(YaeherBannerInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherBannerInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(YaeherBannerInfo.KeyWord))
            {
                YaeherBannerInfo.AndAlso(a => a.BannerTypeCode.Contains(YaeherBannerInfo.KeyWord) ||
                                              a.BannerTypeName.Contains(YaeherBannerInfo.KeyWord) ||
                                              a.BannerName.Contains(YaeherBannerInfo.KeyWord));
            }
            var values = await _yaeherBannerService.YaeherBannerPage(YaeherBannerInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new YaeherBannerOut(values, YaeherBannerInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherBannerPage",
                OperContent = JsonHelper.ToJson(YaeherBannerInfo),
                OperType = "YaeherBannerPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 轮播图 List 
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherBannerList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherBannerList([FromBody]YaeherBannerIn YaeherBannerInfo)
        {
            if (!Commons.CheckSecret(YaeherBannerInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherBannerInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(YaeherBannerInfo.KeyWord))
            {
                YaeherBannerInfo.AndAlso(a => a.BannerTypeCode.Contains(YaeherBannerInfo.KeyWord) ||
                                              a.BannerTypeName.Contains(YaeherBannerInfo.KeyWord) ||
                                              a.BannerName.Contains(YaeherBannerInfo.KeyWord));
            }
            var values = await _yaeherBannerService.YaeherBannerList(YaeherBannerInfo);
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
                OperExplain = "YaeherBannerList",
                OperContent = JsonHelper.ToJson(YaeherBannerInfo),
                OperType = "YaeherBannerList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 轮播图 Byid
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherBannerById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherBannerById([FromBody]YaeherBannerIn YaeherBannerInfo)
        {
            if (!Commons.CheckSecret(YaeherBannerInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherBannerService.YaeherBannerByID(YaeherBannerInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "YaeherBannerById",
                OperContent = JsonHelper.ToJson(YaeherBannerInfo),
                OperType = "YaeherBannerById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 标签配置
        /// <summary>
        /// 标签配置 新增
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherLabelConfig")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherLabelConfig([FromBody] YaeherLabelConfig YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherLabelConfigIn yaeherLabelConfigIn = new YaeherLabelConfigIn();
            yaeherLabelConfigIn.AndAlso(a => a.IsDelete == false);
            yaeherLabelConfigIn.AndAlso(a => a.LabelTypeCode == YaeherLabelConfigInfo.LabelTypeCode);
            if (YaeherLabelConfigInfo.ParentId != 0)
            {
                yaeherLabelConfigIn.AndAlso(a => a.LabelCode == YaeherLabelConfigInfo.LabelCode);
            }
            else
            {
                yaeherLabelConfigIn.AndAlso(a => a.ParentId == YaeherLabelConfigInfo.ParentId);
            }
            var YaeherLabelConfig = await _yaeherLabelConfigService.YaeherLabelConfigList(yaeherLabelConfigIn);
            if (YaeherLabelConfig.Count > 0)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "数据已存在，请重新添加";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var CreateYaeherLabelConfig = new YaeherLabelConfig()
            {
                LabelTypeCode = YaeherLabelConfigInfo.LabelTypeCode,
                LabelTypeName = YaeherLabelConfigInfo.LabelTypeName,
                LabelCode = YaeherLabelConfigInfo.LabelCode,
                LabelName = YaeherLabelConfigInfo.LabelName,
                ParentId = YaeherLabelConfigInfo.ParentId,
                OrderSort = YaeherLabelConfigInfo.OrderSort,
                CreatedBy = userid,
                CreatedOn = DateTime.Now

            };
            var result = await _yaeherLabelConfigService.CreateYaeherLabelConfig(CreateYaeherLabelConfig);
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
                OperExplain = "CreateYaeherLabelConfig",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "CreateYaeherLabelConfig",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 标签配置 修改
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherLabelConfig")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherLabelConfig([FromBody]  YaeherLabelConfig YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateYaeherLabelConfig = await _yaeherLabelConfigService.YaeherLabelConfigByID(YaeherLabelConfigInfo.Id);
            if (UpdateYaeherLabelConfig != null)
            {
                UpdateYaeherLabelConfig.LabelTypeCode = YaeherLabelConfigInfo.LabelTypeCode;
                UpdateYaeherLabelConfig.LabelTypeName = YaeherLabelConfigInfo.LabelTypeName;
                UpdateYaeherLabelConfig.LabelCode = YaeherLabelConfigInfo.LabelCode;
                UpdateYaeherLabelConfig.LabelName = YaeherLabelConfigInfo.LabelName;
                UpdateYaeherLabelConfig.ParentId = YaeherLabelConfigInfo.ParentId;
                UpdateYaeherLabelConfig.OrderSort = YaeherLabelConfigInfo.OrderSort;
                UpdateYaeherLabelConfig.ModifyOn = DateTime.Now;
                UpdateYaeherLabelConfig.ModifyBy = userid;
                var result = await _yaeherLabelConfigService.UpdateYaeherLabelConfig(UpdateYaeherLabelConfig);

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
                OperExplain = "UpdateYaeherLabelConfig",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "UpdateYaeherLabelConfig",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 标签配置 删除
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherLabelConfig")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherLabelConfig([FromBody]  YaeherLabelConfig YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherLabelConfigService.YaeherLabelConfigByID(YaeherLabelConfigInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherLabelConfigService.DeleteYaeherLabelConfig(query);

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
                OperExplain = "DeleteYaeherLabelConfig",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "DeleteYaeherLabelConfig",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 标签配置 List 
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherLabelConfigList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherLabelConfigList([FromBody]YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherLabelConfigInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(YaeherLabelConfigInfo.KeyWord))
            {
                YaeherLabelConfigInfo.AndAlso(a => a.LabelTypeCode.Contains(YaeherLabelConfigInfo.KeyWord) ||
                                                   a.LabelTypeName.Contains(YaeherLabelConfigInfo.KeyWord) ||
                                                   a.LabelCode.Contains(YaeherLabelConfigInfo.KeyWord) ||
                                                   a.LabelName.Contains(YaeherLabelConfigInfo.KeyWord));
            }
            var values = await _yaeherLabelConfigService.YaeherModuleList(YaeherLabelConfigInfo);
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
                OperExplain = "YaeherLabelConfigList",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "YaeherLabelConfigList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 标签配置 List 患者端评价使用
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/LabelConfigList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> LabelConfigList([FromBody]YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherLabelConfigInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(YaeherLabelConfigInfo.KeyWord))
            {
                YaeherLabelConfigInfo.AndAlso(a => a.LabelTypeCode.Contains(YaeherLabelConfigInfo.KeyWord) ||
                                                   a.LabelTypeName.Contains(YaeherLabelConfigInfo.KeyWord) ||
                                                   a.LabelCode.Contains(YaeherLabelConfigInfo.KeyWord) ||
                                                   a.LabelName.Contains(YaeherLabelConfigInfo.KeyWord));
            }
            var values = await _yaeherLabelConfigService.YaeherLabelConfigList(YaeherLabelConfigInfo);
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
                OperExplain = "YaeherLabelConfigList",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "YaeherLabelConfigList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 标签配置 Byid
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherLabelConfigById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherLabelConfigById([FromBody]YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherLabelConfigService.YaeherLabelConfigByID(YaeherLabelConfigInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "YaeherLabelConfigById",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "YaeherLabelConfigById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 标签配置 List 
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherLabelListByCode")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherLabelListByCode([FromBody]YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            if (!Commons.CheckSecret(YaeherLabelConfigInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherLabelConfigInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(YaeherLabelConfigInfo.LabelTypeCode))
            {
                YaeherLabelConfigInfo.AndAlso(a => a.LabelTypeCode == YaeherLabelConfigInfo.LabelTypeCode);
            }
            var values = await _yaeherLabelConfigService.YaeherModuleList(YaeherLabelConfigInfo);
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
                OperExplain = "YaeherLabelListByCode",
                OperContent = JsonHelper.ToJson(YaeherLabelConfigInfo),
                OperType = "YaeherLabelListByCode",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        #endregion

        #region  微信个性化菜单
        /// <summary>
        /// 微信个性化菜单 新增
        /// </summary>
        /// <param name="YaeherConditionalMenuinfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherConditionalMenu")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherConditionalMenu([FromBody] YaeherConditionalMenu YaeherConditionalMenuinfo)
        {
            if (!Commons.CheckSecret(YaeherConditionalMenuinfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SystemParameterIn parain = new SystemParameterIn() { Type = "ConfigPar" }; parain.AndAlso(t => !t.IsDelete && t.Code == YaeherConditionalMenuinfo.RoleCode && t.SystemCode == "WXRole");
            var sys = await _systemParameterService.ParameterList(parain);
            YaeherConditionalMenuIn yaeherConditionalMenuIn = new YaeherConditionalMenuIn();
            yaeherConditionalMenuIn.AndAlso(a => a.IsDelete == false);
            yaeherConditionalMenuIn.AndAlso(a => a.RoleCode == YaeherConditionalMenuinfo.RoleCode);
            yaeherConditionalMenuIn.AndAlso(a => a.ParentID == YaeherConditionalMenuinfo.ParentID);
            var MenuList = await _yaeherConditionalMenuService.YaeherConditionalMenuList(yaeherConditionalMenuIn);
            // 主菜单下只允许增加3个菜单
            if (MenuList.Count >= 3 && YaeherConditionalMenuinfo.ParentID == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "该角色下已存在3个菜单，不可新增菜单！";
                return ObjectResultModule;
            }
            YaeherConditionalMenuIn ChildMenuIn = new YaeherConditionalMenuIn();
            ChildMenuIn.AndAlso(a => a.IsDelete == false);
            ChildMenuIn.AndAlso(a => a.RoleCode == YaeherConditionalMenuinfo.RoleCode);
            ChildMenuIn.AndAlso(a => a.ConditionalName == YaeherConditionalMenuinfo.ConditionalName);
            var ChildMenuList = await _yaeherConditionalMenuService.YaeherConditionalMenuList(ChildMenuIn);
            // 子菜单下不允许存在重名菜单
            if (ChildMenuList.Count > 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "已存在该菜单，不可新增菜单！";
                return ObjectResultModule;
            }
            var CreateConditional = new YaeherConditionalMenu()
            {
                RoleCode = YaeherConditionalMenuinfo.RoleCode,
                RoleName = YaeherConditionalMenuinfo.RoleName,
                TagId = JsonHelper.FromJson<SystemParameterItemValue>(sys.FirstOrDefault().ItemValue).id,  //  获取微信中对应的tagID
                ConditionalName = YaeherConditionalMenuinfo.ConditionalName,
                ConditionalType = YaeherConditionalMenuinfo.ConditionalType,
                ConditionalTypeName = YaeherConditionalMenuinfo.ConditionalTypeName,
                ConditionalUrl = YaeherConditionalMenuinfo.ConditionalUrl,
                AppID = YaeherConditionalMenuinfo.AppID,
                Pagepath = YaeherConditionalMenuinfo.Pagepath,
                ParentID = YaeherConditionalMenuinfo.ParentID,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _yaeherConditionalMenuService.CreateYaeherConditionalMenu(CreateConditional);
            if (result != null)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success!";
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
                OperExplain = "CreateYaeherConditionalMenu",
                OperContent = JsonHelper.ToJson(YaeherConditionalMenuinfo),
                OperType = "CreateYaeherConditionalMenu",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信个性化菜单 修改
        /// </summary>
        /// <param name="YaeherConditionalMenuinfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherConditionalMenu")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherConditionalMenu([FromBody]  YaeherConditionalMenu YaeherConditionalMenuinfo)
        {
            if (!Commons.CheckSecret(YaeherConditionalMenuinfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var Update = await _yaeherConditionalMenuService.YaeherConditionalMenuByID(YaeherConditionalMenuinfo.Id);
            if (Update != null)
            {
                Update.RoleCode = YaeherConditionalMenuinfo.RoleCode;
                Update.RoleName = YaeherConditionalMenuinfo.RoleName;
                Update.TagId = YaeherConditionalMenuinfo.TagId;
                Update.ConditionalName = YaeherConditionalMenuinfo.ConditionalName;
                Update.ConditionalType = YaeherConditionalMenuinfo.ConditionalType;
                Update.ConditionalTypeName = YaeherConditionalMenuinfo.ConditionalTypeName;
                Update.ConditionalUrl = YaeherConditionalMenuinfo.ConditionalUrl;
                Update.AppID = YaeherConditionalMenuinfo.AppID;
                Update.Pagepath = YaeherConditionalMenuinfo.Pagepath;
                Update.ParentID = YaeherConditionalMenuinfo.ParentID;
                Update.ModifyOn = DateTime.Now;
                Update.ModifyBy = userid;
                var result = await _yaeherConditionalMenuService.UpdateYaeherConditionalMenu(Update);

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
                OperExplain = "UpdateYaeherConditionalMenu",
                OperContent = JsonHelper.ToJson(YaeherConditionalMenuinfo),
                OperType = "UpdateYaeherConditionalMenu",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信个性化菜单 删除
        /// </summary>
        /// <param name="YaeherConditionalMenuinfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherConditionalMenu")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherConditionalMenu([FromBody]  YaeherConditionalMenu YaeherConditionalMenuinfo)
        {
            if (!Commons.CheckSecret(YaeherConditionalMenuinfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherConditionalMenuService.YaeherConditionalMenuByID(YaeherConditionalMenuinfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherConditionalMenuService.DeleteYaeherConditionalMenu(query);

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
                OperExplain = "DeleteYaeherConditionalMenu",
                OperContent = JsonHelper.ToJson(YaeherConditionalMenuinfo),
                OperType = "DeleteYaeherConditionalMenu",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 微信个性化菜单 List 
        /// </summary>
        /// <param name="YaeherConditionalMenuInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherConditionalMenuList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherConditionalMenuList([FromBody]YaeherConditionalMenuIn YaeherConditionalMenuInfo)
        {
            if (!Commons.CheckSecret(YaeherConditionalMenuInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherConditionalMenuInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(YaeherConditionalMenuInfo.KeyWord))
            {
                YaeherConditionalMenuInfo.AndAlso(a => a.RoleCode.Contains(YaeherConditionalMenuInfo.KeyWord) ||
                                                   a.ConditionalName.Contains(YaeherConditionalMenuInfo.KeyWord));
            }
            var values = await _yaeherConditionalMenuService.YaeherModuleList(YaeherConditionalMenuInfo);
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
                OperExplain = "YaeherConditionalMenuPage",
                OperContent = JsonHelper.ToJson(YaeherConditionalMenuInfo),
                OperType = "YaeherConditionalMenuPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信个性化菜单 Byid
        /// </summary>
        /// <param name="YaeherConditionalMenuInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherConditionalMenuById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherConditionalMenuById([FromBody]YaeherConditionalMenuIn YaeherConditionalMenuInfo)
        {
            if (!Commons.CheckSecret(YaeherConditionalMenuInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherConditionalMenuService.YaeherConditionalMenuByID(YaeherConditionalMenuInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "YaeherConditionalMenuById",
                OperContent = JsonHelper.ToJson(YaeherConditionalMenuInfo),
                OperType = "YaeherConditionalMenuById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yaeherConditionalMenuInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateWecharMenu")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateWecharMenu([FromBody]  YaeherConditionalMenuIn yaeherConditionalMenuInfo)
        {
            if (!Commons.CheckSecret(yaeherConditionalMenuInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherConditionalMenuIn yaeherConditionalMenuIn = new YaeherConditionalMenuIn();
            yaeherConditionalMenuIn.AndAlso(a => a.IsDelete == false);
            yaeherConditionalMenuIn.AndAlso(a => a.RoleCode == yaeherConditionalMenuInfo.RoleCode);
            var MenuList = await _yaeherConditionalMenuService.YaeherModuleList(yaeherConditionalMenuIn);
            TencentUserManage tencentUserManage = new TencentUserManage();
            #region 获取WecharToken
            // SystemToken systemToken = new SystemToken();
            //  systemToken.TokenType = "Wechar";
            var Tokens = await _systemTokenService.SystemTokenList("Wechar");
            #endregion
            StringBuilder sbwecharMenu = new StringBuilder();
            if (MenuList != null)
            {
                int tag_id = 0;
                string MenuID = string.Empty;
                sbwecharMenu.Append("{\"button\": [ ");
                foreach (var Menu in MenuList)
                {
                    tag_id = Menu.TagId;
                    MenuID = Menu.MenuID;
                    if (Menu.children.Count > 0)
                    {
                        sbwecharMenu.Append("{");
                        sbwecharMenu.Append("\"name\": \"" + Menu.ConditionalName + "\",");
                        sbwecharMenu.Append("\"sub_button\":[");
                        foreach (var childrenmenu in Menu.children)
                        {
                            sbwecharMenu.Append("{");
                            if (Menu.ConditionalType == "view")
                            {
                                sbwecharMenu.Append("\"type\": \"" + childrenmenu.ConditionalType + "\",");
                                sbwecharMenu.Append("\"name\": \"" + childrenmenu.ConditionalName + "\",");
                                sbwecharMenu.Append("\"url\": \"" + childrenmenu.ConditionalUrl + "\"");
                            }
                            else if (Menu.ConditionalType == "click")
                            {
                                sbwecharMenu.Append("\"type\": \"" + childrenmenu.ConditionalType + "\",");
                                sbwecharMenu.Append("\"name\": \"" + childrenmenu.ConditionalName + "\",");
                                sbwecharMenu.Append("\"url\": \"" + childrenmenu.ConditionalUrl + "\"");
                            }
                            sbwecharMenu.Append("},");
                        }
                        sbwecharMenu.ToString().TrimEnd(',');
                        sbwecharMenu.Append("]},");
                    }
                    else
                    {
                        if (Menu.ConditionalType == "view")
                        {
                            sbwecharMenu.Append("{");
                            sbwecharMenu.Append("\"type\": \"" + Menu.ConditionalType + "\",");
                            sbwecharMenu.Append("\"name\": \"" + Menu.ConditionalName + "\",");
                            sbwecharMenu.Append("\"url\": \"" + Menu.ConditionalUrl + "\"");
                            sbwecharMenu.Append("},");
                        }
                        else if (Menu.ConditionalType == "click")
                        {
                            sbwecharMenu.Append("{");
                            sbwecharMenu.Append("\"type\": \"" + Menu.ConditionalType + "\",");
                            sbwecharMenu.Append("\"name\": \"" + Menu.ConditionalName + "\",");
                            sbwecharMenu.Append("\"url\": \"" + Menu.ConditionalUrl + "\"");
                            sbwecharMenu.Append("},");
                        }
                    }
                }
                sbwecharMenu.Append("],");
                sbwecharMenu.Append("\"matchrule\": {\"tag_id\": \"" + tag_id + "\"}}");
                sbwecharMenu = sbwecharMenu.Replace(",]", "]");
                // 先删除对应的菜单
                if (MenuID != null)
                {
                    await tencentUserManage.DeleteWeiXinMenu(MenuID, Tokens.access_token);
                }
                #region 删除已有的所有菜单
                //String[] striID = new string[] { "427805765","427807026","427805763" };
                //foreach (var Item in striID)
                //{
                //    await tencentUserManage.DeleteWeiXinMenu(Item,Tokens.access_token);
                //}
                #endregion
                //新增菜单
                var result = await tencentUserManage.WeiXinCreateMenu(sbwecharMenu.ToString(), Tokens.access_token);
                // 查询所有
                //var WecharMenu = await tencentUserManage.WeiXinMenu(Tokens.access_token);
                var WecharMenuID = result.menuid;
                if (WecharMenuID != null)
                {

                    var YaeherMenuList = await _yaeherConditionalMenuService.YaeherConditionalMenuList(yaeherConditionalMenuIn);
                    if (YaeherMenuList.Count > 0)
                    {
                        foreach (var MenuInfo in YaeherMenuList)
                        {
                            MenuInfo.MenuID = WecharMenuID;
                            await _yaeherConditionalMenuService.UpdateYaeherConditionalMenu(MenuInfo);
                        }
                    }
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                else
                {
                    this.ObjectResultModule.Object = "该公众号的菜单设置了过多的域名外跳";
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "error!";
                }
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
                OperExplain = "UpdateWecharMenu",
                OperContent = JsonHelper.ToJson(yaeherConditionalMenuInfo),
                OperType = "UpdateWecharMenu",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        #endregion

        #region
        /// <summary>
        /// 微信跳转测试
        /// </summary>
        /// <param name="TestWecharInfo"></param>
        /// <returns></returns>
        [Route("api/TestWechar")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> TestWechar([FromBody]TestWecharInfo TestWecharInfo)
        {
            HttpHelper httpHelper = new HttpHelper();
            var TencentJosn = await httpHelper.PostResponseAsync(TestWecharInfo.Url, TestWecharInfo.Content);
            if (TencentJosn == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = TencentJosn;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        #endregion

        #region 微信消息模板
        /// <summary>
        /// 获取微信消息模板
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [Route("api/AchieveMessageTemplate")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AchieveMessageTemplate([FromBody] MessageTemplateIn MessageTemplateInfo)
        {
            if (!Commons.CheckSecret(MessageTemplateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            TencentUserManage tencentUserManage = new TencentUserManage();
            #region 获取WecharToken
            //  SystemToken systemToken = new SystemToken();
            //  systemToken.TokenType = "Wechar";
            var Tokens = await _systemTokenService.SystemTokenList("Wechar");
            #endregion
            var MessageTemplateList = await tencentUserManage.WeCharTemplateList(Tokens.access_token);
            MessageTemplateIn messageTemplateIn = new MessageTemplateIn();
            messageTemplateIn.AndAlso(a => a.IsDelete == false);
            var YaeherTemplateList = await _yaeherMessageTemplateService.MessageTemplateList(messageTemplateIn);
            if (MessageTemplateList.template_list.Count() > 0)
            {
                // 将历史删除
                if (YaeherTemplateList.Count > 0)
                {
                    foreach (var TemplateInfo in YaeherTemplateList)
                    {
                        TemplateInfo.IsDelete = true;
                        TemplateInfo.DeleteBy = userid;
                        TemplateInfo.DeleteTime = DateTime.Now;
                        await _yaeherMessageTemplateService.DeleteMessageTemplate(TemplateInfo);
                    }
                }
                foreach (var TemplatInfo in MessageTemplateList.template_list.Where(a => a.deputy_industry != null && a.deputy_industry != ""))
                {
                    YaeherMessageTemplate MessageTemplate = new YaeherMessageTemplate();
                    MessageTemplate.TemplateCode = "";
                    MessageTemplate.Title = "";
                    MessageTemplate.WecharTitle = TemplatInfo.title;
                    MessageTemplate.TemplateId = TemplatInfo.template_id;
                    MessageTemplate.Content = TemplatInfo.content;
                    MessageTemplate.Example = TemplatInfo.example;
                    MessageTemplate.CreatedBy = userid;
                    MessageTemplate.CreatedOn = DateTime.Now;
                    var result = await _yaeherMessageTemplateService.CreateMessageTemplate(MessageTemplate);
                }
                this.ObjectResultModule.Object = "success";
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success!";
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
                OperExplain = "AchieveMessageTemplate",
                OperContent = JsonHelper.ToJson(MessageTemplateInfo),
                OperType = "AchieveMessageTemplate",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信消息模板 修改
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateMessageTemplate")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateMessageTemplate([FromBody]  YaeherMessageTemplate MessageTemplateInfo)
        {
            if (!Commons.CheckSecret(MessageTemplateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var Update = await _yaeherMessageTemplateService.MessageTemplateByID(MessageTemplateInfo.Id);
            MessageTemplateIn messageTemplateIn = new MessageTemplateIn();
            messageTemplateIn.AndAlso(a => a.IsDelete == false);
            messageTemplateIn.AndAlso(a => a.TemplateCode == MessageTemplateInfo.TemplateCode);
            var MessageTemplateList = await _yaeherMessageTemplateService.MessageTemplateList(messageTemplateIn);
            if (MessageTemplateList.Count > 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "模板编号已经被使用，请重新选择！";
                return ObjectResultModule;
            }
            if (Update != null)
            {
                Update.TemplateCode = MessageTemplateInfo.TemplateCode;
                Update.Title = MessageTemplateInfo.Title;
                Update.ModifyOn = DateTime.Now;
                Update.ModifyBy = userid;
                var result = await _yaeherMessageTemplateService.UpdateMessageTemplate(Update);

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
                OperExplain = "UpdateMessageTemplate",
                OperContent = JsonHelper.ToJson(MessageTemplateInfo),
                OperType = "UpdateMessageTemplate",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 微信消息模板 Byid
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [Route("api/MessageTemplateById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> MessageTemplateById([FromBody]MessageTemplateIn MessageTemplateInfo)
        {
            if (!Commons.CheckSecret(MessageTemplateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherMessageTemplateService.MessageTemplateByID(MessageTemplateInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "MessageTemplateById",
                OperContent = JsonHelper.ToJson(MessageTemplateInfo),
                OperType = "MessageTemplateById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信消息模板 Page
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [Route("api/MessageTemplatePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> MessageTemplatePage([FromBody] MessageTemplateIn MessageTemplateInfo)
        {
            if (!Commons.CheckSecret(MessageTemplateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            MessageTemplateInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(MessageTemplateInfo.KeyWord))
            {
                MessageTemplateInfo.AndAlso(a => a.TemplateCode.Contains(MessageTemplateInfo.KeyWord) ||
                                              a.WecharTitle.Contains(MessageTemplateInfo.KeyWord) ||
                                              a.Title.Contains(MessageTemplateInfo.KeyWord));
            }
            var values = await _yaeherMessageTemplateService.MessageTemplatePage(MessageTemplateInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new MessageTemplateOut(values, MessageTemplateInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "MessageTemplatePage",
                OperContent = JsonHelper.ToJson(MessageTemplateInfo),
                OperType = "MessageTemplatePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 微信消息发送模板
        /// <summary>
        /// 微信消息发送模板 新增
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateSendMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateSendMessage([FromBody] SendMessageTemplate SendMessageInfo)
        {
            if (!Commons.CheckSecret(SendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SendMessageIn sendMessageIn = new SendMessageIn();
            sendMessageIn.AndAlso(a => a.IsDelete == false);
            sendMessageIn.AndAlso(a => a.TemplateCode == SendMessageInfo.TemplateCode);
            sendMessageIn.AndAlso(a => a.OperationType == SendMessageInfo.OperationType);
            sendMessageIn.AndAlso(a => a.OperationType == SendMessageInfo.Recipient);
            var MessageTemplateList = await _sendMessageService.SendMessageList(sendMessageIn);
            if (MessageTemplateList.Count > 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "数据已存在，请重新增加！";
                return ObjectResultModule;
            }
            var CreateSendMessage = new SendMessageTemplate()
            {
                TemplateCode = SendMessageInfo.TemplateCode,
                OperationType = SendMessageInfo.OperationType,
                Recipient = SendMessageInfo.Recipient,
                BackUrl = SendMessageInfo.BackUrl,
                FirstMessage = SendMessageInfo.FirstMessage,
                Keyword1 = SendMessageInfo.Keyword1,
                Keyword2 = SendMessageInfo.Keyword2,
                Keyword3 = SendMessageInfo.Keyword3,
                MessageRemark = SendMessageInfo.MessageRemark,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _sendMessageService.CreateSendMessage(CreateSendMessage);
            if (result != null)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success!";
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
                OperExplain = "CreateSendMessage",
                OperContent = JsonHelper.ToJson(SendMessageInfo),
                OperType = "CreateSendMessage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信消息发送模板 修改
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateSendMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateSendMessage([FromBody]  SendMessageTemplate SendMessageInfo)
        {
            if (!Commons.CheckSecret(SendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var Update = await _sendMessageService.SendMessageByID(SendMessageInfo.Id);
            if (Update != null)
            {
                Update.TemplateCode = SendMessageInfo.TemplateCode;
                Update.OperationType = SendMessageInfo.OperationType;
                Update.Recipient = SendMessageInfo.Recipient;
                Update.BackUrl = SendMessageInfo.BackUrl;
                Update.FirstMessage = SendMessageInfo.FirstMessage;
                Update.Keyword1 = SendMessageInfo.Keyword1;
                Update.Keyword2 = SendMessageInfo.Keyword2;
                Update.Keyword3 = SendMessageInfo.Keyword3;
                Update.MessageRemark = SendMessageInfo.MessageRemark;
                Update.ModifyOn = DateTime.Now;
                Update.ModifyBy = userid;
                var result = await _sendMessageService.UpdateSendMessage(Update);
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
                OperExplain = "UpdateSendMessage",
                OperContent = JsonHelper.ToJson(SendMessageInfo),
                OperType = "UpdateSendMessage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信消息发送模板 删除
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteSendMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteSendMessage([FromBody]  SendMessageTemplate SendMessageInfo)
        {
            if (!Commons.CheckSecret(SendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _sendMessageService.SendMessageByID(SendMessageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _sendMessageService.DeleteSendMessage(query);

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
                OperExplain = "DeleteSendMessage",
                OperContent = JsonHelper.ToJson(SendMessageInfo),
                OperType = "DeleteSendMessage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 微信消息发送模板 List 
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/SendMessageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SendMessageList([FromBody]SendMessageIn SendMessageInfo)
        {
            if (!Commons.CheckSecret(SendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            SendMessageInfo.AndAlso(t => t.IsDelete == false);
            SendMessageInfo.AndAlso(t => t.TemplateCode == SendMessageInfo.TemplateCode);
            if (!String.IsNullOrEmpty(SendMessageInfo.KeyWord))
            {
                SendMessageInfo.AndAlso(a => a.TemplateCode.Contains(SendMessageInfo.KeyWord) ||
                                             a.OperationType.Contains(SendMessageInfo.KeyWord));
            }
            var values = await _sendMessageService.SendMessageList(SendMessageInfo);
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
                OperExplain = "SendMessageList",
                OperContent = JsonHelper.ToJson(SendMessageInfo),
                OperType = "SendMessageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信消息发送模板 Byid
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/SendMessageId")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SendMessageId([FromBody]SendMessageIn SendMessageInfo)
        {
            if (!Commons.CheckSecret(SendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _sendMessageService.SendMessageByID(SendMessageInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "SendMessageId",
                OperContent = JsonHelper.ToJson(SendMessageInfo),
                OperType = "SendMessageId",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region  微信发送模板消息实例
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/SendWecharMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SendWecharMessage([FromBody]  SendMessageInfo sendMessageInfo)
        {
            if (!Commons.CheckSecret(sendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (sendMessageInfo.TemplateCode != null)
            {
                var result = await _wecharSendMessageService.CreateWecharSendMessage(sendMessageInfo);
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
                OperExplain = "SendWecharMessage",
                OperContent = JsonHelper.ToJson(sendMessageInfo),
                OperType = "SendWecharMessage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 定时发送微信信息
        /// </summary>
        /// <param name="HangFireJobInfo"></param>
        /// <returns></returns>
        [Route("api/SendWechar")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SendWechar([FromBody]  HangFireJob HangFireJobInfo)
        {
            if (!Commons.CheckSecret(HangFireJobInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (HangFireJobInfo.BusinessCode != null)
            {
                SendWechaMessage sendWechaMessage = new SendWechaMessage();
                var Tokens = await _systemTokenService.SystemTokenList("Wechar");
                // 发送微信
                var WecharData = HangFireJobInfo.JobParameter;
                var result = await sendWechaMessage.SendWecharMessage(WecharData, Tokens.access_token);
                if (result.errcode != "error")
                {
                    var UpdateHangFireJob = await _hangFireJobService.HangFireJobByBusiness(HangFireJobInfo.BusinessCode, HangFireJobInfo.BusinessID);

                    var wecharMessage = await _wecharSendMessageService.WecharSendMessageByNumber(HangFireJobInfo.BusinessCode, UpdateHangFireJob.BusinessID.ToString());
                    if (wecharMessage != null)
                    {
                        wecharMessage.MsgID = result.msgid;
                        wecharMessage.Status = result.errmsg;
                        await _wecharSendMessageService.UpdateWecharSendMessage(wecharMessage);
                    }

                    if (HangFireJobInfo.JobSates == "Open")//新增状态过来则open状态转换成完成
                    {
                        UpdateHangFireJob.JobSates = "Complete"; // 执行状态
                    }
                    else
                    {
                        UpdateHangFireJob.JobSates = HangFireJobInfo.JobSates; // 执行状态
                    }
                    UpdateHangFireJob.ModifyOn = DateTime.Now;
                    UpdateHangFireJob.ModifyBy = userid;
                    await _hangFireJobService.UpdateHangFireJob(UpdateHangFireJob);
                }
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
        /// 微信消息模板 Byid
        /// </summary>
        /// <param name="WecharSendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/WecharSendMessageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> WecharSendMessageById([FromBody]WecharSendMessageIn WecharSendMessageInfo)
        {
            if (!Commons.CheckSecret(WecharSendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _wecharSendMessageService.WecharSendMessageByID(WecharSendMessageInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "WecharSendMessageById",
                OperContent = JsonHelper.ToJson(WecharSendMessageInfo),
                OperType = "WecharSendMessageById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 微信消息模板 Page
        /// </summary>
        /// <param name="WecharSendMessageInfo"></param>
        /// <returns></returns>
        [Route("api/WecharSendMessagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> WecharSendMessagePage([FromBody] WecharSendMessageIn WecharSendMessageInfo)
        {
            if (!Commons.CheckSecret(WecharSendMessageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            WecharSendMessageInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(WecharSendMessageInfo.KeyWord))
            {
                WecharSendMessageInfo.AndAlso(a => a.ConsultNumber.Contains(WecharSendMessageInfo.KeyWord) ||
                                              a.ConsultantName.Contains(WecharSendMessageInfo.KeyWord) ||
                                              a.DoctorName.Contains(WecharSendMessageInfo.KeyWord) ||
                                              a.TemplateId.Contains(WecharSendMessageInfo.KeyWord));
            }
            var values = await _wecharSendMessageService.WecharSendMessagePage(WecharSendMessageInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new WecharSendMessageOut(values, WecharSendMessageInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "WecharSendMessagePage",
                OperContent = JsonHelper.ToJson(WecharSendMessageInfo),
                OperType = "WecharSendMessagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        #endregion


        #region 接受微信用户发送消息
        /// <summary>
        /// 接受微信用户发送消息 List 
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [Route("api/AcceptWecharList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AcceptWecharList([FromBody]AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            if (!Commons.CheckSecret(AcceptTencentWecharInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            AcceptTencentWecharInfo.AndAlso(t => t.IsDelete == false);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.StartTime))
            {
                StartTime = DateTime.Parse(AcceptTencentWecharInfo.StartTime);
                if (string.IsNullOrEmpty(AcceptTencentWecharInfo.EndTime))
                {
                    AcceptTencentWecharInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.EndTime))
            {
                EndTime = DateTime.Parse(AcceptTencentWecharInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.StartTime))
            {
                AcceptTencentWecharInfo.AndAlso(t => t.CreatedOn >= StartTime);
                AcceptTencentWecharInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!String.IsNullOrEmpty(AcceptTencentWecharInfo.KeyWord))
            {
                AcceptTencentWecharInfo.AndAlso(a => a.FullName.Contains(AcceptTencentWecharInfo.KeyWord) ||
                                                     a.MsgType.Contains(AcceptTencentWecharInfo.KeyWord));
            }
            var values = await _acceptTencentWecharService.AcceptTencentList(AcceptTencentWecharInfo);
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
                OperExplain = "AcceptWecharList",
                OperContent = JsonHelper.ToJson(AcceptTencentWecharInfo),
                OperType = "AcceptWecharList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 接受微信用户发送消息 page 
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [Route("api/AcceptWecharPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AcceptWecharPage([FromBody]AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            if (!Commons.CheckSecret(AcceptTencentWecharInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            AcceptTencentWecharInfo.AndAlso(t => t.IsDelete == false);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.StartTime))
            {
                StartTime = DateTime.Parse(AcceptTencentWecharInfo.StartTime);
                if (string.IsNullOrEmpty(AcceptTencentWecharInfo.EndTime))
                {
                    AcceptTencentWecharInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.EndTime))
            {
                EndTime = DateTime.Parse(AcceptTencentWecharInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.StartTime))
            {
                AcceptTencentWecharInfo.AndAlso(t => t.CreatedOn >= StartTime);
                AcceptTencentWecharInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!String.IsNullOrEmpty(AcceptTencentWecharInfo.KeyWord))
            {
                AcceptTencentWecharInfo.AndAlso(a => a.FullName.Contains(AcceptTencentWecharInfo.KeyWord) ||
                                                     a.MsgType.Contains(AcceptTencentWecharInfo.KeyWord));
            }
            var values = await _acceptTencentWecharService.AcceptTencentPage(AcceptTencentWecharInfo);
            if (values.TotalCount == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new AcceptTencentWecharOut(values, AcceptTencentWecharInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AcceptWecharPage",
                OperContent = JsonHelper.ToJson(AcceptTencentWecharInfo),
                OperType = "AcceptWecharPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 接受微信用户发送消息 Byid
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [Route("api/AcceptWecharId")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AcceptWecharId([FromBody]AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            if (!Commons.CheckSecret(AcceptTencentWecharInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _acceptTencentWecharService.AcceptTencentByID(AcceptTencentWecharInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
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
                OperExplain = "AcceptWecharId",
                OperContent = JsonHelper.ToJson(AcceptTencentWecharInfo),
                OperType = "AcceptWecharId",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 发送消息  客服发送 请使用FromUserName
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [Route("api/SendWechaMessgae")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SendWechaMessgae([FromBody]AcceptTencentWechar AcceptTencentWecharInfo)
        {
            if (!Commons.CheckSecret(AcceptTencentWecharInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //  SystemToken systemToken = new SystemToken();
            //  systemToken.TokenType = "Wechar";
            var Tokens = await _systemTokenService.SystemTokenList("Wechar");
            var textSingleMessage = new TextSingleMessage()
            {
                ToUser = AcceptTencentWecharInfo.FromUserName,
                TextContent = AcceptTencentWecharInfo.Content
            };
            var WecharMessage = textSingleMessage.Send(Tokens.access_token).Result.ToString();
            AcceptTencentWechar acceptTencentWechar = new AcceptTencentWechar();
            YaeherUserIn yaeherUserIn = new YaeherUserIn();
            yaeherUserIn.AndAlso(a => a.IsDelete == false);
            yaeherUserIn.AndAlso(a => a.WecharOpenID == AcceptTencentWecharInfo.FromUserName);
            var YaerherUser = await _yaeherUserService.YaeherUserList(yaeherUserIn);
            if (YaerherUser != null)
            {
                var UserInfo = YaerherUser.FirstOrDefault();
                acceptTencentWechar.FullName = UserInfo.FullName;// 用户全称
                acceptTencentWechar.NickName = UserInfo.NickName;
                acceptTencentWechar.NickName = UserInfo.UserImage;
            }
            acceptTencentWechar.ToUserName = ""; // 客服维护可为空
            acceptTencentWechar.FromUserName = AcceptTencentWecharInfo.FromUserName;
            acceptTencentWechar.CreateTime = "";  //待定
            acceptTencentWechar.MsgType = AcceptTencentWecharInfo.MsgType;
            acceptTencentWechar.Content = AcceptTencentWecharInfo.Content;
            acceptTencentWechar.PicUrl = AcceptTencentWecharInfo.PicUrl;
            acceptTencentWechar.MediaId = AcceptTencentWecharInfo.MediaId;
            acceptTencentWechar.ThumbMediaId = AcceptTencentWecharInfo.ThumbMediaId;
            acceptTencentWechar.Format = AcceptTencentWecharInfo.Format;
            acceptTencentWechar.Recognition = AcceptTencentWecharInfo.Recognition;
            acceptTencentWechar.Title = AcceptTencentWecharInfo.Title;
            acceptTencentWechar.Description = AcceptTencentWecharInfo.Description;
            acceptTencentWechar.Url = AcceptTencentWecharInfo.Url;
            acceptTencentWechar.Location_X = AcceptTencentWecharInfo.Location_X;
            acceptTencentWechar.Location_Y = AcceptTencentWecharInfo.Location_Y;
            acceptTencentWechar.Scale = AcceptTencentWecharInfo.Scale;
            acceptTencentWechar.Label = AcceptTencentWecharInfo.Label;
            acceptTencentWechar.MsgId = AcceptTencentWecharInfo.MsgId;
            acceptTencentWechar.MessageFrom = "Customer";  // 客服回复
            acceptTencentWechar.CreatedBy = userid;
            acceptTencentWechar.CreatedOn = DateTime.Now;
            var Result = await _acceptTencentWecharService.CreateAcceptTencent(acceptTencentWechar);
            if (Result == null)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = Result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 发送消息状态 page 
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [Route("api/AcceptWecharStatePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AcceptWecharStatePage([FromBody]AcceptWecharStateIn AcceptWecharStateInfo)
        {
            if (!Commons.CheckSecret(AcceptWecharStateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            AcceptWecharStateInfo.AndAlso(t => t.IsDelete == false);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(AcceptWecharStateInfo.StartTime))
            {
                StartTime = DateTime.Parse(AcceptWecharStateInfo.StartTime);
                if (string.IsNullOrEmpty(AcceptWecharStateInfo.EndTime))
                {
                    AcceptWecharStateInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(AcceptWecharStateInfo.EndTime))
            {
                EndTime = DateTime.Parse(AcceptWecharStateInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(AcceptWecharStateInfo.StartTime))
            {
                AcceptWecharStateInfo.AndAlso(t => t.CreatedOn >= StartTime);
                AcceptWecharStateInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!String.IsNullOrEmpty(AcceptWecharStateInfo.KeyWord))
            {
                AcceptWecharStateInfo.AndAlso(a => a.ConsultantName.Contains(AcceptWecharStateInfo.KeyWord) ||
                                                     a.CustomerServiceName.Contains(AcceptWecharStateInfo.KeyWord));
            }
            var values = await _acceptWecharStateService.AcceptWecharStatePage(AcceptWecharStateInfo);
            if (values.TotalCount == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new AcceptWecharStateOut(values, AcceptWecharStateInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AcceptWecharStatePage",
                OperContent = JsonHelper.ToJson(AcceptWecharStateInfo),
                OperType = "AcceptWecharStatePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 发送消息状态 Byid
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [Route("api/AcceptWecharStateId")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AcceptWecharStateId([FromBody]AcceptWecharStateIn AcceptWecharStateInfo)
        {
            if (!Commons.CheckSecret(AcceptWecharStateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _acceptWecharStateService.AcceptWecharStateByID(AcceptWecharStateInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                //将数据修改为已读
                values.IsReady = true;
                await _acceptWecharStateService.UpdateAcceptWecharState(values);
                AcceptTencentWecharIn acceptTencentWecharIn = new AcceptTencentWecharIn();
                acceptTencentWecharIn.AndAlso(a => a.IsDelete == false);
                acceptTencentWecharIn.AndAlso(a => a.FromUserName == values.ConsultantOpenID);
                DateTime StartTime = DateTime.Now.AddDays(-1);
                DateTime EndTime = DateTime.Now.AddDays(+1);
                acceptTencentWecharIn.AndAlso(t => t.CreatedOn >= StartTime);
                acceptTencentWecharIn.AndAlso(t => t.CreatedOn < EndTime);
                var Result = await _acceptTencentWecharService.AcceptTencentList(acceptTencentWecharIn);
                this.ObjectResultModule.Object = Result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AcceptWecharStateId",
                OperContent = JsonHelper.ToJson(AcceptWecharStateInfo),
                OperType = "AcceptWecharStateId",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 更新聊天状态
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateWecharState")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateWecharState([FromBody]AcceptWecharState AcceptWecharStateInfo)
        {
            if (!Commons.CheckSecret(AcceptWecharStateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _acceptWecharStateService.AcceptWecharStateByID(AcceptWecharStateInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                //将数据修改为已读
                values.AcceptState = AcceptWecharStateInfo.AcceptState;  //更新状态
                values.AcceptTime = DateTime.Now;  // 获取当前状态
                var Result = await _acceptWecharStateService.UpdateAcceptWecharState(values);
                this.ObjectResultModule.Object = Result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateWecharState",
                OperContent = JsonHelper.ToJson(AcceptWecharStateInfo),
                OperType = "UpdateWecharState",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 联系客户
        /// </summary>
        /// <param name="AcceptWecharStateInfo"></param>
        /// <returns></returns>
        [Route("api/JoinWecharState")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> JoinWecharState([FromBody]AcceptWecharState AcceptWecharStateInfo)
        {
            if (!Commons.CheckSecret(AcceptWecharStateInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            // 查询用户是否存在
            AcceptWecharStateIn acceptWecharStateIn = new AcceptWecharStateIn();
            acceptWecharStateIn.AndAlso(a => a.ConsultantID == AcceptWecharStateInfo.ConsultantID);
            var values = await _acceptWecharStateService.WecharStateList(acceptWecharStateIn);
            if (values.Count == 0)
            {
                YaeherUser yaeherUser = await _yaeherUserService.YaeherUserByID(AcceptWecharStateInfo.ConsultantID);
                if (yaeherUser != null)
                {
                    AcceptWecharState WecharState = new AcceptWecharState();
                    // 需要根据当前客服排班记录查询
                    WecharState.CustomerServiceID = 0;
                    WecharState.CustomerServiceName = "客服";
                    WecharState.CustomerServiceJson = "客服";
                    WecharState.ConsultantID = yaeherUser.Id;
                    WecharState.ConsultantName = yaeherUser.FullName;
                    WecharState.ConsultantJSON = JsonHelper.ToJson(yaeherUser);
                    WecharState.ConsultantOpenID = yaeherUser.WecharOpenID;
                    WecharState.AcceptState = "2";
                    WecharState.IsReady = false;
                    WecharState.AcceptTime = DateTime.Now;
                    //插入回复状态
                    if (yaeherUser.WecharOpenID != null)
                    {
                        var CreateState = await _acceptWecharStateService.CreateAcceptWecharState(WecharState);
                    }
                }
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                AcceptWecharState acceptWecharState = values.FirstOrDefault();
                acceptWecharState.IsReady = false;
                acceptWecharState.AcceptState = "2";     //更新状态
                acceptWecharState.AcceptTime = DateTime.Now;  // 获取当前状态
                var Result = await _acceptWecharStateService.UpdateAcceptWecharState(acceptWecharState);
                this.ObjectResultModule.Object = Result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "JoinWecharState",
                OperContent = JsonHelper.ToJson(AcceptWecharStateInfo),
                OperType = "JoinWecharState",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AcceptTencentWecharInfo"></param>
        /// <returns></returns>
        [Route("api/UnReadyWecharList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UnReadyWecharList([FromBody]AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            if (!Commons.CheckSecret(AcceptTencentWecharInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            AcceptTencentWecharInfo.AndAlso(t => t.IsDelete == false);
            DateTime StartTime = new DateTime();
            DateTime EndTime = DateTime.Now;
            if (!string.IsNullOrEmpty(AcceptTencentWecharInfo.StartTime))
            {
                StartTime = DateTime.Parse(AcceptTencentWecharInfo.StartTime);
            }
            else
            {
                StartTime = StartTime.AddSeconds(60);
            }
            AcceptTencentWecharInfo.AndAlso(t => t.CreatedOn >= StartTime);
            AcceptTencentWecharInfo.AndAlso(t => t.CreatedOn < EndTime);
            var values = await _acceptTencentWecharService.AcceptTencentList(AcceptTencentWecharInfo);
            this.ObjectResultModule.Object = values.Count;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UnReadyWecharList",
                OperContent = JsonHelper.ToJson(AcceptTencentWecharInfo),
                OperType = "UnReadyWecharList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptWechar"></param>
        /// <returns></returns>
        [Route("api/AcceptWecharMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<string> AcceptWecharMessage([FromBody]  AcceptWechar acceptWechar)
        {
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AcceptWecharMessage",
                OperContent = JsonHelper.ToJson(acceptWechar),
                OperType = "AcceptWecharMessage",
                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return acceptWechar.echoStr;
        }
        #endregion

        #region 标签组管理
        /// <summary>
        /// 标签组管理 新增
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [Route("api/CreateRelationLabelGroup")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateRelationLabelGroup([FromBody] RelationLabelGroup RelationLabelGroupInfo)
        {
            if (!Commons.CheckSecret(RelationLabelGroupInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateRelationLabelGroup = new RelationLabelGroup()
            {
                GroupName = RelationLabelGroupInfo.GroupName,
                LableID = RelationLabelGroupInfo.LableID,
                LableName = RelationLabelGroupInfo.LableName,
                CreatedBy = userid,
                CreatedOn = DateTime.Now

            };
            var result = await _relationLabelGroupService.CreateRelationLabelGroup(CreateRelationLabelGroup);
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
        /// 标签组管理 修改
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateRelationLabelGroup")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateRelationLabelGroup([FromBody]  RelationLabelGroup RelationLabelGroupInfo)
        {
            if (!Commons.CheckSecret(RelationLabelGroupInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateRelationLabelGroup = await _relationLabelGroupService.RelationLabelGroupByID(RelationLabelGroupInfo.Id);
            if (UpdateRelationLabelGroup != null)
            {
                UpdateRelationLabelGroup.GroupName = RelationLabelGroupInfo.GroupName;
                UpdateRelationLabelGroup.LableID = RelationLabelGroupInfo.LableID;
                UpdateRelationLabelGroup.LableName = RelationLabelGroupInfo.LableName;
                UpdateRelationLabelGroup.ModifyOn = DateTime.Now;
                UpdateRelationLabelGroup.ModifyBy = userid;
                var result = await _relationLabelGroupService.UpdateRelationLabelGroup(UpdateRelationLabelGroup);
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
        /// 标签组管理 删除
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteRelationLabelGroup")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteRelationLabelGroup([FromBody]  RelationLabelGroup RelationLabelGroupInfo)
        {
            if (!Commons.CheckSecret(RelationLabelGroupInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _relationLabelGroupService.RelationLabelGroupByID(RelationLabelGroupInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _relationLabelGroupService.DeleteRelationLabelGroup(query);

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
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 标签组管理 Page
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [Route("api/RelationLabelGroupPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RelationLabelGroupPage([FromBody] RelationLabelGroupIn RelationLabelGroupInfo)
        {
            if (!Commons.CheckSecret(RelationLabelGroupInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            RelationLabelGroupInfo.AndAlso(t => t.IsDelete == false);
            var values = await _relationLabelGroupService.RelationLabelGroupPage(RelationLabelGroupInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new RelationLabelGroupOut(values, RelationLabelGroupInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 标签组管理 List 
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [Route("api/RelationLabelGroupList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RelationLabelGroupList([FromBody]RelationLabelGroupIn RelationLabelGroupInfo)
        {
            if (!Commons.CheckSecret(RelationLabelGroupInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            RelationLabelGroupInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(RelationLabelGroupInfo.KeyWord))
            {
                RelationLabelGroupInfo.AndAlso(a => a.GroupName.Contains(RelationLabelGroupInfo.KeyWord));
            }
            var values = await _relationLabelGroupService.RelationLabelGroupList(RelationLabelGroupInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 标签组管理 Byid
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [Route("api/RelationLabelGroupById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RelationLabelGroupById([FromBody]RelationLabelGroup RelationLabelGroupInfo)
        {
            if (!Commons.CheckSecret(RelationLabelGroupInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _relationLabelGroupService.RelationLabelGroupByID(RelationLabelGroupInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        #endregion

        #region 医生快捷回复
        /// <summary>
        /// 医生快捷回复 新增
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [Route("api/CreateQuickReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateQuickReply([FromBody] QuickReply QuickReplyInfo)
        {
            if (!Commons.CheckSecret(QuickReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateQuickReply = new QuickReply()
            {
                UseOf = QuickReplyInfo.UseOf,
                Title = QuickReplyInfo.Title,
                Content = QuickReplyInfo.Content,
                DoctorID = userid,
                CreatedBy = userid,
                CreatedOn = DateTime.Now

            };
            var result = await _quickReplyService.CreateQuickReply(CreateQuickReply);
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
        /// 医生快捷回复 修改
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateQuickReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateQuickReply([FromBody]  QuickReply QuickReplyInfo)
        {
            if (!Commons.CheckSecret(QuickReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateQuickReply = await _quickReplyService.QuickReplyByID(QuickReplyInfo.Id);
            if (UpdateQuickReply != null)
            {
                UpdateQuickReply.UseOf = QuickReplyInfo.UseOf;
                UpdateQuickReply.Title = QuickReplyInfo.Title;
                UpdateQuickReply.Content = QuickReplyInfo.Content;
                UpdateQuickReply.DoctorID = userid;
                UpdateQuickReply.ModifyBy = userid;
                UpdateQuickReply.ModifyOn = DateTime.Now;
                var result = await _quickReplyService.UpdateQuickReply(UpdateQuickReply);
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
        /// 医生快捷回复 删除
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteQuickReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteQuickReply([FromBody]  QuickReply QuickReplyInfo)
        {
            if (!Commons.CheckSecret(QuickReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _quickReplyService.QuickReplyByID(QuickReplyInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _quickReplyService.DeleteQuickReply(query);

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
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生快捷回复 Page
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [Route("api/QuickReplyPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuickReplyPage([FromBody] QuickReplyIn QuickReplyInfo)
        {
            if (!Commons.CheckSecret(QuickReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (QuickReplyInfo.Platform == "PC")
            {
                // 判断当角色为医生角色时
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    QuickReplyInfo.AndAlso(t => t.DoctorID == userid);
                }
            }
            else if (QuickReplyInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                { QuickReplyInfo.AndAlso(t => t.DoctorID == userid); }
            }
            QuickReplyInfo.AndAlso(t => t.IsDelete == false);
            var values = await _quickReplyService.QuickReplyPage(QuickReplyInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new QuickReplyOut(values, QuickReplyInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生快捷回复 List 
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [Route("api/QuickReplyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuickReplyList([FromBody]QuickReplyIn QuickReplyInfo)
        {
            if (!Commons.CheckSecret(QuickReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            // var User = _userManagerService.UserManager(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (QuickReplyInfo.Platform == "PC")
            {
                // 判断当角色为医生角色时
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    QuickReplyInfo.AndAlso(t => t.DoctorID == userid);
                }
            }
            else if (QuickReplyInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                { QuickReplyInfo.AndAlso(t => t.DoctorID == userid); }
            }
            QuickReplyInfo.AndAlso(t => t.IsDelete == false);
            if (!String.IsNullOrEmpty(QuickReplyInfo.KeyWord))
            {
                QuickReplyInfo.AndAlso(a => a.Title.Contains(QuickReplyInfo.KeyWord) ||
                                            a.Content.Contains(QuickReplyInfo.KeyWord) ||
                                            a.UseOf.Contains(QuickReplyInfo.KeyWord));
            }
            var values = await _quickReplyService.QuickReplyList(QuickReplyInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生快捷回复 Byid
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [Route("api/QuickReplyById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuickReplyById([FromBody]QuickReply QuickReplyInfo)
        {
            if (!Commons.CheckSecret(QuickReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _quickReplyService.QuickReplyByID(QuickReplyInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        #endregion
    }
}