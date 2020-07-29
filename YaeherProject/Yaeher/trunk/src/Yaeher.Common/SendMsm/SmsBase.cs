using qcloudsms_csharp.httpclient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Common.SendMsm
{
   public class SmsBase
    {
        protected int appid { get; }
        protected string appkey { get; }
        protected IHTTPClient httpclient { get; set; }

        public SmsBase(int appid, string appkey, IHTTPClient httpclient)
        {
            this.appid = 1400150305;
            this.appkey = "5b458e6700046c36f5f252ae4efe0478";
            this.httpclient = httpclient;
        }

        /**
         * Handle http status error
         * @param response   raw http response
         * @return response  raw http response
         * @throws HTTPException  http status exception
         */
        public HTTPResponse handleError(HTTPResponse response)
        {
            if (response.statusCode < 200 || response.statusCode >= 300)
            {
                throw new HTTPException(response.statusCode, response.reason);
            }
            return response;
        }
    }
}
