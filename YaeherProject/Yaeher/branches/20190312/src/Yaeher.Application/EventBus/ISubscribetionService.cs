using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.EventEntitys;

namespace Yaeher.EventBus
{
    public interface ISubscribetionService : IApplicationService
    {
        Task<Subscribetion> CreateSubscribetion(Subscribetion SubscribetionInfo);
        Task<Subscribetion> DeleteSubscribetion(Subscribetion SubscribetionInfo);
        Task<IList<Subscribetion>> SubscribetionList();
        Task<Subscribetion> UpdateSubscribetion(Subscribetion SubscribetionInfo);
    }
}