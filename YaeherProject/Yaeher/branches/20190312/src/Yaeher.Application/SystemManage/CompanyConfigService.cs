using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 公司参数
    /// </summary>
    public class CompanyConfigService : ICompanyConfigService
    {
        private readonly IRepository<CompanyConfig> _repository;
        /// <summary>
        /// 公司参数 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CompanyConfigService(IRepository<CompanyConfig> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 公司参数 List
        /// </summary>
        /// <param name="CompanyConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<CompanyConfig>> CompanyConfigList(CompanyConfigIn CompanyConfigInfo)
        {
            var CompanyConfigs = await _repository.GetAllListAsync(CompanyConfigInfo.Expression);
            return CompanyConfigs.ToList();
        }

        /// <summary>
        /// 公司参数 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CompanyConfig> CompanyConfigByID(int Id)
        {
            var CompanyConfigs = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return CompanyConfigs;
        }
        /// <summary>
        /// 公司参数 page
        /// </summary>
        /// <param name="CompanyConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<CompanyConfig>> CompanyConfigPage(CompanyConfigIn CompanyConfigInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(CompanyConfigInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / CompanyConfigInfo.MaxResultCount;
            var CompanyConfigList = await query.PageBy(CompanyConfigInfo.SkipTotal, CompanyConfigInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<CompanyConfig>(tasksCount, CompanyConfigList.MapTo<List<CompanyConfig>>());
        }
        /// <summary>
        /// 新建公司参数
        /// </summary>
        /// <param name="CompanyConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CompanyConfig> CreateCompanyConfig(CompanyConfig CompanyConfigInfo)
        {
            CompanyConfigInfo.Id = await _repository.InsertAndGetIdAsync(CompanyConfigInfo);
            return CompanyConfigInfo;
        }

        /// <summary>
        /// 修改公司参数
        /// </summary>
        /// <param name="CompanyConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CompanyConfig> UpdateCompanyConfig(CompanyConfig CompanyConfigInfo)
        {
            return await _repository.UpdateAsync(CompanyConfigInfo);
        }

        /// <summary>
        /// 删除公司参数
        /// </summary>
        /// <param name="CompanyConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CompanyConfig> DeleteCompanyConfig(CompanyConfig CompanyConfigInfo)
        {
            return await _repository.UpdateAsync(CompanyConfigInfo);
        }
    }
}
