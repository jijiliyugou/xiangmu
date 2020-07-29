using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 接口管理
    /// </summary>
    public interface IInterfaceSetService : IApplicationService
    {
        /// <summary>
        /// 新建接口管理
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        Task<InterfaceSet> CreateInterfaceSet(InterfaceSet InterfaceSetInfo);
        /// <summary>
        /// 删除接口管理
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        Task<InterfaceSet> DeleteInterfaceSet(InterfaceSet InterfaceSetInfo);
        /// <summary>
        /// 查询接口管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<InterfaceSet> InterfaceSetByID(int Id);
        /// <summary>
        /// 查询接口管理 List
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        Task<IList<InterfaceSet>> InterfaceSetList(InterfaceSetIn InterfaceSetInfo);
        /// <summary>
        /// 查询接口管理 page
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<InterfaceSet>> InterfaceSetPage(InterfaceSetIn InterfaceSetInfo);
        /// <summary>
        /// 修改接口管理
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        Task<InterfaceSet> UpdateInterfaceSet(InterfaceSet InterfaceSetInfo);
    }
}