using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 接口管理
    /// </summary>
    public class InterfaceSetOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="InterfaceSetDto"></param>
        /// <param name="InterfaceSetInfo"></param>
        public InterfaceSetOut(PagedResultDto<InterfaceSet> InterfaceSetDto, InterfaceSetIn InterfaceSetInfo)
        {
            Items = InterfaceSetDto.Items;
            TotalCount = InterfaceSetDto.TotalCount;
            TotalPage = InterfaceSetDto.TotalCount / InterfaceSetInfo.MaxResultCount;
            SkipCount = InterfaceSetInfo.SkipCount;
            MaxResultCount = InterfaceSetInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<InterfaceSet> Items { get; set; }


    }
}
