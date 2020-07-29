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
    /// 用户角色管理
    /// </summary>
    public class YaeherUserRoleService : IYaeherUserRoleService
    {
        private readonly IRepository<YaeherUserRole> _repository;
        /// <summary>
        /// 用户角色管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherUserRoleService(IRepository<YaeherUserRole> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询用户角色管理 List
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherUserRole>> YaeherUserRoleList(YaeherUserRoleIn YaeherUserRoleInfo)
        {
            var YaeherUserRoles = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(a => a.IsDelete == false&& a.UserID== YaeherUserRoleInfo.UserID);
            return  await YaeherUserRoles.ToListAsync();
        }
        /// <summary>
        /// 查询用户角色管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserRole> YaeherUserRoleByID(int Id)
        {
            var YaeherUserRoles = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherUserRoles;
        }
        /// <summary>
        /// 查询用户角色管理 page
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherUserRole>> YaeherUserRolePage(YaeherUserRoleIn YaeherUserRoleInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherUserRoleInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherUserRoleInfo.MaxResultCount;
            var YaeherUserRoleList = await query.PageBy(YaeherUserRoleInfo.SkipTotal, YaeherUserRoleInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherUserRole>(tasksCount, YaeherUserRoleList.MapTo<List<YaeherUserRole>>());
        }
        /// <summary>
        /// 新建用户角色管理
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserRole> CreateYaeherUserRole(YaeherUserRole YaeherUserRoleInfo)
        {
            YaeherUserRoleInfo.Id= await _repository.InsertAndGetIdAsync(YaeherUserRoleInfo);
            return YaeherUserRoleInfo;
        }

        /// <summary>
        /// 修改用户角色管理
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserRole> UpdateYaeherUserRole(YaeherUserRole YaeherUserRoleInfo)
        {
            return await _repository.UpdateAsync(YaeherUserRoleInfo);
        }

        /// <summary>
        /// 删除用户角色管理
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherUserRole> DeleteYaeherUserRole(YaeherUserRole YaeherUserRoleInfo)
        {
            return await _repository.UpdateAsync(YaeherUserRoleInfo);
        }
    }
}
