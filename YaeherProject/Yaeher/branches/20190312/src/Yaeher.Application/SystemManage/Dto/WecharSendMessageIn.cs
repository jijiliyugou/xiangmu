using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class WecharSendMessageIn : ListParameters<WecharSendMessage>, IPagedResultRequest
    {
        /// <summary>
        /// 咨询单号
        /// </summary>
        public virtual string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询姓名
        /// </summary>
        public virtual string ConsultantName { get; set; }
        /// <summary>
        /// 咨询医生姓名
        /// </summary>
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 消息模板类型  交易类型
        /// </summary>
        public virtual string TemplateCode { get; set; }
        /// <summary>
        /// 接收者openid
        /// </summary>
        public virtual string ToUser { get; set; }
        /// <summary>
        /// 微信模板ID
        /// </summary>
        public virtual string TemplateId { get; set; }
        /// <summary>
        /// 模板跳转链接
        /// </summary>
        public virtual string BackUrl { get; set; }
        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        public virtual string Miniprogram { get; set; }
        /// <summary>
        /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系，暂不支持小游戏）
        /// </summary>
        public virtual string Appid { get; set; }
        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），暂不支持小游戏
        /// </summary>
        public virtual string Pagepath { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        public virtual string WecharData { get; set; }
        /// <summary>
        /// 消息类型是事件
        /// </summary>
        public virtual string MsgType { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        public virtual string MsgID { get; set; }
        /// <summary>
        /// 消息类型是事件
        /// </summary>
        public virtual string Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AcceptWechar
    {

        public string echoStr { get; set; }
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
    }
}
