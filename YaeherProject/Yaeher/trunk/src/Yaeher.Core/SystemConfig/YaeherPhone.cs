using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 拨打电话记录
    /// </summary>
    public class YaeherPhone: EntityBaseModule
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [MaxLength(20)]
        public virtual string UserName { get; set; }
        /// <summary>
        /// 主叫电话
        /// </summary>
        [MaxLength(20)]
        public virtual string Caller { get; set; }
        /// <summary>
        /// 被叫电话
        /// </summary>
        [MaxLength(20)]
        public virtual string Callee { get; set; }
        /// <summary>
        /// 呼叫中心电话 预留扩展
        /// </summary>
        [MaxLength(20)]
        public virtual string CallCenterNumber { get; set; }
        /// <summary>
        /// 呼叫状态
        /// </summary>
        [MaxLength(2000)]
        public virtual string StatusCode { get; set; }
    }
}
