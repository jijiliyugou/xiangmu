using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的医生
    /// </summary>
    public class YaeherPatientDoctorIn : ListParameters<YaeherPatientDoctor>, IPagedResultRequest
    {
        /// <summary>
        /// 医生
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生
        /// </summary>
        public int doctorid { get; set; }
        /// <summary>
        /// 收藏人
        /// </summary>

        public int createdby { get; set; }
    }
   
}
