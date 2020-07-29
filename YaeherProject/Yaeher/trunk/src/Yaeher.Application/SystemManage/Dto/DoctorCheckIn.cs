using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生审核
    /// </summary>
    public class DoctorCheckIn : ListParameters<DoctorCheck>, IPagedResultRequest
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual string CheckType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual string CheckState { get; set; }
    }
}
