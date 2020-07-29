using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生上下线设置
    /// </summary>
    public class DoctorOnlineRecordOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorOnlineRecordDto"></param>
        /// <param name="DoctorOnlineRecordInfo"></param>
        public DoctorOnlineRecordOut(PagedResultDto<DoctorOnlineRecord> DoctorOnlineRecordDto, DoctorOnlineRecordIn DoctorOnlineRecordInfo)
        {
            Items = DoctorOnlineRecordDto.Items;
            TotalCount = DoctorOnlineRecordDto.TotalCount;
            TotalPage = DoctorOnlineRecordDto.TotalCount / DoctorOnlineRecordInfo.MaxResultCount;
            SkipCount = DoctorOnlineRecordInfo.SkipCount;
            MaxResultCount = DoctorOnlineRecordInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorOnlineRecord> Items { get; set; }


    }
}
