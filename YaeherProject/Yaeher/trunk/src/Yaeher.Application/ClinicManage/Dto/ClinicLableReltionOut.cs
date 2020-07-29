using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 门诊与标签关系
    /// </summary>
    public class ClinicLableReltionOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicLableReltionDto"></param>
        /// <param name="ClinicLableReltionInfo"></param>
        public ClinicLableReltionOut(PagedResultDto<ClinicLableReltion> ClinicLableReltionDto, ClinicLableReltionIn ClinicLableReltionInfo)
        {
            Items = ClinicLableReltionDto.Items;
            TotalCount = ClinicLableReltionDto.TotalCount;
            TotalPage = ClinicLableReltionDto.TotalCount / ClinicLableReltionInfo.MaxResultCount;
            SkipCount = ClinicLableReltionInfo.SkipCount;
            MaxResultCount = ClinicLableReltionInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ClinicLableReltion> Items { get; set; }
    }
}
