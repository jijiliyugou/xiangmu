using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfigs: EntityBaseModule
    {
        /// <summary>
        /// 固定维护
        /// 适用类型 AliCenter AliMessage TencentWechar TencentPay
        /// </summary>
        [MaxLength(100)]
        public virtual string SystemType { get; set; }
        /// <summary>
        /// 适合名称
        /// </summary>
        [MaxLength(500)]
        public virtual string SystemName { get; set; }
        /// <summary>
        /// 客户ID 默认为string类型  需要自行转换int类型
        /// </summary>
        [MaxLength(500)]
        public virtual string AppID { get; set; }
        /// <summary>
        /// 客户密钥
        /// </summary>
        [MaxLength(500)]
        public virtual string AppKey { get; set; }
        /// <summary>
        /// 客户secret
        /// </summary>
        [MaxLength(500)]
        public virtual string AppSecret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(500)]
        public virtual string AccessKeyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(500)]
        public virtual string AccessSecret { get; set; }
        /// <summary>
        /// 用于发起阿里呼叫时刷新token
        /// </summary>
        [MaxLength(500)]
        public virtual string RefreshTokenCode { get; set; }
        /// <summary>
        /// ali呼叫的电话号码
        /// </summary>
        [MaxLength(500)]
        public virtual string CallCenterNumber { get; set; }
        /// <summary>
        /// 腾讯支付回调Url
        /// </summary>
        [MaxLength(500)]
        public string TenPayNotify { get; set; }
        /// <summary>
        /// 腾讯退款回调Url
        /// </summary>
        [MaxLength(500)]
        public string TenPayRefundNotify { get; set; }
        /// <summary>
        /// 腾讯支付回调Url
        /// </summary>
        [MaxLength(500)]
        public string TenPayWxOpenNotify { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        [MaxLength(500)]
        public string TenPayMchId { get; set; }
        /// <summary>
        /// 商户支付密钥Key。登录微信商户后台，进入栏目【账户设置】【密码安全】【API 安全】【API 密钥】
        /// </summary>
        [MaxLength(500)]
        public string TenPayKey { get; set; }
        /// <summary>
        /// 是否需要分账
        /// Y 分账，N 不分账，默认不分账,Y 大写
        /// </summary>
        public bool TenPayProfitSharing { get; set; }
    }
}
