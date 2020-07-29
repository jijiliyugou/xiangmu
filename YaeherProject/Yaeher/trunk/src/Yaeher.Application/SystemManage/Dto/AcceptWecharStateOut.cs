using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 发送消息状态
    /// </summary>
    public class AcceptWecharStateOut:PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="AcceptWecharStateDto"></param>
        /// <param name="AcceptWecharStateInfo"></param>
        public AcceptWecharStateOut(PagedResultDto<AcceptWecharState> AcceptWecharStateDto, AcceptWecharStateIn AcceptWecharStateInfo)
        {
            Items = AcceptWecharStateDto.Items;
            TotalCount = AcceptWecharStateDto.TotalCount;
            TotalPage = AcceptWecharStateDto.TotalCount / AcceptWecharStateInfo.MaxResultCount;
            SkipCount = AcceptWecharStateInfo.SkipCount;
            MaxResultCount = AcceptWecharStateInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<AcceptWecharState> Items { get; set; }
    }
}
