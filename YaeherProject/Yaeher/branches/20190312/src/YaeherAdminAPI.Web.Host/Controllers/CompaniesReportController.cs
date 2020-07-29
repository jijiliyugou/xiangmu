using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.CompaniesReport;
using Yaeher.CompaniesReport.Dto;
using Yaeher.SystemManage;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 公司收入
    /// </summary>
    public class CompaniesReportController : YaeherAppServiceBase
    {
        private readonly ICorporateIncomeTotalService _corporateIncomeTotalService;
        private readonly ICorporateIncomeDetailsService _corporateIncomeDetailsService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherOperListService _yaeherOperListService;

       /// <summary>
       /// 
       /// </summary>
       /// <param name="corporateIncomeTotalService"></param>
       /// <param name="corporateIncomeDetailsService"></param>
       /// <param name="session"></param>
       /// <param name="yaeherOperListService"></param>
        public CompaniesReportController(ICorporateIncomeTotalService corporateIncomeTotalService,
                                         ICorporateIncomeDetailsService corporateIncomeDetailsService,
                                         IAbpSession session,
                                         IYaeherOperListService yaeherOperListService)
        {
            _corporateIncomeTotalService = corporateIncomeTotalService;
            _corporateIncomeDetailsService = corporateIncomeDetailsService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
        }

        #region 公司收入总计
        /// <summary>
        /// 公司收入总计 新增
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [Route("api/CreateCorporateIncomeTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateCorporateIncomeTotal([FromBody] CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateCorporateIncomeTotal = new CorporateIncomeTotal()
            {
                IncomeType = CorporateIncomeTotalInfo.IncomeType,
                IncomeTotal = CorporateIncomeTotalInfo.IncomeTotal,

                CreatedBy = userid,
                CreatedOn = DateTime.Now,
               
            };
            var result = await _corporateIncomeTotalService.CreateCorporateIncomeTotal(CreateCorporateIncomeTotal);
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
                OperExplain = "CreateCorporateIncomeTotal",
                OperContent = JsonHelper.ToJson(CorporateIncomeTotalInfo),
                OperType = "CreateCorporateIncomeTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 公司收入总计 修改
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateCorporateIncomeTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateCorporateIncomeTotal([FromBody] CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateCorporateIncomeTotal = await _corporateIncomeTotalService.CorporateIncomeTotalByID(CorporateIncomeTotalInfo.Id);
            if (UpdateCorporateIncomeTotal != null)
            {
                UpdateCorporateIncomeTotal.IncomeType = CorporateIncomeTotalInfo.IncomeType;
                UpdateCorporateIncomeTotal.IncomeTotal = CorporateIncomeTotalInfo.IncomeTotal;
                UpdateCorporateIncomeTotal.ModifyOn = DateTime.Now;
                UpdateCorporateIncomeTotal.ModifyBy = userid;
                var result = await _corporateIncomeTotalService.UpdateCorporateIncomeTotal(UpdateCorporateIncomeTotal);
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
                OperExplain = "UpdateCorporateIncomeTotal",
                OperContent = JsonHelper.ToJson(CorporateIncomeTotalInfo),
                OperType = "UpdateCorporateIncomeTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 公司收入总计 删除
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteCorporateIncomeTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteCorporateIncomeTotal([FromBody] CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _corporateIncomeTotalService.CorporateIncomeTotalByID(CorporateIncomeTotalInfo.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _corporateIncomeTotalService.DeleteCorporateIncomeTotal(query);

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
                OperExplain = "DeleteCorporateIncomeTotal",
                OperContent = JsonHelper.ToJson(CorporateIncomeTotalInfo),
                OperType = "DeleteCorporateIncomeTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 公司收入总计 Page
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [Route("api/CorporateIncomeTotalPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CorporateIncomeTotalPage([FromBody]CorporateIncomeTotalIn CorporateIncomeTotalInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(CorporateIncomeTotalInfo.StartTime))
            {
                StartTime = DateTime.Parse(CorporateIncomeTotalInfo.StartTime);
                if (string.IsNullOrEmpty(CorporateIncomeTotalInfo.EndTime))
                {
                    CorporateIncomeTotalInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(CorporateIncomeTotalInfo.EndTime))
            {
                EndTime = DateTime.Parse(CorporateIncomeTotalInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(CorporateIncomeTotalInfo.StartTime))
            {
                CorporateIncomeTotalInfo.AndAlso(t => t.CreatedOn >= StartTime);
                CorporateIncomeTotalInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            CorporateIncomeTotalInfo.AndAlso(t => t.IsDelete == false);
            var values = await _corporateIncomeTotalService.CorporateIncomeTotalPage(CorporateIncomeTotalInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new CorporateIncomeTotalOut(values, CorporateIncomeTotalInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CorporateIncomeTotalPage",
                OperContent = JsonHelper.ToJson(CorporateIncomeTotalInfo),
                OperType = "CorporateIncomeTotalPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 公司收入总计 List 
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [Route("api/CorporateIncomeTotalList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CorporateIncomeTotalList([FromBody]CorporateIncomeTotalIn CorporateIncomeTotalInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(CorporateIncomeTotalInfo.StartTime))
            {
                StartTime = DateTime.Parse(CorporateIncomeTotalInfo.StartTime);
                if (string.IsNullOrEmpty(CorporateIncomeTotalInfo.EndTime))
                {
                    CorporateIncomeTotalInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(CorporateIncomeTotalInfo.EndTime))
            {
                EndTime = DateTime.Parse(CorporateIncomeTotalInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(CorporateIncomeTotalInfo.StartTime))
            {
                CorporateIncomeTotalInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            CorporateIncomeTotalInfo.AndAlso(t => t.IsDelete == false);
            var values = await _corporateIncomeTotalService.CorporateIncomeTotalList(CorporateIncomeTotalInfo);
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
                OperExplain = "CorporateIncomeTotalList",
                OperContent = JsonHelper.ToJson(CorporateIncomeTotalInfo),
                OperType = "CorporateIncomeTotalList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 公司收入总计 Byid
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [Route("api/CorporateIncomeTotalById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CorporateIncomeTotalById([FromBody]CorporateIncomeTotalIn CorporateIncomeTotalInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _corporateIncomeTotalService.CorporateIncomeTotalByID(CorporateIncomeTotalInfo.Id);
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
                OperExplain = "CorporateIncomeTotalById",
                OperContent = JsonHelper.ToJson(CorporateIncomeTotalInfo),
                OperType = "CorporateIncomeTotalById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 公司收入明细
        /// <summary>
        /// 科室公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [Route("api/CreateCorporateIncomeDetails")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateCorporateIncomeDetails([FromBody] CorporateIncomeDetails CorporateIncomeDetailsInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeDetailsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateCorporateIncomeDetails = new CorporateIncomeDetails()
            {
                ConsultNumber = CorporateIncomeDetailsInfo.ConsultNumber,
                ConsultID = CorporateIncomeDetailsInfo.ConsultID,
                ConsultantID = CorporateIncomeDetailsInfo.ConsultantID,
                ConsultantName = CorporateIncomeDetailsInfo.ConsultantName,
                PatientID = CorporateIncomeDetailsInfo.PatientID,
                PatientName = CorporateIncomeDetailsInfo.PatientName,
                DoctorID = CorporateIncomeDetailsInfo.DoctorID,
                DoctorName = CorporateIncomeDetailsInfo.DoctorName,
                OrderNumber = CorporateIncomeDetailsInfo.OrderNumber,
                OrderCurrency = CorporateIncomeDetailsInfo.OrderCurrency,
                OrderMoney = CorporateIncomeDetailsInfo.OrderMoney,
                ProportionMoney = CorporateIncomeDetailsInfo.ProportionMoney,
                CreatedBy = userid,
                CreatedOn =DateTime.Now
            };
            var result = await _corporateIncomeDetailsService.CreateCorporateIncomeDetails(CreateCorporateIncomeDetails);
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
                OperExplain = "CreateCorporateIncomeDetails",
                OperContent = JsonHelper.ToJson(CorporateIncomeDetailsInfo),
                OperType = "CreateCorporateIncomeDetails",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 科室公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateCorporateIncomeDetails")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateCorporateIncomeDetails([FromBody] CorporateIncomeDetails CorporateIncomeDetailsInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeDetailsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateCorporateIncomeDetails = await _corporateIncomeDetailsService.CorporateIncomeDetailsByID(CorporateIncomeDetailsInfo.Id);
            if (UpdateCorporateIncomeDetails != null)
            {
                UpdateCorporateIncomeDetails.ConsultNumber = CorporateIncomeDetailsInfo.ConsultNumber;
                UpdateCorporateIncomeDetails.ConsultID = CorporateIncomeDetailsInfo.ConsultID;
                UpdateCorporateIncomeDetails.ConsultantID = CorporateIncomeDetailsInfo.ConsultantID;
                UpdateCorporateIncomeDetails.ConsultantName = CorporateIncomeDetailsInfo.ConsultantName;
                UpdateCorporateIncomeDetails.PatientID = CorporateIncomeDetailsInfo.PatientID;
                UpdateCorporateIncomeDetails.PatientName = CorporateIncomeDetailsInfo.PatientName;
                UpdateCorporateIncomeDetails.DoctorID = CorporateIncomeDetailsInfo.DoctorID;
                UpdateCorporateIncomeDetails.DoctorName = CorporateIncomeDetailsInfo.DoctorName;
                UpdateCorporateIncomeDetails.OrderNumber = CorporateIncomeDetailsInfo.OrderNumber;
                UpdateCorporateIncomeDetails.OrderCurrency = CorporateIncomeDetailsInfo.OrderCurrency;
                UpdateCorporateIncomeDetails.OrderMoney = CorporateIncomeDetailsInfo.OrderMoney;
                UpdateCorporateIncomeDetails.ProportionMoney = CorporateIncomeDetailsInfo.ProportionMoney;
                UpdateCorporateIncomeDetails.ModifyOn = DateTime.Now;
                UpdateCorporateIncomeDetails.ModifyBy = userid;

                var result = await _corporateIncomeDetailsService.UpdateCorporateIncomeDetails(UpdateCorporateIncomeDetails);

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
                OperExplain = "UpdateCorporateIncomeDetails",
                OperContent = JsonHelper.ToJson(CorporateIncomeDetailsInfo),
                OperType = "UpdateCorporateIncomeDetails",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 公司收入明细删除
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteCorporateIncomeDetails")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteCorporateIncomeDetails([FromBody] CorporateIncomeDetails CorporateIncomeDetailsInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeDetailsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _corporateIncomeDetailsService.CorporateIncomeDetailsByID(CorporateIncomeDetailsInfo.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _corporateIncomeDetailsService.DeleteCorporateIncomeDetails(query);

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
                OperExplain = "DeleteCorporateIncomeDetails",
                OperContent = JsonHelper.ToJson(CorporateIncomeDetailsInfo),
                OperType = "DeleteCorporateIncomeDetails",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 公司收入明细 Page
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [Route("api/CorporateIncomeDetailsPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CorporateIncomeDetailsPage([FromBody]CorporateIncomeDetailsIn CorporateIncomeDetailsInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeDetailsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.StartTime))
            {
                StartTime = DateTime.Parse(CorporateIncomeDetailsInfo.StartTime);
                if (string.IsNullOrEmpty(CorporateIncomeDetailsInfo.EndTime))
                {
                    CorporateIncomeDetailsInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.EndTime))
            {
                EndTime = DateTime.Parse(CorporateIncomeDetailsInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.StartTime))
            {
                CorporateIncomeDetailsInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.KeyWord))
            {
                CorporateIncomeDetailsInfo.AndAlso(t => t.ConsultantName.Contains(CorporateIncomeDetailsInfo.KeyWord) ||
                                                     t.ConsultNumber.Contains(CorporateIncomeDetailsInfo.KeyWord) ||
                                                     t.DoctorName.Contains(CorporateIncomeDetailsInfo.KeyWord) ||
                                                     t.PatientName.Contains(CorporateIncomeDetailsInfo.KeyWord));
            }
            CorporateIncomeDetailsInfo.AndAlso(t => t.IsDelete == false);
            var values = await _corporateIncomeDetailsService.CorporateIncomeDetailsPage(CorporateIncomeDetailsInfo);
        
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            else
            {
                this.ObjectResultModule.Object = new CorporateIncomeDetailsOut(values, CorporateIncomeDetailsInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CorporateIncomeDetailsPage",
                OperContent = JsonHelper.ToJson(CorporateIncomeDetailsInfo),
                OperType = "CorporateIncomeDetailsPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 公司收入明细 List 
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [Route("api/CorporateIncomeDetailsList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CorporateIncomeDetailsList([FromBody]CorporateIncomeDetailsIn CorporateIncomeDetailsInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeDetailsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.StartTime))
            {
                StartTime = DateTime.Parse(CorporateIncomeDetailsInfo.StartTime);
                if (string.IsNullOrEmpty(CorporateIncomeDetailsInfo.EndTime))
                {
                    CorporateIncomeDetailsInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.EndTime))
            {
                EndTime = DateTime.Parse(CorporateIncomeDetailsInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.StartTime))
            {
                CorporateIncomeDetailsInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(CorporateIncomeDetailsInfo.KeyWord))
            {
                CorporateIncomeDetailsInfo.AndAlso(t => t.ConsultantName.Contains(CorporateIncomeDetailsInfo.KeyWord) ||
                                                     t.ConsultNumber.Contains(CorporateIncomeDetailsInfo.KeyWord) ||
                                                     t.DoctorName.Contains(CorporateIncomeDetailsInfo.KeyWord) ||
                                                     t.PatientName.Contains(CorporateIncomeDetailsInfo.KeyWord));
            }
            CorporateIncomeDetailsInfo.AndAlso(t => t.IsDelete == false);
            var values = await _corporateIncomeDetailsService.CorporateIncomeDetailsList(CorporateIncomeDetailsInfo);
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
                OperExplain = "CorporateIncomeDetailsList",
                OperContent = JsonHelper.ToJson(CorporateIncomeDetailsInfo),
                OperType = "CorporateIncomeDetailsList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 公司收入明细 Byid
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [Route("api/CorporateIncomeDetailsById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CorporateIncomeDetailsById([FromBody]CorporateIncomeDetailsIn CorporateIncomeDetailsInfo)
        {
            if (!Commons.CheckSecret(CorporateIncomeDetailsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _corporateIncomeDetailsService.CorporateIncomeDetailsByID(CorporateIncomeDetailsInfo.Id);
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
                OperExplain = "CorporateIncomeDetailsById",
                OperContent = JsonHelper.ToJson(CorporateIncomeDetailsInfo),
                OperType = "CorporateIncomeDetailsById",
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