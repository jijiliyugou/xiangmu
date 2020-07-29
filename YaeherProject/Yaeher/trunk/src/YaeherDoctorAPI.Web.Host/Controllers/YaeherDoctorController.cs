using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Myvas.AspNetCore.TencentCos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.TencentCustom;
using Yaeher.Consultation;
using Yaeher.Doctor;
using Yaeher.Extensions;
using Yaeher.LableManages;
using Yaeher.LableManages.Dto;
using Yaeher.MessageRemind;
using Yaeher.MessageRemind.Dto;
using Yaeher.NumericalStatement;
using Yaeher.NumericalStatement.Dto;
using Yaeher.Quality;
using Yaeher.Release;
using Yaeher.Scheduling;
using Yaeher.Scheduling.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherDoctorAPI.Web.Host.Controllers
{
    /// <summary>
    /// 医生信息管理API
    /// </summary>
    public class YaeherDoctorController : YaeherAppServiceBase
    {
        private readonly IYaeherDoctorService _YaeherDoctorService;
        private readonly IDoctorRelationService _DoctorRelationService;
        private readonly IDoctorFileApplyService _DoctorFileApplyService;
        private readonly IDoctorClinicApplyService _DoctorClinicApplyService;
        private readonly IServiceMoneyListService _ServiceMoneyListService;
        private readonly IDoctorServiceLogService _DoctorServiceLogService;
        private readonly IDoctorPaperService _DoctorPaperService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IClinicInfomationService _clinicInfomationService;
        private readonly IClinicLableReltionService _clinicLableReltionService;
        private readonly IClinicDoctorReltionService _clinicDoctorReltionService;
        private readonly ILableManageService _lableManageService;
        private readonly IYaeherUserService _yaeherUserService;
        private readonly IYaeherMessageRemindService _yaeherMessageRemindService;
        private readonly IDoctorSchedulingService _DoctorSchedulingService;
        private readonly IReleaseManageService _ReleaseManageService;
        private readonly ICollectConsultationService _collectConsultationService;
        private readonly IConsultationService _consultationService;
        private readonly IDoctorEmploymentService _doctorEmploymentService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IAttachmentServices _attachmentServices;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDoctorOnlineRecordService _doctorOnlineRecordService;
        private readonly IQualityCommitteeService _qualityCommitteeService;
        private readonly IUserManagerService _userManagerService;
        private readonly IEvaluationTotalService _evaluationTotalService;
        private readonly IConsultationOrderTotalService _consultationOrderTotalService;
        private readonly IAbpSession _IabpSession;
        private readonly IDoctorOnlineSetLogService _doctorOnlineSetLogService;
        private readonly IYaeherUserPaymentService _yaeherUserPaymentService;
        private readonly IRecommendedOrderingService _recommendedOrderingService; // 指定排序
        private readonly ISystemConfigsService _SystemConfigsService;
        private readonly ITencentCosHandler _cosHandler;
        private readonly IRelationLabelListService _relationLabelListService;
        private readonly ISystemTokenService _systemTokenService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="doctorRelationService"></param>
        /// <param name="doctorFileApplyService"></param>
        /// <param name="doctorClinicApplyService"></param>
        /// <param name="serviceMoneyListService"></param>
        /// <param name="doctorServiceLogService"></param>
        /// <param name="doctorPaperService"></param>
        /// <param name="clinicInfomationService"></param>
        /// <param name="clinicLableReltionService"></param>
        /// <param name="clinicDoctorReltionService"></param>
        /// <param name="lableManageService"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="yaeherMessageRemindService"></param>
        /// <param name="DoctorSchedulingService"></param>
        /// <param name="ReleaseManageService"></param>
        /// <param name="collectConsultationService"></param>
        /// <param name="consultationService"></param>
        /// <param name="doctorEmploymentService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="attachmentServices"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="doctorOnlineRecordService"></param>
        /// <param name="qualityCommitteeService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="evaluationTotalService"></param>
        /// <param name="consultationOrderTotalService"></param>
        /// <param name="session"></param>
        /// <param name="doctorOnlineSetLogService"></param>
        /// <param name="yaeherUserPaymentService"></param>
        /// <param name="recommendedOrderingService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="systemConfigsService"></param>
        /// <param name="cosHandler"></param>
        /// <param name="relationLabelListService"></param>
        /// <param name="systemTokenService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="cacheManager"></param>
        public YaeherDoctorController(IYaeherDoctorService yaeherDoctorService,
                                        IDoctorRelationService doctorRelationService,
                                        IDoctorFileApplyService doctorFileApplyService,
                                        IDoctorClinicApplyService doctorClinicApplyService,
                                        IServiceMoneyListService serviceMoneyListService,
                                        IDoctorServiceLogService doctorServiceLogService,
                                        IDoctorPaperService doctorPaperService,
                                        IClinicInfomationService clinicInfomationService,
                                        IClinicLableReltionService clinicLableReltionService,
                                        IClinicDoctorReltionService clinicDoctorReltionService,
                                        ILableManageService lableManageService,
                                        IYaeherUserService yaeherUserService,
                                        IYaeherMessageRemindService yaeherMessageRemindService,
                                        IDoctorSchedulingService DoctorSchedulingService,
                                        IReleaseManageService ReleaseManageService,
                                        ICollectConsultationService collectConsultationService,
                                        IConsultationService consultationService,
                                        IDoctorEmploymentService doctorEmploymentService,
                                        ISystemParameterService systemParameterService,
                                        IAttachmentServices attachmentServices,
                                        IUnitOfWorkManager unitOfWorkManager,
                                        IDoctorOnlineRecordService doctorOnlineRecordService,
                                        IQualityCommitteeService qualityCommitteeService,
                                        IUserManagerService userManagerService,
                                        IEvaluationTotalService evaluationTotalService,
                                        IConsultationOrderTotalService consultationOrderTotalService,
                                        IAbpSession session,
                                        IDoctorOnlineSetLogService doctorOnlineSetLogService,
                                        IYaeherUserPaymentService yaeherUserPaymentService,
                                        IRecommendedOrderingService recommendedOrderingService,
                                        IYaeherOperListService yaeherOperListService,
                                        ISystemConfigsService systemConfigsService,
                                        ITencentCosHandler cosHandler,
                                        IRelationLabelListService relationLabelListService,
                                        ISystemTokenService systemTokenService,
                                        IOrderTradeRecordService orderTradeRecordService,
                                        ICacheManager cacheManager)
        {
            _YaeherDoctorService = yaeherDoctorService;
            _DoctorRelationService = doctorRelationService;
            _DoctorFileApplyService = doctorFileApplyService;
            _DoctorClinicApplyService = doctorClinicApplyService;
            _ServiceMoneyListService = serviceMoneyListService;
            _DoctorServiceLogService = doctorServiceLogService;
            _DoctorPaperService = doctorPaperService;
            _clinicInfomationService = clinicInfomationService;
            _clinicLableReltionService = clinicLableReltionService;
            _clinicDoctorReltionService = clinicDoctorReltionService;
            _lableManageService = lableManageService;
            _yaeherUserService = yaeherUserService;
            _yaeherMessageRemindService = yaeherMessageRemindService;
            _DoctorSchedulingService = DoctorSchedulingService;
            _ReleaseManageService = ReleaseManageService;
            _collectConsultationService = collectConsultationService;
            _consultationService = consultationService;
            _doctorEmploymentService = doctorEmploymentService;
            _systemParameterService = systemParameterService;
            _attachmentServices = attachmentServices;
            _unitOfWorkManager = unitOfWorkManager;
            _doctorOnlineRecordService = doctorOnlineRecordService;
            _qualityCommitteeService = qualityCommitteeService;
            _userManagerService = userManagerService;
            _evaluationTotalService = evaluationTotalService;
            _consultationOrderTotalService = consultationOrderTotalService;
            _IabpSession = session;
            _doctorOnlineSetLogService = doctorOnlineSetLogService;
            _yaeherUserPaymentService = yaeherUserPaymentService;
            _recommendedOrderingService = recommendedOrderingService;
            _yaeherOperListService = yaeherOperListService;
            _SystemConfigsService = systemConfigsService;
            _cosHandler = cosHandler;
            _relationLabelListService = relationLabelListService;
            _systemTokenService = systemTokenService;
            _orderTradeRecordService = orderTradeRecordService;
            _cacheManager = cacheManager;
        }

        #region   获取微信js-sdk授权
        ///// <summary>
        ///// 获取微信js-sdk授权
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[Route("api/WXTestCreateDoctor")]
        //[HttpPost]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> WXTestCreateDoctor([FromBody]TencentTicletModel input)
        //{
        //    if (!Commons.CheckSecret(input.Secret))
        //    {
        //        this.ObjectResultModule.StatusCode = 422;
        //        this.ObjectResultModule.Message = "Wrong Secret";
        //        this.ObjectResultModule.Object = "";
        //        return this.ObjectResultModule;
        //    }
        //    SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
        //    systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
        //    var configs = await _SystemConfigsService.SystemConfigsList(systemConfigsIn);
        //    var tencentparam = configs.FirstOrDefault();
        //    TencentWXPay tencentWXPay = new TencentWXPay();

        //    var receiver = new receiver();
        //    receiver.name = "";
        //    receiver.type = "PERSONAL_OPENID";
        //    receiver.account = "o4QdD1grpduA8s8RfHePksJ_3pwI";
        //    var addresult = await tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam);
        //    if (addresult.result_code != "SUCCESS")
        //    {
        //        return new ObjectResultModule("", 400, "请求确保填写真实姓名并已经微信实名认证成功！");
        //    }
        //    return new ObjectResultModule("", 200, "");
        //}
        #endregion

        #region 医生信息
        /// <summary>
        /// 医生认证类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/YaeherDoctorAuthType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherDoctorAuthType([FromBody] SecretModel input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorAuthType");
            var paramlist = await _systemParameterService.ParameterList(param);

            var coderesult = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                coderesult.Add(newcode);
            }
            this.ObjectResultModule.Object = coderesult;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherDoctorAuthType",
                OperContent = JsonHelper.ToJson(input),
                OperType = "YaeherDoctorAuthType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 新增医生信息  注册医生
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherDoctor([FromBody] YaeherDoctorIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (string.IsNullOrEmpty(input.VerificationCode))
            {
                return new ObjectResultModule("", 100, "请先填写注册码！");
            }
            if (string.IsNullOrEmpty(input.PhoneNumber))
            {
                return new ObjectResultModule("", 100, "请先填写手机号码！");
            }
            //验证验证码
            var mes = new YaeherMessageRemindIn();
            mes.AndAlso(t => !t.IsDelete);
            mes.AndAlso(t => t.PhoneNumber == input.PhoneNumber);
            mes.AndAlso(t => t.MessageType == "Verification");
            var message = await _yaeherMessageRemindService.YaeherMessageRemindList(mes);
            if (message != null)
            {
                if (message[0].VerificationCode != input.VerificationCode)
                {
                    return new ObjectResultModule("", 100, "请确认验证码正确！");
                }
                if (message[0].EffectiveTime < DateTime.Now)
                {
                    return new ObjectResultModule("", 100, "请重新生成验证码，该验证码已失效！");
                }
            }
            else
            {
                return new ObjectResultModule("", 100, "请先点击发送验证码！");
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherDoctorIn listin = new YaeherDoctorIn();
            listin.AndAlso(t => t.IsDelete == false && t.UserID == userid && (t.CheckRes == "success" || t.CheckRes == "checking"));
            var checkdoctor = await _YaeherDoctorService.YaeherDoctorList(listin);
            if (checkdoctor.Count > 0)
            {
                return new ObjectResultModule("", 100, "请不要重复提交！");
            }
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var User = await _yaeherUserService.YaeherUserByID(userid);
            //Logger.Info(JsonHelper.ToJson(User));
            #region 新增微信分成
            if (!string.IsNullOrEmpty(usermanager.WecharOpenID))
            {
                SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
                systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
                var configs = await _SystemConfigsService.SystemConfigsList(systemConfigsIn);
                var tencentparam = configs.FirstOrDefault();
                TencentWXPay tencentWXPay = new TencentWXPay();

                var receiver = new receiver();
                receiver.name = input.DoctorName;
                receiver.type = "PERSONAL_OPENID";
                receiver.account = usermanager.WecharOpenID;
                var addresult = await tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam);
                //Logger.Info("插入微信分账姓名：" + JsonHelper.ToJson(receiver));
                //Logger.Info("插入微信分账关系结果：" + JsonHelper.ToJson(addresult));
                if (addresult.result_code != "SUCCESS")
                {
                    return new ObjectResultModule("", 400, "请确保填写真实姓名并已经微信实名认证成功！");
                }

            }
            #endregion

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var create = new YaeherDoctor()
                {
                    DoctorName = input.DoctorName,
                    UserID = userid,
                    HospitalName = input.HospitalName,
                    Department = input.Department,
                    WorkYear = input.WorkYear,
                    Title = input.Title,
                    GraduateSchool = input.GraduateSchool,
                    IsBelieveTCM = input.IsBelieveTCM,
                    IsServiceConscious = input.IsServiceConscious,
                    WechatNum = input.WechatNum,
                    PhoneNumber = input.PhoneNumber,
                    Recommender = input.Recommender,
                    RecommenderName = input.RecommenderName,
                    IDCard = input.IDCard,
                    Address = input.Address,
                    CheckRes = "checking",
                    AuthCheckRes = "unupload",
                    BaseTestRes = "UnExam",
                    SimTestRes = "UnExam",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    IsSharing = true,//默认开启分账
                    IsAbroad = input.IsAbroad,
                    ServiceState = true,  // 服务状态 默认开启
                    ReceiptState = true, // 接单状态 默认开启
                };
                #region 历史操作
                //判断是否存在微信首款方式
                //不存在则添加
                //var pay = new YaeherUserPaymentIn(); pay.AndAlso(t => !t.IsDelete && t.UserID == userid && t.PayMethod == "wxpay");
                //var userpay = await _yaeherUserPaymentService.YaeherUserPaymentList(pay);
                //if (userpay.Count < 1)
                //{

                //}
                #endregion

                #region 提交支付
                var CreateUserPayment = new YaeherUserPayment()
                {
                    UserID = userid,
                    FullName = User.FullName,
                    PayMethod = "wxpay",
                    PayMethodName = "微信支付",
                    PaymentAccout = string.IsNullOrEmpty(User.WecharName) ? "" : User.WecharName,
                    BankName = "wx",
                    Subbranch = "wx",
                    BandAdd = "wx",
                    BankNo = "wx",
                    CreatedOn = DateTime.Now,
                    CreatedBy = userid,
                    IsDefault = true,
                };
                YaeherUserPaymentIn yaeherUserPaymentIn = new YaeherUserPaymentIn();
                yaeherUserPaymentIn.AndAlso(a => a.IsDelete == false);
                yaeherUserPaymentIn.AndAlso(a => a.UserID == userid);
                yaeherUserPaymentIn.AndAlso(a => a.PayMethod == "wxpay");
                var UserPaymentList = await _yaeherUserPaymentService.YaeherUserPaymentList(yaeherUserPaymentIn);
                if (UserPaymentList.Count > 0)
                {
                    var UserPayment = UserPaymentList.FirstOrDefault();
                    UserPayment.UserID = CreateUserPayment.UserID;
                    UserPayment.FullName = CreateUserPayment.FullName;
                    UserPayment.PayMethod = CreateUserPayment.PayMethod;
                    UserPayment.PayMethodName = CreateUserPayment.PayMethodName;
                    UserPayment.PaymentAccout = CreateUserPayment.PaymentAccout;
                    UserPayment.BankName = CreateUserPayment.BankName;
                    UserPayment.Subbranch = CreateUserPayment.Subbranch;
                    UserPayment.BandAdd = CreateUserPayment.BandAdd;
                    UserPayment.BankNo = CreateUserPayment.BankNo;
                    UserPayment.ModifyOn = DateTime.Now;
                    UserPayment.ModifyBy = userid;
                    UserPayment.IsDefault = true;
                    var result1 = await _yaeherUserPaymentService.UpdateYaeherUserPayment(UserPayment);
                }
                else
                {
                    var result1 = await _yaeherUserPaymentService.CreateYaeherUserPayment(CreateUserPayment);
                }
                #endregion
                #region 医生执业记录
                var employ = new DoctorEmployment()
                {
                    UserID = userid,
                    HospitalName = input.HospitalName,
                    Department = input.Department,
                    WorkYear = input.WorkYear,
                    Title = input.Title,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                DoctorEmploymentIn doctorEmploymentIn = new DoctorEmploymentIn();
                doctorEmploymentIn.AndAlso(a => a.IsDelete == false);
                doctorEmploymentIn.AndAlso(a => a.UserID == userid);
                var empcreateList = await _doctorEmploymentService.DoctorEmploymentList(doctorEmploymentIn);
                if (empcreateList.Count > 0)
                {
                    var doctorEmployment = empcreateList.FirstOrDefault();
                    doctorEmployment.UserID = userid;
                    doctorEmployment.HospitalName = input.HospitalName;
                    doctorEmployment.Department = input.Department;
                    doctorEmployment.WorkYear = input.WorkYear;
                    doctorEmployment.Title = input.Title;
                    doctorEmployment.ModifyBy = userid;
                    doctorEmployment.ModifyOn = DateTime.Now;
                    var empcreate = await _doctorEmploymentService.UpdateDoctorEmployment(doctorEmployment);
                }
                else
                {
                    var empcreate = await _doctorEmploymentService.CreateDoctorEmployment(employ);
                }
                #endregion

                #region 医生基本信息
                YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
                yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
                yaeherDoctorIn.AndAlso(a => a.UserID == userid);
                YaeherDoctor yaeherDoctor = new YaeherDoctor();
                var yaeherDoctorList = await _YaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);
                if (yaeherDoctorList.Count > 0)
                {
                    var Update = yaeherDoctorList.FirstOrDefault();
                    Update.DoctorName = input.DoctorName;
                    Update.UserID = userid;
                    Update.HospitalName = input.HospitalName;
                    Update.Department = input.Department;
                    Update.WorkYear = input.WorkYear;
                    Update.Title = input.Title;
                    Update.GraduateSchool = input.GraduateSchool;
                    Update.IsBelieveTCM = input.IsBelieveTCM;
                    Update.IsServiceConscious = input.IsServiceConscious;
                    Update.WechatNum = input.WechatNum;
                    Update.PhoneNumber = input.PhoneNumber;
                    Update.Recommender = input.Recommender;
                    Update.RecommenderName = input.RecommenderName;
                    if (Update.AuthCheckRes != "success" && !string.IsNullOrEmpty(input.IDCard))
                    {
                        Update.IDCard = input.IDCard;
                    }
                    Update.Address = input.Address;
                    Update.CheckRes = "checking";
                    Update.AuthCheckRes = "unupload";
                    Update.BaseTestRes = "UnExam";
                    Update.SimTestRes = "UnExam";
                    Update.ModifyBy = userid;
                    Update.ModifyOn = DateTime.Now;
                    yaeherDoctor = await _YaeherDoctorService.UpdateYaeherDoctor(Update);
                }
                else
                {
                    yaeherDoctor = await _YaeherDoctorService.CreateYaeherDoctor(create);
                }
                #endregion

                unitOfWork.Complete();
                this.ObjectResultModule.Object = yaeherDoctor;
            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "sucess";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateYaeherDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateYaeherDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 查看医生基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/YaeherDoctorById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherDoctorById([FromBody] YaeherDoctorIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherDoctor value = new YaeherDoctor();
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            // 当为咨询人时查看医生 使用UserID
            if (usermanager.MobileRoleName == "patient")
            {
                if (input.Id > 0)//患者查看其他医生
                {
                    value = await _YaeherDoctorService.YaeherDoctorByID(input.Id);
                }
                else//患者查看自己申请医生的信息
                {
                    value = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
                }
            }
            else if (usermanager.MobileRoleName == "doctor")
            {
                value = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            }
            else  // 其他角色查看
            {
                value = await _YaeherDoctorService.YaeherDoctorByID(input.Id);
            }
            if (value == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var user = await _yaeherUserService.YaeherUserByID(value.UserID);
            if (user == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorAuthCheckType");
            var paramlist = await _systemParameterService.ParameterList(param);

            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorAuthType");
            var paramlist1 = await _systemParameterService.ParameterList(param);
            if (usermanager.MobileRoleName != "patient")
            {
                var file = new DoctorFileApplyIn();
                file.AndAlso(t => !t.IsDelete && t.CreatedBy == user.Id && t.DocumentsUse == "register");
                var applyfile = await _DoctorFileApplyService.DoctorFileApplyList(file);
                var doctor = new YaeherDoctorUser(value, user, paramlist, applyfile, paramlist1);
                this.ObjectResultModule.Object = doctor;
            }
            else
            {
                var doctor = new YaeherDoctorUser(value, user, paramlist);
                this.ObjectResultModule.Object = doctor;
            }
            if (value.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";

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
                OperExplain = "YaeherDoctorById",
                OperContent = JsonHelper.ToJson(input),
                OperType = "YaeherDoctorById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 客服端查看医生注册信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CustomerServiceYaeherDoctorById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CustomerServiceYaeherDoctorById([FromBody] YaeherDoctorIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            var value = await _YaeherDoctorService.YaeherDoctorByID(input.Id);
            if (value == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var user = await _yaeherUserService.YaeherUserByID(value.UserID);
            if (user == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorAuthCheckType");
            var paramlist = await _systemParameterService.ParameterList(param);

            param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorAuthType");
            var paramlist1 = await _systemParameterService.ParameterList(param);

            if (usermanager.MobileRoleName == "customerservice" || usermanager.IsCustomerService)
            {
                var file = new DoctorFileApplyIn();
                file.AndAlso(t => !t.IsDelete && t.CreatedBy == user.Id && t.DocumentsUse == "register");
                var applyfile = await _DoctorFileApplyService.DoctorFileApplyList(file);

                var doctor = new YaeherDoctorUser(value, user, paramlist, applyfile, paramlist1);
                this.ObjectResultModule.Object = doctor;
            }
            else
            {
                var doctor = new YaeherDoctorUser(value, user, paramlist);
                this.ObjectResultModule.Object = doctor;
            }

            if (value.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";

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
                OperExplain = "CustomerServiceYaeherDoctorById",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CustomerServiceYaeherDoctorById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// YaeherDoctor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/YaeherDoctor")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> YaeherDoctor([FromBody] SecretModel input)
        {
            //if (!Commons.CheckSecret(input.Secret))
            //{
            //    this.ObjectResultModule.StatusCode = 422;
            //    this.ObjectResultModule.Message = "Wrong Secret";
            //    this.ObjectResultModule.Object = "";
            //    return this.ObjectResultModule;
            //}
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //userid = 36;
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            if (doctor == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var user = await _yaeherUserService.YaeherUserByID(userid);
            if (user == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorAuthCheckType");
            var paramlist = await _systemParameterService.ParameterList(param);

            var quality = await _qualityCommitteeService.QualityCommitteeByDoctorID(doctor.Id);

            var doctorview = new YaeherDoctorUser(doctor, user, paramlist, quality);
            if (doctor.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = doctorview;
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
                OperExplain = "YaeherDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "YaeherDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生信息List 
        /// </summary>
        /// <param name="YaeherDoctorInList"> YaeherDoctorInList 数据</param>
        /// <returns></returns>
        [Route("api/YaeherDoctorList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherDoctorList([FromBody]YaeherDoctorIn YaeherDoctorInList)
        {
            if (!Commons.CheckSecret(YaeherDoctorInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            YaeherDoctorInList.AndAlso(t => !t.IsDelete);
            if (YaeherDoctorInList.Checker > 0)
            {
                YaeherDoctorInList.AndAlso(t => t.Checker == YaeherDoctorInList.Checker);
            }
            if (!string.IsNullOrEmpty(YaeherDoctorInList.DoctorName))
            {
                YaeherDoctorInList.AndAlso(t => t.DoctorName.Contains(YaeherDoctorInList.DoctorName));
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherDoctorInList.StartTime))
            {
                StartTime = DateTime.Parse(YaeherDoctorInList.StartTime);
                if (string.IsNullOrEmpty(YaeherDoctorInList.EndTime))
                {
                    YaeherDoctorInList.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherDoctorInList.EndTime))
            {
                EndTime = DateTime.Parse(YaeherDoctorInList.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherDoctorInList.StartTime))
            {
                YaeherDoctorInList.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            var value = await _YaeherDoctorService.YaeherDoctorList(YaeherDoctorInList);
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
                OperExplain = "YaeherDoctorList",
                OperContent = JsonHelper.ToJson(YaeherDoctorInList),
                OperType = "YaeherDoctorList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }


        /// <summary>
        /// 获取医生 Page
        /// </summary>
        /// <param name="YaeherDoctorInList"> DoctorRelationInPage 数据</param>
        /// <returns></returns>
        [Route("api/YaeherDoctorPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherDoctorPage([FromBody]YaeherDoctorIn YaeherDoctorInList)
        {
            if (!Commons.CheckSecret(YaeherDoctorInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            YaeherDoctorInList.AndAlso(t => !t.IsDelete);
            if (YaeherDoctorInList.Checker > 0)
            {
                YaeherDoctorInList.AndAlso(t => t.Checker == YaeherDoctorInList.Checker);
            }
            if (!string.IsNullOrEmpty(YaeherDoctorInList.KeyWord))
            {
                YaeherDoctorInList.AndAlso(t => t.DoctorName.Contains(YaeherDoctorInList.KeyWord));
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherDoctorInList.StartTime))
            {
                StartTime = DateTime.Parse(YaeherDoctorInList.StartTime);
                if (string.IsNullOrEmpty(YaeherDoctorInList.EndTime))
                {
                    YaeherDoctorInList.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherDoctorInList.EndTime))
            {
                EndTime = DateTime.Parse(YaeherDoctorInList.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherDoctorInList.StartTime))
            {
                YaeherDoctorInList.AndAlso(t => t.CreatedOn >= StartTime);
                YaeherDoctorInList.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (usermanager.MobileRoleName == "customerservice" || usermanager.IsCustomerService)
            {
                YaeherDoctorInList.AndAlso(t => t.CheckRes == "success");
            }
            if (usermanager.MobileRoleName == "admin" || usermanager.IsAdmin)
            {
                // 医生审核状态
                if (!string.IsNullOrEmpty(YaeherDoctorInList.CheckRes))
                {
                    YaeherDoctorInList.AndAlso(t => t.CheckRes == YaeherDoctorInList.CheckRes);//待审核
                }
                else
                {
                    YaeherDoctorInList.AndAlso(t => t.CheckRes != "checking");//已处理
                }
            }
            // 基础考试结果
            if (!string.IsNullOrEmpty(YaeherDoctorInList.BaseTestRes))
            {
                YaeherDoctorInList.AndAlso(t => t.BaseTestRes == YaeherDoctorInList.BaseTestRes);
            }
            // 认证审核状态
            if (!string.IsNullOrEmpty(YaeherDoctorInList.AuthCheckRes))
            {
                YaeherDoctorInList.AndAlso(t => t.AuthCheckRes == YaeherDoctorInList.AuthCheckRes);
            }
            var values = await _YaeherDoctorService.YaeherDoctorUserPage(YaeherDoctorInList);
            // var values = await _YaeherDoctorService.YaeherDoctorPage(YaeherDoctorInList);
            this.ObjectResultModule.Object = new YaeherDoctorUserOut(values, YaeherDoctorInList);
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherDoctorPage",
                OperContent = JsonHelper.ToJson(YaeherDoctorInList),
                OperType = "YaeherDoctorPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        ///// <summary>
        ///// 更新医生信息
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[Route("api/UpdateYaeherDoctorByDoctor")]
        //[HttpPost]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> UpdateYaeherDoctorByDoctor([FromBody] YaeherDoctor input)
        //{
        //    if (!Commons.CheckSecret(input.Secret))
        //    {
        //        this.ObjectResultModule.StatusCode = 422;
        //        this.ObjectResultModule.Message = "Wrong Secret";
        //        this.ObjectResultModule.Object = "";
        //        return this.ObjectResultModule;
        //    }
        //    var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
        //    var query = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
        //    //if (query.CheckRes == "success"&&string.IsNullOrEmpty(input.UserImageFile)) { return new ObjectResultModule("", 204, "未审核状态才能提交修改!"); } 
        //    if (query != null && query.CreatedBy == userid)
        //    {

        //    }
        //    this.ObjectResultModule.Object = "";
        //    this.ObjectResultModule.Message = "sucess";
        //    this.ObjectResultModule.StatusCode = 200;
        //    return this.ObjectResultModule;
        //}
        /// <summary>
        /// 更新医生简介信息 允许为空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherDoctorResume")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherDoctorResume([FromBody] YaeherDoctorIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var query = new YaeherDoctor();
            if (usermanager.MobileRoleName == "doctor" || usermanager.IsDoctor)
            {
                query = await _YaeherDoctorService.YaeherDoctorByID(input.Id);
            }
            query.Resume = input.Resume;
            query.ModifyOn = DateTime.Now;
            query.ModifyBy = userid;
            var res = await _YaeherDoctorService.UpdateYaeherDoctor(query);

            this.ObjectResultModule.Object = res;
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateYaeherDoctorResume",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateYaeherDoctorResume",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;

        }
        ///// <summary>
        ///// 更新医生信息
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[Route("api/UpdateYaeherDoctorByDoctor")]
        //[HttpPost]
        //[AbpAuthorize]
        //public async Task<ObjectResultModule> UpdateYaeherDoctorByDoctor([FromBody] YaeherDoctor input)
        //{
        //    if (!Commons.CheckSecret(input.Secret))
        //    {
        //        this.ObjectResultModule.StatusCode = 422;
        //        this.ObjectResultModule.Message = "Wrong Secret";
        //        this.ObjectResultModule.Object = "";
        //        return this.ObjectResultModule;
        //    }
        //    var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
        //    var query = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
        //    //if (query.CheckRes == "success"&&string.IsNullOrEmpty(input.UserImageFile)) { return new ObjectResultModule("", 204, "未审核状态才能提交修改!"); } 
        //    if (query != null && query.CreatedBy == userid)
        //    {

        //    }
        //    this.ObjectResultModule.Object = "";
        //    this.ObjectResultModule.Message = "sucess";
        //    this.ObjectResultModule.StatusCode = 200;
        //    return this.ObjectResultModule;
        //}
        /// <summary>
        /// 更新医生信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherDoctor([FromBody] YaeherDoctorIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var query = new YaeherDoctor();
            if (usermanager.MobileRoleName == "doctor" || usermanager.IsDoctor)
            {
                query = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            }
            else
            {
                query = await _YaeherDoctorService.YaeherDoctorByID(input.Id);
            }
            if (!string.IsNullOrEmpty(input.UserImageFile))
            {
                var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
                var paramlist = await _systemParameterService.ParameterList(param);

                query.UserImageFile = paramlist[0].ItemValue + "/" + input.UserImageFile;
            }

            if (!string.IsNullOrEmpty(input.DoctorName))
            {
                query.DoctorName = input.DoctorName;
            }
            if (!string.IsNullOrEmpty(input.Address))
            {
                query.Address = input.Address;
            }
            if (!string.IsNullOrEmpty(input.HospitalName))
            {
                query.HospitalName = input.HospitalName;
            }
            if (query.AuthCheckRes != "success" && !string.IsNullOrEmpty(input.IDCard))
            {
                query.IDCard = input.IDCard;
            }
            if (!string.IsNullOrEmpty(input.Department))
            {
                query.Department = input.Department;
            }
            if (input.WorkYear > 0)
            {
                query.WorkYear = input.WorkYear;
            }
            if (!string.IsNullOrEmpty(input.Title))
            {
                query.Title = input.Title;
            }
            if (!string.IsNullOrEmpty(input.GraduateSchool))
            {
                query.GraduateSchool = input.GraduateSchool;
            }
            if (!string.IsNullOrEmpty(input.PhoneNumber))
            {
                query.PhoneNumber = input.PhoneNumber;
            }
            if (input.Recommender > 0)
            {
                query.Recommender = input.Recommender;
            }
            if (!string.IsNullOrEmpty(input.WechatNum))
            {
                query.WechatNum = input.WechatNum;
            }
            if (query.CheckRes == "success" && !string.IsNullOrEmpty(input.AuthType))
            {
                query.AuthType = input.AuthType;
            }
            if (input.AuthCheckRes == "upload")
            {
                query.AuthCheckRes = "upload";
            }
            query.ModifyOn = DateTime.Now;
            query.ModifyBy = userid;
            var res = await _YaeherDoctorService.UpdateYaeherDoctor(query);

            this.ObjectResultModule.Object = res;
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateYaeherDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateYaeherDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 删除医生信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherDoctor([FromBody] YaeherDoctor input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _YaeherDoctorService.YaeherDoctorByID(input.Id);
            //  当医生存在订单 不允许删除
            ConsultationIn consultationIn = new ConsultationIn();
            consultationIn.AndAlso(a => a.DoctorID == query.Id);
            consultationIn.AndAlso(a => a.ConsultState != "success" || a.ConsultState != "return");
            var DoctorConsultList = await _consultationService.YaeherConsultationList(consultationIn);
            if (DoctorConsultList.Count() > 0)
            {
                this.ObjectResultModule.Message = "当前医生订单未全部完成，不可删除！";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _YaeherDoctorService.DeleteYaeherDoctor(query);

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
                OperExplain = "DeleteYaeherDoctor",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteYaeherDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        #endregion

        #region 医生与标签关系
        /// <summary>
        /// 新增医生与标签关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorRelation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorRelation([FromBody] DoctorRelationIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
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
                DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
                doctorRelationIn.AndAlso(a => a.IsDelete == false);
                doctorRelationIn.AndAlso(a => a.DoctorID == input.DoctorID);
                var DoctorRelationList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);
                if (DoctorRelationList.Count > 0)
                {
                    foreach (var DoctorRelationInfo in DoctorRelationList)
                    {
                        DoctorRelationInfo.DeleteBy = userid;
                        DoctorRelationInfo.DeleteTime = DateTime.Now;
                        DoctorRelationInfo.IsDelete = true;
                        var result = await _DoctorRelationService.DeleteDoctorRelation(DoctorRelationInfo);
                    }
                }
                #endregion
                #region 再新增
                string[] LableArray = null;
                if (!string.IsNullOrEmpty(input.LableIDJSON))
                {
                    LableArray = input.LableIDJSON.Split(',');
                }
                LableManageIn lableManageIn = new LableManageIn();
                lableManageIn.AndAlso(a => a.IsDelete == false);
                var LableList = await _lableManageService.LableManageList(lableManageIn);
                YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
                yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
                var DoctorList = await _YaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);
                var resultAll = 0;
                StringBuilder LableNameList = new StringBuilder();
                string DoctorName = string.Empty;
                if (LableArray != null && LableArray.Length > 0)
                {
                    for (int a = 0; a < LableArray.Length; a++)
                    {
                        DoctorRelation doctorRelation = new DoctorRelation();
                        doctorRelation.DoctorID = input.DoctorID;
                        var DoctorInfo = DoctorList.Where(t => t.Id == input.DoctorID).FirstOrDefault();
                        if (DoctorInfo != null)
                        {
                            doctorRelation.DoctorName = DoctorInfo.DoctorName;
                            DoctorName = DoctorInfo.DoctorName;
                            var LableInfo = LableList.Where(t => t.Id == int.Parse(LableArray[a].ToString())).FirstOrDefault();
                            if (LableInfo != null)
                            {
                                doctorRelation.LableID = LableInfo.Id;
                                doctorRelation.LableName = LableInfo.LableName;
                                doctorRelation.LableJSON = JsonHelper.ToJson(LableInfo);
                                doctorRelation.CreatedBy = userid;
                                doctorRelation.CreatedOn = DateTime.Now;
                                var res = await _DoctorRelationService.CreateDoctorRelation(doctorRelation);
                                resultAll = +res.Id;
                            }
                        }
                        LableNameList.Append(doctorRelation.LableName + ',');
                    }
                    #region 增加医生与标签关系 
                    RelationLabel relationLabel = new RelationLabel();
                    relationLabel.RelationCode = "Doctor";
                    relationLabel.BusinessID = input.DoctorID;
                    relationLabel.BusinessName = DoctorName;
                    relationLabel.LableID = input.LableIDJSON;
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
                    OperExplain = "CreateDoctorRelation",
                    OperContent = JsonHelper.ToJson(input),
                    OperType = "CreateDoctorRelation",
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
        /// 根据医生ID查询
        /// </summary>
        /// <param name="DoctorRelationInList"></param>
        /// <returns></returns>
        [Route("api/DoctorRelationListByDoctorID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRelationListByDoctorID([FromBody]DoctorRelationIn DoctorRelationInList)
        {
            if (!Commons.CheckSecret(DoctorRelationInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorRelationInList.AndAlso(t => !t.IsDelete);
            if (DoctorRelationInList.DoctorID > 0)
            {
                DoctorRelationInList.AndAlso(t => t.DoctorID == DoctorRelationInList.DoctorID);
            }
            var value = await _DoctorRelationService.DoctorRelationList(DoctorRelationInList);
            if (value.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                DoctorRelationIn doctorRelation = new DoctorRelationIn();
                if (value.Count > 0)
                {
                    foreach (var DoctorRelationInfo in value)
                    {
                        doctorRelation.Id = DoctorRelationInfo.Id;
                        doctorRelation.DoctorName = DoctorRelationInfo.DoctorName;
                        doctorRelation.DoctorID = DoctorRelationInfo.DoctorID;
                        doctorRelation.LableIDJSON += DoctorRelationInfo.LableID + ",";
                    }
                }
                this.ObjectResultModule.Object = doctorRelation;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorRelationListByDoctorID",
                OperContent = JsonHelper.ToJson(DoctorRelationInList),
                OperType = "DoctorRelationListByDoctorID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取医生与标签关系Page
        /// </summary>
        /// <param name="DoctorRelationInPage"> DoctorRelationInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorRelationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRelationPage([FromBody]DoctorRelationIn DoctorRelationInPage)
        {
            if (!Commons.CheckSecret(DoctorRelationInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorRelationInPage.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorRelationInPage.DoctorName))
            {
                DoctorRelationInPage.AndAlso(t => t.DoctorName.Contains(DoctorRelationInPage.DoctorName));
            }
            if (DoctorRelationInPage.DoctorID > 0)
            {
                DoctorRelationInPage.AndAlso(t => t.DoctorID == DoctorRelationInPage.DoctorID);
            }
            var values = await _DoctorRelationService.DoctorRelationPage(DoctorRelationInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorRelationOut(values, DoctorRelationInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorRelationPage",
                OperContent = JsonHelper.ToJson(DoctorRelationInPage),
                OperType = "DoctorRelationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生与标签关系List 
        /// </summary>
        /// <param name="DoctorRelationInList"> DoctorRelationInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorRelationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorRelationList([FromBody]DoctorRelationIn DoctorRelationInList)
        {
            if (!Commons.CheckSecret(DoctorRelationInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorRelationInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorRelationInList.DoctorName))
            {
                DoctorRelationInList.AndAlso(t => t.DoctorName.Contains(DoctorRelationInList.DoctorName));
            }
            if (DoctorRelationInList.DoctorID > 0)
            {
                DoctorRelationInList.AndAlso(t => t.DoctorID == DoctorRelationInList.DoctorID);
            }
            var value = await _DoctorRelationService.DoctorRelationList(DoctorRelationInList);
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
                OperExplain = "DoctorRelationList",
                OperContent = JsonHelper.ToJson(DoctorRelationInList),
                OperType = "DoctorRelationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取咨询疾病类型标签信息
        /// </summary>
        /// <param name="DoctorRelationInList"> DoctorRelationInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorConsultationRelationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorConsultationRelationList([FromBody]DoctorRelationIn DoctorRelationInList)
        {
            if (!Commons.CheckSecret(DoctorRelationInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _YaeherDoctorService.YaeherDoctorByID(DoctorRelationInList.DoctorID);
            if (doctor == null) { { return new ObjectResultModule("", 204, "NoContent"); } }
            DoctorRelationInList.AndAlso(t => !t.IsDelete);
            var label = new LabelDoctorManageIn() { DoctorID = doctor.Id };
            var value = await _lableManageService.LableDoctorManageInList(label);
            //var value = new List<LabelDoctorManage>();
            var labelin = new LableManageIn();
            labelin.AndAlso(t => !t.IsDelete && t.LableName == "其它");
            var otherlabel = await _lableManageService.LableManageByName(labelin);
            value.Add(new LabelDoctorManage()
            {
                CreatedOn = otherlabel.CreatedOn,
                CreatedBy = otherlabel.CreatedBy,
                ModifyBy = otherlabel.ModifyBy,
                ModifyOn = otherlabel.ModifyOn,
                LableName = otherlabel.LableName,
                LableRemark = otherlabel.LableRemark,
                DoctorID = DoctorRelationInList.DoctorID,
                Id = otherlabel.Id
            });
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
                OperExplain = "DoctorConsultationRelationList",
                OperContent = JsonHelper.ToJson(DoctorRelationInList),
                OperType = "DoctorConsultationRelationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 更新医生与标签关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorRelation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorRelation([FromBody] DoctorRelationIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorRelationService.DoctorRelationByID(input.Id);
            if (query != null)
            {
                query.DoctorName = input.DoctorName;
                query.DoctorID = input.DoctorID;
                query.LableID = input.LableID;
                query.LableName = input.LableName;
                query.LableJSON = input.LableJSON;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _DoctorRelationService.UpdateDoctorRelation(query);

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
                OperExplain = "UpdateDoctorRelation",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorRelation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除医生与标签关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorRelation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorRelation([FromBody] DoctorRelation input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorRelationService.DoctorRelationByID(input.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _DoctorRelationService.DeleteDoctorRelation(query);

                #region 删除医生与标签关系 
                RelationLabelListIn relationLabelListIn = new RelationLabelListIn();
                relationLabelListIn.AndAlso(a => a.LableID == res.LableID);
                relationLabelListIn.AndAlso(a => a.BusinessID == res.DoctorID);
                relationLabelListIn.AndAlso(t => t.IsDelete == false);
                var DoctorLable = await _relationLabelListService.RelationLabelListList(relationLabelListIn);
                if (DoctorLable != null && DoctorLable.Count > 0)
                {
                    var DoctorLableInfo = DoctorLable.ToList().FirstOrDefault();
                    DoctorLableInfo.DeleteBy = userid;
                    DoctorLableInfo.DeleteTime = DateTime.Now;
                    DoctorLableInfo.IsDelete = true;
                    var Lableresul = await _relationLabelListService.DeleteRelationLabelList(DoctorLableInfo);
                }
                #endregion

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
                OperExplain = "DeleteDoctorRelation",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteDoctorRelation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        #endregion

        #region 医生申请上传文件
        /// <summary>
        /// 新增医生申请上传文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorFileApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorFileApply([FromBody] DoctorFileApply input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new DoctorFileApply()
            {
                DoctorName = input.DoctorName,
                DoctorID = input.DoctorID,
                DocumentsUse = input.DocumentsUse,
                FileType = input.FileType,
                Address = input.Address,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _DoctorFileApplyService.CreateDoctorFileApply(create);
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
                OperExplain = "CreateDoctorFileApply",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateDoctorFileApply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生申请上传文件Page
        /// </summary>
        /// <param name="DoctorFileApplyInPage"> DoctorFileApplyInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorDoctorFileApplyPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorDoctorFileApplyPage([FromBody]DoctorFileApplyIn DoctorFileApplyInPage)
        {
            if (!Commons.CheckSecret(DoctorFileApplyInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorFileApplyInPage.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorFileApplyInPage.DoctorName))
            {
                DoctorFileApplyInPage.AndAlso(t => t.DoctorName.Contains(DoctorFileApplyInPage.DoctorName));
            }
            var values = await _DoctorFileApplyService.DoctorFileApplyPage(DoctorFileApplyInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorFileApplyOut(values, DoctorFileApplyInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorDoctorFileApplyPage",
                OperContent = JsonHelper.ToJson(DoctorFileApplyInPage),
                OperType = "DoctorDoctorFileApplyPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生申请上传文件List 
        /// </summary>
        /// <param name="DoctorFileApplyInList"> DoctorFileApplyInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorDoctorFileApplyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorDoctorFileApplyList([FromBody]DoctorFileApplyIn DoctorFileApplyInList)
        {
            if (!Commons.CheckSecret(DoctorFileApplyInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DoctorFileApplyInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorFileApplyInList.DoctorName))
            {
                DoctorFileApplyInList.AndAlso(t => t.DoctorName.Contains(DoctorFileApplyInList.DoctorName));
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            if ((!usermanager.IsAdmin && usermanager.IsDoctor) || usermanager.MobileRoleName == "doctor")
            {
                DoctorFileApplyInList.AndAlso(a => a.CreatedBy == userid);
            }
            var value = await _DoctorFileApplyService.DoctorFileApplyList(DoctorFileApplyInList);
            DoctorFileApplyList doctorFileApplyList = new DoctorFileApplyList();
            if (value != null)
            {
                doctorFileApplyList.doctorFileApplies = value;
            }
            doctorFileApplyList.AuthCheckRes = doctor.AuthCheckRes;
            doctorFileApplyList.AuthType = doctor.AuthType;
            doctorFileApplyList.DoctorID = doctor.Id;
            doctorFileApplyList.DoctorName = doctor.DoctorName;
            if (value.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = doctorFileApplyList;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorDoctorFileApplyList",
                OperContent = JsonHelper.ToJson(DoctorFileApplyInList),
                OperType = "DoctorDoctorFileApplyList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新医生申请上传文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorFileApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorFileApply([FromBody] DoctorFileApply input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorFileApplyService.DoctorFileApplyByID(input.Id);
            if (query != null)
            {
                query.DoctorName = input.DoctorName;
                query.DoctorID = input.DoctorID;
                query.DocumentsUse = input.DocumentsUse;
                query.FileType = input.FileType;
                query.Address = input.Address;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _DoctorFileApplyService.UpdateDoctorFileApply(query);

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
                OperExplain = "UpdateDoctorFileApply",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorFileApply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除医生申请上传文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorFileApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorFileApply([FromBody] DoctorFileApply input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorFileApplyService.DoctorFileApplyByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _DoctorFileApplyService.DeleteDoctorFileApply(query);

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
                OperExplain = "DeleteDoctorFileApply",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteDoctorFileApply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 创建及修改附件 身份证 职业证 资格证
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorFileApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorFileApply([FromBody]DoctorFileApplyIn DoctorFileApplyInfo)
        {
            if (!Commons.CheckSecret(DoctorFileApplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var paramlist = await _systemParameterService.ParameterList(param);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if (DoctorFileApplyInfo.Id > 0)
                {
                    var file = await _DoctorFileApplyService.DoctorFileApplyByID(DoctorFileApplyInfo.Id);
                    if (file != null)
                    {
                        file.IsDelete = true;
                        file.DeleteBy = userid;
                        file.DeleteTime = DateTime.Now;
                        await _DoctorFileApplyService.DeleteDoctorFileApply(file);
                    }
                }
                var att = new DoctorFileApply();
                if (!string.IsNullOrEmpty(DoctorFileApplyInfo.TypeDetail))
                {
                    att.TypeDetail = DoctorFileApplyInfo.TypeDetail;
                }
                att.Address = string.IsNullOrEmpty(DoctorFileApplyInfo.Address) ? "" : paramlist[0].ItemValue + "/" + DoctorFileApplyInfo.Address;
                att.DocumentsUse = DoctorFileApplyInfo.DocumentsUse;
                att.FileType = DoctorFileApplyInfo.FileType;

                var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
                att.DoctorID = doctor.Id;
                att.DoctorName = doctor.DoctorName;
                att.CreatedBy = userid;
                var res = await _DoctorFileApplyService.CreateDoctorFileApply(att);
                this.ObjectResultModule.Object = res;
                unitOfWork.Complete();
            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorFileApply",
                OperContent = JsonHelper.ToJson(DoctorFileApplyInfo),
                OperType = "DoctorFileApply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 创建附件 身份证 职业证 资格证
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorFileApplyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorFileApplyList([FromBody]DoctorFileApplyIn DoctorFileApplyInfo)
        {
            if (!Commons.CheckSecret(DoctorFileApplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //获取用户基本信息
            DoctorFileApplyInfo.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            if (!string.IsNullOrEmpty(DoctorFileApplyInfo.DocumentsUse))
            {
                DoctorFileApplyInfo.AndAlso(t => t.DocumentsUse == DoctorFileApplyInfo.DocumentsUse);
            }
            if (!string.IsNullOrEmpty(DoctorFileApplyInfo.FileType))
            {
                DoctorFileApplyInfo.AndAlso(t => t.FileType == DoctorFileApplyInfo.FileType);
            }
            if (!string.IsNullOrEmpty(DoctorFileApplyInfo.TypeDetail))
            {
                DoctorFileApplyInfo.AndAlso(t => t.TypeDetail == DoctorFileApplyInfo.TypeDetail);
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorFileApplyInfo.StartTime))
            {
                StartTime = DateTime.Parse(DoctorFileApplyInfo.StartTime);
                if (string.IsNullOrEmpty(DoctorFileApplyInfo.EndTime))
                {
                    DoctorFileApplyInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorFileApplyInfo.EndTime))
            {
                EndTime = DateTime.Parse(DoctorFileApplyInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorFileApplyInfo.StartTime))
            {
                DoctorFileApplyInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            var values = await _DoctorFileApplyService.DoctorFileApplyList(DoctorFileApplyInfo);
            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorFileApplyList",
                OperContent = JsonHelper.ToJson(DoctorFileApplyInfo),
                OperType = "DoctorFileApplyList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }


        /// <summary>
        /// 新增医生申请上传文件List
        /// </summary>
        /// <param name="DoctorFileApplyListInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorFileList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorFileList([FromBody] DoctorFileApplyList DoctorFileApplyListInfo)
        {
            if (!Commons.CheckSecret(DoctorFileApplyListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            //if (usermanager.MobileRoleName!="doctor")
            //{
            //    return new ObjectResultModule("",400,"非医生用户不允许申请上传文件!");
            //}
            //获取用户基本信息
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);

            var resut = 0;
            if (DoctorFileApplyListInfo != null)
            {
                DoctorFileApplyIn doctorFileApplyIn = new DoctorFileApplyIn();
                doctorFileApplyIn.AndAlso(a => a.IsDelete == false);
                doctorFileApplyIn.AndAlso(a => a.DoctorID == DoctorFileApplyListInfo.DoctorID && a.DocumentsUse == "register" && a.FileType == "certificate");
                var FileApplyList = await _DoctorFileApplyService.DoctorFileApplyList(doctorFileApplyIn);
                if (FileApplyList != null)
                {
                    foreach (var FileApplyInfo in FileApplyList)
                    {
                        FileApplyInfo.IsDelete = true;
                        FileApplyInfo.DeleteBy = userid;
                        FileApplyInfo.DeleteTime = DateTime.Now;
                        var resFile = await _DoctorFileApplyService.DeleteDoctorFileApply(FileApplyInfo);
                    }
                }
                if (DoctorFileApplyListInfo.doctorFileApplies.Count > 0)
                {
                    foreach (var FileApplyInfo in DoctorFileApplyListInfo.doctorFileApplies)
                    {
                        DoctorFileApply create = new DoctorFileApply();
                        create.DoctorName = doctor.DoctorName;
                        create.DoctorID = doctor.Id;
                        create.DocumentsUse = FileApplyInfo.DocumentsUse;
                        create.FileType = FileApplyInfo.FileType;
                        create.TypeDetail = FileApplyInfo.TypeDetail;
                        create.Address = FileApplyInfo.Address;
                        create.CreatedBy = userid;
                        create.CreatedOn = DateTime.Now;
                        var res = await _DoctorFileApplyService.CreateDoctorFileApply(create);
                        resut += res.Id;
                    }
                }
            }
            if (resut > 0)
            {
                YaeherDoctor yaeherDoctor = doctor;
                yaeherDoctor.AuthType = DoctorFileApplyListInfo.AuthType;
                yaeherDoctor.AuthCheckRes = DoctorFileApplyListInfo.AuthCheckRes;
                var DoctorInfo = await _YaeherDoctorService.UpdateYaeherDoctor(yaeherDoctor);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = DoctorInfo;
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
                OperExplain = "CreateDoctorFileList",
                OperContent = JsonHelper.ToJson(DoctorFileApplyListInfo),
                OperType = "CreateDoctorFileList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        #endregion

        #region 医生申请门诊
        /// <summary>
        /// 新增医生申请门诊
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DoctorClinicApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorClinicApply([FromBody] DoctorClinicApplyIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            var clinic = await _clinicInfomationService.ClinicInfomationByID(input.ClinicID);
            var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var paramlist = await _systemParameterService.ParameterList(param);
            var doctorclinic = new DoctorClinicApplyIn();
            doctorclinic.AndAlso(t => !t.IsDelete && t.DoctorID == doctor.Id && t.ClinicID == clinic.Id);
            var cliniList = await _DoctorClinicApplyService.DoctorClinicApplyList(doctorclinic);
            if ((usermanager.MobileRoleName == "doctor") || (!usermanager.IsAdmin && usermanager.IsDoctor))
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    //修改
                    if (cliniList != null && cliniList.Count() > 0)
                    {
                        #region 历史版本
                        ////审核单
                        //var value = await _DoctorClinicApplyService.DoctorClinicApplyByID(input.Id);
                        ////审核附件表
                        //var newfile = new DoctorFileApplyIn();
                        //newfile.AndAlso(t => !t.IsDelete && t.CreatedBy == userid && t.FileType == "certificate" && t.DoctorClinicApplyId == value.Id);
                        //var DoctorFileApplylist = await _DoctorFileApplyService.DoctorFileApplyList(newfile);

                        //var QualifiFile = DoctorFileApplylist.FirstOrDefault(t => t.TypeDetail == "Qualificationcertificate");//附件表1
                        //var CertifiDoctorFileApplylist = DoctorFileApplylist.FirstOrDefault(t => t.TypeDetail == "Certificateofpractice");//附件表2

                        //if (paramlist[0].ItemValue + "/" + input.Qualificationcertificate == QualifiFile.Address && paramlist[0].ItemValue + "/" + input.Certificateofpractice == CertifiDoctorFileApplylist.Address)
                        //{
                        //    return new ObjectResultModule("", 400, "请不要提交未修改数据！");
                        //}
                        //if (value.CheckRes != "success")
                        //{
                        //    new ObjectResultModule("", 400, "未审核数据不可以提交修改!");
                        //}
                        //if (value.CreatedBy != userid)
                        //{
                        //    new ObjectResultModule("", 400, "只允许修改自己创建的数据!");
                        //}
                        //if (input.ClinicID > 0 && value.ClinicID != input.ClinicID)
                        //{
                        //    value.ClinicID = input.ClinicID;
                        //}
                        #endregion

                        // 删除历史申请信息
                        foreach (var DoctorClinincInfo in cliniList)
                        {
                            DoctorClinincInfo.IsDelete = true;
                            DoctorClinincInfo.DeleteBy = userid;
                            DoctorClinincInfo.DeleteTime = DateTime.Now;
                            var DeleteDoctorClinic = await _DoctorClinicApplyService.DeleteDoctorClinicApply(DoctorClinincInfo);
                        }
                        // 增加新的申请信息
                        var create = new DoctorClinicApply()
                        {
                            DoctorName = doctor.DoctorName,
                            DoctorID = doctor.Id,
                            DoctorJSON = JsonHelper.ToJson(doctor),
                            ApplyType = input.ApplyType,
                            ClinicID = clinic.Id,
                            ClinicName = clinic.ClinicName,
                            ClinicJSON = JsonHelper.ToJson(clinic),
                            ApplyRemark = input.ApplyRemark,
                            CheckRes = "checking",//默认审核中
                            CreatedBy = userid,
                            CreatedOn = DateTime.Now
                        };
                        var createDoctorClininc = await _DoctorClinicApplyService.CreateDoctorClinicApply(create);
                        // 删除历史科室与医生关系
                        ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
                        clinicDoctorReltionIn.AndAlso(t => !t.IsDelete && t.DoctorID == doctor.Id && t.ClinicID == clinic.Id);
                        var clinicdoctorrel = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
                        if (clinicdoctorrel != null && clinicdoctorrel.Count > 0)
                        {
                            foreach (var item in clinicdoctorrel)
                            {
                                item.IsDelete = true;
                                item.DeleteBy = userid;
                                item.DeleteTime = DateTime.Now;
                                await _clinicDoctorReltionService.DeleteClinicDoctorReltion(item);
                            }
                        }

                        // 更新资格证书
                        DoctorFileApplyIn Qualificationcertificate = new DoctorFileApplyIn();
                        Qualificationcertificate.AndAlso(a => a.IsDelete);
                        Qualificationcertificate.AndAlso(a => a.DoctorClinicApplyId == clinic.Id && a.DoctorID == doctor.Id);
                        Qualificationcertificate.AndAlso(a => a.DocumentsUse == input.ApplyType && a.TypeDetail == "Qualificationcertificate" && a.FileType == "certificate");
                        var QualifiFileList = await _DoctorFileApplyService.DoctorFileApplyList(Qualificationcertificate);
                        if (QualifiFileList != null)
                        {
                            foreach (var QualifiFile in QualifiFileList)
                            {
                                QualifiFile.IsDelete = true;
                                QualifiFile.DeleteBy = userid;
                                QualifiFile.DeleteTime = DateTime.Now;
                                await _DoctorFileApplyService.UpdateDoctorFileApply(QualifiFile);
                            }
                        }
                        var Quaapplyfileadd = new DoctorFileApply();
                        Quaapplyfileadd.DocumentsUse = input.ApplyType;
                        Quaapplyfileadd.FileType = "certificate";
                        Quaapplyfileadd.Address = paramlist[0].ItemValue + "/" + input.Qualificationcertificate;
                        Quaapplyfileadd.TypeDetail = "Qualificationcertificate";
                        Quaapplyfileadd.DoctorID = doctor.Id;
                        Quaapplyfileadd.DoctorName = doctor.DoctorName;
                        Quaapplyfileadd.DoctorClinicApplyId = createDoctorClininc.Id;
                        Quaapplyfileadd.CreatedBy = userid;
                        var Quaapply = await _DoctorFileApplyService.CreateDoctorFileApply(Quaapplyfileadd);
                        // 更新执业证
                        DoctorFileApplyIn Certificateofpractice = new DoctorFileApplyIn();
                        Certificateofpractice.AndAlso(a => a.IsDelete);
                        Certificateofpractice.AndAlso(a => a.DoctorClinicApplyId == clinic.Id && a.DoctorID == doctor.Id);
                        Certificateofpractice.AndAlso(a => a.DocumentsUse == input.ApplyType && a.TypeDetail == "Certificateofpractice" && a.FileType == "certificate");
                        var CertifiDoctorFileApplylist = await _DoctorFileApplyService.DoctorFileApplyList(Certificateofpractice);
                        if (CertifiDoctorFileApplylist != null)
                        {
                            foreach (var CertifiDoctorFileApply in CertifiDoctorFileApplylist)
                            {
                                CertifiDoctorFileApply.IsDelete = true;
                                CertifiDoctorFileApply.DeleteBy = userid;
                                CertifiDoctorFileApply.DeleteTime = DateTime.Now;
                                await _DoctorFileApplyService.UpdateDoctorFileApply(CertifiDoctorFileApply);
                            }
                        }

                        var Certifapplyfileadd = new DoctorFileApply();
                        Certifapplyfileadd.DocumentsUse = input.ApplyType;
                        Certifapplyfileadd.FileType = "certificate";
                        Certifapplyfileadd.Address = paramlist[0].ItemValue + "/" + input.Certificateofpractice;
                        Certifapplyfileadd.TypeDetail = "Certificateofpractice";
                        Certifapplyfileadd.DoctorID = doctor.Id;
                        Certifapplyfileadd.DoctorName = doctor.DoctorName;
                        Certifapplyfileadd.CreatedBy = userid;
                        Certifapplyfileadd.DoctorClinicApplyId = createDoctorClininc.Id;
                        var Certifapply = await _DoctorFileApplyService.CreateDoctorFileApply(Certifapplyfileadd);
                    }
                    else//新增
                    {
                        var create = new DoctorClinicApply()
                        {
                            DoctorName = doctor.DoctorName,
                            DoctorID = doctor.Id,
                            DoctorJSON = JsonHelper.ToJson(doctor),
                            ApplyType = input.ApplyType,
                            ClinicID = clinic.Id,
                            ClinicName = clinic.ClinicName,
                            ClinicJSON = JsonHelper.ToJson(clinic),
                            ApplyRemark = input.ApplyRemark,
                            CheckRes = "checking",//默认审核中
                            CreatedBy = userid,
                            CreatedOn = DateTime.Now
                        };
                        //已审核数据提交资料修改,原有的科室医生关联关系删除
                        ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
                        clinicDoctorReltionIn.AndAlso(t => !t.IsDelete && t.DoctorID == doctor.Id && t.ClinicID == clinic.Id);
                        var clinicdoctorrel = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
                        if (clinicdoctorrel != null && clinicdoctorrel.Count > 0)
                        {
                            foreach (var item in clinicdoctorrel)
                            {
                                item.IsDelete = true;
                                item.DeleteBy = userid;
                                item.DeleteTime = DateTime.Now;
                                await _clinicDoctorReltionService.DeleteClinicDoctorReltion(item);
                            }
                        }
                        var res = await _DoctorClinicApplyService.CreateDoctorClinicApply(create);
                        if (!string.IsNullOrEmpty(input.Qualificationcertificate))
                        {
                            var applyfile = new DoctorFileApply();
                            applyfile.DocumentsUse = input.ApplyType;
                            applyfile.FileType = "certificate";
                            applyfile.Address = paramlist[0].ItemValue + "/" + input.Qualificationcertificate;
                            applyfile.TypeDetail = "Qualificationcertificate";
                            applyfile.DoctorID = doctor.Id;
                            applyfile.DoctorName = doctor.DoctorName;
                            applyfile.CreatedBy = userid;
                            applyfile.DoctorClinicApplyId = res.Id;
                            var apply = await _DoctorFileApplyService.CreateDoctorFileApply(applyfile);
                        }
                        if (!string.IsNullOrEmpty(input.Certificateofpractice))
                        {
                            var applyfile = new DoctorFileApply();
                            applyfile.DocumentsUse = input.ApplyType;
                            applyfile.FileType = "certificate";
                            applyfile.Address = paramlist[0].ItemValue + "/" + input.Certificateofpractice;
                            applyfile.TypeDetail = "Certificateofpractice";
                            applyfile.DoctorID = doctor.Id;
                            applyfile.DoctorName = doctor.DoctorName;
                            applyfile.CreatedBy = userid;
                            applyfile.DoctorClinicApplyId = res.Id;
                            var apply = await _DoctorFileApplyService.CreateDoctorFileApply(applyfile);
                        }
                        this.ObjectResultModule.Object = res;
                    }
                    unitOfWork.Complete();
                }
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorClinicApply",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DoctorClinicApply",
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
        /// 获取医生申请门诊Page
        /// </summary>
        /// <param name="DoctorClinicApplyInPage"> DoctorClinicApplyInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorClinicApplyPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorClinicApplyPage([FromBody]DoctorClinicApplyIn DoctorClinicApplyInPage)
        {
            if (!Commons.CheckSecret(DoctorClinicApplyInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var param1 = new SystemParameterIn() { Type = "ConfigPar" };
            param1.AndAlso(t => !t.IsDelete && t.SystemCode == "QualityCheckApply");
            var typelist1 = await _systemParameterService.ParameterList(param1);
            DoctorClinicApplyInPage.AndAlso(t => !t.IsDelete);
            if (usermanager.MobileRoleName == "doctor")
            {
                DoctorClinicApplyInPage.AndAlso(t => t.CreatedBy == userid);
            }
            if (!string.IsNullOrEmpty(DoctorClinicApplyInPage.KeyWord))
            {
                DoctorClinicApplyInPage.AndAlso(t => t.DoctorName.Contains(DoctorClinicApplyInPage.KeyWord));
            }
            var values = await _DoctorClinicApplyService.DoctorClinicApplyOutDetailPage(DoctorClinicApplyInPage);
            this.ObjectResultModule.Object = new DoctorClinicApplyOut(values, DoctorClinicApplyInPage, typelist1);
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorClinicApplyPage",
                OperContent = JsonHelper.ToJson(DoctorClinicApplyInPage),
                OperType = "DoctorClinicApplyPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取医生所有门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInPage"> DoctorClinicApplyInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorClinicApplyOut")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorClinicApplyOut([FromBody]DoctorClinicApplyIn DoctorClinicApplyInPage)
        {
            if (!Commons.CheckSecret(DoctorClinicApplyInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var param1 = new SystemParameterIn() { Type = "ConfigPar" };
            param1.AndAlso(t => !t.IsDelete && t.SystemCode == "QualityCheckApply");
            var typelist1 = await _systemParameterService.ParameterList(param1);

            var clinicdoctor = new ClinicDoctorReltionIn();

            DoctorClinicApplyInPage.AndAlso(t => !t.IsDelete);
            if (usermanager.MobileRoleName == "doctor")
            {
                if (DoctorClinicApplyInPage.Platform == "PC")
                {
                    clinicdoctor.AndAlso(t => t.DoctorID == usermanager.DoctorID);
                }
                else if (DoctorClinicApplyInPage.Platform == "Mobile")
                {
                    var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
                    clinicdoctor.AndAlso(t => t.DoctorID == doctor.Id);
                }
                DoctorClinicApplyInPage.AndAlso(t => t.CreatedBy == userid);
            }
            if (usermanager.MobileRoleName == "customerservice")
            {
                clinicdoctor.AndAlso(t => !t.IsDelete && t.DoctorID == DoctorClinicApplyInPage.DoctorId);
            }
            //医生本身已经在的科室列表
            var clinicrela = await _clinicDoctorReltionService.ClinicDoctorReltionApplyList(clinicdoctor);
            //医生申请的科室列表
            var values = await _DoctorClinicApplyService.DoctorClinicApplyOutDetailList(DoctorClinicApplyInPage);
            var Detaila = new List<DoctorClinicApplyOutDetail>();
            if (clinicrela != null && clinicrela.Count > 0)
            {
                foreach (var item in clinicrela)
                {
                    if (values.Find(t => t.ClinicID == item.ClinicID) == null)
                    {
                        Detaila.Add(item);
                    }
                }
            }
            List<DoctorClinicApplyOutDetail> Result = values.Union(Detaila).ToList<DoctorClinicApplyOutDetail>();

            this.ObjectResultModule.Object = Result;
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorClinicApplyOut",
                OperContent = JsonHelper.ToJson(DoctorClinicApplyInPage),
                OperType = "DoctorClinicApplyOut",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取医生申请门诊详情
        /// </summary>
        /// <param name="DoctorClinicApplyInPage"> DoctorClinicApplyInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorClinicApplyById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorClinicApplyById([FromBody]DoctorClinicApplyIn DoctorClinicApplyInPage)
        {
            if (!Commons.CheckSecret(DoctorClinicApplyInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param1 = new SystemParameterIn() { Type = "ConfigPar" };
            param1.AndAlso(t => !t.IsDelete && t.SystemCode == "QualityCheckApply");
            var typelist1 = await _systemParameterService.ParameterList(param1);

            DoctorClinicApplyInPage.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            var values = await _DoctorClinicApplyService.DoctorClinicApplyByID(DoctorClinicApplyInPage.Id);
            var fila = new DoctorFileApplyIn();
            fila.AndAlso(t => !t.IsDelete && t.DoctorClinicApplyId == DoctorClinicApplyInPage.Id);

            var applyfile = await _DoctorFileApplyService.DoctorFileApplyList(fila);

            var doctor = await _YaeherDoctorService.YaeherDoctorByID(values.DoctorID);
            var doctoruser = await _yaeherUserService.YaeherUserByID(doctor.UserID);

            this.ObjectResultModule.Object = new DoctorClinicApplyOutDetail(values, doctor, doctoruser, typelist1, applyfile.ToList());

            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorClinicApplyById",
                OperContent = JsonHelper.ToJson(DoctorClinicApplyInPage),
                OperType = "DoctorClinicApplyById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取医生申请门诊List 
        /// </summary>
        /// <param name="DoctorClinicApplyInList"> DoctorClinicApplyInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorClinicApplyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorClinicApplyList([FromBody]DoctorClinicApplyIn DoctorClinicApplyInList)
        {
            if (!Commons.CheckSecret(DoctorClinicApplyInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorClinicApplyInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorClinicApplyInList.DoctorName))
            {
                DoctorClinicApplyInList.AndAlso(t => t.DoctorName.Contains(DoctorClinicApplyInList.DoctorName));
            }

            var value = await _DoctorClinicApplyService.DoctorClinicApplyList(DoctorClinicApplyInList);
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
                OperExplain = "DoctorClinicApplyList",
                OperContent = JsonHelper.ToJson(DoctorClinicApplyInList),
                OperType = "DoctorClinicApplyList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 客服通过医生申请门诊
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorClinicApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorClinicApply([FromBody] DoctorClinicApplyIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var query = await _DoctorClinicApplyService.DoctorClinicApplyByID(input.Id);
            var clinic = await _clinicInfomationService.ClinicInfomationByID(query.ClinicID);
            if (query != null)
            {
                if (usermanager.MobileRoleName == "customerservice")
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        query.ClinicID = clinic.Id;
                        query.ClinicName = clinic.ClinicName;
                        query.ClinicJSON = JsonHelper.ToJson(clinic);
                        query.CheckTime = DateTime.Now;
                        query.CheckRemark = input.CheckRemark;
                        query.CheckRes = input.CheckRes;
                        query.ModifyOn = DateTime.Now;
                        query.ModifyBy = userid;
                        var res = await _DoctorClinicApplyService.UpdateDoctorClinicApply(query);
                        if (input.CheckRes == "success")
                        {
                            var relin = new ClinicDoctorReltionIn(); relin.AndAlso(t => !t.IsDelete && t.DoctorID == query.DoctorID && t.ClinicID == query.ClinicID);
                            var query1 = await _clinicDoctorReltionService.ClinicDoctorReltionList(relin);
                            if (query1.Count < 1)
                            {
                                var clinicdoctor = new ClinicDoctorReltion();
                                clinicdoctor.DoctorID = query.DoctorID;
                                clinicdoctor.ClinicID = query.ClinicID;
                                clinicdoctor.DoctorName = query.DoctorName;
                                clinicdoctor.ClinicName = query.ClinicName;
                                clinicdoctor.ClinicJSON = JsonHelper.ToJson(clinic);
                                clinicdoctor.DoctorJSON = JsonHelper.ToJson(query.DoctorJSON);
                                clinicdoctor.CreatedBy = userid;
                                await _clinicDoctorReltionService.CreateClinicDoctorReltion(clinicdoctor);
                            }
                        }
                        this.ObjectResultModule.Object = res;

                        unitOfWork.Complete();
                    }
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
                OperExplain = "UpdateDoctorClinicApply",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorClinicApply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除医生申请门诊
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorClinicApply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorClinicApply([FromBody] DoctorClinicApply input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorClinicApplyService.DoctorClinicApplyByID(input.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _DoctorClinicApplyService.DeleteDoctorClinicApply(query);

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
                OperExplain = "DeleteDoctorClinicApply",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteDoctorClinicApply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 医生提供服务费用表
        /// <summary>
        /// 获取提供服务费用表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Route("api/ServiceMoneyListType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ServiceMoneyListType([FromBody] SecretModel input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "ServiceMoneyListType");
            var paramlist = await _systemParameterService.ParameterList(param);

            var coderesult = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                coderesult.Add(newcode);
            }
            this.ObjectResultModule.Object = coderesult;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ServiceMoneyListType",
                OperContent = JsonHelper.ToJson(input),
                OperType = "ServiceMoneyListType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;

        }

        /// <summary>
        /// 新增医生提供服务费用表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateServiceMoneyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateServiceMoneyList([FromBody] ServiceMoneyList input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "ServiceMoneyListType");
            var paramlist = await _systemParameterService.ParameterList(param);

            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            var moneylist = new ServiceMoneyListIn();
            moneylist.AndAlso(t => t.DoctorID == doctor.Id && t.ServiceType == input.ServiceType && !t.IsDelete);
            var ml = await _ServiceMoneyListService.ServiceMoneyListList(moneylist);
            if (ml.Count > 0)
            {
                return new ObjectResultModule("", 100, "同类型数据只能新增一次！");
            }
            #region
            if (input.ServiceExpense < 1)
            {
                return new ObjectResultModule("", 400, "设置费用太低,请调整后重新申请！");
            }
            if (input.ServiceFrequency < 0)
            {
                return new ObjectResultModule("", 400, "设置服务次数太低,请调整后重新申请！");
            }
            if (input.ServiceType == "Phone" && input.ServiceDuration < 1)
            {
                return new ObjectResultModule("", 400, "设置时长太低,请调整后重新申请！");
            }
            #endregion
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var create = new ServiceMoneyList()
                {
                    DoctorName = doctor.DoctorName,
                    DoctorID = doctor.Id,
                    ServiceType = input.ServiceType,
                    ServiceTypeValue = paramlist.Find(t => t.Code == input.ServiceType).Name,
                    ServiceDuration = input.ServiceDuration,
                    ServiceExpense = input.ServiceExpense,
                    ServiceFrequency = input.ServiceFrequency,
                    ServiceState = input.ServiceState,
                    ServiceTime = input.ServiceTime,
                    CreatedBy = userid,
                    ModifyOn = DateTime.Now,
                    ModifyBy = userid,
                    CreatedOn = DateTime.Now,
                    ActualNumber = 0,  // 默认实际接单数为0
                };
                var res = await _ServiceMoneyListService.CreateServiceMoneyList(create);

                var createlog = new DoctorServiceLog()
                {
                    DoctorName = doctor.DoctorName,
                    DoctorID = doctor.Id,
                    ServiceType = paramlist.Find(t => t.Code == input.ServiceType).Name,
                    ServiceDuration = input.ServiceDuration,
                    ServiceExpense = input.ServiceExpense,
                    ServiceFrequency = input.ServiceFrequency,
                    ServiceTime = input.ServiceTime,
                    CreatedBy = userid,
                    ModifyOn = DateTime.Now,
                    ModifyBy = userid,
                    CreatedOn = DateTime.Now
                };
                var reslog = await _DoctorServiceLogService.CreateDoctorServiceLog(createlog);
                this.ObjectResultModule.Object = res;

                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateServiceMoneyList",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateServiceMoneyList",
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
        /// 获取医生提供服务费用表Page
        /// </summary>
        /// <param name="ServiceMoneyListInPage"> ServiceMoneyListIn 数据</param>
        /// <returns></returns>
        [Route("api/ServiceMoneyPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ServiceMoneyPage([FromBody]ServiceMoneyListIn ServiceMoneyListInPage)
        {
            if (!Commons.CheckSecret(ServiceMoneyListInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ServiceMoneyListInPage.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ServiceMoneyListInPage.DoctorName))
            {
                ServiceMoneyListInPage.AndAlso(t => t.DoctorName.Contains(ServiceMoneyListInPage.DoctorName));
            }
            var values = await _ServiceMoneyListService.ServiceMoneyListPage(ServiceMoneyListInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ServiceMoneyListOut(values, ServiceMoneyListInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ServiceMoneyPage",
                OperContent = JsonHelper.ToJson(ServiceMoneyListInPage),
                OperType = "ServiceMoneyPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 医生个人中心获取医生提供服务费用表List 
        /// </summary>
        /// <param name="ServiceMoneyListInList"> ServiceMoneyListInList 数据</param>
        /// <returns></returns>
        [Route("api/ServiceMoneyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ServiceMoneyList([FromBody]ServiceMoneyListIn ServiceMoneyListInList)
        {
            if (!Commons.CheckSecret(ServiceMoneyListInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            ServiceMoneyListInList.AndAlso(t => !t.IsDelete && t.DoctorID == doctor.Id);
            var value = await _ServiceMoneyListService.ServiceMoneyListList(ServiceMoneyListInList);
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
                OperExplain = "ServiceMoneyList",
                OperContent = JsonHelper.ToJson(ServiceMoneyListInList),
                OperType = "ServiceMoneyList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 更新医生提供服务费用表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateServiceMoneyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateServiceMoneyList([FromBody] ServiceMoneyList input)
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
            var query = await _ServiceMoneyListService.ServiceMoneyListByID(input.Id);

            var DoctorOnlineRecord = await _doctorOnlineRecordService.DoctorOnlineRecordDoctorID(doctor.Id);

            if (input.ServiceExpense < 1)
            {
                return new ObjectResultModule("", 400, "设置费用太低,请调整后重新申请！");
            }
            if (input.ServiceFrequency < 0)
            {
                return new ObjectResultModule("", 400, "设置服务次数太低,请调整后重新申请！");
            }
            if (query.ServiceType == "Phone" && input.ServiceDuration < 1)
            {
                return new ObjectResultModule("", 400, "设置时长太低,请调整后重新申请！");
            }

            if (input.ServiceExpense != query.ServiceExpense)
            {
                var moneytime = DoctorOnlineRecord.DoctorMoneyexTime;
                var moneybl = DoctorOnlineRecord.DoctorMoneyExchange;
                if (((moneybl / 100) + 1) * query.ServiceExpense < input.ServiceExpense)
                {
                    return new ObjectResultModule("", 400, "设置价格太高,请调整后重新申请！");
                }

                if (DateTime.Now < query.ModifyOn.AddDays(moneytime) && input.ServiceExpense != query.ServiceExpense)
                {
                    return new ObjectResultModule("", 400, "调整太频繁,请稍后重新申请！");
                }
            }
            if (query != null)
            {
                var param = new SystemParameterIn() { Type = "ConfigPar" };
                param.AndAlso(t => !t.IsDelete && t.SystemCode == "ServiceMoneyListType");
                var paramlist = await _systemParameterService.ParameterList(param);

                if (input.ServiceExpense != query.ServiceExpense)
                {
                    query.ModifyOn = DateTime.Now;
                    query.ModifyBy = userid;
                }
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    query.DoctorName = doctor.DoctorName;
                    query.DoctorID = doctor.Id;
                    query.ServiceDuration = input.ServiceDuration;
                    query.ServiceExpense = input.ServiceExpense;
                    query.ServiceFrequency = input.ServiceFrequency;
                    query.ServiceState = input.ServiceState;
                    query.ServiceTime = input.ServiceTime;

                    var res = await _ServiceMoneyListService.UpdateServiceMoneyList(query);

                    var createlog = new DoctorServiceLog()
                    {
                        DoctorName = doctor.DoctorName,
                        DoctorID = doctor.Id,
                        ServiceDuration = input.ServiceDuration,
                        ServiceExpense = input.ServiceExpense,
                        ServiceFrequency = input.ServiceFrequency,
                        ServiceTime = input.ServiceTime,
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now
                    };
                    var reslog = await _DoctorServiceLogService.CreateDoctorServiceLog(createlog);

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
                OperExplain = "UpdateServiceMoneyList",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateServiceMoneyList",
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
        [Route("api/ServiceMoneyListByID")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> ServiceMoneyListByID([FromBody] ServiceMoneyList input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _ServiceMoneyListService.ServiceMoneyListByID(input.Id);
            if (query != null)
            {
                this.ObjectResultModule.Object = query;
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
                OperExplain = "ServiceMoneyListByID",
                OperContent = JsonHelper.ToJson(input),
                OperType = "ServiceMoneyListByID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        #endregion

        #region 医生提供服务log

        /// <summary>
        /// 获取 医生提供服务logPage
        /// </summary>
        /// <param name="DoctorServiceLogInPage"> DoctorServiceLogInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorServiceLogPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorServiceLogPage([FromBody]DoctorServiceLogIn DoctorServiceLogInPage)
        {
            if (!Commons.CheckSecret(DoctorServiceLogInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DoctorServiceLogInPage.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorServiceLogInPage.DoctorName))
            {
                DoctorServiceLogInPage.AndAlso(t => t.DoctorName.Contains(DoctorServiceLogInPage.DoctorName));
            }
            var values = await _DoctorServiceLogService.DoctorServiceLogPage(DoctorServiceLogInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorServiceLogOut(values, DoctorServiceLogInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取 医生提供服务logList 
        /// </summary>
        /// <param name="DoctorServiceLogInList"> DoctorServiceLogInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorServiceLogList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorServiceLogList([FromBody]DoctorServiceLogIn DoctorServiceLogInList)
        {
            if (!Commons.CheckSecret(DoctorServiceLogInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DoctorServiceLogInList.AndAlso(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(DoctorServiceLogInList.DoctorName))
            {
                DoctorServiceLogInList.AndAlso(t => t.DoctorName.Contains(DoctorServiceLogInList.DoctorName));
            }

            var value = await _DoctorServiceLogService.DoctorServiceLogList(DoctorServiceLogInList);
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
            return this.ObjectResultModule;
        }

        #endregion

        #region 发布文章
        /// <summary>
        /// 新增 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DoctorPaperType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorPaperType([FromBody] DoctorPaper input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorPaperFrom");
            var paramlist = await _systemParameterService.ParameterList(param);

            var coderesult = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                coderesult.Add(newcode);
            }
            var param1 = new SystemParameterIn() { Type = "ConfigPar" };
            param1.AndAlso(t => !t.IsDelete && t.SystemCode == "DoctorPaperState");
            var paramlist1 = await _systemParameterService.ParameterList(param1);
            var coderesult1 = new List<CodeList>();
            foreach (var item in paramlist1)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                coderesult1.Add(newcode);
            }
            this.ObjectResultModule.Object = new DoctorPaperType(coderesult, coderesult1);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorPaperType",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DoctorPaperType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }


        /// <summary>
        /// 新增 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorPaper")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorPaper([FromBody] CreateDoctorPaper input)
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

            var baseurlpara = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var baseurlparamlist = await _systemParameterService.ParameterList(baseurlpara);
            var baseurlparam = baseurlparamlist.FirstOrDefault();

            var create = new DoctorPaper();
            create.DoctorName = doctor.DoctorName;
            create.DoctorID = doctor.Id;
            create.PaperTiltle = input.PaperTiltle;

            create.PaperFrom = input.PaperFrom;
            create.PaperAddress = input.PaperAddress;
            create.CheckState = input.CheckState;
            create.ConsultNumber = input.ConsultNumber;
            create.CreatedBy = userid;
            create.CreatedOn = DateTime.Now;
            #region 上传文件
            var UploadHtmlImageUrlList = input.PaperContent;
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(UploadHtmlImageUrlList);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;

            foreach (var item in sUrlList)
            {
                var baseurl = item.ToString();
                var filetype = baseurl.Substring(baseurl.LastIndexOf(".", StringComparison.Ordinal));
                System.Net.WebRequest webreq = System.Net.WebRequest.Create(baseurl);
                System.Net.WebResponse webres = webreq.GetResponse();
                var contentkey = baseurlparam.ItemValue + "/doctorpaper/image/" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + input.PaperFrom + filetype;
                using (System.IO.Stream stream = webres.GetResponseStream())
                {
                    var uploadresult = await _cosHandler.PutObjectAsync(contentkey, stream);
                }
                UploadHtmlImageUrlList = UploadHtmlImageUrlList.Replace(baseurl, contentkey);
            }
            create.PaperContent = UploadHtmlImageUrlList;
            #endregion 
            if (input.PaperFrom == "Consultation")
            {
                var consultation = await _consultationService.YaeherConsultationByID(input.ConsultID);
                create.ConsultNumber = consultation.ConsultNumber;
            }
            var res = await _DoctorPaperService.CreateDoctorPaper(create);
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
                OperExplain = "CreateDoctorPaper",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateDoctorPaper",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取 发布文章Page
        /// </summary>
        /// <param name="DoctorPaperInPage"> DoctorServiceLogInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorPaperPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorPaperPage([FromBody]DoctorPaperIn DoctorPaperInPage)
        {
            if (!Commons.CheckSecret(DoctorPaperInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());

            DoctorPaperInPage.AndAlso(t => t.IsDelete == false);
            if (DoctorPaperInPage.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor")
                {
                    DoctorPaperInPage.DoctorId = doctor.Id;
                    DoctorPaperInPage.AndAlso(t => t.DoctorID == doctor.Id);
                }
                else if (usermanager.MobileRoleName == "quality")
                {
                    DoctorPaperInPage.AndAlso(t => t.CheckState != "created");
                }
                else if (usermanager.MobileRoleName == "patient")
                {
                    DoctorPaperInPage.AndAlso(t => t.CheckState == "success");
                    if (DoctorPaperInPage.DoctorId > 0)
                    {
                        DoctorPaperInPage.AndAlso(t => t.DoctorID == DoctorPaperInPage.DoctorId);
                    }
                    //else
                    //{
                    //    return new ObjectResultModule("",400,"请选择某医生查看文章！");
                    //}
                }
            }
            else if (DoctorPaperInPage.Platform == "PC")
            {
                if (!usermanager.IsAdmin && usermanager.IsDoctor)
                {
                    DoctorPaperInPage.AndAlso(t => t.DoctorID == doctor.Id);
                }
            }
            if (!string.IsNullOrEmpty(DoctorPaperInPage.CheckState))
            {
                DoctorPaperInPage.AndAlso(t => t.CheckState == DoctorPaperInPage.CheckState);
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(DoctorPaperInPage.StartTime))
            {
                StartTime = DateTime.Parse(DoctorPaperInPage.StartTime);
                if (string.IsNullOrEmpty(DoctorPaperInPage.EndTime))
                {
                    DoctorPaperInPage.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(DoctorPaperInPage.EndTime))
            {
                EndTime = DateTime.Parse(DoctorPaperInPage.EndTime);
            }
            if (!string.IsNullOrEmpty(DoctorPaperInPage.StartTime))
            {
                DoctorPaperInPage.AndAlso(t => t.CreatedOn >= StartTime);
                DoctorPaperInPage.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(DoctorPaperInPage.KeyWord))
            {
                DoctorPaperInPage.AndAlso(t => t.DoctorName.Contains(DoctorPaperInPage.KeyWord) ||
                                               t.PaperTiltle.Contains(DoctorPaperInPage.KeyWord) ||
                                               t.PaperContent.Contains(DoctorPaperInPage.KeyWord));
            }
            var values = await _DoctorPaperService.DoctorPaperViewPage(DoctorPaperInPage);
            this.ObjectResultModule.Object = new DoctorPaperOut(values, DoctorPaperInPage);
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorPaperPage",
                OperContent = JsonHelper.ToJson(DoctorPaperInPage),
                OperType = "DoctorPaperPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取 发布文章ById
        /// </summary>
        /// <param name="DoctorPaperIn"> DoctorPaperIn 数据</param>
        /// <returns></returns>
        [Route("api/DoctorPaperById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorPaperById([FromBody]DoctorPaperIn DoctorPaperIn)
        {
            if (!Commons.CheckSecret(DoctorPaperIn.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _DoctorPaperService.DoctorPaperByID(DoctorPaperIn.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorPagerDetail(values);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorPaperById",
                OperContent = JsonHelper.ToJson(DoctorPaperIn),
                OperType = "DoctorPaperById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取 发布文章List 
        /// </summary>
        /// <param name="DoctorPaperInList"> DoctorServiceLogInList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorPaperList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorPaperList([FromBody]DoctorPaperIn DoctorPaperInList)
        {
            if (!Commons.CheckSecret(DoctorPaperInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorPaperInList.AndAlso(t => !t.IsDelete && t.Checker > 0);
            if (!string.IsNullOrEmpty(DoctorPaperInList.DoctorName))
            {
                DoctorPaperInList.AndAlso(t => t.DoctorName.Contains(DoctorPaperInList.DoctorName));
            }

            var value = await _DoctorPaperService.DoctorPaperList(DoctorPaperInList);
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
                OperExplain = "DoctorPaperList",
                OperContent = JsonHelper.ToJson(DoctorPaperInList),
                OperType = "DoctorPaperList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 更新 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorPaper")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorPaper([FromBody] DoctorPaperIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorPaperService.DoctorPaperByID(input.Id);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(input.PaperTiltle))
                {
                    query.PaperTiltle = input.PaperTiltle;
                }
                if (!string.IsNullOrEmpty(input.PaperContent))
                {
                    query.PaperContent = input.PaperContent;
                }
                if (!string.IsNullOrEmpty(input.PaperFrom))
                {
                    query.PaperFrom = input.PaperFrom;
                }
                if (!string.IsNullOrEmpty(input.ConsultNumber))
                {
                    query.ConsultNumber = input.ConsultNumber;
                }
                if (!string.IsNullOrEmpty(input.PaperAddress))
                {
                    query.PaperAddress = input.PaperAddress;
                }
                if (!string.IsNullOrEmpty(input.CheckRemark))
                {
                    query.CheckRemark = input.CheckRemark;
                }
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _DoctorPaperService.UpdateDoctorPaper(query);

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
                OperExplain = "UpdateDoctorPaper",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorPaper",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除 发布文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorPaper")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorPaper([FromBody] DoctorPaper input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _DoctorPaperService.DoctorPaperByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _DoctorPaperService.DeleteDoctorPaper(query);

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
                OperExplain = "DeleteDoctorPaper",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteDoctorPaper",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 推送医生文章
        /// </summary>
        /// <param name="DoctorPaperInfo"></param>
        /// <returns></returns>
        [Route("api/PushDoctorPaper")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PushDoctorPaper([FromBody] DoctorPaper DoctorPaperInfo)
        {
            if (!Commons.CheckSecret(DoctorPaperInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;


            // 查询文章
            var query = await _DoctorPaperService.DoctorPaperByID(DoctorPaperInfo.Id);

            if (query != null)
            {
                #region 去重判断
                ReleaseManageIn releaseManageIn = new ReleaseManageIn();
                releaseManageIn.AndAlso(t => t.IsDelete == false && t.DoctorPaperID == query.Id);
                var release = await _ReleaseManageService.ReleaseManageList(releaseManageIn);
                if (release.Count > 0)
                {
                    return new ObjectResultModule("", 100, "请不要重复推送文章！");
                }
                #endregion 

                #region 将文章推送到质控审核
                ReleaseManage releaseManage = new ReleaseManage();
                releaseManage.PaperTiltle = query.PaperTiltle;
                releaseManage.PaperContent = query.PaperContent;
                releaseManage.PaperAddress = query.PaperAddress;
                releaseManage.PaperFrom = "doctor";
                releaseManage.DoctorID = query.DoctorID;
                releaseManage.DoctorName = query.DoctorName;
                releaseManage.ConsultNumber = query.ConsultNumber;
                releaseManage.CheckState = "created";
                releaseManage.ReadTotal = 0;
                releaseManage.UpvoteTotal = 0;
                releaseManage.TransTotal = 0;
                releaseManage.CollectTotal = 0;
                releaseManage.ImageFie = query.ImageFie;
                releaseManage.DoctorPaperID = query.Id;   //医生文章
                var result = await _ReleaseManageService.CreateReleaseManage(releaseManage);
                #endregion
                if (result != null)
                {
                    query.CheckState = "Push";
                    query.CheckTime = DateTime.Now;
                    query.CheckRemark = "推送质控审核";
                    // 将文章推送到质控端
                    var res = await _DoctorPaperService.UpdateDoctorPaper(query);
                    this.ObjectResultModule.Object = res;
                    this.ObjectResultModule.Message = "sucess";
                    this.ObjectResultModule.StatusCode = 200;
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.Message = "推送失败";
                    this.ObjectResultModule.StatusCode = 500;
                }
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
                OperExplain = "PushDoctorPaper",
                OperContent = JsonHelper.ToJson(DoctorPaperInfo),
                OperType = "PushDoctorPaper",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        #endregion

        #region 查询科室列表 科室与标签关系  科室与医生关系  医生与标签关系 page
        /// <summary>
        /// 专业分组
        /// </summary>
        /// <param name="clinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherClinicDoctorsPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherClinicDoctorsPage(ClinicInfomationIn clinicInfomationInfo)
        {
            if (!Commons.CheckSecret(clinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            #region 先找科室
            //ClinicType 成人科1，儿科2
            if (clinicInfomationInfo.ClinicType > 0)
            {
                clinicInfomationInfo.AndAlso(t => t.ClinicType == clinicInfomationInfo.ClinicType);
            }
            clinicInfomationInfo.AndAlso(t => !t.IsDelete);
            var ClinicPage = await _clinicInfomationService.ClinicInfomationPage(clinicInfomationInfo);

            // 查询科室信息
            #endregion
            if (ClinicPage.Items.Count() > 0)
            {
                #region  查询缓存
                //List<ClinicInfomationView> ClinicViewList = new List<ClinicInfomationView>();
                ////查询科室标签
                //var lableList = await _lableManageService.ClinicLableManageList();

                //List<DoctorDetailList> doctorDetailList = new List<DoctorDetailList>();

                //#region 查询医生信息
                //YaeherUserIn yaeherUserIn = new YaeherUserIn();
                //yaeherUserIn.AndAlso(a => a.IsDelete == false);
                //var YaeherUser = await _yaeherUserService.YaeherUserList(yaeherUserIn);
                //YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
                //yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
                //var DoctorInfoList = await _YaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);


                //DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
                //doctorRelationIn.AndAlso(a => a.IsDelete == false);
                //var withoutlabeldoc = new List<YaeherDoctor>();//没有标签医生合计
                //var labeldoc = new List<YaeherDoctor>();//有标签医生合计
                //var DoctorList = new List<DoctorRelation>();
                //var doctorlabellist = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);//所有医生标签关联关系
                //if (!string.IsNullOrEmpty(clinicInfomationInfo.KeyWord))
                //{
                //    // doctorRelationIn.AndAlso(a => a.LableName.Contains(DockorSortInfo.KeyWord) || a.DoctorName.Contains(DockorSortInfo.KeyWord));
                //    withoutlabeldoc = DoctorInfoList.FindAll(a => a.DoctorName.Contains(clinicInfomationInfo.KeyWord));
                //    DoctorList = doctorlabellist.FindAll(a => a.LableName.Contains(clinicInfomationInfo.KeyWord) || a.DoctorName.Contains(clinicInfomationInfo.KeyWord));
                //}
                //else
                //{
                //    DoctorList = doctorlabellist;
                //}

                //if (!string.IsNullOrEmpty(clinicInfomationInfo.KeyWord))
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
                //doctorOnlineRecordIn.AndAlso(a => a.IsDelete == false);
                //doctorOnlineRecordIn.AndAlso(a => a.OnlineState == "online");
                //var DoctorOnlinList = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctorOnlineRecordIn);

                //// 医生提供服务状态
                //ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
                //serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
                //var DoctorServiceList = await _ServiceMoneyListService.ServiceMoneyListList(serviceMoneyListIn);

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
                //        bool ImageState = false;
                //        var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
                //        if (ImageTextItem != null)
                //        {
                //            ImageState = ImageTextItem.ServiceState;
                //            DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
                //            //DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
                //        }
                //        bool PhoneState = false;
                //        var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
                //        if (PhoneItem != null)
                //        {
                //            //PhoneState = PhoneItem.ServiceState;
                //            DoctorInfo.PhoneServiceExpense = PhoneItem.ServiceExpense;
                //            //DoctorInfo.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
                //        }
                //        if (ImageState || PhoneState)
                //        {
                //            DoctorInfo.ServiceState = true;  //可咨询
                //        }
                //        else
                //        {
                //            DoctorInfo.ServiceState = false;  //不可咨询
                //        }
                //        if (ImageState)
                //        {
                //            DoctorInfo.ServiceExpense = DoctorInfo.ImageServiceExpense;
                //        }
                //        if (!ImageState && PhoneState)
                //        {
                //            DoctorInfo.ServiceExpense = DoctorInfo.PhoneServiceExpense + 9999;   // 预设值扩大电话权重排序
                //        }
                //    }
                //}
                //#endregion
                //#region 查询统计排名当天数据

                //DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                //DateTime EndTime = StartTime.AddDays(1);
                //EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
                //evaluationTotalIn.AndAlso(a => a.IsDelete == false);
                //evaluationTotalIn.AndAlso(a => a.CreatedOn >= StartTime);
                //evaluationTotalIn.AndAlso(a => a.CreatedOn < EndTime);
                //var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);
                //#endregion
                //// 查询 统计结果集
                //if (evaluationTotalList.Count > 0)
                //{
                //    foreach (var DoctorInfo in doctorDetailList)
                //    {
                //        DoctorInfo.SetTopSort = 999999999;  // 默认将所有医生的置顶设置最大值
                //    }
                //}
                //foreach (var item in ClinicPage.Items)
                //{
                //    #region 查询科室信息 查询科室与医生关系
                //    var clinicInfo = await _clinicInfomationService.ClinicInfomationByID(item.Id);
                //    ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
                //    clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
                //    if (clinicInfo != null)
                //    {
                //        clinicDoctorReltionIn.AndAlso(a => a.ClinicID == clinicInfo.Id);
                //    }
                //    var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);

                //    // 当选择科室时 查询科室内的医生信息
                //    RecommendedOrderIn recommendedOrderIn = new RecommendedOrderIn();
                //    recommendedOrderIn.AndAlso(a => a.IsDelete == false);
                //    recommendedOrderIn.AndAlso(a => a.ClinicID == item.Id);
                //    var Ordering = await _recommendedOrderingService.RecommendedOrderList(recommendedOrderIn);
                //    // 查询置顶

                //    var DoctorLists = from a in clinicDoctorList
                //                      join b in doctorDetailList on a.DoctorID equals b.DoctorID
                //                      select b;
                //    var doctorDetailListitem = DoctorLists.Distinct().ToList();

                //    if (Ordering.Count > 0)
                //    {
                //        foreach (var doctorDetail in doctorDetailListitem)
                //        {
                //            doctorDetail.SetTopSort = Ordering.Where(a => a.DoctorID == doctorDetail.DoctorID).FirstOrDefault().ItemSort;
                //        }
                //    }
                //    doctorDetailListitem = doctorDetailListitem.OrderBy(a => a.OnlineState)
                //                                  .ThenBy(a => a.ServiceState)
                //                                  .ThenBy(a => a.ReceiptState)
                //                                  .ThenBy(a => a.SetTopSort)
                //                                  .ThenByDescending(a => a.AverageEvaluate)
                //                                  .ThenByDescending(a => a.AverageAnswer)
                //                                  .ThenBy(a => a.ServiceExpense)
                //                                  .ThenByDescending(a => a.RegisterDate).ToList();
                //    doctorDetailListitem = doctorDetailListitem.Where((x, i) => doctorDetailListitem.FindIndex(z => z.DoctorID == x.DoctorID) == i).ToList();

                //    var ClinicView = new ClinicInfomationView();
                //    ClinicView.Id = item.Id;
                //    ClinicView.ClinicName = item.ClinicName;
                //    ClinicView.ClinicIntro = item.ClinicIntro;
                //    ClinicView.ClinicDirector = item.ClinicDirector;
                //    ClinicView.ClinicType = item.ClinicType;
                //    ////查询科室与标签关系
                //    ClinicView.lableManages = lableList.Where(t => t.ClinicID == item.Id).ToList();
                //    ClinicView.doctorDetailList = doctorDetailListitem;
                //    ClinicView.DoctorCount = doctorDetailListitem.Count;
                //    ClinicViewList.Add(ClinicView);
                //    #endregion
                //}
                #endregion

                string key = "YaeherClinicDoctorsPage" + clinicInfomationInfo.SkipCount + "ClinicType" + clinicInfomationInfo.ClinicType;

                var ClinicViewList = await _cacheManager.GetCache("YaeherClinicDoctorsPage").GetAsync<string, List<ClinicInfomationView>>(key, async () => await DoctorClinicPage(ClinicPage));

                var tasksCount = ClinicPage.TotalCount;
                //获取总数
                var ClinicViewLists = new PagedResultDto<ClinicInfomationView>(tasksCount, ClinicViewList);
                this.ObjectResultModule.Object = new ClinicInfoOut(ClinicViewLists, clinicInfomationInfo);

            }


            if (ClinicPage.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {

                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherClinicDoctorsPage",
                OperContent = JsonHelper.ToJson(clinicInfomationInfo),
                OperType = "YaeherClinicDoctorsPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        private async Task<List<ClinicInfomationView>> DoctorClinicPage(PagedResultDto<ClinicInfomation> ClinicPage)
        {
            //Logger.Info("缓存测试DoctorClinicPage");
            List<ClinicInfomationView> ClinicViewList = new List<ClinicInfomationView>();

            List<DoctorDetailList> doctorDetailList = new List<DoctorDetailList>();
            //查询科室标签
            var lableList = await _lableManageService.ClinicLableManageList();
            #region 查询医生信息
            YaeherUserIn yaeherUserIn = new YaeherUserIn();
            yaeherUserIn.AndAlso(a => a.IsDelete == false);
            var YaeherUser = await _yaeherUserService.YaeherUserList(yaeherUserIn);
            YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
            yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
            var DoctorInfoList = await _YaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);


            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            var withoutlabeldoc = new List<YaeherDoctor>();//没有标签医生合计
            var labeldoc = new List<YaeherDoctor>();//有标签医生合计
            var DoctorList = new List<DoctorRelation>();
            var doctorlabellist = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);//所有医生标签关联关系

            DoctorList = doctorlabellist;



            // 医生上下线状态
            DoctorOnlineRecordIn doctorOnlineRecordIn = new DoctorOnlineRecordIn();
            doctorOnlineRecordIn.AndAlso(a => a.IsDelete == false);
            doctorOnlineRecordIn.AndAlso(a => a.OnlineState == "online");
            var DoctorOnlinList = await _doctorOnlineRecordService.DoctorOnlineRecordList(doctorOnlineRecordIn);

            // 医生提供服务状态
            ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
            serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
            var DoctorServiceList = await _ServiceMoneyListService.ServiceMoneyListList(serviceMoneyListIn);

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
                                      //   Department = a.Department,
                                      //  WorkYear = a.WorkYear,
                                      Title = a.Title,
                                      //  GraduateSchool = a.GraduateSchool,
                                      //  WechatNum = a.WechatNum,
                                      //  PhoneNumber = a.PhoneNumber,
                                      //  UserImageFile = a.UserImageFile,
                                      //  Resume = a.Resume,
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
                    // DoctorInfo.serviceMoneyList = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false).ToList();
                    // 接单状态
                    bool ImageState = false;
                    var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
                    if (ImageTextItem != null)
                    {
                        ImageState = ImageTextItem.ServiceState;
                        DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
                        //DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
                    }
                    bool PhoneState = false;
                    var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.DoctorID && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
                    if (PhoneItem != null)
                    {
                        //PhoneState = PhoneItem.ServiceState;
                        DoctorInfo.PhoneServiceExpense = PhoneItem.ServiceExpense;
                        //DoctorInfo.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
                    }
                    if (ImageState || PhoneState)
                    {
                        DoctorInfo.ServiceState = true;  //可咨询
                    }
                    else
                    {
                        DoctorInfo.ServiceState = false;  //不可咨询
                    }
                    if (ImageState)
                    {
                        DoctorInfo.ServiceExpense = DoctorInfo.ImageServiceExpense;
                    }
                    if (!ImageState && PhoneState)
                    {
                        DoctorInfo.ServiceExpense = DoctorInfo.PhoneServiceExpense + 9999;   // 预设值扩大电话权重排序
                    }
                }
            }
            #endregion
            #region 查询统计排名当天数据

            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime = StartTime.AddDays(1);
            EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            evaluationTotalIn.AndAlso(a => a.CreatedOn >= StartTime);
            evaluationTotalIn.AndAlso(a => a.CreatedOn < EndTime);
            var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);
            #endregion
            // 查询 统计结果集
            if (evaluationTotalList.Count > 0)
            {
                foreach (var DoctorInfo in doctorDetailList)
                {
                    DoctorInfo.SetTopSort = 999999999;  // 默认将所有医生的置顶设置最大值
                }
            }
            foreach (var item in ClinicPage.Items)
            {
                #region 查询科室信息 查询科室与医生关系
                var clinicInfo = await _clinicInfomationService.ClinicInfomationByID(item.Id);
                ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
                clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
                if (clinicInfo != null)
                {
                    clinicDoctorReltionIn.AndAlso(a => a.ClinicID == clinicInfo.Id);
                }
                var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);

                // 当选择科室时 查询科室内的医生信息
                RecommendedOrderIn recommendedOrderIn = new RecommendedOrderIn();
                recommendedOrderIn.AndAlso(a => a.IsDelete == false);
                recommendedOrderIn.AndAlso(a => a.ClinicID == item.Id);
                var Ordering = await _recommendedOrderingService.RecommendedOrderList(recommendedOrderIn);
                // 查询置顶

                var DoctorLists = from a in clinicDoctorList
                                  join b in doctorDetailList on a.DoctorID equals b.DoctorID
                                  select b;
                var doctorDetailListitem = DoctorLists.Distinct().ToList();

                if (Ordering.Count > 0)
                {
                    foreach (var doctorDetail in doctorDetailListitem)
                    {
                        doctorDetail.SetTopSort = Ordering.Where(a => a.DoctorID == doctorDetail.DoctorID).FirstOrDefault().ItemSort;
                    }
                }
                doctorDetailListitem = doctorDetailListitem.OrderBy(a => a.OnlineState)
                                              .ThenBy(a => a.ServiceState)
                                              .ThenBy(a => a.ReceiptState)
                                              .ThenBy(a => a.SetTopSort)
                                              .ThenByDescending(a => a.AverageEvaluate)
                                              .ThenByDescending(a => a.AverageAnswer)
                                              .ThenBy(a => a.ServiceExpense)
                                              .ThenByDescending(a => a.RegisterDate).ToList();
                doctorDetailListitem = doctorDetailListitem.Where((x, i) => doctorDetailListitem.FindIndex(z => z.DoctorID == x.DoctorID) == i).ToList();

                var ClinicView = new ClinicInfomationView();
                ClinicView.Id = item.Id;
                ClinicView.ClinicName = item.ClinicName;
                ClinicView.ClinicIntro = item.ClinicIntro;
                ClinicView.ClinicDirector = item.ClinicDirector;
                ClinicView.ClinicType = item.ClinicType;
                ////查询科室与标签关系
                ClinicView.lableManages = lableList.Where(t => t.ClinicID == item.Id).ToList();
                ClinicView.doctorDetailList = doctorDetailListitem;
                ClinicView.DoctorCount = doctorDetailListitem.Count;
                ClinicViewList.Add(ClinicView);
                #endregion
            }
            return ClinicViewList;
        }

        #endregion

        #region 查询科室列表 科室与标签关系  科室与医生关系  医生与标签关系 entity
        /// <summary>
        /// 获取患者关注的医生信息信息 ByIDArray
        /// </summary>
        /// <param name="YaeherUserIDArrayInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherPatientDoctorByIDArray")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<List<ClinicDoctorsView>> YaeherPatientDoctorByIDArray([FromBody]YaeherPatientDoctorIDArray YaeherUserIDArrayInfo)
        {
            if (!Commons.CheckSecret(YaeherUserIDArrayInfo.Secret))
            {
                return null;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var sc = new YaeherPatientDoctorSearch() { IdArr = YaeherUserIDArrayInfo.IDArray, KeyWord = YaeherUserIDArrayInfo.KeyWord };
            var values = await _clinicInfomationService.PatientCollectDoctorInformation(sc);
            #region 查询统计排名当天数据

            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime = StartTime.AddDays(1);
            EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            evaluationTotalIn.AndAlso(a => a.CreatedOn >= StartTime);
            evaluationTotalIn.AndAlso(a => a.CreatedOn < EndTime);
            var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);
            #endregion
            // 医生提供服务状态
            ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
            serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
            var DoctorServiceList = await _ServiceMoneyListService.ServiceMoneyListList(serviceMoneyListIn);

            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);

            var doctoralllist = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);

            if (!string.IsNullOrEmpty(YaeherUserIDArrayInfo.KeyWord))
            {
                doctorRelationIn.AndAlso(a => a.LableName.Contains(YaeherUserIDArrayInfo.KeyWord) || a.DoctorName.Contains(YaeherUserIDArrayInfo.KeyWord));
            }
            var DoctorList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);
            if (DoctorList.Count() < 1) { return new List<ClinicDoctorsView>(); }
            if (DoctorList.Count() > 0)
            {
                var DoctorLists = from a in values
                                  join b in DoctorList on a.Id equals b.DoctorID
                                  select a;
                if (DoctorLists.Count() > 0)
                {
                    values = DoctorLists.Distinct().ToList();
                }
                else
                {
                    return null;
                }
            }
            foreach (var item in values)
            {
                var serviceMoneyList = DoctorServiceList.Where(a => a.DoctorID == item.Id && a.IsDelete == false).ToList();
                // 接单状态
                bool ImageState = false;
                var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == item.Id && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
                if (ImageTextItem != null)
                {
                    ImageState = ImageTextItem.ServiceState;
                    item.ImageServiceExpense = ImageTextItem.ServiceExpense;
                    item.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
                }
                bool PhoneState = false;
                var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == item.Id && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
                if (PhoneItem != null)
                {
                    PhoneState = PhoneItem.ServiceState;
                    item.PhoneServiceExpense = PhoneItem.ServiceExpense;
                    item.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
                }
                if (ImageState || PhoneState)
                {
                    item.ServiceState = true;  //可咨询
                }
                else
                {
                    item.ServiceState = false;  //不可咨询
                }
                item.ReceiptState = true;  // 可接单,默认不满额
                item.Doctorslable = doctoralllist.Where(t => t.DoctorID == item.Id).ToList();

                ConsultationIn consultationIn = new ConsultationIn();
                consultationIn.AndAlso(a => a.IsDelete == false);
                consultationIn.AndAlso(a => (a.CreatedOn >= StartTime));
                consultationIn.AndAlso(a => (a.CreatedOn < EndTime));
                consultationIn.AndAlso(a => (a.RefundNumber == null || a.RefundNumber == ""));
                var consultationList = await _consultationService.YaeherConsultationList(consultationIn);
                if (consultationList.Count > 0)
                {
                    var yaeherConsultations = consultationList.Where(a => a.IsDelete == false && a.DoctorID == item.Id).ToList();
                    // 当天图文接单总数
                    var ImageNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "ImageText").Count();
                    // 当天电话接单总数
                    var PhoneNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "Phone").Count();

                    if ((ImageTextItem.ServiceFrequency - ImageNumberTotal > 0) || (PhoneItem.ServiceFrequency - PhoneNumberTotal) > 0)
                    {
                        item.ReceiptState = true;  // 可接单
                    }
                    else
                    {
                        item.ReceiptState = false;  // 满额
                    }

                }
                if (evaluationTotalList.Count > 0)
                {
                    var DoctorEvaluation = evaluationTotalList.Where(a => a.IsDelete == false && a.DoctorID == item.Id).FirstOrDefault();
                    if (DoctorEvaluation != null)
                    {
                        item.EvaluateTotal = DoctorEvaluation.EvaluateTotal;

                        //item.AverageEvaluate = 0;
                        item.OrderTotal = DoctorEvaluation.OrderTotal;
                        item.AverageAnswer = DoctorEvaluation.AverageAnswer;
                        item.RevenueTotal = DoctorEvaluation.RevenueTotal;
                        item.RefundTotal = DoctorEvaluation.RefundTotal;
                        item.CompleteTotal = DoctorEvaluation.CompleteTotal;
                        item.RefundRatio = double.Parse(Convert.ToDecimal(DoctorEvaluation.RefundTotal / DoctorEvaluation.OrderTotal).ToString("0.00"));
                        item.EvaluationCount = DoctorEvaluation.OneStar + DoctorEvaluation.TwoStar + DoctorEvaluation.ThreeStar + DoctorEvaluation.FourStar + DoctorEvaluation.FiveStar;
                        if (item.EvaluationCount >= 15)
                        { item.AverageEvaluate = DoctorEvaluation.AverageEvaluate; }//星级
                    }
                    else
                    {
                        item.EvaluationCount = 0;
                    }
                }
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherPatientDoctorByIDArray",
                OperContent = JsonHelper.ToJson(YaeherUserIDArrayInfo),
                OperType = "YaeherPatientDoctorByIDArray",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return values;
        }
        /// <summary>
        /// 患者端获取医生标签列表
        /// </summary>
        /// <param name="clinicInfomationIn"></param>
        /// <returns></returns>
        [Route("api/DoctorLableManageList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<List<LabelDoctorManage>> DoctorLableManageList(ClinicInfomationIn clinicInfomationIn)
        {
            if (!Commons.CheckSecret(clinicInfomationIn.Secret))
            {
                return null;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var lableList = await _lableManageService.DoctorLableManageList();
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorLableManageList",
                OperContent = JsonHelper.ToJson(clinicInfomationIn),
                OperType = "DoctorLableManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return lableList.ToList();
        }
        /// <summary>
        /// 医生列表
        /// </summary>
        /// <param name="clinicInfomationInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherClinicDoctors")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherClinicDoctors(ClinicInfomationIn clinicInfomationInfo)
        {
            if (!Commons.CheckSecret(clinicInfomationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            clinicInfomationInfo.AndAlso(t => !t.IsDelete);
            if (clinicInfomationInfo.Id > 0)
            {
                clinicInfomationInfo.AndAlso(t => t.Id == clinicInfomationInfo.Id);
            }
            var ClinicInfo = await _clinicInfomationService.DoctorInformation(clinicInfomationInfo);

            //查询科室标签
            DoctorRelationIn doctorRelationIn = new DoctorRelationIn();
            doctorRelationIn.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(clinicInfomationInfo.KeyWord))
            {
                doctorRelationIn.AndAlso(a => a.LableName.Contains(clinicInfomationInfo.KeyWord) || a.DoctorName.Contains(clinicInfomationInfo.KeyWord));
            }
            var lableList = await _DoctorRelationService.DoctorRelationList(doctorRelationIn);

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
                this.ObjectResultModule.Object = new ClinicDoctorInfoOut(ClinicDoctorsViewLists, clinicInfomationInfo);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherClinicDoctors",
                OperContent = JsonHelper.ToJson(clinicInfomationInfo),
                OperType = "YaeherClinicDoctors",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 搜索医生信息
        /// </summary>
        /// <param name="YaeherDoctorSearch"> YaeherDoctorSearch 数据</param>
        /// <returns></returns>
        [Route("api/YaeherDoctorSearch")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherDoctorSearch([FromBody]YaeherDoctorSearch YaeherDoctorSearch)
        {
            if (!Commons.CheckSecret(YaeherDoctorSearch.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
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

            var ClinicInfo = new PagedResultDto<ClinicDoctorsView>();
            // 暂定查询所有状态医生
            //if ((usermanager.MobileRoleName != "customerservice"&&!usermanager.IsCustomerService))
            //{
            //    YaeherDoctorSearch.OnlineState = "online";//客服端查所有,其余端查看上线后的医生
            //}
            if (YaeherDoctorSearch.ClinicID > 0)
            {
                ClinicInfo = await _clinicInfomationService.ClinicDoctorInformation(YaeherDoctorSearch, labelrel);
            }
            else
            {
                ClinicInfo = await _clinicInfomationService.DoctorInformation(YaeherDoctorSearch, labelrel);

            }

            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime = StartTime.AddDays(1);
            EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();
            evaluationTotalIn.AndAlso(a => a.IsDelete == false);
            evaluationTotalIn.AndAlso(a => (a.CreatedOn >= StartTime));
            evaluationTotalIn.AndAlso(a => (a.CreatedOn < EndTime));
            var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);

            // 医生提供服务状态
            ServiceMoneyListIn serviceMoneyListIn = new ServiceMoneyListIn();
            serviceMoneyListIn.AndAlso(a => a.IsDelete == false);
            var DoctorServiceList = await _ServiceMoneyListService.ServiceMoneyListList(serviceMoneyListIn);

            // 查询信息
            if (ClinicInfo != null && ClinicInfo.TotalCount > 0)
            {
                foreach (var DoctorInfo in ClinicInfo.Items)
                {
                    if (evaluationTotalList.Count > 0)
                    {
                        var evaluationTotal = evaluationTotalList.FirstOrDefault(t => t.DoctorID == DoctorInfo.Id);
                        if (evaluationTotal != null)
                        {
                            DoctorInfo.ReceiptNumBer = evaluationTotal == null ? 0 : evaluationTotal.CompleteTotal;//接单数
                            DoctorInfo.AverageTime = evaluationTotal == null ? "0" : evaluationTotal.AverageAnswer.ToString();//平均时长

                            DoctorInfo.EvaluateTotal = evaluationTotal.EvaluateTotal;
                            DoctorInfo.CompleteTotal = evaluationTotal.CompleteTotal;
                            DoctorInfo.EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
                            if (DoctorInfo.EvaluationCount >= 15)
                            { DoctorInfo.AverageEvaluate = evaluationTotal.AverageEvaluate; }//星级
                        }
                    }
                    else
                    {
                        DoctorInfo.ReceiptNumBer = 0;//接单数
                        DoctorInfo.AverageTime = "0";//平均时长
                        DoctorInfo.AverageEvaluate = 0;
                    }
                    DoctorInfo.Doctorslable = lableList.Where(t => t.DoctorID == DoctorInfo.Id).ToList();
                    var qualitycontrol = await _qualityCommitteeService.QualityCommitteeByDoctorID(DoctorInfo.Id);
                    DoctorInfo.QualityControlId = qualitycontrol == null ? 0 : qualitycontrol.Id;

                    DoctorInfo.serviceMoneyList = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false).ToList();
                    // 接单状态
                    bool ImageState = false;   // 图文咨询关闭
                    bool PhoneState = false;   // 电话咨询关闭
                    DoctorInfo.ServiceState = false;   // 停止服务
                    DoctorInfo.ReceiptState = false;   // 不可接单
                    DoctorInfo.ImageServiceFrequency = 0;  // 默认0
                    DoctorInfo.PhoneServiceFrequency = 0;  // 默认0
                    var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
                    if (ImageTextItem != null)
                    {
                        ImageState = ImageTextItem.ServiceState;
                        DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
                        if (ImageState)  // 开启
                        {
                            DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
                        }
                    }
                    var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
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
                    if (DoctorInfo.PhoneServiceFrequency > 0 || DoctorInfo.ImageServiceFrequency > 0)
                    {
                        DoctorInfo.ReceiptState = true;  // 可接单,默认不满额
                    }
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
                OperExplain = "YaeherDoctorSearch",
                OperContent = JsonHelper.ToJson(YaeherDoctorSearch),
                OperType = "YaeherDoctorSearch",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        #endregion

        #region 查询医生详情 entity
        /// <summary>
        /// 查询医生详情
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherClinicDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherClinicDoctor(YaeherDoctorIn YaeherDoctorInfo)
        {
            if (!Commons.CheckSecret(YaeherDoctorInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var DoctorInfo = await _YaeherDoctorService.YaeherDoctorByID(YaeherDoctorInfo.Id);
            //  List<ClinicDoctorsView> ClinicDoctorsViewList = new List<ClinicDoctorsView>();
            DoctorView doctorView = new DoctorView();
            // 查询科室信息
            if (DoctorInfo != null)
            {
                var UserResult = await _yaeherUserService.YaeherUserByID(DoctorInfo.UserID);
                if (UserResult == null) { return new ObjectResultModule("", 204, "NoContent"); }
                //执业经历
                var EmploymentIn = new DoctorEmploymentIn();
                EmploymentIn.AndAlso(t => !t.IsDelete && t.UserID == DoctorInfo.UserID);
                var docemplist = await _doctorEmploymentService.DoctorEmploymentList(EmploymentIn);

                doctorView.DoctorEmployment = docemplist;
                doctorView.UserImage = UserResult.UserImage;
                var DoctorSchedulingInList = new DoctorSchedulingIn();
                DoctorSchedulingInList.AndAlso(t => !t.IsDelete && t.DoctorID == DoctorInfo.Id && t.ServiceState);
                var value = await _DoctorSchedulingService.DoctorSchedulingList(DoctorSchedulingInList);
                doctorView.DoctorScheduling = value;
                var secret = await CreateSecret();
                var Content = "{\"createdby\":" + userid + ",\"doctorid\":" + DoctorInfo.Id + ",\"secret\":\"" + secret + "\"}";
                string uri = Commons.PatientIp + "api/YaeherPatientDoctorList/";
                var YaeherPatientDoctor = await this.PostResponseAsync(uri, Content);
                var YaeherPatientDoctorList = JsonHelper.FromJson<APIResult<ResultModule<List<YaeherPatientDoctor>>>>(YaeherPatientDoctor);

                doctorView.IsCollect = (YaeherPatientDoctorList == null || YaeherPatientDoctorList.result.item == null) ? false : true;//是否收藏

                DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime EndTime = StartTime.AddDays(1);
                EvaluationTotalIn evaluationTotalIn = new EvaluationTotalIn();

                evaluationTotalIn.AndAlso(a => a.IsDelete == false);
                evaluationTotalIn.AndAlso(a => (a.CreatedOn >= StartTime));
                evaluationTotalIn.AndAlso(a => (a.CreatedOn < EndTime));
                evaluationTotalIn.AndAlso(a => a.DoctorID == YaeherDoctorInfo.Id);
                var evaluationTotalList = await _evaluationTotalService.EvaluationTotalList(evaluationTotalIn);
                var evaluationTotal = evaluationTotalList.FirstOrDefault();
                doctorView.ReceiptNumBer = evaluationTotal == null ? 0 : evaluationTotal.CompleteTotal;//接单数
                doctorView.AverageTime = evaluationTotal == null ? "0" : evaluationTotal.AverageAnswer.ToString();//平均时长

                //doctorView.DoctorLevel = "0"; doctorView.AverageEvaluate = "0";
                //if (evaluationTotal.EvaluateTotal >= 15)
                //{
                //    doctorView.AverageEvaluate = evaluationTotal.AverageEvaluate.ToString();
                //    doctorView.DoctorLevel = evaluationTotal.AverageEvaluate.ToString();//星级
                //}//星级
                if (evaluationTotal != null)
                {
                    doctorView.EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
                    if (doctorView.EvaluationCount >= 15)
                    {
                        doctorView.AverageEvaluate = Math.Round(evaluationTotal.AverageEvaluate, 1);
                    }//星级
                }
                //   == null ? "0" : evaluationTotal.AverageEvaluate.ToString();//星级

                doctorView.EvaluationCount = evaluationTotal == null ? 0 : evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;//已评价订单数
                doctorView.UserImageFile = DoctorInfo.UserImageFile;//背景图
                doctorView.DoctorIntroduce = DoctorInfo.Resume;//医生简介信息
                //查询标签
                var LableManageInfo = new LableManageIn();
                LableManageInfo.AndAlso(t => !t.IsDelete);
                var lableList = await _lableManageService.LableManageList(LableManageInfo);
                //提供服务
                var ServiceMoneyListInfo = new ServiceMoneyListIn();
                ServiceMoneyListInfo.AndAlso(t => !t.IsDelete);
                var serviceMoneyLists = await _ServiceMoneyListService.ServiceMoneyStateList(ServiceMoneyListInfo);

                var consin = new ConsultationIn(); consin.AndAlso(a => a.IsDelete == false && a.DoctorID == YaeherDoctorInfo.Id);
                consin.AndAlso(a => (a.CreatedOn >= StartTime));
                consin.AndAlso(a => (a.CreatedOn < EndTime));
                consin.AndAlso(a => (a.RefundNumber == null || a.RefundNumber == ""));
                var doctoryaeherConsultations = await _consultationService.YaeherConsultationList(consin);
                // 当天图文接单总数
                var doctorImageNumberTotal = doctoryaeherConsultations.Where(a => a.ConsultType == "ImageText").Count();
                // 当天电话接单总数
                var doctorPhoneNumberTotal = doctoryaeherConsultations.Where(a => a.ConsultType == "Phone").Count();
                //提供服务
                var servicemoneylists = new List<ServiceMoneyStateList>();
                var ImageServiceFrequency = serviceMoneyLists.Where(a => a.DoctorID == YaeherDoctorInfo.Id && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
                var PhoneServiceFrequency = serviceMoneyLists.Where(a => a.DoctorID == YaeherDoctorInfo.Id && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
                if (ImageServiceFrequency != null)
                {
                    if (ImageServiceFrequency.ServiceFrequency > 0)
                    {
                        ImageServiceFrequency.ReceiptState = true;// 默认可接单
                    }
                    else
                    {
                        ImageServiceFrequency.ReceiptState = false;// 默认可接单
                    }
                    ImageServiceFrequency.ServiceState = ImageServiceFrequency == null ? false : ImageServiceFrequency.ServiceState;
                }

                if (PhoneServiceFrequency != null)
                {
                    if (PhoneServiceFrequency.ServiceFrequency > 0)
                    {
                        PhoneServiceFrequency.ReceiptState = true;// 默认不可接单
                    }
                    else
                    {
                        PhoneServiceFrequency.ReceiptState = false;// 默认不可接单
                    }
                    PhoneServiceFrequency.ServiceState = PhoneServiceFrequency == null ? false : PhoneServiceFrequency.ServiceState;
                }
                if (ImageServiceFrequency != null)
                {
                    if ((ImageServiceFrequency.ServiceFrequency - doctorImageNumberTotal > 0))
                    {
                        ImageServiceFrequency.ReceiptState = true;// 可接单
                    }
                    else
                    {
                        ImageServiceFrequency.ReceiptState = false;// 满额 
                    }
                    servicemoneylists.Add(ImageServiceFrequency);
                }
                if (PhoneServiceFrequency != null)
                {
                    if ((PhoneServiceFrequency.ServiceFrequency - doctorPhoneNumberTotal > 0))
                    {
                        PhoneServiceFrequency.ReceiptState = true;// 可接单
                    }
                    else
                    {
                        PhoneServiceFrequency.ReceiptState = false;// 满额 
                    }
                    servicemoneylists.Add(PhoneServiceFrequency);
                }
                //医生文章  查询医生发布的审核的文章
                var DoctorPaperInfo = new DoctorPaperIn();
                DoctorPaperInfo.DoctorId = DoctorInfo.Id;
                var doctorPapers = await _DoctorPaperService.DoctorPaperViewList(DoctorPaperInfo);

                //查询医生与标签关系
                var DoctorslableInfo = new DoctorRelationIn();
                DoctorslableInfo.AndAlso(t => t.DoctorName == DoctorInfo.DoctorName && !t.IsDelete && t.DoctorID == DoctorInfo.Id);
                var DoctorslableList = await _DoctorRelationService.DoctorRelationList(DoctorslableInfo);
                if (DoctorslableList.Count > 0)
                {
                    var DoctorlableList = from Doctorslables in DoctorslableList
                                          join lables in lableList on Doctorslables.LableID equals lables.Id
                                          select lables;
                    if (DoctorlableList.Count() > 0)
                    {
                        doctorView.lableManages = DoctorlableList.ToList();
                    }
                }
                #region 医生基本信息

                doctorView.Id = DoctorInfo.Id;
                doctorView.DoctorName = DoctorInfo.DoctorName;
                doctorView.UserID = DoctorInfo.UserID;
                doctorView.Address = DoctorInfo.Address;
                doctorView.HospitalName = DoctorInfo.HospitalName;
                doctorView.Department = DoctorInfo.Department;
                doctorView.WorkYear = DoctorInfo.WorkYear;
                doctorView.Title = DoctorInfo.Title;
                doctorView.GraduateSchool = DoctorInfo.GraduateSchool;
                doctorView.IsBelieveTCM = DoctorInfo.IsBelieveTCM;
                doctorView.IsServiceConscious = DoctorInfo.IsServiceConscious;
                doctorView.WechatNum = DoctorInfo.WechatNum;
                doctorView.PhoneNumber = DoctorInfo.PhoneNumber;
                #endregion
                doctorView.serviceMoneyLists = servicemoneylists;
                doctorView.doctorPapers = doctorPapers.Where(a => a.DoctorID == DoctorInfo.Id).ToList();
            }
            if (DoctorInfo == null)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = doctorView;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherClinicDoctor",
                OperContent = JsonHelper.ToJson(YaeherDoctorInfo),
                OperType = "YaeherClinicDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="YaeherSendMsmInfo"></param>
        /// <returns></returns>
        [Route("api/SendMessage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> SendMessage([FromBody] YaeherMessageRemind YaeherSendMsmInfo)
        {
            if (!Commons.CheckSecret(YaeherSendMsmInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var res = await _yaeherMessageRemindService.CreateYaeherMessageRemind(YaeherSendMsmInfo);
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
                OperExplain = "SendMessage",
                OperContent = JsonHelper.ToJson(YaeherSendMsmInfo),
                OperType = "SendMessage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 执业记录
        /// <summary>
        /// 执业记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorEmployment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorEmployment([FromBody] DoctorEmployment input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new DoctorEmployment()
            {
                UserID = userid,
                HospitalName = input.HospitalName,
                Department = input.Department,
                WorkYear = input.WorkYear,
                Title = input.Title,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _doctorEmploymentService.CreateDoctorEmployment(create);
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
                OperExplain = "CreateDoctorEmployment",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateDoctorEmployment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 执业记录Page
        /// </summary>
        /// <param name="DoctorEmploymentInPage"> DoctorEmploymentInPage 数据</param>
        /// <returns></returns>
        [Route("api/DoctorEmploymentPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorEmploymentPage([FromBody]DoctorEmploymentIn DoctorEmploymentInPage)
        {
            if (!Commons.CheckSecret(DoctorEmploymentInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorEmploymentInPage.AndAlso(t => !t.IsDelete && t.UserID == userid);
            var values = await _doctorEmploymentService.DoctorEmploymentPage(DoctorEmploymentInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new DoctorEmploymentOut(values, DoctorEmploymentInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorEmploymentPage",
                OperContent = JsonHelper.ToJson(DoctorEmploymentInPage),
                OperType = "DoctorEmploymentPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 执业记录List 
        /// </summary>
        /// <param name="DoctorEmploymentList"> DoctorEmploymentList 数据</param>
        /// <returns></returns>
        [Route("api/DoctorEmploymentList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorEmploymentList([FromBody]DoctorEmploymentIn DoctorEmploymentList)
        {
            if (!Commons.CheckSecret(DoctorEmploymentList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DoctorEmploymentList.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            var value = await _doctorEmploymentService.DoctorEmploymentList(DoctorEmploymentList);
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
                OperExplain = "DoctorEmploymentList",
                OperContent = JsonHelper.ToJson(DoctorEmploymentList),
                OperType = "DoctorEmploymentList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 执业记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateDoctorEmployment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateDoctorEmployment([FromBody] DoctorEmployment input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorEmploymentService.DoctorEmploymentByID(input.Id);
            if (query != null && query.UserID == userid)
            {
                if (!string.IsNullOrEmpty(input.HospitalName))
                { query.HospitalName = input.HospitalName; }
                if (!string.IsNullOrEmpty(input.Department))
                { query.Department = input.Department; }
                if (input.WorkYear > 0)
                { query.WorkYear = input.WorkYear; }
                if (!string.IsNullOrEmpty(input.Title))
                { query.Title = input.Title; }
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _doctorEmploymentService.UpdateDoctorEmployment(query);

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
                OperExplain = "UpdateDoctorEmployment",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateDoctorEmployment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 执业记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteDoctorEmployment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteDoctorEmployment([FromBody] DoctorEmployment input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorEmploymentService.DoctorEmploymentByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _doctorEmploymentService.DeleteDoctorEmployment(query);

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
                OperExplain = "DeleteDoctorEmployment",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteDoctorEmployment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 执业记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DoctorEmploymentById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorEmploymentById([FromBody] DoctorEmployment input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _doctorEmploymentService.DoctorEmploymentByID(input.Id);
            if (query != null)
            {
                this.ObjectResultModule.Object = query;
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
                OperExplain = "DoctorEmploymentById",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DoctorEmploymentById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 我的收入 收入明细
        /// <summary>
        /// 医生个人中心收入排行  按月统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/RevenueTotal")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RevenueTotal([FromBody]ConsultationOrderTotalIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
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
            if (usermanager.MobileRoleName == "doctor" || (usermanager.IsDoctor))
            {
                var doctor = await _YaeherDoctorService.YaeherDoctorByUserID(userid);
                var eval = new ConsultationOrderTotalIn();

                if (!string.IsNullOrEmpty(input.StartTime))
                {
                    StartTime = DateTime.Parse(input.StartTime);
                    if (string.IsNullOrEmpty(input.EndTime))
                    {
                        input.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                if (!string.IsNullOrEmpty(input.EndTime))
                {
                    EndTime = DateTime.Parse(input.EndTime);
                }
                if (!string.IsNullOrEmpty(input.StartTime))
                {
                    eval.AndAlso(t => t.IsDelete == false);
                    eval.AndAlso(t => t.TotalType == input.TotalType);
                    eval.AndAlso(t => t.TotalDate >= StartTime);
                    eval.AndAlso(t => t.TotalDate < EndTime.AddDays(+1));
                }
                else
                {
                    eval.AndAlso(t => t.IsDelete == false);
                    eval.AndAlso(t => t.TotalType == input.TotalType);
                }
                #region 查询退单金额 查询截至到昨天的退单金额
                //OrderTradeRecordIn refundManageIn = new OrderTradeRecordIn();
                //var refundendtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                //refundManageIn.DoctorId = usermanager.YaeherDoctorInfo.Id;
                //refundManageIn.AndAlso(t => t.IsDelete == false);
                //refundManageIn.AndAlso(t => t.CreatedOn >= StartTime);
                //refundManageIn.AndAlso(t => t.CreatedOn < refundendtime);
                //refundManageIn.AndAlso(t => t.PayMoney < 0);
                //var refundmanageList = await _orderTradeRecordService.DoctorOrderTradeRecordList(refundManageIn);
                //var refund = refundmanageList.OrderBy(t => t.CreatedOn).ToList();
                //double sumrefund = 0;
                //if (refund != null && refund.Count > 0)
                //{
                //    sumrefund = Convert.ToDouble(refund.Sum(t => t.PayMoney));
                //}
                #endregion

                var ordertotal = await _consultationOrderTotalService.ConsultationOrderTotalList(eval);
                if (ordertotal == null || ordertotal.Count() < 1) { return new ObjectResultModule("", 204, "NoContent"); }


                var srordertotalorder = ordertotal.OrderByDescending(t => t.CompleteMoney).ToList();
                var ddordertotalorder = ordertotal.OrderByDescending(t => t.CompleteTotal).ToList();

                var ps = ordertotal.FirstOrDefault(t => t.DoctorID == doctor.Id);//订单汇总表
                if (ps == null) { return new ObjectResultModule("", 204, "NoContent"); }

                var psr = srordertotalorder.IndexOf(ps) + 1;//收入排名
                var psr1 = ddordertotalorder.IndexOf(ps) + 1;//订单排名
                var moneyPercentage = (psr * 100) / srordertotalorder.Count() + "%"; //收入百分比
                var orderPercentage = (psr1 * 100) / ddordertotalorder.Count() + "%";//订单百分比
                //查询明细
                var evaldetail = new ConsultationOrderTotalIn();

                evaldetail.AndAlso(t => !t.IsDelete);
                if (input.TotalType == "month") { evaldetail.AndAlso(t => t.TotalType == "day"); }
                if (!string.IsNullOrEmpty(input.StartTime))
                {
                    StartTime = DateTime.Parse(input.StartTime);
                    if (string.IsNullOrEmpty(input.EndTime))
                    {
                        input.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                if (!string.IsNullOrEmpty(input.EndTime))
                {
                    EndTime = DateTime.Parse(input.EndTime);
                }
                if (!string.IsNullOrEmpty(input.StartTime))
                {
                    evaldetail.AndAlso(t => t.TotalDate >= StartTime);
                    evaldetail.AndAlso(t => t.TotalDate < EndTime.AddDays(+1));
                }
                evaldetail.AndAlso(t => t.DoctorID == doctor.Id);

                var ordertotaldetail = await _consultationOrderTotalService.ConsultationOrderTotalList(evaldetail);//查询明细

                //this.ObjectResultModule.Object = new ConsultationOrderTotalOutDetail(ps, sumrefund, psr, psr1, moneyPercentage, orderPercentage, ordertotaldetail.ToList(), refund);
                this.ObjectResultModule.Object = new ConsultationOrderTotalOutDetail(ps, psr, psr1, moneyPercentage, orderPercentage, ordertotaldetail.ToList());
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "RevenueTotal",
                OperContent = JsonHelper.ToJson(input),
                OperType = "RevenueTotal",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 医生与科室关系
        /// <summary>
        /// 医生与科室关系 新增
        /// </summary>
        /// <param name="DoctorClinicInfo"></param>
        /// <returns></returns>
        [Route("api/CreateDoctorClinic")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateDoctorClinic([FromBody] DoctorClinicIn DoctorClinicInfo)
        {
            if (!Commons.CheckSecret(DoctorClinicInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            try
            {
                var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    #region 先删除
                    ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
                    clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
                    clinicDoctorReltionIn.AndAlso(a => a.DoctorID == DoctorClinicInfo.DoctorID);
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
                    string[] clinicArray = null;
                    if (!string.IsNullOrEmpty(DoctorClinicInfo.ClinicIDJSON))
                    {
                        clinicArray = DoctorClinicInfo.ClinicIDJSON.Split(',');
                    }
                    ClinicInfomationIn clinicInfomationIn = new ClinicInfomationIn();
                    clinicInfomationIn.AndAlso(a => a.IsDelete == false);
                    var clinicList = await _clinicInfomationService.ClinicInfomationList(clinicInfomationIn);
                    YaeherDoctorIn yaeherDoctorIn = new YaeherDoctorIn();
                    yaeherDoctorIn.AndAlso(a => a.IsDelete == false);
                    var DoctorList = await _YaeherDoctorService.YaeherDoctorList(yaeherDoctorIn);

                    #region 检查存在审核科室通过的修改为不通过
                    var apply = new DoctorClinicApplyIn();
                    apply.AndAlso(t => !t.IsDelete && t.DoctorID == DoctorClinicInfo.DoctorID);
                    var clinicrel = await _DoctorClinicApplyService.DoctorClinicApplyList(apply);
                    var successapplyt = clinicrel.FindAll(t => t.CheckRes == "success");

                    foreach (var item in successapplyt)
                    {
                        if (!clinicArray.Contains(item.ClinicID.ToString()))
                        {
                            item.CheckRes = "fail";
                            item.CheckTime = DateTime.Now;
                            item.CheckRemark = "客服取消科室";
                            await _DoctorClinicApplyService.UpdateDoctorClinicApply(item);
                        }
                    }
                    #endregion
                    var resultAll = 0;
                    if (clinicArray != null && clinicArray.Length > 0)
                    {
                        for (int a = 0; a < clinicArray.Length; a++)
                        {
                            ClinicDoctorReltion clinicDoctorReltion = new ClinicDoctorReltion();
                            clinicDoctorReltion.DoctorID = DoctorClinicInfo.DoctorID;
                            var DoctorInfo = DoctorList.Where(t => t.Id == DoctorClinicInfo.DoctorID).FirstOrDefault();
                            if (DoctorInfo != null)
                            {
                                clinicDoctorReltion.DoctorName = DoctorInfo.DoctorName;
                                clinicDoctorReltion.DoctorJSON = JsonHelper.ToJson(DoctorInfo);
                                var clinicInfo = clinicList.Where(t => t.Id == int.Parse(clinicArray[a].ToString())).FirstOrDefault();
                                if (clinicInfo != null)
                                {
                                    clinicDoctorReltion.ClinicID = clinicInfo.Id;
                                    clinicDoctorReltion.ClinicName = clinicInfo.ClinicName;
                                    clinicDoctorReltion.ClinicJSON = JsonHelper.ToJson(clinicInfo);
                                    clinicDoctorReltion.CreatedBy = userid;
                                    clinicDoctorReltion.CreatedOn = DateTime.Now;
                                    var res = await _clinicDoctorReltionService.CreateClinicDoctorReltion(clinicDoctorReltion);
                                    resultAll = +res.Id;
                                    var tosuccess = clinicrel.Find(t => t.ClinicID == clinicInfo.Id && t.CheckRes == "checking");

                                    if (tosuccess != null && tosuccess.Id > 0)
                                    {
                                        tosuccess.CheckRes = "success";
                                        tosuccess.CheckTime = DateTime.Now;
                                        tosuccess.CheckRemark = "客服添加科室";
                                        await _DoctorClinicApplyService.UpdateDoctorClinicApply(tosuccess);
                                    }

                                }
                            }
                        }
                    }
                    #endregion
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "sucess";
                    this.ObjectResultModule.Object = resultAll;
                    #region 操作日志
                    var CreateYaeherOperList = new YaeherOperList()
                    {
                        OperExplain = "CreateDoctorClinic",
                        OperContent = JsonHelper.ToJson(DoctorClinicInfo),
                        OperType = "CreateDoctorClinic",
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now
                    };
                    var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
                    unitOfWork.Complete();
                }
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
        /// 新增医生与科室关系前检查 是否存在审核通过过的科室数据
        /// </summary>
        /// <param name="DoctorClinicInfo"></param>
        /// <returns></returns>
        [Route("api/CheckDoctorClinic")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CheckDoctorClinic([FromBody] DoctorClinicIn DoctorClinicInfo)
        {
            if (!Commons.CheckSecret(DoctorClinicInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            try
            {
                var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
                string[] clinicArray = null;
                if (!string.IsNullOrEmpty(DoctorClinicInfo.ClinicIDJSON))
                {
                    clinicArray = DoctorClinicInfo.ClinicIDJSON.Split(',');
                }
                var apply = new DoctorClinicApplyIn();
                apply.AndAlso(t => !t.IsDelete && t.DoctorID == DoctorClinicInfo.DoctorID && t.CheckRes == "success");
                var clinicrel = await _DoctorClinicApplyService.DoctorClinicApplyList(apply);
                foreach (var item in clinicrel)
                {
                    if (!clinicArray.Contains(item.ClinicID.ToString()))
                    {
                        return new ObjectResultModule("医生" + item.DoctorName + "存在审核通过的科室" + item.ClinicName + ",是否同意取消科室？", 200, "fail");
                    }
                }
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = "";
                #region 操作日志
                var CreateYaeherOperList = new YaeherOperList()
                {
                    OperExplain = "CheckDoctorClinic",
                    OperContent = JsonHelper.ToJson(DoctorClinicInfo),
                    OperType = "CheckDoctorClinic",
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
        /// 根据医生ID 查询科室
        /// </summary>
        /// <param name="DoctorClinicInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorClinicListByDoctorID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorClinicListByDoctorID([FromBody]DoctorClinicIn DoctorClinicInfo)
        {
            if (!Commons.CheckSecret(DoctorClinicInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
            clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
            clinicDoctorReltionIn.AndAlso(a => a.DoctorID == DoctorClinicInfo.DoctorID);
            var clinicDoctorList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
            if (clinicDoctorList.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                DoctorClinicIn doctorClinicIn = new DoctorClinicIn();
                if (clinicDoctorList.Count > 0)
                {
                    foreach (var clinicDoctorInfo in clinicDoctorList)
                    {
                        doctorClinicIn.Id = clinicDoctorInfo.Id;
                        doctorClinicIn.DoctorName = clinicDoctorInfo.DoctorName;
                        doctorClinicIn.DoctorID = clinicDoctorInfo.DoctorID;
                        doctorClinicIn.ClinicIDJSON += clinicDoctorInfo.ClinicID + ",";
                    }
                }
                this.ObjectResultModule.Object = doctorClinicIn;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DoctorClinicListByDoctorID",
                OperContent = JsonHelper.ToJson(DoctorClinicInfo),
                OperType = "DoctorClinicListByDoctorID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        #endregion

        #region 医生审核 认证
        /// <summary>
        /// 医生审核 认证
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        [Route("api/AuthCheckYaeherDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AuthCheckYaeherDoctor([FromBody] YaeherDoctorIn YaeherDoctorInfo)
        {
            if (!Commons.CheckSecret(YaeherDoctorInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var DoctorInfo = await _YaeherDoctorService.YaeherDoctorByID(YaeherDoctorInfo.Id);
            if (DoctorInfo != null)
            {
                if (YaeherDoctorInfo.AuthCheck == "Check")// AuthCheck第一步审核
                {
                    DoctorInfo.CheckRes = YaeherDoctorInfo.CheckRes;                        //审核状态
                    DoctorInfo.Checker = userid;                                            // 审核人
                    DoctorInfo.CheckRemark = YaeherDoctorInfo.CheckRemark;                  // 审核备注
                    DoctorInfo.CheckTime = DateTime.Now;                                    // 审核时间
                }
                else if (YaeherDoctorInfo.AuthCheck == "Test")  // 第二步考试审核
                {
                    DoctorInfo.TsetTime = DateTime.Now;                                     //考试时间
                    DoctorInfo.TestID = "线下考试";                                         // 考题编号
                    DoctorInfo.BaseTestRes = YaeherDoctorInfo.BaseTestRes;                  // 基础考试结果
                    DoctorInfo.SimTestRes = YaeherDoctorInfo.SimTestRes;                    // 模拟开始结果
                }
                else if (YaeherDoctorInfo.AuthCheck == "Authen") // 第三步 认证审核
                {
                    if (DoctorInfo.AuthCheckRes == "unupload")
                    {
                        return new ObjectResultModule("", 400, "认证文件未上传!");
                    }
                    if (DoctorInfo.BaseTestRes != "success" || DoctorInfo.SimTestRes != "success")
                    {
                        return new ObjectResultModule("", 400, "考试未通过不允许认证!");
                    }
                    DoctorInfo.AuthCheckRes = YaeherDoctorInfo.AuthCheckRes;                //认证审核状态
                    DoctorInfo.AuthChecker = userid;                                        // 认证人
                    DoctorInfo.AuthCheckRemark = YaeherDoctorInfo.AuthCheckRemark;          // 认证备注
                    DoctorInfo.AuthCheckTime = DateTime.Now;                                // 认证时间
                }
            }
            DoctorInfo.ModifyBy = userid;
            DoctorInfo.ModifyOn = DateTime.Now;

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if (DoctorInfo != null && YaeherDoctorInfo.AuthCheck == "Authen")
                {
                    #region 客服端认证通过新增上下限
                    if (YaeherDoctorInfo.AuthCheckRes == "success")
                    {
                        var online = await _doctorOnlineRecordService.DoctorOnlineRecordByDoctorID(DoctorInfo.Id);
                        if (online == null)//新增
                        {
                            var param = new SystemParameterIn() { SystemType = "NewDoctorPayProportions" };
                            var paramlist = await _systemParameterService.ParameterList(param);

                            param = new SystemParameterIn() { SystemType = "DoctorReceivablesTime" };
                            var paramlist1 = await _systemParameterService.ParameterList(param);

                            param = new SystemParameterIn() { SystemType = "DoctorMoneyExchange" };
                            var paramlist2 = await _systemParameterService.ParameterList(param);

                            param = new SystemParameterIn() { SystemType = "DoctorMoneyexTime" };
                            var paramlist3 = await _systemParameterService.ParameterList(param);

                            //add上下限
                            var CreateDoctorOnlineRecord = new DoctorOnlineRecord()
                            {
                                DoctorName = DoctorInfo.DoctorName,
                                DoctorID = DoctorInfo.Id,
                                DoctorJSON = JsonHelper.ToJson(DoctorInfo),
                                OnlineState = "online",//上线
                                Checker = userid,//默认审核通过
                                CheckState = "success",
                                CheckTime = DateTime.Now,
                                DivideInto = double.Parse(paramlist[0].ItemValue),//NewDoctorPayProportions code对应的值
                                IncomeDay = int.Parse(paramlist1[0].ItemValue),//DoctorReceivablesTime 医生收款
                                DoctorMoneyExchange = int.Parse(paramlist2[0].ItemValue),//
                                DoctorMoneyexTime = int.Parse(paramlist3[0].ItemValue),//
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                            };
                            var result1 = await _doctorOnlineRecordService.CreateDoctorOnlineRecord(CreateDoctorOnlineRecord);


                            #region 插入log日志表
                            var CreateDoctorOnlineSetLog = new DoctorOnlineSetLog()
                            {
                                DoctorName = DoctorInfo.DoctorName,
                                DoctorID = DoctorInfo.Id,
                                DoctorJSON = JsonHelper.ToJson(DoctorInfo),
                                OnlineState = "online",//上线
                                Checker = userid,//默认审核通过
                                CheckState = "success",
                                CheckTime = DateTime.Now,
                                DivideInto = double.Parse(paramlist[0].ItemValue),//NewDoctorPayProportions code对应的值
                                IncomeDay = int.Parse(paramlist1[0].ItemValue),//DoctorReceivablesTime 医生收款
                                DoctorMoneyExchange = int.Parse(paramlist2[0].ItemValue),//
                                DoctorMoneyexTime = int.Parse(paramlist3[0].ItemValue),//
                                CreatedBy = userid,
                                CreatedOn = DateTime.Now,
                            };
                            await _doctorOnlineSetLogService.CreateDoctorOnlineSetLog(CreateDoctorOnlineSetLog);
                        }
                        #endregion
                    }

                    #endregion
                }
                var result = await _YaeherDoctorService.UpdateYaeherDoctor(DoctorInfo);
                // 当为考试审核时 审核成功将个人角色修改为医生
                if (YaeherDoctorInfo.AuthCheck == "Test")
                {
                    if (YaeherDoctorInfo.BaseTestRes == "success" && YaeherDoctorInfo.SimTestRes == "success")
                    {
                        var UserInfo = await _yaeherUserService.YaeherUserByID(DoctorInfo.UserID);
                        if (UserInfo != null)
                        {
                            UserInfo.PhoneNumber = result.PhoneNumber;
                            UserInfo.FullName = result.DoctorName;
                            UserInfo.RoleName = "doctor";
                            UserInfo.LoginName = result.DoctorName;
                            SystemParameterIn parain = new SystemParameterIn();
                            parain.AndAlso(t => !t.IsDelete && t.SystemCode == "WXRole");
                            var sys = await _systemParameterService.SystemParameterList(parain);
                            SystemParameter doctorParameter = sys.ToList().Find(t => t.Code == "doctor");
                            SystemParameter patientParameter = sys.ToList().Find(t => t.Code == "patient");
                            if (doctorParameter != null && !string.IsNullOrEmpty(doctorParameter.ItemValue) && !string.IsNullOrEmpty(UserInfo.WecharOpenID))
                            {
                                try
                                {

                                    //  SystemToken systemToken = new SystemToken();
                                    //  systemToken.TokenType = "Wechar";
                                    var Tokens = await _systemTokenService.SystemTokenList("Wechar");
                                    TagDetail doctorDetail = JsonHelper.FromJson<TagDetail>(doctorParameter.ItemValue);
                                    #region 增加医生标签 没做多标签判断
                                    //TagDetail patientDetail = JsonHelper.FromJson<TagDetail>(patientParameter.ItemValue);
                                    //TencentUserManage usermanage = new TencentUserManage();
                                    //BatchtaggingTag batchtagging1 = new BatchtaggingTag();
                                    //batchtagging1.openid_list = new List<string>();
                                    //batchtagging1.openid_list.Add(UserInfo.WecharOpenID);
                                    //batchtagging1.tagid = patientDetail.id;
                                    //var responsemsg1 = await usermanage.DeleteWeiXinUserTag(batchtagging1, Tokens.access_token);

                                    //BatchtaggingTag batchtagging = new BatchtaggingTag();
                                    //batchtagging.openid_list = new List<string>();
                                    //batchtagging.openid_list.Add(UserInfo.WecharOpenID);
                                    //batchtagging.tagid = doctorDetail.id;
                                    //var responsemsg = await usermanage.WeiXinUserbatchtaggingTag(batchtagging, Tokens.access_token);
                                    #endregion
                                    // 增加医生标签 增加多标签判断
                                    UserInfo.WecharLable = doctorDetail.name;//微信labelname
                                    UserInfo.WecharLableId = doctorDetail.id;//微信labelid
                                    TencentUserManage usermanage = new TencentUserManage();
                                    var usermsg = await usermanage.WeiXinUserInfoUtils(UserInfo.WecharOpenID, Tokens.access_token);
                                    var responsemsg = await usermanage.YaeherUserLable(usermsg, UserInfo, Tokens.access_token);
                                }
                                catch (Exception ex)
                                {
                                    //Logger.Info("修改微信标签失败！" + ex.Message.ToString());
                                }
                            }
                            await _yaeherUserService.UpdateYaeherUser(UserInfo);
                        }
                    }
                }
                if (result.Id > 0)
                {
                    this.ObjectResultModule.Object = result;
                }
                unitOfWork.Complete();
            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "AuthCheckYaeherDoctor",
                OperContent = JsonHelper.ToJson(YaeherDoctorInfo),
                OperType = "AuthCheckYaeherDoctor",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion
        /// <summary>
        /// 新增医生信息  注册医生
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateForeignYaeherDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateForeignYaeherDoctor([FromBody] YaeherDoctorIn input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            YaeherUserIn yaeherUserIn = new YaeherUserIn();
            yaeherUserIn.AndAlso(t => t.IsDelete == false);
            var nickname = input.WecharName;// System.Web.HttpUtility.UrlEncode(input.WecharName, System.Text.Encoding.UTF8);
            yaeherUserIn.AndAlso(t => t.NickName == nickname);
            //查找医生
            var UserInfoList = await _yaeherUserService.YaeherUserList(yaeherUserIn);

            if (UserInfoList == null || UserInfoList.Count > 1)
            {
                return new ObjectResultModule("", 400, "未找到该用户！");
            }
            var UserInfo = UserInfoList.FirstOrDefault();
            var create = new YaeherDoctor()
            {
                DoctorName = input.DoctorName,
                UserID = UserInfo.Id,
                HospitalName = input.HospitalName,
                Department = input.Department,
                WorkYear = input.WorkYear,
                Title = input.Title,
                GraduateSchool = input.GraduateSchool,
                IsBelieveTCM = input.IsBelieveTCM,
                IsServiceConscious = input.IsServiceConscious,
                WechatNum = input.WechatNum,
                PhoneNumber = input.PhoneNumber,
                Recommender = input.Recommender,
                RecommenderName = input.RecommenderName,
                IDCard = input.IDCard,
                Address = input.Address,
                CheckRes = "success",
                AuthCheckRes = "success",
                BaseTestRes = "success",
                SimTestRes = "success",
                CreatedBy = UserInfo.Id,
                CreatedOn = DateTime.Now,
                IsSharing = false,//是否开启分账
            };
            var doctor = await _YaeherDoctorService.CreateYaeherDoctor(create);//创建医生
            SystemParameterIn parain = new SystemParameterIn();
            parain.AndAlso(t => !t.IsDelete && t.SystemCode == "WXRole");
            var sys = await _systemParameterService.SystemParameterList(parain);

            UserInfo.RoleName = "doctor";
            SystemParameter doctorParameter = sys.ToList().Find(t => t.Code == "doctor");
            TagDetail doctorDetail = JsonHelper.FromJson<TagDetail>(doctorParameter.ItemValue);
            UserInfo.WecharLable = doctorDetail.name;//微信labelname
            UserInfo.WecharLableId = doctorDetail.id;//微信labelid
            var userup = await _yaeherUserService.UpdateYaeherUser(UserInfo);//修改用户标签

            #region 打微信标签 增加标签增加多标签判断
            TencentUserManage usermanage = new TencentUserManage();
            //  SystemToken systemToken = new SystemToken();
            // systemToken.TokenType = "Wechar";
            var Tokens = await _systemTokenService.SystemTokenList("Wechar");
            var usermsg = await usermanage.WeiXinUserInfoUtils(userup.WecharOpenID, Tokens.access_token);
            var responsemsg = await usermanage.YaeherUserLable(usermsg, userup, Tokens.access_token);
            #region 打微信标签没做多标签判断
            //BatchtaggingTag batchtagging1 = new BatchtaggingTag();
            //batchtagging1.openid_list = new List<string>();
            //batchtagging1.openid_list.Add(UserInfo.WecharOpenID);
            //batchtagging1.tagid = patientDetail.id;
            //var responsemsg1 = await usermanage.DeleteWeiXinUserTag(batchtagging1, Tokens.access_token);


            //BatchtaggingTag batchtagging = new BatchtaggingTag();
            //batchtagging.openid_list = new List<string>();
            //batchtagging.openid_list.Add(UserInfo.WecharOpenID);
            //batchtagging.tagid = doctorDetail.id;
            //var responsemsg = await usermanage.WeiXinUserbatchtaggingTag(batchtagging, Tokens.access_token);//打标签
            #endregion
            #endregion
            #region 新增执业记录
            var employ = new DoctorEmployment()
            {
                UserID = UserInfo.Id,
                HospitalName = input.HospitalName,
                Department = input.Department,
                WorkYear = input.WorkYear,
                Title = input.Title,
                CreatedBy = UserInfo.Id,
                CreatedOn = DateTime.Now,
            };
            var empres = await _doctorEmploymentService.CreateDoctorEmployment(employ);
            #endregion
            #region 新增上下线
            var param = new SystemParameterIn() { SystemType = "NewDoctorPayProportions" };
            var paramlist = await _systemParameterService.ParameterList(param);

            param = new SystemParameterIn() { SystemType = "DoctorReceivablesTime" };
            var paramlist1 = await _systemParameterService.ParameterList(param);

            param = new SystemParameterIn() { SystemType = "DoctorMoneyExchange" };
            var paramlist2 = await _systemParameterService.ParameterList(param);

            param = new SystemParameterIn() { SystemType = "DoctorMoneyexTime" };
            var paramlist3 = await _systemParameterService.ParameterList(param);
            //add上下限
            var CreateDoctorOnlineRecord = new DoctorOnlineRecord()
            {
                DoctorName = input.DoctorName,
                DoctorID = doctor.Id,
                DoctorJSON = JsonHelper.ToJson(doctor),
                OnlineState = "online",//上线
                Checker = UserInfo.Id,//默认审核通过
                CheckState = "success",
                CheckTime = DateTime.Now,
                DivideInto = double.Parse(paramlist[0].ItemValue),//NewDoctorPayProportions code对应的值
                IncomeDay = int.Parse(paramlist1[0].ItemValue),//DoctorReceivablesTime 医生收款
                DoctorMoneyExchange = int.Parse(paramlist2[0].ItemValue),//
                DoctorMoneyexTime = int.Parse(paramlist3[0].ItemValue),//
                CreatedBy = UserInfo.Id,
                CreatedOn = DateTime.Now,
            };
            var result1 = await _doctorOnlineRecordService.CreateDoctorOnlineRecord(CreateDoctorOnlineRecord);//新增上下线
            #region 插入log日志表
            var CreateDoctorOnlineSetLog = new DoctorOnlineSetLog()
            {
                DoctorName = input.DoctorName,
                DoctorID = doctor.Id,
                DoctorJSON = JsonHelper.ToJson(doctor),
                OnlineState = "online",//上线
                Checker = UserInfo.Id,//默认审核通过
                CheckState = "success",
                CheckTime = DateTime.Now,
                DivideInto = double.Parse(paramlist[0].ItemValue),//NewDoctorPayProportions code对应的值
                IncomeDay = int.Parse(paramlist1[0].ItemValue),//DoctorReceivablesTime 医生收款
                DoctorMoneyExchange = int.Parse(paramlist2[0].ItemValue),//
                DoctorMoneyexTime = int.Parse(paramlist3[0].ItemValue),//
                CreatedBy = UserInfo.Id,
                CreatedOn = DateTime.Now,
            };
            var res2 = await _doctorOnlineSetLogService.CreateDoctorOnlineSetLog(CreateDoctorOnlineSetLog);//新增上下线日志
            #endregion
            #endregion
            return new ObjectResultModule("", 200, "新增海外医生成功！");
        }
    }
}
