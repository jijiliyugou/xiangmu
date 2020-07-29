using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 医生规则制度 
    /// </summary>
    public class DoctorRules : EntityBaseModule
    {
        /// <summary>
        /// 规则类型  规则类型 指南 规则 制度 评分规则 等类型
        /// </summary>
        [MaxLength(20)]
        public virtual string RulesType { get; set; }
        /// <summary>
        /// 规则标题
        /// </summary>
        [MaxLength(100)]
        public virtual string RulesTitle { get; set; }
        /// <summary>
        /// 规则内容
        /// </summary>
        [MaxLength(5000)]
        public virtual string RulesContent { get; set; }
        /// <summary>
        /// 适用门诊名称
        /// </summary>
        [MaxLength(5000)]
        public virtual string ApplyClinicName { get; set; }
        /// <summary>
        /// 适用门诊ID  
        /// </summary>
        public virtual string ApplyClinicID { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        [MaxLength(100)]
        public string ImageFie { get; set; }
    }
}
