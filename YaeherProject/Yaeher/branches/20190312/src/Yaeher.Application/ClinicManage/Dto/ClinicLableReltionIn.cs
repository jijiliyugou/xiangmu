using Abp.Application.Services.Dto;


namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 门诊与标签关系
    /// </summary>
    public class ClinicLableReltionIn : ListParameters<ClinicLableReltion>, IPagedResultRequest
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// ClinicName
        /// </summary>
        public string ClinicName { get; set; }
    }
}
