using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 腾讯呼叫中心
    /// </summary>
    public class TencentCCC: EntityBaseModule
    {
        /// <summary>
        /// 应用 ID 必选
        /// </summary> 
        public string AppId { get; set; }
        /// <summary>
        /// 主叫号码 - 显示的号码 - 从腾讯云申请的号码  必选
        /// </summary>
        public string Caller { get; set; }
        /// <summary> 
        /// 被叫号码  必选
        /// </summary>
        public string Called { get; set; }
        /// <summary>
        /// 要进行透传的数据，128 个字节  可选
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 超时未接听则挂断，20 ~ 60 之间  可选
        /// </summary>
        public string Timeout { get; set; }
        /// <summary>
        /// 智能外呼标识，为1时生效，用户说话时，会打断正在播放的语音，同时，会上报用户说话通知和识别用户说话的内容  可选
        /// </summary>
        public string EnableAi { get; set; }
    }
}
