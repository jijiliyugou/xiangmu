using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Yaeher.Authorization.Roles;
using Yaeher.Authorization.Users;
using Yaeher.MultiTenancy;

namespace Yaeher.EntityFrameworkCore
{
    public class YaeherDbContext : AbpZeroDbContext<Tenant, Role, User, YaeherDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public YaeherDbContext(DbContextOptions<YaeherDbContext> options)
            : base(options)
        {
        }
    }
}
