using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Consultation;
using Yaeher.DoctorQuality;
using Yaeher.DoctorQuality.Dto;
using Yaeher.Extensions;
using Yaeher.LableManages;
using Yaeher.NumericalStatement;
using Yaeher.NumericalStatement.Dto;
using Yaeher.Quality;
using Yaeher.Quality.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 质控管理
    /// </summary>
    public class QualityController : YaeherAppServiceBase
    {
        private readonly IQualityCommitteeService _qualityCommitteeService;
        private readonly IQualityCommitteeRegisterService _qualityCommitteeRegisterService;
        private readonly IClinicInfomationService _clinicInfomationService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IAbpSession _IabpSession;
        private readonly ILableManageService _lableManageService;
        private readonly IDoctorRelationService _DoctorRelationService;
        private readonly IUserManagerService _userManagerService;
        private readonly IQualityControlManageService _qualityControlManageService;
        private readonly IConsultationOrderTotalService _consultationOrderTotalService;
        private readonly IConsultationService _consultationService;
        private readonly IEvaluationTotalService _evaluationTotalService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="qualityCommitteeService"></param>
        /// <param name="qualityCommitteeRegisterService"></param>
        /// <param name="session"></param>
        /// <param name="clinicInfomationService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="lableManageService"></param>
        /// <param name="doctorRelationService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="qualityControlManageService"></param>
        /// <param name="consultationOrderTotalService"></param>
        /// <param name="consultationService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="evaluationTotalService"></param>
        public QualityController(IQualityCommitteeService qualityCommitteeService,
                                 IQualityCommitteeRegisterService qualityCommitteeRegisterService,
                                 IAbpSession session, IClinicInfomationService clinicInfomationService,
                                 IYaeherDoctorService yaeherDoctorService,
                                 IUnitOfWorkManager unitOfWorkManager,
                                 ILableManageService lableManageService,
                                 IDoctorRelationService doctorRelationService,
                                 IUserManagerService userManagerService,
                                 IQualityControlManageService qualityControlManageService,
                                 IConsultationOrderTotalService consultationOrderTotalService,
                                 IConsultationService consultationService,
                                 IYaeherOperListService yaeherOperListService,
                                 IEvaluationTotalService evaluationTotalService)
        {
            _qualityCommitteeService = qualityCommitteeService;
            _qualityCommitteeRegisterService = qualityCommitteeRegisterService;
            _clinicInfomationService = clinicInfomationService;
            _yaeherDoctorService = yaeherDoctorService;
            _unitOfWorkManager = unitOfWorkManager;
            _IabpSession = session;
            _lableManageService = lableManageService;
            _DoctorRelationService = doctorRelationService;
            _userManagerService = userManagerService;
            _qualityControlManageService = qualityControlManageService;
            _consultationOrderTotalService = consultationOrderTotalService;
            _consultationService = consultationService;
            _yaeherOperListService = yaeherOperListService;
            _evaluationTotalService = evaluationTotalService;
        }
        #region 质控委员
        /// <summary>
        /// 成为质控委员审核
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommittee")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommittee([FromBody] QualityCommitteeAdd QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var register = await _qualityCommitteeRegisterService.QualityCommitteeRegisterByID(QualityCommitteeInfo.QualityCommitteeRegisterID);
            var doctor = await _yaeherDoctorService.YaeherDoctorByID(register.DoctorID);
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if (register.ApplyState == "qualitystart")//申请质控委员
                {
                    if (QualityCommitteeInfo.QualityState == "success")
                    {
                        var CreateQualityCommittee = new QualityCommittee()
                        {
                            //ClinicName = QualityCommitteeInfo.ClinicName,
                            //ClinicID = QualityCommitteeInfo.ClinicID,
                            DoctorID = doctor.Id,
                            DoctorName = doctor.DoctorName,
                            QualityState = QualityCommitteeInfo.QualityState,
                            CreatedBy = userid,
                            CreatedOn = DateTime.Now,
                        };
                        var result = await _qualityCommitteeService.CreateQualityCommittee(CreateQualityCommittee);

                        register.CheckState = "success";
                        register.CheckTime = DateTime.Now;
                        register.Checker = userid;
                        await _qualityCommitteeRegisterService.UpdateQualityCommitteeRegister(register);

                        this.ObjectResultModule.Object = result;
                    }
                    else
                    {
                        register.CheckState = "fail";
                        register.CheckTime = DateTime.Now;
                        register.Checker = userid;
                        register.CheckRemark = QualityCommitteeInfo.CheckRemark;
                        await _qualityCommitteeRegisterService.UpdateQualityCommitteeRegister(register);
                    }
                }
                else//申请取消质控委员
                {
                    if (QualityCommitteeInfo.QualityState == "success")
                    {
                        var qua = await _qualityCommitteeService.QualityCommitteeByDoctorID(doctor.Id);
                        qua.IsDelete = true;
                        await _qualityCommitteeService.UpdateQualityCommittee(qua);
                        register.CheckState = "success";
                        register.CheckTime = DateTime.Now;
                        register.Checker = userid;
                        await _qualityCommitteeRegisterService.UpdateQualityCommitteeRegister(register);
                    }
                    else
                    {
                        register.CheckState = "fail";
                        register.CheckTime = DateTime.Now;
                        register.Checker = userid;
                        register.CheckRemark = QualityCommitteeInfo.CheckRemark;
                        await _qualityCommitteeRegisterService.UpdateQualityCommitteeRegister(register);
                    }
                }
                unitOfWork.Complete();
            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityCommittee",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "QualityCommittee",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 质控手动新增质控委员审核
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/CreateQualityCommittee")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateQualityCommittee([FromBody] QualityCommitteeAdd QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByID(QualityCommitteeInfo.DoctorID);
            var quacheck = await _qualityCommitteeService.QualityCommitteeByDoctorID(QualityCommitteeInfo.DoctorID);
            if (quacheck != null) { return new ObjectResultModule("", 400, "已成为质控委员不允许重复添加！"); }
            var CreateQualityCommittee = new QualityCommittee()
            {
                //ClinicName = QualityCommitteeInfo.ClinicName,
                //ClinicID = QualityCommitteeInfo.ClinicID,
                DoctorID = doctor.Id,
                DoctorName = doctor.DoctorName,
                QualityState = "success",
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
            };
            var result = await _qualityCommitteeService.CreateQualityCommittee(CreateQualityCommittee);
            this.ObjectResultModule.Object = result;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateQualityCommittee",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "CreateQualityCommittee",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 质控委员 修改
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateQualityCommittee")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateQualityCommittee([FromBody] QualityCommittee QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateQualityCommittee = await _qualityCommitteeService.QualityCommitteeByID(QualityCommitteeInfo.Id);
            if (UpdateQualityCommittee != null)
            {
                UpdateQualityCommittee.ClinicName = QualityCommitteeInfo.ClinicName;
                UpdateQualityCommittee.ClinicID = QualityCommitteeInfo.ClinicID;
                UpdateQualityCommittee.DoctorID = QualityCommitteeInfo.DoctorID;
                UpdateQualityCommittee.DoctorName = QualityCommitteeInfo.DoctorName;
                UpdateQualityCommittee.Accomplish = QualityCommitteeInfo.Accomplish;
                UpdateQualityCommittee.QualityState = QualityCommitteeInfo.QualityState;
                UpdateQualityCommittee.CreatedBy = QualityCommitteeInfo.CreatedBy;
                UpdateQualityCommittee.CreatedOn = QualityCommitteeInfo.CreatedOn;
                UpdateQualityCommittee.ModifyOn = DateTime.Now;
                UpdateQualityCommittee.ModifyBy = userid;
                var result = await _qualityCommitteeService.UpdateQualityCommittee(UpdateQualityCommittee);
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
                OperExplain = "UpdateQualityCommittee",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "UpdateQualityCommittee",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 质控委员 删除
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteQualityCommittee")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteQualityCommittee([FromBody] QualityCommitteeIn QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _qualityCommitteeService.QualityCommitteeByID(QualityCommitteeInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var control = new QualityControlManageIn(); control.AndAlso(t => !t.IsDelete && t.DoctorID == query.DoctorID && t.ReplyState == "untreated");
                    var list = await _qualityControlManageService.QualityControlManageList(control);
                    foreach (var item in list)
                    {
                        item.ReplyState = "back";
                        item.ModifyOn = DateTime.Now;
                        item.ModifyBy = userid;
                        await _qualityControlManageService.UpdateQualityControlManage(item);
                    }
                    var res = await _qualityCommitteeService.DeleteQualityCommittee(query);
                    this.ObjectResultModule.Object = res;
                    unitOfWork.Complete();
                }
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
                OperExplain = "DeleteQualityCommittee",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "DeleteQualityCommittee",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 质控委员 Page
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteePage([FromBody]QualityCommitteeIn QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.StartTime))
            {
                StartTime = DateTime.Parse(QualityCommitteeInfo.StartTime);
                if (string.IsNullOrEmpty(QualityCommitteeInfo.EndTime))
                {
                    QualityCommitteeInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.EndTime))
            {
                EndTime = DateTime.Parse(QualityCommitteeInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.StartTime))
            {
                QualityCommitteeInfo.AndAlso(t => t.CreatedOn >= StartTime);
                QualityCommitteeInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.KeyWord))
            {
                QualityCommitteeInfo.AndAlso(t => t.DoctorName.Contains(QualityCommitteeInfo.KeyWord));
            }
            QualityCommitteeInfo.AndAlso(t => t.IsDelete == false);
            //查询科室标签
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.KeyWord))
            {
                doctorRelationIn.AndAlso(a => a.LableName.Contains(QualityCommitteeInfo.KeyWord) || a.DoctorName.Contains(QualityCommitteeInfo.KeyWord));
            }
            var lableList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);
            var docrel = new DoctorRelationIn();
            docrel.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.KeyWord))
            {
                docrel.AndAlso(t => t.DoctorName.Contains(QualityCommitteeInfo.KeyWord) || t.LableName.Contains(QualityCommitteeInfo.KeyWord));
            }
            var labelrel = await _DoctorRelationService.DoctorClinicRelationList(docrel);
            var ClinicInfo = await _qualityCommitteeRegisterService.QualityDoctorInformation(QualityCommitteeInfo, labelrel);
            DateTime StartTime1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime1 = StartTime1.AddDays(+1);
            EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            evaluationTotalIn.AndAlso(a => (a.CreatedOn >= StartTime1));
            evaluationTotalIn.AndAlso(a => (a.CreatedOn < EndTime1));
            var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);

            // 查询科室信息
            if (ClinicInfo != null && ClinicInfo.TotalCount > 0)
            {
                foreach (var item in ClinicInfo.Items)
                {
                    item.Doctorslable = lableList.Where(t => t.DoctorID == item.Id).ToList();
                    var evaluationTotal = evaluationTotalList.Where(t => t.DoctorID == item.Id).FirstOrDefault();
                    if (evaluationTotal != null)
                    {
                        item.ReceiptNumBer = evaluationTotal.CompleteTotal;//接单数
                        item.AverageTime = evaluationTotal.AverageAnswer.ToString();//平均时长                    //  item.DoctorLevel = evaluationTotal == null ? "0" : evaluationTotal.AverageEvaluate.ToString();//星级
                        item.EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
                        if (item.EvaluationCount >= 15)
                        { item.AverageEvaluate = evaluationTotal.AverageEvaluate; }//星级
                    }
                    //item.DoctorLevel = evaluationTotal == null ? "0" : evaluationTotal.AverageEvaluate.ToString();//星级
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
                this.ObjectResultModule.Object = new QualityDoctorInfoOut(ClinicDoctorsViewLists, QualityCommitteeInfo);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityCommitteePage",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "QualityCommitteePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 质控委员 List 
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteeList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteeList([FromBody]QualityCommitteeIn QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.StartTime))
            {
                StartTime = DateTime.Parse(QualityCommitteeInfo.StartTime);
                if (string.IsNullOrEmpty(QualityCommitteeInfo.EndTime))
                {
                    QualityCommitteeInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.EndTime))
            {
                EndTime = DateTime.Parse(QualityCommitteeInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.StartTime))
            {
                QualityCommitteeInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.KeyWord))
            {
                QualityCommitteeInfo.AndAlso(t => t.DoctorName.Contains(QualityCommitteeInfo.KeyWord));
            }
            QualityCommitteeInfo.AndAlso(t => t.IsDelete == false);
            var values = await _qualityCommitteeService.QualityCommitteeList(QualityCommitteeInfo);
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
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityCommitteeList",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "QualityCommitteeList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 质控委员 Byid
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteeById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteeById([FromBody]QualityCommitteeIn QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _qualityCommitteeService.QualityCommitteeByID(QualityCommitteeInfo.Id);
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
                OperExplain = "QualityCommitteeById",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "QualityCommitteeById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 申请质控委员
        /// <summary>
        /// 申请质控委员 新增
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [Route("api/CreateQualityCommitteeRegister")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateQualityCommitteeRegister([FromBody] QualityCommitteeRegister QualityCommitteeRegisterInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeRegisterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            QualityCommitteeRegisterIn qualityCommitteeRegisterIn = new QualityCommitteeRegisterIn();
            qualityCommitteeRegisterIn.AndAlso(a => a.IsDelete == false);
            qualityCommitteeRegisterIn.AndAlso(a => a.DoctorID == doctor.Id);
            qualityCommitteeRegisterIn.AndAlso(a => a.CheckState == "checking");
            var RegisterDoctor = await _qualityCommitteeRegisterService.QualityCommitteeRegisterList(qualityCommitteeRegisterIn);
            if (RegisterDoctor.Count > 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "error!";
                return ObjectResultModule;
            }
            var CreateQualityCommitteeRegister = new QualityCommitteeRegister()
            {
                DoctorID = doctor.Id,
                DoctorName = doctor.DoctorName,
                ApplyRemark = QualityCommitteeRegisterInfo.ApplyRemark,
                ApplyState = QualityCommitteeRegisterInfo.ApplyState,
                CheckState = "checking",
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
            };
            var result = await _qualityCommitteeRegisterService.CreateQualityCommitteeRegister(CreateQualityCommitteeRegister);
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
                OperExplain = "CreateQualityCommitteeRegister",
                OperContent = JsonHelper.ToJson(QualityCommitteeRegisterInfo),
                OperType = "CreateQualityCommitteeRegister",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 质控委员信息
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteeByUser")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteeByUser([FromBody] SecretModel secret)
        {
            if (!Commons.CheckSecret(secret.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var quality = new QualityCommitteeRegisterIn();
            if (secret.Platform == "PC")
            {
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {quality.AndAlso(t => t.CreatedBy == userid);}
            }
            else if (secret.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                { quality.AndAlso(t => t.CreatedBy == userid);}
            }
            quality.AndAlso(t => !t.IsDelete);
            var result = await _qualityCommitteeRegisterService.QualityCommitteeRegisterList(quality);
            if (result.Count > 0)
            {
                this.ObjectResultModule.Object = result;
            }
            else
            {
                this.ObjectResultModule.Object = null;
            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityCommitteeByUser",
                OperContent = JsonHelper.ToJson(secret),
                OperType = "QualityCommitteeByUser",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 申请质控委员 修改
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateQualityCommitteeRegister")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateQualityCommitteeRegister([FromBody] QualityCommitteeRegister QualityCommitteeRegisterInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeRegisterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateQualityCommitteeRegister = await _qualityCommitteeRegisterService.QualityCommitteeRegisterByID(QualityCommitteeRegisterInfo.Id);
            if (UpdateQualityCommitteeRegister != null)
            {
                UpdateQualityCommitteeRegister.ClinicName = QualityCommitteeRegisterInfo.ClinicName;
                UpdateQualityCommitteeRegister.ClinicID = QualityCommitteeRegisterInfo.ClinicID;
                UpdateQualityCommitteeRegister.DoctorID = QualityCommitteeRegisterInfo.DoctorID;
                UpdateQualityCommitteeRegister.DoctorName = QualityCommitteeRegisterInfo.Accomplish;
                UpdateQualityCommitteeRegister.Accomplish = QualityCommitteeRegisterInfo.ApplyRemark;
                UpdateQualityCommitteeRegister.ApplyRemark = QualityCommitteeRegisterInfo.ApplyState;
                UpdateQualityCommitteeRegister.ApplyState = QualityCommitteeRegisterInfo.CheckState;
                UpdateQualityCommitteeRegister.CheckState = QualityCommitteeRegisterInfo.CheckRemark;
                UpdateQualityCommitteeRegister.CheckRemark = QualityCommitteeRegisterInfo.CheckRemark;
                UpdateQualityCommitteeRegister.CheckTime = QualityCommitteeRegisterInfo.CheckTime;
                UpdateQualityCommitteeRegister.Checker = QualityCommitteeRegisterInfo.Checker;
                UpdateQualityCommitteeRegister.CreatedBy = QualityCommitteeRegisterInfo.CreatedBy;
                UpdateQualityCommitteeRegister.CreatedOn = QualityCommitteeRegisterInfo.CreatedOn;
                UpdateQualityCommitteeRegister.ModifyOn = DateTime.Now;
                UpdateQualityCommitteeRegister.ModifyBy = userid;
                var result = await _qualityCommitteeRegisterService.UpdateQualityCommitteeRegister(UpdateQualityCommitteeRegister);

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
                OperExplain = "UpdateQualityCommitteeRegister",
                OperContent = JsonHelper.ToJson(QualityCommitteeRegisterInfo),
                OperType = "UpdateQualityCommitteeRegister",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 申请质控委员 删除
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteQualityCommitteeRegister")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteQualityCommitteeRegister([FromBody] QualityCommitteeRegister QualityCommitteeRegisterInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeRegisterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _qualityCommitteeRegisterService.QualityCommitteeRegisterByID(QualityCommitteeRegisterInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _qualityCommitteeRegisterService.DeleteQualityCommitteeRegister(query);

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
                OperExplain = "DeleteQualityCommitteeRegister",
                OperContent = JsonHelper.ToJson(QualityCommitteeRegisterInfo),
                OperType = "DeleteQualityCommitteeRegister",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;

        }

        /// <summary>
        /// 申请质控委员 Page
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteeRegisterPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteeRegisterPage([FromBody]QualityCommitteeRegisterIn QualityCommitteeRegisterInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeRegisterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.StartTime))
            {
                StartTime = DateTime.Parse(QualityCommitteeRegisterInfo.StartTime);
                if (string.IsNullOrEmpty(QualityCommitteeRegisterInfo.EndTime))
                {
                    QualityCommitteeRegisterInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.EndTime))
            {
                EndTime = DateTime.Parse(QualityCommitteeRegisterInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.StartTime))
            {
                QualityCommitteeRegisterInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.KeyWord))
            {
                QualityCommitteeRegisterInfo.AndAlso(t => t.DoctorName.Contains(QualityCommitteeRegisterInfo.KeyWord));
            }
            QualityCommitteeRegisterInfo.AndAlso(t => t.IsDelete == false);
            //查询科室标签
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.KeyWord))
            {
                doctorRelationIn.AndAlso(a => a.LableName.Contains(QualityCommitteeRegisterInfo.KeyWord) || a.DoctorName.Contains(QualityCommitteeRegisterInfo.KeyWord));
            }
            var lableList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);

            var docrel = new DoctorRelationIn();
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.KeyWord))
            {
                docrel.AndAlso(t => (!t.IsDelete) && (t.DoctorName.Contains(QualityCommitteeRegisterInfo.KeyWord) || t.LableName.Contains(QualityCommitteeRegisterInfo.KeyWord)));
            }
            else
            {
                docrel.AndAlso(t => (!t.IsDelete));
            }
            var labelrel = await _DoctorRelationService.DoctorClinicRelationList(docrel);
            var ClinicInfo = await _qualityCommitteeRegisterService.QualityDoctorRegisterInformation(QualityCommitteeRegisterInfo, labelrel);
            // 查询科室信息

            DateTime StartTime1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime1 = StartTime1.AddDays(+1);
            EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            evaluationTotalIn.AndAlso(a => (a.CreatedOn >= StartTime1));
            evaluationTotalIn.AndAlso(a => (a.CreatedOn < EndTime1));
            var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);

            // var ClinicInfo = await _clinicInfomationService.DoctorInformation(YaeherDoctorSearch, labelrel);
            // 查询信息
            if (ClinicInfo != null && ClinicInfo.TotalCount > 0)
            {
                foreach (var item in ClinicInfo.Items)
                {
                    item.Doctorslable = lableList.Where(t => t.DoctorID == item.Id).ToList();

                    var evaluationTotal = evaluationTotalList.FirstOrDefault(t => t.DoctorID == item.Id);
                    if (evaluationTotal != null)
                    {
                        item.ReceiptNumBer =  evaluationTotal.CompleteTotal;//接单数
                        item.AverageTime =  evaluationTotal.AverageAnswer.ToString();//平均时长                    //  item.DoctorLevel = evaluationTotal == null ? "0" : evaluationTotal.AverageEvaluate.ToString();//星级
                        item.EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
                        if (item.EvaluationCount >= 15)
                        { item.AverageEvaluate = evaluationTotal.AverageEvaluate; }//星级
                    }
                    item.Doctorslable = lableList.Where(t => t.DoctorID == item.Id).ToList();
                    var qualitycontrol = await _qualityCommitteeService.QualityCommitteeByDoctorID(item.Id);
                    item.QualityControlId = qualitycontrol == null ? 0 : qualitycontrol.Id;
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
                this.ObjectResultModule.Object = new QualityDoctorInfoOut(ClinicDoctorsViewLists, QualityCommitteeRegisterInfo);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityCommitteeRegisterPage",
                OperContent = JsonHelper.ToJson(QualityCommitteeRegisterInfo),
                OperType = "QualityCommitteeRegisterPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 申请质控委员 List 
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteeRegisterList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteeRegisterList([FromBody]QualityCommitteeRegisterIn QualityCommitteeRegisterInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeRegisterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.StartTime))
            {
                StartTime = DateTime.Parse(QualityCommitteeRegisterInfo.StartTime);
                if (string.IsNullOrEmpty(QualityCommitteeRegisterInfo.EndTime))
                {
                    QualityCommitteeRegisterInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.EndTime))
            {
                EndTime = DateTime.Parse(QualityCommitteeRegisterInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.StartTime))
            {
                QualityCommitteeRegisterInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.KeyWord))
            {
                QualityCommitteeRegisterInfo.AndAlso(t => t.DoctorName.Contains(QualityCommitteeRegisterInfo.KeyWord));
            }
            QualityCommitteeRegisterInfo.AndAlso(t => t.IsDelete == false);
            var values = await _qualityCommitteeRegisterService.QualityCommitteeRegisterList(QualityCommitteeRegisterInfo);
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
                OperExplain = "QualityCommitteeRegisterList",
                OperContent = JsonHelper.ToJson(QualityCommitteeRegisterInfo),
                OperType = "QualityCommitteeRegisterList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 申请质控委员 Byid
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [Route("api/QualityCommitteeRegisterById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityCommitteeRegisterById([FromBody]QualityCommitteeRegisterIn QualityCommitteeRegisterInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeRegisterInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _qualityCommitteeRegisterService.QualityCommitteeRegisterDoctorMsgByID(QualityCommitteeRegisterInfo.Id);

            //查询标签
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            doctorRelationIn.AndAlso(a => a.DoctorID == values.Id);
            var lableList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);

            values.Doctorslable = lableList.Where(t => t.DoctorID == values.Id).ToList();
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
                OperExplain = "QualityCommitteeRegisterById",
                OperContent = JsonHelper.ToJson(QualityCommitteeRegisterInfo),
                OperType = "QualityCommitteeRegisterById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 报表
        /// <summary>
        /// 质控查看医生排行
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [Route("api/QualityDoctorRanking")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityDoctorRanking([FromBody]ConsultationOrderTotalIn QualityCommitteeInfo)
        {
            if (!Commons.CheckSecret(QualityCommitteeInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.StartTime))
            {
                StartTime = DateTime.Parse(QualityCommitteeInfo.StartTime);
                if (string.IsNullOrEmpty(QualityCommitteeInfo.EndTime))
                {
                    QualityCommitteeInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.EndTime))
            {
                EndTime = DateTime.Parse(QualityCommitteeInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.StartTime))
            {
                QualityCommitteeInfo.AndAlso(t => t.TotalDate >= StartTime );
                QualityCommitteeInfo.AndAlso(t => t.TotalDate < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.KeyWord))
            {
                QualityCommitteeInfo.AndAlso(t => t.DoctorName.Contains(QualityCommitteeInfo.KeyWord));
            }
            QualityCommitteeInfo.AndAlso(t => t.IsDelete == false);
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.TotalType))
            {
                QualityCommitteeInfo.AndAlso(t => t.TotalType == QualityCommitteeInfo.TotalType);
            }
            var values = await _consultationOrderTotalService.ConsultationOrderTotalPage(QualityCommitteeInfo);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ConsultationOrderTotalOut(values, QualityCommitteeInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityDoctorRanking",
                OperContent = JsonHelper.ToJson(QualityCommitteeInfo),
                OperType = "QualityDoctorRanking",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 质控查看退单统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/QualityReturnReport")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityReturnReport([FromBody] ConsultationIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            input.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(input.StartTime))
            {
                StartTime = DateTime.Parse(input.StartTime);
                if (string.IsNullOrEmpty(input.EndTime))
                {
                    input.EndTime = DateTime.Now.ToString();
                }
            }
            if (!string.IsNullOrEmpty(input.EndTime))
            {
                EndTime = DateTime.Parse(input.EndTime);
            }
            if (!string.IsNullOrEmpty(input.StartTime))
            {
                input.AndAlso(t => t.CreatedOn >= StartTime );
                input.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            var model = new RefundReport();
            var consul = await _consultationService.YaeherConsultationList(input);
            var refundcount = consul.Where(t => t.ConsultState == "return").ToList();
            if (consul.Count() <1)
            {
                model.RefundRate = "0";
            }
            else
            {
                model.RefundRate = Math.Round((Convert.ToDouble(refundcount.Count()) / Convert.ToDouble(consul.Count())) * 100, 4).ToString();
            }
            model.RefundCount = refundcount.Count();
            model.ConsultationCount = consul.Count();

            this.ObjectResultModule.Object = model;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityReturnReport",
                OperContent = JsonHelper.ToJson(input),
                OperType = "QualityReturnReport",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }


        #endregion
    }
}