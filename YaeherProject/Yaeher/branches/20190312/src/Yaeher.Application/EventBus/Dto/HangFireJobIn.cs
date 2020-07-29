using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yaeher.HangFire;

namespace Yaeher.EventBus.Dto
{
    /// <summary>
    /// hangfire list
    /// </summary>
    public class HangFireJobIn : ListParameters<HangFireJob>, IPagedResultRequest
    {
       
    }
}