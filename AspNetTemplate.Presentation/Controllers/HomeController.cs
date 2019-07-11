using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetTemplate.ApplicationService.UserService;
using AspNetTemplate.ClientEntity.ViewModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService  _userService;

        public HomeController(IUserService userService) {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            var model = new IndexViewModel();
            return View(model);
        }
    }
}
