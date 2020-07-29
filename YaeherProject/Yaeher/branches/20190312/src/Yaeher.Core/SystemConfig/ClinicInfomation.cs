using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 科室信息维护 
    /// </summary>
    public class ClinicInfomation: EntityBaseModule
    {
        /// <summary>
        /// 门诊名称
        /// </summary>
        [MaxLength(20)]
        public virtual string ClinicName { get; set; }
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
        /// 1成人,2儿童,默认成人
        /// </summary>
        public virtual int ClinicType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int OrderSort { get; set; }
    }
}
