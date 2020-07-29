using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    ///医生参数信息设置 
    public class DoctorParaSet : EntityBaseModule
    {
        /// <summary>
        /// 参数code
        /// </summary>
        [MaxLength(50)]
        public virtual string DoctorParaSetCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        [MaxLength(50)]
        public virtual string DoctorParaSetName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [MaxLength(100)]
        public virtual string ItemValue { get; set; }

    }
}
