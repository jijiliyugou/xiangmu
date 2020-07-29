using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 接口管理
    /// </summary>
    public class InterfaceSetIn : ListParameters<InterfaceSet>, IPagedResultRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string InterfaceName { get; set; }
    }
}
