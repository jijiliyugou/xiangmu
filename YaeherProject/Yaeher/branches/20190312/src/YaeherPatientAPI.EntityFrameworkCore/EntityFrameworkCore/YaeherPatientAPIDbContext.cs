using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Yaeher.Authorization.Roles;
using Yaeher.Authorization.Users;
using Yaeher.MultiTenancy;
using Yaeher;
using Yaeher.EventEntitys;

namespace YaeherPatientAPI.EntityFrameworkCore
{
    public class YaeherPatientAPIDbContext : AbpZeroDbContext<Tenant, Role, User, YaeherPatientAPIDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public YaeherPatientAPIDbContext(DbContextOptions<YaeherPatientAPIDbContext> options)
            : base(options)
        {
        }
        public DbSet<YaeherPatientLeaguerInfo> YaeherPatientLeaguerInfo { get; set; }
        public DbSet<YaeherPatientCollection> YaeherPatientCollection { get; set; }
        public DbSet<YaeherPatientDoctor> YaeherPatientDoctor { get; set; }
        public DbSet<YaeherConsultation> YaeherConsultation { get; set; }
        public DbSet<ConsultationReply> ConsultationReply { get; set; }
        public DbSet<ConsultationEvaluation> ConsultationEvaluation { get; set; }
        public DbSet<PhoneReplyRecord> PhoneReplyRecord { get; set; }
        public DbSet<OrderManage> OrderManage { get; set; }
        public DbSet<OrderTradeRecord> OrderTradeRecord { get; set; }
        public DbSet<RefundManage> RefundManage { get; set; }
        public DbSet<ShareDoctor> ShareDoctor { get; set; }
        public DbSet<Publishs> Publishs { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<ReceiveEvent> ReceiveEvent { get; set; }
        public DbSet<Subscribetion> Subscribetion { get; set; }
        public DbSet<YaeherOperList> YaeherOperList { get; set; }
    }
}
