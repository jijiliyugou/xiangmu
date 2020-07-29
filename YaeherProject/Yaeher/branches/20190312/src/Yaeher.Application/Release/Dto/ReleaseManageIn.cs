using Abp.Application.Services.Dto;


namespace Yaeher.Release
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ReleaseManageIn : ListParameters<ReleaseManage>, IPagedResultRequest
    {
        /// <summary>
        /// 发布文章标题
        /// </summary>
        public string PaperTiltle { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string CheckState { get; set;}

    }
}
