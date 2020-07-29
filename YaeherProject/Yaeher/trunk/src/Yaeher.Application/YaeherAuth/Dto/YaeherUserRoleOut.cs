using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 用户与角色管理
    /// </summary>
    public class YaeherUserRoleOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherUserRoleDto"></param>
        /// <param name="YaeherUserRoleInfo"></param>
        public YaeherUserRoleOut(PagedResultDto<YaeherUserRole> YaeherUserRoleDto, YaeherUserRoleIn YaeherUserRoleInfo)
        {
            Items = YaeherUserRoleDto.Items;
            TotalCount = YaeherUserRoleDto.TotalCount;
            TotalPage = YaeherUserRoleDto.TotalCount / YaeherUserRoleInfo.MaxResultCount;
            SkipCount = YaeherUserRoleInfo.SkipCount;
            MaxResultCount = YaeherUserRoleInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherUserRole> Items { get; set; }


    }
}
