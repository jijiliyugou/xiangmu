using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface ISendMessageService: IApplicationService
    {
        Task<SendMessageTemplate> CreateSendMessage(SendMessageTemplate SendMessageInfo);
        Task<SendMessageTemplate> DeleteSendMessage(SendMessageTemplate SendMessageInfo);
        Task<SendMessageTemplate> SendMessageByID(int Id);
        Task<IList<SendMessageTemplate>> SendMessageList(SendMessageIn SendMessageInfo);
        Task<PagedResultDto<SendMessageTemplate>> SendMessagePage(SendMessageIn SendMessageInfo);
        Task<SendMessageTemplate> UpdateSendMessage(SendMessageTemplate SendMessageInfo);
    }
}