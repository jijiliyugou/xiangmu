using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System;

namespace Yaeher.Release
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ReleaseManageService : IReleaseManageService
    {
        private readonly IRepository<ReleaseManage> _repository;
        /// <summary>
        /// 文章 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public ReleaseManageService(IRepository<ReleaseManage> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询文章 List
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ReleaseManage>> ReleaseManageList(ReleaseManageIn ReleaseManageInfo)
        {
            //初步过滤
            var ReleaseManages = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ReleaseManageInfo.Expression);
            return await ReleaseManages.ToListAsync();
        }

        /// <summary>
        /// 查询文章byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReleaseManage> ReleaseManageByID(int Id)
        {
            var ReleaseManages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ReleaseManages;
        }
        /// <summary>
        /// 查询文章 page
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ReleaseManage>> ReleaseManagePage(ReleaseManageIn ReleaseManageInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ReleaseManageInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ReleaseManageInfo.MaxResultCount;
            var ReleaseManageList = await query.PageBy(ReleaseManageInfo.SkipTotal, ReleaseManageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ReleaseManage>(tasksCount, ReleaseManageList.MapTo<List<ReleaseManage>>());
        }
        /// <summary>
        /// 新建文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReleaseManage> CreateReleaseManage(ReleaseManage ReleaseManageInfo)
        {
            ReleaseManageInfo.Id= await _repository.InsertAndGetIdAsync(ReleaseManageInfo);
            return ReleaseManageInfo;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReleaseManage> UpdateReleaseManage(ReleaseManage ReleaseManageInfo)
        {
            return await _repository.UpdateAsync(ReleaseManageInfo);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReleaseManage> DeleteReleaseManage(ReleaseManage ReleaseManageInfo)
        {
            return await _repository.UpdateAsync(ReleaseManageInfo);
        }
    }
}
