#pragma checksum "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "94164f29bb62130101ca3e32cc61e94af7ed4a6b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_RegisterResult), @"mvc.1.0.view", @"/Views/Account/RegisterResult.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Account/RegisterResult.cshtml", typeof(AspNetCore.Views_Account_RegisterResult))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"94164f29bb62130101ca3e32cc61e94af7ed4a6b", @"/Views/Account/RegisterResult.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e58a2ec860de4ef4a9b309fcfaeedbcf6400c85", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_RegisterResult : Yaeher.Web.Views.YaeherRazorPage<Yaeher.Web.Models.Account.RegisterResultViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
  
    ViewBag.Title = L("SuccessfullyRegistered");

#line default
#line hidden
            BeginContext(111, 54, true);
            WriteLiteral("<div class=\"card\">\n    <div class=\"body\">\n        <h4>");
            EndContext();
            BeginContext(166, 27, false);
#line 7 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
       Write(L("SuccessfullyRegistered"));

#line default
#line hidden
            EndContext();
            BeginContext(193, 60, true);
            WriteLiteral("</h4>\n        <ul>\n            <li><span class=\"text-muted\">");
            EndContext();
            BeginContext(254, 16, false);
#line 9 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                    Write(L("NameSurname"));

#line default
#line hidden
            EndContext();
            BeginContext(270, 9, true);
            WriteLiteral(":</span> ");
            EndContext();
            BeginContext(280, 20, false);
#line 9 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                                              Write(Model.NameAndSurname);

#line default
#line hidden
            EndContext();
            BeginContext(300, 47, true);
            WriteLiteral("</li>\n            <li><span class=\"text-muted\">");
            EndContext();
            BeginContext(348, 16, false);
#line 10 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                    Write(L("TenancyName"));

#line default
#line hidden
            EndContext();
            BeginContext(364, 9, true);
            WriteLiteral(":</span> ");
            EndContext();
            BeginContext(374, 17, false);
#line 10 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                                              Write(Model.TenancyName);

#line default
#line hidden
            EndContext();
            BeginContext(391, 47, true);
            WriteLiteral("</li>\n            <li><span class=\"text-muted\">");
            EndContext();
            BeginContext(439, 13, false);
#line 11 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                    Write(L("UserName"));

#line default
#line hidden
            EndContext();
            BeginContext(452, 9, true);
            WriteLiteral(":</span> ");
            EndContext();
            BeginContext(462, 14, false);
#line 11 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                                           Write(Model.UserName);

#line default
#line hidden
            EndContext();
            BeginContext(476, 47, true);
            WriteLiteral("</li>\n            <li><span class=\"text-muted\">");
            EndContext();
            BeginContext(524, 17, false);
#line 12 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                    Write(L("EmailAddress"));

#line default
#line hidden
            EndContext();
            BeginContext(541, 9, true);
            WriteLiteral(":</span> ");
            EndContext();
            BeginContext(551, 18, false);
#line 12 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
                                                               Write(Model.EmailAddress);

#line default
#line hidden
            EndContext();
            BeginContext(569, 34, true);
            WriteLiteral("</li>\n        </ul>\n        <div>\n");
            EndContext();
#line 15 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
             if (!Model.IsActive)
            {

#line default
#line hidden
            BeginContext(651, 83, true);
            WriteLiteral("                <div class=\"alert alert-warning\" role=\"alert\">\n                    ");
            EndContext();
            BeginContext(735, 32, false);
#line 18 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
               Write(L("WaitingForActivationMessage"));

#line default
#line hidden
            EndContext();
            BeginContext(767, 24, true);
            WriteLiteral("\n                </div>\n");
            EndContext();
#line 20 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
            }

#line default
#line hidden
            BeginContext(805, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 22 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
             if (Model.IsEmailConfirmationRequiredForLogin && !Model.IsEmailConfirmed)
            {

#line default
#line hidden
            BeginContext(907, 83, true);
            WriteLiteral("                <div class=\"alert alert-warning\" role=\"alert\">\n                    ");
            EndContext();
            BeginContext(991, 30, false);
#line 25 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
               Write(L("WaitingForEmailActivation"));

#line default
#line hidden
            EndContext();
            BeginContext(1021, 24, true);
            WriteLiteral("\n                </div>\n");
            EndContext();
#line 27 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Account\RegisterResult.cshtml"
            }

#line default
#line hidden
            BeginContext(1059, 33, true);
            WriteLiteral("        </div>\n    </div>\n</div>\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Yaeher.Web.Models.Account.RegisterResultViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591