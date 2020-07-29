using Abp.Application.Services.Dto;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 门诊与医生关系
    /// </summary>
    public class ClinicDoctorReltionIn : ListParameters<ClinicDoctorReltion>, IPagedResultRequest
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
        public string ClinicName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public virtual string DoctorName { get; set; }
    }
}
