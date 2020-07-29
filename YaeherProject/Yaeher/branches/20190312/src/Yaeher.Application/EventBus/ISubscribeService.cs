using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.EventEntitys;

namespace Yaeher.EventBus
{
    public interface ISubscribeService : IApplicationService
    {
        Task<Subscribe> CreateSubscribe(Subscribe SubscribeInfo);
        Task<Subscribe> DeleteSubscribe(Subscribe SubscribeInfo);
        Task<IList<Subscribe>> SubscribeList();
        Task<Subscribe> UpdateSubscribe(Subscribe SubscribeInfo);
    }
}