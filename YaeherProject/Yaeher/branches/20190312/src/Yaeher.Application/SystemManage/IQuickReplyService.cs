using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IQuickReplyService: IApplicationService

    {
        Task<QuickReply> CreateQuickReply(QuickReply QuickReplyInfo);
        Task<QuickReply> DeleteQuickReply(QuickReply QuickReplyInfo);
        Task<PagedResultDto<QuickReply>> QuickReplyPage(QuickReplyIn QuickReplyInfo);
        Task<QuickReply> QuickReplyByID(int Id);
        Task<List<QuickReply>> QuickReplyList(QuickReplyIn QuickReplyInfo);
        Task<QuickReply> UpdateQuickReply(QuickReply QuickReplyInfo);
    }
}