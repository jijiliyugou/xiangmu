using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 医生提供服务费用表
    /// </summary>
   public class ServiceMoneyList:EntityBaseModule
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 服务类型 
        /// 1，图文 2，电话  3，视频 等方式
        /// </summary>
        [MaxLength(20)]
        public string ServiceType { get; set; }
        /// <summary>
        /// 服务类型 
        /// 1，图文 2，电话  3，视频 等方式
        /// </summary>
        [MaxLength(20)]
        public string ServiceTypeValue { get; set; }
        /// <summary>
        /// 服务时长
        /// </summary>
        public int ServiceDuration { get; set; }
        /// <summary>
        /// 服务费用
        /// </summary>
        public double ServiceExpense { get; set; }
        /// <summary>
        /// 最大服务次数
        /// </summary>
        public int ServiceFrequency { get; set; }
        /// <summary>
        /// 服务开启状态  
        /// </summary>
        public bool ServiceState { get; set; }
        /// <summary>
        /// 开放服务时间
        /// </summary>
        public DateTime ServiceTime { get; set; }
        /// <summary>
        /// 实际接单数
        /// </summary>
        public int ActualNumber { get; set; }

    }
}
