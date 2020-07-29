using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 文章操作日志表
    /// </summary>
    public class ArticleOperList : EntityBaseModule
    {
        /// <summary>
        /// 文章或者问答ID 
        /// </summary>
        public virtual int ArticleID { get; set; }
        /// <summary> 
        /// 操作类型  阅读 点赞 转发 收藏 等操作类型
        /// </summary>
        [MaxLength(20)]
        public virtual string OperType { get; set; }
        /// <summary>
        /// 类型 
        /// </summary>
        [MaxLength(20)]
        public virtual string Type { get; set; }

    }
}
