using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SystemParameterOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="SystemParameterDto"></param>
        /// <param name="SystemParameterInfo"></param>
        public SystemParameterOut(PagedResultDto<SystemParameter> SystemParameterDto, SystemParameterIn SystemParameterInfo)
        {
            Items = SystemParameterDto.Items;
            TotalCount = SystemParameterDto.TotalCount;
            TotalPage = SystemParameterDto.TotalCount / SystemParameterInfo.MaxResultCount;
            SkipCount = SystemParameterInfo.SkipCount;
            MaxResultCount = SystemParameterInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<SystemParameter> Items { get; set; }


    }
}
