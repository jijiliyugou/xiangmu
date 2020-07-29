using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 门诊与医生关系
    /// </summary>
    public class ClinicDoctorReltion:EntityBaseModule
    {
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
        [MaxLength(20)]
        public virtual String ClinicName { get; set; }
        /// <summary>
        /// 门诊JSON
        /// </summary>
        public virtual String ClinicJSON { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
        public virtual String DoctorName { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public virtual String DoctorJSON { get; set; }
    }
}
