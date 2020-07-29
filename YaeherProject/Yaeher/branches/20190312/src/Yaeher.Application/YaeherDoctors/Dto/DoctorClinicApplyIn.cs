using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生申请门诊
    /// </summary>
    public class DoctorClinicApplyIn : ListParameters<DoctorClinicApply>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(20)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生Id
        /// </summary>
        public int DoctorId { get; set; }
        /// <summary>
        /// 类型code
        /// </summary>
        public string ApplyType { get; set; }
        /// <summary>
        /// 科室id
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        public string ApplyRemark { get; set; }
        /// <summary>
        /// 执业证
        /// </summary>
        public string Certificateofpractice { get; set; }
        /// <summary>
        /// 资格证
        /// </summary>
        public string Qualificationcertificate { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string CheckRemark { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        public string CheckRes { get; set; }
    }
   
}
