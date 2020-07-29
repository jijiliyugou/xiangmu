using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 医生申请门诊
    /// </summary>
    public class DoctorClinicApply : EntityBaseModule
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public string DoctorJSON { get; set; }
        /// <summary>
        /// 申请类型
        /// 增加  更换门诊
        /// </summary>
        [MaxLength(10)]
        public string ApplyType { get; set; }
        /// <summary>
        /// 申请门诊ID
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// 申请门诊名称
        /// </summary>
        [MaxLength(20)]
        public string ClinicName { get; set; }
        /// <summary>
        /// 申请门诊JSON
        /// </summary>
        public string ClinicJSON { get; set; }
        /// <summary>
        /// 申请备注原因
        /// </summary>
        [MaxLength(2000)]
        public string ApplyRemark { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(1000)]
        public string CheckRemark { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        [MaxLength(10)]
        public string CheckRes { get; set; }


    }
}
