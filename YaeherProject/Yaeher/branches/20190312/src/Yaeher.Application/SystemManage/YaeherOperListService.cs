using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemManage.Dto;
using System;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class YaeherOperListService : IYaeherOperListService
    {
        private readonly IRepository<YaeherOperList> _repository;
        /// <summary>
        /// 系统操作日志 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherOperListService(IRepository<YaeherOperList> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询系统操作日志 List
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherOperList>> YaeherOperListList(YaeherOperListIn YaeherOperListInfo)
        {
            var YaeherOperLists = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherOperListInfo.Expression);
            return await YaeherOperLists.ToListAsync();
        }

        /// <summary>
        /// 查询系统操作日志byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherOperList> YaeherOperListByID(int Id)
        {
            var YaeherOperLists = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherOperLists;
        }
        /// <summary>
        /// 查询系统操作日志 page
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherOperList>> YaeherOperListPage(YaeherOperListIn YaeherOperListInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherOperListInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherOperListInfo.MaxResultCount;
            var YaeherOperListList = await query.PageBy(YaeherOperListInfo.SkipTotal, YaeherOperListInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherOperList>(tasksCount, YaeherOperListList.MapTo<List<YaeherOperList>>());
        }
        /// <summary>
        /// 新建系统操作日志
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherOperList> CreateYaeherOperList(YaeherOperList YaeherOperListInfo)
        {
            try
            {
                return new YaeherOperList();
                //YaeherOperListInfo.Id = await _repository.InsertAndGetIdAsync(YaeherOperListInfo);
                //return YaeherOperListInfo;
            }
            catch (Exception ex)
            {
                return new YaeherOperList();
            }
        }

        /// <summary>
        /// 修改系统操作日志
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherOperList> UpdateYaeherOperList(YaeherOperList YaeherOperListInfo)
        {
            return await _repository.UpdateAsync(YaeherOperListInfo);
        }

        /// <summary>
        /// 删除系统操作日志
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherOperList> DeleteYaeherOperList(YaeherOperList YaeherOperListInfo)
        {
            return await _repository.UpdateAsync(YaeherOperListInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="YaeherOperListInfo"></param>
        /// <returns></returns>
       [RemoteService(false)]
        public async Task<YaeherOperList> PatientYaeherOperList(YaeherOperList YaeherOperListInfo)
        {
           return await _repository.InsertAsync(YaeherOperListInfo);
        }
    }
}
