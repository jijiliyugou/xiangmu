using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.DoctorRule;
using Yaeher.DoctorRule.Dto;
using Yaeher.Extensions;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.YaeherDoctors;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 医生准则
    /// </summary>
    public class DoctorRulesController : YaeherAppServiceBase
    {
        private readonly IDoctorRulesService _doctorRulesService;
        private readonly IClinicDoctorReltionService _clinicDoctorReltionService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IAbpSession _IabpSession;
        private readonly IUserManagerService _userManagerService;
        private readonly IClinicInfomationService _clinicInfomationService;
        private readonly IYaeherOperListService _yaeherOperListService;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doctorRulesService"></param>
        /// <param name="session"></param>
        /// <param name="clinicDoctorReltionService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="clinicInfomationService"></param>
        /// <param name="yaeherOperListService"></param>
        public DoctorRulesController(IDoctorRulesService doctorRulesService,
                                     IAbpSession session,
                                     IClinicDoctorReltionService clinicDoctorReltionService,
                                     IYaeherDoctorService yaeherDoctorService,
                                     IUserManagerService userManagerService,
                                     IClinicInfomationService clinicInfomationService,
                                     IYaeherOperListService yaeherOperListService)
        {
            _doctorRulesService = doctorRulesService;
            _clinicDoctorReltionService = clinicDoctorReltionService;
            _yaeherDoctorService = yaeherDoctorService;
            _IabpSession = session;
            _userManagerService = userManagerService;
            _clinicInfomationService = clinicInfomationService;
            _yaeherOperListService = yaeherOperListService;
        }

        #region 医生准则
        /// <summary>
        /// 医生准则 新增
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorRules")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorRules([FromBody] DoctorRules DoctorRulesInfo)
        {
            if (!Commons.CheckSecret(DoctorRulesInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            //ApplyClinicID 前段按照1,2,3,4格式返回后端

            string[] IDArray = null; int[] iNums = null;
            if (!string.IsNullOrEmpty(DoctorRulesInfo.ApplyClinicID))
            {
                IDArray = DoctorRulesInfo.ApplyClinicID.TrimEnd(',').Split(',');
            }
            if (IDArray.Length < 1) { return new ObjectResultModule("", 400, "医生制度必须选择科室！"); }
            //转换方法
            iNums = Array.ConvertAll<string, int>(IDArray, s => int.Parse(s));

            var clin = new ClinicInfomationIn(); clin.AndAlso(t => !t.IsDelete);
            var clinicinfo = await _clinicInfomationService.ClinicInfomationListByArrId(clin, iNums.ToList());
            string clinicname = "";
            foreach (var item in clinicinfo)
            {
                string ClinicType = item.ClinicType == 1 ? "(成人)" : "(儿童)";
                clinicname += item.ClinicName + ClinicType+",";
            }
            var CreateDoctorRules = new DoctorRules()
            {
                RulesType = DoctorRulesInfo.RulesType,
                RulesTitle = DoctorRulesInfo.RulesTitle,
                RulesContent = DoctorRulesInfo.RulesContent,
                ApplyClinicName = clinicname.TrimEnd(','),
                ImageFie= DoctorRulesInfo.ImageFie,
                ApplyClinicID =","+ DoctorRulesInfo.ApplyClinicID.TrimEnd(',')+",",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _doctorRulesService.CreateDoctorRules(CreateDoctorRules);
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
                OperExplain = "CreateDoctorRules",
                OperContent = JsonHelper.ToJson(DoctorRulesInfo),
                OperType = "CreateDoctorRules",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生准则 修改
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorRules")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorRules([FromBody] DoctorRules DoctorRulesInfo)
        {
            if (!Commons.CheckSecret(DoctorRulesInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateDoctorRules = await _doctorRulesService.DoctorRulesByID(DoctorRulesInfo.Id);

            //ApplyClinicID 前段按照1,2,3,4格式返回后端

            string[] IDArray = null; int[] iNums = null;
            if (!string.IsNullOrEmpty(DoctorRulesInfo.ApplyClinicID))
            {
                IDArray = DoctorRulesInfo.ApplyClinicID.TrimEnd(',').Split(',');
            }
            if (IDArray.Length < 1) { return new ObjectResultModule("", 400, "医生制度必须选择科室！"); }
            //转换方法
            iNums = Array.ConvertAll<string, int>(IDArray, s => int.Parse(s));
            var clin = new ClinicInfomationIn(); clin.AndAlso(t => !t.IsDelete);
            var clinicinfo = await _clinicInfomationService.ClinicInfomationListByArrId(clin, iNums.ToList());
            string clinicname = "";
            foreach (var item in clinicinfo)
            {
                string ClinicType = item.ClinicType == 1 ? "(成人)" : "(儿童)";
                clinicname += item.ClinicName + ClinicType + ",";
            }

            if (UpdateDoctorRules != null)
            {
                if (!string.IsNullOrEmpty(DoctorRulesInfo.RulesType))
                {
                    UpdateDoctorRules.RulesType = DoctorRulesInfo.RulesType;
                }
                if (!string.IsNullOrEmpty(DoctorRulesInfo.RulesTitle))
                {
                    UpdateDoctorRules.RulesTitle = DoctorRulesInfo.RulesTitle;
                }
                if (!string.IsNullOrEmpty(DoctorRulesInfo.RulesContent))
                {
                    UpdateDoctorRules.RulesContent = DoctorRulesInfo.RulesContent;
                }
                if (!string.IsNullOrEmpty(DoctorRulesInfo.ImageFie))
                {
                    UpdateDoctorRules.ImageFie = DoctorRulesInfo.ImageFie;
                }
                UpdateDoctorRules.ApplyClinicName = clinicname.TrimEnd(',');
                UpdateDoctorRules.ApplyClinicID = ","+DoctorRulesInfo.ApplyClinicID.TrimEnd(',')+",";
                UpdateDoctorRules.CreatedBy = DoctorRulesInfo.CreatedBy;
                UpdateDoctorRules.CreatedOn = DoctorRulesInfo.CreatedOn;
                UpdateDoctorRules.ModifyOn = DateTime.Now;
                UpdateDoctorRules.ModifyBy = userid;

                var result = await _doctorRulesService.UpdateDoctorRules(UpdateDoctorRules);

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
                OperExplain = "UpdateDoctorRules",
                OperContent = JsonHelper.ToJson(DoctorRulesInfo),
                OperType = "UpdateDoctorRules",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生准则 删除
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorRules")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorRules([FromBody] DoctorRules DoctorRulesInfo)
        {
            if (!Commons.CheckSecret(DoctorRulesInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorRulesService.DoctorRulesByID(DoctorRulesInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _doctorRulesService.DeleteDoctorRules(query);

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
                OperExplain = "DeleteDoctorRules",
                OperContent = JsonHelper.ToJson(DoctorRulesInfo),
                OperType = "DeleteDoctorRules",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生准则 Page
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorRulesPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRulesPage([FromBody]DoctorRulesIn DoctorRulesInfo)
        {
            if (!Commons.CheckSecret(DoctorRulesInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            //var usermanager = _userManagerService.UserManager(userid);

            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorRulesInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorRulesInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorRulesInfo.EndTime))
                {
                    DoctorRulesInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorRulesInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorRulesInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorRulesInfo.StartTime))
            {
                DoctorRulesInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorRulesInfo.KeyWord))
            {
                DoctorRulesInfo.AndAlso(t => t.RulesTitle.Contains(DoctorRulesInfo.KeyWord) ||
                                                     t.RulesContent.Contains(DoctorRulesInfo.KeyWord));
            }
            DoctorRulesInfo.AndAlso(t => t.IsDelete == false);
            if (!string.IsNullOrEmpty(DoctorRulesInfo.RulesType))
            {
                DoctorRulesInfo.AndAlso(t => t.RulesType == DoctorRulesInfo.RulesType);
            }
            if ((usermanager.MobileRoleName == "doctor" || usermanager.IsDoctor) && !usermanager.IsAdmin)
            {
                if (DoctorRulesInfo.Platform=="Mobile")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    usermanager.DoctorID = doctor.Id;
                }
                var rel = new ClinicDoctorReltionIn();
                rel.AndAlso(t => !t.IsDelete && t.DoctorID == usermanager.DoctorID);
                var clinicrel = await _clinicDoctorReltionService.ClinicDoctorReltionList(rel);
                List<DoctorRules> Result = new List<DoctorRules>();
                foreach (var item in clinicrel)
                {

                    DoctorRulesInfo.IdCheck= "," + item.ClinicID + ",";
                    var value = await _doctorRulesService.DoctorRulesList(DoctorRulesInfo);
                    Result= Result.Union(value).ToList<DoctorRules>();
                }
                //获取总数 
                var tasksCount = Result.Count();
                //获取总数
                var totalpage = tasksCount / DoctorRulesInfo.MaxResultCount;
                var DoctorRulesList =  Result.OrderByDescending(t => t.CreatedOn).Skip(DoctorRulesInfo.SkipTotal).Take(DoctorRulesInfo.MaxResultCount).ToList();
                var values= new PagedResultDto<DoctorRules>(tasksCount, DoctorRulesList.MapTo<List<DoctorRules>>());
                
                this.ObjectResultModule.Object = new DoctorRulesOut(values, DoctorRulesInfo);
            }
            else
            {
                var values = await _doctorRulesService.DoctorRulesPage(DoctorRulesInfo);
                this.ObjectResultModule.Object = new DoctorRulesOut(values, DoctorRulesInfo);
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorRulesPage",
                OperContent = JsonHelper.ToJson(DoctorRulesInfo),
                OperType = "DoctorRulesPage",
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
        /// 医生准则 List 
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorRulesList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRulesList([FromBody]DoctorRulesIn DoctorRulesInfo)
        {
            if (!Commons.CheckSecret(DoctorRulesInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorRulesInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorRulesInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorRulesInfo.EndTime))
                {
                    DoctorRulesInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorRulesInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorRulesInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorRulesInfo.StartTime))
            {
                DoctorRulesInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorRulesInfo.KeyWord))
            {
                DoctorRulesInfo.AndAlso(t => t.RulesTitle.Contains(DoctorRulesInfo.KeyWord) ||
                                                     t.RulesContent.Contains(DoctorRulesInfo.KeyWord));
            }
            DoctorRulesInfo.AndAlso(t => t.IsDelete == false);
            var values = await _doctorRulesService.DoctorRulesList(DoctorRulesInfo);
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
                OperExplain = "DoctorRulesList",
                OperContent = JsonHelper.ToJson(DoctorRulesInfo),
                OperType = "DoctorRulesList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 医生准则 Byid
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorRulesById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRulesById([FromBody]DoctorRulesIn DoctorRulesInfo)
        {
            if (!Commons.CheckSecret(DoctorRulesInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _doctorRulesService.DoctorRulesByID(DoctorRulesInfo.Id);
            var outvalue = new DoctorRulesDetailView(values);
            if (outvalue == null)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = outvalue;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorRulesById",
                OperContent = JsonHelper.ToJson(DoctorRulesInfo),
                OperType = "DoctorRulesById",
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