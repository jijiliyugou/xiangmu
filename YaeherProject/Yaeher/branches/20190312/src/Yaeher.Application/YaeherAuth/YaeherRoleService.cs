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
using System;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class YaeherRoleService : IYaeherRoleService
    {
        private readonly IRepository<YaeherRole> _repository;
        /// <summary>
        /// 角色管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherRoleService(IRepository<YaeherRole> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询角色管理 List
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherRole>> YaeherRoleList(YaeherRoleIn YaeherRoleInfo)
        {
            var query = _repository.GetAll().Where(YaeherRoleInfo.Expression).OrderByDescending(a => a.CreatedOn);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询角色管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRole> YaeherRoleByID(int Id)
        {
            var YaeherRoles = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherRoles;
        }
        /// <summary>
        /// 查询角色管理 page
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherRole>> YaeherRolePage(YaeherRoleIn YaeherRoleInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(YaeherRoleInfo.Expression).OrderByDescending(a => a.CreatedOn);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherRoleInfo.MaxResultCount;
            var YaeherRoleList = await query.PageBy(YaeherRoleInfo.SkipTotal, YaeherRoleInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherRole>(tasksCount, YaeherRoleList.MapTo<List<YaeherRole>>());
        }
        /// <summary>
        /// 新建角色管理
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRole> CreateYaeherRole(YaeherRole YaeherRoleInfo)
        {
            YaeherRoleInfo.Id= await _repository.InsertAndGetIdAsync(YaeherRoleInfo);
            return YaeherRoleInfo;
        }

        /// <summary>
        /// 修改角色管理
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRole> UpdateYaeherRole(YaeherRole YaeherRoleInfo)
        {
            return await _repository.UpdateAsync(YaeherRoleInfo);
        }

        /// <summary>
        /// 删除角色管理
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherRole> DeleteYaeherRole(YaeherRole YaeherRoleInfo)
        {
            return await _repository.UpdateAsync(YaeherRoleInfo);
        }
    }
}
