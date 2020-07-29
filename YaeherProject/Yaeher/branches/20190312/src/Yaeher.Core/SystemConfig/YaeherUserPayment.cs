using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 用户支付表
    /// </summary>
    public class YaeherUserPayment: EntityBaseModule
    {
        /// <summary>
        /// UserID
        /// </summary>
        public virtual int UserID { get; set; }
        /// <summary>
        /// 用户全称
        /// </summary>
        [MaxLength(500)]
        public virtual string FullName { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [MaxLength(10)]
        public virtual string PayMethod  { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        [MaxLength(20)]
        public virtual string PayMethodName { get; set; }
        /// <summary>
        /// 支付账号
        /// </summary>
        [MaxLength(50)]
        public virtual string PaymentAccout { get; set; }
        /// <summary>
        /// 开户行名称
        /// </summary>
        [MaxLength(100)]
        public virtual string BankName { get; set; }
        /// <summary>
        /// 开户行支行
        /// </summary>
        [MaxLength(100)]
        public virtual string Subbranch { get; set; }
        /// <summary>
        /// 开户行地址
        /// </summary>
        [MaxLength(300)]
        public virtual string BandAdd { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [MaxLength(30)]
        public virtual string BankNo { get; set; }
        /// <summary>
        /// 是否默认账号 
        /// </summary>
        public virtual bool IsDefault { get; set; }
    }
}
