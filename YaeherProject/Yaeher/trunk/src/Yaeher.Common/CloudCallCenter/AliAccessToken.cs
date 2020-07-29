using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.Constants;
using Yaeher.Common.HttpHelpers;
using Yaeher.Common.SendMsm;
using Yaeher.Common.SystemHelper;
using Yaeher.SystemConfig;

namespace Yaeher.Common.CloudCallCenter
{
    /// <summary>
    /// 获取alitoken值
    /// </summary>
    public class AliAccessToken
    {

        /// <summary>
        /// 通过Code获取对应的AccessToken
        /// </summary>
        /// <param name="accessTokenInfo"></param>
        /// <returns></returns>
        public async Task<AliAccessTokenEntity> AccessToken(AccessTokenInfo accessTokenInfo)
        {
            HttpHelper httpHelper = new HttpHelper();
            accessTokenInfo.code = accessTokenInfo.code;
            accessTokenInfo.client_id = "4936579438986719389";
            accessTokenInfo.client_secret = "CJqGqWVISKpVzl7oB1CPJ8EL0HZ54ERbT2PzDAfSSV9PFjrd6bRhO489ge5NmFkI";
            accessTokenInfo.redirect_uri = "https://www.integraltel.com";
            accessTokenInfo.grant_type = "authorization_code";
            string url = "http://oauth.aliyun.com/v1/token?";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(url);
            stringBuilder.Append("code=" + accessTokenInfo.code);
            stringBuilder.Append("&client_id=" + accessTokenInfo.client_id);
            stringBuilder.Append("&client_secret=" + accessTokenInfo.client_secret);
            stringBuilder.Append("&redirect_uri=" + accessTokenInfo.redirect_uri);
            stringBuilder.Append("&grant_type=" + accessTokenInfo.grant_type);
            var AliAccessTokenJson =await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
            var AliAccessToken = JsonHelper.FromJson<AliAccessTokenEntity>(AliAccessTokenJson);
            return AliAccessToken;
        }
        /// <summary>
        /// 通过refresh_token获取对应的AccessToken
        /// </summary>
        /// <param name="accessTokenInfo"></param>
        /// <returns></returns>
        public async Task<AliAccessTokenEntity> RefreshAccessToken(AccessTokenInfo accessTokenInfo)
        {
            HttpHelper httpHelper = new HttpHelper();
            accessTokenInfo.client_id = "4936579438986719389";
            accessTokenInfo.grant_type = "refresh_token";
            accessTokenInfo.client_secret = "CJqGqWVISKpVzl7oB1CPJ8EL0HZ54ERbT2PzDAfSSV9PFjrd6bRhO489ge5NmFkI";
            accessTokenInfo.refresh_token = accessTokenInfo.refresh_token;
            accessTokenInfo.redirect_uri = "/aliyun/refresh";
            string url = "http://oauth.aliyun.com/v1/token?";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(url);
            stringBuilder.Append("&client_id=" + accessTokenInfo.client_id);
            stringBuilder.Append("&grant_type=" + accessTokenInfo.grant_type);
            stringBuilder.Append("&client_secret=" + accessTokenInfo.client_secret);
            stringBuilder.Append("&refresh_token=" + accessTokenInfo.refresh_token);
            stringBuilder.Append("&redirect_uri=" + accessTokenInfo.redirect_uri);
            var AliAccessTokenJson = await httpHelper.PostResponseAsync(stringBuilder.ToString(), "");
            var AliAccessToken = JsonHelper.FromJson<AliAccessTokenEntity>(AliAccessTokenJson);
            return AliAccessToken;
        }
    }
}
