using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfigsIn : ListParameters<SystemConfigs>, IPagedResultRequest
    {
        /// <summary>
        /// 固定维护
        /// 适用类型 AliCenter AliMessage TencentWechar TencentPay  
        /// </summary>
        public virtual string SystemType { get; set; }
    }
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemTokenIn : ListParameters<SystemToken>, IPagedResultRequest
    {
      /// <summary>
      /// 
      /// </summary>
        public virtual string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string YaeherPlatform { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string AppSecret { get; set; }
    }
}
