using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;

namespace Yaeher
{
    /// <summary>
    /// 我的收藏
    /// </summary>
    public class PatientCollectionService : IPatientCollectionService
    {
        private readonly IRepository<YaeherPatientCollection> _repository;
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="repository"></param>
        public PatientCollectionService(IRepository<YaeherPatientCollection> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 我的收藏 List
        /// </summary>
        /// <param name="YaeherPatientCollectionInfoList"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherPatientCollection>> YaeherPatientCollectionList(YaeherPatientCollectionIn YaeherPatientCollectionInfoList)
        {
            var YaeherPatientCollections = await _repository.GetAllListAsync(YaeherPatientCollectionInfoList.Expression);
            return YaeherPatientCollections.ToList();
        }

        /// <summary>
        /// 我的收藏byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientCollection> YaeherPatientCollectionByID(int Id)
        {
            var YaeherPatientCollections = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherPatientCollections;
        }
        /// <summary>
        /// 我的收藏 page
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherPatientCollection>> YaeherPatientCollectionPage(YaeherPatientCollectionIn YaeherPatientCollectionInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherPatientCollectionInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherPatientCollectionInfo.MaxResultCount;
            var YaeherPatientCollectionList = await query.PageBy(YaeherPatientCollectionInfo.SkipTotal, YaeherPatientCollectionInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherPatientCollection>(tasksCount, YaeherPatientCollectionList.MapTo<List<YaeherPatientCollection>>());
        }
        /// <summary>
        /// 新建我的收藏
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientCollection> CreateYaeherPatientCollection(YaeherPatientCollection YaeherPatientCollectionInfo)
        {
            YaeherPatientCollectionInfo.Id= await _repository.InsertAndGetIdAsync(YaeherPatientCollectionInfo);
            return YaeherPatientCollectionInfo;
        }

        /// <summary>
        /// 修改我的收藏
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientCollection> UpdateYaeherPatientCollection(YaeherPatientCollection YaeherPatientCollectionInfo)
        {
            return await _repository.UpdateAsync(YaeherPatientCollectionInfo);
        }

        /// <summary>
        /// 删除我的收藏
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherPatientCollection> DeleteYaeherPatientCollection(YaeherPatientCollection YaeherPatientCollectionInfo)
        {
            return await _repository.UpdateAsync(YaeherPatientCollectionInfo);
        }

    }
    
}
