using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Yaeher.Controllers
{
    public abstract class YaeherControllerBase: AbpController
    {
        public ObjectResultModule ObjectResultModule { get; set; }
        protected YaeherControllerBase()
        {
            LocalizationSourceName = YaeherConsts.LocalizationSourceName;
            ObjectResultModule = new ObjectResultModule(new object(), 200, "success");
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        /// <summary>
        /// 构造Secret
        /// </summary>
        /// <returns></returns>
        protected virtual Task<string> CreateSecret()
        {
            var nonce = Guid.NewGuid();
            var timestamp = GetCurrentTimeStepNumber().ToString();
            //var dataparams = ValidHelper.GetSecretParams(new SecretModels
            //{
            //    apptype = "Web",
            //    nonce = nonce.ToString(),
            //    timestamp = timestamp
            //});
            //var token = ValidHelper.GetSignature(dataparams);
            var token =
                ValidHelper.GetSignature(
                    ValidHelper.GetSecretParams(new SecretModels
                    {
                        apptype = "system",
                        nonce = nonce.ToString(),
                        timestamp = timestamp
                    }));
            var secret =
                $"timestamp={timestamp}&nonce={nonce.ToString()}&apptype={"system"}&signature={token}";
            return Task.FromResult(ValidHelper.Base64Code(secret));
        }
        /// <summary>
        /// 获取当前实例的日期和时间的计时周期数(Ticks表示一百纳秒的一千万分之一)
        /// </summary>
        /// <returns></returns>
        protected static int GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - _unixEpoch;
            return (int)delta.TotalSeconds;
        }
        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        protected virtual async Task<string> PostResponseAsync(string url, string postData)
        {
            try
            {
                //Logger.Info("url:" + url + "content:" + postData);
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpContent httpContent = new StringContent(postData, Encoding.UTF8,
                                     "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJhZG1pbiJdLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoie1xyXG4gIFwiTW9iaWxlUm9sZU5hbWVcIjogXCJhZG1pblwiLFxyXG4gIFwiSXNBZG1pblwiOiBmYWxzZSxcclxuICBcIklzRG9jdG9yXCI6IGZhbHNlLFxyXG4gIFwiSXNDdXN0b21lclNlcnZpY2VcIjogZmFsc2UsXHJcbiAgXCJJc1FDXCI6IGZhbHNlLFxyXG4gIFwiV2VjaGFyT3BlbklEXCI6IG51bGwsXHJcbiAgXCJEb2N0b3JJRFwiOiAwXHJcbn0iLCJzdWIiOiIxIiwianRpIjoiMWEwYmI0Y2YtZGY2OS00MGVkLThjNjItNzUwYTM3OTc1OTg0IiwiaWF0IjoxNTUwODAzNDk0LCJuYmYiOjE1NTA4MDM0OTQsImV4cCI6MTYzNzIwMzQ5NCwiaXNzIjoiWWFlaGVyQVBJIiwiYXVkIjoiWWFlaGVyQVBJIn0.k0lFUXTAWzFpIzsT_qqrAXmBJyPH6_PNrLAQr-c8sJ0");
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    //Logger.Info("result:" + result);
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
