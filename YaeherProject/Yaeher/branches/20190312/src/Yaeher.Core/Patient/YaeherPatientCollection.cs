using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的收藏
    /// </summary>
    public class YaeherPatientCollection : EntityBaseModule
    {
        /// <summary>
        /// 患者ID
        /// YaeherUser表ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        ///操作类型
        ///收藏 点赞
        /// </summary>
        [MaxLength(10)]
        public string Opentype { get; set; }
        /// <summary>
        ///收藏类型 
        ///问答或者文章
        /// </summary>
        [MaxLength(10)]
        public string CollectionType { get; set; }
        /// <summary>
        ///收藏链接地址
        /// </summary>
        [MaxLength(300)]
        public string CollectionUrl { get; set; }
        /// <summary>
        ///收藏信息JSON
        /// </summary>
        public string CollectionJSON { get; set; }

    }
}
