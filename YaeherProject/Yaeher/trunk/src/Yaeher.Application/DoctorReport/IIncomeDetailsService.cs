using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.DoctorReport.Dto;

namespace Yaeher.DoctorReport
{
    /// <summary>
    /// 医生收入明细
    /// </summary>
    public interface IIncomeDetailsService : IApplicationService
    {
        /// <summary>
        /// 新建医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<IncomeDetails> CreateIncomeDetails(IncomeDetails IncomeDetailsInfo);
        /// <summary>
        /// 新建医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        Task TotalIncomeDetails(IncomeDetails IncomeDetailsInfo);
        /// <summary>
        /// 删除医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<IncomeDetails> DeleteIncomeDetails(IncomeDetails IncomeDetailsInfo);
        /// <summary>
        /// 查询医生收入明细byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IncomeDetails> IncomeDetailsByID(int Id);
        /// <summary>
        /// 查询医生收入明细 List
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<IList<IncomeDetails>> IncomeDetailsList(IncomeDetailsIn IncomeDetailsInfo);
        /// <summary>
        /// 查询医生收入明细 Page
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<IncomeDetails>> IncomeDetailsPage(IncomeDetailsIn IncomeDetailsInfo);
        /// <summary>
        /// 修改医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        Task<IncomeDetails> UpdateIncomeDetails(IncomeDetails IncomeDetailsInfo);

        /// <summary>
        /// 结算医生收入
        /// </summary>
        /// <param name="counttime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        Task<string> IncomeTotalAll(DateTime counttime,DateTime enddatetime,string Ipaddress);
        /// <summary>
        ///测试服务器 结算医生收入
        /// </summary>
        /// <param name="counttime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        Task<string> IncomeTotalTestServerAll(DateTime counttime,DateTime enddatetime,string Ipaddress);
        
    }
}