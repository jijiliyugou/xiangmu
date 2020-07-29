using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherAuth.Dto;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public interface IYaeherModuleService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        Task<IList<YaeherModule>> YaeherModule(YaeherModuleIn YaeherModulefo);
        /// <summary>
        /// 新建菜单管理
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        Task<YaeherModule> CreateYaeherModule(YaeherModule YaeherModulefo);
        /// <summary>
        /// 删除菜单管理
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        Task<YaeherModule> DeleteYaeherModule(YaeherModule YaeherModulefo);
        /// <summary>
        /// 修改菜单管理
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        Task<YaeherModule> UpdateYaeherModule(YaeherModule YaeherModulefo);
        /// <summary>
        /// 查询菜单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherModule> YaeherModuleByID(int Id);
        /// <summary>
        /// 查询菜单管理 List
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        Task<IList<YaeherModuleNode>> YaeherModuleList(YaeherModuleIn YaeherModulefo);
        /// <summary>
        /// 查询菜单管理 page
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherModule>> YaeherModulePage(YaeherModuleIn YaeherModulefo);
    }
}