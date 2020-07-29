using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    public class SendMessageIn : ListParameters<SendMessageTemplate>, IPagedResultRequest
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public virtual string TemplateCode { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual string OperationType { get; set; }
    }
}
