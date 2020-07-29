using Abp.Application.Services.Dto;

namespace Yaeher.CompaniesReport.Dto
{
    /// <summary>
    /// 公司收入明细
    /// </summary>
    public class CorporateIncomeDetailsIn : ListParameters<CorporateIncomeDetails>, IPagedResultRequest
    {
        /// <summary>
        /// 咨询单号
        /// </summary>
        public virtual string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询ID
        /// </summary>
        public virtual int ConsultID { get; set; }
        /// <summary>
        /// 咨询用户ID
        /// </summary>
        public virtual int ConsultantID { get; set; }
        /// <summary>
        /// 咨询用户
        /// </summary>
        public virtual string ConsultantName { get; set; }
        /// <summary>
        /// 患者ID
        /// </summary>
        public virtual int PatientID { get; set; }
        /// <summary>
        /// 患者名称
        /// </summary>
        public virtual string PatientName { get; set; }
        /// <summary>
        /// 咨询医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 订单单号
        /// </summary>
        public virtual string OrderNumber { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        public virtual string OrderCurrency { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public virtual decimal OrderMoney { get; set; }
        /// <summary>
        /// 分成金额
        /// </summary>
        public virtual decimal ProportionMoney { get; set; }

    }
}
