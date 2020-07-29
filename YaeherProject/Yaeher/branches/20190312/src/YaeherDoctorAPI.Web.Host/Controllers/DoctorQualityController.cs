using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.DoctorQuality;
using Yaeher.DoctorQuality.Dto;
using Yaeher.Extensions;
using Yaeher.LableManages;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherDoctorAPI.Web.Host.Controllers
{

    /// <summary>
    /// 质控处理API
    /// </summary>
    public class DoctorQualityController : YaeherAppServiceBase
    {
        private readonly IQualityControlManageService _QualityControlManageService;
        private readonly IConsultationService _consultationService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IConsultationEvaluationService _consultationEvaluationService;
        private readonly IConsultationReplyService _consultationReplyService;
        private readonly IPhoneReplyRecordService _phoneReplyRecordService;
        private readonly IYaeherUserService _yaeherUser;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IAbpSession _IabpSession;
        private readonly IAttachmentServices _attachmentServices;
        private readonly ICollectConsultationService _collectConsultationService;
        private readonly IUserManagerService _userManagerService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILableManageService _lableManageService;
        private readonly IDoctorRelationService _DoctorRelationService;
        private readonly IClinicInfomationService _clinicInfomationService;
        private readonly IYaeherOperListService _yaeherOperListService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qualityControlManageService"></param>
        /// <param name="consultationService"></param>
        /// <param name="session"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="consultationEvaluationService"></param>
        /// <param name="consultationReplyService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="phoneReplyRecordService"></param>
        /// <param name="orderManageService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="attachmentServices"></param>
        /// <param name="collectConsultationService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="lableManageService"></param>
        /// <param name="doctorRelationService"></param>
        /// <param name="clinicInfomationService"></param>
        /// <param name="yaeherOperListService"></param>
        public DoctorQualityController(IQualityControlManageService qualityControlManageService,
                                        IConsultationService consultationService,
                                        IAbpSession session,
                                        IYaeherDoctorService yaeherDoctorService,
                                        IConsultationEvaluationService consultationEvaluationService,
                                        IConsultationReplyService consultationReplyService,
                                        ISystemParameterService systemParameterService,
                                        IPhoneReplyRecordService phoneReplyRecordService,
                                        IOrderManageService orderManageService,
                                        IOrderTradeRecordService orderTradeRecordService,
                                        IYaeherUserService yaeherUserService,
                                        IAttachmentServices attachmentServices,
                                        ICollectConsultationService collectConsultationService,
                                        IUserManagerService userManagerService,
                                        IUnitOfWorkManager unitOfWorkManager,
                                        ILableManageService lableManageService,
                                        IDoctorRelationService doctorRelationService,
                                        IClinicInfomationService clinicInfomationService,
                                        IYaeherOperListService yaeherOperListService)
        {
            _QualityControlManageService = qualityControlManageService;
            _consultationService = consultationService;
            _yaeherDoctorService = yaeherDoctorService;
            _phoneReplyRecordService = phoneReplyRecordService;
            _yaeherUser = yaeherUserService;
            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _IabpSession = session;
            _consultationEvaluationService = consultationEvaluationService;
            _consultationReplyService = consultationReplyService;
            _systemParameterService = systemParameterService;
            _attachmentServices = attachmentServices;
            _collectConsultationService = collectConsultationService;
            _userManagerService = userManagerService;
            _unitOfWorkManager = unitOfWorkManager;
            _lableManageService = lableManageService;
            _DoctorRelationService = doctorRelationService;
            _clinicInfomationService = clinicInfomationService;
            _yaeherOperListService = yaeherOperListService;
        }
        /// <summary>
        ///  质控委员处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateQualityControlManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateQualityControlManage([FromBody] QualityControlManage input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var consult = await _consultationService.YaeherConsultationByID(input.ConsultID);
            var doctor = await _yaeherDoctorService.YaeherDoctorByID(input.DoctorID);
            var comtrol = await _QualityControlManageService.QualityControlManageByConsultStateID(consult.Id);
            if (comtrol != null) { return new ObjectResultModule("", 400, "质控委员未处理完不允许质控重新分发质控委员！"); }
            if (doctor.Id == consult.DoctorID) { return new ObjectResultModule("", 400, "咨询单不允许提交给咨询医生！"); }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var create = new QualityControlManage()
                {
                    ConsultNumber = consult.ConsultNumber,
                    ConsultID = consult.Id,
                    ConsultantID = consult.ConsultantID,
                    ConsultantName = consult.ConsultantName,
                    DoctorID = input.DoctorID,
                    DoctorName = doctor.DoctorName,
                    ConsultType = consult.ConsultType,
                    ReplyState = "untreated",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var res = await _QualityControlManageService.CreateQualityControlManage(create);
                this.ObjectResultModule.Object = res;
                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateQualityControlManage",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateQualityControlManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "sucess";
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 质控委员处理列表Page
        /// </summary>
        /// <param name="QualityControlManageInPage"> QualityControlManageInPage 数据</param>
        /// <returns></returns>
        [Route("api/QualityControlManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityControlManagePage([FromBody]QualityControlManageIn QualityControlManageInPage)
        {
            if (!Commons.CheckSecret(QualityControlManageInPage.Secret))
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
            if (!string.IsNullOrEmpty(QualityControlManageInPage.StartTime))
            {
                StartTime = DateTime.Parse(QualityControlManageInPage.StartTime);
                if (string.IsNullOrEmpty(QualityControlManageInPage.EndTime))
                {
                    QualityControlManageInPage.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QualityControlManageInPage.EndTime))
            {
                EndTime = DateTime.Parse(QualityControlManageInPage.EndTime);
            }
            if (!string.IsNullOrEmpty(QualityControlManageInPage.StartTime))
            {
                QualityControlManageInPage.AndAlso(t => t.CreatedOn >= StartTime);
                QualityControlManageInPage.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (QualityControlManageInPage.Platform == "PC")
            {
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    QualityControlManageInPage.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
            }
            else if (QualityControlManageInPage.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    QualityControlManageInPage.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            if (!string.IsNullOrEmpty(QualityControlManageInPage.KeyWord))
            {
                QualityControlManageInPage.AndAlso(t => t.DoctorName.Contains(QualityControlManageInPage.KeyWord) ||
                                                        t.ConsultantName.Contains(QualityControlManageInPage.KeyWord));
            }
            QualityControlManageInPage.AndAlso(t => !t.IsDelete);
            var values = await _QualityControlManageService.QualityControlManagePage(QualityControlManageInPage);

            this.ObjectResultModule.Object = new QualityControlManagePageOut(values, QualityControlManageInPage);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityControlManagePage",
                OperContent = JsonHelper.ToJson(QualityControlManageInPage),
                OperType = "QualityControlManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取患者成员List 
        /// </summary>
        /// <param name="QualityControlManageInList"> QualityControlManageInList 数据</param>
        /// <returns></returns>
        [Route("api/QualityControlManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityControlManageList([FromBody]QualityControlManageIn QualityControlManageInList)
        {
            if (!Commons.CheckSecret(QualityControlManageInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            QualityControlManageInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(QualityControlManageInList.KeyWord))
            {
                QualityControlManageInList.AndAlso(t => t.DoctorName.Contains(QualityControlManageInList.KeyWord) ||
                                                        t.ConsultantName.Contains(QualityControlManageInList.KeyWord));
            }
            var values = await _QualityControlManageService.QualityControlManageList(QualityControlManageInList);
            if (values.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityControlManageList",
                OperContent = JsonHelper.ToJson(QualityControlManageInList),
                OperType = "QualityControlManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新质控打分质控委员打分
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/QualityControlManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityControlManage([FromBody] QualityControlManageIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var query = await _QualityControlManageService.QualityControlManageByID(input.Id);
            if (query != null)
            {
                if (input.ReplyState == "treated" && query.ReplyState == "untreated")
                {
                    query.QualityLevel = input.QualityLevel;
                    query.RepayIllnessDescription = input.RepayIllnessDescription;
                    query.ModifyOn = DateTime.Now;
                    query.ModifyBy = userid;
                    query.ReplyState = "treated";
                }
                else if (input.ReplyState == "back" && query.ReplyState == "untreated")
                {
                    query.ModifyOn = DateTime.Now;
                    query.ModifyBy = userid;
                    query.ReplyState = "back";
                }
                var res = await _QualityControlManageService.UpdateQualityControlManage(query);
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
                OperExplain = "QualityControlManage",
                OperContent = JsonHelper.ToJson(input),
                OperType = "QualityControlManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除患者成员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteQualityControlManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteQualityControlManage([FromBody] QualityControlManage input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _QualityControlManageService.QualityControlManageByID(input.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _QualityControlManageService.DeleteQualityControlManage(query);

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
                OperExplain = "DeleteQualityControlManage",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteQualityControlManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 质控咨询详情
        /// </summary>
        /// <param name="QualityControlManage"></param>
        /// <returns></returns>
        [Route("api/QualityControlManageDetail")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityControlManageDetail([FromBody]QualityControlManageIn QualityControlManage)
        {
            if (!Commons.CheckSecret(QualityControlManage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var qualitycontrol = await _QualityControlManageService.QualityControlManageByID(QualityControlManage.Id);
            var values = await _consultationService.YaeherConsultationByID(qualitycontrol.ConsultID);
            if (values == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var EvaluationIn = new ConsultationEvaluationIn();
            EvaluationIn.AndAlso(t => !t.IsDelete && t.ConsultNumber == values.ConsultNumber);
            var eva = await _consultationEvaluationService.ConsultationEvaluationList(EvaluationIn);
            ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
            consultationReplyIn.ConsultNumber = values.ConsultNumber;
            var replys = await _consultationReplyService.ReplyDetailList(consultationReplyIn);

            PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
            phoneReplyRecordIn.ConsultNumber = values.ConsultNumber;
            var phonereplys = await _phoneReplyRecordService.ReplyDetailList(phoneReplyRecordIn);

            var AttachmentInfo = new AttachmentIn() { ConsultNumber = values.ConsultNumber };
            var Attachmentreply = await _attachmentServices.ReplyDetailList(AttachmentInfo);

            var doctor = JsonHelper.FromJson<YaeherDoctor>(values.DoctorJSON);
            var order = await _orderManageService.OrderManageByconsultNumber(values.ConsultNumber);
            var serverid = order.ServiceID;
            var UserResult = await _yaeherUser.YaeherUserByID(doctor.UserID);

            //var UserResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherUser>>>(user);
            //if (UserResult == null || UserResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var collect = await _collectConsultationService.CollectConsultationByExpression(t => !t.IsDelete && t.ConsultID == values.Id && t.CreatedBy == userid);
            //var where = new CollectConsultationIn();where.AndAlso(t => !t.IsDelete && t.ConsultID == values.Id && t.CreatedBy == userid);
            //var collect = await _collectConsultationService.CollectConsultationListAsync(where);

            if (phonereplys.Count > 0)
            {
                replys = replys.Union(phonereplys).ToList();
            }
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
                    //var rep = Attachmentreply.result.item.(t => t.ReplyId == item.ReplyId).ToList();
                    //if (rep != null) { item.Message = rep; }
                }
                //replys = replys.Union(Attachmentreply.result.item).ToList();
                // replys = replys.Select(t => new ReplyDetail(t, Attachmentreply.result.item)).ToList();
                questionatt = Attachmentreply.Where(t => t.ConsultNumber == values.ConsultNumber && t.ServiceType == "consultation").ToList();
            }
            // var questionatt = Attachmentreply.Where(t => t.ConsultID == values.Id && t.ServiceType == "consultation").ToList();
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultState");
            var paramlist = await _systemParameterService.ParameterList(param);

            var IIInesslabel = JsonHelper.FromJson<LableManage>(values.IIInessJSON);
            this.ObjectResultModule.Object = new QualityControlManageOutDetail(qualitycontrol, values, collect == null ? false : true, questionatt, replys, UserResult, eva, serverid, IIInesslabel.Id, paramlist);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityControlManageDetail",
                OperContent = JsonHelper.ToJson(QualityControlManage),
                OperType = "QualityControlManageDetail",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 质控搜索新医生信息
        /// </summary>
        /// <param name="YaeherDoctorSearch"> YaeherDoctorSearch 数据</param>
        /// <returns></returns>
        [Route("api/QualityYaeherDoctorSearch")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityYaeherDoctorSearch([FromBody]YaeherDoctorSearch YaeherDoctorSearch)
        {
            if (!Commons.CheckSecret(YaeherDoctorSearch.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var param1 = new SystemParameterIn() { SystemType = "newdoctor" };
            var paramlist1 = await _systemParameterService.ParameterList(param1);
            var consul = new DoctorNewIn();
            consul.RecordCount = int.Parse(paramlist1[0].ItemValue);
            var NewDoctor = await _consultationService.DoctorNewList(consul);
            if (NewDoctor.Count < 1)
            {
                this.ObjectResultModule.Object = new List<ClinicDoctorsView>();
                return this.ObjectResultModule;
            }
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(YaeherDoctorSearch.KeyWord))
            {
                doctorRelationIn.AndAlso(a => a.LableName.Contains(YaeherDoctorSearch.KeyWord) || a.DoctorName.Contains(YaeherDoctorSearch.KeyWord));
            }
            var lableList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);

            var docrel = new DoctorRelationIn();
            docrel.AndAlso(t => (!t.IsDelete));
            if (!string.IsNullOrEmpty(YaeherDoctorSearch.KeyWord))
            {
                docrel.AndAlso(t => (t.DoctorName.Contains(YaeherDoctorSearch.KeyWord) || t.LableName.Contains(YaeherDoctorSearch.KeyWord)));
            }
            var labelrel = await _DoctorRelationService.DoctorClinicRelationList(docrel);
            if (labelrel.Count < 1)
            {
                this.ObjectResultModule.Object = new List<ClinicDoctorsView>();
                return this.ObjectResultModule;
            }
            var seach = new ClinicInfomationIn() { MaxResultCount = YaeherDoctorSearch.MaxResultCount, SkipTotal = YaeherDoctorSearch.SkipTotal };
            var ClinicInfo = await _clinicInfomationService.QualityDoctorInformation(YaeherDoctorSearch, labelrel, NewDoctor);
            // 查询科室信息
            if (ClinicInfo != null && ClinicInfo.TotalCount > 0)
            {
                foreach (var item in ClinicInfo.Items)
                {
                    item.Doctorslable = lableList.Where(t => t.DoctorID == item.Id).ToList();
                }
            }
            if (ClinicInfo.TotalCount < 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var ClinicDoctorsViewLists = ClinicInfo;
                this.ObjectResultModule.Object = new ClinicDoctorInfoOut(ClinicDoctorsViewLists, YaeherDoctorSearch);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityYaeherDoctorSearch",
                OperContent = JsonHelper.ToJson(YaeherDoctorSearch),
                OperType = "QualityYaeherDoctorSearch",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
    }
}
