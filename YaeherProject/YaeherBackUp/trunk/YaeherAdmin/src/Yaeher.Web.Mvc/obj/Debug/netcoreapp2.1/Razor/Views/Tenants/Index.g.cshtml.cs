#pragma checksum "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "98e0012c301b8ee6802e6ae47d5742e166c269ef"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Tenants_Index), @"mvc.1.0.view", @"/Views/Tenants/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Tenants/Index.cshtml", typeof(AspNetCore.Views_Tenants_Index))]
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
#line 1 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
using Abp.Authorization.Users;

#line default
#line hidden
#line 2 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
using Abp.MultiTenancy;

#line default
#line hidden
#line 3 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
using Yaeher.MultiTenancy;

#line default
#line hidden
#line 4 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
using Yaeher.Web.Startup;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98e0012c301b8ee6802e6ae47d5742e166c269ef", @"/Views/Tenants/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e58a2ec860de4ef4a9b309fcfaeedbcf6400c85", @"/Views/_ViewImports.cshtml")]
    public class Views_Tenants_Index : Yaeher.Web.Views.YaeherRazorPage<Abp.Application.Services.Dto.PagedResultDto<Yaeher.MultiTenancy.Dto.TenantDto>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/view-resources/Views/Tenants/Index.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("names", "Development", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/view-resources/Views/Tenants/Index.min.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("names", "Staging,Production", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", new global::Microsoft.AspNetCore.Html.HtmlString("tenantCreateForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("role", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-validation"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 6 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
  
    ViewBag.CurrentPageName = PageNames.Tenants; // The menu item will be active for this page.

#line default
#line hidden
            DefineSection("scripts", async() => {
                BeginContext(313, 5, true);
                WriteLiteral("\n    ");
                EndContext();
                BeginContext(318, 150, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c9295c28cc1c48db9022f197977965bc", async() => {
                    BeginContext(351, 9, true);
                    WriteLiteral("\n        ");
                    EndContext();
                    BeginContext(360, 89, false);
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4713ef98820443296a45b65ab6db968", async() => {
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 12 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
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
                    BeginContext(449, 5, true);
                    WriteLiteral("\n    ");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper.Names = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(468, 6, true);
                WriteLiteral("\n\n    ");
                EndContext();
                BeginContext(474, 161, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0c59cffce6df487590e565d8edfaa2b9", async() => {
                    BeginContext(514, 9, true);
                    WriteLiteral("\n        ");
                    EndContext();
                    BeginContext(523, 93, false);
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f7c213fedc9f4295aa3dc9329ef03392", async() => {
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_2.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
#line 16 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
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
                    BeginContext(616, 5, true);
                    WriteLiteral("\n    ");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper.Names = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(635, 1, true);
                WriteLiteral("\n");
                EndContext();
            }
            );
            BeginContext(638, 186, true);
            WriteLiteral("<div class=\"row clearfix\">\n    <div class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12\">\n        <div class=\"card\">\n            <div class=\"header\">\n                <h2>\n                    ");
            EndContext();
            BeginContext(825, 12, false);
#line 24 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
               Write(L("Tenants"));

#line default
#line hidden
            EndContext();
            BeginContext(837, 586, true);
            WriteLiteral(@"
                </h2>
                <ul class=""header-dropdown m-r--5"">
                    <li class=""dropdown"">
                        <a href=""javascript:void(0);"" class=""dropdown-toggle"" data-toggle=""dropdown"" role=""button"" aria-haspopup=""true"" aria-expanded=""false"">
                            <i class=""material-icons"">more_vert</i>
                        </a>
                        <ul class=""dropdown-menu pull-right"">
                            <li><a id=""RefreshButton"" href=""javascript:void(0);"" class=""waves-effect waves-block""><i class=""material-icons"">refresh</i>");
            EndContext();
            BeginContext(1424, 12, false);
#line 32 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                                                                  Write(L("Refresh"));

#line default
#line hidden
            EndContext();
            BeginContext(1436, 282, true);
            WriteLiteral(@"</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <div class=""body table-responsive"">
                <table class=""table"">
                    <thead>
                        <tr>
                            <th>");
            EndContext();
            BeginContext(1719, 16, false);
#line 41 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                           Write(L("TenancyName"));

#line default
#line hidden
            EndContext();
            BeginContext(1735, 38, true);
            WriteLiteral("</th>\n                            <th>");
            EndContext();
            BeginContext(1774, 9, false);
#line 42 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                           Write(L("Name"));

#line default
#line hidden
            EndContext();
            BeginContext(1783, 38, true);
            WriteLiteral("</th>\n                            <th>");
            EndContext();
            BeginContext(1822, 13, false);
#line 43 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                           Write(L("IsActive"));

#line default
#line hidden
            EndContext();
            BeginContext(1835, 93, true);
            WriteLiteral("</th>\n                        </tr>\n                    </thead>\n                    <tbody>\n");
            EndContext();
#line 47 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                         foreach (var tenant in Model.Items)
                        {

#line default
#line hidden
            BeginContext(2015, 69, true);
            WriteLiteral("                            <tr>\n                                <td>");
            EndContext();
            BeginContext(2085, 18, false);
#line 50 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                               Write(tenant.TenancyName);

#line default
#line hidden
            EndContext();
            BeginContext(2103, 42, true);
            WriteLiteral("</td>\n                                <td>");
            EndContext();
            BeginContext(2146, 11, false);
#line 51 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                               Write(tenant.Name);

#line default
#line hidden
            EndContext();
            BeginContext(2157, 67, true);
            WriteLiteral("</td>\n                                <td><i class=\"material-icons\"");
            EndContext();
            BeginWriteAttribute("style", " style=\"", 2224, "\"", 2273, 3);
            WriteAttributeValue("", 2232, "color:", 2232, 6, true);
#line 52 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
WriteAttributeValue("", 2238, tenant.IsActive ? "green":"red", 2238, 34, false);

#line default
#line hidden
            WriteAttributeValue("", 2272, ";", 2272, 1, true);
            EndWriteAttribute();
            BeginContext(2274, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(2277, 57, false);
#line 52 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                            Write(tenant.IsActive ? "check_box" : "indeterminate_check_box");

#line default
#line hidden
            EndContext();
            BeginContext(2335, 524, true);
            WriteLiteral(@"</i></td>
                                <td class=""dropdown"">
                                    <a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown"" role=""button"" aria-haspopup=""true"" aria-expanded=""false"">
                                        <i class=""material-icons"">menu</i>
                                    </a>
                                    <ul class=""dropdown-menu pull-right"">
                                        <li><a href=""#"" class=""waves-effect waves-block edit-tenant"" data-tenant-id=""");
            EndContext();
            BeginContext(2860, 9, false);
#line 58 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                                Write(tenant.Id);

#line default
#line hidden
            EndContext();
            BeginContext(2869, 87, true);
            WriteLiteral("\" data-toggle=\"modal\" data-target=\"#TenantEditModal\"><i class=\"material-icons\">edit</i>");
            EndContext();
            BeginContext(2957, 9, false);
#line 58 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                                                                                                                                 Write(L("Edit"));

#line default
#line hidden
            EndContext();
            BeginContext(2966, 129, true);
            WriteLiteral("</a></li>\n                                        <li><a href=\"#\" class=\"waves-effect waves-block delete-tenant\" data-tenant-id=\"");
            EndContext();
            BeginContext(3096, 9, false);
#line 59 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                                  Write(tenant.Id);

#line default
#line hidden
            EndContext();
            BeginContext(3105, 21, true);
            WriteLiteral("\" data-tenancy-name=\"");
            EndContext();
            BeginContext(3127, 18, false);
#line 59 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                                                                 Write(tenant.TenancyName);

#line default
#line hidden
            EndContext();
            BeginContext(3145, 44, true);
            WriteLiteral("\"><i class=\"material-icons\">delete_sweep</i>");
            EndContext();
            BeginContext(3190, 11, false);
#line 59 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                                                                                                                                                Write(L("Delete"));

#line default
#line hidden
            EndContext();
            BeginContext(3201, 124, true);
            WriteLiteral("</a></li>\n                                    </ul>\n                                </td>\n                            </tr>\n");
            EndContext();
#line 63 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                        }

#line default
#line hidden
            BeginContext(3351, 598, true);
            WriteLiteral(@"                    </tbody>
                </table>
                <button type=""button"" class=""btn btn-primary btn-circle waves-effect waves-circle waves-float pull-right"" data-toggle=""modal"" data-target=""#TenantCreateModal"">
                    <i class=""material-icons"">add</i>
                </button>
            </div>
        </div>
    </div>
</div>

<div class=""modal fade"" id=""TenantCreateModal"" tabindex=""-1"" role=""dialog"" aria-labelledby=""TenantCreateModalLabel"" data-backdrop=""static"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            ");
            EndContext();
            BeginContext(3949, 2603, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "deec63e5a9f443e4ba11db9c07d97913", async() => {
                BeginContext(4026, 119, true);
                WriteLiteral("\n                <div class=\"modal-header\">\n                    <h4 class=\"modal-title\">\n                        <span>");
                EndContext();
                BeginContext(4146, 20, false);
#line 80 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                         Write(L("CreateNewTenant"));

#line default
#line hidden
                EndContext();
                BeginContext(4166, 297, true);
                WriteLiteral(@"</span>
                    </h4>
                </div>
                <div class=""modal-body"">
                    <div class=""form-group form-float"">
                        <div class=""form-line"">
                            <input class=""form-control"" type=""text"" name=""TenancyName"" required");
                EndContext();
                BeginWriteAttribute("maxlength", " maxlength=\"", 4463, "\"", 4510, 1);
#line 86 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
WriteAttributeValue("", 4475, AbpTenantBase.MaxTenancyNameLength, 4475, 35, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4511, 70, true);
                WriteLiteral(" minlength=\"2\">\n                            <label class=\"form-label\">");
                EndContext();
                BeginContext(4582, 16, false);
#line 87 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                 Write(L("TenancyName"));

#line default
#line hidden
                EndContext();
                BeginContext(4598, 259, true);
                WriteLiteral(@"</label>
                        </div>
                    </div>
                    <div class=""form-group form-float"">
                        <div class=""form-line"">
                            <input type=""text"" name=""Name"" class=""form-control"" required");
                EndContext();
                BeginWriteAttribute("maxlength", " maxlength=\"", 4857, "\"", 4890, 1);
#line 92 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
WriteAttributeValue("", 4869, Tenant.MaxNameLength, 4869, 21, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4891, 56, true);
                WriteLiteral(">\n                            <label class=\"form-label\">");
                EndContext();
                BeginContext(4948, 9, false);
#line 93 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                 Write(L("Name"));

#line default
#line hidden
                EndContext();
                BeginContext(4957, 262, true);
                WriteLiteral(@"</label>
                        </div>
                    </div>
                    <div class=""form-group form-float"">
                        <div class=""form-line"">
                            <input type=""text"" name=""ConnectionString"" class=""form-control""");
                EndContext();
                BeginWriteAttribute("maxlength", " maxlength=\"", 5219, "\"", 5271, 1);
#line 98 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
WriteAttributeValue("", 5231, AbpTenantBase.MaxConnectionStringLength, 5231, 40, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(5272, 56, true);
                WriteLiteral(">\n                            <label class=\"form-label\">");
                EndContext();
                BeginContext(5329, 29, false);
#line 99 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                 Write(L("DatabaseConnectionString"));

#line default
#line hidden
                EndContext();
                BeginContext(5358, 2, true);
                WriteLiteral(" (");
                EndContext();
                BeginContext(5361, 13, false);
#line 99 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                 Write(L("Optional"));

#line default
#line hidden
                EndContext();
                BeginContext(5374, 274, true);
                WriteLiteral(@")</label>
                        </div>
                    </div>
                    <div class=""form-group form-float"">
                        <div class=""form-line"">
                            <input type=""email"" name=""AdminEmailAddress"" class=""form-control"" required");
                EndContext();
                BeginWriteAttribute("maxlength", " maxlength=\"", 5648, "\"", 5694, 1);
#line 104 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
WriteAttributeValue("", 5660, AbpUserBase.MaxEmailAddressLength, 5660, 34, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(5695, 56, true);
                WriteLiteral(">\n                            <label class=\"form-label\">");
                EndContext();
                BeginContext(5752, 22, false);
#line 105 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                 Write(L("AdminEmailAddress"));

#line default
#line hidden
                EndContext();
                BeginContext(5774, 299, true);
                WriteLiteral(@"</label>
                        </div>
                    </div>
                    <div class=""checkbox"">
                        <input type=""checkbox"" name=""IsActive"" value=""true"" id=""CreateTenantIsActive"" class=""filled-in"" checked />
                        <label for=""CreateTenantIsActive"">");
                EndContext();
                BeginContext(6074, 13, false);
#line 110 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                     Write(L("IsActive"));

#line default
#line hidden
                EndContext();
                BeginContext(6087, 59, true);
                WriteLiteral("</label>\n                    </div>\n                    <p>");
                EndContext();
                BeginContext(6147, 71, false);
#line 112 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                  Write(L("DefaultPasswordIs", Yaeher.Authorization.Users.User.DefaultPassword));

#line default
#line hidden
                EndContext();
                BeginContext(6218, 171, true);
                WriteLiteral("</p>\n                </div>\n                <div class=\"modal-footer\">\n                    <button type=\"button\" class=\"btn btn-default waves-effect\" data-dismiss=\"modal\">");
                EndContext();
                BeginContext(6390, 11, false);
#line 115 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                                               Write(L("Cancel"));

#line default
#line hidden
                EndContext();
                BeginContext(6401, 89, true);
                WriteLiteral("</button>\n                    <button type=\"submit\" class=\"btn btn-primary waves-effect\">");
                EndContext();
                BeginContext(6491, 9, false);
#line 116 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Tenants\Index.cshtml"
                                                                          Write(L("Save"));

#line default
#line hidden
                EndContext();
                BeginContext(6500, 45, true);
                WriteLiteral("</button>\n                </div>\n            ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("novalidate", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(6552, 288, true);
            WriteLiteral(@"
        </div>
    </div>
</div>

<div class=""modal fade"" id=""TenantEditModal"" tabindex=""-1"" role=""dialog"" aria-labelledby=""TenantEditModalLabel"" data-backdrop=""static"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">

        </div>
    </div>
</div>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Abp.Application.Services.Dto.PagedResultDto<Yaeher.MultiTenancy.Dto.TenantDto>> Html { get; private set; }
    }
}
#pragma warning restore 1591
