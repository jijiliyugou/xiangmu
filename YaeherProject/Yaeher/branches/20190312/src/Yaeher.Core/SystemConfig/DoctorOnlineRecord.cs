using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 医生上下线设置记录
    /// </summary>
    public class DoctorOnlineRecord : EntityBaseModule
    {
        /// <summary>
        /// 医生名称
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public virtual string DoctorJSON { get; set; }
        /// <summary>
        /// 上下线状态
        /// </summary>
        [MaxLength(10)]
        public virtual string OnlineState { get; set; }
        /// <summary>
        /// 分成设置
        /// </summary>
        public virtual double DivideInto { get; set; }

        /// <summary>
        ///   医生资费调价幅度百分比(%)
        /// </summary>
        public double DoctorMoneyExchange{get;set;}
        /// <summary>
        /// 医生资费调价时间
        /// </summary>
        public virtual int DoctorMoneyexTime { get; set; }

        /// <summary>
        /// 收入时间设置
        /// </summary>
        public virtual int IncomeDay { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [MaxLength(2000)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [MaxLength(10)]
        public virtual string CheckState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public virtual string CheckRemark { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime CheckTime { get; set; } 
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual int Checker { get; set; }

    }
}
