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
    /// 
    /// </summary>
    public class PublishsService : IPublishsService
    {
        private readonly IRepository<Publishs> _repository;
        /// <summary>
        /// 事件服务处理结果
        /// </summary>
        /// <param name="repository"></param>
        public PublishsService(IRepository<Publishs> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增 事件服务处理结果
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Publishs> CreatePublishs(Publishs PublishsInfo)
        {

            PublishsInfo.Id = await _repository.InsertAndGetIdAsync(PublishsInfo);
            return PublishsInfo;
        }
        [RemoteService(false)]
        public async Task<Publishs> PublishsById(int Id)
        {
            return await _repository.FirstOrDefaultAsync(t=>t.Id==Id);
        }
        

        /// <summary>
        /// 修改 事件服务处理结果
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<Publishs> UpdatePublishs(Publishs PublishsInfo)
        {
            return await _repository.UpdateAsync(PublishsInfo);
        }

        /// <summary>
        /// 删除 事件服务处理结果
        /// </summary>
        /// <param name="PublishsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]  
        public async Task<Publishs> DeletePublishs(Publishs PublishsInfo)
        {
            return await _repository.UpdateAsync(PublishsInfo);
        }

        /// <summary>
        /// 查询发发布消息 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<Publishs>> PublishsList()
        {
            var PublishsLists = await _repository.GetAllListAsync();
            return PublishsLists.Where(a => a.IsDelete == false).ToList();
        }
    }
}
