using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.NumericalStatement
{
    /// <summary>
    /// 评分汇总表
    /// </summary>
    public class EvaluationTotal: EntityBaseModule
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
        /// 评价总分
        /// </summary>
        public int EvaluateTotal { get; set; }
        /// <summary>
        /// 5星单总数
        /// </summary>
        public int FiveStar { get; set; }
        /// <summary>
        /// 4星单总数
        /// </summary>
        public int FourStar { get; set; }
        /// <summary>
        /// 3星单总数
        /// </summary>
        public int ThreeStar { get; set; }
        /// <summary>
        /// 2星单总数
        /// </summary>
        public int TwoStar { get; set; }
        /// <summary>
        /// 1星单总数
        /// </summary>
        public int OneStar { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public double AverageEvaluate { get; set; }
        /// <summary>
        /// 总单数
        /// </summary>
        public int OrderTotal { get; set; }
        /// <summary>
        /// 平均回复时长
        /// </summary>
        public double AverageAnswer { get; set; }
        /// <summary>
        /// 收入总额
        /// </summary>
        public double RevenueTotal { get; set; }
        /// <summary>
        /// 退单数
        /// </summary>
        public int RefundTotal { get; set; }
        /// <summary>
        /// 完成数
        /// </summary>
        public int CompleteTotal { get; set; }
    }
}
