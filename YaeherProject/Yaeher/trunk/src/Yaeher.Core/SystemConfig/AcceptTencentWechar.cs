using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 接受微信发送消息
    /// </summary>
    public class AcceptTencentWechar : EntityBaseModule
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        [MaxLength(100)]
        public virtual string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号
        /// </summary>
        [MaxLength(100)]
        public virtual string FromUserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [MaxLength(500)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户图像
        /// </summary>
        [MaxLength(300)]
        public string UserImage { get; set; }
        /// <summary>
        /// 咨询者全称
        /// </summary>
        [MaxLength(500)]
        public virtual string FullName { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        [MaxLength(20)]
        public virtual string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        [MaxLength(50)]
        public virtual string MsgType { get; set; }
        /// <summary>
        /// 文本消息内容
        /// </summary>
        [MaxLength(5000)]
        public virtual string Content { get; set; }
        /// <summary>
        /// 图片链接
        /// </summary>
        [MaxLength(300)]
        public virtual string PicUrl { get; set; }
        /// <summary>
        /// 视频消息媒体id
        /// </summary>
        [MaxLength(100)]
        public virtual string MediaId { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id
        /// </summary>
        [MaxLength(100)]
        public virtual string ThumbMediaId { get; set; }
        /// <summary>
        /// 语音格式
        /// </summary>
        [MaxLength(100)]
        public virtual string Format { get; set; }
        /// <summary>
        /// 语音识别结果
        /// </summary>
        [MaxLength(3000)]
        public virtual string Recognition { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        [MaxLength(200)]
        public virtual string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        [MaxLength(5000)]
        public virtual string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        [MaxLength(500)]
        public virtual string Url { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        [MaxLength(50)]
        public virtual string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        [MaxLength(50)]
        public virtual string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        [MaxLength(500)]
        public virtual string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        [MaxLength(200)]
        public virtual string Label { get; set; }
        /// <summary>
        /// 消息iD
        /// </summary>
        [MaxLength(200)]
        public virtual string MsgId { get; set; }
        /// <summary>
        /// 消息来源  客服 咨询者
        /// </summary>
        [MaxLength(200)]
        public virtual string MessageFrom { get; set; }
    }
}
