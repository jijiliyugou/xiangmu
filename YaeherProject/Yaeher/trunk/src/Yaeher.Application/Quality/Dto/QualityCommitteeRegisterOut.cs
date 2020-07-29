using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.Quality.Dto
{
    /// <summary>
    /// 质控委员注册
    /// </summary>
    public class QualityCommitteeRegisterOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="QualityCommitteeRegisterDto"></param>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        public QualityCommitteeRegisterOut(PagedResultDto<QualityCommitteeRegister> QualityCommitteeRegisterDto, QualityCommitteeRegisterIn QualityCommitteeRegisterInfo)
        {
            Items = QualityCommitteeRegisterDto.Items;
            TotalCount = QualityCommitteeRegisterDto.TotalCount;
            TotalPage = QualityCommitteeRegisterDto.TotalCount / QualityCommitteeRegisterInfo.MaxResultCount;
            SkipCount = QualityCommitteeRegisterInfo.SkipCount;
            MaxResultCount = QualityCommitteeRegisterInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<QualityCommitteeRegister> Items { get; set; }
    }
}
