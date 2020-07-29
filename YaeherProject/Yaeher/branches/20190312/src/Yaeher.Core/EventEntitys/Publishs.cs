using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.EventEntitys
{
    /// <summary>
    /// 
    /// </summary>
    public class Publishs: EntityBaseModule
    {
        /// <summary>
        /// 发布模板
        /// </summary>
        [MaxLength(200)]
        public String TemplateCode { set; get; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [MaxLength(200)]
        public String OperationType { set; get; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [MaxLength(5000)]
        public String MessageRemark { set; get; }
        /// <summary>
        /// 发布者名称  Site+名称
        /// </summary>
        [MaxLength(200)]
        public String Publisher { set; get; }
        /// <summary>
        /// 发布者url
        /// </summary>
        [MaxLength(200)]
        public String PublishUrl { get; set; }
        /// <summary>
        /// 发布事件名称
        /// </summary>
        [MaxLength(200)]
        public String EventName { set; get; }
        /// <summary>
        /// 发布事件编号
        /// </summary>
        [MaxLength(200)]
        public String EventCode { set; get; }
        /// <summary>
        /// 事件对应的业务ID
        /// </summary>
        [MaxLength(20)]
        public String BusinessID { set; get; }
        /// <summary>
        /// 事件对应的业务编号
        /// </summary>
        [MaxLength(20)]
        public String BusinessCode { set; get; }
        /// <summary>
        /// 事件json 对象(发布实体类的json格式)
        /// </summary>
        public String BusinessJSON { set; get; }
        /// <summary>
        /// 业务发生的时间
        /// </summary>
        public DateTime PublishedTime { set; get; }
        /// <summary>
        /// 发送的状态
        /// </summary>
        public bool PublishStatus { set; get; }
        /// <summary>
        /// 来源 Server Client
        /// </summary>
        [MaxLength(20)]
        public string ServerClient { get; set; }
    }
}
