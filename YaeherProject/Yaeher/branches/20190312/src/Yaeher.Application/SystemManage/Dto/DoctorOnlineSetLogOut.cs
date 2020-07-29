using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生上下线设置Log
    /// </summary>
    public class DoctorOnlineSetLogOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorOnlineSetLogDto"></param>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        public DoctorOnlineSetLogOut(PagedResultDto<DoctorOnlineSetLog> DoctorOnlineSetLogDto, DoctorOnlineSetLogIn DoctorOnlineSetLogInfo)
        {
            Items = DoctorOnlineSetLogDto.Items;
            TotalCount = DoctorOnlineSetLogDto.TotalCount;
            TotalPage = DoctorOnlineSetLogDto.TotalCount / DoctorOnlineSetLogInfo.MaxResultCount;
            SkipCount = DoctorOnlineSetLogInfo.SkipCount;
            MaxResultCount = DoctorOnlineSetLogInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorOnlineSetLog> Items { get; set; }


    }
}
