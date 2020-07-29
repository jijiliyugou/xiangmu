using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.Quality.Dto
{
    /// <summary>
    /// 质控委员会
    /// </summary>
    public class QualityCommitteeOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="QualityCommitteeDto"></param>
        /// <param name="QualityCommitteeInfo"></param>
        public QualityCommitteeOut(PagedResultDto<QualityCommittee> QualityCommitteeDto, QualityCommitteeIn QualityCommitteeInfo)
        {
            Items = QualityCommitteeDto.Items;
            TotalCount = QualityCommitteeDto.TotalCount;
            TotalPage = QualityCommitteeDto.TotalCount / QualityCommitteeInfo.MaxResultCount;
            SkipCount = QualityCommitteeInfo.SkipCount;
            MaxResultCount = QualityCommitteeInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<QualityCommittee> Items { get; set; }
    }
}
