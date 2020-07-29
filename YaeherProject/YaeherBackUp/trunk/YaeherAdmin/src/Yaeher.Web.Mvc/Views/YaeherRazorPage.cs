using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace Yaeher.Web.Views
{
    public abstract class YaeherRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected YaeherRazorPage()
        {
            LocalizationSourceName = YaeherConsts.LocalizationSourceName;
        }
    }
}
