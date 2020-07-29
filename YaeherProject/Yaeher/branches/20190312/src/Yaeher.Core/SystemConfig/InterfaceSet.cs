using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 对接接口设置 
    /// </summary>
    public class InterfaceSet : EntityBaseModule
    {
        /// <summary>
        /// 对接端口名称
        /// </summary>
        [MaxLength(200)]
        public virtual string InterfaceName { get; set; }
        /// <summary>
        /// 对接端口说明
        /// </summary>
        [MaxLength(200)]
        public virtual string InterfaceIntro { get; set; }
        /// <summary>
        /// 对接端口地址
        /// </summary>
        [MaxLength(200)]
        public virtual string InterfaceAddress { get; set; }
    }
}
