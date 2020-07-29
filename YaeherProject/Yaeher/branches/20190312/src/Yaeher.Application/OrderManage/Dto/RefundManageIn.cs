using Abp.Application.Services.Dto;
using System;

namespace Yaeher
{
    /// <summary>
    /// 根据name搜索
    /// </summary>
    public class RefundManageIn : ListParameters<RefundManage>, IPagedResultRequest
    {
        /// <summary>
        /// 退单类型 doctorreturn,patientreturn,qualityreturn
        /// </summary>
        public string RefundType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CheckState { get; set; }
        /// <summary>
        /// 审核
        /// </summary>
        public string CheckRemark { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>
        public string RefundNumber { get; set; }
        /// <summary>
        /// 咨询ID
        /// YaeherPatientConsultation表ID
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 咨询单号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 订单id
        /// YaeherPatientConsultation表ID
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 咨询用户ID
        /// YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询用户
        ///YaeherUser表fullname
        /// </summary>
        public string ConsultantName { get; set; }
        /// <summary>
        /// 患者ID
        /// YaeherPatientLeaguerInfo表ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        /// 患者名称
        ///YaeherPatientLeaguerInfo表LeaguerName
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 咨询医生ID
        /// YaeherUser表ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        ///YaeherUser表fullname
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal OrderMoney { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ServiceID { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 微信支付商户号
        /// </summary>
        public string SellerMoneyID { get; set; }
        
    }
    /// <summary>
    /// 患者退单
    /// </summary>
    public class RefundManagePatientInfo : RefundManageIn
    {
        /// <summary>
        /// 退单理由
        /// </summary>
        public string RefundReason { get; set; }
        /// <summary>
        /// 标签id
        /// </summary>
        public int LabelId { get; set; }
        /// <summary>
        /// 退单原因
        /// </summary>
        public string RefundRemarks { get; set; }
        /// <summary>
        /// 退单理由Code
        /// </summary>
        public string RefundManageCode { get; set; }

    }
    /// <summary>
    /// 患者退单
    /// </summary>
    public class RefundManageDoctorInfo : RefundManageIn
    {
        /// <summary>
        /// 退单理由Code
        /// </summary>
        public string RefundManageCode { get; set; }
        /// <summary>
        /// 标签id
        /// </summary>
        public int LabelId { get; set; }
        /// <summary>
        /// 退单理由
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
        /// </summary>
        public string RecommendDoctorName { get; set; }
    }
}