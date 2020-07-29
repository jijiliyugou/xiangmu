using Abp.Application.Services.Dto;


namespace Yaeher.DoctorQuality.Dto
{
    /// <summary>
    /// 处理质控
    /// </summary>
    public class QualityControlManageIn : ListParameters<QualityControlManage>, IPagedResultRequest
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string ReplyState { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public int QualityLevel { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string RepayIllnessDescription { get; set; }
        /// <summary>
        /// 处理医生
        /// </summary>
        public string QualityDoctor { get; set; }
    }
   
}
