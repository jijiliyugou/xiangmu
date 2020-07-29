using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 消息发送记录
    /// </summary>
    public class WecharSendMessage: EntityBaseModule
    {
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(30)]
        public virtual string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询姓名
        /// </summary>
        [MaxLength(500)]
        public virtual string ConsultantName { get; set; }
        /// <summary>
        /// 咨询医生姓名
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 接收者openid
        /// </summary>
        [MaxLength(100)]
        public virtual string ToUser { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        [MaxLength(100)]
        public virtual string TemplateCode { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [MaxLength(100)]
        public virtual string OperationType { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        [MaxLength(100)]
        public virtual string TemplateId { get; set; }
        /// <summary>
        /// 模板跳转链接
        /// </summary>
        [MaxLength(500)]
        public virtual string BackUrl { get; set; }
        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        [MaxLength(100)]
        public virtual string Miniprogram { get; set; }
        /// <summary>
        /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系，暂不支持小游戏）
        /// </summary>
        [MaxLength(100)]
        public virtual string Appid { get; set; }
        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），暂不支持小游戏
        /// </summary>
        [MaxLength(100)]
        public virtual string Pagepath { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        [MaxLength(5000)]
        public virtual string WecharData { get; set; }
        /// <summary>
        /// 消息类型是事件
        /// </summary>
        [MaxLength(100)]
        public virtual string MsgType { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        [MaxLength(100)]
        public virtual string MsgID { get; set; }
        /// <summary>
        /// 消息类型是事件
        /// </summary>
        [MaxLength(500)]
        public virtual string Status { get; set; }
        /// <summary>
        /// 发送头条
        /// </summary>
        [MaxLength(500)]
        public virtual string FirstMessage { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        [MaxLength(500)]
        public virtual string MessageRemark { get; set; }
        /// <summary>
        /// 关键字1
        /// </summary>
        [MaxLength(500)]
        public virtual string Keyword1 { get; set; }
        /// <summary>
        /// 关键字2
        /// </summary>
        [MaxLength(500)]
        public virtual string Keyword2 { get; set; }
        /// <summary>
        /// 关键字3
        /// </summary>
        [MaxLength(500)]
        public virtual string Keyword3 { get; set; }
        /// <summary>
        /// 咨询类型JSon
        /// </summary>
        public virtual string ConsultJson { get; set; }
    }
}
