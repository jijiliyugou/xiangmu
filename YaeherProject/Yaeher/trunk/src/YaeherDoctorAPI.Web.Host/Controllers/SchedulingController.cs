using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Extensions;
using Yaeher.Scheduling;
using Yaeher.Scheduling.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;

namespace YaeherDoctorAPI.Web.Host.Controllers
{

    /// <summary>
    /// 医生排班管理API
    /// </summary>
    public class SchedulingController : YaeherAppServiceBase
    {
        private readonly IDoctorSchedulingService _DoctorSchedulingService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherDoctorService _YaeherDoctorService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IUserManagerService _userManagerService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="doctorSchedulingService"></param>
        /// <param name="session"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="userManagerService"></param>
        public SchedulingController(IDoctorSchedulingService doctorSchedulingService, 
                                    IAbpSession session, 
                                    ISystemParameterService systemParameterService,
                                    IYaeherDoctorService yaeherDoctorService,
                                    IYaeherOperListService yaeherOperListService,
                                    IUserManagerService userManagerService)
        {
            _DoctorSchedulingService = doctorSchedulingService;
            _systemParameterService = systemParameterService;
            _YaeherDoctorService = yaeherDoctorService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
            _userManagerService = userManagerService;
        }

        /// <summary>
        /// 获取门诊排版类型
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        [Route("api/DoctorSchedulingType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorSchedulingType([FromBody] SecretModel secret)
        {
            if (!Commons.CheckSecret(secret.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingTime");
            var paramlist = await _systemParameterService.ParameterList(param);
            var timelist = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                timelist.Add(newcode);
            }
            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingType");
            paramlist = await _systemParameterService.ParameterList(param);
            var typelist = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                typelist.Add(newcode);
            }
            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingState");
            paramlist = await _systemParameterService.ParameterList(param);
            var statelist = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                statelist.Add(newcode);
            }
            this.ObjectResultModule.Object = new DoctorSchedulingType(timelist, typelist, statelist);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorSchedulingType",
                OperContent = JsonHelper.ToJson(secret),
                OperType = "DoctorSchedulingType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 新增医生排班
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorScheduling")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorScheduling([FromBody] DoctorSchedulingInAdd input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            if (doctor == null)
            {
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "该角色不是医生账号，不可排班！";
                this.ObjectResultModule.Object = "";
            }
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingTime");
            var paramlist = await _systemParameterService.ParameterList(param);
            var timelist = new List<CodeList>();
            foreach (var item in input.SchedulingTimeList)
            {
                var codelist = paramlist.Find(t => t.Code == item.Code);
                var newcode = new CodeList() { Code = codelist.Code, Value = codelist.Name, Type = codelist.SystemType, TypeCode = codelist.SystemCode };
                timelist.Add(newcode);
            }
            
            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingType");
            var typelist = await _systemParameterService.ParameterList(param);
            var type = typelist.Find(t => t.Code == input.Duplication);
            var typecode = new CodeList() { Code = type.Code, Value = type.Name, Type = type.SystemType, TypeCode = type.SystemCode };
            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingState");
            var statelist = await _systemParameterService.ParameterList(param);
            var state = statelist.Find(t => t.Code == input.ClinicType);
            var statecode = new CodeList() { Code = state.Code, Value = state.Name, Type = state.SystemType, TypeCode = state.SystemCode };

            var create = new DoctorScheduling()
            {
                DoctorName = doctor.DoctorName,
                DoctorID = doctor.Id,
                DoctorJSON = JsonHelper.ToJson(doctor),
                SchedulingDate = input.SchedulingDate,
                SchedulingTime = JsonHelper.ToJson(timelist),
                Duplication = JsonHelper.ToJson(typecode),
                ClinicType = JsonHelper.ToJson(statecode),
                ClinicIDAdd = input.ClinicIDAdd,
                RegistrationFee = input.RegistrationFee,
                ServiceState = input.ServiceState,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _DoctorSchedulingService.CreateDoctorScheduling(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
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
                OperExplain = "CreateDoctorScheduling",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateDoctorScheduling",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生排班Page
        /// </summary>
        /// <param name="DoctorSchedulingInPage"> DoctorSchedulingInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorSchedulingPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorSchedulingPage([FromBody]DoctorSchedulingIn DoctorSchedulingInPage)
        {
            if (!Commons.CheckSecret(DoctorSchedulingInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if((!usermanager.IsAdmin&& usermanager.IsDoctor)|| usermanager.MobileRoleName== "doctor")
            {
                DoctorSchedulingInPage.AndAlso(t =>t.CreatedBy==userid);
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorSchedulingInPage.StartTime))
            {
                StartTime = DateTime.Parse(DoctorSchedulingInPage.StartTime);
                if (string.IsNullOrEmpty(DoctorSchedulingInPage.EndTime))
                {
                    DoctorSchedulingInPage.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorSchedulingInPage.EndTime))
            {
                EndTime = DateTime.Parse(DoctorSchedulingInPage.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorSchedulingInPage.StartTime))
            {
                DoctorSchedulingInPage.AndAlso(a => a.CreatedOn >= StartTime );
                DoctorSchedulingInPage.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            DoctorSchedulingInPage.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorSchedulingInPage.KeyWord))
            {
                DoctorSchedulingInPage.AndAlso(t => t.ClinicType.Contains(DoctorSchedulingInPage.KeyWord)||
                                                    t.ClinicIDAdd.Contains(DoctorSchedulingInPage.KeyWord) ||
                                                    t.Duplication.Contains(DoctorSchedulingInPage.KeyWord) ||
                                                    t.SchedulingTime.Contains(DoctorSchedulingInPage.KeyWord));
            }
            var values = await _DoctorSchedulingService.DoctorSchedulingPage(DoctorSchedulingInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorSchedulingOut(values, DoctorSchedulingInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorSchedulingPage",
                OperContent = JsonHelper.ToJson(DoctorSchedulingInPage),
                OperType = "DoctorSchedulingPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生排班List 
        /// </summary>
        /// <param name="DoctorSchedulingInList"> DoctorSchedulingInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorSchedulingList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorSchedulingList([FromBody]DoctorSchedulingIn DoctorSchedulingInList)
        {
            if (!Commons.CheckSecret(DoctorSchedulingInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if ((!usermanager.IsAdmin && usermanager.IsDoctor)||usermanager.MobileRoleName=="doctor")
            {
                DoctorSchedulingInList.AndAlso(t => t.CreatedBy == userid);
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorSchedulingInList.StartTime))
            {
                StartTime = DateTime.Parse(DoctorSchedulingInList.StartTime);
                if (string.IsNullOrEmpty(DoctorSchedulingInList.EndTime))
                {
                    DoctorSchedulingInList.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorSchedulingInList.EndTime))
            {
                EndTime = DateTime.Parse(DoctorSchedulingInList.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorSchedulingInList.StartTime))
            {
                DoctorSchedulingInList.AndAlso(a => a.CreatedOn >= StartTime);
                DoctorSchedulingInList.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            DoctorSchedulingInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorSchedulingInList.KeyWord))
            {
                DoctorSchedulingInList.AndAlso(t => t.ClinicType.Contains(DoctorSchedulingInList.KeyWord) ||
                                                    t.ClinicIDAdd.Contains(DoctorSchedulingInList.KeyWord) ||
                                                    t.Duplication.Contains(DoctorSchedulingInList.KeyWord) ||
                                                    t.SchedulingTime.Contains(DoctorSchedulingInList.KeyWord));
            }
            var value = await _DoctorSchedulingService.DoctorSchedulingList(DoctorSchedulingInList);
            if (value.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = value;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorSchedulingList",
                OperContent = JsonHelper.ToJson(DoctorSchedulingInList),
                OperType = "DoctorSchedulingList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 医生提供服务费用表ById
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DoctorSchedulingByID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorSchedulingByID([FromBody] DoctorSchedulingIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorSchedulingService.DoctorSchedulingByID(input.Id);
            if (query != null)
            {
                this.ObjectResultModule.Object = new DoctorSchedulingOutDetail(query);
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
                OperExplain = "DoctorSchedulingByID",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DoctorSchedulingByID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 更新医生排班
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorScheduling")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorScheduling([FromBody] DoctorSchedulingInAdd input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorSchedulingService.DoctorSchedulingByID(input.Id);

            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingTime");
            var paramlist = await _systemParameterService.ParameterList(param);
            var timelist = new List<CodeList>();
            foreach (var item in input.SchedulingTimeList)
            {
                var codelist = paramlist.Find(t => t.Code == item.Code);
                var newcode = new CodeList() { Code = codelist.Code, Value = codelist.Name, Type = codelist.SystemType, TypeCode = codelist.SystemCode };
                timelist.Add(newcode);
            }
            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingType");
            var typelist = await _systemParameterService.ParameterList(param);
            var type = typelist.Find(t => t.Code == input.Duplication);
            var typecode = new CodeList() { Code = type.Code, Value = type.Name, Type = type.SystemType, TypeCode = type.SystemCode };

            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorSchedulingState");
            var statelist = await _systemParameterService.ParameterList(param);
            var state = statelist.Find(t => t.Code == input.ClinicType);
            var statecode = new CodeList() { Code = state.Code, Value = state.Name, Type = state.SystemType, TypeCode = state.SystemCode };

            if (query != null&&query.CreatedBy==userid)
            {
                query.SchedulingDate = input.SchedulingDate;
                query.SchedulingTime = JsonHelper.ToJson(timelist);
                query.Duplication = JsonHelper.ToJson(typecode);
                query.ClinicType = JsonHelper.ToJson(statecode);
                query.ClinicIDAdd = input.ClinicIDAdd;
                query.RegistrationFee = input.RegistrationFee;
                query.ServiceState = input.ServiceState;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _DoctorSchedulingService.UpdateDoctorScheduling(query);

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
                OperExplain = "UpdateDoctorScheduling",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorScheduling",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除医生排班
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorScheduling")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorScheduling([FromBody] DoctorScheduling input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorSchedulingService.DoctorSchedulingByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _DoctorSchedulingService.DeleteDoctorScheduling(query);

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
                OperExplain = "DeleteDoctorScheduling",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteDoctorScheduling",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
    }
}
