using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.MessageRemind.Dto
{
    /// <summary>
    /// 电话
    /// </summary>
    public class YaeherPhoneIn : ListParameters<YaeherPhone>, IPagedResultRequest
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 主叫
        /// </summary>
        public virtual string Caller { get; set; }
        /// <summary>
        /// 被叫
        /// </summary>
        public virtual string Callee { get; set; }
    }
    /// <summary>
    /// 呼叫中心
    /// </summary>
    public class YaeherPhoneInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 主叫电话
        /// </summary>
        public virtual string Caller { get; set; }
        /// <summary>
        /// 被叫电话
        /// </summary>
        public virtual string Callee { get; set; }
        /// <summary>
        /// 呼叫中心电话 预留扩展
        /// </summary>
        public virtual string CallCenterNumber { get; set; }
        /// <summary>
        /// 呼叫状态
        /// </summary>
        public virtual string StatusCode { get; set; }

    }

}
