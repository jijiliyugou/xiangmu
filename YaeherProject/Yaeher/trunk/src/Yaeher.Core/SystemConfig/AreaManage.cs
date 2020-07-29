using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 地区维护
    /// </summary>
    public class AreaManage: EntityBaseModule
    {       
        /// <summary>
        /// 地区编号
        /// </summary>
        [MaxLength(20)]
        public virtual string Code { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        [MaxLength(20)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(5000)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [MaxLength(20)]
        public virtual string Postcode { get; set; }
    }
}
