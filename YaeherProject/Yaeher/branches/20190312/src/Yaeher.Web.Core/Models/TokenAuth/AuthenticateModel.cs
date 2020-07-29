using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace Yaeher.Models.TokenAuth
{
    public class AuthenticateModel
    {
       
        [StringLength(20)]
        public string UserNameOrEmailAddress { get; set; }

      
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        public string Secret { get; set; }

        /// <summary>
        /// 平台类型  PC or Mobile 
        /// </summary>

        public string Platform { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string WXCode { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string WecharOpenID { get; set; }
    }
}
