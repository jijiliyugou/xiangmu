using Yaeher.SystemConfig;

namespace Yaeher.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
        public string WecharOpenID { get; set; }
        public string MobileRoleName { get; set; }
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public UserManager userManager { get; set; }
    }
}
