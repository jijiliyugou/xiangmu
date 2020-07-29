using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.Doctor;

namespace Yaeher
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class YaeherUserInfoView
    {
        /// <summary>
        /// 个人基本信息
        /// </summary>
        public YaeherUser yaeherUser { get; set; }
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public YaeherDoctor yaeherDoctor { get; set; }
        /// <summary>
        /// 认证资料
        /// </summary>
        public List<DoctorFileApply> doctorAppliesFile { get; set; }
        /// <summary>
        /// 科室与医生关系
        /// </summary>
        public List<DoctorClinicInfo> clinicDoctorReltions { get; set; }
        /// <summary>
        /// 医生从业经历
        /// </summary>
        public List<DoctorEmployment> doctorEmployments { get; set; }
    }

    /// <summary>
    /// 科室与医生关系
    /// </summary>
    public class DoctorClinicInfo
    {
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
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
        public virtual String DoctorName { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public virtual String DoctorJSON { get; set; }
        /// <summary>
        /// 申请科室类型
        /// </summary>
        public string ApplyType { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>
        public string CheckRes { get; set; }
        /// <summary>
        /// 科室认证资料
        /// </summary>
        public List<DoctorFileApply> DoctorClinicFileApplies { get; set; }
    }
}
