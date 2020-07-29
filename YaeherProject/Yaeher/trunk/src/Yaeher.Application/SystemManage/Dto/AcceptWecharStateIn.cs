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
    public class AcceptWecharStateIn : ListParameters<AcceptWecharState>, IPagedResultRequest
    {
        /// <summary>
        /// 咨询用户ID
        /// YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询姓名
        /// YaeherUser表fullname
        /// </summary>
        public string ConsultantName { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 咨询人微信OpenID
        /// </summary>
        public string ConsultantOpenID { get; set; }
        /// <summary>
        /// 客服ID
        /// YaeherUser表ID
        /// </summary>
        public int CustomerServiceID { get; set; }
        /// <summary>
        /// 客服姓名
        /// YaeherUser表fullname
        /// </summary>
        public string CustomerServiceName { get; set; }
    }

    /// <summary>
    /// 微信聊天状态
    /// </summary>
    public class WecharState {

        /// <summary>
        /// 消息状态id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 咨询用户ID
        /// YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询姓名
        /// YaeherUser表fullname
        /// </summary>
        public string ConsultantName { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 咨询人微信OpenID
        /// </summary>
        public string ConsultantOpenID { get; set; }
        /// <summary>
        /// 客服ID
        /// YaeherUser表ID
        /// </summary>
        public int CustomerServiceID { get; set; }
        /// <summary>
        /// 客服姓名
        /// YaeherUser表fullname
        /// </summary>
        public string CustomerServiceName { get; set; }
        /// <summary>
        /// 客服用户JSON
        /// </summary>
        public string CustomerServiceJson { get; set; }
        /// <summary>
        /// 处理状态 2（开启） 3（关闭） 1（待办）
        /// </summary>
        public string AcceptState { get; set; }
        /// <summary>
        /// 读取状态
        /// </summary>
        public bool IsReady { get; set; }
        /// <summary>
        /// 用户图像
        /// </summary>
        public string UserImg { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WecharName { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime AcceptTime { get; set; }

    }
}
