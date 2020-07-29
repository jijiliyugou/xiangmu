using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 地区管理
    /// </summary>
    public class AreaManageService : IAreaManageService
    {
        private readonly IRepository<AreaManage> _repository;
        /// <summary>
        /// 地区管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public AreaManageService(IRepository<AreaManage> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询地区管理 List
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<AreaManage>> AreaManageList(AreaManageIn AreaManageInfo)
        {
            var AreaManages = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(AreaManageInfo.Expression);
            return await AreaManages.ToListAsync();
        }

        /// <summary>
        /// 查询地区管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AreaManage> AreaManageByID(int Id)
        {
            var AreaManages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return AreaManages;
        }
        /// <summary>
        /// 查询地区管理 page
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<AreaManage>> AreaManagePage(AreaManageIn AreaManageInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(AreaManageInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / AreaManageInfo.MaxResultCount;
            var AreaManageList = await query.PageBy(AreaManageInfo.SkipTotal, AreaManageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<AreaManage>(tasksCount, AreaManageList.MapTo<List<AreaManage>>());
        }
        /// <summary>
        /// 新建地区管理
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AreaManage> CreateAreaManage(AreaManage AreaManageInfo)
        {
            AreaManageInfo.Id= await _repository.InsertAndGetIdAsync(AreaManageInfo);
            return AreaManageInfo;
        }

        /// <summary>
        /// 修改地区管理
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AreaManage> UpdateAreaManage(AreaManage AreaManageInfo)
        {
            return await _repository.UpdateAsync(AreaManageInfo);
        }

        /// <summary>
        /// 删除地区管理
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AreaManage> DeleteAreaManage(AreaManage AreaManageInfo)
        {
            return await _repository.UpdateAsync(AreaManageInfo);
        }
    }
}
