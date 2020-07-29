using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.HttpHelpers;
using Yaeher.SystemConfig;

namespace Yaeher.Common.TencentCustom
{
    public class TencentUserManage
    {
        TencentToken tencentoken = new TencentToken();
        HttpHelper httpHelper = new HttpHelper();

        /// <summary>
        /// 获取关注用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<TencentFocusALLUser> WeiXinAllUserInfoUtils(string systemToken,string NextOpenID=null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?";
            stringBuilder.Append(url);
            stringBuilder.Append("&access_token=" + systemToken);
            if (NextOpenID != null)
            {
                 stringBuilder.Append("&next_openid=" + NextOpenID);
            }
            var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
            return JsonHelper.FromJson<TencentFocusALLUser>(AccessToken);

        }

        /// <summary>
        /// 获取单个关注用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<TencentUserToken> WeiXinUserToken(string code, TencentWeCharEntity tencentWeCharEntity)
        {
            try
            {
                string grant_type = "authorization_code";
                string appid = tencentWeCharEntity.appid;
                string secret = tencentWeCharEntity.secret;
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?";
                stringBuilder.Append(url);
                stringBuilder.Append("&appid=" + appid);
                stringBuilder.Append("&secret=" + secret);
                stringBuilder.Append("&code=" + code);
                stringBuilder.Append("&grant_type=" + grant_type);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
                return JsonHelper.FromJson<TencentUserToken>(AccessToken);
            }
            catch (Exception ex)
            {
                return new TencentUserToken();
            }
        }

