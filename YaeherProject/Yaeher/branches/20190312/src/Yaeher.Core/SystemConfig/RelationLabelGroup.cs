using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 标签分组
    /// </summary>
     public class RelationLabelGroup:EntityBaseModule
    {
        /// <summary>
        /// 标签组名
        /// </summary>
        [MaxLength(100)]
        public virtual string GroupName { get; set; }
        /// <summary>
        /// 标签ID 已数组格式存储
        /// </summary>
        [MaxLength(5000)]
        public virtual string LableID { get; set; }
        /// <summary>
        /// 标签名称 已数组格式存储
        /// </summary>
        [MaxLength(5000)]
        public virtual string LableName { get; set; }

    }
}
