using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.MessageRemind.Dto;
using Yaeher.SystemConfig;

namespace Yaeher.MessageRemind
{
    /// <summary>
    /// 
    /// </summary>
    public interface IYaeherPhoneService:IApplicationService
    {
        Task<YaeherPhone> CreateYaeherPhone(YaeherPhone YaeherPhoneInfo);
        Task<YaeherPhone> YaeherPhoneByID(int Id);
        Task<IList<YaeherPhone>> YaeherPhoneList(YaeherPhoneIn YaeherPhoneInfo);
        Task<PagedResultDto<YaeherPhone>> YaeherPhonePage(YaeherPhoneIn YaeherPhoneInfo);
    }
}