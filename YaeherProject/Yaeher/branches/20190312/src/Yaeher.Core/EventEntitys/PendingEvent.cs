using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.EventEntitys
{
    /// <summary>
    /// 待办事件： 比如 a b 订阅了 c ，c发送了一条消息，则待办就有两条数据
    /// 可以看作是Publish 发布者和  Subscribe 订阅者的合集
    /// </summary>
    public class PendingEvent : Publishs
    {
        /// <summary>
        /// 订阅者ID
        /// </summary>
        public int SubscribeID { get; set; }
        /// <summary>
        /// 订阅者
        /// </summary>
        public String Subscriber { set; get; }
        /// <summary>
        /// 订阅者url
        /// </summary>
        public String CallbackUrl { get; set; }
        /// <summary>
        /// 订阅时间
        /// </summary>
        public DateTime RegisterTime { set; get; }
        /// <summary>
        /// 订阅状态
        /// </summary>
        public bool ActionStatus { set; get; }
    }
}
