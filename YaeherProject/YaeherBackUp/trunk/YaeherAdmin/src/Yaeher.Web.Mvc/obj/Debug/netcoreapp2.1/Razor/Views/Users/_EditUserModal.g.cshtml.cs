#pragma checksum "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "929a088d4299bd2e3015dc0fc6ec569d22996627"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Users__EditUserModal), @"mvc.1.0.view", @"/Views/Users/_EditUserModal.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Users/_EditUserModal.cshtml", typeof(AspNetCore.Views_Users__EditUserModal))]
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
#line 1 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
using Yaeher.Web.Models.Common.Modals;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"929a088d4299bd2e3015dc0fc6ec569d22996627", @"/Views/Users/_EditUserModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e58a2ec860de4ef4a9b309fcfaeedbcf6400c85", @"/Views/_ViewImports.cshtml")]
    public class Views_Users__EditUserModal : Yaeher.Web.Views.YaeherRazorPage<Yaeher.Web.Models.Users.EditUserModalViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", new global::Microsoft.AspNetCore.Html.HtmlString("UserEditForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("role", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-validation"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/view-resources/Views/Users/_EditUserModal.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
  
    Layout = null;

#line default
#line hidden
            BeginContext(118, 98, false);
#line 6 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
Write(Html.Partial("~/Views/Shared/Modals/_ModalHeader.cshtml", new ModalHeaderViewModel(L("EditUser"))));

#line default
#line hidden
            EndContext();
            BeginContext(216, 31, true);
            WriteLiteral("\n\n<div class=\"modal-body\">\n    ");
            EndContext();
            BeginContext(247, 4226, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f701f6c9aeec45dbbe6b15189a99b60c", async() => {
                BeginContext(320, 39, true);
                WriteLiteral("\n        <input type=\"hidden\" name=\"Id\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 359, "\"", 381, 1);
#line 10 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 367, Model.User.Id, 367, 14, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(382, 165, true);
                WriteLiteral(" />\n        <ul class=\"nav nav-tabs tab-nav-right\" role=\"tablist\">\n            <li role=\"presentation\" class=\"active\"><a href=\"#edit-user-details\" data-toggle=\"tab\">");
                EndContext();
                BeginContext(548, 16, false);
#line 12 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                                             Write(L("UserDetails"));

#line default
#line hidden
                EndContext();
                BeginContext(564, 91, true);
                WriteLiteral("</a></li>\n            <li role=\"presentation\"><a href=\"#edit-user-roles\" data-toggle=\"tab\">");
                EndContext();
                BeginContext(656, 14, false);
#line 13 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                            Write(L("UserRoles"));

#line default
#line hidden
                EndContext();
                BeginContext(670, 460, true);
                WriteLiteral(@"</a></li>
        </ul>
        <div class=""tab-content"">
            <div role=""tabpanel"" class=""tab-pane animated fadeIn active"" id=""edit-user-details"">

                <div class=""row clearfix"" style=""margin-top:10px;"">
                    <div class=""col-sm-12"">
                        <div class=""form-group form-float"">
                            <div class=""form-line"">
                                <input id=""username"" type=""text"" name=""UserName""");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1130, "\"", 1158, 1);
#line 22 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 1138, Model.User.UserName, 1138, 20, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1159, 143, true);
                WriteLiteral(" required maxlength=\"32\" minlength=\"2\" class=\"validate form-control\">\n                                <label for=\"username\" class=\"form-label\">");
                EndContext();
                BeginContext(1303, 13, false);
#line 23 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                    Write(L("UserName"));

#line default
#line hidden
                EndContext();
                BeginContext(1316, 396, true);
                WriteLiteral(@"</label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class=""row clearfix"">
                    <div class=""col-sm-6"">
                        <div class=""form-group form-float"">
                            <div class=""form-line"">
                                <input id=""name"" type=""text"" name=""Name""");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1712, "\"", 1736, 1);
#line 33 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 1720, Model.User.Name, 1720, 16, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1737, 125, true);
                WriteLiteral(" required maxlength=\"32\" class=\"validate form-control\">\n                                <label for=\"name\" class=\"form-label\">");
                EndContext();
                BeginContext(1863, 9, false);
#line 34 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                Write(L("Name"));

#line default
#line hidden
                EndContext();
                BeginContext(1872, 335, true);
                WriteLiteral(@"</label>
                            </div>
                        </div>
                    </div>
                    <div class=""col-sm-6"">
                        <div class=""form-group form-float"">
                            <div class=""form-line"">
                                <input id=""surname"" type=""text"" name=""Surname""");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 2207, "\"", 2234, 1);
#line 41 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 2215, Model.User.Surname, 2215, 19, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(2235, 128, true);
                WriteLiteral(" required maxlength=\"32\" class=\"validate form-control\">\n                                <label for=\"surname\" class=\"form-label\">");
                EndContext();
                BeginContext(2364, 12, false);
#line 42 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                   Write(L("Surname"));

#line default
#line hidden
                EndContext();
                BeginContext(2376, 407, true);
                WriteLiteral(@"</label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class=""row clearfix"">
                    <div class=""col-sm-12"">
                        <div class=""form-group form-float"">
                            <div class=""form-line"">
                                <input id=""email"" type=""email"" name=""EmailAddress""");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 2783, "\"", 2815, 1);
