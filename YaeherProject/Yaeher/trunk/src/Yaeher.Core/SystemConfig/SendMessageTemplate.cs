using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 微信发送消息模板
    /// </summary>
    public class SendMessageTemplate: EntityBaseModule
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        [MaxLength(200)]
        public virtual string TemplateCode { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [MaxLength(100)]
        public virtual string OperationType { get; set; }
        /// <summary>
        /// 接受人
        /// </summary>
        [MaxLength(50)]
        public virtual string Recipient { get; set; }
        /// <summary>
        /// 回访Url
        /// </summary>
        [MaxLength(500)]
        public virtual string BackUrl { get; set; }
        /// <summary>
        /// FirstMessage
        /// </summary>
        [MaxLength(200)]
        public virtual string FirstMessage { get; set; }
        /// <summary>
        /// Keyword1
        /// </summary>
        [MaxLength(100)]
        public virtual string Keyword1 { get; set; }
        /// <summary>
        /// Keyword2
        /// </summary>
        [MaxLength(100)]
        public virtual string Keyword2 { get; set; }
        /// <summary>
        /// Keyword3
        /// </summary>
        [MaxLength(100)]
        public virtual string Keyword3 { get; set; }
        /// <summary>
        /// MessageRemark
        /// </summary>
        [MaxLength(200)]
        public virtual string MessageRemark { get; set; }
    }
}
