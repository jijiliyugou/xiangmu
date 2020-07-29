using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 标签关系表
    /// </summary>
    public class RelationLabelList:EntityBaseModule
    {
        /// <summary>
        /// 关系类型 科室 Clinic 医生 Doctor 文章 Paper 问答 Question
        /// </summary>
        [MaxLength(10)]
        public String RelationCode { set; get; }
        /// <summary>
        /// 事件对应的业务ID  对应的科室ID 医生ID 文章ID 问答ID
        /// </summary>
        public int BusinessID { set; get; }
        /// <summary>
        /// 对应的 科室名称 医生名称 文章ID 问答ID
        /// </summary>
        [MaxLength(50)]
        public String BusinessName { set; get; }
        /// <summary>
        /// 标签组名
        /// </summary>
        [MaxLength(100)]
        public virtual string GroupName { get; set; }
        /// <summary>
        /// 标签ID 
        /// </summary>
        public virtual int LableID { get; set; }
        /// <summary>
        /// 标签名称 
        /// </summary>
        [MaxLength(100)]
        public virtual string LableName { get; set; }
    }
}
