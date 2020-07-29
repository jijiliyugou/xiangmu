using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 微信个性化菜单
    /// </summary>
    public class YaeherConditionalMenu : EntityBaseModule
    {
        /// <summary>
        /// 所属角色Code
        /// </summary>
        [MaxLength(20)]
        public virtual string RoleCode { get; set; }
        /// <summary>
        /// 所属角色名称
        /// </summary>
        [MaxLength(10)]
        public virtual string RoleName { get; set; }
        /// <summary>
        ///  对应微信个性化菜单ID
        /// </summary>
        public virtual int TagId { get; set; }
        /// <summary>
        /// 微信菜单显示名称
        /// </summary>
        [MaxLength(20)]
        public virtual string ConditionalName { get; set; }
        /// <summary>
        /// 菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型
        /// </summary>
        [MaxLength(10)]
        public virtual string ConditionalType { get; set; }
        /// <summary>
        /// 菜单相应动作类型ID,SystemParameter
        /// </summary>
        [MaxLength(100)]
        public virtual string ConditionalTypeName { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        [MaxLength(500)]
        public virtual string ConditionalUrl { get; set; }
        /// <summary>
        /// 小程序的appid
        /// </summary>
        [MaxLength(100)]
        public virtual string AppID { get; set; }
        /// <summary>
        /// 小程序的页面路径
        /// </summary>
        [MaxLength(200)]
        public virtual string Pagepath { get; set; }
        /// <summary>
        /// parentID
        /// </summary>
        public virtual int ParentID { get; set; }
        /// <summary>
        /// 个性化菜单微信ID
        /// </summary>
        [MaxLength(10)]
        public string MenuID { get; set; }
    }
}
