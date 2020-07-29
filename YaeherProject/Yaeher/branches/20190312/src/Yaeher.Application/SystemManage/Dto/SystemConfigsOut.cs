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
    public class SystemConfigsOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="SystemConfigsDto"></param>
        /// <param name="SystemConfigsInfo"></param>
        public SystemConfigsOut(PagedResultDto<SystemConfigs> SystemConfigsDto, SystemConfigsIn SystemConfigsInfo)
        {
            Items = SystemConfigsDto.Items;
            TotalCount = SystemConfigsDto.TotalCount;
            TotalPage = SystemConfigsDto.TotalCount / SystemConfigsInfo.MaxResultCount;
            SkipCount = SystemConfigsInfo.SkipCount;
            MaxResultCount = SystemConfigsInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<SystemConfigs> Items { get; set; }
    }
    /// <summary>
    /// 微信端，敏感信息需要过滤
    /// </summary>
    public class SystemConfigsMobileOut:SystemConfigs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configs"></param>
        public SystemConfigsMobileOut(SystemConfigs configs)
        {
            Id = configs.Id;
            SystemType = configs.SystemType;
            SystemName = configs.SystemName;
            AppID = configs.AppID;
        }
    }
}
