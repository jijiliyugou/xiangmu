using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生排序
    /// </summary>
    public class RecommendedOrderingService : IRecommendedOrderingService
    {
        private readonly IRepository<RecommendedOrdering> _repository;
        /// <summary>
        /// 医生排序 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public RecommendedOrderingService(IRepository<RecommendedOrdering> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 医生排序 List
        /// </summary>
        /// <param name="RecommendedOrderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<RecommendedOrdering>> RecommendedOrderList(RecommendedOrderIn RecommendedOrderInfo)
        {
            var SystemParameters = await _repository.GetAllListAsync(RecommendedOrderInfo.Expression);
            return SystemParameters.ToList();
        }

        /// <summary>
        /// 医生排序  byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RecommendedOrdering> RecommendedOrderByID(int Id)
        {
            var RecommendedOrders = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return RecommendedOrders;
        }
        /// <summary>
        /// 医生排序 page
        /// </summary>
        /// <param name="RecommendedOrderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<RecommendedOrdering>> RecommendedOrderPage(RecommendedOrderIn RecommendedOrderInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(RecommendedOrderInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / RecommendedOrderInfo.MaxResultCount;
            var RecommendedOrderList = await query.PageBy(RecommendedOrderInfo.SkipTotal, RecommendedOrderInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<RecommendedOrdering>(tasksCount, RecommendedOrderList.MapTo<List<RecommendedOrdering>>());
        }
        /// <summary>
        /// 新建 医生排序
        /// </summary>
        /// <param name="RecommendedOrderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RecommendedOrdering> CreateRecommendedOrder(RecommendedOrdering RecommendedOrderInfo)
        {
            RecommendedOrderInfo.Id = await _repository.InsertAndGetIdAsync(RecommendedOrderInfo);
            return RecommendedOrderInfo;
        }

        /// <summary>
        /// 修改 医生排序
        /// </summary>
        /// <param name="RecommendedOrderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RecommendedOrdering> UpdateRecommendedOrder(RecommendedOrdering RecommendedOrderInfo)
        {
            return await _repository.UpdateAsync(RecommendedOrderInfo);
        }

        /// <summary>
        /// 删除 医生排序
        /// </summary>
        /// <param name="RecommendedOrderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RecommendedOrdering> DeleteRecommendedOrder(RecommendedOrdering RecommendedOrderInfo)
        {
            return await _repository.UpdateAsync(RecommendedOrderInfo);
        }
    }
}
