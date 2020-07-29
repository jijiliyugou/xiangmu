using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface ICompanyConfigService : IApplicationService
    {
        Task<CompanyConfig> CompanyConfigByID(int Id);
        Task<IList<CompanyConfig>> CompanyConfigList(CompanyConfigIn CompanyConfigInfo);
        Task<PagedResultDto<CompanyConfig>> CompanyConfigPage(CompanyConfigIn CompanyConfigInfo);
        Task<CompanyConfig> CreateCompanyConfig(CompanyConfig CompanyConfigInfo);
        Task<CompanyConfig> DeleteCompanyConfig(CompanyConfig CompanyConfigInfo);
        Task<CompanyConfig> UpdateCompanyConfig(CompanyConfig CompanyConfigInfo);
    }
}