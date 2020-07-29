using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 腾讯短信对接实体
    /// </summary>
    public class TencentMessage : EntityBaseModule
    {
        /// <summary>
        /// 支持发送的区号
        /// </summary>
        public string NationCode { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 模板 ID，在控制台审核通过的模板 ID  在短信正文中的模板Id
        /// </summary>
        public int TemplateId { get; set; }
        /// <summary>
        /// 模板参数，若模板没有参数，请提供为空数组,
        /// </summary>
        public string[] Parameters { get; set; }
        /// <summary>
        /// 短信签名，如果使用默认签名，该字段可缺省
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 短信码号扩展号，格式为纯数字串，其他格式无效。默认没有开通，开通请联系 腾讯云短信技术支持
        /// </summary>
        public string Extend { get; set; }
        /// <summary>
        /// 用户的 session 内容，腾讯 server 回包中会原样返回，可选字段，不需要就填空
        /// </summary>
        public string Ext { get; set; }
    }
}
