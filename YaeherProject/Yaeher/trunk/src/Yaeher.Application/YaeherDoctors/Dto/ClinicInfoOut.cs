using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.ClinicManage.Dto;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ClinicInfoOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicInfomationDto"></param>
        /// <param name="ClinicInfomationInfo"></param>
        public ClinicInfoOut(PagedResultDto<ClinicInfomationView> ClinicInfomationDto,
                            ClinicInfomationIn ClinicInfomationInfo)
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
        public IReadOnlyList<ClinicInfomationView> Items { get; set; }
    }
}
