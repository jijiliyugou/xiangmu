using Abp.Application.Services.Dto;
using System;

namespace Yaeher
{
    /// <summary>
    /// 根据name搜索
    /// </summary>
    public class OrderTradeRecordIn : ListParameters<OrderTradeRecord>, IPagedResultRequest
    {
        /// <summary>
        /// 医生服务类型id
        /// </summary>
        public int ServiceID{get;set;}
        /// <summary>
        /// 医生Id
        /// </summary>
        public int DoctorId { get; set;}
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 支付类型  1现金 银行卡 支付宝 微信支付 代金券 积分抵扣 等
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 支付币别
        /// </summary>
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 支付账号
        /// </summary>
        public string TenpayNumber { get; set; }
        /// <summary>
        /// 代金券编号
        /// </summary>
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
        public string PaySerialNumber { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public string PaymentState { get; set; }
    }
  
}