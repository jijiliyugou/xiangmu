using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherAuth.Dto;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public interface IYaeherUserRoleService : IApplicationService
    {
        /// <summary>
        /// 新建用户角色管理
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        Task<YaeherUserRole> CreateYaeherUserRole(YaeherUserRole YaeherUserRoleInfo);
        /// <summary>
        /// 删除用户角色管理
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        Task<YaeherUserRole> DeleteYaeherUserRole(YaeherUserRole YaeherUserRoleInfo);
        /// <summary>
        /// 修改用户角色管理
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        Task<YaeherUserRole> UpdateYaeherUserRole(YaeherUserRole YaeherUserRoleInfo);
        /// <summary>
        /// 查询用户角色管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherUserRole> YaeherUserRoleByID(int Id);
        /// <summary>
        /// 查询用户角色管理 List
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherUserRole>> YaeherUserRoleList(YaeherUserRoleIn YaeherUserRoleInfo);
        /// <summary>
        /// 查询用户角色管理 page
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherUserRole>> YaeherUserRolePage(YaeherUserRoleIn YaeherUserRoleInfo);
    }
}