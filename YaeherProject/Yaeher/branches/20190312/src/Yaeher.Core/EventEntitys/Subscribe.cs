using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.EventEntitys
{
    /// <summary>
    /// 订阅者
    /// </summary>
    public class Subscribe: EntityBaseModule
    {
        /// <summary>
        /// 订阅者
        /// </summary>
        [MaxLength(100)]
        public String Subscriber { set; get; }
        /// <summary>
        /// 订阅者url/事件处理url
        /// </summary>
        [MaxLength(500)]
        public String CallbackUrl { get; set; }
        /// <summary>
        /// 订阅事件名称
        /// </summary>
        [MaxLength(100)]
        public String EventName { get; set; }
        /// <summary>
        /// 订阅事件
        /// </summary>
        [MaxLength(100)]
        public String EventCode { set; get; }
        /// <summary>
        /// 订阅时间
        /// </summary>
        public DateTime RegisterTime { set; get; }
        /// <summary>
        /// 订阅状态 0 /1 未订阅/订阅
        /// </summary>
        public bool ActionStatus { set; get; }
        /// <summary>
        /// 来源 Server Client
        /// </summary>
        [MaxLength(10)]
        public string ServerClient { get; set; }
    }
}
