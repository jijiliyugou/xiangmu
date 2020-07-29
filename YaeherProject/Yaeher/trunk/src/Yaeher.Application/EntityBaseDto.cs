using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    public class EntityBaseDto: EntityDto<int>
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public virtual int CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreatedOn { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public virtual int ModifyBy { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public virtual DateTime ModifyOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDelete { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public virtual int DeleteBy { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime DeleteTime { get; set; }
    }
    /// <summary>
    /// 根据分页搜索
    /// </summary>
    public class PageBaseModel<T>: ListParameters<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int SkipCount { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int MaxResultCount { get; set; }

    }
    /// <summary>
    /// 根据分页搜索
    /// </summary>
    public class PageBaseModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int SkipCount { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int MaxResultCount { get; set; }

    }
}
