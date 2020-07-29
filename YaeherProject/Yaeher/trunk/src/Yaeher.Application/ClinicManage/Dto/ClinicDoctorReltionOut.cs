using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 门诊与医生关系
    /// </summary>
    public class ClinicDoctorReltionOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicDoctorReltionDto"></param>
        /// <param name="ClinicDoctorReltionInfo"></param>
        public ClinicDoctorReltionOut(PagedResultDto<ClinicDoctorReltion> ClinicDoctorReltionDto, ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            Items = ClinicDoctorReltionDto.Items;
            TotalCount = ClinicDoctorReltionDto.TotalCount;
            TotalPage = ClinicDoctorReltionDto.TotalCount / ClinicDoctorReltionInfo.MaxResultCount;
            SkipCount = ClinicDoctorReltionInfo.SkipCount;
            MaxResultCount = ClinicDoctorReltionInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ClinicDoctorReltion> Items { get; set; }
    }
}
