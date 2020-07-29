using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.Doctor;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生执业
    /// </summary>
    public class DoctorEmploymentOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorEmploymentDto"></param>
        /// <param name="DoctorEmploymentInfo"></param>
        public DoctorEmploymentOut(PagedResultDto<DoctorEmployment> DoctorEmploymentDto, DoctorEmploymentIn DoctorEmploymentInfo)
        {
            Items = DoctorEmploymentDto.Items;
            TotalCount = DoctorEmploymentDto.TotalCount;
            TotalPage = DoctorEmploymentDto.TotalCount / DoctorEmploymentInfo.MaxResultCount;
            SkipCount = DoctorEmploymentInfo.SkipCount;
            MaxResultCount = DoctorEmploymentInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorEmployment> Items { get; set; }


    }
}
