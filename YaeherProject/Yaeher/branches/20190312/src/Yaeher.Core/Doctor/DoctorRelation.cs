using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 医生与标签关系
    /// </summary>
    public class DoctorRelation : EntityBaseModule
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
        ///标签ID
        /// </summary>
        public int LableID { get; set; }
        /// <summary>
        ///标签名称
        /// </summary>
        [MaxLength(20)]
        public string LableName { get; set; }
        /// <summary>
        /// 标签JSON
        /// </summary>
        public string LableJSON { get; set; }


    }
}
