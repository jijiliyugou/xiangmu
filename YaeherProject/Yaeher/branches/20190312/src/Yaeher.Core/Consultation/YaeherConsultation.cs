using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 咨询管理
    /// </summary>
    public class YaeherConsultation : EntityBaseModule
    {
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(30)]
        [Description("咨询单号")]
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
        [MaxLength(500)]
        public string ConsultantName { get; set; }
        /// <summary>
        /// 咨询用户JSON
        /// </summary>
        public string ConsultantJSON { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
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
        [MaxLength(500)]
        public string PatientName { get; set; }
        /// <summary>
        /// 患者JSON
        /// </summary>
        public string PatientJSON { get; set; }
        /// <summary>
        /// 咨询类型
        /// 图文 电话 或者其他
        /// </summary>
        [MaxLength(20)]
        public string ConsultType { get; set; }
        /// <summary>
        /// 疾病类型
        /// </summary>
        [MaxLength(20)]
        public string IIInessType { get; set; }
        /// <summary>
        /// 疾病类型JSON
        /// </summary>
        public string IIInessJSON { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        [MaxLength(200)]
        public string PatientCity { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        [MaxLength(6000)]
        public string IIInessDescription { get; set; }
        /// <summary>
        /// 最大追问次数
        /// </summary>
        public int InquiryTimes { get; set; }
     
        /// <summary>
        /// 咨询问诊状态,已创建，咨询中,已退单,已完成
        /// </summary>
        [MaxLength(20)]
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
        [MaxLength(20)]
        public string RefundType { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>
        [MaxLength(20)]
        public string RefundNumber { get; set; }
        /// <summary>
        /// 退单状态
        /// </summary>
        [MaxLength(20)]
        public string RefundState { get; set; }
        /// <summary>
        /// 退单理由
        /// 理由选择
        /// </summary>
        [MaxLength(1000)]
        public string RefundReason { get; set; }
        /// <summary>
        /// 退单原因 手填
        /// </summary>
        [MaxLength(1000)]
        public string RefundRemarks { get; set; }
        /// <summary>
        /// 推荐医生
        /// </summary>
        public int RecommendDoctorID { get; set; }
        /// <summary>
        /// 推荐医生姓名
        /// YaeherUser表name  医生name
        /// </summary>
        [MaxLength(20)]
        public string RecommendDoctorName { get; set; }
        /// <summary>
        /// 是否已回复
        /// </summary>
        public bool HasReply { get; set; }
        /// <summary>
        /// 患者年龄
        /// </summary>
        [MaxLength(20)]
        public string Age { get; set; }
        /// <summary>
        /// 医生提供服务费用表Id
        /// </summary>
        public int ServiceMoneyListId { get; set;}
        /// <summary>
        /// 剩余追问次数
        /// </summary>
        public int HasInquiryTimes { get; set; }
        
        /// <summary>
        /// 是否已回访
        /// </summary>
        public bool IsReturnVisit { get; set; }

        /// <summary>
        /// 是否评价
        /// </summary>
        public bool IsEvaluate { get; set; }

        /// <summary>
        /// 回访内容
        /// </summary>
        [MaxLength(1000)]
        public string ReturnVisit { get; set; }
        /// <summary>
        /// 回访时间
        /// </summary>
        public DateTime ReturnVisitTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime Completetime { get; set; }

    }
}
