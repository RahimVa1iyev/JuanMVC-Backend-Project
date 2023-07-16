using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Email;
using JuanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ContactUsController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly IMailService _mailService;

        public ContactUsController(JuanDbContext context,IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.UserContacts.Include(x => x.AppUser).AsQueryable();

            return View(PaginatedList<UserContact>.Create(query,page,5));
        }

        public IActionResult SendEmail(int id)
        {
            var message = _context.UserContacts.Find(id);

            return View(message);   
        }

        [HttpPost]

        public async Task<IActionResult> SendEmail(UserContact userContact)
        {
            var message = _context.UserContacts.Find(userContact.Id);

            await _mailService.SendEmailAsync(new MailRequest
            {

                ToEmail = message.Email,
                Subject = "Mail Confirmation",
                Body = $"<p> {userContact.Response} </p>"
            });

            TempData["response"] = "success";

            return RedirectToAction("index");
        }
    }
}
