using Abp.Application.Services.Dto;
using System;

namespace Yaeher.Quality.Dto
{
    /// <summary>
    /// 质控委员注册
    /// </summary>
    public class QualityCommitteeRegisterIn : ListParameters<QualityCommitteeRegister>, IPagedResultRequest
    {
        /// <summary>
        /// 门诊名称
        /// </summary>
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
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        public virtual string Accomplish { get; set; }
        /// <summary>
        /// 申请理由
        /// </summary>
        public virtual string ApplyRemark { get; set; }
        /// <summary>
        /// 申请类型    申请质控 申请停用质控
        /// </summary>
        public virtual string ApplyState { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
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
