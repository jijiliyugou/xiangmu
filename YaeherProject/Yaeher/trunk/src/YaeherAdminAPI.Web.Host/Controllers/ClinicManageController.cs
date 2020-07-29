using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
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
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 科室 科室与标签 科室与医生关系
    /// </summary>
    public class ClinicManageController : YaeherAppServiceBase
    {

        private readonly IClinicInfomationService _clinicInfomationService;
        private readonly IClinicDoctorReltionService _clinicDoctorReltionService;
        private readonly IClinicLableReltionService _clinicLableReltionService;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherDoctorService _YaeherDoctorService;
        private readonly IAbpSession _IabpSession;
        private readonly IClinicManageService _clinicManageService;
        private readonly ILableManageService _lableManageService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IRelationLabelListService _relationLabelListService;

        /// <summary>
        /// 科室 科室与标签 科室与医生关系
        /// </summary>
        /// <param name="clinicInfomationService"></param>
        /// <param name="clinicDoctorReltionService"></param>
        /// <param name="clinicLableReltionService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="session"></param>
        /// <param name="clinicManageService"></param>
        /// <param name="lableManageService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="relationLabelListService"></param>
        public ClinicManageController(IClinicInfomationService clinicInfomationService,
                                      IClinicDoctorReltionService clinicDoctorReltionService,
                                      IClinicLableReltionService clinicLableReltionService,
                                      IUserManagerService userManagerService,
                                      IYaeherDoctorService yaeherDoctorService,
                                      IAbpSession session,
                                      IClinicManageService clinicManageService,
                                      ILableManageService lableManageService,
                                      IYaeherOperListService yaeherOperListService,
                                      IRelationLabelListService relationLabelListService)
        {
            _clinicInfomationService = clinicInfomationService;
            _clinicDoctorReltionService = clinicDoctorReltionService;
            _clinicLableReltionService = clinicLableReltionService;
            _userManagerService = userManagerService;
            _YaeherDoctorService = yaeherDoctorService;
            _IabpSession = session;
            _clinicManageService = clinicManageService;
            _lableManageService = lableManageService;
            _yaeherOperListService = yaeherOperListService;
            _relationLabelListService = relationLabelListService;
        }


        #region 维护科室
        /// <summary>
        /// 科室新增
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/CreateClinic")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateClinic([FromBody] ClinicInfomation ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicInfomationIn clinicInfomationIn = new ClinicInfomationIn();
            clinicInfomationIn.AndAlso(a => a.IsDelete == false);
            clinicInfomationIn.AndAlso(a => a.ClinicName == ClinicInfomationInfo.ClinicName);
            clinicInfomationIn.AndAlso(a => a.ClinicType == ClinicInfomationInfo.ClinicType);
            var ClinicList = await _clinicInfomationService.ClinicInfomationList(clinicInfomationIn);
            // 重复数据判断
            if (ClinicList.Count() > 0)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "Data Duplication";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var CreateClinic = new ClinicInfomation()
            {
                ClinicName = ClinicInfomationInfo.ClinicName,
                ClinicIntro = ClinicInfomationInfo.ClinicIntro,
                ClinicDirector = ClinicInfomationInfo.ClinicDirector,
                ClinicType = ClinicInfomationInfo.ClinicType,
                OrderSort = ClinicInfomationInfo.OrderSort,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
            };
            var result = await _clinicInfomationService.CreateClinicInfomation(CreateClinic);
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
                OperExplain = "CreateClinic",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "CreateClinic",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室修改
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateClinic")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateClinic([FromBody] ClinicInfomation ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicInfomationIn clinicInfomationIn = new ClinicInfomationIn();
            clinicInfomationIn.AndAlso(a => a.IsDelete == false);
            clinicInfomationIn.AndAlso(a => a.ClinicName == ClinicInfomationInfo.ClinicName);
            clinicInfomationIn.AndAlso(a => a.ClinicType == ClinicInfomationInfo.ClinicType);
            var ClinicList = await _clinicInfomationService.ClinicInfomationList(clinicInfomationIn);
            // 重复数据判断
            if (ClinicList.Count() > 1)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "Data Duplication";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var UpdateClinic = await _clinicInfomationService.ClinicInfomationByID(ClinicInfomationInfo.Id);
            if (UpdateClinic != null)
            {
                if (!string.IsNullOrEmpty(ClinicInfomationInfo.ClinicIntro))
                {
                    UpdateClinic.ClinicIntro = ClinicInfomationInfo.ClinicIntro;
                }
                if (ClinicInfomationInfo.ClinicDirector > 0)
                {
                    UpdateClinic.ClinicDirector = ClinicInfomationInfo.ClinicDirector;
                }
                if (ClinicInfomationInfo.ClinicType > 0)
                {
                    UpdateClinic.ClinicType = ClinicInfomationInfo.ClinicType;
                }
                if (!string.IsNullOrEmpty(ClinicInfomationInfo.ClinicName))
                {
                    UpdateClinic.ClinicName = ClinicInfomationInfo.ClinicName;
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 100;
                    this.ObjectResultModule.Message = "科室名称不可为空！";
                    return ObjectResultModule;
                }
                UpdateClinic.OrderSort = ClinicInfomationInfo.OrderSort;
                UpdateClinic.ModifyOn = DateTime.Now;
                UpdateClinic.ModifyBy = userid;
                var result = await _clinicInfomationService.UpdateClinicInfomation(UpdateClinic);
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
                OperExplain = "UpdateClinic",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "UpdateClinic",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室删除
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteClinic")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteClinic([FromBody] ClinicInfomation ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _clinicInfomationService.ClinicInfomationByID(ClinicInfomationInfo.Id);
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            clinicDoctorReltionIn.AndAlso(a => a.ClinicID == ClinicInfomationInfo.Id);
            var ClinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            if (ClinicDoctorList.Count() > 0)
            {
                this.ObjectResultModule.Message = "科室存在医生不可删除！";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _clinicInfomationService.DeleteClinicInfomation(query);

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
                OperExplain = "DeleteClinic",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "DeleteClinic",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;

        }

        /// <summary>
        /// 获取科室 Page
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicPage([FromBody]ClinicInfomationIn ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ClinicInfomationInfo.StartTime))
            {
                StartTime = DateTime.Parse(ClinicInfomationInfo.StartTime);
                if (string.IsNullOrEmpty(ClinicInfomationInfo.EndTime))
                {
                    ClinicInfomationInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ClinicInfomationInfo.EndTime))
            {
                EndTime = DateTime.Parse(ClinicInfomationInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ClinicInfomationInfo.StartTime))
            {
                ClinicInfomationInfo.AndAlso(t => t.CreatedOn >= StartTime);
                ClinicInfomationInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ClinicInfomationInfo.KeyWord))
            {
                ClinicInfomationInfo.AndAlso(t => t.ClinicName.Contains(ClinicInfomationInfo.KeyWord));
            }
            ClinicInfomationInfo.AndAlso(a => a.IsDelete == false);
            if (ClinicInfomationInfo.ClinicType > 0)
            {
                ClinicInfomationInfo.AndAlso(t => t.ClinicType == ClinicInfomationInfo.ClinicType);
            }
            var values = await _clinicInfomationService.ClinicInfomationPage(ClinicInfomationInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ClinicInfomationOut(values, ClinicInfomationInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ClinicPage",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "ClinicPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取科室 List 
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicList([FromBody]ClinicInfomationIn ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicInfomationInfo.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ClinicInfomationInfo.ClinicName))
            {
                ClinicInfomationInfo.AndAlso(t => t.ClinicName.Contains(ClinicInfomationInfo.ClinicName));
            }
            if (ClinicInfomationInfo.ClinicType > 0)
            {
                ClinicInfomationInfo.AndAlso(t => t.ClinicType == ClinicInfomationInfo.ClinicType);
            }
            var values = await _clinicInfomationService.ClinicInfomationList(ClinicInfomationInfo);
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
                OperExplain = "ClinicList",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "ClinicList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 获取医生不在的科室 List 
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOutClinicList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOutClinicList([FromBody]ClinicInfomationIn ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            if (usermanager.MobileRoleName == "doctor" || usermanager.IsDoctor)
            {
                var DoctorId = 0;
                if (ClinicInfomationInfo.Platform == "Mobile")
                {
                    var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
                    DoctorId = doctor.Id;
                }
                else if (ClinicInfomationInfo.Platform == "PC")
                {
                    DoctorId = usermanager.DoctorID;
                }
                ClinicInfomationInfo.DoctorId = DoctorId;

            }
            if (usermanager.MobileRoleName == "customerservice" || usermanager.IsCustomerService)
            {
                var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(ClinicInfomationInfo.DoctorId);
                ClinicInfomationInfo.DoctorId = doctor.Id;
            }
            ClinicInfomationInfo.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ClinicInfomationInfo.ClinicName))
            {
                ClinicInfomationInfo.AndAlso(t => t.ClinicName.Contains(ClinicInfomationInfo.ClinicName));
            }
            if (ClinicInfomationInfo.ClinicType > 0)
            {
                ClinicInfomationInfo.AndAlso(t => t.ClinicType == ClinicInfomationInfo.ClinicType);
            }
            var rel = new ClinicDoctorReltionIn(); rel.AndAlso(t => !t.IsDelete && t.DoctorID == ClinicInfomationInfo.DoctorId);
            var clinicdoctor = await _clinicDoctorReltionService.ClinicDoctorReltionList(rel);
            var values = await _clinicInfomationService.ClinicInfomationList(ClinicInfomationInfo, clinicdoctor.ToList());

            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorOutClinicList",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "DoctorOutClinicList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 获取科室 Byid
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicById([FromBody]ClinicInfomationIn ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _clinicInfomationService.ClinicInfomationByID(ClinicInfomationInfo.Id);
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
                OperExplain = "ClinicById",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "ClinicById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 科室与医生
        /// <summary>
        /// 科室与医生
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [Route("api/CreateClinicDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateClinicDoctor([FromBody] ClinicDoctorReltion ClinicDoctorReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicDoctorReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.ClinicID == ClinicDoctorReltionInfo.ClinicID);
            clinicDoctorReltionIn.AndAlso(a => a.DoctorID == ClinicDoctorReltionInfo.DoctorID);
            var ClinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            if (ClinicDoctorList.Count() > 0)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "Data Duplication";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var CreateClinicDoctor = new ClinicDoctorReltion()
            {
                ClinicID = ClinicDoctorReltionInfo.ClinicID,
                ClinicName = ClinicDoctorReltionInfo.ClinicName,
                ClinicJSON = ClinicDoctorReltionInfo.ClinicJSON,
                DoctorID = ClinicDoctorReltionInfo.DoctorID,
                DoctorName = ClinicDoctorReltionInfo.DoctorName,
                DoctorJSON = ClinicDoctorReltionInfo.DoctorJSON,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
            };
            var result = await _clinicDoctorReltionService.CreateClinicDoctorReltion(CreateClinicDoctor);
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
                OperExplain = "CreateClinicDoctor",
                OperContent = JsonHelper.ToJson(ClinicDoctorReltionInfo),
                OperType = "CreateClinicDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室与医生修改
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateClinicDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateClinicDoctor([FromBody] ClinicDoctorReltion ClinicDoctorReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicDoctorReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateClinicDoctor = await _clinicDoctorReltionService.ClinicDoctorReltionByID(ClinicDoctorReltionInfo.Id);
            if (UpdateClinicDoctor != null)
            {
                UpdateClinicDoctor.ClinicID = ClinicDoctorReltionInfo.ClinicID;
                UpdateClinicDoctor.ClinicName = ClinicDoctorReltionInfo.ClinicName;
                UpdateClinicDoctor.ClinicJSON = ClinicDoctorReltionInfo.ClinicJSON;
                UpdateClinicDoctor.DoctorID = ClinicDoctorReltionInfo.DoctorID;
                UpdateClinicDoctor.DoctorName = ClinicDoctorReltionInfo.DoctorName;
                UpdateClinicDoctor.DoctorJSON = ClinicDoctorReltionInfo.DoctorJSON;
                UpdateClinicDoctor.ModifyOn = DateTime.Now;
                UpdateClinicDoctor.ModifyBy = userid;
                var result = await _clinicDoctorReltionService.UpdateClinicDoctorReltion(UpdateClinicDoctor);

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
                OperExplain = "UpdateClinicDoctor",
                OperContent = JsonHelper.ToJson(ClinicDoctorReltionInfo),
                OperType = "UpdateClinicDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室与医生删除
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteClinicDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteClinicDoctor([FromBody] ClinicDoctorReltion ClinicDoctorReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicDoctorReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _clinicDoctorReltionService.ClinicDoctorReltionByID(ClinicDoctorReltionInfo.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _clinicDoctorReltionService.DeleteClinicDoctorReltion(query);

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
                OperExplain = "DeleteClinicDoctor",
                OperContent = JsonHelper.ToJson(ClinicDoctorReltionInfo),
                OperType = "DeleteClinicDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 科室与医生 Page
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicDoctorPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicDoctorPage([FromBody]ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicDoctorReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.StartTime))
            {
                StartTime = DateTime.Parse(ClinicDoctorReltionInfo.StartTime);
                if (string.IsNullOrEmpty(ClinicDoctorReltionInfo.EndTime))
                {
                    ClinicDoctorReltionInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.EndTime))
            {
                EndTime = DateTime.Parse(ClinicDoctorReltionInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.StartTime))
            {
                ClinicDoctorReltionInfo.AndAlso(t => t.CreatedOn >= StartTime);
                ClinicDoctorReltionInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.KeyWord))
            {
                ClinicDoctorReltionInfo.AndAlso(t => t.ClinicName.Contains(ClinicDoctorReltionInfo.KeyWord) ||
                                                     t.DoctorName.Contains(ClinicDoctorReltionInfo.KeyWord));
            }
            ClinicDoctorReltionInfo.AndAlso(t => t.IsDelete == false);
            var values = await _clinicDoctorReltionService.ClinicDoctorReltionPage(ClinicDoctorReltionInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ClinicDoctorReltionOut(values, ClinicDoctorReltionInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ClinicDoctorPage",
                OperContent = JsonHelper.ToJson(ClinicDoctorReltionInfo),
                OperType = "ClinicDoctorPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 科室与医生 List 
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicDoctorList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicDoctorList([FromBody]ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicDoctorReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.StartTime))
            {
                StartTime = DateTime.Parse(ClinicDoctorReltionInfo.StartTime);
                if (string.IsNullOrEmpty(ClinicDoctorReltionInfo.EndTime))
                {
                    ClinicDoctorReltionInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.EndTime))
            {
                EndTime = DateTime.Parse(ClinicDoctorReltionInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.StartTime))
            {
                ClinicDoctorReltionInfo.AndAlso(t => t.CreatedOn >= StartTime);
                ClinicDoctorReltionInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.KeyWord))
            {
                ClinicDoctorReltionInfo.AndAlso(t => t.ClinicName.Contains(ClinicDoctorReltionInfo.KeyWord) ||
                                                     t.DoctorName.Contains(ClinicDoctorReltionInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.ClinicName))
            {
                ClinicDoctorReltionInfo.AndAlso(t => t.ClinicName.Contains(ClinicDoctorReltionInfo.ClinicName));
            }
            if (!string.IsNullOrEmpty(ClinicDoctorReltionInfo.DoctorName))
            {
                ClinicDoctorReltionInfo.AndAlso(t => t.DoctorName.Contains(ClinicDoctorReltionInfo.DoctorName));
            }
            ClinicDoctorReltionInfo.AndAlso(t => t.IsDelete == false);
            var values = await _clinicDoctorReltionService.ClinicDoctorReltionList(ClinicDoctorReltionInfo);
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
                OperExplain = "ClinicDoctorList",
                OperContent = JsonHelper.ToJson(ClinicDoctorReltionInfo),
                OperType = "ClinicDoctorList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室与医生 Byid
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicDoctorById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicDoctorById([FromBody]ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicDoctorReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _clinicDoctorReltionService.ClinicDoctorReltionByID(ClinicDoctorReltionInfo.Id);
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
                OperExplain = "ClinicDoctorById",
                OperContent = JsonHelper.ToJson(ClinicDoctorReltionInfo),
                OperType = "ClinicDoctorById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 科室与标签
        /// <summary>
        /// 科室与标签注册
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [Route("api/CreateClinicLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateClinicLable([FromBody] ClinicLableReltion ClinicLableReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicLableReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicLableReltionIn clinicLableReltionIn = new ClinicLableReltionIn();
            clinicLableReltionIn.AndAlso(a => a.IsDelete == false);
            clinicLableReltionIn.AndAlso(a => a.LableID == ClinicLableReltionInfo.LableID);
            clinicLableReltionIn.AndAlso(a => a.ClinicID == ClinicLableReltionInfo.ClinicID);
            var ClinicLableList = await _clinicLableReltionService.ClinicDoctorReltionList(clinicLableReltionIn);
            if (ClinicLableList.Count() > 0)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "Data Duplication";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var CreateClinicLable = new ClinicLableReltion()
            {
                ClinicID = ClinicLableReltionInfo.ClinicID,
                ClinicName = ClinicLableReltionInfo.ClinicName,
                ClinicJSON = ClinicLableReltionInfo.ClinicJSON,
                LableID = ClinicLableReltionInfo.LableID,
                LableName = ClinicLableReltionInfo.LableName,
                LableJSON = ClinicLableReltionInfo.LableJSON,

                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _clinicLableReltionService.CreateClinicDoctorReltion(CreateClinicLable);
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
                OperExplain = "CreateClinicLable",
                OperContent = JsonHelper.ToJson(ClinicLableReltionInfo),
                OperType = "CreateClinicLable",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室与标签修改
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateClinicLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateClinicLable([FromBody] ClinicLableReltion ClinicLableReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicLableReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateClinicLable = await _clinicLableReltionService.ClinicDoctorReltionByID(ClinicLableReltionInfo.Id);
            if (UpdateClinicLable != null)
            {
                UpdateClinicLable.ClinicID = ClinicLableReltionInfo.ClinicID;
                UpdateClinicLable.ClinicName = ClinicLableReltionInfo.ClinicName;
                UpdateClinicLable.ClinicJSON = ClinicLableReltionInfo.ClinicJSON;
                UpdateClinicLable.LableID = ClinicLableReltionInfo.LableID;
                UpdateClinicLable.LableName = ClinicLableReltionInfo.LableName;
                UpdateClinicLable.LableJSON = ClinicLableReltionInfo.LableJSON;
                UpdateClinicLable.ModifyOn = DateTime.Now;
                UpdateClinicLable.ModifyBy = userid;
                var result = await _clinicLableReltionService.UpdateClinicDoctorReltion(UpdateClinicLable);
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
                OperExplain = "UpdateClinicLable",
                OperContent = JsonHelper.ToJson(ClinicLableReltionInfo),
                OperType = "UpdateClinicLable",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室与标签删除
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteClinicLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteClinicLable([FromBody] ClinicLableReltion ClinicLableReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicLableReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _clinicLableReltionService.ClinicDoctorReltionByID(ClinicLableReltionInfo.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _clinicLableReltionService.DeleteClinicDoctorReltion(query);

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
                OperExplain = "DeleteClinicLable",
                OperContent = JsonHelper.ToJson(ClinicLableReltionInfo),
                OperType = "DeleteClinicLable",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 科室与标签 Page
        /// </summary>
        /// <param name="ClinicLableReltionInfo"> </param>
        /// <returns></returns>
        [Route("api/ClinicLablePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicLablePage([FromBody]ClinicLableReltionIn ClinicLableReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicLableReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.StartTime))
            {
                StartTime = DateTime.Parse(ClinicLableReltionInfo.StartTime);
                if (string.IsNullOrEmpty(ClinicLableReltionInfo.EndTime))
                {
                    ClinicLableReltionInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.EndTime))
            {
                EndTime = DateTime.Parse(ClinicLableReltionInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.StartTime))
            {
                ClinicLableReltionInfo.AndAlso(t => t.CreatedOn >= StartTime);
                ClinicLableReltionInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.KeyWord))
            {
                ClinicLableReltionInfo.AndAlso(t => t.ClinicName.Contains(ClinicLableReltionInfo.KeyWord) ||
                                                     t.LableName.Contains(ClinicLableReltionInfo.KeyWord));
            }
            ClinicLableReltionInfo.AndAlso(t => t.IsDelete == false);
            var values = await _clinicLableReltionService.ClinicDoctorReltionPage(ClinicLableReltionInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ClinicLableReltionOut(values, ClinicLableReltionInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ClinicLablePage",
                OperContent = JsonHelper.ToJson(ClinicLableReltionInfo),
                OperType = "ClinicLablePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 科室与标签 List 
        /// </summary>
        /// <param name="ClinicLableReltionInfo"> </param>
        /// <returns></returns>
        [Route("api/ClinicLableList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicLableList([FromBody]ClinicLableReltionIn ClinicLableReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicLableReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.StartTime))
            {
                StartTime = DateTime.Parse(ClinicLableReltionInfo.StartTime);
                if (string.IsNullOrEmpty(ClinicLableReltionInfo.EndTime))
                {
                    ClinicLableReltionInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.EndTime))
            {
                EndTime = DateTime.Parse(ClinicLableReltionInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.StartTime))
            {
                ClinicLableReltionInfo.AndAlso(t => t.CreatedOn >= StartTime);
                ClinicLableReltionInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ClinicLableReltionInfo.KeyWord))
            {
                ClinicLableReltionInfo.AndAlso(t => t.ClinicName.Contains(ClinicLableReltionInfo.KeyWord) ||
                                                     t.LableName.Contains(ClinicLableReltionInfo.KeyWord));
            }
            ClinicLableReltionInfo.AndAlso(t => t.IsDelete == false);
            var values = await _clinicLableReltionService.ClinicDoctorReltionList(ClinicLableReltionInfo);
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
                OperExplain = "ClinicLableList",
                OperContent = JsonHelper.ToJson(ClinicLableReltionInfo),
                OperType = "ClinicLableList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 科室与标签 Byid
        /// </summary>
        /// <param name="ClinicLableReltionInfo"> </param>
        /// <returns></returns>
        [Route("api/ClinicLableById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicLableById([FromBody]ClinicLableReltionIn ClinicLableReltionInfo)
        {
            if (!Commons.CheckSecret(ClinicLableReltionInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _clinicLableReltionService.ClinicDoctorReltionByID(ClinicLableReltionInfo.Id);
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
                OperExplain = "ClinicLableById",
                OperContent = JsonHelper.ToJson(ClinicLableReltionInfo),
                OperType = "ClinicLableById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 门诊 标签  医生 关系
        /// <summary>
        /// 门诊 标签  医生 关系
        /// </summary>
        /// <param name="ClinicInfomationInfo"> </param>
        /// <returns></returns>
        [Route("api/ClinicDetailList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicDetailList([FromBody]ClinicInfomationIn ClinicInfomationInfo)
        {
            if (!Commons.CheckSecret(ClinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicInfomationInfo.AndAlso(a => a.IsDelete == false);
            if (ClinicInfomationInfo.Id > 0)
            {
                ClinicInfomationInfo.AndAlso(a => a.Id == ClinicInfomationInfo.Id);
            }
            var values = await _clinicManageService.ClinicInfoList(ClinicInfomationInfo);
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
                OperExplain = "ClinicDetailList",
                OperContent = JsonHelper.ToJson(ClinicInfomationInfo),
                OperType = "ClinicDetailList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 科室与标签关系
        /// <summary>
        /// 科室与医生关系新增
        /// </summary>
        /// <param name="ClinicLableInfo"></param>
        /// <returns></returns>
        [Route("api/CreateClinicAndLable")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateClinicAndLable([FromBody] ClinicLableIn ClinicLableInfo)
        {
            if (!Commons.CheckSecret(ClinicLableInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            try
            {
                var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
                #region 先删除
                ClinicLableReltionIn clinicLableReltionIn = new ClinicLableReltionIn();
                clinicLableReltionIn.AndAlso(a => a.IsDelete == false);
                clinicLableReltionIn.AndAlso(a => a.ClinicID == ClinicLableInfo.ClinicID);
                var clinicLableList = await _clinicLableReltionService.ClinicDoctorReltionList(clinicLableReltionIn);
                if (clinicLableList.Count > 0)
                {
                    foreach (var clinicLableInfo in clinicLableList)
                    {
                        clinicLableInfo.DeleteBy = userid;
                        clinicLableInfo.DeleteTime = DateTime.Now;
                        clinicLableInfo.IsDelete = true;
                        var result = await _clinicLableReltionService.DeleteClinicDoctorReltion(clinicLableInfo);
                    }
                }
                #endregion
                #region 再新增
                string[] LableIDArray = null;
                if (!string.IsNullOrEmpty(ClinicLableInfo.LableIDJSON))
                {
                    LableIDArray = ClinicLableInfo.LableIDJSON.Split(',');
                }
                LableManageIn lableManageIn = new LableManageIn();
                lableManageIn.AndAlso(a => a.IsDelete == false);
                var lableList = await _lableManageService.LableManageList(lableManageIn);
                ClinicInfomationIn clinicInfomationIn = new ClinicInfomationIn();
                clinicInfomationIn.AndAlso(a => a.IsDelete == false);
                var clinicList = await _clinicInfomationService.ClinicInfomationList(clinicInfomationIn);
                var resultAll = 0;
                StringBuilder LableNameList = new StringBuilder();
                string ClinicName = string.Empty;
                if (LableIDArray != null && LableIDArray.Length > 0)
                {
                    for (int a = 0; a < LableIDArray.Length; a++)
                    {
                        ClinicLableReltion clinicLableReltion = new ClinicLableReltion();
                        clinicLableReltion.ClinicID = ClinicLableInfo.ClinicID;
                        var clinicInfos = clinicList.Where(t => t.Id == ClinicLableInfo.ClinicID).FirstOrDefault();
                        if (clinicInfos != null)
                        {
                            clinicLableReltion.ClinicName = clinicInfos.ClinicName;
                            ClinicName = clinicInfos.ClinicName;
                            clinicLableReltion.ClinicJSON = JsonHelper.ToJson(clinicInfos);
                            var LabelInfo = lableList.Where(t => t.Id == int.Parse(LableIDArray[a].ToString())).FirstOrDefault();
                            if (LabelInfo != null)
                            {
                                clinicLableReltion.LableID = LabelInfo.Id;
                                clinicLableReltion.LableName = LabelInfo.LableName;
                                clinicLableReltion.LableJSON = JsonHelper.ToJson(LabelInfo);
                                clinicLableReltion.CreatedBy = userid;
                                clinicLableReltion.CreatedOn = DateTime.Now;
                                var res = await _clinicLableReltionService.CreateClinicDoctorReltion(clinicLableReltion);
                                resultAll = +res.Id;
                            }
                        }
                        LableNameList.Append(clinicLableReltion.LableName + ',');
                    }
                    #region 增加科室与标签关系 
                    RelationLabel relationLabel = new RelationLabel();
                    relationLabel.RelationCode = "Clinic";
                    relationLabel.BusinessID = ClinicLableInfo.ClinicID;
                    relationLabel.BusinessName = ClinicName;
                    relationLabel.LableID = ClinicLableInfo.LableIDJSON;
                    relationLabel.LableName = LableNameList.ToString(); ;
                    var Lableresul = await _relationLabelListService.CreateRelationLabe(relationLabel);
                    #endregion
                }
                #endregion
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = resultAll;
                #region 操作日志
                var CreateYaeherOperList = new YaeherOperList()
                {
                    OperExplain = "CreateClinicAndLable",
                    OperContent = JsonHelper.ToJson(ClinicLableInfo),
                    OperType = "CreateClinicAndLable",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
                #endregion
                return this.ObjectResultModule;
            }
            catch (Exception ex)
            {
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
                this.ObjectResultModule.Object = ex.ToString();
                return this.ObjectResultModule;
            }
        }

        /// <summary>
        /// 科室与标签关系ByClinicID
        /// </summary>
        /// <param name="ClinicLableInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicLableByClinicID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicLableByClinicID([FromBody]ClinicLableIn ClinicLableInfo)
        {
            if (!Commons.CheckSecret(ClinicLableInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicLableReltionIn clinicLableReltionIn = new ClinicLableReltionIn();
            clinicLableReltionIn.AndAlso(a => a.IsDelete == false);
            clinicLableReltionIn.AndAlso(a => a.ClinicID == ClinicLableInfo.ClinicID);
            var clinicLableList = await _clinicLableReltionService.ClinicDoctorReltionList(clinicLableReltionIn);
            if (clinicLableList.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                ClinicLableIn clinicLable = new ClinicLableIn();
                if (clinicLableList.Count > 0)
                {
                    foreach (var clinicLableInfo in clinicLableList)
                    {
                        clinicLable.Id = clinicLableInfo.Id;
                        clinicLable.ClinicID = clinicLableInfo.ClinicID;
                        clinicLable.ClinicName = clinicLableInfo.ClinicName;
                        clinicLable.ClinicJSON = clinicLableInfo.ClinicJSON;
                        clinicLable.LableIDJSON += clinicLableInfo.LableID + ",";
                    }
                }
                this.ObjectResultModule.Object = clinicLable;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ClinicLableByClinicID",
                OperContent = JsonHelper.ToJson(ClinicLableInfo),
                OperType = "ClinicLableByClinicID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        #endregion

        #region 科室与医生关系
        /// <summary>
        /// 科室与医生关系新增
        /// </summary>
        /// <param name="ClinicInfo"></param>
        /// <returns></returns>
        [Route("api/CreateClinicAndDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateClinicAndDoctor([FromBody] ClinicDoctorIn ClinicInfo)
        {
            if (!Commons.CheckSecret(ClinicInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            #region 先删除
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            clinicDoctorReltionIn.AndAlso(a => a.ClinicID == ClinicInfo.ClinicID);
            var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            if (clinicDoctorList.Count > 0)
            {
                foreach (var clinicDoctorInfo in clinicDoctorList)
                {
                    clinicDoctorInfo.DeleteBy = userid;
                    clinicDoctorInfo.DeleteTime = DateTime.Now;
                    clinicDoctorInfo.IsDelete = true;
                    var result = await _clinicDoctorReltionService.DeleteClinicDoctorReltion(clinicDoctorInfo);
                }
            }
            #endregion
            #region 再新增
            string[] DoctorIDArray = null;
            if (!string.IsNullOrEmpty(ClinicInfo.DoctorIDJSON))
            {
                DoctorIDArray = ClinicInfo.DoctorIDJSON.Split(',');
            }
            YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
            yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
            var DoctorList = await _YaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);
            var ClinicInfos = await _clinicInfomationService.ClinicInfomationByID(ClinicInfo.ClinicID);
            var resultAll = 0;
            if (DoctorIDArray.Length > 0)
            {
                for (int a = 0; a < DoctorIDArray.Length; a++)
                {
                    ClinicDoctorReltion clinicDoctorReltion = new ClinicDoctorReltion();
                    clinicDoctorReltion.ClinicID = ClinicInfos.Id;
                    clinicDoctorReltion.ClinicName = ClinicInfos.ClinicName;
                    clinicDoctorReltion.ClinicJSON = JsonHelper.ToJson(ClinicInfos);
                    var DoctorListInfo = DoctorList.Where(t => t.Id == int.Parse(DoctorIDArray[a].ToString())).FirstOrDefault();
                    if (DoctorListInfo != null)
                    {
                        clinicDoctorReltion.DoctorID = DoctorListInfo.Id;
                        clinicDoctorReltion.DoctorName = DoctorListInfo.DoctorName;
                        clinicDoctorReltion.DoctorJSON = JsonHelper.ToJson(DoctorListInfo);
                        clinicDoctorReltion.CreatedBy = userid;
                        clinicDoctorReltion.CreatedOn = DateTime.Now;
                        var res = await _clinicDoctorReltionService.CreateClinicDoctorReltion(clinicDoctorReltion);
                        resultAll = +res.Id;
                    }
                }
            }
            #endregion
            if (resultAll > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = resultAll;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 科室与医生关系ByClinicID
        /// </summary>
        /// <param name="ClinicInfo"></param>
        /// <returns></returns>
        [Route("api/ClinicDoctorByClinicID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ClinicDoctorByClinicID([FromBody]ClinicDoctorIn ClinicInfo)
        {
            if (!Commons.CheckSecret(ClinicInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            clinicDoctorReltionIn.AndAlso(a => a.ClinicID == ClinicInfo.ClinicID);
            var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            if (clinicDoctorList.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                ClinicDoctorIn ClinicDoctor = new ClinicDoctorIn();
                if (clinicDoctorList.Count > 0)
                {
                    foreach (var ClinicDoctorInfo in clinicDoctorList)
                    {
                        ClinicDoctor.Id = ClinicDoctorInfo.Id;
                        ClinicDoctor.ClinicID = ClinicDoctorInfo.ClinicID;
                        ClinicDoctor.ClinicName = ClinicDoctorInfo.ClinicName;
                        ClinicDoctor.ClinicJSON = ClinicDoctorInfo.ClinicJSON;
                        ClinicDoctor.DoctorIDJSON += ClinicDoctorInfo.DoctorID + ",";
                    }
                }
                this.ObjectResultModule.Object = ClinicDoctor;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            return this.ObjectResultModule;
        }
        #endregion
    }
}