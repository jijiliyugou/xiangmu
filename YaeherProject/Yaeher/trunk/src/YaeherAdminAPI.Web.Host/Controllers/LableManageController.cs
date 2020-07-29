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
using Yaeher.Extensions;
using Yaeher.LableManages;
using Yaeher.LableManages.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class LableManageController : YaeherAppServiceBase
    {
        private readonly ILableManageService _lableManageService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IDoctorRelationService _doctorRelationService;
        private readonly IAbpSession _IabpSession;
        private readonly IUserManagerService _userManagerService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IClinicDoctorReltionService _clinicDoctorReltionService;
        private readonly IClinicLableReltionService _clinicLableReltionService;
        private readonly IRelationLabelListService _relationLabelListService;
        
         /// <summary>
         /// 
         /// </summary>
         /// <param name="lableManageService"></param>
         /// <param name="session"></param>
         /// <param name="yaeherDoctorService"></param>
         /// <param name="doctorRelationService"></param>
         /// <param name="unitOfWorkManager"></param>
         /// <param name="userManagerService"></param>
         /// <param name="yaeherOperListService"></param>
         /// <param name="clinicDoctorReltionService"></param>
         /// <param name="clinicLableReltionService"></param>
         /// <param name="relationLabelListService"></param>
        public LableManageController(ILableManageService lableManageService,
                                     IAbpSession session,
                                     IYaeherDoctorService yaeherDoctorService,
                                     IDoctorRelationService doctorRelationService,
                                     IUnitOfWorkManager unitOfWorkManager,
                                     IUserManagerService userManagerService,
                                     IYaeherOperListService yaeherOperListService,
                                     IClinicDoctorReltionService clinicDoctorReltionService,
                                     IClinicLableReltionService clinicLableReltionService,
                                     IRelationLabelListService relationLabelListService)
        {
            _lableManageService = lableManageService;
            _yaeherDoctorService = yaeherDoctorService;
            _doctorRelationService = doctorRelationService;
            _unitOfWorkManager = unitOfWorkManager;
            _IabpSession = session;
            _userManagerService = userManagerService;
            _yaeherOperListService = yaeherOperListService;
            _clinicDoctorReltionService = clinicDoctorReltionService;
            _clinicLableReltionService = clinicLableReltionService;
            _relationLabelListService = relationLabelListService;
        }

        #region 标签
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateLable([FromBody] LableManage LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //var User = _userManagerService.UserManager(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            LableManageIn lableManageIn = new LableManageIn();
            lableManageIn.AndAlso(a => !a.IsDelete && a.LableName == LableManageInfo.LableName);
            var LableList = await _lableManageService.LableManageList(lableManageIn);
            DoctorRelation doctorRelation = new DoctorRelation();
            int LableID = 0;
            string LableName = string.Empty;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if (LableList.Count() > 0)
                {
                    if ((!usermanager.IsAdmin && usermanager.IsDoctor) || (usermanager.MobileRoleName == "doctor"))
                    {
                        DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
                        doctorRelationIn.AndAlso(a => a.IsDelete == false);
                        doctorRelationIn.AndAlso(a => a.DoctorID == doctor.Id);
                        doctorRelationIn.AndAlso(a => a.LableID == LableList[0].Id);
                        var doctorRelationList = await _doctorRelationService.DoctorClinicRelationList(doctorRelationIn);
                        if (doctorRelationList.Count == 0)
                        {
                            var rel = new DoctorRelation();
                            rel.DoctorName = doctor.DoctorName;
                            rel.DoctorID = doctor.Id;
                            rel.LableID = LableList[0].Id;
                            rel.LableName = LableList[0].LableName;
                            rel.LableJSON = JsonHelper.ToJson(LableList);
                            rel.CreatedBy = userid;
                            LableID = LableList[0].Id;
                            LableName = LableList[0].LableName;
                            doctorRelation = await _doctorRelationService.CreateDoctorRelation(rel);
                        }
                        else
                        {
                            this.ObjectResultModule.Object = "";
                            this.ObjectResultModule.StatusCode = 201;
                            this.ObjectResultModule.Message = "已存在该标签，不用重复提交！";
                        }
                    }
                }
                else
                {
                    var CreateLable = new LableManage()
                    {
                        LableName = LableManageInfo.LableName,
                        LableRemark = LableManageInfo.LableRemark,
                        OrderSort = LableManageInfo.OrderSort,
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                    };
                    var result = await _lableManageService.CreateLableManage(CreateLable);
                    #region 判断是否为医生  当为医生新建时 绑定给到新医生
                    if ((!usermanager.IsAdmin && usermanager.IsDoctor) || (usermanager.MobileRoleName == "doctor"))
                    {
                        var rel = new DoctorRelation();
                        rel.DoctorName = doctor.DoctorName;
                        rel.DoctorID = doctor.Id;
                        rel.LableID = result.Id;
                        rel.LableName = result.LableName;
                        rel.LableJSON = JsonHelper.ToJson(result);
                        rel.CreatedBy = userid;
                        LableID = result.Id;
                        LableName = result.LableName;
                        doctorRelation = await _doctorRelationService.CreateDoctorRelation(rel);
                    }
                    #endregion
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
                }
                #region 增加医生与标签关系 
                if ((!usermanager.IsAdmin && usermanager.IsDoctor) || (usermanager.MobileRoleName == "doctor"))
                { 
                    RelationLabelList relationLabel = new RelationLabelList();
                    relationLabel.RelationCode = "Doctor";
                    relationLabel.BusinessID = doctor.Id;
                    relationLabel.BusinessName = doctor.DoctorName;
                    relationLabel.LableID = LableID;
                    relationLabel.LableName = LableName;
                    relationLabel.CreatedBy = userid;
                    relationLabel.CreatedOn = DateTime.Now;
                    var Lableresul = await _relationLabelListService.CreateRelationLabelList(relationLabel);
                }
                #endregion
                #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateLable",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "CreateLable",
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
        /// 标签修改
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateLable([FromBody] LableManage LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateLable = await _lableManageService.LableManageByID(LableManageInfo.Id);
            if (UpdateLable != null)
            {
                UpdateLable.LableName = LableManageInfo.LableName;
                UpdateLable.LableRemark = LableManageInfo.LableRemark;
                UpdateLable.CreatedBy = LableManageInfo.CreatedBy;
                UpdateLable.CreatedOn = LableManageInfo.CreatedOn;
                UpdateLable.OrderSort = LableManageInfo.OrderSort;
                UpdateLable.ModifyOn = DateTime.Now;
                UpdateLable.ModifyBy = userid;
                var result = await _lableManageService.UpdateLableManage(UpdateLable);

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
                OperExplain = "UpdateLable",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "UpdateLable",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 标签删除
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteLable([FromBody] LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _lableManageService.LableManageByID(LableManageInfo.Id);
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            doctorRelationIn.AndAlso(a => a.LableID == LableManageInfo.Id);
            var doctorLabelList = await _doctorRelationService.DoctorClinicRelationList(doctorRelationIn);
            if (doctorLabelList.Count() > 0)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "该标签绑定医生或者科室，不可删除！";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _lableManageService.DeleteLableManage(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteLable",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "DeleteLable",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取科室对应标签 Page
        /// </summary>
        /// <param name="LableManageInfo"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/LablePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> LablePage([FromBody]LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
           // var usermanager = _userManagerService.UserManager(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(LableManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(LableManageInfo.StartTime);
                if (string.IsNullOrEmpty(LableManageInfo.EndTime))
                {
                    LableManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(LableManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(LableManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(LableManageInfo.StartTime))
            {
                LableManageInfo.AndAlso(t => t.CreatedOn >= StartTime);
                LableManageInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(LableManageInfo.KeyWord))
            {
                LableManageInfo.AndAlso(t => t.LableName.Contains(LableManageInfo.KeyWord));
            }
            LableManageInfo.AndAlso(t => t.IsDelete == false);
       
            if (usermanager.MobileRoleName == "doctor"||usermanager.IsDoctor)
            {
                if (LableManageInfo.Platform == "PC")
                {
                    LableManageInfo.DoctorID = usermanager.DoctorID;
                }
                else if(LableManageInfo.Platform == "Mobile")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    LableManageInfo.DoctorID = doctor.Id;
                }
                var value = await _doctorRelationService.LabelDoctorRelationPage(LableManageInfo);
                this.ObjectResultModule.Object = new DoctorRelationOut(value, LableManageInfo);
            }
            else
            {
                var values = await _lableManageService.LableManagePage(LableManageInfo);
                this.ObjectResultModule.Object = new LableManageOut(values, LableManageInfo);
            }

            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "LablePage",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "LablePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取科室对应标签 List 
        /// </summary>
        /// <param name="LableManageInfo"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/LableList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> LableList([FromBody]LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //var usermanager = _userManagerService.UserManager(userid);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(LableManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(LableManageInfo.StartTime);
                if (string.IsNullOrEmpty(LableManageInfo.EndTime))
                {
                    LableManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(LableManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(LableManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(LableManageInfo.StartTime))
            {
                LableManageInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(LableManageInfo.KeyWord))
            {
                LableManageInfo.AndAlso(t => t.LableName.Contains(LableManageInfo.KeyWord));
            }
            LableManageInfo.AndAlso(t => t.IsDelete == false);
            var values = await _lableManageService.LableManageList(LableManageInfo);
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
                OperExplain = "LableList",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "LableList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 获取科室对应标签 List 
        /// </summary>
        /// <param name="LableManageInfo"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/LableClinicList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> LableClinicList([FromBody]LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
          
            var LableManagemanage = new LabelClinicManageIn() { ClinicID = LableManageInfo.ClinicID };
            var values = await _lableManageService.LableClinicManageInList(LableManagemanage);
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
                OperExplain = "LableClinicList",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "LableClinicList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 获取标签信息 Byid
        /// </summary>
        /// <param name="LableManageInfo"> LableManageIn 数据</param>
        /// <returns></returns>
        [Route("api/LableById")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> LableById([FromBody]LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _lableManageService.LableManageByID(LableManageInfo.Id);
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
                OperExplain = "LableById",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "LableById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 获取标签信息 Byid
        /// </summary>
        /// <param name="LableManageInfo"> LableManageIn 数据</param>
        /// <returns></returns>
        [Route("api/LableByName")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> LableByName([FromBody]LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (!string.IsNullOrEmpty(LableManageInfo.LableName))
            {
                LableManageInfo.AndAlso(t => t.LableName == LableManageInfo.LableName);
            }
            if (LableManageInfo.Id > 0)
            {
                LableManageInfo.AndAlso(t => t.Id == LableManageInfo.Id);
            }
            LableManageInfo.AndAlso(t => !t.IsDelete);
            var values = await _lableManageService.LableManageByName(LableManageInfo);
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
                OperExplain = "LableByName",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "LableByName",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 获取标签信息 Byid
        /// </summary>
        /// <param name="LableManageInfo"> LableManageIn 数据</param>
        /// <returns></returns>
        [Route("api/DoctorLables")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> DoctorLables([FromBody]LableManageIn LableManageInfo)
        {
            if (!Commons.CheckSecret(LableManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (!string.IsNullOrEmpty(LableManageInfo.LableName))
            {
                LableManageInfo.AndAlso(t => t.LableName == LableManageInfo.LableName);
            }
            if (LableManageInfo.Id > 0)
            {
                LableManageInfo.AndAlso(t => t.Id == LableManageInfo.Id);
            }
            LableManageInfo.AndAlso(t => !t.IsDelete);
            // 医生现有标签
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.DoctorID == LableManageInfo.DoctorID);
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            var DoctorLableList = await _doctorRelationService.DoctorRelationList(doctorRelationIn);
            // 查询所有标签
            LableManageIn lableManageIn = new LableManageIn();
            lableManageIn.AndAlso(a => a.IsDelete == false);
            var LableList = await _lableManageService.LableManageList(lableManageIn);
            // 查询医生所在的科室
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            clinicDoctorReltionIn.AndAlso(a => a.DoctorID == LableManageInfo.DoctorID);
            var clinicDoctor = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            List<LableManage> lableManageList = new List<LableManage>();
            if (clinicDoctor!=null)
            {
                // 查询所有科室的标签数据
                ClinicLableReltionIn clinicLableReltionIn = new ClinicLableReltionIn();
                clinicLableReltionIn.AndAlso(a => a.IsDelete == false);
                var clininLable = await _clinicLableReltionService.ClinicDoctorReltionList(clinicLableReltionIn);
                if(clininLable!=null)
                {
                    var lableManages = from a in clinicDoctor
                                       join b in clininLable on a.ClinicID equals b.ClinicID
                                       join c in LableList on b.LableID equals c.Id
                                       select c;
                    if (lableManages != null)
                    {
                        lableManageList = lableManages.ToList();
                    }
                }
            }
            if (lableManageList == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                if (LableManageInfo.Platform == "PC")
                {
                    this.ObjectResultModule.Object = lableManageList;
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                else
                {
                    var LableLists = from a in lableManageList
                                     where !(from b in DoctorLableList select b.LableID).Contains(a.Id)
                                     select a;
                    this.ObjectResultModule.Object = LableLists.ToList();
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "LableByName",
                OperContent = JsonHelper.ToJson(LableManageInfo),
                OperType = "LableByName",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion
    }
}