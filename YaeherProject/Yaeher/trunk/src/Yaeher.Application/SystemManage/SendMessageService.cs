using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 发送消息模板
    /// </summary>
    public class SendMessageService : ISendMessageService
    {
        private readonly IRepository<SendMessageTemplate> _repository;
        /// <summary>
        /// 发送消息模板 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public SendMessageService(IRepository<SendMessageTemplate> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 发送消息模板 List
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<SendMessageTemplate>> SendMessageList(SendMessageIn SendMessageInfo)
        {
            //初步过滤
            var SendMessagess = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(SendMessageInfo.Expression);
            return await SendMessagess.ToListAsync();
        }

        /// <summary>
        /// 发送消息模板 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SendMessageTemplate> SendMessageByID(int Id)
        {
            var SendMessages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return SendMessages;
        }
        /// <summary>
        /// 发送消息模板 page
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<SendMessageTemplate>> SendMessagePage(SendMessageIn SendMessageInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(SendMessageInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / SendMessageInfo.MaxResultCount;
            var DoctorCheckList = await query.PageBy(SendMessageInfo.SkipTotal, SendMessageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<SendMessageTemplate>(tasksCount, DoctorCheckList.MapTo<List<SendMessageTemplate>>());
        }
        /// <summary>
        /// 新建 发送消息模板
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SendMessageTemplate> CreateSendMessage(SendMessageTemplate SendMessageInfo)
        {
            SendMessageInfo.Id = await _repository.InsertAndGetIdAsync(SendMessageInfo);
            return SendMessageInfo;
        }

        /// <summary>
        /// 修改 发送消息模板
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SendMessageTemplate> UpdateSendMessage(SendMessageTemplate SendMessageInfo)
        {
            return await _repository.UpdateAsync(SendMessageInfo);
        }

        /// <summary>
        /// 删除 发送消息模板
        /// </summary>
        /// <param name="SendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SendMessageTemplate> DeleteSendMessage(SendMessageTemplate SendMessageInfo)
        {
            return await _repository.UpdateAsync(SendMessageInfo);
        }
    }
}
