using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.CompaniesReport;
using Yaeher.CompaniesReport.Dto;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.DoctorReport;
using Yaeher.DoctorReport.Dto;
using Yaeher.Extensions;
using Yaeher.NumericalStatement;
using Yaeher.NumericalStatement.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 统计报表
    /// </summary>
    public class NumericalStatementController : YaeherAppServiceBase
    {
        private readonly IConsultationOrderTotalService _consultationOrderTotalService;
        private readonly IEvaluationTotalService _evaluationTotalService;
        private readonly IAbpSession _IabpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserManagerService _userManagerService;
        private readonly IConsultationEvaluationService _consultationEvaluationService;

        private readonly IConsultationService _consultationService;     //咨询管理
        private readonly IOrderTradeRecordService _orderTradeRecordService;     //订单信息 报表用
        private readonly IYaeherUserService _yaeherUserService;

        private readonly IClinicInfomationService _clinicInfomationService;  //科室信息
        private readonly IClinicDoctorReltionService _clinicDoctorReltionService; // 科室与医生关系

        private readonly IDoctorOnlineRecordService _doctorOnlineRecordService; // 医生上下线状态

        private readonly IYaeherDoctorService _yaeherDoctorService;  // 医生基本信息
        private readonly IDoctorRelationService _doctorRelationService;  //医生与标签关系
        private readonly IServiceMoneyListService _serviceMoneyListService; // 医生提供费用
        private readonly IRecommendedOrderingService _recommendedOrderingService; // 指定排序
        private readonly ICorporateIncomeTotalService _corporateIncomeTotalService;//管理端收入统计
        private readonly IIncomeDetailsService _incomeDetailsService;//收入明细

        private readonly IYaeherOperListService _yaeherOperListService;

        private readonly ICacheManager _cacheManager;//cach缓存
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultationOrderTotalService"></param>
        /// <param name="session"></param>
        /// <param name="evaluationTotalService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="consultationEvaluationService"></param>
        /// <param name="clinicInfomationService"></param>
        /// <param name="clinicDoctorReltionService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="serviceMoneyListService"></param>
        /// <param name="recommendedOrderingService"></param>
        /// <param name="doctorRelationService"></param>
        /// <param name="consultationService"></param>
        /// <param name="doctorOnlineRecordService"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="corporateIncomeTotalService"></param>
        /// <param name="incomeDetailsService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="cacheManager"></param>
        public NumericalStatementController(IConsultationOrderTotalService consultationOrderTotalService,
                                            IAbpSession session,
                                            IEvaluationTotalService evaluationTotalService,
                                            IUserManagerService userManagerService,
                                            IUnitOfWorkManager unitOfWorkManager,
                                            IConsultationEvaluationService consultationEvaluationService,
                                            IClinicInfomationService clinicInfomationService,
                                            IClinicDoctorReltionService clinicDoctorReltionService,
                                            IYaeherDoctorService yaeherDoctorService,
                                            IServiceMoneyListService serviceMoneyListService,
                                            IRecommendedOrderingService recommendedOrderingService,
                                            IDoctorRelationService doctorRelationService,
                                            IConsultationService consultationService,
                                            IDoctorOnlineRecordService doctorOnlineRecordService,
                                            IYaeherUserService yaeherUserService,
                                            IOrderTradeRecordService orderTradeRecordService,
                                            ICorporateIncomeTotalService corporateIncomeTotalService,
                                            IIncomeDetailsService incomeDetailsService,
                                            IYaeherOperListService yaeherOperListService,
                                            ICacheManager cacheManager)
        {
            _consultationOrderTotalService = consultationOrderTotalService;
            _evaluationTotalService = evaluationTotalService;
            _unitOfWorkManager = unitOfWorkManager;
            _IabpSession = session;
            _userManagerService = userManagerService;
            _consultationEvaluationService = consultationEvaluationService;

            _clinicInfomationService = clinicInfomationService;
            _clinicDoctorReltionService = clinicDoctorReltionService;
            _yaeherDoctorService = yaeherDoctorService;
            _serviceMoneyListService = serviceMoneyListService;
            _recommendedOrderingService = recommendedOrderingService;
            _doctorRelationService = doctorRelationService;

            _consultationService = consultationService;
            _doctorOnlineRecordService = doctorOnlineRecordService;

            _yaeherUserService = yaeherUserService;
            _orderTradeRecordService = orderTradeRecordService;
            _corporateIncomeTotalService = corporateIncomeTotalService;
            _incomeDetailsService = incomeDetailsService;
            _yaeherOperListService = yaeherOperListService;
            _cacheManager = cacheManager;
        }
        //[Route("api/secretTask")]
        //[HttpPost]
        //[AbpAllowAnonymous]
        //public async Task<ObjectResultModule> secretTask([FromBody] SecretModel secretModel)
        //{
        //    //dGltZXN0YW1wPTE1NDUyMjQ2NDUmbm9uY2U9MzlmODJmZDEtNDNhMS00ZjlhLTg2ODktM2FlZTQwYTNlZTA4JmFwcHR5cGU9c3lzdGVtJnNpZ25hdHVyZT1jM2M2NWQyMTljOTc5NjAwZmY3NTQ2NWY5Zjk2MWQ0OA==
        //    //var newsecret = await CreateSecret();
        //    var newsecret = secretModel.Secret;
        //    var checkresult = Commons.CheckSecret(newsecret);
        //    return new ObjectResultModule(newsecret, 200, checkresult.ToString());

        //}

        #region 订单汇总表
        /// <summary>
        /// 创建 订单汇总表
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [Route("api/CreateConsultationOrderTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateConsultationOrderTotal([FromBody] ConsultationOrderTotal ConsultationOrderTotalInfo)
        {
            if (!Commons.CheckSecret(ConsultationOrderTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateConsultationOrderTotal = new ConsultationOrderTotal()
                {
                    DoctorName = ConsultationOrderTotalInfo.DoctorName,
                    DoctorID = ConsultationOrderTotalInfo.DoctorID,
                    DoctorJSON = ConsultationOrderTotalInfo.DoctorJSON,
                    OrderTotal = ConsultationOrderTotalInfo.OrderTotal,
                    RevenueTotal = ConsultationOrderTotalInfo.RevenueTotal,
                    TotalType = ConsultationOrderTotalInfo.TotalType,
                    //TotalDates = ConsultationOrderTotalInfo.TotalDates,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                var result = await _consultationOrderTotalService.CreateConsultationOrderTotal(CreateConsultationOrderTotal);
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
                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateConsultationOrderTotal",
                OperContent = JsonHelper.ToJson(ConsultationOrderTotalInfo),
                OperType = "CreateConsultationOrderTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 订单统计报表 Page
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationOrderTotalInPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationOrderTotalInPage([FromBody]ConsultationOrderTotalIn ConsultationOrderTotalInfo)
        {
            if (!Commons.CheckSecret(ConsultationOrderTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            // 判断当角色为医生角色时
            if (ConsultationOrderTotalInfo.Platform == "PC")
            {
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    ConsultationOrderTotalInfo.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
            }
            else if (ConsultationOrderTotalInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    ConsultationOrderTotalInfo.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationOrderTotalInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationOrderTotalInfo.EndTime))
                {
                    ConsultationOrderTotalInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationOrderTotalInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.StartTime))
            {
                ConsultationOrderTotalInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.KeyWord))
            {
                ConsultationOrderTotalInfo.AndAlso(t => t.DoctorName.Contains(ConsultationOrderTotalInfo.KeyWord));
            }
            ConsultationOrderTotalInfo.AndAlso(t => t.TotalType == ConsultationOrderTotalInfo.TotalType);
            ConsultationOrderTotalInfo.AndAlso(t => t.IsDelete == false);
            var values = await _consultationOrderTotalService.ConsultationOrderTotalPage(ConsultationOrderTotalInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            else
            {
                this.ObjectResultModule.Object = new ConsultationOrderTotalOut(values, ConsultationOrderTotalInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationOrderTotalInPage",
                OperContent = JsonHelper.ToJson(ConsultationOrderTotalInfo),
                OperType = "ConsultationOrderTotalInPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 订单统计报表 List 
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationOrderTotalList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationOrderTotalList([FromBody]ConsultationOrderTotalIn ConsultationOrderTotalInfo)
        {
            if (!Commons.CheckSecret(ConsultationOrderTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.StartTime))
            {
                StartTime = DateTime.Parse(ConsultationOrderTotalInfo.StartTime);
                if (string.IsNullOrEmpty(ConsultationOrderTotalInfo.EndTime))
                {
                    ConsultationOrderTotalInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.EndTime))
            {
                EndTime = DateTime.Parse(ConsultationOrderTotalInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.StartTime))
            {
                ConsultationOrderTotalInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ConsultationOrderTotalInfo.KeyWord))
            {
                ConsultationOrderTotalInfo.AndAlso(t => t.DoctorName.Contains(ConsultationOrderTotalInfo.KeyWord));
            }
            ConsultationOrderTotalInfo.AndAlso(t => t.IsDelete == false);
            var values = await _consultationOrderTotalService.ConsultationOrderTotalList(ConsultationOrderTotalInfo);
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
                OperExplain = "ConsultationOrderTotalList",
                OperContent = JsonHelper.ToJson(ConsultationOrderTotalInfo),
                OperType = "ConsultationOrderTotalList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        #endregion

        #region 评分汇总表
        /// <summary>
        /// 创建 评分汇总表
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [Route("api/CreateEvaluationTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateEvaluationTotal([FromBody] EvaluationTotal EvaluationTotalInfo)
        {
            if (!Commons.CheckSecret(EvaluationTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateEvaluationTotal = new EvaluationTotal()
                {
                    DoctorName = EvaluationTotalInfo.DoctorName,
                    DoctorID = EvaluationTotalInfo.DoctorID,
                    DoctorJSON = EvaluationTotalInfo.DoctorJSON,
                    EvaluateTotal = EvaluationTotalInfo.EvaluateTotal,
                    FiveStar = EvaluationTotalInfo.FiveStar,
                    FourStar = EvaluationTotalInfo.FourStar,
                    ThreeStar = EvaluationTotalInfo.ThreeStar,
                    TwoStar = EvaluationTotalInfo.TwoStar,
                    OneStar = EvaluationTotalInfo.OneStar,
                    AverageEvaluate = EvaluationTotalInfo.AverageEvaluate,
                    OrderTotal = EvaluationTotalInfo.OrderTotal,
                    AverageAnswer = EvaluationTotalInfo.AverageAnswer,
                    RevenueTotal = EvaluationTotalInfo.RevenueTotal,
                    RefundTotal = EvaluationTotalInfo.RefundTotal,
                    CompleteTotal = EvaluationTotalInfo.CompleteTotal,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                var result = await _evaluationTotalService.CreateEvaluationTotal(EvaluationTotalInfo);
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
                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateEvaluationTotal",
                OperContent = JsonHelper.ToJson(EvaluationTotalInfo),
                OperType = "CreateEvaluationTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 评分汇总表 Page
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [Route("api/EvaluationTotalPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> EvaluationTotalPage([FromBody]EvaluationTotalIn EvaluationTotalInfo)
        {
            if (!Commons.CheckSecret(EvaluationTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            if (EvaluationTotalInfo.Platform == "PC")
            {
                // 判断当角色为医生角色时
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    EvaluationTotalInfo.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
            }
            else if (EvaluationTotalInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                    EvaluationTotalInfo.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            // 评价查询当天统计数据 无时间查询条件
            DateTime StartTime = DateTime.Now;
            DateTime EndTime = StartTime.AddDays(+1);

            if (!string.IsNullOrEmpty(EvaluationTotalInfo.StartTime))
            {
                EvaluationTotalInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(EvaluationTotalInfo.KeyWord))
            {
                EvaluationTotalInfo.AndAlso(t => t.DoctorName.Contains(EvaluationTotalInfo.KeyWord));
            }
            EvaluationTotalInfo.AndAlso(t => t.IsDelete == false);
            var values = await _evaluationTotalService.EvaluationTotalPage(EvaluationTotalInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            else
            {
                this.ObjectResultModule.Object = new EvaluationTotalOut(values, EvaluationTotalInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "EvaluationTotalPage",
                OperContent = JsonHelper.ToJson(EvaluationTotalInfo),
                OperType = "EvaluationTotalPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 评分汇总表 List 
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [Route("api/EvaluationTotalList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> EvaluationTotalList([FromBody]EvaluationTotalIn EvaluationTotalInfo)
        {
            if (!Commons.CheckSecret(EvaluationTotalInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(EvaluationTotalInfo.StartTime))
            {
                StartTime = DateTime.Parse(EvaluationTotalInfo.StartTime);
                if (string.IsNullOrEmpty(EvaluationTotalInfo.EndTime))
                {
                    EvaluationTotalInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(EvaluationTotalInfo.EndTime))
            {
                EndTime = DateTime.Parse(EvaluationTotalInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(EvaluationTotalInfo.StartTime))
            {
                EvaluationTotalInfo.AndAlso(t => t.CreatedOn >= StartTime);
                DateTime endtime = EndTime.AddDays(+1);
                EvaluationTotalInfo.AndAlso(t => t.CreatedOn < endtime);
            }
            if (!string.IsNullOrEmpty(EvaluationTotalInfo.KeyWord))
            {
                EvaluationTotalInfo.AndAlso(t => t.DoctorName.Contains(EvaluationTotalInfo.KeyWord));
            }
            EvaluationTotalInfo.AndAlso(t => t.IsDelete == false);
            var values = await _evaluationTotalService.EvaluationTotalList(EvaluationTotalInfo);
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
                OperExplain = "EvaluationTotalList",
                OperContent = JsonHelper.ToJson(EvaluationTotalInfo),
                OperType = "EvaluationTotalList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 质控评价统计
        /// <summary>
        ///质控评价统计
        /// </summary>
        [Route("api/QualityEvaluationTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityEvaluationTotal([FromBody]EvaluationTotalIn secret)
        {
            if (!Commons.CheckSecret(secret.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var TotalIn = new EvaluationTotalIn();
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime beforedate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime afterdate = beforedate.AddDays(+1);
            TotalIn.AndAlso(t => !t.IsDelete);
            TotalIn.AndAlso(t => t.CreatedOn > beforedate);
            TotalIn.AndAlso(t => t.CreatedOn < afterdate);

            var list = await _evaluationTotalService.EvaluationTotalList(TotalIn);
            var hasevaluation = list.Where(t => t.AverageEvaluate > 0);
            //var query = from a in list
            //             into g
            //            select new
            //            {
            //                OneStar = g.Sum(c => c.OneStar),
            //                TwoStar = g.Sum(c => c.TwoStar),
            //                ThreeStar = g.Sum(c => c.ThreeStar),
            //                FourStar = g.Sum(c => c.FourStar),
            //                FiveStar = g.Sum(c => c.FiveStar),
            //                AverageEvaluate = g.Sum(c => c.AverageEvaluate),
            //                OrderTotal = g.Sum(c => c.OrderTotal),
            //                RefundTotal = g.Sum(c => c.RefundTotal),
            //                CompleteTotal = g.Sum(c => c.CompleteTotal)
            //            };
            //var one = query.ToList();
            var listonestar = list.Sum(x => x.OneStar);
            var listTwoStar = list.Sum(x => x.TwoStar);
            var listThreeStar = list.Sum(x => x.ThreeStar);
            var listFourStar = list.Sum(x => x.FourStar);
            var listFiveStar = list.Sum(x => x.FiveStar);
            var listCompleteTotal = list.Sum(x => x.CompleteTotal);
            var listAverageEvaluate = list.Sum(x => x.AverageEvaluate);
            var listOrderTotal = list.Sum(x => x.OrderTotal);
            if (list.Count > 0)
            {
                var counttotal = new QualityEvaluationTotal();
                counttotal.OneStar = listonestar;
                counttotal.TwoStar = listTwoStar;
                counttotal.ThreeStar = listThreeStar;
                counttotal.FourStar = listFourStar;
                counttotal.FiveStar = listFiveStar;
                counttotal.EvaluationToTal = listonestar + listTwoStar + listThreeStar + listFourStar + listFiveStar;//已评价数
                counttotal.NoEvaluationToTal = listCompleteTotal - counttotal.EvaluationToTal;
                var allstar = listonestar * 1 + listTwoStar * 2 + listThreeStar * 3 + listFourStar * 4 + listFiveStar * 5;
                counttotal.AverageEvaluate = counttotal.EvaluationToTal > 0 ? Math.Round(Convert.ToDouble(allstar) / Convert.ToDouble(counttotal.EvaluationToTal), 1) : 0;
                counttotal.OrderTotal = listOrderTotal;
                this.ObjectResultModule.Object = counttotal;
            }
            else
            {
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityEvaluationTotal",
                OperContent = JsonHelper.ToJson(secret),
                OperType = "QualityEvaluationTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            this.ObjectResultModule.Message = "success";
            return this.ObjectResultModule;

        }
        /// <summary>
        ///质控评价统计列表
        /// </summary>
        [Route("api/QualityEvaluationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QualityEvaluationPage([FromBody]ConsultationEvaluationIn secret)
        {
            if (!Commons.CheckSecret(secret.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(secret.StartTime))
            {
                StartTime = DateTime.Parse(secret.StartTime);
                if (string.IsNullOrEmpty(secret.EndTime))
                {
                    secret.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(secret.EndTime))
            {
                EndTime = DateTime.Parse(secret.EndTime);
            }
            if (!string.IsNullOrEmpty(secret.StartTime))
            {
                secret.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (secret.EvaluateLevel > 0)
            {
                switch (secret.EvaluateLevel)
                {
                    case 1:
                        secret.AndAlso(t => t.EvaluateLevel >= 1).AndAlso(t => t.EvaluateLevel < 2);
                        break;
                    case 2:
                        secret.AndAlso(t => t.EvaluateLevel >= 2).AndAlso(t => t.EvaluateLevel < 3);
                        break;
                    case 3:
                        secret.AndAlso(t => t.EvaluateLevel >= 3).AndAlso(t => t.EvaluateLevel < 4);
                        break;
                    case 4:
                        secret.AndAlso(t => t.EvaluateLevel >= 4).AndAlso(t => t.EvaluateLevel < 5);
                        break;
                    case 5:
                        secret.AndAlso(t => t.EvaluateLevel == 5);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(secret.KeyWord))
            {
                secret.AndAlso(t => t.DoctorName.Contains(secret.KeyWord)
                                  || t.ConsultNumber.Contains(secret.KeyWord));
            }
            secret.AndAlso(t => !t.IsDelete);
            var values = await _consultationEvaluationService.ConsultationEvaluationPage(secret);
            this.ObjectResultModule.Object = new ConsultationEvaluationOut(values, secret);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QualityEvaluationPage",
                OperContent = JsonHelper.ToJson(secret),
                OperType = "QualityEvaluationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        #endregion
        #region 医生咨询回复内容缓存
        /// <summary>
        /// 回复内容缓存 新增，修改
        /// </summary>
        /// <param name="Consultation"></param>
        /// <returns></returns>
        [Route("api/ConsultReplyMemory")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultReplyMemory([FromBody]ConsultationReplyAdd Consultation)
        {
            // await _cacheManager.GetCache("DoctorDetailList").RemoveAsync("key");//删除
            //await _cacheManager.GetCache("DoctorDetailList").SetAsync("key","value");//新建

            if (!Commons.CheckSecret(Consultation.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            await _cacheManager.GetCache("DoctorReply").SetAsync(Consultation.ConsultNumber, Consultation.RepayIllnessDescription);//新建

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 回复内容缓存获取
        /// </summary>
        /// <param name="Consultation"></param>
        /// <returns></returns>
        [Route("api/GetReplyMemory")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> GetReplyMemory([FromBody]ConsultationReplyAdd Consultation)
        {
            // await _cacheManager.GetCache("DoctorDetailList").RemoveAsync("key");//删除
            //await _cacheManager.GetCache("DoctorDetailList").SetAsync("key","value");//新建

            if (!Commons.CheckSecret(Consultation.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var reply = await _cacheManager.GetCache("DoctorReply").GetOrDefaultAsync(Consultation.ConsultNumber);//获取
            if (reply == null)
            {
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = reply.ToString();
            }
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 回复内容缓存 删除
        /// </summary>
        /// <param name="Consultation"></param>
        /// <returns></returns>
        [Route("api/RemoveConsultReplyMemory")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RemoveConsultReplyMemory([FromBody]ConsultationReplyAdd Consultation)
        {
            // await _cacheManager.GetCache("DoctorDetailList").RemoveAsync("key");//删除
            //await _cacheManager.GetCache("DoctorDetailList").SetAsync("key","value");//新建

            if (!Commons.CheckSecret(Consultation.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            await _cacheManager.GetCache("DoctorReply").RemoveAsync(Consultation.ConsultNumber);//新建

            return this.ObjectResultModule;
        }

        #endregion
        #region 医生排序
        /// <summary>
        /// 评分汇总表 List 
        /// </summary>
        /// <param name="DockorSortInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorSortList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorSortList([FromBody]DockorSortIn DockorSortInfo)
        {
            if (!Commons.CheckSecret(DockorSortInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            #region 缓存查询
            //List<DoctorDetailList> doctorDetailList = new List<DoctorDetailList>();
            //#region 查询统计排名当天数据
            //DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            //DateTime EndTime = StartTime.AddDays(+1);

            //EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            //evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            //evaluationTotalIn.AndAlso(a => a.CreatedOn >= StartTime);
            //evaluationTotalIn.AndAlso(a => a.CreatedOn < EndTime);
            //var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);
            //#endregion
            //#region 查询科室信息 查询科室与医生关系
            //var clinicInfo = await _clinicInfomationService.ClinicInfomationByID(DockorSortInfo.ClinliId);
            //ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            //clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            //if (clinicInfo != null)
            //{
            //    clinicDoctorReltionIn.AndAlso(a => a.ClinicID == clinicInfo.Id);
            //}
            //var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            //#endregion
            //#region 查询医生信息
            //YaeherUserIn yaeherUserIn = new YaeherUserIn();
            //yaeherUserIn.AndAlso(a => a.IsDelete == false);
            //var YaeherUser = await _yaeherUserService.YaeherUserList(yaeherUserIn);
            //YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
            //yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
            //var DoctorInfoList = await _yaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);

            //DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            //doctorRelationIn.AndAlso(a => a.IsDelete == false);
            //var withoutlabeldoc = new List<YaeherDoctor>();//没有标签医生合计
            //var labeldoc = new List<YaeherDoctor>();//有标签医生合计
            //var DoctorList = new List<DoctorRelation>();
            //var doctorlabellist = await _doctorRelationService.DoctorRelationList(doctorRelationIn);//所有医生标签关联关系
            //if (!string.IsNullOrEmpty(DockorSortInfo.KeyWord))
            //{
            //    // doctorRelationIn.AndAlso(a => a.LableName.Contains(DockorSortInfo.KeyWord) || a.DoctorName.Contains(DockorSortInfo.KeyWord));
            //    withoutlabeldoc = DoctorInfoList.FindAll(a => a.DoctorName.Contains(DockorSortInfo.KeyWord));
            //    DoctorList = doctorlabellist.FindAll(a => a.LableName.Contains(DockorSortInfo.KeyWord) || a.DoctorName.Contains(DockorSortInfo.KeyWord));
            //}
            //else
            //{
            //    DoctorList = doctorlabellist;
            //}

            //if (!string.IsNullOrEmpty(DockorSortInfo.KeyWord))
            //{
            //    if (DoctorList.Count() > 0)
            //    {
            //        var DoctorLists = from a in DoctorList
            //                          join b in DoctorInfoList on a.DoctorID equals b.Id
            //                          select b;
            //        if (DoctorLists.Count() > 0)
            //        {
            //            DoctorInfoList = DoctorLists.Union(withoutlabeldoc).ToList();
            //        }
            //        else
            //        {
            //            DoctorInfoList = withoutlabeldoc;
            //        }
            //    }
            //    else
            //    {
            //        DoctorInfoList = withoutlabeldoc;
            //    }
            //}


            //// 医生上下线状态
            //DoctorOnlineRecordIn doctorOnlineRecordIn = new DoctorOnlineRecordIn();
            //doctorOnlineRecordIn.AndAlso(a => a.IsDelete == false && a.OnlineState == "online");
            //var DoctorOnlinList = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctorOnlineRecordIn);

            //// 医生提供服务状态
            //ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
            //serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
            //var DoctorServiceList = await _serviceMoneyListService.ServiceMoneyListList(serviceMoneyListIn);

            //if (DoctorInfoList.Count > 0)
            //{
            //    var DoctorLists = from a in DoctorInfoList
            //                      join b in DoctorOnlinList on a.Id equals b.DoctorID
            //                      join c in YaeherUser on a.UserID equals c.Id
            //                      select new DoctorDetailList
            //                      {
            //                          DoctorID = a.Id,
            //                          DoctorName = a.DoctorName,
            //                          UserID = a.UserID,
            //                          Address = a.Address,
            //                          HospitalName = a.HospitalName,
            //                          Department = a.Department,
            //                          WorkYear = a.WorkYear,
            //                          Title = a.Title,
            //                          GraduateSchool = a.GraduateSchool,
            //                          WechatNum = a.WechatNum,
            //                          PhoneNumber = a.PhoneNumber,
            //                          UserImageFile = a.UserImageFile,
            //                          Resume = a.Resume,
            //                          RegisterDate = a.CreatedOn,
            //                          OnlineState = b.OnlineState,
            //                          UserImage = c.UserImage
            //                      };
            //    if (DoctorLists.Count() > 0)
            //    {
            //        doctorDetailList = DoctorLists.ToList();
            //    }
            //}
            //if (doctorDetailList.Count() > 0)
            //{
            //    foreach (var DoctorInfo in doctorDetailList)
            //    {

            //        DoctorInfo.serviceMoneyList = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
            //        // 接单状态
            //        bool ImageState = false;   // 图文咨询关闭
            //        bool PhoneState = false;   // 电话咨询关闭
            //        DoctorInfo.ServiceState = false;   // 停止服务
            //        DoctorInfo.ReceiptState = false;   // 不可接单
            //        DoctorInfo.ImageServiceFrequency = 0;  // 默认0
            //        DoctorInfo.PhoneServiceFrequency = 0;  // 默认0
            //        var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
            //        if (ImageTextItem != null)
            //        {
            //            ImageState = ImageTextItem.ServiceState;
            //            DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
            //            if (ImageState)  // 开启
            //            {
            //                DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
            //            }
            //        }
            //        var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
            //        if (PhoneItem != null)
            //        {
            //            PhoneState = PhoneItem.ServiceState;
            //            DoctorInfo.PhoneServiceExpense = PhoneItem.ServiceExpense;
            //            if (PhoneState)  // 开启
            //            {
            //                DoctorInfo.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
            //            }
            //        }
            //        if (ImageState || PhoneState)
            //        {
            //            DoctorInfo.ServiceState = true;  //可咨询
            //        }
            //        if (ImageState)
            //        {
            //            DoctorInfo.ServiceExpense = DoctorInfo.ImageServiceExpense;
            //        }
            //        if (!ImageState && PhoneState)
            //        {
            //            DoctorInfo.ServiceExpense = DoctorInfo.PhoneServiceExpense + 9999;   // 预设值扩大电话权重排序
            //        }
            //        DoctorInfo.Doctorslable = doctorlabellist.Where(t => t.DoctorID == DoctorInfo.DoctorID).ToList();
            //        if (DoctorInfo.PhoneServiceFrequency > 0 || DoctorInfo.ImageServiceFrequency > 0)
            //        {
            //            DoctorInfo.ReceiptState = true;  // 可接单,默认不满额
            //        }
            //    }
            //}
            //#endregion
            //#region 查询当天咨询汇总
            //ConsultationIn consultationIn = new ConsultationIn();
            //consultationIn.AndAlso(a => a.IsDelete == false);
            //consultationIn.AndAlso(a => (a.CreatedOn >= StartTime));
            //consultationIn.AndAlso(a => (a.CreatedOn < EndTime));
            //consultationIn.AndAlso(a => (a.RefundNumber == null || a.RefundNumber == ""));
            //var consultationList = await _consultationService.YaeherConsultationList(consultationIn);
            //if (consultationList.Count > 0)
            //{
            //    foreach (var DoctorInfo in doctorDetailList)
            //    {
            //        DoctorInfo.ReceiptState = false;  // 满额
            //        DoctorInfo.yaeherConsultations = consultationList.Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.DoctorID).ToList();
            //        // 当天图文接单总数
            //        DoctorInfo.ImageNumberTotal = DoctorInfo.yaeherConsultations.Where(a => a.ConsultType == "ImageText").Count();
            //        // 当天电话接单总数
            //        DoctorInfo.PhoneNumberTotal = DoctorInfo.yaeherConsultations.Where(a => a.ConsultType == "Phone").Count();

            //        if ((DoctorInfo.ImageServiceFrequency - DoctorInfo.ImageNumberTotal > 0) || (DoctorInfo.PhoneServiceFrequency - DoctorInfo.PhoneNumberTotal) > 0)
            //        {
            //            DoctorInfo.ReceiptState = true;  // 可接单
            //        }
            //    }
            //}
            //#endregion 
            //// 查询 统计结果集
            //if (evaluationTotalList.Count > 0)
            //{
            //    foreach (var DoctorInfo in doctorDetailList)
            //    {
            //        var DoctorEvaluation = evaluationTotalList.Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.DoctorID).FirstOrDefault();
            //        if (DoctorEvaluation != null)
            //        {
            //            DoctorInfo.EvaluateTotal = DoctorEvaluation.EvaluateTotal;
            //            DoctorInfo.AverageEvaluate = DoctorEvaluation.AverageEvaluate;
            //            DoctorInfo.OrderTotal = DoctorEvaluation.OrderTotal;
            //            DoctorInfo.AverageAnswer = DoctorEvaluation.AverageAnswer;
            //            DoctorInfo.RevenueTotal = DoctorEvaluation.RevenueTotal;
            //            DoctorInfo.RefundTotal = DoctorEvaluation.RefundTotal;
            //            DoctorInfo.CompleteTotal = DoctorEvaluation.CompleteTotal;
            //            DoctorInfo.RefundRatio = double.Parse(Convert.ToDecimal(DoctorEvaluation.RefundTotal / DoctorEvaluation.OrderTotal).ToString("0.00"));
            //            DoctorInfo.EvaluationCount = DoctorEvaluation.OneStar + DoctorEvaluation.TwoStar + DoctorEvaluation.ThreeStar + DoctorEvaluation.FourStar + DoctorEvaluation.FiveStar;
            //        }
            //        else
            //        {
            //            DoctorInfo.EvaluationCount = 0;
            //        }
            //        DoctorInfo.SetTopSort = 999999999;  // 默认将所有医生的置顶设置最大值
            //    }
            //}
            //// 当选择科室时 查询科室内的医生信息
            //RecommendedOrderIn recommendedOrderIn = new RecommendedOrderIn();
            //recommendedOrderIn.AndAlso(a => a.IsDelete == false);
            //recommendedOrderIn.AndAlso(a => a.ClinicID == DockorSortInfo.ClinliId);
            //var Ordering = await _recommendedOrderingService.RecommendedOrderList(recommendedOrderIn);
            //// 查询置顶
            //var DoctorListss = from a in clinicDoctorList
            //                   join b in doctorDetailList on a.DoctorID equals b.DoctorID
            //                   select b;
            //doctorDetailList = DoctorListss.Distinct().ToList();
            //if (Ordering.Count > 0)
            //{
            //    foreach (var doctorDetail in doctorDetailList)
            //    {
            //        doctorDetail.SetTopSort = Ordering.Where(a => a.DoctorID == doctorDetail.DoctorID).FirstOrDefault().ItemSort;
            //    }
            //}
            //doctorDetailList = doctorDetailList.Where((x, i) => doctorDetailList.FindIndex(z => z.DoctorID == x.DoctorID) == i).ToList();
            //foreach (var item in doctorDetailList)
            //{
            //    item.Doctorslable = doctorlabellist.Where(t => t.DoctorID == item.DoctorID).ToList();
            //}
            #endregion
            string key = "GetDoctorSortListByClinicId" + DockorSortInfo.ClinliId;
            //Logger.Info("DoctorSortListkey" + key);
            var doctorDetailList = await _cacheManager.GetCache("DoctorDetailList").GetAsync<string, List<DoctorDetailList>>(key, async () => await DoctorSortMemoryCache(DockorSortInfo));
            // await _cacheManager.GetCache("DoctorDetailList").RemoveAsync("key");//删除
            //await _cacheManager.GetCache("DoctorDetailList").SetAsync("key","value");//新建

            //默认排序
            if (DockorSortInfo.SortType == "Default")
            {
                doctorDetailList = doctorDetailList.OrderBy(a => a.OnlineState)
                                                   .ThenByDescending(a => a.ReceiptState)
                                                   .ThenByDescending(a => a.ServiceState)
                                                   .ThenBy(a => a.SetTopSort)
                                                   .ThenByDescending(a => a.AverageEvaluate)
                                                   .ThenByDescending(a => a.AverageAnswer)
                                                   .ThenBy(a => a.ServiceExpense)
                                                   .ThenByDescending(a => a.RegisterDate).ToList();
            }

            // 平均分
            else if (DockorSortInfo.SortType == "Evaluation")
            {
                if (DockorSortInfo.SortDesc == "Asc")
                {
                    doctorDetailList = doctorDetailList.OrderByDescending(a => a.ServiceState).ThenByDescending(a => a.ReceiptState).ThenBy(a => a.AverageEvaluate).ThenBy(a => a.RegisterDate).ToList();
                }
                else
                {
                    doctorDetailList = doctorDetailList.OrderByDescending(a => a.ServiceState).ThenByDescending(a => a.ReceiptState).ThenByDescending(a => a.AverageEvaluate).ThenBy(a => a.RegisterDate).ToList();
                }
            }
            // 回复时长
            else if (DockorSortInfo.SortType == "AnswerTimer")
            {
                if (DockorSortInfo.SortDesc == "Asc")
                {
                    doctorDetailList = doctorDetailList.OrderByDescending(a => a.ServiceState).ThenByDescending(a => a.ReceiptState).ThenBy(a => a.AverageAnswer).ThenBy(a => a.RegisterDate).ToList();
                }
                else
                {
                    doctorDetailList = doctorDetailList.OrderByDescending(a => a.ServiceState).ThenByDescending(a => a.ReceiptState).ThenByDescending(a => a.AverageAnswer).ThenBy(a => a.RegisterDate).ToList();
                }
            }
            // 费用
            else if (DockorSortInfo.SortType == "Expense")
            {
                if (DockorSortInfo.SortDesc == "Asc")
                {
                    doctorDetailList = doctorDetailList.OrderByDescending(a => a.ServiceState).ThenByDescending(a => a.ReceiptState).ThenBy(a => a.ServiceExpense).ThenBy(a => a.RegisterDate).ToList();
                }
                else
                {
                    doctorDetailList = doctorDetailList.OrderByDescending(a => a.ServiceState).ThenByDescending(a => a.ReceiptState).ThenByDescending(a => a.ServiceExpense).ThenBy(a => a.RegisterDate).ToList();
                }
            }

            if (doctorDetailList.Count == 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            else
            {
                this.ObjectResultModule.Object = doctorDetailList;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DockorSortList",
                OperContent = JsonHelper.ToJson(DockorSortInfo),
                OperType = "DockorSortList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// DoctorSortMemoryCache
        /// </summary>
        /// <returns></returns>
        private async Task<List<DoctorDetailList>> DoctorSortMemoryCache(DockorSortIn DockorSortInfo)
        {
            //Logger.Info("DoctorSortMemoryCatch" + DockorSortInfo.ClinliId);
            List<DoctorDetailList> doctorDetailList = new List<DoctorDetailList>();
            #region 查询统计排名当天数据
            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime = StartTime.AddDays(+1);

            EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            evaluationTotalIn.AndAlso(a => a.CreatedOn >= StartTime);
            evaluationTotalIn.AndAlso(a => a.CreatedOn < EndTime);
            var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);
            #endregion
            #region 查询科室信息 查询科室与医生关系
            var clinicInfo = await _clinicInfomationService.ClinicInfomationByID(DockorSortInfo.ClinliId);
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            if (clinicInfo != null)
            {
                clinicDoctorReltionIn.AndAlso(a => a.ClinicID == clinicInfo.Id);
            }
            var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            #endregion
            #region 查询医生信息
            YaeherUserIn yaeherUserIn = new YaeherUserIn();
            yaeherUserIn.AndAlso(a => a.IsDelete == false);
            var YaeherUser = await _yaeherUserService.YaeherUserList(yaeherUserIn);
            YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
            yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
            var DoctorInfoList = await _yaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);

            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            var withoutlabeldoc = new List<YaeherDoctor>();//没有标签医生合计
            var labeldoc = new List<YaeherDoctor>();//有标签医生合计
            var DoctorList = new List<DoctorRelation>();
            var doctorlabellist = await _doctorRelationService.DoctorRelationList(doctorRelationIn);//所有医生标签关联关系
            if (!string.IsNullOrEmpty(DockorSortInfo.KeyWord))
            {
                // doctorRelationIn.AndAlso(a => a.LableName.Contains(DockorSortInfo.KeyWord) || a.DoctorName.Contains(DockorSortInfo.KeyWord));
                withoutlabeldoc = DoctorInfoList.FindAll(a => a.DoctorName.Contains(DockorSortInfo.KeyWord));
                DoctorList = doctorlabellist.FindAll(a => a.LableName.Contains(DockorSortInfo.KeyWord) || a.DoctorName.Contains(DockorSortInfo.KeyWord));
            }
            else
            {
                DoctorList = doctorlabellist;
            }

            if (!string.IsNullOrEmpty(DockorSortInfo.KeyWord))
            {
                if (DoctorList.Count() > 0)
                {
                    var DoctorLists = from a in DoctorList
                                      join b in DoctorInfoList on a.DoctorID equals b.Id
                                      select b;
                    if (DoctorLists.Count() > 0)
                    {
                        DoctorInfoList = DoctorLists.Union(withoutlabeldoc).ToList();
                    }
                    else
                    {
                        DoctorInfoList = withoutlabeldoc;
                    }
                }
                else
                {
                    DoctorInfoList = withoutlabeldoc;
                }
            }


            // 医生上下线状态
            DoctorOnlineRecordIn doctorOnlineRecordIn = new DoctorOnlineRecordIn();
            doctorOnlineRecordIn.AndAlso(a => a.IsDelete == false && a.OnlineState == "online");
            var DoctorOnlinList = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctorOnlineRecordIn);

            // 医生提供服务状态
            ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
            serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
            var DoctorServiceList = await _serviceMoneyListService.ServiceMoneyListList(serviceMoneyListIn);

            if (DoctorInfoList.Count > 0)
            {
                var DoctorLists = from a in DoctorInfoList
                                  join b in DoctorOnlinList on a.Id equals b.DoctorID
                                  join c in YaeherUser on a.UserID equals c.Id
                                  select new DoctorDetailList
                                  {
                                      DoctorID = a.Id,
                                      DoctorName = a.DoctorName,
                                      //    UserID = a.UserID,
                                      //  Address = a.Address,
                                      HospitalName = a.HospitalName,
                                      //  Department = a.Department,
                                      // WorkYear = a.WorkYear,
                                      Title = a.Title,
                                      //  GraduateSchool = a.GraduateSchool,
                                      //  WechatNum = a.WechatNum,
                                      //  PhoneNumber = a.PhoneNumber,
                                      //  UserImageFile = a.UserImageFile,
                                      // Resume = a.Resume,
                                      RegisterDate = a.CreatedOn,
                                      OnlineState = b.OnlineState,
                                      UserImage = c.UserImage
                                  };
                if (DoctorLists.Count() > 0)
                {
                    doctorDetailList = DoctorLists.ToList();
                }
            }
            if (doctorDetailList.Count() > 0)
            {
                foreach (var DoctorInfo in doctorDetailList)
                {

                    //   DoctorInfo.serviceMoneyList = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
                    // 接单状态
                    bool ImageState = false;   // 图文咨询关闭
                    bool PhoneState = false;   // 电话咨询关闭
                    DoctorInfo.ServiceState = false;   // 停止服务
                    DoctorInfo.ReceiptState = false;   // 不可接单
                    DoctorInfo.ImageServiceFrequency = 0;  // 默认0
                    DoctorInfo.PhoneServiceFrequency = 0;  // 默认0
                    var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
                    if (ImageTextItem != null)
                    {
                        ImageState = ImageTextItem.ServiceState;
                        DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
                        if (ImageState)  // 开启
                        {
                            DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
                        }
                    }
                    var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
                    if (PhoneItem != null)
                    {
                        PhoneState = PhoneItem.ServiceState;
                        DoctorInfo.PhoneServiceExpense = PhoneItem.ServiceExpense;
                        if (PhoneState)  // 开启
                        {
                            DoctorInfo.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
                        }
                    }
                    if (ImageState || PhoneState)
                    {
                        DoctorInfo.ServiceState = true;  //可咨询
                    }
                    if (ImageState)
                    {
                        DoctorInfo.ServiceExpense = DoctorInfo.ImageServiceExpense;
                    }
                    if (!ImageState && PhoneState)
                    {
                        DoctorInfo.ServiceExpense = DoctorInfo.PhoneServiceExpense + 9999;   // 预设值扩大电话权重排序
                    }
                  //  DoctorInfo.Doctorslable = doctorlabellist.Where(t => t.DoctorID == DoctorInfo.DoctorID).ToList();
                    if (DoctorInfo.PhoneServiceFrequency > 0 || DoctorInfo.ImageServiceFrequency > 0)
                    {
                        DoctorInfo.ReceiptState = true;  // 可接单,默认不满额
                    }
                }
            }
            #endregion
            #region 查询当天咨询汇总
            ConsultationIn consultationIn = new ConsultationIn();
            consultationIn.AndAlso(a => a.IsDelete == false);
            consultationIn.AndAlso(a => (a.CreatedOn >= StartTime));
            consultationIn.AndAlso(a => (a.CreatedOn < EndTime));
            consultationIn.AndAlso(a => (a.RefundNumber == null || a.RefundNumber == ""));
            var consultationList = await _consultationService.YaeherConsultationList(consultationIn);
            if (consultationList.Count > 0)
            {
                foreach (var DoctorInfo in doctorDetailList)
                {
                    DoctorInfo.ReceiptState = false;  // 满额
                    var yaeherConsultations = consultationList.Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.DoctorID).ToList();
                    // 当天图文接单总数
                    DoctorInfo.ImageNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "ImageText").Count();
                    // 当天电话接单总数
                    DoctorInfo.PhoneNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "Phone").Count();

                    if ((DoctorInfo.ImageServiceFrequency - DoctorInfo.ImageNumberTotal > 0) || (DoctorInfo.PhoneServiceFrequency - DoctorInfo.PhoneNumberTotal) > 0)
                    {
                        DoctorInfo.ReceiptState = true;  // 可接单
                    }
                }
            }
            #endregion 
            // 查询 统计结果集
            if (evaluationTotalList.Count > 0)
            {
                foreach (var DoctorInfo in doctorDetailList)
                {
                    var DoctorEvaluation = evaluationTotalList.Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.DoctorID).FirstOrDefault();
                    if (DoctorEvaluation != null)
                    {
                        DoctorInfo.EvaluateTotal = DoctorEvaluation.EvaluateTotal;
                        DoctorInfo.OrderTotal = DoctorEvaluation.OrderTotal;
                        DoctorInfo.AverageAnswer = DoctorEvaluation.AverageAnswer;
                        DoctorInfo.RevenueTotal = DoctorEvaluation.RevenueTotal;
                        DoctorInfo.RefundTotal = DoctorEvaluation.RefundTotal;
                        DoctorInfo.CompleteTotal = DoctorEvaluation.CompleteTotal;
                        DoctorInfo.RefundRatio = double.Parse(Convert.ToDecimal(DoctorEvaluation.RefundTotal / DoctorEvaluation.OrderTotal).ToString("0.00"));
                        DoctorInfo.EvaluationCount = DoctorEvaluation.OneStar + DoctorEvaluation.TwoStar + DoctorEvaluation.ThreeStar + DoctorEvaluation.FourStar + DoctorEvaluation.FiveStar;
                        if (DoctorInfo.EvaluationCount >= 15)
                        { DoctorInfo.AverageEvaluate = DoctorEvaluation.AverageEvaluate; }//星级
                    }
                    else
                    {
                        DoctorInfo.EvaluationCount = 0;
                    }
                    DoctorInfo.SetTopSort = 999999999;  // 默认将所有医生的置顶设置最大值
                }
            }
            // 当选择科室时 查询科室内的医生信息
            RecommendedOrderIn recommendedOrderIn = new RecommendedOrderIn();
            recommendedOrderIn.AndAlso(a => a.IsDelete == false);
            recommendedOrderIn.AndAlso(a => a.ClinicID == DockorSortInfo.ClinliId);
            var Ordering = await _recommendedOrderingService.RecommendedOrderList(recommendedOrderIn);
            // 查询置顶
            var DoctorListss = from a in clinicDoctorList
                               join b in doctorDetailList on a.DoctorID equals b.DoctorID
                               select b;
            doctorDetailList = DoctorListss.Distinct().ToList();
            if (Ordering.Count > 0)
            {
                foreach (var doctorDetail in doctorDetailList)
                {
                    doctorDetail.SetTopSort = Ordering.Where(a => a.DoctorID == doctorDetail.DoctorID).FirstOrDefault().ItemSort;
                }
            }
            doctorDetailList = doctorDetailList.Where((x, i) => doctorDetailList.FindIndex(z => z.DoctorID == x.DoctorID) == i).ToList();
            foreach (var item in doctorDetailList)
            {
                item.Doctorslable = doctorlabellist.Where(t => t.DoctorID == item.DoctorID).ToList();
            }
            return doctorDetailList;
        }
        #endregion

        /// <summary>
        ///管理订单统计列表
        /// </summary>
        [Route("api/AdminConsultationReport")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AdminConsultationReport([FromBody]ConsultationOrderTotalIn totalin)
        {
            if (!Commons.CheckSecret(totalin.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                StartTime = DateTime.Parse(totalin.StartTime);
                if (string.IsNullOrEmpty(totalin.EndTime))
                {
                    totalin.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(totalin.EndTime))
            {
                EndTime = DateTime.Parse(totalin.EndTime);
            }
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                totalin.AndAlso(t => t.TotalDate >= StartTime && t.TotalDate < EndTime.AddDays(+1));
            }
            totalin.AndAlso(t => !t.IsDelete);
            totalin.AndAlso(t => t.TotalType == totalin.TotalType);
            var total = await _consultationOrderTotalService.ConsultationOrderTotalList(totalin);

            var RefundTotal = total.Sum(t => t.RefundTotal);//退单数
            var OrderTotal = total.Sum(t => t.OrderTotal);//订单数
            var CompleteTotal = total.Sum(t => t.CompleteTotal);//完成数
            var NotCompleteTotal = Math.Round(OrderTotal - CompleteTotal - RefundTotal, 2);//未完成数

            var RevenueTotal = total.Sum(t => t.RevenueTotal);//订单总价格
            RevenueTotal = Math.Round(RevenueTotal, 2);
            var AverageMoney = OrderTotal == 0 ? 0 : RevenueTotal / OrderTotal;
            AverageMoney = Math.Round(AverageMoney, 2);
            this.ObjectResultModule.Object = new OrderTotalReport(RefundTotal, OrderTotal, CompleteTotal, NotCompleteTotal, RevenueTotal.ToString(), AverageMoney.ToString());
            this.ObjectResultModule.StatusCode = 200;

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AdminConsultationReport",
                OperContent = JsonHelper.ToJson(totalin),
                OperType = "AdminConsultationReport",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;

        }
        /// <summary>
        ///管理流量列表
        /// </summary>
        [Route("api/AdminFlowReport")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AdminFlowReport([FromBody]YaeherUserIn totalin)
        {
            if (!Commons.CheckSecret(totalin.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            ConsultationIn consultationInnew = new ConsultationIn();
            ConsultationIn consultationInold = new ConsultationIn();
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                StartTime = DateTime.Parse(totalin.StartTime);
                if (string.IsNullOrEmpty(totalin.EndTime))
                {
                    totalin.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(totalin.EndTime))
            {
                EndTime = DateTime.Parse(totalin.EndTime);
            }
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                totalin.AndAlso(t => t.CreatedOn >= StartTime);
                totalin.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
                consultationInnew.AndAlso(t => t.CreatedOn >= StartTime);
                consultationInnew.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
                consultationInold.AndAlso(t => t.CreatedOn < StartTime);
            }
            totalin.AndAlso(t => !t.IsDelete);
            consultationInnew.AndAlso(t => !t.IsDelete);
            consultationInold.AndAlso(t => !t.IsDelete);
            var timetotal = await _yaeherUserService.YaeherUserList(totalin); //新增用户数
            var alluser = new YaeherUserIn();
            alluser.AndAlso(t => !t.IsDelete);
            var total = await _yaeherUserService.YaeherUserList(alluser);//总用户数

            var orde = new OrderTradeRecordIn(); orde.AndAlso(t => !t.IsDelete && t.PaymentState == "paid" && t.PaymentSourceCode == "order");
            var orderlist = await _orderTradeRecordService.OrderTradeRecordReportList(orde);

            var distinctorder = orderlist.Select(i => new { i.CreatedBy }).Distinct().ToList();//付费用户数

            var NewoldPaidUser = orderlist.Where(t => t.CreatedOn < StartTime).Select(i => new { i.CreatedBy }).Distinct().ToList();

            var NewPaidUser = orderlist.Where(t => t.CreatedOn < EndTime.AddDays(+1)).Select(i => new { i.CreatedBy }).Distinct().ToList();

            var yaeherconsultationnew = await _consultationService.YaeherConsultationList(consultationInnew);
            var usernew = yaeherconsultationnew.Select(t => new { t.CreatedBy }).Distinct().ToList();
            var yaeherconsultationold = await _consultationService.YaeherConsultationList(consultationInold);

            var userold = yaeherconsultationold.Select(t => new { t.CreatedBy }).Distinct().ToList();

            var intersectedList = usernew.Intersect(userold);
            var fglist = intersectedList.ToList().Count();
            var buyagain = fglist == 0 ? 0.00 : fglist;
            var oldbyagain = Convert.ToDouble(userold.Count);
            var buyagainprice = 0.00;
            if (oldbyagain > 0)
            {
                buyagainprice = Math.Round((buyagain / oldbyagain) * 100, 2);
            }
            var newpaid = (NewPaidUser.Count() - NewoldPaidUser.Count());//新增付费用户数

            var doc = new YaeherDoctorIn(); doc.AndAlso(t => !t.IsDelete);
            var doctor = await _yaeherDoctorService.YaeherDoctorList(doc);

            var olddoctor = doctor.Where(t => t.CreatedOn < StartTime).ToList();
            var newdoctor = doctor.Where(t => t.CreatedOn < EndTime.AddDays(+1)).ToList();
            var newdoctorcount = (newdoctor.Count() - olddoctor.Count());//医生

            this.ObjectResultModule.Object = new UserReport(total.Count(), timetotal.Count(), distinctorder.Count(), newpaid, newdoctorcount, fglist, buyagainprice.ToString());

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AdminFlowReport",
                OperContent = JsonHelper.ToJson(totalin),
                OperType = "AdminFlowReport",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            this.ObjectResultModule.StatusCode = 200;
            return this.ObjectResultModule;

        }

        /// <summary>
        ///管理收入列表
        /// </summary>
        [Route("api/AdminIncomeReport")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AdminIncomeReport([FromBody]CorporateIncomeTotalIn totalin)
        {
            if (!Commons.CheckSecret(totalin.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            totalin.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                StartTime = DateTime.Parse(totalin.StartTime);
                if (string.IsNullOrEmpty(totalin.EndTime))
                {
                    totalin.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(totalin.EndTime))
            {
                EndTime = DateTime.Parse(totalin.EndTime);
            }
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                totalin.AndAlso(t => t.CreatedOn >= StartTime).AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(totalin.IncomeType))
            {
                totalin.AndAlso(t => t.IncomeType == totalin.IncomeType);
            }
            var income = await _corporateIncomeTotalService.CorporateIncomeTotalList(totalin);
            if (income.Count > 0)
            {
                this.ObjectResultModule.Object = new IncomeTotal(income.FirstOrDefault());
            }
            else
            {
                this.ObjectResultModule.Object = new IncomeTotal();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AdminIncomeReport",
                OperContent = JsonHelper.ToJson(totalin),
                OperType = "AdminIncomeReport",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            this.ObjectResultModule.StatusCode = 200;
            return this.ObjectResultModule;
        }
        /// <summary>
        ///管理收入列表明细
        /// </summary>
        [Route("api/AdminIncomeDetailReport")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AdminIncomeDetailReport([FromBody]CorporateIncomeTotalIn totalin)
        {
            if (!Commons.CheckSecret(totalin.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            totalin.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                StartTime = DateTime.Parse(totalin.StartTime);
                if (string.IsNullOrEmpty(totalin.EndTime))
                {
                    totalin.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(totalin.EndTime))
            {
                EndTime = DateTime.Parse(totalin.EndTime);
            }
            if (!string.IsNullOrEmpty(totalin.StartTime))
            {
                totalin.AndAlso(t => t.TotalDate >= StartTime).AndAlso(t => t.TotalDate < EndTime.AddDays(+1));
            }
            switch (totalin.IncomeType)
            {
                case "year":
                    totalin.AndAlso(t => t.IncomeType == "month");
                    break;
                case "month":
                    totalin.AndAlso(t => t.IncomeType == "day");
                    break;
                case "day":
                    break;
            }
            if (totalin.IncomeType != "day")
            {
                var income = await _corporateIncomeTotalService.CorporateIncomeTotalList(totalin);
                this.ObjectResultModule.Object = income.Select(t => new IncomeTotal(t)).ToList();
            }
            else
            {
                var ind = new IncomeDetailsIn();
                ind.AndAlso(t => !t.IsDelete && t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1) && t.DoctorID != 0);
                var detail = await _consultationService.IncomeConsultationDetail(ind);
                this.ObjectResultModule.Object = detail;
            }
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AdminIncomeDetailReport",
                OperContent = JsonHelper.ToJson(totalin),
                OperType = "AdminIncomeDetailReport",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

    }
}