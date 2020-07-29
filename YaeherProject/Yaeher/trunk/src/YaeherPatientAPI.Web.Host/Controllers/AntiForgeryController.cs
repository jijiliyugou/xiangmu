using Microsoft.AspNetCore.Antiforgery;
using Yaeher.Controllers;

namespace YaeherPatientAPI.Web.Host.Controllers
{
    public class AntiForgeryController : YaeherControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
