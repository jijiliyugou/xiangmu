using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    public class YaeherUser: EntityBaseModule
    {
        /// <summary>
        /// 登陆名称
        /// </summary>
        [MaxLength(500)]
        public virtual string LoginName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [MaxLength(50)]
        public virtual string LoginPwd { get; set; }
        /// <summary>
        /// 用户全称
        /// </summary>
        [MaxLength(500)]
        public virtual string FullName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [MaxLength(20)]
        public virtual string PhoneNumber { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [MaxLength(100)] 
        public virtual string Email{ get; set; }
        /// <summary>
        /// 用户身份证号码
        /// </summary>
        [MaxLength(20)]
        public virtual string IDCard{ get; set; }
        /// <summary>
        /// 性别 1,男，2，女
        /// </summary>
        public virtual int Sex{ get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime Birthday{ get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual bool Enabled{ get; set; }
        /// <summary>
        /// 登陆错误次数
        /// </summary>
        public virtual int ErrorCount{ get; set; }
        /// <summary>
        /// 登陆次数
        /// </summary>
        public virtual int LoginCount{ get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        [MaxLength(10)]
        public virtual string Userorigin{ get; set; }
        /// <summary>
        /// 关联微信号
        /// </summary>
        [MaxLength(100)]
        public virtual string WecharNo{ get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
       [MaxLength(200)]
        public virtual string WecharName{ get; set; }
        /// <summary>
        /// 微信openID
        /// </summary>
        [MaxLength(200)]
        public virtual string WecharOpenID { get; set; }
        /// <summary>
        /// 图像地址
        /// </summary>
        [MaxLength(500)]
        public virtual string UserImage{ get; set; }
        /// <summary>
        /// 角色说明
        /// </summary>
        [MaxLength(50)]
        public virtual string RoleName { get; set; } 
        /// <summary>
        /// 微信个性话标签  存中文名称
        /// </summary> 
        [MaxLength(20)]
        public virtual string WecharLable { get; set; }
        /// <summary>
        /// 微信个性话标签ID
        /// </summary> 
        public virtual int WecharLableId { get; set; }
        /// <summary>
        /// 用户微信基本信息
        /// </summary>
        public string WeCharUserJson { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(500)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户OpenID
        /// </summary>
        [MaxLength(200)]
        public string OpenID { get; set; }
        /// <summary>
        /// 是否已经打过微信的标签
        /// </summary>
        public bool IsLabel { get; set; }
        /// <summary>
        /// 是否存在支付方式
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 是否已经更新OpenID
        /// </summary>
        public bool IsUpdate { get; set; }
        /// <summary>
        /// 是否生成分账账号
        /// </summary>
        public bool IsProfitSharing { get; set; }
    }
}
