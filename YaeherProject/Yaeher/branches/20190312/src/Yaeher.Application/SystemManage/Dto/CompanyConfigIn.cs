using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 公司配置说明
    /// </summary>
    public class CompanyConfigIn : ListParameters<CompanyConfig>, IPagedResultRequest
    {
        /// <summary>
        /// 参数类别
        /// </summary>
        public virtual string ConfigType { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        public virtual string ConfigImageUrl { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        public virtual string ConfigTitle { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        public virtual string ConfigContent { get; set; }
    }
}
