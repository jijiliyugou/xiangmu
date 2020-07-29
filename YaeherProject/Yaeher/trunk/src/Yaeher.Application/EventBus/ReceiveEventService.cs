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
    /// 处理记录
    /// </summary>
    public class ReceiveEventService : IReceiveEventService
    {
        private readonly IRepository<ReceiveEvent> _repository;
        /// <summary>
        /// 事件服务处理结果
        /// </summary>
        /// <param name="repository"></param>
        public ReceiveEventService(IRepository<ReceiveEvent> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增 事件服务处理结果
        /// </summary>
        /// <param name="ReceiveEventInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReceiveEvent> CreateReceiveEvent(ReceiveEvent ReceiveEventInfo)
        {
            ReceiveEventInfo.Id = await _repository.InsertAndGetIdAsync(ReceiveEventInfo);
            return ReceiveEventInfo;
        }

        /// <summary>
        /// 修改 事件服务处理结果
        /// </summary>
        /// <param name="ReceiveEventInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReceiveEvent> UpdateReceiveEvent(ReceiveEvent ReceiveEventInfo)
        {
            return await _repository.UpdateAsync(ReceiveEventInfo);
        }

        /// <summary>
        /// 删除 事件服务处理结果
        /// </summary>
        /// <param name="ReceiveEventInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ReceiveEvent> DeleteReceiveEvent(ReceiveEvent ReceiveEventInfo)
        {
            return await _repository.UpdateAsync(ReceiveEventInfo);
        }

        /// <summary>
        /// 事件服务处理结果 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ReceiveEvent>> ReceiveEventList()
        {
            var ReceiveEventLists = await _repository.GetAllListAsync();
            return ReceiveEventLists.Where(a => a.IsDelete = false).ToList();
        }
    }
}
