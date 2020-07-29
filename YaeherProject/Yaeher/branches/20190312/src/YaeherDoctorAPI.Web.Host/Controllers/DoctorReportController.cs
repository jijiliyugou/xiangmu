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
using Yaeher.DoctorReport;
using Yaeher.DoctorReport.Dto;
using Yaeher.Extensions;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.YaeherDoctors;

namespace YaeherDoctorAPI.Web.Host.Controllers
{
    /// <summary>
    /// 医生报表API
    /// </summary>
    public class DoctorReportController : YaeherAppServiceBase
    {
        private readonly IDoctorIncomeService _DoctorIncomeService;
        private readonly IIncomeDetailsService _IncomeDetailsService;
        private readonly IAbpSession _IabpSession;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IYaeherDoctorService _yaeherDoctorService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="doctorIncomeService"></param>
        /// <param name="incomeDetailsService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="session"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="yaeherDoctorService"></param>
        public DoctorReportController(IDoctorIncomeService doctorIncomeService, 
                                      IIncomeDetailsService incomeDetailsService,
                                      IUserManagerService userManagerService,
                                      IAbpSession session,
                                      IYaeherOperListService yaeherOperListService,
                                      IYaeherDoctorService yaeherDoctorService)
        {
            _DoctorIncomeService = doctorIncomeService;
            _IncomeDetailsService = incomeDetailsService;
            _userManagerService = userManagerService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
            _yaeherDoctorService = yaeherDoctorService;
        }
        /// <summary>
        /// 新增我的收入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorIncome")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorIncome([FromBody] DoctorIncome input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new DoctorIncome()
            {
                DoctorName = input.DoctorName,
                DoctorID = input.DoctorID,
                IncomeTimeType = input.IncomeTimeType,
                Total = input.Total,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _DoctorIncomeService.CreateDoctorIncome(create);
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
                OperExplain = "CreateDoctorIncome",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateDoctorIncome",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的收入Page
        /// </summary>
        /// <param name="DoctorIncomeInPage"> DoctorIncomeInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorIncomePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorIncomePage([FromBody]DoctorIncomeIn DoctorIncomeInPage)
        {
            if (!Commons.CheckSecret(DoctorIncomeInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            // 判断当角色为医生角色时
            if ((!usermanager.IsAdmin && usermanager.IsDoctor)||usermanager.MobileRoleName== "doctor")
            {
                if (DoctorIncomeInPage.Platform == "PC")
                {
                    DoctorIncomeInPage.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
                else if (DoctorIncomeInPage.Platform == "Mobile")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    DoctorIncomeInPage.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DoctorIncomeInPage.DoctorName))
                {
                    DoctorIncomeInPage.AndAlso(t => t.DoctorName.Contains(DoctorIncomeInPage.DoctorName));
                }
            }
            DoctorIncomeInPage.AndAlso(t => !t.IsDelete);
            var values = await _DoctorIncomeService.DoctorIncomePage(DoctorIncomeInPage);
            this.ObjectResultModule.Object = new DoctorIncomeOut(values, DoctorIncomeInPage);
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorIncomePage",
                OperContent = JsonHelper.ToJson(DoctorIncomeInPage),
                OperType = "DoctorIncomePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的收入List 
        /// </summary>
        /// <param name="DoctorIncomeInList"> DoctorIncomeInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorIncomeList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorIncomeList([FromBody]DoctorIncomeIn DoctorIncomeInList)
        {
            if (!Commons.CheckSecret(DoctorIncomeInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorIncomeInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorIncomeInList.DoctorName))
            {
                DoctorIncomeInList.AndAlso(t => t.DoctorName.Contains(DoctorIncomeInList.DoctorName));
            }

            var value = await _DoctorIncomeService.DoctorIncomeList(DoctorIncomeInList);
            this.ObjectResultModule.Object = value;
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorIncomeList",
                OperContent = JsonHelper.ToJson(DoctorIncomeInList),
                OperType = "DoctorIncomeList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新我的收入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorIncome")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorIncome([FromBody] DoctorIncome input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorIncomeService.DoctorIncomeByID(input.Id);
            if (query != null)
            {
                query.DoctorName = input.DoctorName;
                query.DoctorID = input.DoctorID;
                query.IncomeTimeType = input.IncomeTimeType;
                query.Total = input.Total;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _DoctorIncomeService.UpdateDoctorIncome(query);

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
                OperExplain = "UpdateDoctorIncome",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorIncome",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除我的收入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorIncome")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteLeaguerInfo([FromBody] DoctorIncome input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorIncomeService.DoctorIncomeByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _DoctorIncomeService.DeleteDoctorIncome(query);

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
                OperExplain = "DeleteLeaguerInfo",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteLeaguerInfo",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 新增我的收入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateIncomeDetails")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateIncomeDetails([FromBody] IncomeDetails input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new IncomeDetails()
            {
                DoctorName = input.DoctorName,
                DoctorID = input.DoctorID,
                ConsultID = input.ConsultID,
                ConsultNumber = input.ConsultNumber,
                ConsultJSON = input.ConsultJSON,
                OrderNumber = input.OrderNumber,
                OrderCurrency = input.OrderCurrency,
                OrderMoney = input.OrderMoney,
                ProportionMoney = input.ProportionMoney,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _IncomeDetailsService.CreateIncomeDetails(create);
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
                OperExplain = "CreateIncomeDetails",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateIncomeDetails",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion


            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的收入Page
        /// </summary>
        /// <param name="DoctorIncomeInPage"> DoctorIncomeInPage 数据</param>
        /// <returns></returns>
        [Route("api/IncomeDetailsPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDetailsPage([FromBody]IncomeDetailsIn DoctorIncomeInPage)
        {
            if (!Commons.CheckSecret(DoctorIncomeInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorIncomeInPage.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorIncomeInPage.DoctorName))
            {
                DoctorIncomeInPage.AndAlso(t => t.DoctorName.Contains(DoctorIncomeInPage.DoctorName));
            }
            var values = await _IncomeDetailsService.IncomeDetailsPage(DoctorIncomeInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new IncomeDetailsOut(values, DoctorIncomeInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "IncomeDetailsPage",
                OperContent = JsonHelper.ToJson(DoctorIncomeInPage),
                OperType = "IncomeDetailsPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取我的收入List 
        /// </summary>
        /// <param name="IncomeDetailsInList"> IncomeDetailsInList 数据</param>
        /// <returns></returns>
        [Route("api/IncomeDetailsList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDetailsList([FromBody]IncomeDetailsIn IncomeDetailsInList)
        {
            if (!Commons.CheckSecret(IncomeDetailsInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            IncomeDetailsInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(IncomeDetailsInList.DoctorName))
            {
                IncomeDetailsInList.AndAlso(t => t.DoctorName.Contains(IncomeDetailsInList.DoctorName));
            }

            var values = await _IncomeDetailsService.IncomeDetailsList(IncomeDetailsInList);
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
                OperExplain = "IncomeDetailsList",
                OperContent = JsonHelper.ToJson(IncomeDetailsInList),
                OperType = "IncomeDetailsList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新我的收入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateIncomeDetails")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateIncomeDetails([FromBody] IncomeDetails input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _IncomeDetailsService.IncomeDetailsByID(input.Id);
            if (query != null)
            {
                query.DoctorName = input.DoctorName;
                query.DoctorID = input.DoctorID;
                query.ConsultID = input.ConsultID;
                query.ConsultNumber = input.ConsultNumber;
                query.ConsultJSON = input.ConsultJSON;
                query.OrderNumber = input.OrderNumber;
                query.OrderCurrency = input.OrderCurrency;
                query.OrderMoney = input.OrderMoney;
                query.ProportionMoney = input.ProportionMoney;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _IncomeDetailsService.UpdateIncomeDetails(query);

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
                OperExplain = "UpdateIncomeDetails",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateIncomeDetails",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除我的收入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteIncomeDetails")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteIncomeDetails([FromBody] IncomeDetails input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _IncomeDetailsService.IncomeDetailsByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _IncomeDetailsService.DeleteIncomeDetails(query);

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
                OperExplain = "DeleteIncomeDetails",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteIncomeDetails",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
    }
}
