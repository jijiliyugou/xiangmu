using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生上传文件
    /// </summary>
    public class DoctorFileApplyIn : ListParameters<DoctorFileApply>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        ///文件用途  申请上传，转科室上传、增加科室上传
        /// </summary>
        public string DocumentsUse { get; set; }
        /// <summary>
        ///上传类型   身份证 职业证 资格证
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 身份证上 下
        /// </summary>
        public string TypeDetail { get; set; }
        /// <summary>
        /// 上传地址
        /// </summary>
        public string Address { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DoctorFileApplyList : ListParameters<DoctorFileApply>, IPagedResultRequest
    {

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 申请类型
        /// </summary>
        public string AuthType { get; set; }
        /// <summary>
        /// 上传状态
        /// </summary>
        public string AuthCheckRes { get; set; }

        /// <summary>
        /// 上传资料记录
        /// </summary>
        public List<DoctorFileApply> doctorFileApplies { get; set; }
    }
}
