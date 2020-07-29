using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.HangFire
{
    /// <summary>
    /// 定时服务表
    /// </summary>
    public class HangFireJob: EntityBaseModule
    {
        /// <summary>
        /// 执行job名称 咨询完成   咨询退单  咨询预警  评分统计 订单统计
        /// </summary>
        [MaxLength(50)]
        public string JobName { get; set; }
        /// <summary>
        /// 执行job编号  CompleteConsultation  ReturnConsultation WarningConsultation EvaluationTotal OrderTotal IncomeTotal RemindInquiry RemindEvaluation
        /// </summary>
        [MaxLength(50)]
        public string JobCode { get; set; }
        /// <summary>
        /// 事件对应的业务ID
        /// </summary>
        public int BusinessID { set; get; }
        /// <summary>
        /// 事件对应的业务编号
        /// </summary>
        [MaxLength(20)]
        public string BusinessCode { set; get; }
        /// <summary>
        /// Job执行时间
        /// </summary>
        public DateTime JobRunTime { get; set; }
        /// <summary>
        /// Job执行返回ID
        /// </summary>
        [MaxLength(100)]
        public string JobRunID { get; set; }
        /// <summary>
        /// 执行job状态  Open Complete Close
        /// </summary>
        [MaxLength(20)]
        public string JobSates { get; set; }
        /// <summary>
        /// 回调job方法
        /// </summary>
        [MaxLength(500)]
        public string CallbackUrl { get; set; }
        /// <summary>
        /// 回调方法执行参数
        /// </summary>
        public string JobParameter { get; set; }
    }
}
