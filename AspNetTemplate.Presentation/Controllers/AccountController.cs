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

namespace AspNetTemplate.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
        public async Task<IActionResult> AddExpense() {

        }

        ClaimsPrincipal getPrincipal(User user) {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.LastName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in user.Roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Title));
            

            return new ClaimsPrincipal(identity);
        }
    }
}