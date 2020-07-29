using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的医生
    /// </summary>
    public class YaeherPatientDoctor : EntityBaseModule
    {
        /// <summary>
        /// 用户id
        /// YaeherUser表ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        ///医生ID
        ///YaeherUser表ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        ///医生名称
        ///YaeherUser表fullname
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        ///医生JSON 存储医生所有信息
        /// </summary>
        public string DoctorJSON { get; set; }

    }
}
