using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.DoctorRule.Dto
{
    /// <summary>
    /// 医生规则 制度 指南
    /// </summary>
    public class DoctorRulesOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorRulesDto"></param>
        /// <param name="DoctorRulesInfo"></param>
        public DoctorRulesOut(PagedResultDto<DoctorRules> DoctorRulesDto, DoctorRulesIn DoctorRulesInfo)
        {
            Items = DoctorRulesDto.Items;
            TotalCount = DoctorRulesDto.TotalCount;
            TotalPage = DoctorRulesDto.TotalCount / DoctorRulesInfo.MaxResultCount;
            SkipCount = DoctorRulesInfo.SkipCount;
            MaxResultCount = DoctorRulesInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorRules> Items { get; set; }
    }
    public class DoctorRulesView : DoctorRules
    {
        public DateTime CreatedOn { get; set; }
    }
    public class DoctorRulesDetailView : DoctorRules
    {
        public DateTime CreatedOn { get; set; }

        public DoctorRulesDetailView(DoctorRules rules)
        {
            RulesType = rules.RulesType;
            RulesTitle = rules.RulesTitle;
            RulesContent = rules.RulesContent;
            ApplyClinicName = rules.ApplyClinicName;
            ApplyClinicID = rules.ApplyClinicID.TrimStart(',').TrimEnd(',');
            ImageFie = rules.ImageFie;
            Id = rules.Id;
            CreatedBy = rules.CreatedBy;
            CreatedOn = rules.CreatedOn;
        }
    }
}
