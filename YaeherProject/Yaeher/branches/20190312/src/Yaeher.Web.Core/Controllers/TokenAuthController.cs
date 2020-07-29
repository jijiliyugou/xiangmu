using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Yaeher.Authentication.External;
using Yaeher.Authentication.JwtBearer;
using Yaeher.Authorization;
using Yaeher.Authorization.Users;
using Yaeher.Models.TokenAuth;
using Yaeher.MultiTenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Yaeher.Configuration;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Microsoft.AspNetCore.Cors;
using Yaeher.SystemConfig;
using Yaeher.Common.TencentCustom;
using Yaeher.SystemManage.Dto;
using Yaeher.SystemManage;
using Yaeher.YaeherDoctors;
using Abp.Domain.Uow;

namespace Yaeher.Controllers
{
    //启用跨域
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : YaeherControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IYaeherUserService _yaeherUserService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IUserManagerService _userManagerService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherUserPaymentService _yaeherUserPaymentService;
        private readonly ISystemConfigsService _systemConfigsService;
        private readonly ISystemTokenService _systemTokenService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public TokenAuthController(LogInManager logInManager,
                                    ITenantCache tenantCache,
                                    AbpLoginResultTypeHelper abpLoginResultTypeHelper,
                                    TokenAuthConfiguration configuration,
                                    IExternalAuthConfiguration externalAuthConfiguration,
                                    IExternalAuthManager externalAuthManager,
                                    UserRegistrationManager userRegistrationManager,
                                    IYaeherUserService yaeherUserService,
                                    IHostingEnvironment hostingEnvironment,
                                    IUserManagerService userManagerService,
                                    ISystemParameterService systemParameterService,
                                    IYaeherUserPaymentService yaeherUserPaymentService,
                                    ISystemConfigsService systemConfigsService,
                                    ISystemTokenService systemTokenService,
                                    IYaeherDoctorService yaeherDoctorService,
                                    IUnitOfWorkManager unitOfWorkManager)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _yaeherUserService = yaeherUserService;
            _userManagerService = userManagerService;
            _systemParameterService = systemParameterService;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
            _yaeherUserPaymentService = yaeherUserPaymentService;
            _systemConfigsService = systemConfigsService;
            _systemTokenService = systemTokenService;
            _yaeherDoctorService = yaeherDoctorService;
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        private async Task<YaeherUser> UserAsync(string url, string postData)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpContent httpContent = new StringContent(postData, Encoding.UTF8,
                                     "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var index = result.LastIndexOf(":{", StringComparison.Ordinal) + 1;
                    var length = result.LastIndexOf("},", StringComparison.Ordinal) - 9;
                    var fullfiletype = result.Substring(index, length);
                    var user = JsonHelper.FromJson<YaeherUser>(fullfiletype);
                    return user;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<ObjectResultModule> Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {

                ///配置文件判断来源
                ///1.当来源于Patient 患者端，开启http请求到Admin管理端查询用户名密码进行匹配.
                ///2.当来源于Admin管理端或者Doctor医生端则直接查询数据库进行匹配
                if (!Commons.CheckSecret(model.Secret))
                {
                    return new ObjectResultModule("", 422, "自签名错误！");
                }
                YaeherUser user = null;
                TencentUserManage usermanage = new TencentUserManage();
                // 获取微信WecharToken
                // SystemToken systemToken = new SystemToken();
                //  systemToken.TokenType = "Wechar";

                string openid = "";
                if (!string.IsNullOrEmpty(model.WXCode))//微信code登陆
                {
                    var Tokens = await _systemTokenService.SystemTokenList("Wechar");
                    //Logger.Info("modelWXCode:"+JsonHelper.ToJson(model));
                    ///调用微信接口获取openid
                    ///查找数据库
                    TencentWeCharEntity tencentWeCharEntity = new TencentWeCharEntity();
                    SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
                    systemConfigsIn.AndAlso(t => !t.IsDelete && t.SystemType == "TencentWechar");
                    var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
                    var tencentparam = configs.FirstOrDefault();
                    tencentWeCharEntity.grant_type = "authorization_code";
                    tencentWeCharEntity.appid = tencentparam.AppID;
                    tencentWeCharEntity.secret = tencentparam.AppSecret;
                    var usertoken = usermanage.WeiXinUserToken(model.WXCode, tencentWeCharEntity).Result;
                    if (usertoken == null || string.IsNullOrEmpty(usertoken.openid))
                    {
                        return new ObjectResultModule("", 400, "获取用户信息失败！");
                    }
                    openid = usertoken.openid;
                }
                else if (!string.IsNullOrEmpty(model.WecharOpenID))//微信openid登陆
                {
                    openid = model.WecharOpenID;
                }
                else
                {
                    if (_appConfiguration["Authentication:AUTHTO"] == "YaeherPatientAPI")//swagger登陆
                    {
                        var Content = "{\"LoginName\":\"" + model.UserNameOrEmailAddress + "\",\"LoginPwd\":\"" + model.Password + "\",\"Secret\":\"" + model.Secret + "\"}";
                        user = await UserAsync(_appConfiguration["Authentication:AUTHURL"], Content);
                    }
                    else//用户名密码登陆登陆
                    {
                        user = await _yaeherUserService.YaeherUserByExpress(t => (t.Email == model.UserNameOrEmailAddress || t.LoginName == model.UserNameOrEmailAddress || t.PhoneNumber == model.UserNameOrEmailAddress)
                                                                                    && t.LoginPwd == model.Password
                                                                                    && !t.IsDelete);
                    }
                    if (user == null || user.Id < 0)
                    {
                        return new ObjectResultModule("Login failed", 400, "用户名或者密码错误!");
                    }
                    if (!user.Enabled)
                    {
                        return new ObjectResultModule("Login failed", 400, "用户账号没激活，请联系管理员!");
                    }
                }
                SystemConfig.UserManager userManager = null;
                if (model.Platform == "PC")
                {
                    if (_appConfiguration["Authentication:AUTHTO"] != "YaeherPatientAPI")
                    {
                        userManager = _userManagerService.UserManager(user.Id);
                    }
                }
                else
                {
                    // 利用OPenID登陆
                    if (!string.IsNullOrEmpty(openid))
                    {
                        var Tokens = await _systemTokenService.SystemTokenList("Wechar");
                        try
                        {
                            var usermsg = usermanage.WeiXinUserInfoUtils(openid, Tokens.access_token).Result;
                            // 未关注不可进入系统
                            if (usermsg.subscribe != 0)
                            {
                                #region 同步提交
                                //using (var unitOfWork = _unitOfWorkManager.Begin())
                                //{
                                //    TencentWXPay tencentWXPay = new TencentWXPay();
                                //    user = _yaeherUserService.YaeherUserInfo(openid, Tokens.access_token);
                                //    if (user.Id > 0)
                                //    {
                                //        user = await usermanage.YaeherUserLable(usermsg, user, Tokens.access_token);
                                //        if (!user.IsPay)
                                //        {
                                //            var payment = await _yaeherUserPaymentService.YaeherUserPaymentByUserID(user.Id);
                                //            if (payment == null || payment.Id < 1)
                                //            {
                                //                //http请求微信信息，获取账户的信息 新增用户payment
                                //                var CreateUserPayment = new YaeherUserPayment()
                                //                {
                                //                    UserID = user.Id,
                                //                    FullName = user.FullName,
                                //                    PayMethod = "wxpay",
                                //                    PayMethodName = "微信支付",
                                //                    PaymentAccout = user.WecharName,
                                //                    BankName = "wx",
                                //                    Subbranch = "wx",
                                //                    BandAdd = "wx",
                                //                    BankNo = "wx",
                                //                    CreatedOn = DateTime.Now,
                                //                    IsDefault = true,
                                //                };
                                //                CreateUserPayment = await _yaeherUserPaymentService.CreateYaeherUserPayment(CreateUserPayment);
                                //            }
                                //            user.IsPay = true;
                                //        }
                                //        if (!user.IsUpdate)
                                //        {
                                //            var DoctorInfo = await _yaeherDoctorService.YaeherDoctorByUserID(user.Id);
                                //            if (DoctorInfo != null && DoctorInfo.IsSharing && user.IsProfitSharing == false)   //医生角色切没有生成分账账号
                                //            {
                                //                //查询分账配置
                                //                SystemConfigsIn systemConfigsIn = new SystemConfigsIn();
                                //                systemConfigsIn.AndAlso(a => a.IsDelete == false);
                                //                systemConfigsIn.AndAlso(a => a.SystemType == "TencentWechar");
                                //                var configs = await _systemConfigsService.SystemConfigsList(systemConfigsIn);
                                //                // 查询医生信息
                                //                var tencentparam = configs.ToList().FirstOrDefault();
                                //                var receiver = new receiver();
                                //                receiver.name = DoctorInfo.DoctorName;
                                //                receiver.type = "PERSONAL_OPENID";
                                //                receiver.account = openid;
                                //                var addresult = tencentWXPay.ProfitSharingAddReceiver(receiver, tencentparam).Result;
                                //                if (addresult.result_code == "SUCCESS")  //插入成功后更新状态
                                //                {
                                //                    user.IsProfitSharing = true;
                                //                }
                                //            }
                                //        }
                                //        user = await _yaeherUserService.UpdateYaeherUser(user);
                                //    }
                                //    unitOfWork.Complete();
                                //}
                                #endregion
                                string OperType = "用户登陆";
                                user = await _yaeherUserService.YaeherUserInfo(usermsg, Tokens.access_token, OperType);
                            }
                            else
                            {
                                return new ObjectResultModule("", 402, "用户未关注,请重新关注公众号！");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Info("我是一个通过页面进来的错误" + ex.ToString() + "DateTime:" + DateTime.Now);
                        }
                        if (user == null)
                        {
                            return new ObjectResultModule("Login failed", 400, "openid错误,请刷新页面!");
                        }
                        if (!user.Enabled)
                        {
                            return new ObjectResultModule("Login failed", 400, "用户账号没激活，请联系管理员!");
                        }
                    }
                    else
                    {
                        return new ObjectResultModule("", 402, "用户未关注,请重新关注公众号！");
                    }
                }
                // 登陆判断用户是否存在 
                if (user != null)
                {
                    var Identity = GenerateUserIdentity(user, userManager, ClaimTypes.NameIdentifier);
                    var accessToken = CreateAccessToken(CreateJwtClaims(Identity));
                    this.ObjectResultModule.Object = new AuthenticateResultModel
                    {
                        AccessToken = accessToken,
                        EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                        ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                        UserId = user.Id,
                        MobileRoleName = user.RoleName,
                        WecharOpenID = user.WecharOpenID,
                        userManager = userManager
                    };
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "sucess";
                    return this.ObjectResultModule;
                }
                else
                {
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "获取用户信息失败";
                    return this.ObjectResultModule;
                }
            }
            catch (Exception ex)
            {
                this.ObjectResultModule.Message = "error";
                this.ObjectResultModule.StatusCode = 500;
                this.ObjectResultModule.Object = ex.Message;
                return this.ObjectResultModule;
            }
        }
        private ClaimsIdentity GenerateUserIdentity(YaeherUser user, SystemConfig.UserManager userManager, string authenticationType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            string userdata = "";
            UserMemory userMemory = new UserMemory();
            userMemory.WecharOpenID = user.WecharOpenID;
            userMemory.MobileRoleName = user.RoleName;
            if (userManager != null)
            {
                userMemory.IsAdmin = userManager.IsAdmin;
                userMemory.IsCustomerService = userManager.IsCustomerService;
                userMemory.IsDoctor = userManager.IsDoctor;
                userMemory.IsQC = userManager.IsQC;
                userMemory.DoctorID = userManager.YaeherUserInfo.RoleName == "doctor" ? userManager.YaeherDoctorInfo.Id : 0;

            }
            userdata = JsonHelper.ToJson(userMemory);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, string.IsNullOrEmpty(user.LoginName)?"":user.LoginName),
                new Claim(ClaimTypes.Role,string.IsNullOrEmpty(user.RoleName)?"":user.RoleName),
                new Claim(ClaimTypes.UserData,userdata),
            };
            var identity = new ClaimsIdentity(claims, authenticationType, ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return identity;
        }
        //[HttpGet]
        private List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        //[HttpPost]
        private async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterExternalUserAsync(externalUser);
                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                WaitingForActivation = true
                            };
                        }

                        // Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                externalUser.Name,
                externalUser.Surname,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                Authorization.Users.User.CreateRandomPassword(),
                true
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            var roleClaim = claims.First(c => c.Type == ClaimTypes.Role);
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Role, roleClaim.Value),
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
    }
}
