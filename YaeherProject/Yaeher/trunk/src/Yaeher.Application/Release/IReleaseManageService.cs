using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher.Release
{
    /// <summary>
    /// 文章
    /// </summary>
    public interface IReleaseManageService : IApplicationService
    {
        /// <summary>
        /// 新建文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        Task<ReleaseManage> CreateReleaseManage(ReleaseManage ReleaseManageInfo);
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        Task<ReleaseManage> DeleteReleaseManage(ReleaseManage ReleaseManageInfo);
        /// <summary>
        /// 查询文章byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ReleaseManage> ReleaseManageByID(int Id);
        /// <summary>
        /// 查询文章 List
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        Task<IList<ReleaseManage>> ReleaseManageList(ReleaseManageIn ReleaseManageInfo);
        /// <summary>
        /// 查询文章 page
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ReleaseManage>> ReleaseManagePage(ReleaseManageIn ReleaseManageInfo);
        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        Task<ReleaseManage> UpdateReleaseManage(ReleaseManage ReleaseManageInfo);
    }
}