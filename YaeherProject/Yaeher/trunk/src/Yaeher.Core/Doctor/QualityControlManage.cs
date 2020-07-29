using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 质控委员处理
    /// </summary>
    public class QualityControlManage : EntityBaseModule
    {
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(30)]
        public virtual string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询ID
        /// </summary>
        public virtual int ConsultID { get; set; }
        /// <summary>
        /// 咨询用户ID
        /// </summary>
        public virtual int ConsultantID { get; set; }
        /// <summary>
        /// 咨询用户
        /// </summary>
        [MaxLength(500)]
        public virtual string ConsultantName { get; set; }
        /// <summary>
        /// 咨询医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 咨询类型  图文 电话 或者其他
        /// </summary>
        [MaxLength(20)]
        public virtual string ConsultType { get; set; }
        /// <summary>
        /// 质控类型  来源评分 来源投诉
        /// </summary>
        [MaxLength(20)]
        public virtual string QualityType { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public virtual int QualityLevel { get; set; }
        /// <summary>
        /// 处理描述
        /// </summary>
        [MaxLength(5000)]
        public virtual string RepayIllnessDescription { get; set; }
        /// <summary>
        /// 处理状态  未处理  处理  退回
        /// </summary>
        [MaxLength(20)]
        public virtual string ReplyState { get; set; }
    }
}
