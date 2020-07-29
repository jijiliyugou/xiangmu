using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 用户支付表
    /// </summary>
    public class YaeherUserPaymentIn : ListParameters<YaeherUserPayment>, IPagedResultRequest
    {
        /// <summary>
        /// UserID
        /// </summary>
        public virtual int UserID { get; set; }
        /// <summary>
        /// 用户全称
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public virtual string PayMethod { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public virtual string PayMethodName { get; set; }
        /// <summary>
        /// 支付账号
        /// </summary>
        public virtual string PaymentAccout { get; set; }
        /// <summary>
        /// 开户行名称
        /// </summary>
        public virtual string BankName { get; set; }
        /// <summary>
        /// 开户行支行
        /// </summary>
        public virtual string Subbranch { get; set; }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public virtual string BandAdd { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public virtual string BankNo { get; set; }
        /// <summary>
        /// 是否默认账号 
        /// </summary>
        public virtual bool IsDefault { get; set; }
    }
}
