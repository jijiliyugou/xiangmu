using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Common.SystemParameter
{
    /// <summary>
    /// 配置Code说明
    /// </summary>
    public class ParameterList
    {

        #region  配置参数
        /// <summary>
        /// 文章操作
        /// </summary>
        public string Articleoper = "articleoper";
        /// <summary>
        /// 咨询单状态
        /// </summary>
        public string ConsultState = "ConsultState";
        /// <summary>
        /// 咨询回答类型
        /// </summary>
        public string ConsultationReplyType = "ConsultationReplyType";
        /// <summary>
        /// 咨询回答状态
        /// </summary>
        public string ConsultationReplyState = "ConsultationReplyState";
        /// <summary>
        /// 医生退单类型
        /// </summary>
        public string DoctorRefundManageType = "DoctorRefundManageType";
        /// <summary>
        /// 交易记录单据来源
        /// </summary>
        public string OrderTradeRecordPaymentSource = "OrderTradeRecordPaymentSource";
        /// <summary>
        /// 支付类型
        /// </summary>
        public string OrderTradeRecordPayType = "OrderTradeRecordPayType";
        /// <summary>
        /// 支付币别
        /// </summary>
        public string OrderCurrency = "OrderCurrency";
        /// <summary>
        /// 支付状态
        /// </summary>
        public string PaymentState = "PaymentState";
        /// <summary>
        /// 医生提供服务类型
        /// </summary>
        public string ServiceMoneyListType = "ServiceMoneyListType";
        /// <summary>
        /// 评价大标题
        /// </summary>
        public string ConsultationEvaluationReason = "ConsultationEvaluationReason";
        /// <summary>
        /// 评价标签
        /// </summary>
        public string ConsultationEvaluationDetail = "ConsultationEvaluationDetail";
        /// <summary>
        /// 患者退单类型
        /// </summary>
        public string PatientRefundManageType = "PatientRefundManageType";
        /// <summary>
        /// 咨询超时提醒时间
        /// </summary>
        public string ConsultationOverTimeReimd = "ConsultationOverTimeReimd";
        /// <summary>
        /// 自签名有效时间(分钟)
        /// </summary>
        public string timestampexpires = "timestampexpires";
        /// <summary>
        /// 医生审核状态
        /// </summary>
        public string DoctorCheckRes = "DoctorCheckRes";
        /// <summary>
        /// 医生认证状态
        /// </summary>
        public string DoctorAuthCheckType = "DoctorAuthCheckType";
        #endregion

        #region 参数类型
        /// <summary>
        /// 患者退单时效(分钟)
        /// </summary>
        public string ReplyMinutesTime = "ReplyMinutesTime";
        /// <summary>
        /// 咨询完成时间(分钟)
        /// </summary>
        public string ConsultationSucessTime = "ConsultationSucessTime";
        /// <summary>
        /// 咨询最小完成时间
        /// </summary>
        public string EvaluationLastTime = "EvaluationLastTime";
        /// <summary>
        /// 评价标签
        /// </summary>
        public string EvaluationTipsTime = "EvaluationTipsTime";
        /// <summary>
        /// 系统放号时间
        /// </summary>
        public string SystemReleaseTime = "SystemReleaseTime";
        /// <summary>
        /// 系统时退款时限
        /// </summary>
        public string SystemOverTime = "SystemOverTime";
        /// <summary>
        /// 下单编辑可内容(分钟)
        /// </summary>
        public string ConsultationCanEditTime = "ConsultationCanEditTime";
        /// <summary>
        /// 撤销追问时间
        /// </summary>
        public string RevokeReplyTime = "RevokeReplyTime";
        /// <summary>
        /// 医生收款(分钟)
        /// </summary>
        public string DoctorReceivablesTime = "DoctorReceivablesTime";
        /// <summary>
        /// 新医生多久不纳入评分统计
        /// </summary>
        public string DoctorNotCountTime = "DoctorNotCountTime";
        /// <summary>
        /// 新医生判断咨询单数
        /// </summary>
        public string newdoctor = "newdoctor";
        /// <summary>
        /// 医生分成比例
        /// </summary>
        public string DoctorPayProportions = "DoctorPayProportions";
        /// <summary>
        /// 新医生分成比例
        /// </summary>
        public string NewDoctorPayProportions = "NewDoctorPayProportions";
        /// <summary>
        /// 医生资费调价幅度
        /// </summary>
        public string DoctorMoneyExchange = "DoctorMoneyExchange";
        /// <summary>
        /// 医生资费调价时间
        /// </summary>
        public string DoctorMoneyexTime = "DoctorMoneyexTime";
        /// <summary>
        /// 咨询上传图片张数
        /// </summary>
        public string ConsultationImageCount = "ConsultationImageCount";
        /// <summary>
        /// 咨询上传图片大小
        /// </summary>
        public string ConsultationImagesize = "ConsultationImagesize";
        /// <summary>
        /// 咨询内容字数
        /// </summary>
        public string ConsultationContentLength = "ConsultationContentLength";
        /// <summary>
        /// 咨询追问字数
        /// </summary>
        public string ConsultationReplyLength = "ConsultationReplyLength";
        /// <summary>
        /// 咨询视频大小
        /// </summary>
        public string VideoCount = "VideoCount";
        /// <summary>
        /// 咨询视频条数
        /// </summary>
        public string VideoSize = "VideoSize";
        #endregion
    }
}
