using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Yaeher.Web.Core
{
    public class WebApiMiddleware : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var message = (from modelState in actionContext.ModelState.Values
                           where modelState.Errors.Any()
                           from error in modelState.Errors
                           select error.ErrorMessage).FirstOrDefault();
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = new BadRequestObjectResult(message);
            }
            if (actionContext.ModelState.IsValid) return;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
          //  var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();
            switch (context.Result.GetType().Name)
            {
                case "ObjectResultModule":
                    var objectResult = context.Result as ObjectResultModule;
                    //获取token action 跳过公用过滤
                    if (objectResult.StatusCode == 200)
                    {
                        //context.Result = new ObjectResult(new { Token = string.IsNullOrEmpty(authorization) ? "" : authorization.Substring(7, authorization.Length - 8), code = objectResult.StatusCode, msg = objectResult.Message, item = objectResult.Object });
                        context.Result = new ObjectResult(new { code = objectResult.StatusCode, msg = objectResult.Message, item = objectResult.Object });
                    }
                    else
                    {
                        context.Result = new ObjectResult(new { code = objectResult.StatusCode, msg = objectResult.Message, item = objectResult.Object });
                    }
                    break;
                case "EmptyResult":
                    context.Result = new ObjectResult(new { code = 404, msg = "未找到资源", item = "" });
                    break;
                //case "ContentResult":
                //    context.Result = new ObjectResult(new {  code = 200, msg = "", item = (context.Result as ContentResult).Content });
                //    break;
                case "StatusCodeResult":
                    context.Result = new ObjectResult(new { code = (context.Result as StatusCodeResult).StatusCode, msg = "", item = "" });
                    break;
                case "BadRequestObjectResult":
                    context.Result = new ObjectResult(new { code = (context.Result as BadRequestObjectResult).StatusCode, msg = (context.Result as BadRequestObjectResult).Value.ToString(), item = "" });
                    break;
            }
           
        }
    }
}
