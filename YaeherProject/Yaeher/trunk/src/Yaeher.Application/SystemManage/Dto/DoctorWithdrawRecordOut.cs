using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生提现记录表
    /// </summary>
    public class DoctorWithdrawRecordOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorWithdrawRecordDto"></param>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        public DoctorWithdrawRecordOut(PagedResultDto<DoctorWithdrawRecord> DoctorWithdrawRecordDto, DoctorWithdrawRecordIn DoctorWithdrawRecordInfo)
        {
            Items = DoctorWithdrawRecordDto.Items;
            TotalCount = DoctorWithdrawRecordDto.TotalCount;
            TotalPage = DoctorWithdrawRecordDto.TotalCount / DoctorWithdrawRecordInfo.MaxResultCount;
            SkipCount = DoctorWithdrawRecordInfo.SkipCount;
            MaxResultCount = DoctorWithdrawRecordInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorWithdrawRecord> Items { get; set; }


    }
}
