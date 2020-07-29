using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class YaeherOperListIn : ListParameters<YaeherOperList>, IPagedResultRequest
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual string OperType { get; set; }

    }
}
