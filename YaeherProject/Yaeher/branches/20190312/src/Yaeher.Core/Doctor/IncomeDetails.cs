using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 收入明细
    /// </summary>
    public class IncomeDetails : EntityBaseModule
    {
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
        /// 咨询ID
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(30)]
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 咨询JSON
        /// </summary>
        public string ConsultJSON { get; set; }
        /// <summary>
        /// 订单单号
        /// </summary>
        [MaxLength(20)]
        public string OrderNumber { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        [MaxLength(10)]
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public double OrderMoney { get; set; }
        /// <summary>
        /// 分成金额
        /// </summary>
        public double ProportionMoney { get; set; }
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime TotalDate { get; set; }

    }
}
