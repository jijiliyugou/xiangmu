using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的医生
    /// </summary>
    public class YaeherPatientDoctorOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherPatientDoctorDto"></param>
        /// <param name="YaeherPatientDoctorInfo"></param>
        public YaeherPatientDoctorOut(PagedResultDto<YaeherPatientDoctor> YaeherPatientDoctorDto, YaeherPatientDoctorIn YaeherPatientDoctorInfo)
        {
            Items = YaeherPatientDoctorDto.Items;
            TotalCount = YaeherPatientDoctorDto.TotalCount;
            TotalPage = YaeherPatientDoctorDto.TotalCount / YaeherPatientDoctorInfo.MaxResultCount;
            SkipCount = YaeherPatientDoctorInfo.SkipCount;
            MaxResultCount = YaeherPatientDoctorInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherPatientDoctor> Items { get; set; }

    }
}
