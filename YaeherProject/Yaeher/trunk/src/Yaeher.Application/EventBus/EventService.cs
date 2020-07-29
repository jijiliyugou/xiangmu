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
    /// 事件服务执行处理 
    /// </summary>
    public class EventService
    {
        private readonly IRepository<Publishs> _PublishsRepository;
        private readonly IRepository<Subscribe> _SubscribeRepository;
        private readonly IRepository<Subscribetion> _SubscribetionRepository;
        /// <summary>
        /// 事件服务处理结果
        /// </summary>
        /// <param name="PublishsRepository"></param>
        /// <param name="SubscribeRepository"></param>
        /// <param name="SubscribetionRepository"></param>
        public EventService(IRepository<Publishs> PublishsRepository,
                            IRepository<Subscribe> SubscribeRepository,
                            IRepository<Subscribetion> SubscribetionRepository)
        {
            _PublishsRepository = PublishsRepository;
            _SubscribeRepository = SubscribeRepository;
            _SubscribetionRepository = SubscribetionRepository;
        }

        
    }
}
