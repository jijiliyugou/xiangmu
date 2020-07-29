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
using Yaeher.Consultation;
using Yaeher.Doctor;
using Yaeher.DoctorQuality;
using Yaeher.DoctorQuality.Dto;
using Yaeher.Extensions;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherDoctorAPI.Web.Host.Controllers
{

    /// <summary>
    /// 病例夹API
    /// </summary>
    public class CollectConsultationController : YaeherAppServiceBase
    {
        private readonly IQualityControlManageService _QualityControlManageService;
        private readonly ICollectConsultationService _collectConsultationService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IConsultationService _consultationService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IAbpSession _IabpSession;
        private readonly IUserManagerService _userManagerService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qualityControlManageService"></param>
        /// <param name="session"></param>
        /// <param name="collectConsultationService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="consultationService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="userManagerService"></param>
        public CollectConsultationController(IQualityControlManageService qualityControlManageService,
                                             IAbpSession session, ICollectConsultationService collectConsultationService,
                                             IYaeherDoctorService yaeherDoctorService,
                                             IConsultationService consultationService,
                                             ISystemParameterService systemParameterService,
                                             IYaeherOperListService yaeherOperListService,
                                             IUserManagerService userManagerService)
        {
            _QualityControlManageService = qualityControlManageService;
            _collectConsultationService = collectConsultationService;
            _yaeherDoctorService = yaeherDoctorService;
            _consultationService = consultationService;
            _systemParameterService = systemParameterService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
            _userManagerService = userManagerService;
        }
        /// <summary>
        /// 收藏/取消收藏咨询 病例夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CollectConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CollectConsultation([FromBody] CollectConsultation input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
            if (doctor == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var consolutation = _consultationService.YaeherConsultationByID(input.ConsultID);
            if (consolutation == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var collect = await this._collectConsultationService.CollectConsultationByExpression(t => t.CreatedBy == userid && t.ConsultID == input.ConsultID);
            if (collect != null)
            {
                if (!collect.IsDelete)
                {
                    collect.IsDelete = true;
                    collect.DeleteBy = userid;
                    collect.DeleteTime = DateTime.Now;
                    var res = await _collectConsultationService.UpdateCollectConsultation(collect);
                    this.ObjectResultModule.Message = "uncollect success";
                }
                else
                {
                    collect.IsDelete = false;
                    collect.DeleteBy = 0;
                    var res = await _collectConsultationService.UpdateCollectConsultation(collect);
                    this.ObjectResultModule.Message = "collect success";
                }

                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var create = new CollectConsultation()
                {
                    DoctorID = doctor.Id,
                    ConsultID = input.ConsultID,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var res = await _collectConsultationService.CreateCollectConsultation(create);
                if (res.Id > 0)
                {
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "collect sucess";
                    this.ObjectResultModule.Object = res;
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
                OperExplain = "CollectConsultation",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CollectConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        ///病例夹Page
        /// </summary>
        /// <param name="CollectConsultationPage"> CollectConsultationPage 数据</param>
        /// <returns></returns>
        [Route("api/CollectConsultationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CollectConsultationPage([FromBody]CollectConsultationIn CollectConsultationPage)
        {
            if (!Commons.CheckSecret(CollectConsultationPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //  var User = _userManagerService.UserManager(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (CollectConsultationPage.Platform == "PC")
            {
                if ((!usermanager.IsAdmin && usermanager.IsDoctor))
                {
                    CollectConsultationPage.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
            }
            else if (CollectConsultationPage.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    CollectConsultationPage.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            CollectConsultationPage.AndAlso(t => !t.IsDelete);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(CollectConsultationPage.StartTime))
            {
                StartTime = DateTime.Parse(CollectConsultationPage.StartTime);
                if (string.IsNullOrEmpty(CollectConsultationPage.EndTime))
                {
                    CollectConsultationPage.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(CollectConsultationPage.EndTime))
            {
                EndTime = DateTime.Parse(CollectConsultationPage.EndTime);
            }
            if (!string.IsNullOrEmpty(CollectConsultationPage.StartTime))
            {
                CollectConsultationPage.AndAlso(t => t.CreatedOn >= StartTime);
                CollectConsultationPage.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            var values = await _collectConsultationService.CollectConsultationPage(CollectConsultationPage);
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
                this.ObjectResultModule.Object = new ConsultationOut(values, CollectConsultationPage, paramlist);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CollectConsultationPage",
                OperContent = JsonHelper.ToJson(CollectConsultationPage),
                OperType = "CollectConsultationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 病例夹List 
        /// </summary>
        /// <param name="CollectConsultationList"> CollectConsultationList 数据</param>
        /// <returns></returns>
        [Route("api/CollectConsultationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CollectConsultationList([FromBody] CollectConsultationIn CollectConsultationList)
        {
            if (!Commons.CheckSecret(CollectConsultationList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            CollectConsultationList.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            var values = await _collectConsultationService.CollectConsultationList(CollectConsultationList);
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
                OperExplain = "CollectConsultationList",
                OperContent = JsonHelper.ToJson(CollectConsultationList),
                OperType = "CollectConsultationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新病例夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateCollectConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateCollectConsultation([FromBody]CollectConsultation input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _collectConsultationService.CollectConsultationByID(input.Id);
            if (query != null && query.CreatedBy == userid)
            {

                query.ConsultID = input.ConsultID;

                query.DoctorID = input.DoctorID;

                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _collectConsultationService.UpdateCollectConsultation(query);

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
                OperExplain = "UpdateCollectConsultation",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateCollectConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除病例夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteCollectConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteCollectConsultation([FromBody] CollectConsultation input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _collectConsultationService.CollectConsultationByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _collectConsultationService.DeleteCollectConsultation(query);

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
                OperExplain = "DeleteCollectConsultation",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteCollectConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
    }
}
