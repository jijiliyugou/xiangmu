using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 我的收藏
    /// </summary>
    public interface IPatientCollectionService: IApplicationService
    {
        /// <summary>
        /// 新建我的收藏
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        Task<YaeherPatientCollection> CreateYaeherPatientCollection(YaeherPatientCollection YaeherPatientCollectionInfo);
        /// <summary>
        /// 删除我的收藏
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        Task<YaeherPatientCollection> DeleteYaeherPatientCollection(YaeherPatientCollection YaeherPatientCollectionInfo);
        /// <summary>
        /// 修改我的收藏
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        Task<YaeherPatientCollection> UpdateYaeherPatientCollection(YaeherPatientCollection YaeherPatientCollectionInfo);
        /// <summary>
        /// 我的收藏byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherPatientCollection> YaeherPatientCollectionByID(int Id);
        /// <summary>
        /// 我的收藏 List
        /// </summary>
        /// <param name="YaeherPatientCollectionInfoList"></param>
        /// <returns></returns>
        Task<IList<YaeherPatientCollection>> YaeherPatientCollectionList(YaeherPatientCollectionIn YaeherPatientCollectionInfoList);
        /// <summary>
        /// 我的收藏 page
        /// </summary>
        /// <param name="YaeherPatientCollectionInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherPatientCollection>> YaeherPatientCollectionPage(YaeherPatientCollectionIn YaeherPatientCollectionInfo);
    }
}