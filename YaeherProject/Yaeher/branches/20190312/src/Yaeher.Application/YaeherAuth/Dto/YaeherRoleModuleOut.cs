using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 角色与菜单管理
    /// </summary>
    public class YaeherRoleModuleOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherRoleModuleDto"></param>
        /// <param name="YaeherRoleModuleInfo"></param>
        public YaeherRoleModuleOut(PagedResultDto<YaeherRoleModule> YaeherRoleModuleDto, YaeherRoleModuleIn YaeherRoleModuleInfo)
        {
            Items = YaeherRoleModuleDto.Items;
            TotalCount = YaeherRoleModuleDto.TotalCount;
            TotalPage = YaeherRoleModuleDto.TotalCount / YaeherRoleModuleInfo.MaxResultCount;
            SkipCount = YaeherRoleModuleInfo.SkipCount;
            MaxResultCount = YaeherRoleModuleInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherRoleModule> Items { get; set; }


    }
}
