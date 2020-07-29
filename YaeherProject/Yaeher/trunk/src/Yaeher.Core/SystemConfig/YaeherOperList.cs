using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 操作日志表
    /// </summary>
    public class YaeherOperList : EntityBaseModule
    {
        /// <summary>
        /// 操作说明
        /// </summary>
        [MaxLength(2000)]
        public virtual string OperExplain { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public virtual string OperContent { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [MaxLength(100)]
        public virtual string OperType { get; set; }

    }
}
