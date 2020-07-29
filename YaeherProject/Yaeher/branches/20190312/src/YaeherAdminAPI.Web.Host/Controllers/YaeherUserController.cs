using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.TenPay.V3;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.ClinicManage.Dto;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.TencentCustom;
using Yaeher.Extensions;
using Yaeher.MessageRemind;
using Yaeher.MessageRemind.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 注册用户信息
    /// </summary>
    public class YaeherUserController : YaeherAppServiceBase
    {
        private readonly IYaeherUserService _yaeherUserService;
        private readonly IYaeherUserPaymentService _yaeherUserPaymentService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _IabpSession;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherOperListService _yaeherOperListService;
        // 医生申请上传资料
        private readonly IDoctorFileApplyService _doctorFileApplyService;
        // 医生从业经历
        private readonly IDoctorEmploymentService _doctorEmploymentService;
        // 申请科室
        private readonly IDoctorClinicApplyService _doctorClinicApplyService;
        // 医生与科室关系
        private readonly IClinicDoctorReltionService _clinicDoctorReltionService;
        // 医生提供服务表
        private readonly IServiceMoneyListService _serviceMoneyListService;
        private readonly IYaeherMessageRemindService _yaeherMessageRemindService;
        /// <summary>
        /// 医生信息
        /// </summary>
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly ISystemTokenService _systemTokenService;
        private readonly IAcceptTencentWecharService _acceptTencentWecharService;

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="yaeherUserService"></param>
        /// <param name="yaeherUserPaymentService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="userManagerService"></param>
        /// <param name="session"></param>
        /// <param name="doctorFileApplyService"></param>
        /// <param name="doctorEmploymentService"></param>
        /// <param name="doctorClinicApplyService"></param>
        /// <param name="clinicDoctorReltionService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="serviceMoneyListService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="yaeherMessageRemindService"></param>
        /// <param name="systemTokenService"></param>
        /// <param name="acceptTencentWecharService"></param>
        public YaeherUserController(IYaeherUserService yaeherUserService,
                                    IYaeherUserPaymentService yaeherUserPaymentService,
                                    ISystemParameterService systemParameterService,
                                    IUnitOfWorkManager unitOfWorkManager,
                                    IUserManagerService userManagerService,
                                    IAbpSession session,
                                    IDoctorFileApplyService doctorFileApplyService,
                                    IDoctorEmploymentService doctorEmploymentService,
                                    IDoctorClinicApplyService doctorClinicApplyService,
                                    IClinicDoctorReltionService clinicDoctorReltionService,
                                    IYaeherOperListService yaeherOperListService,
                                    IServiceMoneyListService serviceMoneyListService,
                                    IYaeherDoctorService yaeherDoctorService,
                                    IYaeherMessageRemindService yaeherMessageRemindService,
                                    ISystemTokenService systemTokenService,
                                    IAcceptTencentWecharService acceptTencentWecharService)
        {
            _yaeherUserService = yaeherUserService;
            _yaeherUserPaymentService = yaeherUserPaymentService;
            _systemParameterService = systemParameterService;
            _unitOfWorkManager = unitOfWorkManager;
            _userManagerService = userManagerService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
            _doctorFileApplyService = doctorFileApplyService;
            _doctorEmploymentService = doctorEmploymentService;
            _doctorClinicApplyService = doctorClinicApplyService;
            _clinicDoctorReltionService = clinicDoctorReltionService;
            _serviceMoneyListService=serviceMoneyListService;
            _yaeherDoctorService = yaeherDoctorService;
            _yaeherMessageRemindService = yaeherMessageRemindService;
            _systemTokenService = systemTokenService;
            _acceptTencentWecharService = acceptTencentWecharService;
        }

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/CreateMobileYaeherUser")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> CreateMobileYaeherUser([FromBody] YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //  var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateUser = new YaeherUser()
            {
                LoginName = YaeherUserInfo.LoginName,
                LoginPwd = YaeherUserInfo.LoginPwd,
                FullName = YaeherUserInfo.FullName,
                PhoneNumber = YaeherUserInfo.PhoneNumber,
                Email = YaeherUserInfo.Email,
                IDCard = YaeherUserInfo.IDCard,
                Sex = YaeherUserInfo.Sex,
                Birthday = YaeherUserInfo.Birthday,
                ErrorCount = YaeherUserInfo.ErrorCount,
                LoginCount = YaeherUserInfo.LoginCount,
                Userorigin = YaeherUserInfo.Userorigin,
                WecharNo = YaeherUserInfo.WecharNo,
                WecharName = YaeherUserInfo.WecharName,
                UserImage = YaeherUserInfo.UserImage,
                RoleName = YaeherUserInfo.RoleName,
                CreatedOn = DateTime.Now,
                IsLabel = false,
                IsPay = false,
                IsUpdate = false,

            };

            if (!string.IsNullOrEmpty(YaeherUserInfo.Enabled))
            {
                CreateUser.Enabled = bool.Parse(YaeherUserInfo.Enabled);
            }
            if (string.IsNullOrEmpty(YaeherUserInfo.LoginName))
            {
                CreateUser.LoginName = YaeherUserInfo.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.RoleName))
            {
                SystemParameterIn systemParameterIn = new SystemParameterIn();
                systemParameterIn.Type = "ConfigPar";
                systemParameterIn.AndAlso(a => a.SystemCode == "WXRole");
                systemParameterIn.AndAlso(a => a.IsDelete == false);
                systemParameterIn.AndAlso(a => a.Code != "doctor");
                var Parameters = await _systemParameterService.ParameterList(systemParameterIn);

                var code = Parameters.Find(t => t.Code == YaeherUserInfo.RoleName);
                if (code == null) { return new ObjectResultModule("", 400, "平台角色授权失败！"); }

            }
            var result = await _yaeherUserService.CreateYaeherUser(CreateUser);

            //http请求微信信息，获取账户的信息 新增用户
            var CreateUserPayment = new YaeherUserPayment()
            {
                UserID = result.Id,
                FullName = CreateUser.FullName,
                PayMethod = "wxpay",
                PayMethodName = "微信支付",
                PaymentAccout = string.IsNullOrEmpty(YaeherUserInfo.WecharName) ? "" : YaeherUserInfo.WecharName,
                BankName = "wx",
                Subbranch = "wx",
                BandAdd = "wx",
                BankNo = "wx",
                CreatedOn = DateTime.Now,
                CreatedBy = CreateUser.Id,
                IsDefault = true,
            };
            var result1 = await _yaeherUserPaymentService.CreateYaeherUserPayment(CreateUserPayment);

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
                OperExplain = "CreateMobileYaeherUser",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "CreateMobileYaeherUser",
                CreatedBy = 0,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 用户注册  手动创建用户
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherUser")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherUser([FromBody] YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateUser = new YaeherUser()
            {
                LoginName = YaeherUserInfo.LoginName,
                LoginPwd = YaeherUserInfo.LoginPwd,
                FullName = YaeherUserInfo.FullName,
                PhoneNumber = YaeherUserInfo.PhoneNumber,
                Email = YaeherUserInfo.Email,
                IDCard = YaeherUserInfo.IDCard,
                Sex = YaeherUserInfo.Sex,
                Birthday = YaeherUserInfo.Birthday,
                ErrorCount = YaeherUserInfo.ErrorCount,
                LoginCount = YaeherUserInfo.LoginCount,
                Userorigin = YaeherUserInfo.Userorigin,
                WecharNo = YaeherUserInfo.WecharNo,
                WecharName = YaeherUserInfo.WecharName,
                UserImage = YaeherUserInfo.UserImage,
                RoleName = YaeherUserInfo.RoleName,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,
                IsLabel = false,
                IsPay = false,
                IsUpdate = false,

            };
            if (!string.IsNullOrEmpty(YaeherUserInfo.Enabled))
            {
                CreateUser.Enabled = bool.Parse(YaeherUserInfo.Enabled);
            }
            if (string.IsNullOrEmpty(YaeherUserInfo.LoginName))
            {
                CreateUser.LoginName = "Yaeher" + YaeherUserInfo.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.RoleName))
            {
                SystemParameterIn systemParameterIn = new SystemParameterIn();
                systemParameterIn.Type = "ConfigPar";
                systemParameterIn.AndAlso(a => a.SystemCode == "WXRole");
                systemParameterIn.AndAlso(a => a.IsDelete == false);
                systemParameterIn.AndAlso(a => a.Code != "doctor");
                var Parameters = await _systemParameterService.ParameterList(systemParameterIn);

                var code = Parameters.Find(t => t.Code == YaeherUserInfo.RoleName);
                if (code == null) { return new ObjectResultModule("", 400, "平台角色授权失败！"); }

            //    CreateUser.WecharLable = "患者";
              //  CreateUser.WecharLableId = JsonHelper.FromJson<Tag>(code.ItemValue).id;
                
              //  CreateUser.WeCharUserJson = JsonHelper.ToJson(code.ItemValue);

            }
            var result = await _yaeherUserService.CreateYaeherUser(CreateUser);

            ////http请求微信信息，获取账户的信息

            var CreateUserPayment = new YaeherUserPayment()
            {
                UserID = result.Id,
                FullName = CreateUser.FullName,
                PayMethod = "wxpay",
                PayMethodName = "微信支付",
                PaymentAccout = string.IsNullOrEmpty(YaeherUserInfo.WecharName) ?"": YaeherUserInfo.WecharName,
                BankName = "wx",
                Subbranch = "wx",
                BandAdd = "wx",
                BankNo = "wx",
                CreatedOn = DateTime.Now,
                CreatedBy = CreateUser.Id
            };
            var result1 = await _yaeherUserPaymentService.CreateYaeherUserPayment(CreateUserPayment);

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
                OperExplain = "CreateYaeherUser",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "CreateYaeherUser",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 用户修改
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherUser")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherUser([FromBody] YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
           // var usermanager = _userManagerService.UserManager(userid);
             var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var UpdateUser = new YaeherUser();
            // 医生自己只允许修改自己 
            if (YaeherUserInfo.Platform == "Mobile")
            {
                if (usermanager.MobileRoleName == "doctor" || usermanager.MobileRoleName == "patient")
                {
                    UpdateUser = await _yaeherUserService.YaeherUserByID(userid);
                }
            }
            else if(YaeherUserInfo.Platform == "PC")
            {
                if (usermanager.IsAdmin)
                {
                    UpdateUser = await _yaeherUserService.YaeherUserByID(YaeherUserInfo.Id);
                }
            }
            if (UpdateUser.Id > 0)
            {
                if(!string.IsNullOrEmpty(YaeherUserInfo.PhoneNumber))
                {
                    // 当电话号码不为空且不为现有电话号码才更新
                    if (UpdateUser.PhoneNumber != YaeherUserInfo.PhoneNumber)
                    {
                        UpdateUser.PhoneNumber = YaeherUserInfo.PhoneNumber;
                    }
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.UserImage))
                {
                    var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
                    var paramlist = await _systemParameterService.ParameterList(param);
                    UpdateUser.UserImage = paramlist[0].ItemValue + "/" + YaeherUserInfo.UserImage;
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.Email))
                {
                    UpdateUser.Email = YaeherUserInfo.Email;
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.LoginName))
                {
                    UpdateUser.LoginName = YaeherUserInfo.LoginName;
                    UpdateUser.FullName = YaeherUserInfo.LoginName;
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.IDCard))
                {
                    UpdateUser.IDCard = YaeherUserInfo.IDCard;
                }
                if (YaeherUserInfo.Sex > 0)
                {
                    UpdateUser.Sex = YaeherUserInfo.Sex;
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.Enabled) && bool.Parse(YaeherUserInfo.Enabled) != UpdateUser.Enabled)
                {
                    UpdateUser.Enabled = bool.Parse(YaeherUserInfo.Enabled);
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.WecharNo))
                {
                    UpdateUser.WecharNo = YaeherUserInfo.WecharNo;
                    UpdateUser.WecharName = YaeherUserInfo.WecharName;
                }
                if (!string.IsNullOrEmpty(YaeherUserInfo.RoleName))
                {
                    UpdateUser.RoleName = YaeherUserInfo.RoleName;
                }
                UpdateUser.ModifyBy = userid;
                UpdateUser.ModifyOn = DateTime.Now;
                var result = await _yaeherUserService.UpdateYaeherUser(UpdateUser);
                this.ObjectResultModule.Object = result;
            }
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateYaeherUser",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "UpdateYaeherUser",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherUser")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherUser([FromBody] YaeherUser YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherUserService.YaeherUserByID(YaeherUserInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherUserService.DeleteYaeherUser(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteYaeherUser",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "DeleteYaeherUser",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取注册用户信息 Page
        /// </summary>
        /// <param name="YaeherUserInfo"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUserPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserPage([FromBody]YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherUserInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherUserInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherUserInfo.EndTime))
                {
                    YaeherUserInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherUserInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.StartTime))
            {
                YaeherUserInfo.AndAlso(t => t.CreatedOn >= StartTime);
                YaeherUserInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.LoginName))
            {
                YaeherUserInfo.AndAlso(t => t.LoginName.Contains(YaeherUserInfo.LoginName));
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.Enabled))
            {
                YaeherUserInfo.AndAlso(t => t.Enabled == bool.Parse(YaeherUserInfo.Enabled));
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.WecharRole))
            {
                //YaeherUserInfo.AndAlso(t => t.RoleName != "doctor");
                YaeherUserInfo.AndAlso(t => t.RoleName == YaeherUserInfo.WecharRole);
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.KeyWord))
            {
                YaeherUserInfo.AndAlso(t => t.LoginName.Contains(YaeherUserInfo.KeyWord)|| 
                                            t.FullName.Contains(YaeherUserInfo.KeyWord)|| 
                                            t.PhoneNumber.Contains(YaeherUserInfo.KeyWord) || 
                                            t.Email.Contains(YaeherUserInfo.KeyWord) || 
                                            t.WecharNo.Contains(YaeherUserInfo.KeyWord) );
            }
            YaeherUserInfo.AndAlso(a => a.IsDelete == false);
            var values = await _yaeherUserService.YaeherUserPage(YaeherUserInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new YaeherUserOut(values, YaeherUserInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherUserPage",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "YaeherUserPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取注册用户信息 List 
        /// </summary>
        /// <param name="YaeherUserInfo"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUserList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserList([FromBody]YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherUserInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherUserInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherUserInfo.EndTime))
                {
                    YaeherUserInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherUserInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.StartTime))
            {
                YaeherUserInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.LoginName))
            {
                YaeherUserInfo.AndAlso(t => t.LoginName.Contains(YaeherUserInfo.LoginName));
            }
            if (!string.IsNullOrEmpty(YaeherUserInfo.Enabled))
            {
                YaeherUserInfo.AndAlso(t => t.Enabled == bool.Parse(YaeherUserInfo.Enabled));
            }
            YaeherUserInfo.AndAlso(a => a.IsDelete == false);
            var values = await _yaeherUserService.YaeherUserList(YaeherUserInfo);
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
                OperExplain = "YaeherUserList",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "YaeherUserList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="YaeherUserInfo"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUserById")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> YaeherUserById([FromBody]YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherUserService.YaeherUserByID(YaeherUserInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 400;
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
                OperExplain = "YaeherUserById",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "YaeherUserById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="YaeherUserMsg"> YaeherUserMsg 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUser")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUser([FromBody]YaeherUserMsg YaeherUserMsg)
        {
            if (!Commons.CheckSecret(YaeherUserMsg.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherUserService.YaeherUserByID(userid);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 400;
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
                OperExplain = "YaeherUser",
                OperContent = JsonHelper.ToJson(YaeherUserMsg),
                OperType = "YaeherUser",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 获取注册用户信息 Byname and pwd
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserByNameAndPwd")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<YaeherUser> YaeherUserByNameAndPwd([FromBody]YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                return null;
            }
            var values = await _yaeherUserService.YaeherUserByExpress(t => (t.LoginPwd == YaeherUserInfo.LoginPwd && !t.IsDelete) && (t.LoginName == YaeherUserInfo.LoginName || t.Email == YaeherUserInfo.LoginName));
            return values;
        }
        /// <summary>
        /// 获取注册用户信息 ByIDArray
        /// </summary>
        /// <param name="YaeherUserIDArrayInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserByIDArray")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<List<YaeherUserDocMsg>> YaeherUserByIDArray([FromBody]YaeherUserIDArray YaeherUserIDArrayInfo)
        {
            if (!Commons.CheckSecret(YaeherUserIDArrayInfo.Secret))
            {
                return null;
            }
            var values = await _yaeherUserService.YaeherUserListByArray(YaeherUserIDArrayInfo.IDArray);
            return values;
        }

        /// <summary>
        /// 修改移动端角色
        /// </summary>
        /// <param name="YaeherDoctorsInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateUserWXRole")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateUserWXRole([FromBody]YaeherWecharUser YaeherDoctorsInfo)
        {
            if (!Commons.CheckSecret(YaeherDoctorsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var YaeherDoctorUser = await _yaeherUserService.YaeherUserByID(YaeherDoctorsInfo.UserID);
           // SystemToken systemToken = new SystemToken();
          //  systemToken.TokenType = "Wechar";
            var Tokens = await _systemTokenService.SystemTokenList("Wechar");
            if (YaeherDoctorUser != null)
            {
                if (string.IsNullOrEmpty(YaeherDoctorsInfo.RoleName))
                {
                    return new ObjectResultModule("",400,"请先勾选移动端角色后保存！");
                }
                if (!string.IsNullOrEmpty(YaeherDoctorUser.WecharOpenID))
                {
                    SystemParameterIn parain = new SystemParameterIn();
                    parain.AndAlso(t => !t.IsDelete && t.SystemCode == "WXRole");
                    var sys = await _systemParameterService.SystemParameterList(parain);
                    SystemParameter doctorParameter = sys.ToList().Find(t => t.Code == YaeherDoctorsInfo.RoleName);
                   
                    SystemParameter patientParameter = sys.ToList().Find(t => t.Code == YaeherDoctorUser.RoleName);
                    if (doctorParameter == null || patientParameter == null) { return new ObjectResultModule("", 400, "角色授权失败，Code值查找失败，请联系管理员！"); }

                    TagDetail doctorDetail = JsonHelper.FromJson<TagDetail>(doctorParameter.ItemValue);
                    TagDetail patientDetail = JsonHelper.FromJson<TagDetail>(patientParameter.ItemValue);
                    TencentUserManage usermanage = new TencentUserManage();

                    BatchtaggingTag batchtagging1 = new BatchtaggingTag();
                    batchtagging1.openid_list = new List<string>();
                    batchtagging1.openid_list.Add(YaeherDoctorUser.WecharOpenID);
                    batchtagging1.tagid = patientDetail.id;
                    var responsemsg1 = await usermanage.DeleteWeiXinUserTag(batchtagging1,Tokens.access_token);
                    if(responsemsg1.errmsg!="ok")
                    {
                        return new ObjectResultModule("",400,"标签验证失败，请重新修改权限！");
                    }

                    BatchtaggingTag batchtagging = new BatchtaggingTag();
                    batchtagging.openid_list = new List<string>();
                    batchtagging.openid_list.Add(YaeherDoctorUser.WecharOpenID);
                    batchtagging.tagid = doctorDetail.id;
                    var responsemsg = await usermanage.WeiXinUserbatchtaggingTag(batchtagging,Tokens.access_token);
                    YaeherDoctorUser.RoleName = YaeherDoctorsInfo.RoleName;

                    YaeherDoctorUser.WecharLable = doctorDetail.name;
                    YaeherDoctorUser.WecharLableId = doctorDetail.id;

                    var result = await _yaeherUserService.UpdateYaeherUser(YaeherDoctorUser);

                    this.ObjectResultModule.Object = result;
                }
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
                OperExplain = "UpdateUserWXRole",
                OperContent = JsonHelper.ToJson(YaeherDoctorsInfo),
                OperType = "UpdateUserWXRole",
                CreatedBy = 0,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 用户与角色关系List
        /// </summary>
        /// <param name="YaeherDoctorsInfo"></param>
        /// <returns></returns>
        [Route("api/UserWecharRoleByUserID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UserWecharRoleByUserID([FromBody]YaeherWecharUser YaeherDoctorsInfo)
        {
            if (!Commons.CheckSecret(YaeherDoctorsInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherWecharUser yaeherWecharUser = new YaeherWecharUser();
            yaeherWecharUser.yaeherUser = await _yaeherUserService.YaeherUserByID(YaeherDoctorsInfo.UserID);
            SystemParameterIn systemParameterIn = new SystemParameterIn();
            systemParameterIn.Type = "ConfigPar";
            systemParameterIn.AndAlso(a => a.SystemCode == "WXRole");
            systemParameterIn.AndAlso(a => a.IsDelete == false);
            var Parameters = await _systemParameterService.ParameterList(systemParameterIn);
            List<WecharRole> wecharRoles = new List<WecharRole>();
            if (Parameters != null)
            {
                foreach (var ParametersInfo in Parameters)
                {
                    WecharRole wecharRole = new WecharRole();
                    wecharRole.SystemCode = ParametersInfo.SystemCode;
                    wecharRole.Code = ParametersInfo.Code;
                    wecharRole.Name = ParametersInfo.Name;
                    if (wecharRole.Code != "doctor")
                    {
                        wecharRoles.Add(wecharRole);
                    }
                }
            }
            yaeherWecharUser.wecharRoles = wecharRoles;
            yaeherWecharUser.RoleName = yaeherWecharUser.yaeherUser.RoleName;
            if (yaeherWecharUser == null)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = yaeherWecharUser;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UserWecharRoleByUserID",
                OperContent = JsonHelper.ToJson(YaeherDoctorsInfo),
                OperType = "UserWecharRoleByUserID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 用户账户
        /// <summary>
        /// 用户账户类型获取
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserPaymentType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserPaymentType([FromBody] SecretModel YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var user = await _yaeherUserService.YaeherUserByID(userid);

            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "OrderTradeRecordPayType");
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
                OperExplain = "YaeherUserPaymentType",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "YaeherUserPaymentType",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 用户账户新增
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherUserPayment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherUserPayment([FromBody] YaeherUserPayment YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var user = await _yaeherUserService.YaeherUserByID(userid);

            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "OrderTradeRecordPayType");
            var paramlist = await _systemParameterService.ParameterList(param);

            var CreateUserPayment = new YaeherUserPayment()
            {
                UserID = userid,
                FullName = user.FullName,
                PayMethod = YaeherUserPaymentInfo.PayMethod,
                PayMethodName = paramlist.Find(t => t.Code == YaeherUserPaymentInfo.PayMethod).Name,
                PaymentAccout = YaeherUserPaymentInfo.PaymentAccout,
                BankName = YaeherUserPaymentInfo.BankName,
                Subbranch = YaeherUserPaymentInfo.Subbranch,
                BandAdd = YaeherUserPaymentInfo.BandAdd,
                BankNo = YaeherUserPaymentInfo.BankNo,
                CreatedOn = DateTime.Now,
                CreatedBy = userid

            };
            var result = await _yaeherUserPaymentService.CreateYaeherUserPayment(CreateUserPayment);
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
                OperExplain = "CreateYaeherUserPayment",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "CreateYaeherUserPayment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 用户账户修改
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherUserPayment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherUserPayment([FromBody] YaeherUserPayment YaeherUserPaymentInfo)
        {
            //Logger.Info("YaeherUserPaymentInfo:" + JsonHelper.ToJson(YaeherUserPaymentInfo));
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateUserPayment = await _yaeherUserPaymentService.YaeherUserPaymentByID(YaeherUserPaymentInfo.Id);

            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "OrderTradeRecordPayType");
            var paramlist = await _systemParameterService.ParameterList(param);

            if (UpdateUserPayment != null)
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.PayMethod))
                    {
                        UpdateUserPayment.PayMethod = YaeherUserPaymentInfo.PayMethod;
                        UpdateUserPayment.PayMethodName = paramlist.Find(t => t.Code == YaeherUserPaymentInfo.PayMethod).Name;
                    }
                    if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.PaymentAccout))
                    {
                        UpdateUserPayment.PaymentAccout = YaeherUserPaymentInfo.PaymentAccout;
                    }
                    if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.BankName))
                    {
                        UpdateUserPayment.BankName = YaeherUserPaymentInfo.BankName;
                    }
                    if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.Subbranch))
                    {
                        UpdateUserPayment.Subbranch = YaeherUserPaymentInfo.Subbranch;
                    }
                    if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.BandAdd))
                    {
                        UpdateUserPayment.BandAdd = YaeherUserPaymentInfo.BandAdd;
                    }
                    if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.BankNo))
                    {
                        UpdateUserPayment.BankNo = YaeherUserPaymentInfo.BankNo;
                    }
                    if (YaeherUserPaymentInfo.IsDefault)
                    {
                        var pay = new YaeherUserPaymentIn(); pay.AndAlso(t => !t.IsDelete && t.IsDefault && t.UserID == userid);
                        var query = await _yaeherUserPaymentService.YaeherUserPaymentList(pay);
                        if (query.Count > 0)
                        {
                            foreach (var item in query)
                            {
                                if (item.Id != UpdateUserPayment.Id)
                                {
                                    item.IsDefault = false;
                                    var queryresult = await _yaeherUserPaymentService.UpdateYaeherUserPayment(item);
                                }
                            }
                        }
                        UpdateUserPayment.IsDefault = YaeherUserPaymentInfo.IsDefault;
                    }
                    UpdateUserPayment.ModifyBy = userid;
                    UpdateUserPayment.ModifyOn = DateTime.Now;
                    var result = await _yaeherUserPaymentService.UpdateYaeherUserPayment(UpdateUserPayment);
                    this.ObjectResultModule.Object = result;
                    unitOfWork.Complete();
                }

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
                OperExplain = "UpdateYaeherUserPayment",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "UpdateYaeherUserPayment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// YaeherUserPaymentByDocID
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserPaymentByUserID")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> YaeherUserPaymentByUserID([FromBody] YaeherUserPayment YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateUserPayment = await _yaeherUserPaymentService.YaeherUserPaymentByUserID(YaeherUserPaymentInfo.UserID);
            if (UpdateUserPayment != null)
            {
                this.ObjectResultModule.Object = UpdateUserPayment;
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
                OperExplain = "YaeherUserPaymentByUserID",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "YaeherUserPaymentByUserID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherUserPayment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherUserPayment([FromBody] YaeherUserPayment YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherUserPaymentService.YaeherUserPaymentByID(YaeherUserPaymentInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherUserPaymentService.DeleteYaeherUserPayment(query);

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
                OperExplain = "DeleteYaeherUserPayment",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "DeleteYaeherUserPayment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 获取注册用户账户信息 Page
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"> YaeherUserPaymentInfo 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUserPaymentPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserPaymentPage([FromBody]YaeherUserPaymentIn YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherUserPaymentInfo.AndAlso(t => !t.IsDelete && t.UserID == userid);
            var values = await _yaeherUserPaymentService.YaeherUserPaymentPage(YaeherUserPaymentInfo);
            if (values.Items.Count > 0)
            {
                this.ObjectResultModule.Object = new YaeherUserPaymentOut(values, YaeherUserPaymentInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherUserPaymentPage",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "YaeherUserPaymentPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 获取注册用户账户信息 List 
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"> YaeherUserPaymentInfo 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUserPaymentList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserPaymentList([FromBody]YaeherUserPaymentIn YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (!string.IsNullOrEmpty(YaeherUserPaymentInfo.FullName))
            {
                YaeherUserPaymentInfo.AndAlso(t => t.FullName.Contains(YaeherUserPaymentInfo.FullName));
            }
            YaeherUserPaymentInfo.AndAlso(t => !t.IsDelete);
            var values = await _yaeherUserPaymentService.YaeherUserPaymentList(YaeherUserPaymentInfo);
            if (values.Count > 0)
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherUserPaymentList",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "YaeherUserPaymentList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 获取注册用户账户信息 Byid
        /// </summary>
        /// <param name="YaeherUserPaymentInfo"> YaeherUserPaymentInfo 数据</param>
        /// <returns></returns>
        [Route("api/YaeherUserPaymentById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserByPaymentId([FromBody]YaeherUserPaymentIn YaeherUserPaymentInfo)
        {
            if (!Commons.CheckSecret(YaeherUserPaymentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherUserPaymentService.YaeherUserPaymentByID(YaeherUserPaymentInfo.Id);
            if (values != null)
            {
                this.ObjectResultModule.Object = values;
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
                OperExplain = "YaeherUserByPaymentId",
                OperContent = JsonHelper.ToJson(YaeherUserPaymentInfo),
                OperType = "YaeherUserByPaymentId",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 用户基本信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="YaeherUserInfo"> </param>
        /// <returns></returns>
        [Route("api/YaeherUserDetailInfo")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserDetailInfo([FromBody]YaeherUser YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var usermanager = JsonHelper.FromJson<UserMemory>(_IabpSession.GetUserData());
            var User = await _yaeherUserService.YaeherUserByID(userid);
          
            var YaeherUserInfoViewInfo = new YaeherUserInfoView();
            YaeherUserInfoViewInfo.yaeherUser = User;
          //  if (usermanager.IsAdmin || usermanager.IsDoctor || usermanager.MobileRoleName == "doctor")
             if (usermanager.IsDoctor || usermanager.MobileRoleName == "doctor")
            {
                var doctor = await _yaeherDoctorService.YaeherDoctorByUserID(userid);
                //医生基本信息
                YaeherUserInfoViewInfo.yaeherDoctor = doctor;
                // 认证资料
                DoctorFileApplyIn doctorFileApplyIn = new DoctorFileApplyIn();
                doctorFileApplyIn.AndAlso(a => a.DoctorID == doctor.Id);
                doctorFileApplyIn.AndAlso(a => a.IsDelete == false);
                var DoctorFileApply = await _doctorFileApplyService.DoctorFileApplyList(doctorFileApplyIn);
                if (DoctorFileApply != null)
                {
                    YaeherUserInfoViewInfo.doctorAppliesFile = DoctorFileApply.Where(a => a.DocumentsUse == "register").ToList();
                }
                // 医生科室信息 包含申请  转科室 新增科室
                // 现有科室
                ClinicDoctorReltionIn clinicDoctorReltionIn = new ClinicDoctorReltionIn();
                clinicDoctorReltionIn.AndAlso(a => a.DoctorID == doctor.Id);
                clinicDoctorReltionIn.AndAlso(a => a.IsDelete == false);
                var DoctorClinicList = await _clinicDoctorReltionService.ClinicDoctorReltionList(clinicDoctorReltionIn);
                // 申请记录
                DoctorClinicApplyIn doctorClinicApplyIn = new DoctorClinicApplyIn();
                doctorClinicApplyIn.AndAlso(a => a.DoctorID == doctor.Id);
                doctorClinicApplyIn.AndAlso(a => a.IsDelete == false);
                doctorClinicApplyIn.AndAlso(a => a.CheckRes != "success");
                var DoctorApplyClinicList = await _doctorClinicApplyService.DoctorClinicApplyList(doctorClinicApplyIn);
                List<DoctorClinicInfo> DoctorClinicInfos = new List<DoctorClinicInfo>();
                if (DoctorClinicList != null)
                {
                    foreach (var DoctorClinicInfo in DoctorClinicList)
                    {
                        DoctorClinicInfo doctorClinicInfo = new DoctorClinicInfo();
                        doctorClinicInfo.ClinicID = DoctorClinicInfo.ClinicID;
                        doctorClinicInfo.ClinicName = DoctorClinicInfo.ClinicName;
                        doctorClinicInfo.ClinicJSON = DoctorClinicInfo.ClinicJSON;
                        doctorClinicInfo.DoctorID = DoctorClinicInfo.DoctorID;
                        doctorClinicInfo.DoctorName = DoctorClinicInfo.DoctorName;
                        doctorClinicInfo.DoctorJSON = DoctorClinicInfo.DoctorJSON;
                        doctorClinicInfo.ApplyType = "审核完成";
                        doctorClinicInfo.CheckRes = "success";
                        doctorClinicInfo.DoctorClinicFileApplies = DoctorFileApply.Where(a => a.DocumentsUse == "register").ToList();
                        DoctorClinicInfos.Add(doctorClinicInfo);
                    }
                }
                if (DoctorApplyClinicList != null)
                {
                    foreach (var DoctorApplyClinicInfo in DoctorApplyClinicList)
                    {
                        DoctorClinicInfo doctorClinicInfo = new DoctorClinicInfo();
                        doctorClinicInfo.ClinicID = DoctorApplyClinicInfo.ClinicID;
                        doctorClinicInfo.ClinicName = DoctorApplyClinicInfo.ClinicName;
                        doctorClinicInfo.ClinicJSON = DoctorApplyClinicInfo.ClinicJSON;
                        doctorClinicInfo.DoctorID = DoctorApplyClinicInfo.DoctorID;
                        doctorClinicInfo.DoctorName = DoctorApplyClinicInfo.DoctorName;
                        doctorClinicInfo.DoctorJSON = DoctorApplyClinicInfo.DoctorJSON;
                        doctorClinicInfo.ApplyType = DoctorApplyClinicInfo.ApplyType;
                        doctorClinicInfo.CheckRes = DoctorApplyClinicInfo.CheckRes;
                        doctorClinicInfo.DoctorClinicFileApplies = DoctorFileApply.Where(a => a.DocumentsUse != "register" && a.DoctorClinicApplyId == DoctorApplyClinicInfo.Id).ToList();
                        DoctorClinicInfos.Add(doctorClinicInfo);
                    }
                }
                if (DoctorClinicInfos.Count > 0)
                {
                    YaeherUserInfoViewInfo.clinicDoctorReltions = DoctorClinicInfos;
                }
                // 医生从业经历
                DoctorEmploymentIn doctorEmploymentIn = new DoctorEmploymentIn();
                doctorEmploymentIn.AndAlso(a => a.UserID == userid);
                doctorEmploymentIn.AndAlso(a => a.IsDelete == false);
                var doctorEmployments = await _doctorEmploymentService.DoctorEmploymentList(doctorEmploymentIn);
                if (doctorEmployments != null)
                {
                    YaeherUserInfoViewInfo.doctorEmployments = doctorEmployments.ToList();
                }
            }
            if (YaeherUserInfoViewInfo == null)
            {
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = YaeherUserInfoViewInfo;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherUserDetailInfo",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "YaeherUserDetailInfo",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="yaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserModule")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserModule([FromBody]YaeherUserInfo yaeherUserInfo)
        {
            if (!Commons.CheckSecret(yaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (yaeherUserInfo.Platform != "PC")
            {
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var YaeherUserInfos = _userManagerService.UserManager(yaeherUserInfo.Id);
            if (YaeherUserInfos != null)
            {
                this.ObjectResultModule.Object = YaeherUserInfos;
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
                OperExplain = "UserInfo",
                OperContent = JsonHelper.ToJson(yaeherUserInfo),
                OperType = "UserInfo",
                CreatedBy = 0,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }


        /// <summary>
        /// 获取用户信息,打上标签
        /// </summary>
        /// <param name="yaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/TestGetAllFocusUser")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> TestGetAllFocusUser([FromBody]YaeherUserInfo yaeherUserInfo)
        {
            if (!Commons.CheckSecret(yaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            //string password = "e10adc3949ba59abbe56e057f20f883e";  //默认密码
            var Content = _acceptTencentWecharService.SendWecharMesaage(yaeherUserInfo.WecharOpenID).Result;
            //SystemToken KejisystemToken = new SystemToken();
            //KejisystemToken.TokenType = "Wechar";
            //var KejiTokens = await _systemTokenService.SystemTokenList(KejisystemToken);

            //var UserInfo = _yaeherUserService.YaeherUserInfo(yaeherUserInfo.WecharOpenID, KejiTokens.access_token);
            //var TencentUserManage = new TencentUserManage();

            //SystemToken Token = new SystemToken();
            //Token.TokenType = "Wechar";
            //var KejiTokens = await _systemTokenService.SystemTokenList(Token);
            //TencentFocusUser kejiFocusUser = await TencentUserManage.WeiXinUserInfoUtils(yaeherUserInfo.WecharOpenID, KejiTokens.access_token);


            //SystemToken Token1 = new SystemToken();
            //Token1.TokenType = "Wechar1";
            //var KejiTokens1 = await _systemTokenService.SystemTokenList(Token1);
            //TencentFocusUser kejiFocusUser1 = await TencentUserManage.WeiXinUserInfoUtils(yaeherUserInfo.WecharOpenID, KejiTokens1.access_token); 
            //var UserInfo = _yaeherUserService.YaeherUserInfo(yaeherUserInfo.WecharOpenID, KejiTokens.access_token);
            return new ObjectResultModule("", 200, "success");
        }
        
        #endregion

        #region 微信支付



        #endregion

        #region 修改用户电话号码
        /// <summary>
        /// 修改用户电话号码  修改用户密码
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/UserAuthentication")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UserAuthentication([FromBody] YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (string.IsNullOrEmpty(YaeherUserInfo.VerificationCode))
            {
                return new ObjectResultModule("", 100, "请先填写注册码！");
            }
            if (string.IsNullOrEmpty(YaeherUserInfo.PhoneNumber))
            {
                return new ObjectResultModule("", 100, "请先填写手机号码！");
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UserInfo = await _yaeherUserService.YaeherUserByID(userid);
            if (UserInfo!=null)
            {
                //验证验证码
                var mes = new YaeherMessageRemindIn(); mes.AndAlso(t => !t.IsDelete && t.PhoneNumber == YaeherUserInfo.PhoneNumber && t.MessageType == "Authentication");
                var message = await _yaeherMessageRemindService.YaeherMessageRemindList(mes);
                if (message != null)
                {
                    if (message[0].VerificationCode != YaeherUserInfo.VerificationCode)
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
                //验证验证码
                UserInfo.PhoneNumber = YaeherUserInfo.PhoneNumber;
                var Userresult= await _yaeherUserService.UpdateYaeherUser(UserInfo);
                if (Userresult != null)
                {
                    var DoctorInfo = await _yaeherDoctorService.YaeherDoctorByUserID(UserInfo.Id);
                    if (DoctorInfo != null)
                    {
                        DoctorInfo.PhoneNumber = YaeherUserInfo.PhoneNumber;
                        var Doctorresult = await _yaeherDoctorService.UpdateYaeherDoctor(DoctorInfo);
                        if (Doctorresult != null)
                        {
                            this.ObjectResultModule.Object = Doctorresult;
                            this.ObjectResultModule.StatusCode = 200;
                            this.ObjectResultModule.Message = "success";
                        }
                        else
                        {
                            this.ObjectResultModule.Object = "";
                            this.ObjectResultModule.StatusCode = 400;
                            this.ObjectResultModule.Message = "修改失败！";
                        }
                        return ObjectResultModule;
                    }
                    this.ObjectResultModule.Object = Userresult;
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "error";
                }
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
                OperExplain = "UserAuthentication",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "UserAuthentication",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 修改用户密码
        /// <summary>
        /// 修改用户电话号码  修改用户密码
        /// </summary>
        /// <param name="YaeherUserInfo"></param>
        /// <returns></returns>
        [Route("api/ChangePassword")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ChangePassword([FromBody] YaeherUserIn YaeherUserInfo)
        {
            if (!Commons.CheckSecret(YaeherUserInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (string.IsNullOrEmpty(YaeherUserInfo.VerificationCode))
            {
                return new ObjectResultModule("", 100, "请先填写注册码！");
            }
            if (string.IsNullOrEmpty(YaeherUserInfo.LoginPwd))
            {
                return new ObjectResultModule("", 100, "请先填写用户密码！");
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UserInfo = await _yaeherUserService.YaeherUserByID(userid);
            if (UserInfo != null)
            {
                //验证验证码
                var mes = new YaeherMessageRemindIn(); mes.AndAlso(t => !t.IsDelete && t.PhoneNumber == YaeherUserInfo.PhoneNumber && t.MessageType == "ChangePassword");
                var message = await _yaeherMessageRemindService.YaeherMessageRemindList(mes);
                if(message!=null)
                {
                    if (message[0].VerificationCode != YaeherUserInfo.VerificationCode)
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
               
                // 修改用户密码
                UserInfo.LoginPwd = YaeherUserInfo.LoginPwd;
                var Userresult = await _yaeherUserService.UpdateYaeherUser(UserInfo);
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
                OperExplain = "UserAuthentication",
                OperContent = JsonHelper.ToJson(YaeherUserInfo),
                OperType = "UserAuthentication",
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