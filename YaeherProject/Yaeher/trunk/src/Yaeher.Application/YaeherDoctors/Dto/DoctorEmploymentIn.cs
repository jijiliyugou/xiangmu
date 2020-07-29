using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.Doctor;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生执业
    /// </summary>
    public class DoctorEmploymentIn : ListParameters<DoctorEmployment>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        public int DoctorID{ get; set; }
    }
  
}
