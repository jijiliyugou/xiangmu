using Abp.Application.Services.Dto;


namespace Yaeher.DoctorRule.Dto
{
    /// <summary>
    /// 医生规则 制度 指南
    /// </summary>
    public class DoctorRulesIn : ListParameters<DoctorRules>, IPagedResultRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 规则类型  规则类型 指南 规则 制度 评分规则 等类型
        /// </summary>
        public virtual string RulesType { get; set; }
        /// <summary>
        /// 处理后的科室id  如:  ,1,
        /// </summary>
        public string IdCheck { get; set; }

    }
}
