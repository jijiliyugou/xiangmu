using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.DoctorReport.Dto;

namespace Yaeher.DoctorReport
{
    /// <summary>
    /// 医生收入汇总
    /// </summary>
    public class DoctorIncomeService : IDoctorIncomeService
    {
        private readonly IRepository<DoctorIncome> _repository;
        /// <summary>
        /// 医生收入汇总 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorIncomeService(IRepository<DoctorIncome> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询医生收入汇总 List
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorIncome>> DoctorIncomeList(DoctorIncomeIn DoctorIncomeInfo)
        {
            var DoctorIncomes = await _repository.GetAllListAsync(DoctorIncomeInfo.Expression);
            return DoctorIncomes.ToList();
        }

        /// <summary>
        /// 查询医生收入汇总byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorIncome> DoctorIncomeByID(int Id)
        {
            var DoctorIncomes = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorIncomes;
        }
        /// <summary>
        /// 查询医生收入汇总 page
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorIncome>> DoctorIncomePage(DoctorIncomeIn DoctorIncomeInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorIncomeInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorIncomeInfo.MaxResultCount;
            var DoctorIncomeList = await query.PageBy(DoctorIncomeInfo.SkipTotal, DoctorIncomeInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorIncome>(tasksCount, DoctorIncomeList.MapTo<List<DoctorIncome>>());
        }
        /// <summary>
        /// 新建医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorIncome> CreateDoctorIncome(DoctorIncome DoctorIncomeInfo)
        {
            DoctorIncomeInfo.Id= await _repository.InsertAndGetIdAsync(DoctorIncomeInfo);
            return DoctorIncomeInfo;
        }
        /// <summary>
        /// 新建医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task ToTalDoctorIncome(DoctorIncome DoctorIncomeInfo)
        {
             await _repository.InsertAsync(DoctorIncomeInfo);
        }
        

        /// <summary>
        /// 修改医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorIncome> UpdateDoctorIncome(DoctorIncome DoctorIncomeInfo)
        {
            return await _repository.UpdateAsync(DoctorIncomeInfo);
        }

        /// <summary>
        /// 删除医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorIncome> DeleteDoctorIncome(DoctorIncome DoctorIncomeInfo)
        {
            return await _repository.UpdateAsync(DoctorIncomeInfo);
        }
    }
}
