using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.NumericalStatement
{
    /// <summary>
    /// 订单汇总表 
    /// </summary>
    public class ConsultationOrderTotal : EntityBaseModule
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
        public String DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public String DoctorJSON { get; set; }
        /// <summary>
        /// 订单总数
        /// </summary>
        public double OrderTotal { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public double RevenueTotal { get; set; }
        /// <summary>
        /// 统计时间类型 日、月、年
        /// </summary>
        [MaxLength(10)]
        public String TotalType { get; set; }

        /// <summary>
        /// 退单数
        /// </summary>
        public int RefundTotal { get; set; }
        /// <summary>
        /// 完成数
        /// </summary>
        public int CompleteTotal { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime TotalDate { get; set; }
        /// <summary>
        /// 退单金额
        /// </summary>
        public double RefundMoney { get; set; }
        /// <summary>
        /// 完成单金额
        /// </summary>
        public double CompleteMoney { get; set; }

    }
}
