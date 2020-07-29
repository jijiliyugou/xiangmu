using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Yaeher
{
    /// <summary>
    ///ILeaguerInfoService 接口
    /// </summary>
    public interface ILeaguerInfoService : IApplicationService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        Task<YaeherPatientLeaguerInfo> CreateLeaguerInfo(YaeherPatientLeaguerInfo Leaguer);
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherPatientLeaguerInfo>> LeaguerInfoPage(LeaguerInfo Leaguer);
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        Task<List<YaeherPatientLeaguerInfo>> LeaguerInfoList(LeaguerInfo Leaguer);
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        Task<List<YaeherPatientLeaguerInfo>> QueryLeaguerInfoList(LeaguerInfoIn Leaguer);
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<YaeherPatientLeaguerInfo> LeaguerInfoById(int ID);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        Task<YaeherPatientLeaguerInfo> UpdateLeaguerInfo(YaeherPatientLeaguerInfo Leaguer);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Leaguer"></param>
        /// <returns></returns>
        Task<YaeherPatientLeaguerInfo> DeleteLeaguerInfo(YaeherPatientLeaguerInfo Leaguer);
    }
}
