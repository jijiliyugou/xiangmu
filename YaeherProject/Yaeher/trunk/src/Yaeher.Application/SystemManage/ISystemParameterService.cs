using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public interface ISystemParameterService : IApplicationService
    {
        /// <summary>
        /// 新建系统参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<SystemParameter> CreateSystemParameter(SystemParameter SystemParameterInfo);
        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<SystemParameter> DeleteSystemParameter(SystemParameter SystemParameterInfo);
        /// <summary>
        /// 查询系统参数byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<SystemParameter> SystemParameterByID(int Id);
        /// <summary>
        /// 查询系统参数 List
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<IList<SystemParameter>> SystemParameterList(SystemParameterIn SystemParameterInfo);
        /// <summary>
        /// 查询系统参数 page
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<SystemParameter>> SystemParameterPage(SystemParameterIn SystemParameterInfo);
        /// <summary>
        /// 修改系统参数
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<SystemParameter> UpdateSystemParameter(SystemParameter SystemParameterInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<List<SystemParameter>> ParameterList(SystemParameterIn SystemParameterInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SystemParameterInfo"></param>
        /// <returns></returns>
        Task<List<SystemParameter>> PatientParameterList(SystemParameterIn SystemParameterInfo);
        
    }
}