using Hangfire;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.Constants;
using Yaeher.HangFire;
using YaeherCommon;

namespace Yaeher.Common.HangfireJob
{
    public class HangfireScheduleJob
    {
        private static object objlock = new object();
        private static object delobjlock = new object();
        private static object enqueueobjlock = new object();
        public string Schedule(JobModel model)
        {
            lock (objlock)
            {
                try
                {
                    var jobid = "";
                    if (model.queues == "doctorqueue")
                    {

                        jobid = BackgroundJob.Schedule(() => PostJobResponse(model.CallbackUrl, model.CallbackContent), model.Timespan);
                    }
                    else if (model.queues == "totalqueue")
                    {
                        jobid = BackgroundJob.Schedule(() => TotalPostJobResponse(model.CallbackUrl, model.CallbackContent), model.Timespan);
                    }
                    else
                    {

                        jobid = BackgroundJob.Schedule(() => AdminPostJobResponse(model.CallbackUrl, model.CallbackContent), model.Timespan);
                    }
                    return jobid;
                }
                catch (Exception ex)
                {
                    return "error" + ex.Message.ToString();
                }
            }
        }
        public string Enqueue(JobModel model)
        {
            lock (enqueueobjlock)
            {
                var jobid = "";
                try
                {

                    jobid =BackgroundJob.Enqueue(()=>EqueueJobResponse(model.CallbackUrl,model.CallbackContent));
                }
                catch (Exception ex)
                {
                    return "error" + ex.Message.ToString();
                }
                 return jobid;
            }
        }
        public string NotifyEnqueue(JobModel model)
        {
            lock (enqueueobjlock)
            {
                var jobid = "";
                try
                {
                    jobid = BackgroundJob.Enqueue(() => PayNotifyJobResponse(model.CallbackUrl, model.CallbackContent));
                }
                catch (Exception ex)
                {
                    return "error" + ex.Message.ToString();
                }
                return jobid;
            }
        }
        public void DeleteSchedule(string JobId)
        {
            lock (delobjlock)
            {
                try
                {
                    var jobid = BackgroundJob.Delete(JobId);
                }
                catch (Exception ex)
                {
                }
            }
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        [Queue("doctorqueue")]
        [AutomaticRetry(Attempts = 2)]
        public async Task PostJobResponse(string url, string postData)
        {
            var content = JsonHelper.FromJson<HangFireJob>(postData);
            content.Secret = await GetSecret();
            var CallbackContent = JsonHelper.ToJson(content);

            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(CallbackContent, Encoding.UTF8,
                                 "application/json");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJhZG1pbiJdLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoie1xyXG4gIFwiTW9iaWxlUm9sZU5hbWVcIjogXCJhZG1pblwiLFxyXG4gIFwiSXNBZG1pblwiOiBmYWxzZSxcclxuICBcIklzRG9jdG9yXCI6IGZhbHNlLFxyXG4gIFwiSXNDdXN0b21lclNlcnZpY2VcIjogZmFsc2UsXHJcbiAgXCJJc1FDXCI6IGZhbHNlLFxyXG4gIFwiV2VjaGFyT3BlbklEXCI6IG51bGwsXHJcbiAgXCJEb2N0b3JJRFwiOiAwXHJcbn0iLCJzdWIiOiIxIiwianRpIjoiMWEwYmI0Y2YtZGY2OS00MGVkLThjNjItNzUwYTM3OTc1OTg0IiwiaWF0IjoxNTUwODAzNDk0LCJuYmYiOjE1NTA4MDM0OTQsImV4cCI6MTYzNzIwMzQ5NCwiaXNzIjoiWWFlaGVyQVBJIiwiYXVkIjoiWWFlaGVyQVBJIn0.k0lFUXTAWzFpIzsT_qqrAXmBJyPH6_PNrLAQr-c8sJ0");
            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(JsonHelper.ToJson(response));
            }

        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        [Queue("adminqueue")]
        [AutomaticRetry(Attempts = 2)]
        public async Task AdminPostJobResponse(string url, string postData)
        {
            var content = JsonHelper.FromJson<HangFireJob>(postData);
            content.Secret = await GetSecret();
            var CallbackContent = JsonHelper.ToJson(content);
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(CallbackContent, Encoding.UTF8,
                                 "application/json");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJhZG1pbiJdLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoie1xyXG4gIFwiTW9iaWxlUm9sZU5hbWVcIjogXCJhZG1pblwiLFxyXG4gIFwiSXNBZG1pblwiOiBmYWxzZSxcclxuICBcIklzRG9jdG9yXCI6IGZhbHNlLFxyXG4gIFwiSXNDdXN0b21lclNlcnZpY2VcIjogZmFsc2UsXHJcbiAgXCJJc1FDXCI6IGZhbHNlLFxyXG4gIFwiV2VjaGFyT3BlbklEXCI6IG51bGwsXHJcbiAgXCJEb2N0b3JJRFwiOiAwXHJcbn0iLCJzdWIiOiIxIiwianRpIjoiMWEwYmI0Y2YtZGY2OS00MGVkLThjNjItNzUwYTM3OTc1OTg0IiwiaWF0IjoxNTUwODAzNDk0LCJuYmYiOjE1NTA4MDM0OTQsImV4cCI6MTYzNzIwMzQ5NCwiaXNzIjoiWWFlaGVyQVBJIiwiYXVkIjoiWWFlaGVyQVBJIn0.k0lFUXTAWzFpIzsT_qqrAXmBJyPH6_PNrLAQr-c8sJ0");
            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(JsonHelper.ToJson(response));
            }
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        [Queue("totalqueue")]
        [AutomaticRetry(Attempts = 0)]
        public async Task TotalPostJobResponse(string url, string postData)
        {
            var content = JsonHelper.FromJson<HangFireJob>(postData);
            content.Secret = await GetSecret();
            var CallbackContent = JsonHelper.ToJson(content);
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(CallbackContent, Encoding.UTF8,
                                 "application/json");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJhZG1pbiJdLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoie1xyXG4gIFwiTW9iaWxlUm9sZU5hbWVcIjogXCJhZG1pblwiLFxyXG4gIFwiSXNBZG1pblwiOiBmYWxzZSxcclxuICBcIklzRG9jdG9yXCI6IGZhbHNlLFxyXG4gIFwiSXNDdXN0b21lclNlcnZpY2VcIjogZmFsc2UsXHJcbiAgXCJJc1FDXCI6IGZhbHNlLFxyXG4gIFwiV2VjaGFyT3BlbklEXCI6IG51bGwsXHJcbiAgXCJEb2N0b3JJRFwiOiAwXHJcbn0iLCJzdWIiOiIxIiwianRpIjoiMWEwYmI0Y2YtZGY2OS00MGVkLThjNjItNzUwYTM3OTc1OTg0IiwiaWF0IjoxNTUwODAzNDk0LCJuYmYiOjE1NTA4MDM0OTQsImV4cCI6MTYzNzIwMzQ5NCwiaXNzIjoiWWFlaGVyQVBJIiwiYXVkIjoiWWFlaGVyQVBJIn0.k0lFUXTAWzFpIzsT_qqrAXmBJyPH6_PNrLAQr-c8sJ0");
            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(JsonHelper.ToJson(response));
            }
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        [Queue("paynotifyqueue")]
        [AutomaticRetry(Attempts = 0)]
        public async Task PayNotifyJobResponse(string url, string postData)
        {
            var CallbackContent =postData;
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(CallbackContent, Encoding.UTF8,
                                 "application/json");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJhZG1pbiJdLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoie1xyXG4gIFwiTW9iaWxlUm9sZU5hbWVcIjogXCJhZG1pblwiLFxyXG4gIFwiSXNBZG1pblwiOiBmYWxzZSxcclxuICBcIklzRG9jdG9yXCI6IGZhbHNlLFxyXG4gIFwiSXNDdXN0b21lclNlcnZpY2VcIjogZmFsc2UsXHJcbiAgXCJJc1FDXCI6IGZhbHNlLFxyXG4gIFwiV2VjaGFyT3BlbklEXCI6IG51bGwsXHJcbiAgXCJEb2N0b3JJRFwiOiAwXHJcbn0iLCJzdWIiOiIxIiwianRpIjoiMWEwYmI0Y2YtZGY2OS00MGVkLThjNjItNzUwYTM3OTc1OTg0IiwiaWF0IjoxNTUwODAzNDk0LCJuYmYiOjE1NTA4MDM0OTQsImV4cCI6MTYzNzIwMzQ5NCwiaXNzIjoiWWFlaGVyQVBJIiwiYXVkIjoiWWFlaGVyQVBJIn0.k0lFUXTAWzFpIzsT_qqrAXmBJyPH6_PNrLAQr-c8sJ0");
            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(JsonHelper.ToJson(response));
            }
        }
        /// <summary>
        /// 退单post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        [Queue("adminqueue")]
        [AutomaticRetry(Attempts = 2)]
        public async Task EqueueJobResponse(string url, string postData)
        {
            var CallbackContent = postData;
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(CallbackContent, Encoding.UTF8,
                                 "application/json");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJhZG1pbiJdLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoie1xyXG4gIFwiTW9iaWxlUm9sZU5hbWVcIjogXCJhZG1pblwiLFxyXG4gIFwiSXNBZG1pblwiOiBmYWxzZSxcclxuICBcIklzRG9jdG9yXCI6IGZhbHNlLFxyXG4gIFwiSXNDdXN0b21lclNlcnZpY2VcIjogZmFsc2UsXHJcbiAgXCJJc1FDXCI6IGZhbHNlLFxyXG4gIFwiV2VjaGFyT3BlbklEXCI6IG51bGwsXHJcbiAgXCJEb2N0b3JJRFwiOiAwXHJcbn0iLCJzdWIiOiIxIiwianRpIjoiMWEwYmI0Y2YtZGY2OS00MGVkLThjNjItNzUwYTM3OTc1OTg0IiwiaWF0IjoxNTUwODAzNDk0LCJuYmYiOjE1NTA4MDM0OTQsImV4cCI6MTYzNzIwMzQ5NCwiaXNzIjoiWWFlaGVyQVBJIiwiYXVkIjoiWWFlaGVyQVBJIn0.k0lFUXTAWzFpIzsT_qqrAXmBJyPH6_PNrLAQr-c8sJ0");
            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(JsonHelper.ToJson(response));
            }
        }
        public Task<string> GetSecret()
        {
            var nonce = Guid.NewGuid();
            var timestamp =Commons.GetCurrentTimeStepNumber().ToString();
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
       
    }


}
