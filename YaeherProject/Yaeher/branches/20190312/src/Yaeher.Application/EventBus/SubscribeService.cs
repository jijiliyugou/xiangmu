using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.EventEntitys;

namespace Yaeher.EventBus
{
    /// <summary>
    /// 订阅者
    /// </summary>
    public class SubscribeService : ISubscribeService
    {
        private readonly IRepository<Subscribe> _repository;
        /// <summary>
        /// 事件服务处理结果
        /// </summary>
        /// <param name="repository"></param>
        public SubscribeService(IRepository<Subscribe> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增 事件服务处理结果
        /// </summary>
        /// <param name="SubscribeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Subscribe> CreateSubscribe(Subscribe SubscribeInfo)
        {
            SubscribeInfo.Id = await _repository.InsertAndGetIdAsync(SubscribeInfo);
            return SubscribeInfo;
        }

        /// <summary>
        /// 修改 事件服务处理结果
        /// </summary>
        /// <param name="SubscribeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Subscribe> UpdateSubscribe(Subscribe SubscribeInfo)
        {
            return await _repository.UpdateAsync(SubscribeInfo);
        }

        /// <summary>
        /// 删除 事件服务处理结果
        /// </summary>
        /// <param name="SubscribeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Subscribe> DeleteSubscribe(Subscribe SubscribeInfo)
        {
            return await _repository.UpdateAsync(SubscribeInfo);
        }

        /// <summary>
        /// 查询订阅 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<Subscribe>> SubscribeList()
        {
            var SubscribeLists = await _repository.GetAllListAsync();
            return SubscribeLists.Where(a => a.IsDelete == false).ToList();
        }
    }
}
