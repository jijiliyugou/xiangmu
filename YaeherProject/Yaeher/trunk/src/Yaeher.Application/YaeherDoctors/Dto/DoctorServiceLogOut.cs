using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生提供服务Log日志
    /// </summary>
    public class DoctorServiceLogOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorServiceLogOutDto"></param>
        /// <param name="DoctorServiceLogInfo"></param>
        public DoctorServiceLogOut(PagedResultDto<DoctorServiceLog> DoctorServiceLogOutDto, DoctorServiceLogIn DoctorServiceLogInfo)
        {
            Items = DoctorServiceLogOutDto.Items;
            TotalCount = DoctorServiceLogOutDto.TotalCount;
            TotalPage = DoctorServiceLogOutDto.TotalCount / DoctorServiceLogInfo.MaxResultCount;
            SkipCount = DoctorServiceLogInfo.SkipCount;
            MaxResultCount = DoctorServiceLogInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorServiceLog> Items { get; set; }

    }
}
