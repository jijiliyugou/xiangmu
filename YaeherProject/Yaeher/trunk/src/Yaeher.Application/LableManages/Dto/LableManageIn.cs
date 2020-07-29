using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Yaeher.LableManages.Dto
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class LableManageIn : ListParameters<LableManage>, IPagedResultRequest
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string LableName { get; set; }
        /// <summary>
        /// 标签说明
        /// </summary>
        public virtual string LableRemark { get; set; }
        /// <summary>
        /// 门诊Id
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 关联表Id
        /// </summary>
        public virtual int DoctorRelationID { get; set; }
        /// <summary>
        /// 医生id
        /// </summary>
        public virtual int DoctorID { get; set; }
    }
    /// <summary>
    /// 带科室id的标签管理
    /// </summary>
    public class LabelClinicManage : LableManage
    {
        /// <summary>
        /// 门诊Id
        /// </summary>
        public virtual int ClinicID { get; set; }
    }
    public class LabelClinicManageIn
    {
        public virtual int ClinicID { get; set; }

    }
    /// <summary>
    /// 带科室id的标签管理
    /// </summary>
    public class LabelDoctorManage : LableManage
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        public virtual int DoctorID { get; set; }
    }
    public class LabelDoctorManageIn
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        public virtual int DoctorID { get; set; }
    }
}
