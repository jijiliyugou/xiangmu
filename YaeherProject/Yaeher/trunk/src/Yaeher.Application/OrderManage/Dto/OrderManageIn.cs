using Abp.Application.Services.Dto;


namespace Yaeher
{
    /// <summary>
    /// 根据name搜索
    /// </summary>
    public class OrderManageIn : ListParameters<OrderManage>, IPagedResultRequest
    {

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 咨询单号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
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
        public string PatientName { get; set; }
        /// <summary>
        /// 医生姓名
        /// YaeherUser表ID
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// YaeherUser表fullname
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal OrderMoney { get; set; }
        /// <summary>
        /// 医生收款类型 
        /// </summary>
        public string ReceivablesType { get; set; }
        /// <summary>
        /// 医生收款账号
        /// </summary>
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
        public string ServiceName { get; set; }
        /// <summary>
        /// 微信支付分配的商户号  
        /// </summary>
        public string SellerMoneyID { get; set; }
        /// <summary>
        /// 交易类型    
        /// </summary>
        public string TradeType { get; set; }
        /// <summary>
        /// 验证来自统计的请求
        /// </summary>
        public string TotalType { get; set; }
    }
   
}