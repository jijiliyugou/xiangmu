using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace YaeherDoctorAPI.EntityFrameworkCore
{
    public static class YaeherDoctorAPIDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<YaeherDoctorAPIDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<YaeherDoctorAPIDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
