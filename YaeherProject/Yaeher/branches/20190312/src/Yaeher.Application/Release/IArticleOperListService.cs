using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher.Release
{
    /// <summary>
    /// 操作文章 问答日志
    /// </summary>
    public interface IArticleOperListService : IApplicationService
    {
        /// <summary>
        /// 查询操作文章 问答日志byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ArticleOperList> ArticleOperListByID(int Id);
        /// <summary>
        /// 查询操作文章 问答日志 List
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        Task<IList<ArticleOperList>> ArticleOperListList(ArticleOperListIn ArticleOperListInfo);
        /// <summary>
        /// 查询操作文章 问答日志 page
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ArticleOperList>> ArticleOperListPage(ArticleOperListIn ArticleOperListInfo);
        /// <summary>
        /// 新建操作文章 问答日志
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        Task<ArticleOperList> CreateArticleOperList(ArticleOperList ArticleOperListInfo);
        /// <summary>
        /// 文章byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<List<ArticleOperList>> ArticleOperListByExpression(Expression<Func<ArticleOperList, bool>> whereExpression);
        /// <summary>
        /// 删除操作文章 问答日志
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        Task<ArticleOperList> DeleteArticleOperList(ArticleOperList ArticleOperListInfo);
        /// <summary>
        /// 修改操作文章 问答日志
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        Task<ArticleOperList> UpdateArticleOperList(ArticleOperList ArticleOperListInfo);
    }
}