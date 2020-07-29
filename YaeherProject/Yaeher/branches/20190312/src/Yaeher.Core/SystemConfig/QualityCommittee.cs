using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 质控委员
    /// </summary>
    public class QualityCommittee : EntityBaseModule
    {
        /// <summary>
        /// 门诊名称
        /// </summary>
        [MaxLength(20)]
        public virtual string ClinicName { get; set; }
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        [MaxLength(200)]
        public virtual string Accomplish { get; set; }
        /// <summary>
        /// 状态  启用 停用
        /// </summary>
        [MaxLength(20)]
        public virtual string QualityState { get; set; }

    }
}
