using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.CompaniesReport.Dto;
using System;

namespace Yaeher.CompaniesReport
{
    /// <summary>
    /// 公司收入明细
    /// </summary>
    public class CorporateIncomeDetailsService : ICorporateIncomeDetailsService
    {
        private readonly IRepository<CorporateIncomeDetails> _repository;
        /// <summary>
        /// 公司收入明细 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CorporateIncomeDetailsService(IRepository<CorporateIncomeDetails> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 公司收入明细 List
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<CorporateIncomeDetails>> CorporateIncomeDetailsList(CorporateIncomeDetailsIn CorporateIncomeDetailsInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(CorporateIncomeDetailsInfo.Expression);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 公司收入明细byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeDetails> CorporateIncomeDetailsByID(int Id)
        {
            var CorporateIncomeDetailss = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return CorporateIncomeDetailss;
        }
        /// <summary>
        /// 公司收入明细 page
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<CorporateIncomeDetails>> CorporateIncomeDetailsPage(CorporateIncomeDetailsIn CorporateIncomeDetailsInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(CorporateIncomeDetailsInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / CorporateIncomeDetailsInfo.MaxResultCount;
            var CorporateIncomeDetailsList = await query.PageBy(CorporateIncomeDetailsInfo.SkipTotal, CorporateIncomeDetailsInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<CorporateIncomeDetails>(tasksCount, CorporateIncomeDetailsList.MapTo<List<CorporateIncomeDetails>>());
        }
        /// <summary>
        /// 新建公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeDetails> CreateCorporateIncomeDetails(CorporateIncomeDetails CorporateIncomeDetailsInfo)
        {
            CorporateIncomeDetailsInfo.Id= await _repository.InsertAndGetIdAsync(CorporateIncomeDetailsInfo);
            return CorporateIncomeDetailsInfo;
        }

        /// <summary>
        /// 修改公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeDetails> UpdateCorporateIncomeDetails(CorporateIncomeDetails CorporateIncomeDetailsInfo)
        {
            return await _repository.UpdateAsync(CorporateIncomeDetailsInfo);
        }

        /// <summary>
        /// 删除公司收入明细
        /// </summary>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeDetails> DeleteCorporateIncomeDetails(CorporateIncomeDetails CorporateIncomeDetailsInfo)
        {
            return await _repository.UpdateAsync(CorporateIncomeDetailsInfo);
        }
    }
}
