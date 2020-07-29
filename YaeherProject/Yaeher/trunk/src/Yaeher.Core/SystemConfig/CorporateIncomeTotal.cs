using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 公司收入统计表
    /// </summary>
    public class CorporateIncomeTotal : EntityBaseModule
    {
        /// <summary>
        /// 收入时间类型  年 月 季 天
        /// </summary>
        [MaxLength(20)]
        public virtual string IncomeType { get; set; }
        /// <summary>
        /// 统计金额，公司进账
        /// </summary>
        public virtual decimal IncomeTotal { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public virtual decimal OrderTotalMoney { get; set; }
        /// <summary>
        /// 退单总金额
        /// </summary>
        public virtual decimal RefundTotalMoney { get; set; }
        /// <summary>
        /// 订单数（分账的订单数）
        /// </summary>
        public virtual int OrderTotal { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime TotalDate { get; set; }
    }
}
