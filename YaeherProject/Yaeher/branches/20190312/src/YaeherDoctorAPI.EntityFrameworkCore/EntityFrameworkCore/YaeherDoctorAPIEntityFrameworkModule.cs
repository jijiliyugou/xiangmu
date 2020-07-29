using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Yaeher;
using YaeherDoctorAPI.EntityFrameworkCore.Seed;

namespace YaeherDoctorAPI.EntityFrameworkCore
{
    [DependsOn(
        typeof(YaeherCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class YaeherDoctorAPIEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<YaeherDoctorAPIDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        YaeherDoctorAPIDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        //if (options.ConnectionString.Contains("YaeherPatientAPIDb"))
                        //{ return; }
                        YaeherDoctorAPIDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YaeherDoctorAPIEntityFrameworkModule).GetAssembly());
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
