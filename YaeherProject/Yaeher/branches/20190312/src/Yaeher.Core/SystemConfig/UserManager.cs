using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public YaeherUser YaeherUserInfo { get; set; }
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public YaeherDoctor YaeherDoctorInfo { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<YaeherRole> RoleList { get; set; }
        /// <summary>
        /// 用户菜单
        /// </summary>
        public List<YaeherModuleNode> ModuleList { get; set; }
        /// <summary>
        /// 移动端角色
        /// </summary>
        public String MobileRoleName { get; set; }
        /// <summary>
        /// PC端是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// PC端是否医生
        /// </summary>
        public bool IsDoctor { get; set; }
        /// <summary>
        /// PC端是否客服
        /// </summary>
        public bool IsCustomerService { get; set; }
        /// <summary>
        /// PC端是否质控
        /// </summary>
        public bool IsQC { get; set; }
    }
    /// <summary>
    /// 登陆缓存
    /// </summary>
    public class UserMemory
    {
        /// <summary>
        /// 移动端角色
        /// </summary>
        public String MobileRoleName { get; set; }
        /// <summary>
        /// PC端是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// PC端是否医生
        /// </summary>
        public bool IsDoctor { get; set; }
        /// <summary>
        /// PC端是否客服
        /// </summary>
        public bool IsCustomerService { get; set; }
        /// <summary>
        /// PC端是否质控
        /// </summary>
        public bool IsQC { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string WecharOpenID{ get; set; }
        /// <summary>
        /// doctorid
        /// </summary>
        public int DoctorID{ get; set; }
    }
    /// <summary>
    /// 医生基本信息
    /// </summary>
    public class YaeherDoctorInfo
    {
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public YaeherUser YaeherUserInfo { get; set; }
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public YaeherDoctor DoctorInfo { get; set; }
        /// <summary>
        /// 医生与标签关系 
        /// </summary>
        public List<DoctorRelation> DoctorRelationList { get; set; }
        /// <summary>
        /// 医生提供服务
        /// </summary>
        public List<ServiceMoneyList> ServiceMoneyLists { get; set; }
        /// <summary>
        /// 医生支付方式
        /// </summary>
        public List<YaeherUserPayment> YaeherUserPayment { get; set; }
        /// <summary>
        /// 标签 疾病类型
        /// </summary>
        public List<LableManage> LableManageList { get; set; }
        /// <summary>
        /// InquiryMaxCount
        /// </summary>
        public SystemParameter InquiryMaxCount { get; set; }
        /// <summary>
        /// WxPayBusinessId
        /// </summary>
        public SystemParameter WxPayBusinessId { get; set; }
        /// <summary>
        /// ConsultationSucessTime
        /// </summary>
        public SystemParameter ConsultationSucessTime { get; set; }
        /// <summary>
        /// ReplyMinutesTime
        /// </summary>
        public SystemParameter ReplyMinutesTime { get; set; }
        /// <summary>
        /// 医生上下线信息
        /// </summary>
        public DoctorOnlineRecord DoctorOnlineRecord { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class YaeherModuleNode
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public virtual string Names { get; set; }
        /// <summary>
        /// LinkUrl
        /// </summary>
        public virtual string LinkUrls { get; set; }
        /// <summary>
        /// Area
        /// </summary>
        public virtual string Areas { get; set; }
        /// <summary>
        /// Controller
        /// </summary>
        public virtual string Controllers { get; set; }
        /// <summary>
        /// Actions
        /// </summary>
        public virtual string Actionss { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public virtual string Icons { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public virtual string Codes { get; set; }
        /// <summary>
        /// OrderSort
        /// </summary>
        public virtual int OrderSort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 是否菜单  当选择不是菜单时  ParentId为0
        /// </summary>
        public virtual bool IsMenu { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<YaeherModuleNode> children { get; set; }

    }
}
