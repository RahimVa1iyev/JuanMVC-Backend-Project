using JuanMVC.DAL;
using JuanMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly JuanDbContext _context;

        public HomeController(JuanDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM vm = new HomeVM
            {
                Sliders = _context.Sliders.OrderBy(x => x.Order).ToList(),
                Services = _context.Services.ToList(),
                OurProducts = _context.Products.Include(x => x.Images).Include(x => x.Brand).Include(x => x.Category).Take(5).ToList(),
                Campanies = _context.Campanies.ToList(),
                NewProducts = _context.Products.Include(x=>x.Images.Where(x=>x.ImageStatus==true)).Where(x=>x.IsNew==true).ToList(),
                Sponsors = _context.Sponsors.ToList(),

            };

            return View(vm);
        }

        public IActionResult GetSearch(string searchValue)
        {
            var datas = _context.Products.Where(x => x.Name.Contains(searchValue)).ToList();


            return Json(datas);

        }
    }
}
