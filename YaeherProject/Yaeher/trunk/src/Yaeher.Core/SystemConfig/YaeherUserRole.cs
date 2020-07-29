using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 角色与用户关联表
    /// </summary>
    public class YaeherUserRole : EntityBaseModule
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual int RoleID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserID { get; set; }
    }
}
