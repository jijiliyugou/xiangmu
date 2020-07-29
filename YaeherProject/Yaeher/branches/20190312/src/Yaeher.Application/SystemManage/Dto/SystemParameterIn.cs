using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SystemParameterIn : ListParameters<SystemParameter>, IPagedResultRequest
    {
        /// <summary>
        /// 参数类型 ConfigPar配置参数 SetPar 系统参数
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 参数类别
        /// </summary>
        public virtual string SystemType { get; set; }
        /// <summary>
        /// 参数编号
        /// </summary>
        public virtual string SystemCode { get; set; }
        /// <summary>
        /// 参数编号
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    /// <summary>
    /// 获取item中的参数值
    /// </summary>
    public class SystemParameterItemValue
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

    }
}
