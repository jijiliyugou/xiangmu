using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IWecharSendMessageService: IApplicationService
    {
        Task<WecharSendMessage> CreateWecharSendMessage(SendMessageInfo sendMessageInfo);
        Task<WecharSendMessage> WecharSendMessageByID(int Id);
        Task<IList<WecharSendMessage>> WecharSendMessageList(WecharSendMessageIn WecharSendMessageInfo);
        Task<PagedResultDto<WecharSendMessage>> WecharSendMessagePage(WecharSendMessageIn WecharSendMessageInfo);
        Task<WecharSendMessage> WecharSendMessageByNumber(string ConsultNumber,string MsgID);
        Task<WecharSendMessage> UpdateWecharSendMessage(WecharSendMessage WecharSendMessageInfo);
    }
}