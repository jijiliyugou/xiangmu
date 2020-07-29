using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 科室基本信息 以及科室对应标签  科室对应医生   医生对应标签 医生基本设置信息
    /// </summary>
    public class ClinicInfo
    {
        /// <summary>
        /// 科室基本信息
        /// </summary>
        public ClinicInfomation clinicInfomation { get; set; }
        /// <summary>
        /// 科室与医生关系
        /// </summary>
        public List<ClinicDoctorReltion> clinicDoctorReltion { get; set; }
        /// <summary>
        /// 科室与标签关系
        /// </summary>
        public List<ClinicLableReltion> clinicLableReltion { get; set; }
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public List<YaeherDoctorList> yaeherDoctorList { get; set; }

    }

    /// <summary>
    /// 医生基本信息
    /// </summary>
    public class YaeherDoctorList
    {
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public YaeherDoctor yaeherDoctor { get; set; }
        /// <summary>
        /// 医生与标签关系
        /// </summary>
        public List<DoctorRelation> doctorRelation { get; set; }
        /// <summary>
        /// 医生上下线设置状态
        /// </summary>

        public DoctorOnlineRecord doctorOnlineRecord { get; set; }
        /// <summary>
        /// 医生申请文件
        /// </summary>

        public List<DoctorFileApply> doctorFileApply { get; set; }
        /// <summary>
        /// 医生排班
        /// </summary>

        public List<DoctorScheduling> doctorScheduling { get; set; }
        /// <summary>
        /// 医生提供服务信息
        /// </summary>

        public List<ServiceMoneyList> serviceMoneyList { get; set; }
    }
}
