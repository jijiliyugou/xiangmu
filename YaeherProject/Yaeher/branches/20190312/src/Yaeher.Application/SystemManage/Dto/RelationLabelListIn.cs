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
    public class RelationLabelListIn: ListParameters<RelationLabelList>, IPagedResultRequest
    {
        /// <summary>
        /// 关系类型 科室 医生 文章 问答
        /// </summary>
        public String RelationCode { set; get; }
        /// <summary>
        /// 事件对应的业务ID  对应的科室ID 医生ID 文章ID 问答ID
        /// </summary>
        public int BusinessID { set; get; }
        /// <summary>
        /// 对应的 科室名称 医生名称 文章ID 问答ID
        /// </summary>
        public String BusinessName { set; get; }
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

    /// <summary>
    /// 维护标签关系
    /// </summary>
    public class RelationLabel: ListParameters<RelationLabelList>, IPagedResultRequest
    {
        /// <summary>
        /// 关系类型 科室 医生 文章 问答
        /// </summary>
        public String RelationCode { set; get; }
        /// <summary>
        /// 事件对应的业务ID  对应的科室ID 医生ID 文章ID 问答ID
        /// </summary>
        public int BusinessID { set; get; }
        /// <summary>
        /// 对应的 科室名称 医生名称 文章ID 问答ID
        /// </summary>
        public String BusinessName { set; get; }
        /// <summary>
        /// 标签ID 数组集合
        /// </summary>
        public virtual string LableID { get; set; }
        /// <summary>
        /// 标签名称 
        /// </summary>
        public virtual string LableName { get; set; }
    }
}
