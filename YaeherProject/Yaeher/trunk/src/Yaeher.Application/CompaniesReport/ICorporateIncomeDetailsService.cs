using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.CompaniesReport.Dto;

namespace Yaeher.CompaniesReport
{
    /// <summary>
    /// 公司收入明细
    /// </summary>
    public interface ICorporateIncomeDetailsService : IApplicationService
    {
        /// <summary>
        /// 公司收入明细byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<CorporateIncomeDetails> CorporateIncomeDetailsByID(int Id);
        /// <summary>
        /// 公司收入明细 List
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<IList<CorporateIncomeDetails>> CorporateIncomeDetailsList(CorporateIncomeDetailsIn CorporateIncomeDetailsInfo);
        /// <summary>
        /// 公司收入明细 page
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<CorporateIncomeDetails>> CorporateIncomeDetailsPage(CorporateIncomeDetailsIn CorporateIncomeDetailsInfo);
        /// <summary>
        /// 新建公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<CorporateIncomeDetails> CreateCorporateIncomeDetails(CorporateIncomeDetails CorporateIncomeDetailsInfo);
        /// <summary>
        /// 删除公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<CorporateIncomeDetails> DeleteCorporateIncomeDetails(CorporateIncomeDetails CorporateIncomeDetailsInfo);
        /// <summary>
        /// 修改公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<CorporateIncomeDetails> UpdateCorporateIncomeDetails(CorporateIncomeDetails CorporateIncomeDetailsInfo);
    }
}