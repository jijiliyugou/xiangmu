using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 医生排班管理
    /// </summary>
    public class DoctorScheduling : EntityBaseModule
    {
        /// <summary>
        /// 医生名称
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public virtual string DoctorJSON { get; set; }
        /// <summary>
        /// 排班时间
        /// </summary>
        public virtual DateTime SchedulingDate { get; set; }
        /// <summary>
        /// 排班时段 上午，下午，晚上
        /// </summary>
        [MaxLength(1000)]
        public virtual string SchedulingTime { get; set; }
        /// <summary>
        /// 重复方式 每天，每周，从不
        /// </summary>
        [MaxLength(2000)]
        public virtual string Duplication { get; set; }
        /// <summary>
        /// 门诊类型    普通 ，特诊，专家
        /// </summary>
        [MaxLength(2000)]
        public virtual string ClinicType { get; set; }
        /// <summary>
        /// 门诊地点
        /// </summary>
        [MaxLength(200)]
        public virtual string ClinicIDAdd { get; set; }
        /// <summary>
        /// 挂号费
        /// </summary>
        public virtual decimal RegistrationFee { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public virtual bool ServiceState { get; set; }

    }
}
