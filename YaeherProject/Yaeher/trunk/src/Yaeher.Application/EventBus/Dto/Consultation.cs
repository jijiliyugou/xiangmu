using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.EventBus.Dto
{
    /// <summary>
    /// 新增咨询
    /// </summary>
    public class Consultation
    {
        /// <summary>
        ///  咨询表
        /// </summary>
        public YaeherConsultation yaeherConsultation { get; set; }
        /// <summary>
        /// 订单表
        /// </summary>
        public OrderManage  orderManage { get; set; }
        /// <summary>
        /// 订单交易记录
        /// </summary>
        public OrderTradeRecord orderTradeRecords { get; set; }
        /// <summary>
        /// 退单表
        /// </summary>
        public RefundManage refundManage { get; set; }
        /// <summary>
        /// 咨询评分
        /// </summary>
        public ConsultationEvaluation consultationEvaluation { get; set; }
        /// <summary>
        /// 回复 回答 追问
        /// </summary>
        public ConsultationReply consultationReply { get; set; }
        /// <summary>
        /// 电话回复
        /// </summary>
        public PhoneReplyRecord phoneReplyRecord { get; set; }
    }
}
