using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {

            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                FullName = registerVM.FullName,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                PhoneNumber = registerVM.Phone,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();

                }

            }


            await _userManager.AddToRoleAsync(user, "Member");

            return View("login");
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);

            if(user == null)
            {
                ModelState.AddModelError("", "Username or Passwor is incorrect");
                return View();
            }

          

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password,false,false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Passwor is incorrect");
                return View();
            }

            return RedirectToAction("index", "home");

        }

        public async Task<IActionResult> Profile()
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            ProfileVM vm = new ProfileVM
            {
                MemberProfile = new MemberAccountProfileVM
                {
                    FullName = member.FullName,
                    Username = member.UserName,
                    Email = member.Email,
                    Phone = member.PhoneNumber
                }
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> MemberUpdate(MemberAccountProfileVM profileVM)
        {
            if (!ModelState.IsValid)
            {
                ProfileVM vm = new ProfileVM() { MemberProfile = profileVM };

                return View("profile", vm);
            }

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FullName = profileVM.FullName;
            user.PhoneNumber = profileVM.Phone;
            user.Email = profileVM.Email;
            user.UserName = profileVM.Username;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);

                    ProfileVM vm = new ProfileVM() { MemberProfile = profileVM };

                    return View("profile",vm);
                }
            }

            if (profileVM.NewPassword != null)
            {
                var checkResult = await _signInManager.PasswordSignInAsync(user, profileVM.CurrentPassword,false,false);

                if (!checkResult.Succeeded)
                {
                    ModelState.AddModelError("CurrentPassword", "Pasword is incorrect");
                    ProfileVM vm = new ProfileVM() { MemberProfile = profileVM };

                    return View("profile",vm);

                }

                await _userManager.ChangePasswordAsync(user, profileVM.CurrentPassword, profileVM.NewPassword);

            }


            await _signInManager.SignInAsync(user, false);


            return RedirectToAction("profile");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");

        }
    }
}
