using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.NumericalStatement.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsultationOrderTotalIn : ListParameters<ConsultationOrderTotal>, IPagedResultRequest
    {
        /// <summary>
        /// 统计类型
        /// </summary>
        public string TotalType { get; set; }
    }
    /// <summary>
    /// 执行统计参数
    /// </summary>
    public class OrderTotalIn {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// 订单统计数
    /// </summary>
    public class ConsultationOrder {
        public string ConsultState { get; set; }
        public string ConsultNumber { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string DoctorJSON { get; set; }
        public string RefundNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime RefundTime { get; set; }
        public DateTime Completetime { get; set; }
        public string OrderCurrency { get; set; }
        public decimal OrderMoney { get; set; }
    }
}
