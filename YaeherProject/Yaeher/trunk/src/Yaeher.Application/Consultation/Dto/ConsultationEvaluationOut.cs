using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Yaeher.Common;
using Yaeher.Consultation.Dto;

namespace Yaeher
{
    /// <summary>
    /// ConsultationEvaluationOut
    /// </summary>
    public class ConsultationEvaluationOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ConsultationEvaluationDto"></param>
        /// <param name="ConsultationEvaluationInfo"></param>
        public ConsultationEvaluationOut(PagedResultDto<ConsultationEvaluation> ConsultationEvaluationDto, ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            Items = ConsultationEvaluationDto.Items.Select(t => new DoctorConsultationEvaluation(t)).ToList();
            TotalCount = ConsultationEvaluationDto.TotalCount;
            TotalPage = ConsultationEvaluationDto.TotalCount / ConsultationEvaluationInfo.MaxResultCount;
            SkipCount = ConsultationEvaluationInfo.SkipCount;
            MaxResultCount = ConsultationEvaluationInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ConsultationEvaluation> Items { get; set; }
    }
    public class doctorCountView
    {
        public int ReceiptNumBer { get; set; }//接单数
        public string AverageTime { get; set; }//平均时长
        public double AverageEvaluate { get; set; } //星级

        public string UserImage { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DoctorConsultationEvaluation : ConsultationEvaluation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public DoctorConsultationEvaluation(ConsultationEvaluation item)
        {
            ConsultNumber = item.ConsultNumber;
            ConsultID = item.ConsultID;
            ConsultantID = item.ConsultantID;
            ConsultantName = item.ConsultantName;
            PatientID = item.PatientID;
            PatientName = item.PatientName;
            DoctorName = item.DoctorName;
            DoctorID = item.DoctorID;
            EvaluateContent = item.EvaluateContent;
            EvaluateReason = item.EvaluateReason;
            StarTitle = item.StarTitle;
            EvaluateLevel = item.EvaluateLevel;
            QualityLevel = item.QualityLevel;
            IsQuality = item.IsQuality;
            QualityReason = item.QualityReason;
            Id = item.Id;
            CreatedOn = item.CreatedOn;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
    /// <summary>
    /// 标题
    /// </summary>
    public class ConsultationEvaluationReason : CodeList
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public int ModifyBy { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public int DeleteBy { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>

        public DateTime DeleteTime { get; set; }
        /// <summary>
        /// 自签名
        /// </summary>
        [NotMapped]
        public string Secret { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
    /// <summary>
    /// 小标签
    /// </summary>
    public class EvaluationDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public int ModifyBy { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public int DeleteBy { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>

        public DateTime DeleteTime { get; set; }
        /// <summary>
        /// 自签名
        /// </summary>
        [NotMapped]
        public string Secret { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Message { get; set; }
    }
    /// <summary>
    /// 默认List
    /// </summary>
    public class ConsultationEvaluationReasonList
    {
        /// <summary>
        /// title
        /// </summary>
        public List<ConsultationEvaluationReason> ConsultationEvaluationReason { get; set; }
        /// <summary>
        /// detail
        /// </summary>
        public List<CodeList> EvaluationDetail { get; set; }
    }
    /// <summary>
    /// 评价详情
    /// </summary>
    public class ConsultationEvaluationDetail : ConsultationEvaluation
    {
        /// <summary>
        /// 星级说明
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 医生头像
        /// </summary>
        public string DoctorImage { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="item"></param>
        public ConsultationEvaluationDetail(ConsultationEvaluation item, YaeherUserDocMsg doctor)
        {
            ConsultNumber = item.ConsultNumber;
            ConsultID = item.ConsultID;
            ConsultantID = item.ConsultantID;
            ConsultantName = item.ConsultantName;
            PatientID = item.PatientID;
            PatientName = item.PatientName;
            DoctorName = item.DoctorName;
            DoctorID = item.DoctorID;
            EvaluateContent = item.EvaluateContent;
            EvaluateReason = item.EvaluateReason;
            EvaluateLevel = item.EvaluateLevel;
            QualityLevel = item.QualityLevel;
            QualityReason = item.QualityReason;
            IsQuality = item.IsQuality;
            QualityReason = item.QualityReason;
            Reason = item.StarTitle;
            Id = item.Id;
            DoctorImage = doctor.UserImage;
        }
    }
}
