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
using System.Linq.Expressions;

namespace Yaeher.CompaniesReport
{
    /// <summary>
    /// 公司收入汇总
    /// </summary>
    public class CorporateIncomeTotalService : ICorporateIncomeTotalService
    {
        private readonly IRepository<CorporateIncomeTotal> _repository;
        /// <summary>
        /// 公司收入汇总构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CorporateIncomeTotalService(IRepository<CorporateIncomeTotal> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询公司收入汇总 List
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<CorporateIncomeTotal>> CorporateIncomeTotalList(CorporateIncomeTotalIn CorporateIncomeTotalInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(CorporateIncomeTotalInfo.Expression).OrderByDescending(a => a.CreatedOn);
            return await query.ToListAsync();
        }
       
        
        /// <summary>
        /// 查询公司收入汇总byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeTotal> CorporateIncomeTotalByID(int Id)
        {
            var CorporateIncomeTotals = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return CorporateIncomeTotals;
        }
        /// <summary>
        /// 查询公司收入汇总 page
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<CorporateIncomeTotal>> CorporateIncomeTotalPage(CorporateIncomeTotalIn CorporateIncomeTotalInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(CorporateIncomeTotalInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / CorporateIncomeTotalInfo.MaxResultCount;
            var CorporateIncomeTotalList = await query.PageBy(CorporateIncomeTotalInfo.SkipTotal, CorporateIncomeTotalInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<CorporateIncomeTotal>(tasksCount, CorporateIncomeTotalList.MapTo<List<CorporateIncomeTotal>>());
        }
        /// <summary>
        /// 新建公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeTotal> CreateCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            CorporateIncomeTotalInfo.Id= await _repository.InsertAndGetIdAsync(CorporateIncomeTotalInfo);
            return CorporateIncomeTotalInfo;
        }
        /// <summary>
        /// 新建公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task TotalCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            await _repository.InsertAsync(CorporateIncomeTotalInfo);
        }
        
        /// <summary>
        /// 修改公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeTotal> UpdateCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            return await _repository.UpdateAsync(CorporateIncomeTotalInfo);
        }

        /// <summary>
        /// 删除公司收入汇总
        /// </summary>
        /// <param name="CorporateIncomeTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CorporateIncomeTotal> DeleteCorporateIncomeTotal(CorporateIncomeTotal CorporateIncomeTotalInfo)
        {
            return await _repository.UpdateAsync(CorporateIncomeTotalInfo);
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public Task<CorporateIncomeTotal> CorporateIncomeTotalExpress(Expression<Func<CorporateIncomeTotal, bool>> whereExpression)
        {
            //初步过滤
            var query = _repository.GetAll().FirstOrDefaultAsync(whereExpression);
            return query;
        }


    }
}
