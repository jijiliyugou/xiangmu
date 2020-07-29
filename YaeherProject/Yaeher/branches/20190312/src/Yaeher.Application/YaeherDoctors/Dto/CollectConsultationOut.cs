using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.Doctor;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 订单收入分配_医生
    /// </summary>
    public class CollectConsultationOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="CollectConsultationDto"></param>
        /// <param name="CollectConsultationInfo"></param>
        public CollectConsultationOut(PagedResultDto<CollectConsultation> CollectConsultationDto, CollectConsultationIn CollectConsultationInfo)
        {
            Items = CollectConsultationDto.Items;
            TotalCount = CollectConsultationDto.TotalCount;
            TotalPage = CollectConsultationDto.TotalCount / CollectConsultationInfo.MaxResultCount;
            SkipCount = CollectConsultationInfo.SkipCount;
            MaxResultCount = CollectConsultationInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<CollectConsultation> Items { get; set; }


    }
}
