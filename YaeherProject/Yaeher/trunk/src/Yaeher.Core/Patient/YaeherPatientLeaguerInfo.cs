using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 患者成员管理
    /// </summary>
    public class YaeherPatientLeaguerInfo : EntityBaseModule
    {
        /// <summary>
        /// 成员名称
        /// </summary>
        [MaxLength(20)]
        public string LeaguerName { get; set; }
        /// <summary>
        /// 患者ID
        /// YaeherUser表ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        ///与本人关系
        /// </summary>
        [MaxLength(20)]
        public string Relationship { get; set; }
        /// <summary>
        ///成员联系电话
        /// </summary>
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        /// <summary>
        ///生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        ///性别 1，男，2，女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        ///过敏史
        /// </summary>
        [MaxLength(200)]
        public string AllergicHistory { get; set; }
        /// <summary>
        /// 有无过敏史
        /// </summary>
        public bool HasAllergic { get; set; }
        /// <summary>
        ///地址
        /// </summary>
        [MaxLength(300)]
        public string Address { get; set; }
        /// <summary>
        ///邮箱
        /// </summary>
        [MaxLength(100)]
        public string Email { get; set; }
        /// <summary>
        ///微信号
        /// </summary>
        [MaxLength(100)]
        public string Wechat { get; set; }
        /// <summary>
        ///用户身份证号码
        /// </summary>
        [MaxLength(20)]
        public string IDCard { get; set; }

    }
}
