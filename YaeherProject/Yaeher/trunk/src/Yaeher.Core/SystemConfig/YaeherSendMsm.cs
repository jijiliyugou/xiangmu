using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 对接阿里短信实体
    /// </summary>
    public class YaeherSendMsm:EntityBaseModule
    {
        /// <summary>
        /// 短信接收号码,支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,
        /// </summary>
        public string PhoneNumbers { get; set; }
        /// <summary>
        /// 短信签名
        /// </summary>
        public string SignName { get; set; }
        /// <summary>
        /// 短信模板ID
        /// </summary>
        public string TemplateCode { get; set; }
        /// <summary>
        /// 短信模板变量替换JSON串,
        /// </summary>
        public string TemplateParam { get; set; }
        /// <summary>
        /// 上行短信扩展码,无特殊需要此字段的用户请忽略此字段
        /// </summary>
        public string SmsUpExtendCode { get; set; }
        /// <summary>
        /// 外部流水扩展字段
        /// </summary>
        public string OutId { get; set; }

        /// <summary>
        /// 短信模板类型
        /// </summary>
        public string MessageType { get; set; }
    }
}
