using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.DoctorReport.Dto
{
    /// <summary>
    /// 医生收入明细
    /// </summary>
    public class IncomeDetailsOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="IncomeDetailsDto"></param>
        /// <param name="IncomeDetailsInfo"></param>
        public IncomeDetailsOut(PagedResultDto<IncomeDetails> IncomeDetailsDto, IncomeDetailsIn IncomeDetailsInfo)
        {
            Items = IncomeDetailsDto.Items;
            TotalCount = IncomeDetailsDto.TotalCount;
            TotalPage = IncomeDetailsDto.TotalCount / IncomeDetailsInfo.MaxResultCount;
            SkipCount = IncomeDetailsInfo.SkipCount;
            MaxResultCount = IncomeDetailsInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<IncomeDetails> Items { get; set; }
    }
}
