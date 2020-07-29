using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生上下线设置Log
    /// </summary>
    public class DoctorOnlineSetLogIn : ListParameters<DoctorOnlineSetLog>, IPagedResultRequest
    {

    }
}
