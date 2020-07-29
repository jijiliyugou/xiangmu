using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System.Linq.Expressions;
using System;

namespace Yaeher.Release
{
    /// <summary>
    /// 操作文章 问答日志
    /// </summary>
    public class ArticleOperListService : IArticleOperListService
    {
        private readonly IRepository<ArticleOperList> _repository;
        /// <summary>
        /// 操作文章 问答日志 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public ArticleOperListService(IRepository<ArticleOperList> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询操作文章 问答日志 List
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ArticleOperList>> ArticleOperListList(ArticleOperListIn ArticleOperListInfo)
        {
            //初步过滤
            var ArticleOperLists = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ArticleOperListInfo.Expression);
            return await ArticleOperLists.ToListAsync();
        }

        /// <summary>
        /// 查询操作文章 问答日志byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ArticleOperList> ArticleOperListByID(int Id)
        {
            var ArticleOperLists = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ArticleOperLists;
        }
        /// <summary>
        /// 查询操作文章 问答日志 page
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ArticleOperList>> ArticleOperListPage(ArticleOperListIn ArticleOperListInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ArticleOperListInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ArticleOperListInfo.MaxResultCount;
            var ArticleOperListList = await query.PageBy(ArticleOperListInfo.SkipTotal, ArticleOperListInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ArticleOperList>(tasksCount, ArticleOperListList.MapTo<List<ArticleOperList>>());
        }
        /// <summary>
        /// 新建操作文章 问答日志
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ArticleOperList> CreateArticleOperList(ArticleOperList ArticleOperListInfo)
        {
            ArticleOperListInfo.Id= await _repository.InsertAndGetIdAsync(ArticleOperListInfo);
            return ArticleOperListInfo;
        }

        /// <summary>
        /// 文章 问答byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<ArticleOperList>> ArticleOperListByExpression(Expression<Func<ArticleOperList, bool>> whereExpression)
        {
            var ArticleOperList = await _repository.GetAllListAsync(whereExpression);
            return ArticleOperList;
        }
        /// <summary>
        /// 修改操作文章 问答日志
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ArticleOperList> UpdateArticleOperList(ArticleOperList ArticleOperListInfo)
        {
            return await _repository.UpdateAsync(ArticleOperListInfo);
        }

        /// <summary>
        /// 删除操作文章 问答日志
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ArticleOperList> DeleteArticleOperList(ArticleOperList ArticleOperListInfo)
        {
            return await _repository.UpdateAsync(ArticleOperListInfo);
        }
    }
}
