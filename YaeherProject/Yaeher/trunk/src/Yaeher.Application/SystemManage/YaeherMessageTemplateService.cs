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
    /// 微信消息模板
    /// </summary>
    public  class YaeherMessageTemplateService : IYaeherMessageTemplateService
    {
        private readonly IRepository<YaeherMessageTemplate> _repository;
        /// <summary>
        /// 微信消息模板 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherMessageTemplateService(IRepository<YaeherMessageTemplate> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 微信消息模板 List
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherMessageTemplate>> MessageTemplateList(MessageTemplateIn MessageTemplateInfo)
        {
            //初步过滤
            var YaeherBanners = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(MessageTemplateInfo.Expression);
            return await YaeherBanners.ToListAsync();
        }
        /// <summary>
        /// 微信消息模板 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageTemplate> MessageTemplateByID(int Id)
        {
            var YaeherOperLists = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherOperLists;
        }
        /// <summary>
        /// 微信消息模板 page
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherMessageTemplate>> MessageTemplatePage(MessageTemplateIn MessageTemplateInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(MessageTemplateInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / MessageTemplateInfo.MaxResultCount;
            var YaeherOperListList = await query.PageBy(MessageTemplateInfo.SkipTotal, MessageTemplateInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherMessageTemplate>(tasksCount, YaeherOperListList.MapTo<List<YaeherMessageTemplate>>());
        }
        /// <summary>
        /// 新建 微信消息模板
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageTemplate> CreateMessageTemplate(YaeherMessageTemplate MessageTemplateInfo)
        {
            MessageTemplateInfo.Id = await _repository.InsertAndGetIdAsync(MessageTemplateInfo);
            return MessageTemplateInfo;
        }

        /// <summary>
        /// 修改 微信消息模板
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageTemplate> UpdateMessageTemplate(YaeherMessageTemplate MessageTemplateInfo)
        {
            return await _repository.UpdateAsync(MessageTemplateInfo);
        }
        /// <summary>
        /// 删除 微信消息模板
        /// </summary>
        /// <param name="MessageTemplateInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageTemplate> DeleteMessageTemplate(YaeherMessageTemplate MessageTemplateInfo)
        {
            return await _repository.UpdateAsync(MessageTemplateInfo);
        }
    }
}
