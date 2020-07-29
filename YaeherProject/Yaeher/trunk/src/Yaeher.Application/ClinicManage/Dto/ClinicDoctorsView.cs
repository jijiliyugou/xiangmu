using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Yaeher.LableManages.Dto;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ClinicDoctorsView : YaeherDoctor
    {
        /// <summary>
        /// 平均时长
        /// </summary>
        public string AverageTime { get; set; }
        /// <summary>
        /// 接单数
        /// </summary>
        public int ReceiptNumBer { get; set; }
        /// <summary>
        /// 已评价订单数
        /// </summary>
        public int EvaluationCount { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 医生星级
        /// </summary>
        public string DoctorLevel { get; set; }

        /// <summary>
        /// 1，正常2，今日已满额3，暂停服务
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 医生标签
        /// </summary>
        public IList<DoctorRelation> Doctorslable { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 质控委员Id
        /// </summary>
        public int QualityControlId { get; set; }
        /// <summary>
        /// 上下线状态
        /// </summary>
        public string OnlineState{get;set;}
        /// <summary>
        /// 上下线Id
        /// </summary>
        public int DoctorOnlineRecordId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ServiceMoneyList> serviceMoneyList { get; set; }
        /// <summary>
        /// 图文咨询接单费用
        /// </summary>
        public double ImageServiceExpense { get; set; }
        /// <summary>
        /// 图文咨询接单最大数
        /// </summary>
        public int ImageServiceFrequency { get; set; }
        /// <summary>
        /// 电话接单费用
        /// </summary>
        public double PhoneServiceExpense { get; set; }
        /// <summary>
        ///电话接单最大数
        /// </summary>
        public int PhoneServiceFrequency { get; set; }

        /// <summary>
        /// 服务状态  是否可以咨询
        /// </summary>
        public bool ServiceState { get; set; }
        /// <summary>
        /// 接单状态 是否满单
        /// </summary>
        public bool ReceiptState { get; set; }
        /// <summary>
        /// 医生总退单率
        /// </summary>
        public double RefundRatio { get; set; }
        /// <summary>
        /// 评价总分
        /// </summary>
        public int EvaluateTotal { get; set; }
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
