using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.CompaniesReport.Dto
{
    /// <summary>
    /// 公司收入明细
    /// </summary>
    public class CorporateIncomeDetailsOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="CorporateIncomeDetailsDto"></param>
        /// <param name="CorporateIncomeDetailsInfo"></param>
        public CorporateIncomeDetailsOut(PagedResultDto<CorporateIncomeDetails> CorporateIncomeDetailsDto, CorporateIncomeDetailsIn CorporateIncomeDetailsInfo)
        {
            Items = CorporateIncomeDetailsDto.Items;
            TotalCount = CorporateIncomeDetailsDto.TotalCount;
            TotalPage = CorporateIncomeDetailsDto.TotalCount / CorporateIncomeDetailsInfo.MaxResultCount;
            SkipCount = CorporateIncomeDetailsInfo.SkipCount;
            MaxResultCount = CorporateIncomeDetailsInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<CorporateIncomeDetails> Items { get; set; }
    }
}
