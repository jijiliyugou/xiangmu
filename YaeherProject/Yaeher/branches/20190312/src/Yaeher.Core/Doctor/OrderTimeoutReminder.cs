using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 订单超时提醒记录_医生
    /// </summary>
    public class OrderTimeoutReminder : EntityBaseModule
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
        /// 订单id
        /// </summary>
        public virtual int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(20)]
        public virtual string OrderNumber { get; set; }
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
        /// 患者ID
        /// </summary>
        public virtual int PatientID { get; set; }
        /// <summary>
        /// 患者名称
        /// </summary>
        [MaxLength(500)]
        public virtual string PatientName { get; set; }
        /// <summary>
        /// 咨询医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 超时提醒内容
        /// </summary>
        [MaxLength(1000)]
        public virtual string RemindDescription { get; set; }

    }
}
