using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 注册质控
    /// </summary>
    public class QualityCommitteeRegister : EntityBaseModule
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
        public virtual int  DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        [MaxLength(20)]
        public virtual string Accomplish { get; set; }
        /// <summary>
        /// 申请理由
        /// </summary>
        [MaxLength(1000)]
        public virtual string ApplyRemark { get; set; }
        /// <summary>
        /// 申请类型    申请质控 申请停用质控
        /// </summary>
        [MaxLength(20)]
        public virtual string ApplyState { get; set; }
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
        public virtual DateTime  CheckTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual int Checker { get; set; }

    }
}
