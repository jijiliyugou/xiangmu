using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 用户与角色管理
    /// </summary>
    public class YaeherUserRoleIn : ListParameters<YaeherUserRole>, IPagedResultRequest
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
    /// <summary>
    /// 用户分配角色
    /// </summary>
    public class YaeherUserRoleJSON : ListParameters<YaeherUserRole>, IPagedResultRequest
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual string RoleID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserID { get; set; }
    }
}
