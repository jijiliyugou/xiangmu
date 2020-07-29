using Abp.AspNetCore.Mvc.ViewComponents;

namespace Yaeher.Web.Views
{
    public abstract class YaeherViewComponent : AbpViewComponent
    {
        protected YaeherViewComponent()
        {
            LocalizationSourceName = YaeherConsts.LocalizationSourceName;
        }
    }
}
