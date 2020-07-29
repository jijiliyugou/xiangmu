using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
   
    /// <summary>
    /// 医生上下线设置
    /// </summary>
    public class DoctorOnlineRecordIn : ListParameters<DoctorOnlineRecord>, IPagedResultRequest
    {
        /// <summary>
        /// 医生名称
        /// </summary>
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 上下线状态
        /// </summary>
        public virtual string OnlineState { get; set; }
    }
}
