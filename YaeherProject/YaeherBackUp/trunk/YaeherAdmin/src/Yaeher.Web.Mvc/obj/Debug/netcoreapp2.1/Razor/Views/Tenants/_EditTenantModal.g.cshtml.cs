#pragma checksum "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4d9f53c7ec2f320a5017d5adfdb9c1f440b019fd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Tenants__EditTenantModal), @"mvc.1.0.view", @"/Views/Tenants/_EditTenantModal.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Tenants/_EditTenantModal.cshtml", typeof(AspNetCore.Views_Tenants__EditTenantModal))]
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
#line 1 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
using Abp.MultiTenancy;

#line default
#line hidden
#line 2 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
using Yaeher.MultiTenancy.Dto;

#line default
#line hidden
#line 3 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
using Yaeher.Web.Models.Common.Modals;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4d9f53c7ec2f320a5017d5adfdb9c1f440b019fd", @"/Views/Tenants/_EditTenantModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e58a2ec860de4ef4a9b309fcfaeedbcf6400c85", @"/Views/_ViewImports.cshtml")]
    public class Views_Tenants__EditTenantModal : Yaeher.Web.Views.YaeherRazorPage<TenantDto>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", new global::Microsoft.AspNetCore.Html.HtmlString("TenantEditForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("role", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-validation"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/view-resources/Views/Tenants/_EditTenantModal.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 5 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
  
    Layout = null;

#line default
#line hidden
            BeginContext(136, 100, false);
#line 8 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
Write(Html.Partial("~/Views/Shared/Modals/_ModalHeader.cshtml", new ModalHeaderViewModel(L("EditTenant"))));

#line default
#line hidden
            EndContext();
            BeginContext(236, 31, true);
            WriteLiteral("\n\n<div class=\"modal-body\">\n    ");
            EndContext();
            BeginContext(267, 1083, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3d9b8db734424a91a3866583af2009e8", async() => {
                BeginContext(342, 39, true);
                WriteLiteral("\n        <input type=\"hidden\" name=\"Id\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 381, "\"", 398, 1);
#line 12 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
WriteAttributeValue("", 389, Model.Id, 389, 9, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(399, 157, true);
                WriteLiteral("/>\n        <div class=\"form-group form-float\">\n            <div class=\"form-line\">\n                <input class=\"form-control\" type=\"text\" name=\"TenancyName\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 556, "\"", 582, 1);
#line 15 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
WriteAttributeValue("", 564, Model.TenancyName, 564, 18, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(583, 82, true);
                WriteLiteral(" required maxlength=\"64\" minlength=\"2\">\n                <label class=\"form-label\">");
                EndContext();
                BeginContext(666, 16, false);
#line 16 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
                                     Write(L("TenancyName"));

#line default
#line hidden
                EndContext();
                BeginContext(682, 190, true);
                WriteLiteral("</label>\n            </div>\n        </div>\n        <div class=\"form-group form-float\">\n            <div class=\"form-line\">\n                <input type=\"text\" name=\"Name\" class=\"form-control\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 872, "\"", 891, 1);
#line 21 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
WriteAttributeValue("", 880, Model.Name, 880, 11, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(892, 69, true);
                WriteLiteral(" required maxlength=\"128\">\n                <label class=\"form-label\">");
                EndContext();
                BeginContext(962, 9, false);
#line 22 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
                                     Write(L("Name"));

#line default
#line hidden
                EndContext();
                BeginContext(971, 196, true);
                WriteLiteral("</label>\n            </div>\n        </div>\n        <div class=\"form-group form-float\">\n            <div class=\"\">\n                <input id=\"isactive\" type=\"checkbox\" name=\"IsActive\" value=\"true\" ");
                EndContext();
                BeginContext(1169, 31, false);
#line 27 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
                                                                              Write(Model.IsActive ? "checked" : "");

#line default
#line hidden
                EndContext();
                BeginContext(1201, 81, true);
                WriteLiteral(" class=\"form-control\"/>\n                <label for=\"isactive\" class=\"form-label\">");
                EndContext();
                BeginContext(1283, 13, false);
#line 28 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
                                                    Write(L("IsActive"));

#line default
#line hidden
                EndContext();
                BeginContext(1296, 47, true);
                WriteLiteral("</label>\n            </div>\n        </div>\n    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("novalidate", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1350, 9, true);
            WriteLiteral("\n</div>\n\n");
            EndContext();
            BeginContext(1360, 74, false);
#line 34 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
Write(Html.Partial("~/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml"));

#line default
#line hidden
            EndContext();
            BeginContext(1434, 2, true);
            WriteLiteral("\n\n");
            EndContext();
            BeginContext(1436, 100, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "354ad9c65dc6443e8f421facdd804670", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
#line 36 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\_EditTenantModal.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1536, 1, true);
            WriteLiteral("\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TenantDto> Html { get; private set; }
    }
}
#pragma warning restore 1591
