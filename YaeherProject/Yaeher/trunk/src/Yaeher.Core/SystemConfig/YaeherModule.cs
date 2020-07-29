using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 菜单表
    /// </summary>
    public class YaeherModule : EntityBaseModule
    {
        /// <summary>
        /// ParentId
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [MaxLength(20)]
        public virtual string Names { get; set; }
        /// <summary>
        /// LinkUrl
        /// </summary>
        [MaxLength(100)]
        public virtual string LinkUrls { get; set; }
        /// <summary>
        /// Area
        /// </summary>
        [MaxLength(20)]
        public virtual string Areas { get; set; }
        /// <summary>
        /// Controller
        /// </summary>
        [MaxLength(20)]
        public virtual string Controllers { get; set; }
        /// <summary>
        /// Actions
        /// </summary>
        [MaxLength(20)]
        public virtual string Actionss { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        [MaxLength(100)]
        public virtual string Icons{ get; set; }
        /// <summary>
        /// Code
        /// </summary>
        [MaxLength(20)]
        public virtual string Codes { get; set; }
        /// <summary>
        /// OrderSort
        /// </summary>
        public virtual int OrderSort { get; set; }
        /// <summary>
        /// 登陆名称
        /// </summary>
        [MaxLength(20)]
        public virtual string Description { get; set; }
        /// <summary>
        /// 是否菜单  当选择不是菜单时  ParentId为0
        /// </summary>
        public virtual bool IsMenu { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual bool Enabled { get; set; }

    }
}
