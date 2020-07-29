using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 获取平台对应的token值
    /// </summary>
    public class SystemToken:EntityBaseModule
    {
        /// <summary>
        ///  Token类型  Wechar  Ali  JSWechar 
        /// </summary>
        [MaxLength(100)]
        public virtual string TokenType { get; set; }
        /// <summary>
        /// 平台设置
        /// </summary>
        [MaxLength(100)]
        public virtual string YaeherPlatform { get; set; }
        /// <summary>
        /// 对应的Appid
        /// </summary>
        [MaxLength(500)]
        public virtual string Appid { get; set; }
        /// <summary>
        /// 对应Secret
        /// </summary>
        [MaxLength(500)]
        public virtual string AppSecret { get; set; }
        /// <summary>
        /// 对应Refresh_token
        /// </summary>
        [MaxLength(500)]
        public virtual string Refresh_token { get; set; }
        /// <summary>
        /// 刷新token有效期
        /// </summary>
        [MaxLength(500)]
        public virtual DateTime RefreshEffectiveTime { get; set; }
        /// <summary>
        /// 返回token
        /// </summary>
        [MaxLength(500)]
        public virtual string access_token { get; set; }
        /// <summary>
        /// 返回整个实体类
        /// </summary>
        public virtual string TokenJson { get; set; }
        /// <summary>
        /// 过期时效
        /// </summary>
        public virtual int EffectiveLength { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime EffectiveTime { get; set; }

    }
}
