using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 推荐排序
    /// </summary>
    public class RecommendedOrdering: EntityBaseModule
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int ItemSort { get; set; }
    }
}
