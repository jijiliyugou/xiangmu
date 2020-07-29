using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 标签管理 
    /// </summary>
    public class LableManage : EntityBaseModule
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        [MaxLength(20)]
        [Required(ErrorMessage = "标签名称，不能为空")]
        [StringLength(20,ErrorMessage = "标签名称，字符太长")]
        public virtual string LableName { get; set; }
        /// <summary>
        /// 标签说明
        /// </summary>
        [MaxLength(100)]
        public virtual string LableRemark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int OrderSort { get; set; }

    }
}
