using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.CompaniesReport.Dto
{
    /// <summary>
    /// 公司收入汇总
    /// </summary>
    public class CorporateIncomeTotalOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="CorporateIncomeTotalDto"></param>
        /// <param name="CorporateIncomeTotalInfo"></param>
        public CorporateIncomeTotalOut(PagedResultDto<CorporateIncomeTotal> CorporateIncomeTotalDto, CorporateIncomeTotalIn CorporateIncomeTotalInfo)
        {
            Items = CorporateIncomeTotalDto.Items;
            TotalCount = CorporateIncomeTotalDto.TotalCount;
            TotalPage = CorporateIncomeTotalDto.TotalCount / CorporateIncomeTotalInfo.MaxResultCount;
            SkipCount = CorporateIncomeTotalInfo.SkipCount;
            MaxResultCount = CorporateIncomeTotalInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<CorporateIncomeTotal> Items { get; set; }
    }
}
