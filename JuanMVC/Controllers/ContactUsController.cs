using JuanMVC.DAL;
using JuanMVC.Models;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JuanMVC.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactUsController(JuanDbContext context , UserManager<AppUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user =  User.Identity.IsAuthenticated ? await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
            if (user != null)
            {
                ContactVM vm = new ContactVM
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber
                };

                return View(vm);

            }

        
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactVM userContact)
        {
            if (!ModelState.IsValid)
            {
                var user = User.Identity.IsAuthenticated ? await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
                if (user != null)
                {
                    ContactVM vm = new ContactVM
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        Phone = user.PhoneNumber
                    };

                    return View(vm);

                }

                return View();

            }


            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

            if (userId != null)
            {
                UserContact model = new UserContact
                {
                    AppUserId = userId,
                    Email = userContact.Email,
                    Phone = userContact.Phone,
                    FullName = userContact.FullName,
                    Subject = userContact.Subject,
                    Text = userContact.Text,
                };

                _context.UserContacts.Add(model);

            }
            else
            {
                UserContact model = new UserContact
                {
                    Email = userContact.Email,
                    Phone = userContact.Phone,
                    FullName = userContact.FullName,
                    Subject = userContact.Subject,
                    Text = userContact.Text,
                };
                _context.UserContacts.Add(model);
            }


            _context.SaveChanges();

            TempData["Success"] = "Message send successfully";

            return RedirectToAction("index","contactus");
        }
    }

   
}
