using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// ListParameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListParameters<T> : Criteria<T>
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
        private int DefaultMaxResultCount = 10;
        /// <summary>
        /// ListParameters
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public object Parameters { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Range(0, 999999999)]
        public int SkipCount {get { return this.DefaultSkipCount - 1; } set { this.DefaultSkipCount = value; }  }
        /// <summary>
        /// 跳转条数
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int SkipTotal { get { return (this.DefaultSkipCount - 1)*this.DefaultMaxResultCount; } set { this.DefaultSkipCount = (value* this.DefaultMaxResultCount); } }
        /// <summary>
        /// 每页最大数量
        /// </summary>
        [Range(0, 100)]
        public int MaxResultCount { get { return this.DefaultMaxResultCount; } set { this.DefaultMaxResultCount = value; } }
        /// <summary>
        /// 自签名
        /// </summary>
        public string Secret { get; set; }
        
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 实例ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 平台类型  PC or Mobile 
        /// </summary>

        public string Platform { get; set; }
        /// <summary>
        /// 构造ListParameters
        /// </summary>
        /// <param name="initCriteria"></param>
        public ListParameters(Expression<Func<T, bool>> initCriteria = null) : base(initCriteria)
        {
        }
        /// <summary>
        /// RecordCount
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public long RecordCount { get; set; }
        /// <summary>
        /// Asc
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Expression<Func<T, object>> Asc { get; set; }
        /// <summary>
        /// Desc
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Expression<Func<T, object>> Desc { get; set; }
        /// <summary>
        /// Includes
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<Expression<Func<T, object>>> Includes { get; set; }
        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="includeExpr"></param>
        /// <returns></returns>
        public ListParameters<T> Include(Expression<Func<T, object>> includeExpr)
        {
            IList<Expression<Func<T, object>>> includes = null;
            if (this.Includes == null) Includes = includes = new List<Expression<Func<T, object>>>();
            else
            {
                includes = this.Includes as IList<Expression<Func<T, object>>>;
                if (includes == null) Includes = includes = new List<Expression<Func<T, object>>>(this.Includes);

            }
            includes.Add(includeExpr);
            this.Includes = includes;
            return this;
        }
        [Newtonsoft.Json.JsonIgnore]
        IList<T> _Items;
        /// <summary>
        /// 数据集合
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public IList<T> Items
        {
            get
            {
                if (_Items == null) _Items = new List<T>();
                return _Items;
            }
            set
            {
                this._Items = value;
                if (this.RecordCount == 0 && value != null) this.RecordCount = value.Count;
            }
        }

    }
}
