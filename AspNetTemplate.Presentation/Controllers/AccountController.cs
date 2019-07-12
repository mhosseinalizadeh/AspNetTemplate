using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetTemplate.ApplicationService.UserService;
using AspNetTemplate.ClientEntity.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AspNetTemplate.ApplicationService;
using AspNetTemplate.DomainEntity;
using Microsoft.AspNetCore.Authentication;
using AspNetTemplate.ApplicationService.AccountService;
using AspNetTemplate.DataAccess.UnitOfWork;
using AspNetTemplate.ClientEntity.ViewModel;
using AspNetTemplate.CommonService;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace AspNetTemplate.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService localizationService;


        public AccountController(IUserService userService,
            IAccountService accountService,
            IUnitOfWork unitOfWork,
            ILocalizationService _localizer)
        {
            _userService = userService;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        [Route("/account/login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginModel)
        {
            var result = await _userService.CheckPassword(loginModel);

            if (result.Status == ServiceResultStatus.Exception)
                return Json(result);
                        
            var user = (User)result.Result;
            var principal = getPrincipal(user);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            user.Password = null;
            return Json(new ServiceResult(ServiceResultStatus.Success, user));
        }

        [Route("/account/logout")]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync();
            Response.Redirect("/");
            return Json(null);
        }

        [Route("/account/addexpense")]
        public IActionResult AddExpense() {
            return View();
        }

        [HttpPost]
        [Route("/account/addexpense")]
        public async Task<IActionResult> AddExpense(ExpenseUploadDto model)
        {
            if (!ModelState.IsValid) {
                return View(new ServiceResult(ServiceResultStatus.Exception, null, getErrorMessages(ModelState)));
            }

            var userid = GetUserId();
            if (userid != 0)
                model.UserId = userid;

            ServiceResult result = await _accountService.AddExpense(model);

            _unitOfWork.Commit();
            return View(result);
        }

        [Route("/account/viewexpenses")]
        public async Task<IActionResult> ViewExpenses() {
            return View();
        }

        [Route("/account/LoadAllUserExpenses")]
        [HttpGet]
        public async Task<IActionResult> LoadAllUserExpenses() {
            var userid = GetUserId();
            var results = await _accountService.LoadAllUserExpenses(userid);
            return Json(results.Result);
        }


        int GetUserId() {
            int userid;
            int.TryParse(User.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault(), out userid);
            return userid;
        }

        List<string> getErrorMessages(ModelStateDictionary ModelState) {
            var messages = new List<string>();
            foreach (var errorEntry in ModelState.Values)
                foreach (var error in errorEntry.Errors)
                    messages.Add(error.ErrorMessage);

            return messages;
        }
        ClaimsPrincipal getPrincipal(User user) {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.LastName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim("UserId", user.Id.ToString()));
            foreach (var role in user.Roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Title));          

            return new ClaimsPrincipal(identity);
        }
    }
}