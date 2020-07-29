using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 微信消息模板
    /// </summary>
    public class YaeherMessageTemplate: EntityBaseModule
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        [MaxLength(100)]
        public virtual string TemplateCode { get; set; }
        /// <summary>
        /// 模板类型名称
        /// </summary>
        [MaxLength(100)]
        public virtual string Title { get; set; }
        /// <summary>
        /// 微信模板名称
        /// </summary>
        [MaxLength(100)]
        public virtual string WecharTitle { get; set; }
        /// <summary>
        /// 微信模板编号
        /// </summary>
        [MaxLength(1000)]
        public virtual string TemplateId { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 模板示例
        /// </summary>
        [MaxLength(2000)]
        public virtual string Example { get; set; }

    }
}
