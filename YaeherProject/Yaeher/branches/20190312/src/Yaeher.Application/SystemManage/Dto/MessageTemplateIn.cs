using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageTemplateIn : ListParameters<YaeherMessageTemplate>, IPagedResultRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Title { get; set; }
    }

}
