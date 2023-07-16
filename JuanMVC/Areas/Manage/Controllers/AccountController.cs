using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHubContext<JuanHub> _hubContext;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager,IHubContext<JuanHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> CreateRole()
        {
            //await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            //await _roleManager.CreateAsync(new IdentityRole("Admin"));

            //await _roleManager.CreateAsync(new IdentityRole("Member"));
            await _roleManager.CreateAsync(new IdentityRole("Hr"));




            return Content("created");

        }


        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser()
            {
                FullName = "SuperAdmin",
                UserName = "SuperAdmin",
                IsAdmin = true
                
            };

            await _userManager.CreateAsync(admin, "Admin@123");
            await _userManager.AddToRoleAsync(admin, "SuperAdmin");

            return Content("yaradildi");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVM adminVM, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            var admin = await _userManager.FindByNameAsync(adminVM.UserName);

            if (admin == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(admin, adminVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            await _hubContext.Clients.All.SendAsync("LoginInfo");



            return returnUrl == null ? RedirectToAction("index", "dashboard") : Redirect(returnUrl);
        }


    }
}
