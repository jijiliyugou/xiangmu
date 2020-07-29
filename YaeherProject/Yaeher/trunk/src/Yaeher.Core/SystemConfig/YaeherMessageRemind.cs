using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 短信发送记录表
    /// </summary>
    public class YaeherMessageRemind : EntityBaseModule
    {
        /// <summary>
        /// 接收用户ID
        /// </summary>
        public virtual int UserID { get; set; }
        /// <summary>
        /// 接收用户姓名
        /// </summary>
        [MaxLength(20)]
        public virtual string UserName { get; set; }
        /// <summary>
        /// 接收用户手机号码
        /// </summary>
        [MaxLength(20)]
        public virtual string PhoneNumber { get; set; }
        /// <summary>
        /// 短信类型  注册 认证 咨询提醒  咨询退单系统  
        /// </summary>
        [MaxLength(20)]
        public virtual string MessageType { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        [MaxLength(200)]
        public virtual string Message { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        [MaxLength(10)]
        public virtual string VerificationCode { get; set; }
        /// <summary>
        /// 有效时长
        /// </summary>
        public virtual int EffectiveLength { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public virtual DateTime EffectiveTime { get; set; }

    }
}
