using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.EventEntitys;

namespace Yaeher.EventBus
{
    public interface IPublishsService: IApplicationService
    {
        Task<Publishs> CreatePublishs(Publishs PublishsInfo);
        Task<Publishs> PublishsById(int id);
        Task<Publishs> DeletePublishs(Publishs PublishsInfo);
        Task<IList<Publishs>> PublishsList();
        Task<Publishs> UpdatePublishs(Publishs PublishsInfo);
    }
}