using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IRecommendedOrderingService : IApplicationService
    {
        Task<RecommendedOrdering> CreateRecommendedOrder(RecommendedOrdering RecommendedOrderInfo);
        Task<RecommendedOrdering> DeleteRecommendedOrder(RecommendedOrdering RecommendedOrderInfo);
        Task<RecommendedOrdering> RecommendedOrderByID(int Id);
        Task<IList<RecommendedOrdering>> RecommendedOrderList(RecommendedOrderIn RecommendedOrderInfo);
        Task<PagedResultDto<RecommendedOrdering>> RecommendedOrderPage(RecommendedOrderIn RecommendedOrderInfo);
        Task<RecommendedOrdering> UpdateRecommendedOrder(RecommendedOrdering RecommendedOrderInfo);
    }
}