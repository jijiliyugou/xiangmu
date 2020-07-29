using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityBaseModule : Entity<int>
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public int ModifyBy { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public int DeleteBy { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        
        public DateTime DeleteTime { get; set; }
        /// <summary>
        /// 自签名
        /// </summary>
        [NotMapped]
        public string Secret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityBaseModule()
        {
            //Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
            IsDelete = false;
        }
    }
}
