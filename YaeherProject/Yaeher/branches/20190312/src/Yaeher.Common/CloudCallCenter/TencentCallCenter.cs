using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.HttpHelpers;
using Yaeher.SystemConfig;

namespace Yaeher.Common.CloudCallCenter
{
    /// <summary>
    /// 腾讯呼叫中心
    /// </summary>
    public class TencentCallCenter
    {
       

        public async Task<string> TencetOutCall()
        {
            TencentCCC tencentCCC = new TencentCCC();
            HttpHelper httpHelper = new HttpHelper();
            ///发起呼叫
            string OutCallurl = "http://118.25.118.91/ipcc/call/outCall";

            tencentCCC.AppId = "100007679791";
            tencentCCC.Caller = "7566114325";
            tencentCCC.Called = "18588466529";
            String test = "{\"appId\":" + tencentCCC.AppId + ",\"caller\":" + tencentCCC.Caller + ",\"called\":" + tencentCCC.Called + "}"; ;
            string ResultCode =await httpHelper.PostResponseAsync(OutCallurl, test);

            ///呼叫状态通知
            ///播放TTS
            ///转接
            ///呼叫状态通知
            ///呼叫状态通知

            return ResultCode;
        }
    }
}
