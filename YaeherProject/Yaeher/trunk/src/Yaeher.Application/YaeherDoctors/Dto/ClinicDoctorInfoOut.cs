using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yaeher.ClinicManage.Dto;
using Yaeher.Quality.Dto;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ClinicDoctorInfoOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicInfomationDto"></param>
        /// <param name="ClinicInfomationInfo"></param>
        public ClinicDoctorInfoOut(PagedResultDto<ClinicDoctorsView> ClinicInfomationDto,
                            ClinicInfomationIn ClinicInfomationInfo)
        {
            Items = ClinicInfomationDto.Items;
            TotalCount = ClinicInfomationDto.TotalCount;
            TotalPage = ClinicInfomationDto.TotalCount / ClinicInfomationInfo.MaxResultCount;
            SkipCount = ClinicInfomationInfo.SkipCount;
            MaxResultCount = ClinicInfomationInfo.MaxResultCount;
        }

        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicInfomationDto"></param>
        /// <param name="ClinicInfomationInfo"></param>
        public ClinicDoctorInfoOut(PagedResultDto<ClinicDoctorsView> ClinicInfomationDto,
                            YaeherDoctorSearch ClinicInfomationInfo)
        {
            Items = ClinicInfomationDto.Items;
            TotalCount = ClinicInfomationDto.TotalCount;
            TotalPage = ClinicInfomationDto.TotalCount / ClinicInfomationInfo.MaxResultCount;
            SkipCount = ClinicInfomationInfo.SkipCount;
            MaxResultCount = ClinicInfomationInfo.MaxResultCount;
        }
        /// <summary>
        /// 科室集合
        /// </summary>
        public IReadOnlyList<ClinicDoctorsView> Items { get; set; }
    }
  

}
