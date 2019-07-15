#pragma checksum "H:\Omid\prj\Sample\AspNetTemplate.Presentation\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "496d43022170c9425bebe562e3eca7fe07bcd7a0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 2 "H:\Omid\prj\Sample\AspNetTemplate.Presentation\Views\Home\Index.cshtml"
using System.Security.Claims;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"496d43022170c9425bebe562e3eca7fe07bcd7a0", @"/Views/Home/Index.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AspNetTemplate.ClientEntity.ViewModel.IndexViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/pages/home/index.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "H:\Omid\prj\Sample\AspNetTemplate.Presentation\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home page";
    var UserRole = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();

#line default
#line hidden
            DefineSection("Scripts", async() => {
                BeginContext(266, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(272, 48, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "496d43022170c9425bebe562e3eca7fe07bcd7a03625", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(320, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
            BeginContext(325, 158, true);
            WriteLiteral("<div class=\"container text-center\">\r\n    <h3 class=\"display-4 mb-5\">Welcome to Manage Expense System</h3>\r\n\r\n    <div class=\"row justify-content-md-center\">\r\n");
            EndContext();
#line 14 "H:\Omid\prj\Sample\AspNetTemplate.Presentation\Views\Home\Index.cshtml"
         if (!User.Identity.IsAuthenticated)
        {

#line default
#line hidden
            BeginContext(540, 1050, true);
            WriteLiteral(@"            <div class=""col col-md-6 col-sm-12"">
                <form>
                    <div class=""form-group"">
                        <label for=""email"" class=""float-left"">Email address</label>
                        <input type=""email"" name=""email"" class=""form-control"" id=""email"" aria-describedby=""emailHelp"" placeholder=""Enter email"">
                        <small id=""emailHelp"" class=""form-text text-muted"">Team lead account email: sam@test.com</small>
                    </div>
                    <div class=""form-group"">
                        <label for=""password"" class=""float-left"">Password</label>
                        <input type=""password"" name=""password"" class=""form-control"" id=""password"" placeholder=""Password"">
                        <small id=""emailHelp"" class=""form-text text-muted"">Team lead account password: sam@test.com</small>
                    </div>
                    <button id=""btn-login"" type=""submit"" class=""btn btn-primary"">Login</button>
                </for");
            WriteLiteral("m>\r\n\r\n            </div>\r\n");
            EndContext();
#line 32 "H:\Omid\prj\Sample\AspNetTemplate.Presentation\Views\Home\Index.cshtml"
        }
        else
        {
            if (UserRole == "TeamLead")
            {


#line default
#line hidden
            BeginContext(1684, 826, true);
            WriteLiteral(@"                <div class=""col-md-12 text-left"">
                    <div class=""card"">
                        <h5 class=""card-header"">Adding User Note</h5>
                        <div class=""card-body"">
                            <h5 class=""card-title"">Adding User</h5>
                            <p class=""card-text"">Please add employee user and finance user through ""Add User"" menu or button below. Then you can use those account to login and test the system.<br />
                                <b>For finance user use a valid email address because we send notification email to finance user.</b>
                            </p>
                            <a href=""/account/adduser"" class=""btn btn-primary"">Add User</a>
                        </div>
                    </div>
                </div>
");
            EndContext();
            BeginContext(2516, 593, true);
            WriteLiteral(@"                <div class=""col-md-12 mt-1 text-left"">
                    <div class=""card"">
                        <h5 class=""card-header"">Manage Expenses Note</h5>
                        <div class=""card-body"">
                            <h5 class=""card-title"">Managing Expenses</h5>
                            <p>For manage expenses use ""Manage Expenses"" menu button or button below.</p>
                            <a href=""/account/manageexpenses"" class=""btn btn-primary"">Manage Expenses</a>
                        </div>
                    </div>
                </div>
");
            EndContext();
#line 63 "H:\Omid\prj\Sample\AspNetTemplate.Presentation\Views\Home\Index.cshtml"



            }
        }

#line default
#line hidden
            BeginContext(3141, 20, true);
            WriteLiteral("    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AspNetTemplate.ClientEntity.ViewModel.IndexViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
