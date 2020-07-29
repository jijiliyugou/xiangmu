using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 分享医生
    /// </summary>
    public class ShareDoctor : EntityBaseModule
    {
        /// <summary>
        /// 分享人ID
        ///YaeherUser表ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        /// 分享人姓名
        ///YaeherUser表fullname
        /// </summary>
        [MaxLength(500)]
        public string PatientName { get; set; }
        /// <summary>
        /// 分享人Json
        ///YaeherUser表fullname
        /// </summary>
        [MaxLength(20)]
        public string PatientJSON { get; set; }
        /// <summary>
        /// 医生Id
        /// YaeherUser表ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        ///YaeherUser表fullname
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生Json
        /// </summary>  
        public string DoctorJSON { get; set; }

    }
}
