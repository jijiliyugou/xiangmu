using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 快捷回复
    /// </summary>
    public class QuickReply: EntityBaseModule
    {
        /// <summary>
        /// 用途
        /// </summary>
        [MaxLength(100)]
        public virtual string UseOf { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(100)]
        public virtual string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(5000)]
        public virtual string Content { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
    }
}
