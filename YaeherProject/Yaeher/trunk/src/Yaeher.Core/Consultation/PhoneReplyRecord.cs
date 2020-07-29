using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 电话回复记录
    /// </summary>
    public class PhoneReplyRecord : EntityBaseModule
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [MaxLength(20)]
        public string SequenceNo { get; set; }
        /// <summary>
        /// 咨询单号
        ///YaeherPatientConsultation表ConsultNumber
        /// </summary>
        [MaxLength(30)]
        public string ConsultNumber { get; set; }

        /// <summary>
        /// 咨询ID
        ///YaeherPatientConsultation表ID
        /// </summary>
        public int ConsultID { get; set; }

        /// <summary>
        /// 咨询用户ID
        ///YaeherUser表ID
        /// </summary>
        public int ConsultantID { get; set; }
        /// <summary>
        /// 咨询用户
        ///YaeherUser表fullname
        /// </summary>
        [MaxLength(500)]
        public string ConsultantName { get; set; }
        /// <summary>
        /// 患者ID
        ///YaeherPatientLeaguerInfo表ID
        /// </summary>
        public int PatientID { get; set; }
        /// <summary>
        /// 患者名称
        ///YaeherPatientLeaguerInfo表LeaguerName
        /// </summary>
        [MaxLength(500)]
        public string PatientName { get; set; }
        /// <summary>
        /// 咨询医生ID
        ///YaeherUser表ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 医生名称
        ///YaeherUser表fullname
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 拨通时间
        /// </summary>
        public DateTime CallTime { get; set; }
        /// <summary>
        /// 拨通时长
        /// </summary>
        public int CallDuration { get; set; }
        /// <summary>
        /// 拨通说明
        /// </summary>
        [MaxLength(500)]
        public string CallIntro { get; set; }
        /// <summary>
        /// 录音地址
        /// </summary>
        [MaxLength(200)]
        public string RecordAddress { get; set; }
        /// <summary>
        /// 主叫电话号码
        /// </summary>
        [MaxLength(20)]
        public string Caller { get; set; }
        /// <summary>
        /// 被叫电话号码
        /// </summary>
        [MaxLength(20)]
        public string Callee { get; set; }
        /// <summary>
        /// 是否上传
        /// </summary>
        public bool IsUpload { get; set; }
        /// <summary>
        /// 存储地址
        /// </summary>
        [MaxLength(200)]
        public string StorageAddress { get; set; }

    }
}
