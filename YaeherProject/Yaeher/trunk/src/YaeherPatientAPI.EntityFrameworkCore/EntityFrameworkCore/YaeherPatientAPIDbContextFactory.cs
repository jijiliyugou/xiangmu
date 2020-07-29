using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Yaeher;
using Yaeher.Configuration;
using Yaeher.Web;

namespace YaeherPatientAPI.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class YaeherPatientAPIDbContextFactory : IDesignTimeDbContextFactory<YaeherPatientAPIDbContext>
    {
        public YaeherPatientAPIDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<YaeherPatientAPIDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder("YaeherPatientAPI"));

            YaeherPatientAPIDbContextConfigurer.Configure(builder, configuration.GetConnectionString(YaeherConsts.ConnectionStringName));

            return new YaeherPatientAPIDbContext(builder.Options);
        }
    }
}
