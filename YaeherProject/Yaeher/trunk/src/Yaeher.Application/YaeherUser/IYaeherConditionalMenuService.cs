using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;

namespace Yaeher
{
    public interface IYaeherConditionalMenuService: IApplicationService
    {
        Task<YaeherConditionalMenu> CreateYaeherConditionalMenu(YaeherConditionalMenu YaeherConditionalMenufo);
        Task<YaeherConditionalMenu> DeleteYaeherConditionalMenu(YaeherConditionalMenu YaeherConditionalMenufo);
        Task<YaeherConditionalMenu> UpdateYaeherConditionalMenu(YaeherConditionalMenu YaeherConditionalMenufo);
        Task<YaeherConditionalMenu> YaeherConditionalMenuByID(int Id);
        Task<IList<YaeherConditionalMenu>> YaeherConditionalMenuList(YaeherConditionalMenuIn YaeherConditionalMenuInfo);
        Task<IList<WecharMenu>> YaeherModuleList(YaeherConditionalMenuIn YaeherConditionalMenuInfo);
        Task<PagedResultDto<YaeherConditionalMenu>> YaeherConditionalMenuPage(YaeherConditionalMenuIn YaeherConditionalMenuInfo);
    }
}