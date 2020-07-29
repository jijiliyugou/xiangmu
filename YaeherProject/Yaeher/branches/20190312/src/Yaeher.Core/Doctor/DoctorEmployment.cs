using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.Doctor
{
    /// <summary>
    /// 医生从业
    /// </summary>
    public class DoctorEmployment : EntityBaseModule
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 工作医院
        /// </summary>
        [MaxLength(200)]
        public string HospitalName { get; set; }
        /// <summary>
        /// 所在科室
        /// </summary>
        [MaxLength(20)]
        public string Department { get; set; }
        /// <summary>
        /// 工作年限
        /// </summary>
        public double WorkYear { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [MaxLength(20)]
        public string Title { get; set; }
        /// <summary>
        ///起始日期
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        ///结束日期
        /// </summary>
        public DateTime EndTime { get; set; }

    }
}
