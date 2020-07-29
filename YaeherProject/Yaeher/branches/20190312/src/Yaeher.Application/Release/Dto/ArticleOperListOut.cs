using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.Release
{
    /// <summary>
    /// 操作文章 问答日志
    /// </summary>
    public class ArticleOperListOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ArticleOperListDtos"></param>
        /// <param name="ArticleOperListInfo"></param>
        public ArticleOperListOut(PagedResultDto<ArticleOperList> ArticleOperListDtos, ArticleOperListIn ArticleOperListInfo)
        {
            Items = ArticleOperListDtos.Items;
            TotalCount = ArticleOperListDtos.TotalCount;
            TotalPage = ArticleOperListDtos.TotalCount / ArticleOperListInfo.MaxResultCount;
            SkipCount = ArticleOperListInfo.SkipCount;
            MaxResultCount = ArticleOperListInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ArticleOperList> Items { get; set; }
    }
    public class ArticleOperListOParam
    {
        /// <summary>
        /// 状态
        /// </summary>
        public List<CodeList> ArticleOperType { get; set; }
        public ArticleOperListOParam(List<CodeList> articleOperType)
        {
            ArticleOperType = articleOperType;
        }

    }
}
