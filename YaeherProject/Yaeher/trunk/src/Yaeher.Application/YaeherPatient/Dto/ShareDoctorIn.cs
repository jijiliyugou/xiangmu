using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的分享
    /// </summary>
    public class ShareDoctorIn : ListParameters<ShareDoctor>, IPagedResultRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string DoctorName { get; set; }
    }
}
