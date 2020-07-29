using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.MessageRemind.Dto;
using Yaeher.Common.SendMsm;
using Yaeher.SystemConfig;
using Yaeher.Common.CloudCallCenter;
using System;

namespace Yaeher.MessageRemind
{
    /// <summary>
    /// 短信对接
    /// </summary>
    public class YaeherMessageRemindService : IYaeherMessageRemindService
    {
        private readonly IRepository<YaeherMessageRemind> _repository;
        /// <summary>
        /// 短信对接 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherMessageRemindService(IRepository<YaeherMessageRemind> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询短信对接 List
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherMessageRemind>> YaeherMessageRemindList(YaeherMessageRemindIn YaeherMessageRemindInfo)
        {
            var YaeherMessageReminds = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherMessageRemindInfo.Expression);
            return await YaeherMessageReminds.ToListAsync();
        }

        /// <summary>
        /// 查询短信对接byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageRemind> YaeherMessageRemindByID(int Id)
        {
            var YaeherMessageReminds = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherMessageReminds;
        }
        /// <summary>
        /// 查询短信对接 page
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherMessageRemind>> YaeherMessageRemindPage(YaeherMessageRemindIn YaeherMessageRemindInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherMessageRemindInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherMessageRemindInfo.MaxResultCount;
            var YaeherMessageRemindList = await query.PageBy(YaeherMessageRemindInfo.SkipTotal, YaeherMessageRemindInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherMessageRemind>(tasksCount, YaeherMessageRemindList.MapTo<List<YaeherMessageRemind>>());
        }
        /// <summary>
        /// 新建短信对接
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageRemind> CreateYaeherMessageRemind(YaeherMessageRemind YaeherMessageRemindInfo)
        {
            #region AliCallCenter
            AliCallCenter aliCallCenter = new AliCallCenter();
            //aliCallCenter.StartBack2BackCall();  呼叫
            //await aliCallCenter.ListCallDetailRecords();

            //AliAccessToken aliAccessToken = new AliAccessToken();
            //AccessTokenInfo accessTokenInfo = new AccessTokenInfo();
            //// 人工维护
            //accessTokenInfo.code = "BQV6KDyUJ7IrILYgBtz3USuJA3R8nbPv";
            //AliAccessTokenEntity aliAccessTokenEntity1= await aliAccessToken.AccessToken(accessTokenInfo);
            //accessTokenInfo.refresh_token = aliAccessTokenEntity1.refresh_token;
            //AliAccessTokenEntity aliAccessTokenEntity2 = await aliAccessToken.RefreshAccessToken(accessTokenInfo);
            #endregion
            YaeherMessageRemindInfo.Id = await _repository.InsertAndGetIdAsync(YaeherMessageRemindInfo);
            return YaeherMessageRemindInfo;
        }

        /// <summary>
        /// 修改短信对接
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageRemind> UpdateYaeherMessageRemind(YaeherMessageRemind YaeherMessageRemindInfo)
        {
            return await _repository.UpdateAsync(YaeherMessageRemindInfo);
        }

        /// <summary>
        /// 删除短信对接
        /// </summary>
        /// <param name="YaeherMessageRemindInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherMessageRemind> DeleteYaeherMessageRemind(YaeherMessageRemind YaeherMessageRemindInfo)
        {
            return await _repository.UpdateAsync(YaeherMessageRemindInfo);
        }
    }
}
