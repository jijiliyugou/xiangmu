using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Yaeher.Authorization.Roles;
using Yaeher.Authorization.Users;
using Yaeher.MultiTenancy;
using Yaeher;
using Yaeher.Doctor;
using Yaeher.SystemConfig;
using Yaeher.NumericalStatement;
using Yaeher.EventEntitys;
using Yaeher.HangFire;

namespace YaeherDoctorAPI.EntityFrameworkCore
{
    public class YaeherDoctorAPIDbContext : AbpZeroDbContext<Tenant, Role, User, YaeherDoctorAPIDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public YaeherDoctorAPIDbContext(DbContextOptions<YaeherDoctorAPIDbContext> options)
            : base(options)
        {
        }

        public DbSet<YaeherDoctor> YaeherDoctor { get; set; }
        public DbSet<DoctorRelation> DoctorRelation { get; set; }
        public DbSet<DoctorFileApply> DoctorFileApply { get; set; }
        public DbSet<DoctorClinicApply> DoctorClinicApply { get; set; }
        public DbSet<ServiceMoneyList> ServiceMoneyList { get; set; }
        public DbSet<DoctorServiceLog> DoctorServiceLog { get; set; }
        public DbSet<DoctorPaper> DoctorPaper { get; set; }
        public DbSet<DoctorIncome> DoctorIncome { get; set; }
        public DbSet<IncomeDetails> IncomeDetails { get; set; }
        public DbSet<YaeherConsultation> YaeherConsultation { get; set; }
        public DbSet<ConsultationReply> ConsultationReply { get; set; }
        public DbSet<ConsultationEvaluation> ConsultationEvaluation { get; set; }
        public DbSet<PhoneReplyRecord> PhoneReplyRecord { get; set; }
        public DbSet<OrderManage> OrderManage { get; set; }
        public DbSet<OrderTradeRecord> OrderTradeRecord { get; set; }
        public DbSet<RefundManage> RefundManage { get; set; }
        public DbSet<IncomeDevide> IncomeDevide { get; set; }
        public DbSet<OrderTimeoutReminder> OrderTimeoutReminder { get; set; }
        public DbSet<OrderEarlyWarning> OrderEarlyWarning { get; set; }
        public DbSet<DoctorScheduling> DoctorScheduling { get; set; }
        public DbSet<QualityControlManage> QualityControlManage { get; set; }
        public DbSet<YaeherUser> YaeherUser { get; set; }
        public DbSet<YaeherUserPayment> YaeherUserPayment { get; set; }
        public DbSet<AttachmentService> AttachmentService { get; set; }
        public DbSet<ClinicInfomation> ClinicInfomation { get; set; }
        public DbSet<ClinicDoctorReltion> ClinicDoctorReltion { get; set; }
        public DbSet<ClinicLableReltion> ClinicLableReltion { get; set; }
        public DbSet<ReleaseManage> ReleaseManage { get; set; }
        public DbSet<QuestionRelease> QuestionRelease { get; set; }
        public DbSet<ArticleOperList> ArticleOperList { get; set; }
        public DbSet<LableManage> LableManage { get; set; }
        public DbSet<DoctorOnlineRecord> DoctorOnlineRecord { get; set; }
        public DbSet<DoctorOnlineSetLog> DoctorOnlineSetLog { get; set; }
        public DbSet<DoctorCheck> DoctorCheck { get; set; }
        public DbSet<AreaManage> AreaManage { get; set; }
        public DbSet<DoctorWithdrawRecord> DoctorWithdrawRecord { get; set; }
        public DbSet<SystemParameter> SystemParameter { get; set; }
        public DbSet<DoctorParaSet> DoctorParaSet { get; set; }
        public DbSet<InterfaceSet> InterfaceSet { get; set; }
        public DbSet<CorporateIncomeTotal> CorporateIncomeTotal { get; set; }
        public DbSet<CorporateIncomeDetails> CorporateIncomeDetails { get; set; }
        public DbSet<YaeherRole> YaeherRole { get; set; }
        public DbSet<YaeherUserRole> YaeherUserRole { get; set; }
        public DbSet<YaeherModule> YaeherModule { get; set; }
        public DbSet<YaeherRoleModule> YaeherRoleModule { get; set; }
        public DbSet<QualityCommitteeRegister> QualityCommitteeRegister { get; set; }
        public DbSet<QualityCommittee> QualityCommittee { get; set; }
        public DbSet<DoctorRules> DoctorRules { get; set; }
        public DbSet<YaeherOperList> YaeherOperList { get; set; }
        public DbSet<YaeherMessageRemind> YaeherMessageRemind { get; set; }
        public DbSet<DoctorEmployment> DoctorEmployment { get; set; }
        public DbSet<CollectConsultation> CollectConsultation { get; set; }
        public DbSet<RecommendedOrdering> RecommendedOrdering { get; set; }
        public DbSet<CompanyConfig> CompanyConfig { get; set; }
        public DbSet<EvaluationTotal> EvaluationTotal { get; set; }
        public DbSet<ConsultationOrderTotal> ConsultationOrderTotal { get; set; }
        public DbSet<SystemConfigs> SystemConfigs { get; set; }
        public DbSet<Publishs> Publishs { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<ReceiveEvent> ReceiveEvent { get; set; }
        public DbSet<Subscribetion> Subscribetion { get; set; }
        public DbSet<YaeherPhone> YaeherPhone { get; set; }
        public DbSet<HangFireJob> HangFireJob { get; set; }
        public DbSet<YaeherBanner> YaeherBanner { get; set; }
        /// <summary>
        /// 2018-11-26
        /// </summary>
        public DbSet<YaeherLabelConfig> YaeherLabelConfig { get; set; }
        public DbSet<YaeherConditionalMenu> YaeherConditionalMenu { get; set; }
        public DbSet<YaeherMessageTemplate> YaeherMessageTemplate { get; set; }
        public DbSet<WecharSendMessage> WecharSendMessage { get; set; }
        public DbSet<SendMessageTemplate> SendMessageTemplate { get; set; }
        public DbSet<AcceptTencentWechar> AcceptTencentWechar { get; set; }
        /// <summary>
        /// 2018-12-06
        /// </summary>
        public DbSet<AcceptWecharState> AcceptWecharState { get; set; }
        public DbSet<RelationLabelGroup> RelationLabelGroup { get; set; }
        public DbSet<RelationLabelList> RelationLabelList { get; set; }
        public DbSet<SystemToken> SystemToken { get; set; }
        /// <summary>
        /// 2019-01-14
        /// </summary>
        public DbSet<QuickReply> QuickReply { get; set; }

    }
}
