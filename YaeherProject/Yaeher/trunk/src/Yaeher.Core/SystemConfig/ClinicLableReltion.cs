using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 科室与标签的关系
    /// </summary>
    public class ClinicLableReltion : EntityBaseModule
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        [MaxLength(20)]
        public virtual String ClinicName { get; set; }
        /// <summary>
        /// 科室JSon
        /// </summary>
        public virtual String ClinicJSON { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public virtual int LableID { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [MaxLength(20)]
        public virtual String LableName { get; set; }
        /// <summary>
        /// 标签JSon
        /// </summary>
        public virtual String LableJSON { get; set; }
    }
}
