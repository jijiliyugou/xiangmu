using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yaeher
{
    /// <summary>
    /// OutPageBaseModel
    /// </summary>
    public class PagerViewModel
    {
        /// <summary>
        /// DefaultSkipCount
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        private int DefaultSkipCount = 1;
        /// <summary>
        /// DefaultSkipCount
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        private int DefaultTotalPage = 1;
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set ; }
        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPage { get { return this.DefaultTotalPage + 1; } set { this.DefaultTotalPage = value; } }
        /// <summary>
        /// 当前页
        /// </summary>
        public int SkipCount { get { return this.DefaultSkipCount + 1; } set { this.DefaultSkipCount = value; } }
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int MaxResultCount { get; set; }

    }
    /// <summary>
    /// code,value
    /// </summary>
    public class CodeList
    {
        /// <summary>
        /// code key
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 所属类型名称
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 所属类型code
        /// </summary>
        public string TypeCode { get; set; }
    }
    
}
