using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
   
    /// <summary>
    /// 医生提供服务费用表
    /// </summary>
    public class ServiceMoneyListIn : ListParameters<ServiceMoneyList>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(20)]
        public string DoctorName { get; set; }
    }
  
}
