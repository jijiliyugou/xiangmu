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
    /// 轮播图管理
    /// </summary>
    public class YaeherBannerService : IYaeherBannerService
    {
        private readonly IRepository<YaeherBanner> _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public YaeherBannerService(IRepository<YaeherBanner> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 轮播图管理 List
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherBanner>> YaeherBannerList(YaeherBannerIn YaeherBannerInfo)
        {
            //初步过滤
            var YaeherBanners = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherBannerInfo.Expression);
            return await YaeherBanners.ToListAsync();
        }

        /// <summary>
        /// 轮播图管理 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherBanner> YaeherBannerByID(int Id)
        {
            var YaeherBanners = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherBanners;
        }
        /// <summary>
        /// 轮播图管理 page
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherBanner>> YaeherBannerPage(YaeherBannerIn YaeherBannerInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherBannerInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherBannerInfo.MaxResultCount;
            var YaeherBannerList = await query.PageBy(YaeherBannerInfo.SkipTotal, YaeherBannerInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherBanner>(tasksCount, YaeherBannerList.MapTo<List<YaeherBanner>>());
        }
        /// <summary>
        /// 新建 轮播图管理
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherBanner> CreateYaeherBanner(YaeherBanner YaeherBannerInfo)
        {
            YaeherBannerInfo.Id = await _repository.InsertAndGetIdAsync(YaeherBannerInfo);
            return YaeherBannerInfo;
        }

        /// <summary>
        /// 修改 轮播图管理
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherBanner> UpdateYaeherBanner(YaeherBanner YaeherBannerInfo)
        {
            return await _repository.UpdateAsync(YaeherBannerInfo);
        }

        /// <summary>
        /// 删除 轮播图管理
        /// </summary>
        /// <param name="YaeherBannerInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherBanner> DeleteYaeherBanner(YaeherBanner YaeherBannerInfo)
        {
            return await _repository.UpdateAsync(YaeherBannerInfo);
        }
    }
}
