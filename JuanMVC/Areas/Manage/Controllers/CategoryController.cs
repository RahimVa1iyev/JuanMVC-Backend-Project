using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly JuanDbContext _context;

        public CategoryController(JuanDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1,string search = null)
        {
            ViewBag.Search = search;

            var query = _context.Categories.Include(x => x.Products).AsQueryable();

            if (search != null)
            {
               query =  query.Where(x => x.Name.Contains(search));
            }

            return View(PaginatedList<Category>.Create(query,page,5));
        }


        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Categories.Any(x=>x.Name==category.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
