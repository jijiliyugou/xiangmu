using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.EventEntitys
{
    /// <summary>
    /// 订阅者实体  订阅执行
    /// </summary>
    public class Subscribetion: EntityBaseModule
    {
        /// <summary>
        /// 订阅者
        /// </summary>
        [MaxLength(100)]
        public String Subscriber { set; get; }
        /// <summary>
        /// 订阅url
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
        [MaxLength(50)]
        public String EventCode { set; get; }
        /// <summary>
        /// 事件业务ID
        /// </summary>
        [MaxLength(10)]
        public String BusinessID { set; get; }
        /// <summary>
        /// 事件业务Code
        /// </summary>
        [MaxLength(50)]
        public String BusinessCode { set; get; }
        /// <summary>
        /// 事件业务json对象
        /// </summary>
        public String BusinessJSON { set; get; }
        /// <summary>
        /// 事件业务发生时间
        /// </summary>
        public DateTime PublishedTime { set; get; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public bool ExecuteStatus { set; get; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? ExecuteDate { set; get; }
    }
}
