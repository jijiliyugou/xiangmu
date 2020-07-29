using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace YaeherPatientAPI.EntityFrameworkCore
{
    public static class YaeherPatientAPIDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<YaeherPatientAPIDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<YaeherPatientAPIDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
