using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 公司配置
    /// </summary>
    public class CompanyConfig: EntityBaseModule
    {
        /// <summary>
        /// 参数类别
        /// </summary>
        [MaxLength(10)]
        public virtual string ConfigType { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        [MaxLength(100)]
        public virtual string ConfigImageUrl { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        [MaxLength(100)]
        public virtual string ConfigTitle { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        [MaxLength(2000)]
        public virtual string ConfigContent { get; set; }
    }
}
