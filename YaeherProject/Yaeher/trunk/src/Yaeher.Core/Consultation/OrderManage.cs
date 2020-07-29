using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderManage:EntityBaseModule
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [MaxLength(20)]
        public string SequenceNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(20)]
        public string OrderNumber { get; set; }

        /// <summary>
        /// 咨询单号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
        [MaxLength(30)]
        public string ConsultNumber { get; set; }
        /// <summary>
        ///咨询ID
        ///YaeherPatientConsultation表ID
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 咨询类型
        /// 图文 电话 或者其他
        /// </summary>
        [MaxLength(20)]
        public string ConsultType { get; set; }
        /// <summary>
        ///咨询用户ID
        ///YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询用户
        /// YaeherUser表fullname
        /// </summary>
        [MaxLength(500)]
        public string ConsultantName { get; set; }
        /// <summary>
        ///患者ID
        ///YaeherPatientLeaguerInfo表ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        ///患者名称
        ///YaeherPatientLeaguerInfo表LeaguerName
        /// </summary>
        [MaxLength(500)]
        public string PatientName { get; set; }
        /// <summary>
        /// 医生姓名
        /// YaeherUser表ID
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// YaeherUser表fullname
        /// </summary>
        public int DoctorID { get; set; }
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
        /// 医生收款类型 
        /// </summary>
        [MaxLength(20)]
        public string ReceivablesType { get; set; }
        /// <summary>
        /// 医生收款账号
        /// </summary>
        [MaxLength(20)]
        public string ReceivablesNumber { get; set; }
        /// <summary>
        /// 产品ID
        /// 医生提供服务费用表ServiceMoneyList表ID
        /// </summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// 产品名称
        /// 医生提供服务费用表ServiceMoneyList表名称
        /// </summary>
        [MaxLength(200)]
        public string ServiceName { get; set; }
        /// <summary>
        /// 微信支付分配的商户号  
        /// </summary>
        [MaxLength(200)]
        public string  SellerMoneyID { get; set; }
        /// <summary>
        /// 交易类型    
        /// </summary>
        [MaxLength(10)]
        public string TradeType { get; set; }
      
}
}
