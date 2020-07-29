using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public  class DoctorParaSetOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorParaSetDto"></param>
        /// <param name="DoctorParaSetInfo"></param>
        public DoctorParaSetOut(PagedResultDto<DoctorParaSet> DoctorParaSetDto, DoctorParaSetIn DoctorParaSetInfo)
        {
            Items = DoctorParaSetDto.Items;
            TotalCount = DoctorParaSetDto.TotalCount;
            TotalPage = DoctorParaSetDto.TotalCount / DoctorParaSetInfo.MaxResultCount;
            SkipCount = DoctorParaSetInfo.SkipCount;
            MaxResultCount = DoctorParaSetInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorParaSet> Items { get; set; }


    }
}
