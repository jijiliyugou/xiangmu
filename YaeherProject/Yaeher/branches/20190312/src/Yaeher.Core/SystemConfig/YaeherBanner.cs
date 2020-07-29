using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 轮播图设置
    /// </summary>
    public class YaeherBanner: EntityBaseModule
    {
        /// <summary>
        /// 轮播图类型Code
        /// </summary>
        [MaxLength(100)]
        public virtual string BannerTypeCode { get; set; }
        /// <summary>
        /// 轮播图名称
        /// </summary>
        [MaxLength(100)]
        public virtual string BannerTypeName { get; set; }
        /// <summary>
        /// 轮播图名称
        /// </summary>
        [MaxLength(100)]
        public virtual string BannerName { get; set; }
        /// <summary>
        /// 轮播图 图片上传地址
        /// </summary>
        [MaxLength(500)]
        public virtual string BannerImageUrl { get; set; }
        /// <summary>
        /// 轮播图 跳转Url
        /// </summary>
        [MaxLength(500)]
        public virtual string BannerUrl { get; set; }
        /// <summary>
        /// 播放开始时间
        /// </summary>
        public virtual DateTime PlayStartTime { get; set; }
        /// <summary>
        /// 播放结束时间
        /// </summary>
        public virtual DateTime PlayEndTime { get; set; }
    }
}
