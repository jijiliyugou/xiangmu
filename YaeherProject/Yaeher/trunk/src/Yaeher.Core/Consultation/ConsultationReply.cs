using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 咨询回答
    /// </summary>
    public class ConsultationReply : EntityBaseModule
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
        /// 咨询类型
        ///  图文 电话 或者其他
        /// </summary>
        [MaxLength(20)]
        public string ConsultType { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(20)]
        public string PatientTelephone { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        [MaxLength(200)]
        public string PatientCity { get; set; }
        /// <summary>
        /// 咨询问题描述
        /// </summary>
        [MaxLength(6000)]
        public string IllnessDescription { get; set; }
        /// <summary>
        /// 回答类型   回答 追问
        /// </summary>
        [MaxLength(10)]
        public string ReplyType { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        [MaxLength(6000)]
        public string RepayIllnessDescription { get; set; }
        /// <summary>
        /// 处理状态  草稿 提交
        /// </summary>
        [MaxLength(20)]
        public string ReplyState { get; set; }


    }
}
