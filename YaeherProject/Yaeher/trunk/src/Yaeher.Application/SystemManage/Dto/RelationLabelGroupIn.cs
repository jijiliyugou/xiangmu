using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 标签组
    /// </summary>
    public class RelationLabelGroupIn: ListParameters<RelationLabelGroup>, IPagedResultRequest
    {
        /// <summary>
        /// 标签组名
        /// </summary>
        public virtual string GroupName { get; set; }
        /// <summary>
        /// 标签名称 已数组格式存储
        /// </summary>
        public virtual string LableName { get; set; }
    }

    /// <summary>
    /// 标签组
    /// </summary>
    public class LabelGroup : ListParameters<RelationLabelGroup>, IPagedResultRequest
    {
        /// <summary>
        /// 标签组名
        /// </summary>
        public virtual string GroupName { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public virtual int LableID { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string LableName { get; set; }
    }
}
