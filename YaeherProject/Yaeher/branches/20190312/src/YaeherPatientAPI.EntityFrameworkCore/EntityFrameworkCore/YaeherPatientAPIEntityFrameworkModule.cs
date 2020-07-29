using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Yaeher;
using YaeherPatientAPI.EntityFrameworkCore.Seed;

namespace YaeherPatientAPI.EntityFrameworkCore
{
    [DependsOn(
        typeof(YaeherCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class YaeherPatientAPIEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<YaeherPatientAPIDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        YaeherPatientAPIDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        YaeherPatientAPIDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
           
            IocManager.RegisterAssemblyByConvention(typeof(YaeherPatientAPIEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
