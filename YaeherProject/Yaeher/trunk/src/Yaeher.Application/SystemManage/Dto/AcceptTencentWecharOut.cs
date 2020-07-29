using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class AcceptTencentWecharOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="AcceptTencentWecharDto"></param>
        /// <param name="AcceptTencentWecharInfo"></param>
        public AcceptTencentWecharOut(PagedResultDto<AcceptTencentWechar> AcceptTencentWecharDto, AcceptTencentWecharIn AcceptTencentWecharInfo)
        {
            Items = AcceptTencentWecharDto.Items;
            TotalCount = AcceptTencentWecharDto.TotalCount;
            TotalPage = AcceptTencentWecharDto.TotalCount / AcceptTencentWecharInfo.MaxResultCount;
            SkipCount = AcceptTencentWecharInfo.SkipCount;
            MaxResultCount = AcceptTencentWecharInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<AcceptTencentWechar> Items { get; set; }
    }
}
