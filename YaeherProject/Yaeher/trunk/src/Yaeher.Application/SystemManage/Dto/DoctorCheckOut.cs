using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生审核
    /// </summary>
    public class DoctorCheckOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorCheckDto"></param>
        /// <param name="DoctorCheckInfo"></param>
        public DoctorCheckOut(PagedResultDto<DoctorCheck> DoctorCheckDto, DoctorCheckIn DoctorCheckInfo)
        {
            Items = DoctorCheckDto.Items;
            TotalCount = DoctorCheckDto.TotalCount;
            TotalPage = DoctorCheckDto.TotalCount / DoctorCheckInfo.MaxResultCount;
            SkipCount = DoctorCheckInfo.SkipCount;
            MaxResultCount = DoctorCheckInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorCheck> Items { get; set; }


    }
}
