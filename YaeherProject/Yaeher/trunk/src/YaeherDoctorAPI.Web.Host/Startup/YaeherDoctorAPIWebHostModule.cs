using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yaeher.Configuration;
using Yaeher;

namespace YaeherDoctorAPI.Web.Host.Startup
{
    [DependsOn(
       typeof(YaeherWebCoreModule))]
    public class YaeherDoctorAPIWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YaeherDoctorAPIWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YaeherDoctorAPIWebHostModule).GetAssembly());
        }
    }
}
