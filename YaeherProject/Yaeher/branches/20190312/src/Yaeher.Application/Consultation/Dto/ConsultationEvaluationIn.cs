using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemManage.Dto;

namespace Yaeher.Consultation.Dto
{
    /// <summary>
    /// 咨询评分
    /// </summary>
    public class ConsultationEvaluationIn : ListParameters<ConsultationEvaluation>, IPagedResultRequest
    {
        /// <summary>
        /// 咨询单号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        ///咨询ID
        ///YaeherPatientConsultation表ID
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        ///咨询用户ID
        ///YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询用户
        /// YaeherUser表fullname
        /// </summary>
        public string ConsultantName { get; set; }
        /// <summary>
        ///患者ID
        ///YaeherPatientLeaguerInfo表ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        ///患者名称
        ///YaeherPatientLeaguerInfo表LeaguerName
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 医生姓名
        /// YaeherUser表ID
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// YaeherUser表fullname
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string EvaluateContent { get; set; }
        /// <summary>
        /// 评论内容
        /// 理由选择
        /// </summary>
        public string EvaluateReason { get; set; }
        /// <summary>
        /// 评论星级
        /// </summary>
        public int EvaluateLevel { get; set; }
        /// <summary>
        /// 质控评分
        /// </summary>
        public int QualityLevel { get; set; }
        /// <summary>
        /// 是否质控评分
        /// </summary>
        public bool IsQuality { get; set; }
        /// <summary>
        /// 质控原因
        /// </summary>
        public string QualityReason { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string TotalType { get; set; }
    }
    /// <summary>
    /// 患者评分
    /// </summary>
    public class ConsultationEvaluationAdd: ConsultationEvaluation
    {
        /// <summary>
        /// 星级说明标签
        /// </summary>
        public int LabelId { get; set; }
        /// <summary>
        /// 多选标签Id
        /// </summary>
        public List<YaeherLabelList>  TitleList { get; set; }
    }
}
