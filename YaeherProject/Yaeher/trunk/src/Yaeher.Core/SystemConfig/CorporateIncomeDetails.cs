using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 公司收入明细
    /// </summary>
    public class CorporateIncomeDetails : EntityBaseModule
    {
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(30)]
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
        [MaxLength(500)]
        public virtual string ConsultantName { get; set; }
        /// <summary>
        /// 患者ID
        /// </summary>
        public virtual int PatientID { get; set; }
        /// <summary>
        /// 患者名称
        /// </summary>
        [MaxLength(500)]
        public virtual string PatientName { get; set; }
        /// <summary>
        /// 咨询医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 订单单号
        /// </summary>
        [MaxLength(20)]
        public virtual string OrderNumber { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        [MaxLength(20)]
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
