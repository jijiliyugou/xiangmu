using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherAuth.Dto;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 角色与菜单管理
    /// </summary>
    public interface IYaeherRoleModuleService : IApplicationService
    {
        /// <summary>
        /// 新建角色与菜单管理
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        Task<YaeherRoleModule> CreateYaeherRoleModule(YaeherRoleModule YaeherRoleModuleInfo);
        /// <summary>
        /// 删除角色与菜单管理
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        Task<YaeherRoleModule> DeleteYaeherRoleModule(YaeherRoleModule YaeherRoleModuleInfo);
        /// <summary>
        /// 修改角色与菜单管理
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        Task<YaeherRoleModule> UpdateYaeherRoleModule(YaeherRoleModule YaeherRoleModuleInfo);
        /// <summary>
        /// 查询角色与菜单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherRoleModule> YaeherRoleModuleByID(int Id);
        /// <summary>
        /// 查询角色与菜单管理 List
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherRoleModule>> YaeherRoleModuleList(YaeherRoleModuleIn YaeherRoleModuleInfo);
        /// <summary>
        /// 查询角色与菜单管理 page
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherRoleModule>> YaeherRoleModulePage(YaeherRoleModuleIn YaeherRoleModuleInfo);
    }
}