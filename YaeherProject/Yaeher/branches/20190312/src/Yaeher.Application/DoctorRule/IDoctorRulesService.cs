using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.DoctorRule.Dto;

namespace Yaeher.DoctorRule
{
    /// <summary>
    /// 医生规则 制度 指南
    /// </summary>
    public interface IDoctorRulesService : IApplicationService
    {
        /// <summary>
        /// 新建医生规则
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        Task<DoctorRules> CreateDoctorRules(DoctorRules DoctorRulesInfo);
        /// <summary>
        /// 删除医生规则
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        Task<DoctorRules> DeleteDoctorRules(DoctorRules DoctorRulesInfo);
        /// <summary>
        /// 查询医生规则byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorRules> DoctorRulesByID(int Id);
        /// <summary>
        /// 查询医生规则 List
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        Task<List<DoctorRules>> DoctorRulesList(DoctorRulesIn DoctorRulesInfo);
        /// <summary>
        /// 查询所有医生规则 page
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorRules>> DoctorRulesPage(DoctorRulesIn DoctorRulesInfo);
        /// <summary>
        /// 修改医生规则
        /// </summary>
        /// <param name="DoctorRulesInfo"></param>
        /// <returns></returns>
        Task<DoctorRules> UpdateDoctorRules(DoctorRules DoctorRulesInfo);
    }
}