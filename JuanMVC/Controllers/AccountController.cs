using JuanMVC.DAL;
using JuanMVC.Email;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JuanMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JuanDbContext _context;
        private readonly IMailService _mailService;
        private readonly IHubContext<JuanHub> _hubContext;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager,JuanDbContext context,IMailService mailService, IHubContext<JuanHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _mailService = mailService;
            _hubContext = hubContext;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {

            if (!ModelState.IsValid) return View();

            var member = await _userManager.FindByEmailAsync(registerVM.Email);
            if (member != null)
            {
                ModelState.AddModelError("", "Email is already exist");
                return View();
            }

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
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = Url.Action("confirmedemail", "account", new { email = user.Email, token = token }, Request.Scheme);

            await _mailService.SendEmailAsync(new MailRequest
            {

                ToEmail = user.Email,
                Subject = "Mail Confirmation",
                Body = $"<a href={url}  >Click Here</a>"
            });


            await _userManager.AddToRoleAsync(user, "Member");

            return View("login");
        }

        public async Task<IActionResult> ConfirmedEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return RedirectToAction("login");

            }
            else
            {
                return View("error");
            }


        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM , string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);

            if(user == null || user.IsAdmin)
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

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Mail do confirm");

                return View();

            }

            await _hubContext.Clients.All.SendAsync("LoginInfo");



            return returnUrl == null ? RedirectToAction("index", "home") : Redirect(returnUrl);

        }

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Profile( string tab = "Profile" )
        {
            ViewBag.Tab = tab;

            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            ProfileVM vm = new ProfileVM
            {
                MemberProfile = new MemberAccountProfileVM
                {
                    FullName = member.FullName,
                    Username = member.UserName,
                    Email = member.Email,
                    Phone = member.PhoneNumber
                },
                Orders = _context.Orders.Include(x => x.OrderItems).Where(x => x.UserId == member.Id).ToList()
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

        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM passwordVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByEmailAsync(passwordVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email is incorrect");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("verify", "account", new { email = passwordVM.Email, token = token  }, Request.Scheme);

            await _mailService.SendEmailAsync(new MailRequest
            {

                ToEmail = user.Email,
                Subject = "Change Password",
                Body = $"<a href={url}  >Click Here</a>"
            });

            return RedirectToAction("index");
                   
        }

        public async Task<IActionResult> Verify(string email , string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (await _userManager.VerifyUserTokenAsync(user,_userManager.Options.Tokens.PasswordResetTokenProvider,"ResetPassword",token))
            {
                TempData["Email"] = email;
                TempData["Token"] = token;


                return RedirectToAction("resetpassword");   
            }

            return View("Error");

        }

        public IActionResult ResetPassword()
        {
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetVM)
        {
            AppUser user = await _userManager.FindByEmailAsync(resetVM.Email);

            var result = await _userManager.ResetPasswordAsync(user, resetVM.Token, resetVM.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", "Password can't change");
                    return View();
                }
            }




            return RedirectToAction("login");
        }
    }
}
