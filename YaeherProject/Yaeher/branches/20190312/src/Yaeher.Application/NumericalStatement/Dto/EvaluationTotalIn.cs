using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.NumericalStatement.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class EvaluationTotalIn : ListParameters<EvaluationTotal>, IPagedResultRequest
    {
       
    }
    /// <summary>
    /// 医生排序
    /// </summary>
    public class DockorSortIn : ListParameters<EvaluationTotal>, IPagedResultRequest
    {
        /// <summary>
        /// 排序类型 默认Default  平均分Evaluation 回复时长AnswerTimer  费用Expense
        /// </summary>
        public string SortType { get; set; }
        /// <summary>
        /// 科室ID
        /// </summary>
        public int ClinliId { get; set; }
        /// <summary>
        /// 排序规则 ACS正序  倒叙 DESC
        /// </summary>
        public string SortRule { get; set; }
        /// <summary>
        /// desc,asc
        /// </summary>
        public string SortDesc { get; set; }
    }

    public class EvaluationTotalInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
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
        /// 评分
        /// </summary>
        public int Evaluate { get; set; }
        /// <summary>
        /// 平均回复时长
        /// </summary>
        public double AnswerTime { get; set; }
        /// <summary>
        /// 退单单号
        /// </summary>
        public string RefundNumber { get; set; }
        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 退单时间
        /// </summary>
        public DateTime RefundTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime Completetime { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal OrderMoney { get; set; }
    }
}
