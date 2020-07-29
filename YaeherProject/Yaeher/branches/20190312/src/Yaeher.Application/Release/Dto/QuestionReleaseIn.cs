using Abp.Application.Services.Dto;


namespace Yaeher.Release
{
    /// <summary>
    /// 问答
    /// </summary>
    public class QuestionReleaseIn : ListParameters<QuestionRelease>, IPagedResultRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string DescriptionTiltle { get; set; }
    }
}
