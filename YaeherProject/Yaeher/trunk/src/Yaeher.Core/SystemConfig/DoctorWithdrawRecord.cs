using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 医生提现记录表
    /// </summary>
    public class DoctorWithdrawRecord : EntityBaseModule
    {
        /// <summary>
        /// 提现流水号
        /// </summary>
        [MaxLength(20)]
        public virtual string SequenceNo { get; set; }
        /// <summary>
        /// 医生姓名
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
        /// 提现金额
        /// </summary>
        public virtual decimal WithdrawMoney { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public virtual DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [MaxLength(10)]
        public virtual string CheckState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(2000)]
        public virtual string CheckRemark { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual int Checker { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime CheckTime { get; set; }

    }
}
