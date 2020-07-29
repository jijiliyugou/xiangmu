using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 咨询评分
    /// </summary>
    public class ConsultationEvaluation : EntityBaseModule
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [MaxLength(20)]
        public string SequenceNo { get; set; }
        /// <summary>
        /// 咨询单号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
        [MaxLength(30)]
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
        [MaxLength(500)]
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
        [MaxLength(500)]
        public string PatientName { get; set; }
        /// <summary>
        /// 医生姓名
        /// YaeherUser表ID
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// YaeherUser表fullname
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [MaxLength(5000)]
        public string EvaluateContent { get; set; }
        /// <summary>
        /// 评论内容
        /// 理由选择
        /// </summary>
        public string EvaluateReason { get; set; }
        /// <summary>
        /// 星级title
        /// </summary>
        public string StarTitle { get; set; }
        /// <summary>
        /// 评论星级
        /// </summary>
        public int EvaluateLevel { get; set; }
        /// <summary>
        /// 星级标签 Json
        /// </summary>
        [MaxLength(100)]
        public string EvaluateLabel { get; set; }
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
        [MaxLength(5000)]
        public string QualityReason { get; set; }

    }
}