#line 52 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 2791, Model.User.EmailAddress, 2791, 24, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(2816, 118, true);
                WriteLiteral(" maxlength=\"256\" class=\"validate form-control\">\n                                <label for=\"email\" class=\"form-label\">");
                EndContext();
                BeginContext(2935, 17, false);
#line 53 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                 Write(L("EmailAddress"));

#line default
#line hidden
                EndContext();
                BeginContext(2952, 414, true);
                WriteLiteral(@"</label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class=""row clearfix"">
                    <div class=""col-sm-12"">
                        <div class=""form-group form-float"">
                            <div class="""">
                                <input id=""IsActive"" type=""checkbox"" name=""IsActive"" value=""true"" ");
                EndContext();
                BeginContext(3368, 36, false);
#line 63 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                                              Write(Model.User.IsActive ? "checked" : "");

#line default
#line hidden
                EndContext();
                BeginContext(3405, 98, true);
                WriteLiteral(" class=\"form-control\" />\n                                <label for=\"IsActive\" class=\"form-label\">");
                EndContext();
                BeginContext(3504, 13, false);
#line 64 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                    Write(L("IsActive"));

#line default
#line hidden
                EndContext();
                BeginContext(3517, 312, true);
                WriteLiteral(@"</label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div role=""tabpanel"" class=""tab-pane animated fadeIn"" id=""edit-user-roles"">
                <div class=""row"">
                    <div class=""col-sm-12 "">
");
                EndContext();
#line 74 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                         foreach (var role in Model.Roles)
                        {

#line default
#line hidden
                BeginContext(3914, 117, true);
                WriteLiteral("                            <div class=\"col-sm-6\">\n                                <input type=\"checkbox\" name=\"role\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 4031, "\"", 4059, 1);
#line 77 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 4039, role.NormalizedName, 4039, 20, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("title", " title=\"", 4060, "\"", 4085, 1);
#line 77 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 4068, role.Description, 4068, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4086, 18, true);
                WriteLiteral(" class=\"filled-in\"");
                EndContext();
                BeginWriteAttribute("id", " id=\"", 4104, "\"", 4142, 1);
#line 77 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 4109, string.Format("role{0}",role.Id), 4109, 33, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4143, 1, true);
                WriteLiteral(" ");
                EndContext();
                BeginContext(4146, 41, false);
#line 77 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                                                                                                                               Write(Model.UserIsInRole(role) ? "checked" : "");

#line default
#line hidden
                EndContext();
                BeginContext(4188, 42, true);
                WriteLiteral(" />\n                                <label");
                EndContext();
                BeginWriteAttribute("for", " for=\"", 4230, "\"", 4269, 1);
#line 78 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 4236, string.Format("role{0}",role.Id), 4236, 33, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("title", " title=\"", 4270, "\"", 4295, 1);
#line 78 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
WriteAttributeValue("", 4278, role.DisplayName, 4278, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4296, 1, true);
                WriteLiteral(">");
                EndContext();
                BeginContext(4298, 9, false);
#line 78 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                                                                                                    Write(role.Name);

#line default
#line hidden
                EndContext();
                BeginContext(4307, 44, true);
                WriteLiteral("</label>\n                            </div>\n");
                EndContext();
#line 80 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
                        }

#line default
#line hidden
                BeginContext(4377, 89, true);
                WriteLiteral("                    </div>\n                </div>\n            </div>\n\n        </div>\n    ");
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
            BeginContext(4473, 9, true);
            WriteLiteral("\n</div>\n\n");
            EndContext();
            BeginContext(4483, 74, false);
#line 89 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
Write(Html.Partial("~/Views/Shared/Modals/_ModalFooterWithSaveAndCancel.cshtml"));

#line default
#line hidden
            EndContext();
            BeginContext(4557, 2, true);
            WriteLiteral("\n\n");
            EndContext();
            BeginContext(4559, 96, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b7b29f066f1848c585cfe7ae035447fa", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
#line 91 "D:\WorkPlace\Yaeher\trunk\YaeherAdmin\src\Yaeher.Web.Mvc\Views\Users\_EditUserModal.cshtml"
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
            BeginContext(4655, 1, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Yaeher.Web.Models.Users.EditUserModalViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
