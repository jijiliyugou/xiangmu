using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Yaeher.LableManages.Dto;
using Yaeher.NumericalStatement.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 科室信息查询
    /// </summary>
    public class ClinicInfomationView
    {
        /// <summary>
        /// 门诊Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
        [MaxLength(20)]
        public virtual string ClinicName { get; set; }
        /// <summary>
        /// 科室类型 1成人2儿童
        /// </summary>
        public int ClinicType { get; set; }
        /// <summary>
        /// 门诊说明
        /// </summary>
        [MaxLength(100)]
        public virtual string ClinicIntro { get; set; }
        /// <summary>
        /// 门诊负责人  用户ID
        /// </summary>
        public virtual int ClinicDirector { get; set; }
        /// <summary>
        /// 门诊医生总数
        /// </summary>
        public virtual int DoctorCount { get; set; }
        /// <summary>
        /// 科室标签
        /// </summary>
        public IList<LabelClinicManage> lableManages { get; set; }
        /// <summary>
        /// 医生信息
        /// </summary>
        public List<DoctorDetailList> doctorDetailList { get; set; }

    }
}
