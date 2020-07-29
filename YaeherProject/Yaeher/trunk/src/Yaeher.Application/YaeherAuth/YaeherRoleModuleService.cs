using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.YaeherAuth.Dto;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 角色与菜单管理
    /// </summary>
    public class YaeherRoleModuleService : IYaeherRoleModuleService
    {
        private readonly IRepository<YaeherRoleModule> _repository;
        /// <summary>
        /// 角色与菜单管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherRoleModuleService(IRepository<YaeherRoleModule> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询角色与菜单管理 List
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherRoleModule>> YaeherRoleModuleList(YaeherRoleModuleIn YaeherRoleModuleInfo)
        {
            var YaeherRoleModules = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(a => a.IsDelete == false && a.RoleId == YaeherRoleModuleInfo.RoleId) ;
            return await YaeherRoleModules.ToListAsync();
        }

        /// <summary>
        /// 查询角色与菜单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRoleModule> YaeherRoleModuleByID(int Id)
        {
            var YaeherRoleModules = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherRoleModules;
        }
        /// <summary>
        /// 查询角色与菜单管理 page
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherRoleModule>> YaeherRoleModulePage(YaeherRoleModuleIn YaeherRoleModuleInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherRoleModuleInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherRoleModuleInfo.MaxResultCount;
            var YaeherRoleModuleList = await query.PageBy(YaeherRoleModuleInfo.SkipTotal, YaeherRoleModuleInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherRoleModule>(tasksCount, YaeherRoleModuleList.MapTo<List<YaeherRoleModule>>());
        }
        /// <summary>
        /// 新建角色与菜单管理
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRoleModule> CreateYaeherRoleModule(YaeherRoleModule YaeherRoleModuleInfo)
        {
            YaeherRoleModuleInfo.Id= await _repository.InsertAndGetIdAsync(YaeherRoleModuleInfo);
            return YaeherRoleModuleInfo;
        }

        /// <summary>
        /// 修改角色与菜单管理
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRoleModule> UpdateYaeherRoleModule(YaeherRoleModule YaeherRoleModuleInfo)
        {
            return await _repository.UpdateAsync(YaeherRoleModuleInfo);
        }

        /// <summary>
        /// 删除角色与菜单管理
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRoleModule> DeleteYaeherRoleModule(YaeherRoleModule YaeherRoleModuleInfo)
        {
            return await _repository.UpdateAsync(YaeherRoleModuleInfo);
        }
    }
}
