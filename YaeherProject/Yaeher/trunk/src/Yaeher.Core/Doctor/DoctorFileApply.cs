using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 医生申请上传文件
    /// </summary>
    public class DoctorFileApply : EntityBaseModule
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
        ///文件用途  申请上传，转科室上传、增加科室上传
        /// </summary>
        [MaxLength(20)]
        public string DocumentsUse { get; set; }
        /// <summary>
        ///上传类型   身份证 职业证 资格证
        /// </summary>
        [MaxLength(200)]
        public string FileType { get; set; }
        /// <summary>
        /// 身份证上 下
        /// </summary>
        [MaxLength(200)]
        public string TypeDetail { get; set; }
        /// <summary>
        /// 上传地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 医生申请或修改科室Id
        /// </summary>
        public int DoctorClinicApplyId{get;set;}

    }
}
