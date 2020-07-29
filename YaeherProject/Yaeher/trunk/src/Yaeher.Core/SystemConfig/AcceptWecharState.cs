using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 客服处理咨询问答回复状态
    /// </summary>
    public class AcceptWecharState:EntityBaseModule
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
        [MaxLength(500)]
        public string ConsultantName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [MaxLength(500)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户图像
        /// </summary>
        [MaxLength(500)]
        public string UserImage { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 咨询人微信OpenID
        /// </summary>
        [MaxLength(100)]
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
        [MaxLength(50)]
        public string CustomerServiceName { get; set; }
        /// <summary>
        /// 客服用户JSON
        /// </summary>
        public string CustomerServiceJson { get; set; }
        /// <summary>
        /// 处理状态 2（开启） 3（关闭） 1（待办）
        /// </summary>
        [MaxLength(5)]
        public string AcceptState { get; set; }
        /// <summary>
        /// 读取状态
        /// </summary>
        public bool IsReady { get; set; }
        /// <summary>
        /// 系统接受消息时间
        /// </summary>
        public DateTime AcceptTime { get; set; }
    }
}
