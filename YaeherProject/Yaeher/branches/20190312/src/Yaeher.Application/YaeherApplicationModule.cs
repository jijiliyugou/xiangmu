using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yaeher.Authorization;

namespace Yaeher
{
    [DependsOn(
        typeof(YaeherCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class YaeherApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<YaeherAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(YaeherApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
