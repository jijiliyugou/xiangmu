using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Yaeher
{
    /// <summary>
    /// 咨询管理
    /// </summary>
    public class ConsultationIn : ListParameters<YaeherConsultation>, IPagedResultRequest
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int ServiceMoneyListId { get; set; }
        /// <summary>
        /// 创建订单userid
        /// </summary>
        public int CreatedBy { get; set;}
        /// <summary>
        /// 咨询单号
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询用户ID
        /// YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询姓名
        /// YaeherUser表fullname
        /// </summary>
        public string ConsultantName { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 咨询医生JSON
        /// </summary>
        public string DoctorJSON { get; set; }
        /// <summary>
        /// 患者ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 患者JSON
        /// </summary>
        public string PatientJSON { get; set; }
        /// <summary>
        /// 咨询类型
        /// 图文 电话 或者其他
        /// </summary>
        public string ConsultType { get; set; }
        /// <summary>
        /// 疾病类型
        /// </summary>
        public string IIInessType { get; set; }
        /// <summary>
        /// 疾病类型JSON
        /// </summary>
        public string IIInessJSON { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string PatientCity { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string IIInessDescription { get; set; }
        /// <summary>
        /// 最大追问次数
        /// </summary>
        public int InquiryTimes { get; set; }
        /// <summary>
        /// 咨询问诊状态
        /// </summary>
        public string ConsultState { get; set; }
        /// <summary>
        /// 咨询超时提醒次数
        /// </summary>
        public int OvertimeRemindTimes { get; set; }
        /// <summary>
        /// 咨询退单超时时间
        /// </summary>
        public DateTime Overtime { get; set; }
        /// <summary>
        /// 退单人
        /// </summary>
        public int RefundBy { get; set; }
        /// <summary>
        /// 退单时间
        /// </summary>
        public DateTime RefundTime { get; set; }
        /// <summary>
        /// 退单类型
        /// 来源 患者 医生 系统
        /// </summary>
        public string RefundType { get; set; }
        /// <summary>
        /// 退单类型
        /// 来源 患者 医生 系统
        /// </summary>
        public string RefundNumber { get; set; }
        /// <summary>
        /// 退单状态
        /// </summary>
        public string RefundState { get; set; }
        /// <summary>
        /// 退单理由
        /// 理由选择
        /// </summary>
        public string RefundReason { get; set; }
        /// <summary>
        /// 退单原因
        /// </summary>
        public string RefundRemarks { get; set; }
        /// <summary>
        /// 推荐医生
        /// </summary>
        public int RecommendDoctorID { get; set; }
        /// <summary>
        /// 推荐医生姓名
        /// YaeherUser表name  医生name
        /// </summary>
        public string RecommendDoctorName { get; set; }
        /// <summary>
        /// 是否已回复 2，已回复，1未回复，3默认所有
        /// </summary>
        public int HasReply { get; set; }
        /// <summary>
        /// 质控退单
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 咨询评分
        /// </summary>
        public int EvaluateLevel { get; set; }

    }
    /// <summary>
    /// 创建咨询
    /// </summary>
    public class YaeherConsultationAdd : YaeherConsultation
    {
        /// <summary>
        /// 疾病类型Id
        /// </summary>
        public int IIInessId { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentIn> Attach { get; set; }
        /// <summary>
        /// 微信支付商户订单号
        /// </summary>
        public string sp_billno { get; set; }
    }
}