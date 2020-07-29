using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.Constants;
using Yaeher.SystemConfig;

namespace Yaeher.Common.HttpHelpers
{
    public class HttpHelper
    {
        public ILogger Logger;
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public async Task<string> PostResponseAsync(string url, string postData)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpContent httpContent = new StringContent(postData, Encoding.UTF8,
                                     "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.ContentType.CharSet = "utf-8";

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    if (url.Contains("admin.integraltel.com")||url.Contains("admin.yaeherhealth.com")||url.Contains(Commons.WXAccessTokenUrl))  // 测试
                    {
                        return JsonHelper.FromJson<TencentTransferModel<TencentTransferResultModel>>(result).result.item;
                    }
                    else
                    {
                        return result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Info("exceptions:" + ex.Message.ToString() + ex.StackTrace.ToString());
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public async Task<string> PostResponseAsync(string url, HttpContent postData)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpContent httpContent = postData;

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Info("exceptions:" + ex.Message.ToString() + ex.StackTrace.ToString());
                return null;
            }
        }
    }
}
