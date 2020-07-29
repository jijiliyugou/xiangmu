using Abp.Application.Services.Dto;


namespace Yaeher.Release
{
    /// <summary>
    /// 操作文章 问答日志
    /// </summary>
    public class ArticleOperListIn : ListParameters<ArticleOperList>, IPagedResultRequest
    {
        /// <summary>
        /// 操作文章ID
        /// </summary>
        public int ArticleID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string OperType { get; set; }
    }
}
