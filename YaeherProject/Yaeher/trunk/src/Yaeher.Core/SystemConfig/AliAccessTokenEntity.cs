using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 获取阿里Token值
    /// </summary>
    public class AliAccessTokenEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string request_id { get; set; }
        /// <summary>
        /// access_token 有效期为3小时 由设置  
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 刷新tokencode值 刷新token有效期为一年
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public additional_information additional_information { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expires_in { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class additional_information {
        /// <summary>
        /// 
        /// </summary>
        public string refresh_token_id { get; set; }
    }
    /// <summary>
    /// 获取Alitakon
    /// </summary>
    public class AccessTokenInfo {
        /// <summary>
        /// 第一步获取的Code
        /// </summary>
        public string code { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
        public string grant_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}
