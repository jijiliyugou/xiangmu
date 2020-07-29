using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherAuth.Dto;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IYaeherRoleService : IApplicationService
    {
        /// <summary>
        /// 新建角色管理
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        Task<YaeherRole> CreateYaeherRole(YaeherRole YaeherRoleInfo);
        /// <summary>
        /// 删除角色管理
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        Task<YaeherRole> DeleteYaeherRole(YaeherRole YaeherRoleInfo);
        /// <summary>
        /// 修改角色管理
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        Task<YaeherRole> UpdateYaeherRole(YaeherRole YaeherRoleInfo);
        /// <summary>
        /// 查询角色管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherRole> YaeherRoleByID(int Id);
        /// <summary>
        /// 查询角色管理 List
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherRole>> YaeherRoleList(YaeherRoleIn YaeherRoleInfo);
        /// <summary>
        /// 查询角色管理 page
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherRole>> YaeherRolePage(YaeherRoleIn YaeherRoleInfo);
    }
}