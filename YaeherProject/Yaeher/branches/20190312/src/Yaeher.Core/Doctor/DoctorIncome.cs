using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的收入
    /// </summary>
    public class DoctorIncome : EntityBaseModule
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
        /// 收入时间类型 
        ///年 月 季 天
        /// </summary>
        [MaxLength(10)]
        public string IncomeTimeType { get; set; }
        /// <summary>
        /// 统计金额
        /// </summary>
        public double Total { get; set; }
        /// <summary>
        /// 更新版本
        /// </summary>
        public DateTime TotalDate { get; set;}

    }
}
