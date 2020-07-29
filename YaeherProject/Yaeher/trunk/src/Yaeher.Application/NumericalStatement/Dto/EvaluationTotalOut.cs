using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.NumericalStatement.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class EvaluationTotalOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="EvaluationTotalDto"></param>
        /// <param name="EvaluationTotalInfo"></param>
        public EvaluationTotalOut(PagedResultDto<EvaluationTotal> EvaluationTotalDto, EvaluationTotalIn EvaluationTotalInfo)
        {
            Items = EvaluationTotalDto.Items;
            TotalCount = EvaluationTotalDto.TotalCount;
            TotalPage = EvaluationTotalDto.TotalCount / EvaluationTotalInfo.MaxResultCount;
            SkipCount = EvaluationTotalInfo.SkipCount;
            MaxResultCount = EvaluationTotalInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<EvaluationTotal> Items { get; set; }
    }
    /// <summary>
    /// 质控
    /// </summary>
    public class QualityEvaluationTotal
    {
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
        /// 已经评分数目
        /// </summary>
        public int EvaluationToTal { get; set; }
        /// <summary>
        /// 未评分数目
        /// </summary>
        public int NoEvaluationToTal { get; set; }
        /// <summary>
        /// 总单数
        /// </summary>
        public int OrderTotal { get; set; }
        /// <summary>
        /// 退单数
        /// </summary>
        public int RefundTotal { get; set; }
        /// <summary>
        /// 完成数
        /// </summary>
        public int CompleteTotal { get; set; }
        /// <summary>
        /// 均分
        /// </summary>
        public double AverageEvaluate { get; set; }
    }

    /// <summary>
    /// 医生查询列表
    /// </summary>
    public class DoctorSortList
    {
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public DoctorInfo doctorInfo { get; set; }
        /// <summary>
        /// 评分统计表 只查询当天数据
        /// </summary>
        public EvaluationTotal evaluationTotal { get; set; }
       
    }

    /// <summary>
    /// 医生信息
    /// </summary>
    public class DoctorInfo {
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public YaeherDoctor YaeherDoctorInfo { get; set; }
        /// <summary>
        /// 医生标签
        /// </summary>
        public List<LableManage> lableManages { get; set; }

    }
    /// <summary>
    /// 医生统计信息
    /// </summary>
    public class DoctorDetailList
    {
        /// <summary>
        /// 
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 工作医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 医生注册时间
        /// </summary>
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 个人头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 上下线状态
        /// </summary>
        public string OnlineState { get; set; }
        /// <summary>
        /// 服务状态  是否可以咨询
        /// </summary>
        public bool ServiceState { get; set; }
        /// <summary>
        /// 接单状态 是否满单
        /// </summary>
        public bool ReceiptState { get; set; }
        /// <summary>
        /// 图文咨询接单费用
        /// </summary>
        public double ImageServiceExpense { get; set; }
        /// <summary>
        /// 图文咨询接单最大数
        /// </summary>
        public int ImageServiceFrequency { get; set; }
        /// <summary>
        ///图文接单数
        /// </summary>
        public int ImageNumberTotal { get; set; }
        /// <summary>
        ///电话接单
        /// </summary>
        public int PhoneNumberTotal { get; set; }
        /// <summary>
        /// 电话接单费用
        /// </summary>
        public double PhoneServiceExpense { get; set; }
        /// <summary>
        ///电话接单最大数
        /// </summary>
        public int PhoneServiceFrequency { get; set; }
        /// <summary>
        /// 综合费用   根据权重计算
        /// </summary>
        public double ServiceExpense { get; set; }
        /// <summary>
        /// 置顶排序
        /// </summary>
        public int SetTopSort { get; set; }
        /// <summary>
        /// 医生总退单率
        /// </summary>
        public double RefundRatio { get; set; }
        /// <summary>
        /// 评价总分
        /// </summary>
        public int EvaluateTotal { get; set; }
        /// <summary>
        /// 评价单数
        /// </summary>
        public int EvaluationCount { get; set; }
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
        /// <summary>
        /// 标签
        /// </summary>
        public List<DoctorRelation> Doctorslable { get; set; }
    }
}
