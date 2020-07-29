using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 角色与菜单关系表
    /// </summary>
    public class YaeherRoleModule : EntityBaseModule
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual int ModuleId { get; set; }

    }
}
