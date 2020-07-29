using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.MessageRemind.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.AutoMapper;

namespace Yaeher.MessageRemind
{
    /// <summary>
    /// 拨打电话
    /// </summary>
    public class YaeherPhoneService : IYaeherPhoneService
    {
        private readonly IRepository<YaeherPhone> _repository;
        /// <summary>
        /// 拨打电话 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherPhoneService(IRepository<YaeherPhone> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 电话记录 List
        /// </summary>
        /// <param name="YaeherPhoneInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherPhone>> YaeherPhoneList(YaeherPhoneIn YaeherPhoneInfo)
        {
            var YaeherPhones = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherPhoneInfo.Expression);
            return await YaeherPhones.ToListAsync();
        }

        /// <summary>
        /// 电话 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPhone> YaeherPhoneByID(int Id)
        {
            var YaeherPhones = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherPhones;
        }
        /// <summary>
        /// 电话 page
        /// </summary>
        /// <param name="YaeherPhoneInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherPhone>> YaeherPhonePage(YaeherPhoneIn YaeherPhoneInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherPhoneInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherPhoneInfo.MaxResultCount;
            var YaeherPhoneList = await query.PageBy(YaeherPhoneInfo.SkipTotal, YaeherPhoneInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherPhone>(tasksCount, YaeherPhoneList.MapTo<List<YaeherPhone>>());
        }
        /// <summary>
        /// 新建 电话
        /// </summary>
        /// <param name="YaeherPhoneInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPhone> CreateYaeherPhone(YaeherPhone YaeherPhoneInfo)
        {
            YaeherPhoneInfo.Id = await _repository.InsertAndGetIdAsync(YaeherPhoneInfo);
            return YaeherPhoneInfo;
        }
    }
}
