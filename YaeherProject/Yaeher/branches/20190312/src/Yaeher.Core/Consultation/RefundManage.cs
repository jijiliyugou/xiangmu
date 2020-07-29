using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 订单退单管理
    /// </summary>
    public class RefundManage : EntityBaseModule
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [MaxLength(20)]
        public string SequenceNo { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>
        [MaxLength(20)]
        public string  RefundNumber { get; set; }
        /// <summary>
        /// 咨询ID
        /// YaeherPatientConsultation表ID
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 咨询单号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
        [MaxLength(30)]
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 订单id
        /// YaeherPatientConsultation表ID
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(20)]
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
        [MaxLength(500)]
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
        [MaxLength(500)]
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
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        [MaxLength(10)]
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
        [MaxLength(200)]
        public string ServiceName { get; set; }
        /// <summary>
        /// 微信支付商户号
        /// </summary>
        [MaxLength(200)]
        public string SellerMoneyID { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [MaxLength(100)]
        public string CheckState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(1000)]
        public string CheckRemark { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime { get; set; }
        /// <summary>
        /// 审核人 userid
        /// </summary>
        public int Checker { get; set; }
        /// <summary>
        /// 退单内容
        /// </summary>
        [MaxLength(1000)]
        public string RefundRemarks { get; set; }
        /// <summary>
        /// 退单原因json
        /// </summary>
        [MaxLength(1000)]
        public string RefundReason { get; set; }
    }
}
