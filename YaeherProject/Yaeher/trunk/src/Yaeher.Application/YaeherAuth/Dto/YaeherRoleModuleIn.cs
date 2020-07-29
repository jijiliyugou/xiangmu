using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 角色与菜单管理
    /// </summary>
    public class YaeherRoleModuleIn : ListParameters<YaeherRoleModule>, IPagedResultRequest
    {
        /// <summary>
        /// 角色ID 查询角色与菜单关系 只需传角色ID
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual int ModuleId { get; set; }
    }
    /// <summary>
    /// 设置角色权限
    /// </summary>
    public class YaeherRoleModuleJSon : ListParameters<YaeherRoleModule>, IPagedResultRequest
    {
        /// <summary>
        /// 角色ID 查询角色与菜单关系 只需传角色ID
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual string ModuleId { get; set; }
    }

}
