using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 医生审核
    /// </summary>
    public class DoctorCheck : EntityBaseModule
    {
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
        /// 医生JSon
        /// </summary>
        public virtual string DoctorJSon { get; set; }
        /// <summary>
        /// 审核类型  基础审核  认证审核
        /// </summary>
        [MaxLength(20)]
        public virtual string CheckType { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [MaxLength(20)]
        public virtual string CheckState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(1000)]
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
