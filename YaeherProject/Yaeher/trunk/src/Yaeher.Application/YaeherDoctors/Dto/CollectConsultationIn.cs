using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Yaeher.Doctor;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 订单收入分配_医生
    /// </summary>
    public class CollectConsultationIn : ListParameters<CollectConsultation>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(20)]
        public string DoctorName { get; set; }
    }
   
}
