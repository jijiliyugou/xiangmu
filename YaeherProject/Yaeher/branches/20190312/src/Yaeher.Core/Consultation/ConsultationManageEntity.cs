using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Consultation
{
    /// <summary>
    /// 咨询实体
    /// </summary>
    public class ConsultationManageEntity
    {
        /// <summary>
        /// 咨询主表
        /// </summary>
        public YaeherConsultation ConsultationInfo { get; set; }
        /// <summary>
        /// 咨询文件附件地址
        /// </summary>
        public List<AttachmentService> ConsultationFileList { get; set; }
        /// <summary>
        /// 回答 追问
        /// </summary>
        public List<ConsultationReplyInfo> ReplyList { get; set; }
        /// <summary>
        /// 电话回复
        /// </summary>
        public List<PhoneReplyRecord> PhoneReplyList { get; set; }
        /// <summary>
        /// 订单评分
        /// </summary>
        public List<ConsultationEvaluation> EvaluationList { get; set; }
        /// <summary>
        /// 订单管理
        /// </summary>
        public List<OrderManage> orderManageList { get; set; }
        /// <summary>
        /// 订单交易记录
        /// </summary>
        public List<OrderTradeRecord> OrderTradeList { get; set; }
        /// <summary>
        /// 订单退单记录
        /// </summary>
        public List<RefundManage> refundManageList { get; set; }
    }

    /// <summary>
    /// 回答 追问
    /// </summary>
    public class ConsultationReplyInfo {

        /// <summary>
        /// 回答 追问
        /// </summary>
        public ConsultationReply  ReplyInfo { get; set; }
        /// <summary>
        /// 回答 追问附件
        /// </summary>
        public List<AttachmentService> ReplyAttachmentFile { get; set; }

    }
}
