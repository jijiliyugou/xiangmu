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
    /// 医生快捷回复
    /// </summary>
    public class QuickReplyService : IQuickReplyService
    {
        private readonly IRepository<QuickReply> _repository;

        /// <summary>
        /// 医生快捷回复 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public QuickReplyService(IRepository<QuickReply> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 医生快捷回复 List
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<QuickReply>> QuickReplyList(QuickReplyIn QuickReplyInfo)
        {
            var QuickReplys = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(QuickReplyInfo.Expression);
            return await QuickReplys.ToListAsync();
        }

        /// <summary>
        /// 医生快捷回复byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuickReply>QuickReplyByID(int Id)
        {
            var QuickReplys = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return QuickReplys;
        }
        /// <summary>
        /// 医生快捷回复 page
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QuickReply>> QuickReplyPage(QuickReplyIn QuickReplyInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(QuickReplyInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / QuickReplyInfo.MaxResultCount;
            var QuickReplyList = await query.PageBy(QuickReplyInfo.SkipTotal, QuickReplyInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QuickReply>(tasksCount, QuickReplyList.MapTo<List<QuickReply>>());
        }
        /// <summary>
        /// 新建医生快捷回复
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuickReply> CreateQuickReply(QuickReply QuickReplyInfo)
        {
            QuickReplyInfo.Id = await _repository.InsertAndGetIdAsync(QuickReplyInfo);
            return QuickReplyInfo;
        }

        /// <summary>
        /// 修改医生快捷回复
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuickReply> UpdateQuickReply(QuickReply QuickReplyInfo)
        {
            return await _repository.UpdateAsync(QuickReplyInfo);
        }

        /// <summary>
        /// 删除医生快捷回复
        /// </summary>
        /// <param name="QuickReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QuickReply> DeleteQuickReply(QuickReply QuickReplyInfo)
        {
            return await _repository.UpdateAsync(QuickReplyInfo);
        }
    }
}
