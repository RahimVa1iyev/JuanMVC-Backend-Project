using JuanMVC.DAL;
using JuanMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class UserController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public UserController(JuanDbContext context,UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        public IActionResult Index()
        {

          var users= _usermanager.Users.Where(x=>!x.IsAdmin).ToList();

            return View(users);
        }
    }
}
