using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class YaeherModuleIn : ListParameters<YaeherModule>, IPagedResultRequest
    {
        /// <summary>
        /// 上一级 上级菜单ID  菜单获取上一级的时候 传为0  当不传时 默认值为-1
        /// </summary>
        public virtual int UpperLevel { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual String Enabled { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class YaeherModuleNode : ListParameters<YaeherModule>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public virtual string Names { get; set; }
        /// <summary>
        /// LinkUrl
        /// </summary>
        public virtual string LinkUrls { get; set; }
        /// <summary>
        /// Area
        /// </summary>
        public virtual string Areas { get; set; }
        /// <summary>
        /// Controller
        /// </summary>
        public virtual string Controllers { get; set; }
        /// <summary>
        /// Actions
        /// </summary>
        public virtual string Actionss { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public virtual string Icons { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public virtual string Codes { get; set; }
        /// <summary>
        /// OrderSort
        /// </summary>
        public virtual int OrderSort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 是否菜单  当选择不是菜单时  ParentId为0
        /// </summary>
        public virtual bool IsMenu { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<YaeherModuleNode> children { get; set; }

    }
}
