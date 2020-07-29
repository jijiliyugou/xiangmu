using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Consultation.Dto
{
    /// <summary>
    /// 咨询回答
    /// </summary>
    public class ConsultationReplyIn : ListParameters<ConsultationReply>, IPagedResultRequest
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
        /// 咨询类型
        ///  图文 电话 或者其他
        /// </summary>
        public string ConsultType { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string PatientTelephone { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string PatientCity { get; set; }
        /// <summary>
        /// 咨询问题描述
        /// </summary>
        public string IllnessDescription { get; set; }
        /// <summary>
        /// 回答类型 
        /// </summary>
        /// </summary>
        public string ReplyType { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string RepayIllnessDescription { get; set; }
        /// <summary>
        /// 处理状态  草稿 提交
        /// </summary>
        public string ReplyState { get; set; }
    }
    /// <summary>
    /// 创建
    /// </summary>
    public class ConsultationReplyAdd: ConsultationReply
    {
        /// <summary>
        /// 1.回答2.追问
        /// </summary>
        public string ReplyTypeCode { get; set; }
        /// <summary>
        /// 1.草稿 2.提交
        /// </summary>
        public string ReplyStateCode { get; set; }
        /// <summary>
        /// 推荐医生ID
        /// </summary>
        public int RecommendDoctorID { get; set; }
        /// <summary>
        /// 推荐医生姓名
        /// </summary>
        public string RecommendDoctorName { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentIn> Attach { get; set; }
    }
   
}
