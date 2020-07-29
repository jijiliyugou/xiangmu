using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 用户基础表
    /// </summary>
    public class YaeherUserIn : ListParameters<YaeherUser>, IPagedResultRequest
    {
        /// <summary>
        /// 登陆
        /// </summary>
        public virtual string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string LoginPwd { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual string Enabled { get; set; }
        /// <summary>
        /// 用户全称
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public virtual string PhoneNumber { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// 用户身份证号码
        /// </summary>
        public virtual string IDCard { get; set; }
        /// <summary>
        /// 性别 1,男，2，女
        /// </summary>
        public virtual int Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime Birthday { get; set; }
        /// <summary>
        /// 登陆错误次数
        /// </summary>
        public virtual int ErrorCount { get; set; }
        /// <summary>
        /// 登陆次数
        /// </summary>
        public virtual int LoginCount { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        public virtual string Userorigin { get; set; }
        /// <summary>
        /// 关联微信号
        /// </summary>
        public virtual string WecharNo { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public virtual string WecharName { get; set; }
        /// <summary>
        /// 图像地址
        /// </summary>
        public virtual string UserImage { get; set; }
        /// <summary>
        /// 角色说明
        /// </summary>
        public virtual string RoleName { get; set; }
        /// <summary>
        /// 分配移动端角色
        /// </summary>
        public string WecharRole { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
        /// <summary>
        /// 用户微信openID
        /// </summary>
        public virtual string WecharOpenID { get; set; }
    }
    /// <summary>
    /// 登陆后获取个人信息
    /// </summary>
    public class YaeherUserMsg
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
    }
    /// <summary>
    /// IDArray 
    /// </summary>
    public class YaeherUserIDArray
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Id数据集
        /// </summary>
        public string IDArray { get; set; }
    }
    /// <summary>
    /// IDArray 
    /// </summary>
    public class YaeherPatientDoctorIDArray
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Id数据集
        /// </summary>
        public string IDArray { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string KeyWord { get; set; }
    }
    /// <summary>
    /// YaeherUserDocMsg
    /// </summary>
    public class YaeherUserDocMsg
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int YaeherUserId { get; set; }
        /// <summary>
        /// 医生Id
        /// </summary>
        public int YaeherDoctorId { get; set; }
        /// <summary>
        /// 图像地址
        /// </summary>
        public virtual string UserImage { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class YaeherWecharUser : ListParameters<YaeherUser>, IPagedResultRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 医生基本信息
        /// </summary>
        public YaeherUser yaeherUser { get; set; }
        /// <summary>
        /// 移动端角色
        /// </summary>
        public List<WecharRole> wecharRoles { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
    /// <summary>
    /// 移动端角色
    /// </summary>
    public class WecharRole
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        public string SystemCode { get; set; }
        /// <summary>
        /// 角色code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class YaeherUserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 平台类型  PC or Mobile 
        /// </summary>

        public string Platform { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WecharOpenID { get; set; }
    }
}