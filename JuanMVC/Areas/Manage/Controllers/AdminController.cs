using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Area("manage")]

    public class AdminController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(JuanDbContext context , UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.Where(x=>x.IsAdmin == true ).ToList();

           
            List<AdminDetailVM> list = new List<AdminDetailVM>();

            foreach (var user in users)
            {
                AdminDetailVM  vm = new AdminDetailVM()
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                };

                vm.Roles = await _userManager.GetRolesAsync(user);

                list.Add(vm);

            };
            
          
            return View(list);
        }

        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CreateAdminVM adminVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser admin = new AppUser
            {
                FullName = adminVM.FullName,
                UserName = adminVM.UserName,
                IsAdmin = true

            };



            var resultPassword = await _userManager.CreateAsync(admin, adminVM.Password);

            if (!resultPassword.Succeeded)
            {
                foreach (var err in resultPassword.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }

            var resultRole = await _userManager.AddToRoleAsync(admin, adminVM.Role);


            if (!resultRole.Succeeded)
            {
                foreach (var err in resultRole.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return View("Error");

            EditAdminVM adminVM = new EditAdminVM
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Role = String.Join(",", _userManager.GetRolesAsync(user).Result)

            };


            return View(adminVM);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(EditAdminVM editAdmin)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(editAdmin.UserName);

                if (user == null) return View("Error");

                EditAdminVM adminVM = new EditAdminVM
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Role = String.Join(",", _userManager.GetRolesAsync(user).Result)

                };


                return View(adminVM);

            }

            if(!_userManager.Users.Any(x=>x.UserName == editAdmin.UserName))
            {
                ModelState.AddModelError("", "Username is already taken");

                var user = await _userManager.FindByNameAsync(editAdmin.UserName);

                if (user == null) return View("Error");

                EditAdminVM adminVM = new EditAdminVM
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Role = String.Join(",", _userManager.GetRolesAsync(user).Result)

                };


                return View(adminVM);

            }

            var existUser = await _userManager.FindByNameAsync(editAdmin.UserName);

            existUser.FullName = editAdmin.UserName;
            existUser.UserName = editAdmin.UserName;

            var existRoles = await _userManager.GetRolesAsync(existUser);
            var result = await _userManager.RemoveFromRolesAsync(existUser,existRoles);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    var user = await _userManager.FindByNameAsync(editAdmin.UserName);

                    if (user == null) return View("Error");

                    EditAdminVM adminVM = new EditAdminVM
                    {
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Role = String.Join(",", _userManager.GetRolesAsync(user).Result)

                    };


                    return View(adminVM);

                }


            }

           var addResult = await _userManager.AddToRoleAsync(existUser,editAdmin.Role);

            if (!addResult.Succeeded)
            {
                foreach (var err in addResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    var user = await _userManager.FindByNameAsync(editAdmin.UserName);

                    if (user == null) return View("Error");

                    EditAdminVM adminVM = new EditAdminVM
                    {
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Role = String.Join(",", _userManager.GetRolesAsync(user).Result)

                    };
                }
            }


            return RedirectToAction("index");
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return View("Error");

            var result  = await _userManager.DeleteAsync(user);
                      
            return RedirectToAction("index");
        }
       
    }
}
