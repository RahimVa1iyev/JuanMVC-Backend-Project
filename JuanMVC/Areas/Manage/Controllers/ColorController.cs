using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Area("manage")]
    public class ColorController : Controller
    {
        private readonly JuanDbContext _context;

        public ColorController(JuanDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            ViewBag.Search = search;

            var query = _context.Colors.Include(x => x.Products).AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            return View(PaginatedList<Color>.Create(query, page, 5));
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Colors.Any(x => x.Name == color.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            _context.Colors.Add(color);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var existColor = _context.Colors.Find(id);

            if (existColor == null) return View("Error");

            return View(existColor);
        }

        [HttpPost]
        public IActionResult Edit(Color color)
        {
            if (!ModelState.IsValid) return View();

            var existColor = _context.Colors.Find(color.Id);

            if (existColor == null) return View("Error");

            if (existColor.Name != color.Name && _context.Colors.Any(x => x.Name == color.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            existColor.Name = color.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existColor = _context.Colors.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            if (existColor == null) return View("Error");

            if (existColor.Products.Count > 0)
            {
                return StatusCode(400);
            }
            _context.Colors.Remove(existColor);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
