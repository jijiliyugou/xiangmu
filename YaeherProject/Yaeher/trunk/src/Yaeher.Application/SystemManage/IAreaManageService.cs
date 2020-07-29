using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 地区管理
    public interface IAreaManageService : IApplicationService
    {
        /// <summary>
        /// 查询地区管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<AreaManage> AreaManageByID(int Id);
        /// <summary>
        /// 查询地区管理 List
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        Task<IList<AreaManage>> AreaManageList(AreaManageIn AreaManageInfo);
        /// <summary>
        /// 查询地区管理 page
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<AreaManage>> AreaManagePage(AreaManageIn AreaManageInfo);
        /// <summary>
        /// 新建地区管理
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        Task<AreaManage> CreateAreaManage(AreaManage AreaManageInfo);
        /// <summary>
        /// 删除地区管理
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        Task<AreaManage> DeleteAreaManage(AreaManage AreaManageInfo);
        /// <summary>
        /// 修改地区管理
        /// </summary>
        /// <param name="AreaManageInfo"></param>
        /// <returns></returns>
        Task<AreaManage> UpdateAreaManage(AreaManage AreaManageInfo);
    }
}