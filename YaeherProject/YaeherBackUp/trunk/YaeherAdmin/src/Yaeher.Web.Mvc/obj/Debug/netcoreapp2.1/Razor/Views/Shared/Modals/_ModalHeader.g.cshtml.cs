#pragma checksum "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Shared\Modals\_ModalHeader.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "72c33027cdeb2cb68aa75b13f8e3a4d0cb3d8b1a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Modals__ModalHeader), @"mvc.1.0.view", @"/Views/Shared/Modals/_ModalHeader.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/Modals/_ModalHeader.cshtml", typeof(AspNetCore.Views_Shared_Modals__ModalHeader))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\_ViewImports.cshtml"
using Abp.Localization;

#line default
#line hidden
#line 1 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Shared\Modals\_ModalHeader.cshtml"
using Yaeher.Web.Models.Common.Modals;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72c33027cdeb2cb68aa75b13f8e3a4d0cb3d8b1a", @"/Views/Shared/Modals/_ModalHeader.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e58a2ec860de4ef4a9b309fcfaeedbcf6400c85", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Modals__ModalHeader : Yaeher.Web.Views.YaeherRazorPage<ModalHeaderViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(67, 160, true);
            WriteLiteral("<div class=\"modal-header\">\n    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\"></button>\n    <h4 class=\"modal-title\">\n        <span>");
            EndContext();
            BeginContext(228, 21, false);
#line 6 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Shared\Modals\_ModalHeader.cshtml"
         Write(Html.Raw(Model.Title));

#line default
#line hidden
            EndContext();
            BeginContext(249, 25, true);
            WriteLiteral("</span>\n    </h4>\n</div>\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ModalHeaderViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