        /// <summary>
        /// 获取单个关注用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<TencentFocusUser> WeiXinUserInfoUtils(string openid,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/user/info?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                stringBuilder.Append("&openid=" + openid);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
                return JsonHelper.FromJson<TencentFocusUser>(AccessToken);
            }
            catch (Exception ex)
            {
                return new TencentFocusUser();
            }
        }
        /// <summary>
        /// 获取公众号已创建的标签
        /// </summary>
        /// <returns></returns>
        public async Task<Tags> WeiXinTags(string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/get?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
                return JsonHelper.FromJson<Tags>(AccessToken);
            }
            catch (Exception ex)
            {
                return new Tags();
            }
        }
        /// <summary>
        /// 获取标签下粉丝列表
        /// </summary>
        /// <returns></returns>
        public async Task<TencentTagUsers> WeiXinTagUsers(int tagid,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/user/tag/get?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"tagid\":" + tagid + ",\"next_openid\":\"\"}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<TencentTagUsers>(AccessToken);
            }
            catch (Exception ex)
            {
                return new TencentTagUsers();
            }
        }
        /// <summary>
        /// 获取公众号已创建的标签
        /// </summary>
        /// <returns></returns>
        public async Task<CreateTagDetail> CreateWeiXinTag(TagDetail TAG,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/create?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"tag\":{\"name\":\"" + TAG.name + "\"}}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<CreateTagDetail>(AccessToken);
            }
            catch (Exception ex)
            {
                return new CreateTagDetail();
            }
        }
        /// <summary>
        /// 更新公众号已创建的标签
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage> UpdateWeiXinTag(Tag TAG,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/update?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"tag\":{\"id\":" + TAG.id + ",\"name\":\"" + TAG.name + "\"}}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<ResponseMessage>(AccessToken);
            }
            catch (Exception ex)
            {
                return new ResponseMessage();
            }
        }
        
        /// <summary>
        /// 删除公众号已创建的标签
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage> DeleteWeiXinTag(Tag TAG,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/delete?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"tag\":{\"id\":" + TAG.id + "}}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<ResponseMessage>(AccessToken);
            }
            catch (Exception ex)
            {
                return new ResponseMessage();
            }
        }
        /// <summary>
        /// 批量为用户打标签
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage> WeiXinUserbatchtaggingTag(BatchtaggingTag batchtag,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"openid_list\":[";
                foreach (var item in batchtag.openid_list)
                {
                    Content += "\"" + item + "\",";
                }
                Content = Content.TrimEnd(',');
                Content += "] ,\"tagid\":" + batchtag.tagid + "}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<ResponseMessage>(AccessToken);
            }
            catch (Exception ex)
            {
                return new ResponseMessage();
            }
        }

        /// <summary>
        /// 批量删除用户标签
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseMessage> DeleteWeiXinUserTag(BatchtaggingTag batchtag,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/members/batchuntagging?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"openid_list\":[";
                foreach (var item in batchtag.openid_list)
                {
                    Content += "\"" + item + "\",";
                }
                Content = Content.TrimEnd(',');
                Content += "] ,\"tagid\":" + batchtag.tagid + "}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<ResponseMessage>(AccessToken);
            }
            catch (Exception ex)
            {
                return new ResponseMessage();
            }
        }

        /// <summary>
        /// 获取用户的标签
        /// </summary>
        /// <returns></returns>
        public async Task<Tagidlist> WeiXinUserTag(string openid,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/tags/getidlist?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"openid\":\"" + openid + "\"}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<Tagidlist>(AccessToken);
            }
            catch (Exception ex)
            {
                return new Tagidlist();
            }
        }
        /// <summary>
        /// 菜单查询接口
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public async Task<string> WeiXinMenu(string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/menu/get?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
                return AccessToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 创建个性化菜单
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public async Task<Menu> WeiXinCreateMenu(string Content,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/menu/addconditional?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<Menu>(AccessToken);
            }
            catch (Exception ex)
            {
                return new Menu();
            }
        }
        /// <summary>
        /// 删除个性才菜单
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public async Task<Menu> DeleteWeiXinMenu(string menuid,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/menu/delconditional?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"menuid\":\""+ menuid + "\"}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<Menu>(AccessToken);
            }
            catch (Exception ex)
            {
                return new Menu();
            }
        }
        /// <summary>
        /// 获取自定义菜单配置接口 
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public async Task<Menu> WeiXinselfMenu(string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
                return JsonHelper.FromJson<Menu>(AccessToken);
            }
            catch (Exception ex)
            {
                return new Menu();
            }
        }

        

        #region 微信消息模板
        /// <summary>
        /// 获取消息模板List
        /// </summary>
        /// <returns></returns>
        public async Task<TemplateItems> WeCharTemplateList(string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
                return JsonHelper.FromJson<TemplateItems>(AccessToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取消息模板List
        /// </summary>
        /// <returns></returns>
        public async Task<TemplateInfo> DeleteWeCharTemplate(string TemplateId,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var Content = "{\"template_id\":\"" + TemplateId + "\"}";
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<TemplateInfo>(AccessToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取消息模板List
        /// </summary>
        /// <returns></returns>
        public async Task<TemplateModel> SendWeCharTemplate(String Content,string systemToken)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?";
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken);
                var AccessToken = await httpHelper.PostResponseAsync(stringBuilder.ToString(), Content);
                return JsonHelper.FromJson<TemplateModel>(AccessToken);
            }
            catch (Exception ex)
            {
                TemplateModel templateModel = new TemplateModel();
                int length = ex.ToString().Length;
                if (length > 500)
                {
                    length = 499;
                }
                templateModel.errmsg = ex.ToString().Substring(0,length);
                templateModel.errcode = "error";
                return templateModel;
            }
        }
        #endregion

        #region 微信分账
        //public async Task<PostResultModel> Profitsharingaddreceiver(CreateReceiver model)
        //{
        //    try
        //    {
        //        StringBuilder stringBuilder = new StringBuilder();
        //        string url = "https://api.mch.weixin.qq.com/pay/profitsharingaddreceiver/";
        //        var Content =JsonHelper.ToJson(model);
        //        var AccessToken = await httpHelper.PostResponseAsync(url, Content);
        //        return JsonHelper.FromJson<PostResultModel>(AccessToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new PostResultModel();
        //    }
        //}
        #endregion
        #region 检测用户标签
        /// <summary>
        /// 检测用户标签
        /// </summary>
        /// <param name="tencentFocusUser"></param>
        /// <param name="yaeherUser"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public async Task<YaeherUser> YaeherUserLable(TencentFocusUser tencentFocusUser, YaeherUser yaeherUser, string access_token)
        {
            TencentUserManage usermanage = new TencentUserManage();
            // 检查用户标签功能
            if (tencentFocusUser.tagid_list != null && tencentFocusUser.tagid_list.Count > 0)
            {
                bool IsExis = false;
                if (tencentFocusUser.tagid_list.Exists(a => a == yaeherUser.WecharLableId))
                {
                    IsExis = true;
                    // 剔除已存在的标签 
                    tencentFocusUser.tagid_list.Remove(yaeherUser.WecharLableId);
                }
                //  将不是系统存在的标签删除
                if (tencentFocusUser.tagid_list.Count > 0)
                {
                    foreach (var item in tencentFocusUser.tagid_list)
                    {
                        BatchtaggingTag batchtagging1 = new BatchtaggingTag();
                        batchtagging1.openid_list = new List<string>();
                        batchtagging1.openid_list.Add(yaeherUser.WecharOpenID);
                        batchtagging1.tagid = item;
                        var responsemsg1 = await usermanage.DeleteWeiXinUserTag(batchtagging1, access_token);
                    }
                }
                if (!IsExis)
                {
                    BatchtaggingTag batchtagging = new BatchtaggingTag();
                    batchtagging.openid_list = new List<string>();
                    batchtagging.openid_list.Add(yaeherUser.WecharOpenID);
                    batchtagging.tagid = yaeherUser.WecharLableId;
                    var responsemsg = await usermanage.WeiXinUserbatchtaggingTag(batchtagging, access_token);
                }
            }
            else  // 当没标签时将数据回复
            {
                BatchtaggingTag batchtagging = new BatchtaggingTag();
                batchtagging.openid_list = new List<string>();
                batchtagging.openid_list.Add(yaeherUser.WecharOpenID);
                batchtagging.tagid = yaeherUser.WecharLableId;
                var responsemsg = await usermanage.WeiXinUserbatchtaggingTag(batchtagging, access_token);
            }
            return yaeherUser;
        }
        #endregion
    }
}
