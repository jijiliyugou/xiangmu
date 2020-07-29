using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Yaeher;
using Yaeher.Configuration;
using Yaeher.Web;

namespace YaeherDoctorAPI.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class YaeherDoctorAPIDbContextFactory : IDesignTimeDbContextFactory<YaeherDoctorAPIDbContext>
    {
        public YaeherDoctorAPIDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<YaeherDoctorAPIDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder("YaeherDoctorAPI"));

            YaeherDoctorAPIDbContextConfigurer.Configure(builder, configuration.GetConnectionString(YaeherConsts.ConnectionStringName));

            return new YaeherDoctorAPIDbContext(builder.Options);
        }
    }
}
