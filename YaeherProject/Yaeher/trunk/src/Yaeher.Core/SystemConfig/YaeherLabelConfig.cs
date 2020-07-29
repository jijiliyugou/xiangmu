using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 标签配置
    /// </summary>
    public class YaeherLabelConfig: EntityBaseModule
    {
        /// <summary>
        /// 标签类型
        /// </summary>
        [MaxLength(100)]
        public virtual string LabelTypeCode { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        [MaxLength(100)]
        public virtual string LabelTypeName { get; set; }
        /// <summary>
        /// 标签编号
        /// </summary>
        [MaxLength(100)]
        public virtual string LabelCode { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [MaxLength(100)]
        public virtual string LabelName { get; set; }
        /// <summary>
        /// parentID
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// OrderSort
        /// </summary>
        public virtual int OrderSort { get; set; }
    }
}
