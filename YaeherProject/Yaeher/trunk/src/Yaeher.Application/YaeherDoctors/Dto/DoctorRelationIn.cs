using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生与标签关系
    /// </summary>
    public class DoctorRelationIn : ListParameters<DoctorRelation>, IPagedResultRequest
    {
        /// <summary>
        /// 医生名称
        /// </summary>
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
        public string LableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LableJSON { get; set; }
        /// <summary>
        /// 科室ID
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// 标签IDlist
        /// </summary>
        public string LableIDJSON { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DoctorClinicIn : ListParameters<ClinicDoctorReltion>, IPagedResultRequest
    {
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
        public virtual String ClinicName { get; set; }
        /// <summary>
        /// 门诊JSON
        /// </summary>
        public virtual String ClinicJSON { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public virtual String DoctorName { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public virtual String DoctorJSON { get; set; }
        /// <summary>
        /// 科室IDJSON
        /// </summary>
        public virtual String ClinicIDJSON { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DoctorLale : DoctorRelation
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int OrderSort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string LableRemark { get; set; }
    }
}
