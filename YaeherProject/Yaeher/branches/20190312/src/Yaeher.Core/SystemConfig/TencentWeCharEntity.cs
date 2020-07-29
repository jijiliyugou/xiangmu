using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// tencent 微信对接实体类 TencentToken
    /// </summary>
    public class TencentWeCharEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string grant_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    /// <summary>
    /// Tencent Token
    /// </summary>
    public class TencentTokens
    {
        /// <summary>
        /// TokenCode
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string expires_in { get; set; }
    }
    /// <summary>
    /// 获取微信IP
    /// </summary>
    public class WechaIP {
        /// <summary>
        /// 
        /// </summary>
        public String[] ip_list { get; set; }
    }
    /// <summary>
    /// Tencent关注者
    /// </summary>
    public class TencentFocusALLUser
    {
        /// <summary>
        /// total
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// openid数据集
        /// </summary>
        public data data { get; set; }
        /// <summary>
        /// 获取关注用户列表偏移量
        /// </summary>
        public string next_openid { get; set; }
    }

    /// <summary>
    /// 数据信息
    /// </summary>
    public class data
    {
        /// <summary>
        /// openid
        /// </summary>
        public List<string> openid { get; set; }
    }
    /// <summary>
    /// 用户token
    /// </summary>
    public class TencentUserToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class TencentFocusUser
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int subscribe { get; set; }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户的昵称       
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public int subscribe_time { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID（暂时兼容用户分组旧接口）
        /// </summary>
        public int groupid { get; set; }
        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        public List<int> tagid_list { get; set; }
        /// <summary>
        /// 返回用户关注的渠道来源，ADD_SCENE_SEARCH 公众号搜索，ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，ADD_SCENE_PROFILE_CARD 名片分享，ADD_SCENE_QR_CODE 扫描二维码，ADD_SCENEPROFILE LINK 图文页内名称点击，ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，ADD_SCENE_PAID 支付后关注，ADD_SCENE_OTHERS 其他
        /// </summary>
        public string subscribe_scene { get; set; }
        /// <summary>
        /// 二维码扫码场景（开发者自定义）
        /// </summary>
        public int qr_scene { get; set; }
        /// <summary>
        /// 二维码扫码场景描述（开发者自定义）
        /// </summary>
        public string qr_scene_str { get; set; }
    }
    /// <summary>
    /// 标签列表
    /// </summary>
    public class Tags
    {
        /// <summary>
        /// 标签
        /// </summary>
        public List<Tag> tags { get; set; }
    }
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 此标签下粉丝数
        /// </summary>
        public int count { get; set; }
    }
    public class CreateTagDetail
    {
        public TagDetail tag { get; set; }
    }
    public class TagDetail
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    /// <summary>
    /// 给用户批量打标签
    /// </summary>
    public class BatchtaggingTag
    {
        public List<string> openid_list { get; set; }
        public int tagid { get; set; }
    }
    /// <summary>
    /// 用户身上的标签列表
    /// </summary>
    public class Tagidlist
    {
        public List<int> tagid_list { get; set; }
    }
    public class ResponseMessage
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
    
    /// <summary>
    /// 标签下用户信息
    /// </summary>
    public class TencentTagUsers
    {
        public int count { get; set; }
        public data data { get; set; }
        public string next_openid { get; set; }
    }
    public class Menu
    {
        public string menuid { get; set; }
    }

    public class TencentTransferModel<TModel>
    {
        public TModel result { get; set; }
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }
    public class TencentTransferResultModel
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string item { get; set; }
    }
    /// <summary>
    /// 发送图文
    /// </summary>
    public class SendNewsMessageArticlesModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string picurl { get; set; }
    }


    #region 消息模板
    /// <summary>
    /// 
    /// </summary>
    public class TemplateItems
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TemplateInfo> template_list { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TemplateInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 订阅模板消息
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string primary_industry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deputy_industry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string example { get; set; }
    }

    /// <summary>
    /// 发送模板
    /// </summary>
    public class SendTemplate
    {
        /// <summary>
        /// 模板编号
        /// </summary>
        public string TemplateCode { get; set; }
        /// <summary>
        /// 接收者openid
        /// </summary>
        public virtual string ToUser { get; set; }
        /// <summary>
        /// 微信模板ID
        /// </summary>
        public virtual string TemplateId { get; set; }
        /// <summary>
        /// 模板跳转链接
        /// </summary>
        public virtual string BackUrl { get; set; }
        /// <summary>
        /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系，暂不支持小游戏）
        /// </summary>
        public virtual string Appid { get; set; }
        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），暂不支持小游戏
        /// </summary>
        public virtual string Pagepath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string FirstMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Keyword1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Keyword2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Keyword3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string MessageRemark { get; set; }
        /// <summary>
        /// 短信提醒 
        /// </summary>
        public virtual string SMSMessage { get; set; }
        /// <summary>
        /// 短信提醒类型
        /// </summary>
        public virtual string MessageType { get; set; }

    }

    /// <summary>
    /// 发送消息类
    /// </summary>
    public class SendMessageInfo: EntityBaseModule
    {

        /// <summary>
        /// 发送模板
        /// </summary>
        public string TemplateCode { get; set; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// 咨询单号
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 发送消息内容
        /// </summary>
        public string MessageRemark { get; set; }
        /// <summary>
        /// 评分星级  内容格式  5星
        /// </summary>
        public string EvaluateLevel { get; set; }
        /// <summary>
        /// 患者追问时间  内容格式  4小时
        /// </summary>
        public string Inquiry { get; set; }
        /// <summary>
        /// 预警时间  内容格式为 4小时
        /// </summary>
        public string WarningTime { get;set;}
    }

    /// <summary>
    /// 
    /// </summary>
    public class TemplateModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msgid { get; set; }
    }
    #endregion 
}
