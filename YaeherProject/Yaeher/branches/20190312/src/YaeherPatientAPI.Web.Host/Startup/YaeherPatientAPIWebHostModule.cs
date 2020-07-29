using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yaeher.Configuration;
using Yaeher;

namespace YaeherPatientAPI.Web.Host.Startup
{
    [DependsOn(
       typeof(YaeherWebCoreModule))]
    public class YaeherPatientAPIWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YaeherPatientAPIWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YaeherPatientAPIWebHostModule).GetAssembly());
        }
    }
}
