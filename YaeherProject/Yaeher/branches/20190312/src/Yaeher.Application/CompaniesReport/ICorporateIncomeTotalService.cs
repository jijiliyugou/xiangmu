using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.CompaniesReport.Dto;

namespace Yaeher.CompaniesReport
{
    /// <summary>
    /// 公司收入汇总
    /// </summary>
    public interface ICorporateIncomeTotalService : IApplicationService
    {
        /// <summary>
        /// 查询公司收入汇总byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<CorporateIncomeTotal> CorporateIncomeTotalByID(int Id);
        /// <summary>
        /// 查询公司收入汇总 List
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        Task<IList<CorporateIncomeTotal>> CorporateIncomeTotalList(CorporateIncomeTotalIn CorporateIncomeTotalInfo);
        /// <summary>
        /// 查询公司收入汇总 List
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<CorporateIncomeTotal> CorporateIncomeTotalExpress(Expression<Func<CorporateIncomeTotal, bool>> whereExpression);
        /// <summary>
        /// 查询公司收入汇总 page
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<CorporateIncomeTotal>> CorporateIncomeTotalPage(CorporateIncomeTotalIn CorporateIncomeTotalInfo);
        /// <summary>
        /// 新建公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        Task<CorporateIncomeTotal> CreateCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo);
        /// <summary>
        /// 新建公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        Task TotalCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo);
        /// <summary>
        /// 删除公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        Task<CorporateIncomeTotal> DeleteCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo);
        /// <summary>
        /// 修改公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        Task<CorporateIncomeTotal> UpdateCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo);
    }
}