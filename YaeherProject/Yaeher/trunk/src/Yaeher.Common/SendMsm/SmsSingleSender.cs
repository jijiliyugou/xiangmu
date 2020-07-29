﻿using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.Common.SendMsm
{
    public class SmsSingleSender:SmsBase
    {
        private string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms";

        public SmsSingleSender(int appid, string appkey)
            : base(appid, appkey, new DefaultHTTPClient())
        {

        }

        public SmsSingleSender(int appid, string appkey, IHTTPClient httpclient)
            : base(appid, appkey, httpclient)
        { }
        
        /// <summary>
        /// Send single SMS message.
        /// </summary>
        /// <param name="type">SMS message type, Enum{0: normal SMS, 1: marketing SMS}</param>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="msg">SMS message content< /param>
        /// <param name="extend">extend field, default is empty string</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsSingleSenderResult</returns>
        public SmsSingleSenderResult send(int type, string nationCode, string phoneNumber,string msg, string extend, string ext)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", (new JSONObjectBuilder()).Put("nationcode", nationCode).Put("mobile", phoneNumber).Build())
                .Put("type", type)
                .Put("msg", msg)
                .Put("sig", SmsSenderUtil.calculateSignature(this.appkey, random, now, phoneNumber))
                .Put("time", now)
                .Put("extend", !String.IsNullOrEmpty(extend) ? extend : "")
                .Put("ext", !String.IsNullOrEmpty(ext) ? ext : "");
            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Conetent-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(body.Build().ToString());
            // May throw HttpRequestException
            HTTPResponse res =httpclient.fetch(req);
            // May throw HTTPException
            handleError(res);
            SmsSingleSenderResult result = new SmsSingleSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);
            return result;
        }

        /// <summary> 
        /// Send single SMS message with template paramters.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="templateId">template id</param>
        /// <param name="parameters">template parameters</param>
        /// <param name="sign">Sms user sign</param>
        /// <param name="extend">extend field, default is empty string</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsSingleSenderResult</returns>
        public SmsSingleSenderResult SendMessage(TencentMessage tencentMessage)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", (new JSONObjectBuilder()).Put("nationcode", tencentMessage.NationCode).Put("mobile", tencentMessage.PhoneNumber).Build())
                .Put("sig", SmsSenderUtil.calculateSignature(appkey, random, now, tencentMessage.PhoneNumber))
                .Put("tpl_id", tencentMessage.TemplateId)
                .PutArray("params", tencentMessage.Parameters)
                .Put("sign", !String.IsNullOrEmpty(tencentMessage.Sign) ? tencentMessage.Sign : "")
                .Put("time", now)
                .Put("extend", !String.IsNullOrEmpty(tencentMessage.Extend) ? tencentMessage.Extend : "")
                .Put("ext", !String.IsNullOrEmpty(tencentMessage.Ext) ? tencentMessage.Ext : "");
            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Conetent-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(body.Build().ToString());
            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);
            // May throw HTTPException
            handleError(res);
            SmsSingleSenderResult result = new SmsSingleSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);
            return result;
        }
    }
}
