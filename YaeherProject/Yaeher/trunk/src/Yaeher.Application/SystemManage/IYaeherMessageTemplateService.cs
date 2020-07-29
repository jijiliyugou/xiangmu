using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IYaeherMessageTemplateService:IApplicationService
    {
        Task<IList<YaeherMessageTemplate>> MessageTemplateList(MessageTemplateIn MessageTemplateInfo);
        Task<YaeherMessageTemplate> CreateMessageTemplate(YaeherMessageTemplate MessageTemplateInfo);
        Task<YaeherMessageTemplate> MessageTemplateByID(int Id);
        Task<PagedResultDto<YaeherMessageTemplate>> MessageTemplatePage(MessageTemplateIn MessageTemplateInfo);
        Task<YaeherMessageTemplate> UpdateMessageTemplate(YaeherMessageTemplate MessageTemplateInfo);
        Task<YaeherMessageTemplate> DeleteMessageTemplate(YaeherMessageTemplate MessageTemplateInfo);
    }
}