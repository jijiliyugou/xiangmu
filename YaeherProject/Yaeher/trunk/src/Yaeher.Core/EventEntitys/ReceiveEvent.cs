using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.EventEntitys
{
    /// <summary>
    /// 处理记录
    /// </summary>
    public class ReceiveEvent: EntityBaseModule
    {
        /// <summary>
        /// 发布的消息id
        /// </summary>
        public int PublishId { get; set; }
        /// <summary>
        /// 接受者ID/// 订阅者ID
        /// </summary>
        public int SubscribeID { get; set; }
        /// <summary>
        /// 接收结果
        /// </summary>
        [MaxLength(1000)]
        public String ReceptionMessage { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime ReceptionDate { get; set; }
    }
}
