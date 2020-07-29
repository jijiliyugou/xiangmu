using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 公司配置
    /// </summary>
    public class CompanyConfigOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="CompanyConfigDto"></param>
        /// <param name="CompanyConfigInfo"></param>
        public CompanyConfigOut(PagedResultDto<CompanyConfig> CompanyConfigDto, CompanyConfigIn CompanyConfigInfo)
        {
            Items = CompanyConfigDto.Items;
            TotalCount = CompanyConfigDto.TotalCount;
            TotalPage = CompanyConfigDto.TotalCount / CompanyConfigInfo.MaxResultCount;
            SkipCount = CompanyConfigInfo.SkipCount;
            MaxResultCount = CompanyConfigInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<CompanyConfig> Items { get; set; }
    }
}
