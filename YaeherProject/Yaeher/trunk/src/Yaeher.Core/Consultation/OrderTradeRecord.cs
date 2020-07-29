using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 订单交易记录
    /// </summary>
    public class OrderTradeRecord : EntityBaseModule
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [MaxLength(20)]
        public string SequenceNo { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(20)]
        public string OrderNumber { get; set; }
        /// <summary>
        /// 支付类型  1现金 银行卡 支付宝 微信支付 代金券 积分抵扣 等
        /// </summary>
        [MaxLength(20)]
        public string PayType { get; set; }
        /// <summary>
        /// 支付币别
        /// </summary>
        [MaxLength(10)]
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 支付账号
        /// </summary>
        [MaxLength(50)]
        public string TenpayNumber { get; set; }
        /// <summary>
        /// 代金券编号
        /// </summary>
        [MaxLength(50)]
        public string VoucherNumber { get; set; }
        /// <summary>
        /// 代金券JSON
        /// </summary>
        public string VoucherJSON { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        ///支付完成时间 其他第三方返回时间
        /// </summary>
        public DateTime PayAchiveTime { get; set; }
        /// <summary>
        /// 支付流水号 其他第三方返回单号
        /// </summary>
        [MaxLength(100)]
        public string PaySerialNumber { get; set; }
        /// <summary>
        /// 支付状态 paid 已支付，unpaid 未支付
        /// </summary>
        [MaxLength(20)]
        public string PaymentState { get; set; }
        /// <summary>
        /// 单据来源(订单，退单)
        /// </summary>
        [MaxLength(20)]
        public string PaymentSource { get; set; }
        /// <summary>
        /// 单据来源(订单code，退单code)
        /// </summary>
        [MaxLength(20)]
        public string PaymentSourceCode { get; set; }
        ///微信支付商户订单号(供查询订单状态信息和退单用)
        [MaxLength(200)]
        public string WXPayBillno { get; set; }
        /// <summary>
        /// 微信支付订单号	
        /// </summary>
        [MaxLength(200)]
        public string WXTransactionId { get; set; }
        /// <summary>
        /// 微信订单信息
        /// </summary>
        [MaxLength(5000)]
        public string WXOrderQuery { get; set; }

    }
}
