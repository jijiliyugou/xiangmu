using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 标签配置
    /// </summary>
    public class YaeherLabelConfigIn : ListParameters<YaeherLabelConfig>, IPagedResultRequest
    {
        /// <summary>
        /// 标签类型
        /// </summary>
        public virtual string LabelTypeCode { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string LabelTypeName { get; set; }
        /// <summary>
        /// 标签编号
        /// </summary>
        public virtual string LabelCode { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string LabelName { get; set; }
        /// <summary>
        /// parentID
        /// </summary>
        public virtual int ParentID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class YaeherLabelList : ListParameters<YaeherModule>
    {
        /// <summary>
        /// 标签类型
        /// </summary>
        public virtual string LabelTypeCode { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string LabelTypeName { get; set; }
        /// <summary>
        /// 标签编号
        /// </summary>
        public virtual string LabelCode { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string LabelName { get; set; }
        /// <summary>
        /// parentID
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// OrderSort
        /// </summary>
        public virtual int OrderSort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<YaeherLabelList> children { get; set; }
    }
}
