using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.HttpHelpers;
using Yaeher.SystemConfig;

namespace Yaeher.Common.TencentCustom
{
    /// <summary>
    /// 获取TencentToken
    /// </summary>
    public class TencentToken
    {
        HttpHelper httpHelper = new HttpHelper();
        /// <summary>
        /// TencentToken
        /// </summary>
        /// <returns></returns>
        public async Task<TencentTokens> TencentAccessToken(SystemToken systemToken)
        {
            try
            {
                string grant_type = "client_credential";
                string appid = systemToken.Appid;
                string secret = systemToken.AppSecret;
                string url = "https://api.weixin.qq.com/cgi-bin/token?";
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(url);
                stringBuilder.Append("&grant_type=" + grant_type);
                stringBuilder.Append("&appid=" + appid);
                stringBuilder.Append("&secret=" + secret);
                string Url = "http://admin."+systemToken.YaeherPlatform+".com/api/TestWechar";   // 拼拼接访问url
                var body = "{\"url\":\"" + stringBuilder.ToString() + "\",\"content\":\"\"}";
                var AccessToken = await httpHelper.PostResponseAsync(Url, body);
                return JsonHelper.FromJson<TencentTokens>(AccessToken);
            }
            catch (Exception ex)
            {
                return new TencentTokens();
            }
        }
        /// <summary>
        /// TencentToken
        /// </summary>
        /// <returns></returns>
        public async Task<JsApiTicketResult> TencentTicket(string type, string systemToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}",
                                        systemToken, type);
            JsApiTicketResult result = await Senparc.CO2NET.HttpUtility.Get.GetJsonAsync<JsApiTicketResult>(url);
            return result;
        }

        /// <summary>
        /// 获取微信IP地址
        /// </summary>
        /// <param name="systemToken"></param>
        /// <returns></returns>
        public async Task<WechaIP> WeCharIP(SystemToken systemToken)
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/getcallbackip?";
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(url);
                stringBuilder.Append("&access_token=" + systemToken.access_token);
                string Url = "http://admin." + systemToken.YaeherPlatform + ".com/api/TestWechar";   // 拼拼接访问url
                var body = "{\"url\":\"" + stringBuilder.ToString() + "\",\"content\":\"\"}";
                var AccessToken = await httpHelper.PostResponseAsync(Url, body);
                return JsonHelper.FromJson<WechaIP>(AccessToken);
            }
            catch (Exception ex)
            {
                return new WechaIP();
            }
        }
    }
}
