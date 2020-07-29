using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.EventEntitys;

namespace Yaeher.EventBus
{
    public interface IReceiveEventService : IApplicationService
    {
        Task<ReceiveEvent> CreateReceiveEvent(ReceiveEvent ReceiveEventInfo);
        Task<ReceiveEvent> DeleteReceiveEvent(ReceiveEvent ReceiveEventInfo);
        Task<IList<ReceiveEvent>> ReceiveEventList();
        Task<ReceiveEvent> UpdateReceiveEvent(ReceiveEvent ReceiveEventInfo);
    }
}