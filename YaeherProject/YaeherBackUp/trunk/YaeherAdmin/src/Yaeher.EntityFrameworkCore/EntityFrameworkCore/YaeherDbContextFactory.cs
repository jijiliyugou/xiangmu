using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Yaeher.Configuration;
using Yaeher.Web;

namespace Yaeher.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class YaeherDbContextFactory : IDesignTimeDbContextFactory<YaeherDbContext>
    {
        public YaeherDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<YaeherDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            YaeherDbContextConfigurer.Configure(builder, configuration.GetConnectionString(YaeherConsts.ConnectionStringName));

            return new YaeherDbContext(builder.Options);
        }
    }
}
