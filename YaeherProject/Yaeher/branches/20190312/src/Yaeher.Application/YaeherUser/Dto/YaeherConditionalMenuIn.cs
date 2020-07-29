using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    public class YaeherConditionalMenuIn : ListParameters<YaeherConditionalMenu>, IPagedResultRequest
    {
        /// <summary>
        /// 所属角色Code
        /// </summary>
        public virtual string RoleCode { get; set; }
        /// <summary>
        /// 所属角色名称
        /// </summary>
        public virtual string RoleName { get; set; }      
        /// <summary>
        /// 微信菜单显示名称
        /// </summary>
        public virtual string ConditionalName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string ConditionalUrl { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WecharMenu : ListParameters<YaeherConditionalMenu>
    {
        /// <summary>
        /// 所属角色Code
        /// </summary>
        public virtual string RoleCode { get; set; }
        /// <summary>
        /// 所属角色名称
        /// </summary>
        public virtual string RoleName { get; set; }
        /// <summary>
        ///  对应微信个性化菜单ID
        /// </summary>
        public virtual int TagId { get; set; }
        /// <summary>
        /// 微信菜单显示名称
        /// </summary>
        public virtual string ConditionalName { get; set; }
        /// <summary>
        /// 菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型
        /// </summary>
        public virtual string ConditionalType { get; set; }
        /// <summary>
        /// 菜单相应动作类型ID,SystemParameter
        /// </summary>
        public virtual string ConditionalTypeName { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public virtual string ConditionalUrl { get; set; }
        /// <summary>
        /// 小程序的appid
        /// </summary>
        public virtual string AppID { get; set; }
        /// <summary>
        /// 小程序的页面路径
        /// </summary>
        public virtual string Pagepath { get; set; }
        /// <summary>
        /// parentID
        /// </summary>
        public virtual int ParentID { get; set; }
        /// <summary>
        /// 个性化菜单微信ID
        /// </summary>
        public string MenuID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WecharMenu> children { get; set; }
    }
}
