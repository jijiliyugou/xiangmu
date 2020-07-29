using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.EventEntitys;
using System.Linq;

namespace Yaeher.EventBus
{
    /// <summary>
    /// 订阅者实体  订阅执行
    /// </summary>
    public class SubscribetionService : ISubscribetionService
    {
        private readonly IRepository<Subscribetion> _repository;
        /// <summary>
        /// 订阅者实体  订阅执行
        /// </summary>
        /// <param name="repository"></param>
        public SubscribetionService(IRepository<Subscribetion> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增 订阅者实体  订阅执行
        /// </summary>
        /// <param name="SubscribetionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Subscribetion> CreateSubscribetion(Subscribetion SubscribetionInfo)
        {
            SubscribetionInfo.Id = await _repository.InsertAndGetIdAsync(SubscribetionInfo);
            return SubscribetionInfo;
        }
        /// <summary>
        /// 修改 订阅者实体  订阅执行
        /// </summary>
        /// <param name="SubscribetionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Subscribetion> UpdateSubscribetion(Subscribetion SubscribetionInfo)
        {
            return await _repository.UpdateAsync(SubscribetionInfo);
        }

        /// <summary>
        /// 删除 订阅者实体  订阅执行
        /// </summary>
        /// <param name="SubscribetionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Subscribetion> DeleteSubscribetion(Subscribetion SubscribetionInfo)
        {
            return await _repository.UpdateAsync(SubscribetionInfo);
        }
        /// <summary>
        /// 执行转发 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<Subscribetion>> SubscribetionList()
        {
            var SubscribetionLists= await _repository.GetAllListAsync();
            return SubscribetionLists.Where(a => a.IsDelete = false).ToList();
        }
    }
}
