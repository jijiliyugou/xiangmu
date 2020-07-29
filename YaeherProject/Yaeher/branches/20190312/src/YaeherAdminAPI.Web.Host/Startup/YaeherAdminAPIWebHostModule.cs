using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yaeher.Configuration;
using Yaeher;

namespace YaeherAdminAPI.Web.Host.Startup
{
    [DependsOn(
       typeof(YaeherWebCoreModule))]
    public class YaeherAdminAPIWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public YaeherAdminAPIWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YaeherAdminAPIWebHostModule).GetAssembly());
        }
    }
}
