using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 订单收入分配_医生
    /// </summary>
    public class IncomeDevide : EntityBaseModule
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
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 咨询医生
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(20)]
        public virtual string OrderNumber { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public virtual decimal OrderMoney { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        [MaxLength(20)]
        public virtual string OrderCurrency { get; set; }
        /// <summary>
        /// 订单分成金额(医生)
        /// </summary>
        public virtual decimal DevideMoney { get; set; }
        /// <summary>
        /// 订单分成比例(医生)
        /// </summary>
        public virtual double DevideRatio { get; set; }
        /// <summary>
        /// 订单分成时间(医生)
        /// </summary>
        public virtual DateTime DevideTime { get; set; }
        /// <summary>
        /// 微信分成状态
        /// </summary>
        [MaxLength(20)]
        public string WXSharing { get; set; }
        /// <summary>
        /// 微信分成结果
        /// </summary>
        public string WXSharingJson { get; set;}
    }
}
