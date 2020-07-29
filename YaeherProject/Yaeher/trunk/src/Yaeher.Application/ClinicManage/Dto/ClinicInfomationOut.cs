using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 门诊信息
    /// </summary>
    public class ClinicInfomationOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ClinicInfomationDto"></param>
        /// <param name="ClinicInfomationInfo"></param>
        public ClinicInfomationOut(PagedResultDto<ClinicInfomation> ClinicInfomationDto, ClinicInfomationIn ClinicInfomationInfo)
        {
            Items = ClinicInfomationDto.Items;
            TotalCount = ClinicInfomationDto.TotalCount;
            TotalPage = ClinicInfomationDto.TotalCount / ClinicInfomationInfo.MaxResultCount;
            SkipCount = ClinicInfomationInfo.SkipCount;
            MaxResultCount = ClinicInfomationInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ClinicInfomation> Items { get; set; }
    }
}
