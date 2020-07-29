using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Yaeher.Authentication.JwtBearer;
using Yaeher.Configuration;
using YaeherDoctorAPI.EntityFrameworkCore;
using YaeherPatientAPI.EntityFrameworkCore;
using Abp.Configuration.Startup;
using Abp.Runtime.Caching.Redis;

namespace Yaeher
{
    [DependsOn(
         typeof(YaeherApplicationModule),
         typeof(YaeherDoctorAPIEntityFrameworkModule),
         typeof(YaeherPatientAPIEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        , typeof(AbpAspNetCoreSignalRModule)
         , typeof(AbpRedisCacheModule)
     )]
    //[DependsOn(
    //     typeof(YaeherApplicationModule),
    //     typeof(AbpAspNetCoreModule)
    //    , typeof(AbpAspNetCoreSignalRModule)
    // )]
    public class YaeherWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YaeherWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //审计日志取消
            Configuration.Auditing.IsEnabled = false;
            //错误日志打印
            Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;

            //配置使用Redis缓存
            Configuration.Caching.UseRedis(options => {

                options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
                options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
            }
                );
            //配置指定的Cache过期时间为10分钟
            Configuration.Caching.Configure("YaeherClinicDoctorsPage", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(10);
            });
            Configuration.Caching.Configure("DoctorDetailList", cache =>
          {
              cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(10);
          });
            Configuration.Caching.Configure("DoctorReply", cache =>
          {
              cache.DefaultSlidingExpireTime = TimeSpan.FromDays(1);
          });

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                YaeherConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(YaeherApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            // tokenAuthConfig.Expiration = TimeSpan.FromDays(1000);
            tokenAuthConfig.Expiration = TimeSpan.FromHours(2);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YaeherWebCoreModule).GetAssembly());
        }
    }
}
