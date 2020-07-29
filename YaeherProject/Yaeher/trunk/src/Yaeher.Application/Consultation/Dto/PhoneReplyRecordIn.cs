using Abp.Application.Services.Dto;
using System;

namespace Yaeher
{
    /// <summary>
    /// 根据name搜索
    /// </summary>
    public class PhoneReplyRecordIn : ListParameters<PhoneReplyRecord>, IPagedResultRequest
    {
        /// <summary>
        /// 咨询单号
        ///YaeherPatientConsultation表ConsultNumber
        /// </summary>
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
        public string CallIntro { get; set; }
        /// <summary>
        /// 录音地址
        /// </summary>
        public string RecordAddress { get; set; }
    }
}