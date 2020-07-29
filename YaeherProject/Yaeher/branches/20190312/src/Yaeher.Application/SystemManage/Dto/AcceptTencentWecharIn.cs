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
    public class AcceptTencentWecharIn : ListParameters<AcceptTencentWechar>, IPagedResultRequest
    {
        /// <summary>
        /// 用户全称
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public virtual string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual string MsgType { get; set; }
        /// <summary>
        /// 接受者
        /// </summary>
        public virtual string ToUser { get; set; }
        /// <summary>
        /// 发送文本
        /// </summary>
        public virtual string TextContent { get; set; }
    }
}
