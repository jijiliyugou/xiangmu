using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.HttpHelpers;

namespace Yaeher.Common.CloudCallCenter
{
    /// <summary>
    /// 管理阿里token值
    /// </summary>
    public class AccessToken
    {
        HttpHelper httpHelper = new HttpHelper();
        /// <summary>
        /// 第一步获取授权码
        /// </summary>
        /// <returns></returns>
        public async Task<string> AuthorizationCode()
        {
            string client_id = "4936579438986719389";
            string redirect_uri = "https://www.integraltel.com";
            string response_type = "code";
            string scope = "openid%20%2Facs%2Fccc";
            string access_type = "offline";
            String aliUrl = "https://signin.aliyun.com/oauth2/v1/auth?client_id='"+ client_id + "'&redirect_uri='"+ redirect_uri + "'&response_type='"+ response_type + "'&scope='"+ scope + "'&access_type='"+ access_type + "'";
            var AuthorizationCode =await httpHelper.PostResponseAsync(aliUrl,"");
            return "";
        }
        /// <summary>
        /// 用授权码换取访问令牌
        /// </summary>
        /// <returns></returns>
        public string Access_Token()
        {
            return "";
        }
        public string Remove_Token()
        {
            return "";
        }
    }
}
