using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的收藏
    /// </summary>
    public class YaeherPatientCollectionOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherPatientCollectionDto"></param>
        /// <param name="YaeherPatientCollectionInfo"></param>
        public YaeherPatientCollectionOut(PagedResultDto<YaeherPatientCollection> YaeherPatientCollectionDto, YaeherPatientCollectionIn YaeherPatientCollectionInfo)
        {
            Items = YaeherPatientCollectionDto.Items;
            TotalCount = YaeherPatientCollectionDto.TotalCount;
            TotalPage = YaeherPatientCollectionDto.TotalCount / YaeherPatientCollectionInfo.MaxResultCount;
            SkipCount = YaeherPatientCollectionInfo.SkipCount;
            MaxResultCount = YaeherPatientCollectionInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherPatientCollection> Items { get; set; }

    }
}
