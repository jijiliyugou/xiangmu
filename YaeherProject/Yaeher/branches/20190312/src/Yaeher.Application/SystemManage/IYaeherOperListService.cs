using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public interface IYaeherOperListService : IApplicationService
    {
        /// <summary>
        /// 新建系统操作日志
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        Task<YaeherOperList> CreateYaeherOperList(YaeherOperList YaeherOperListInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        Task<YaeherOperList> PatientYaeherOperList(YaeherOperList YaeherOperListInfo);
        /// <summary>
        /// 删除系统操作日志
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        Task<YaeherOperList> DeleteYaeherOperList(YaeherOperList YaeherOperListInfo);
        /// <summary>
        /// 修改系统操作日志
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        Task<YaeherOperList> UpdateYaeherOperList(YaeherOperList YaeherOperListInfo);
        /// <summary>
        /// 查询系统操作日志byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherOperList> YaeherOperListByID(int Id);
        /// <summary>
        /// 查询系统操作日志 List
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherOperList>> YaeherOperListList(YaeherOperListIn YaeherOperListInfo);
        /// <summary>
        /// 查询系统操作日志 page
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherOperList>> YaeherOperListPage(YaeherOperListIn YaeherOperListInfo);
    }
}