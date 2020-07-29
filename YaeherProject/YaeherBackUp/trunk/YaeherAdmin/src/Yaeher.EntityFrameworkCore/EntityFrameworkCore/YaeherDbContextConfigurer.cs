using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Yaeher.EntityFrameworkCore
{
    public static class YaeherDbContextConfigurer
    {

        /// <summary>
        /// 数据库连接方式
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        public static void Configure(DbContextOptionsBuilder<YaeherDbContext> builder, string connectionString)
        {
            //builder.UseSqlServer(connectionString);
            builder.UseMySql(connectionString);
        }
        /// <summary>
        /// 数据库连接方式
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connection"></param>
        public static void Configure(DbContextOptionsBuilder<YaeherDbContext> builder, DbConnection connection)
        {
            //builder.UseSqlServer(connection);
            builder.UseMySql(connection);
        }
    }
}
