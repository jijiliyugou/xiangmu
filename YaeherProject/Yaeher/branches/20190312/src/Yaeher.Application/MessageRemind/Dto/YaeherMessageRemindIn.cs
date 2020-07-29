using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.MessageRemind.Dto
{
    /// <summary>
    /// 短信对接
    /// </summary>
    public class YaeherMessageRemindIn : ListParameters<YaeherMessageRemind>, IPagedResultRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string MessageType { get; set; }
    }
}
