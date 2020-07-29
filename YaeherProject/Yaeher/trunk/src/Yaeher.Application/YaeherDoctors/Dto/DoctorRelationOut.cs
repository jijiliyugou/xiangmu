using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.LableManages.Dto;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生与标签关系
    /// </summary>
    public class DoctorRelationOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorRelationOutDto"></param>
        /// <param name="DoctorRelationInfo"></param>
        public DoctorRelationOut(PagedResultDto<DoctorRelation> DoctorRelationOutDto, DoctorRelationIn DoctorRelationInfo)
        {
            Items = DoctorRelationOutDto.Items;
            TotalCount = DoctorRelationOutDto.TotalCount;
            TotalPage = DoctorRelationOutDto.TotalCount / DoctorRelationInfo.MaxResultCount;
            SkipCount = DoctorRelationInfo.SkipCount;
            MaxResultCount = DoctorRelationInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorRelationOutDto"></param>
        /// <param name="DoctorRelationInfo"></param>
        public DoctorRelationOut(PagedResultDto<DoctorLale> DoctorRelationOutDto, LableManageIn DoctorRelationInfo)
        {
            Items = DoctorRelationOutDto.Items;
            TotalCount = DoctorRelationOutDto.TotalCount;
            TotalPage = DoctorRelationOutDto.TotalCount / DoctorRelationInfo.MaxResultCount;
            SkipCount = DoctorRelationInfo.SkipCount;
            MaxResultCount = DoctorRelationInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorRelation> Items { get; set; }

    }


}
