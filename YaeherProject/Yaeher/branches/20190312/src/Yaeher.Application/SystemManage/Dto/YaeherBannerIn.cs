using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 轮播图
    /// </summary>
    public class YaeherBannerIn : ListParameters<YaeherBanner>, IPagedResultRequest
    {
        /// <summary>
        /// 轮播图类型Code
        /// </summary>
        public virtual string BannerTypeCode { get; set; }
        /// <summary>
        /// 轮播图名称
        /// </summary>
        public virtual string BannerTypeName { get; set; }
        /// <summary>
        /// 轮播图名称
        /// </summary>
        public virtual string BannerName { get; set; }
        /// <summary>
        /// 轮播图 图片上传地址
        /// </summary>
        public virtual string BannerImageUrl { get; set; }
        /// <summary>
        /// 轮播图 跳转Url
        /// </summary>
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
