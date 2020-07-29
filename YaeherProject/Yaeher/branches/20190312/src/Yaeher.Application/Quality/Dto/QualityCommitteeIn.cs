using Abp.Application.Services.Dto;


namespace Yaeher.Quality.Dto
{
    /// <summary>
    /// 质控委员会
    /// </summary>
    public class QualityCommitteeIn : ListParameters<QualityCommittee>, IPagedResultRequest
    {
        /// <summary>
        /// 门诊名称
        /// </summary>
        public virtual string ClinicName { get; set; }
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        public virtual string Accomplish { get; set; }
        /// <summary>
        /// 状态  启用 停用
        /// </summary>
        public virtual string QualityState { get; set; }
    }
    /// <summary>
    /// 新增使用类型
    /// </summary>
    public class QualityCommitteeAdd: QualityCommittee
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public int QualityCommitteeRegisterID { get; set; }

        /// <summary>
        /// 审核不通过原因
        /// </summary>
        public string CheckRemark { get; set; }
    }
}
