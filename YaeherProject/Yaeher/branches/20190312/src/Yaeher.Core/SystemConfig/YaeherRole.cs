using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 角色表 
    /// </summary>
    public class YaeherRole : EntityBaseModule
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [MaxLength(20)]
        public virtual string RoleName { get; set; }
        /// <summary>
        /// 角色Code
        /// </summary>
        [MaxLength(20)]
        public virtual string RoleCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public virtual string Description { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        public virtual bool Enabled { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public virtual bool IsAdmin { get; set; }

    }
}
