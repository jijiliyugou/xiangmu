using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Yaeher.Controllers;

namespace Yaeher.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : YaeherControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
