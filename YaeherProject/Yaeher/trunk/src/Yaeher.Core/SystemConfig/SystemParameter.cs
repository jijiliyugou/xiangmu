using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 系统参数设置
    /// </summary>
    public class SystemParameter : EntityBaseModule
    {
        /// <summary>
        /// 参数类别
        /// </summary>
        [MaxLength(50)]
        public virtual string SystemType { get; set; }
        /// <summary>
        /// 参数类别Code
        /// </summary>
        [MaxLength(50)]
        public virtual string SystemCode { get; set; }
        /// <summary>
        /// 参数编号
        /// </summary>
        [MaxLength(50)]
        public virtual string Code { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        [MaxLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 项目参数值
        /// </summary>
        [MaxLength(100)]
        public virtual string ItemValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// c
        /// </summary>
        [MaxLength(100)]
        public virtual string Parameter { get; set; }
    }
}
