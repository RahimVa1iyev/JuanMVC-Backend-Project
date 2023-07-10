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
    public class SizeController :Controller
    {

        private readonly JuanDbContext _context;

        public SizeController(JuanDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            ViewBag.Search = search;

            var query = _context.Sizes.Include(x => x.ProductSizes).AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            return View(PaginatedList<Size>.Create(query, page, 5));
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public IActionResult Create(Size size)
        {
            if (!ModelState.IsValid) return View();

            if (_context.Sizes.Any(x => x.Name == size.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            _context.Sizes.Add(size);
            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var existSize = _context.Sizes.Find(id);

            if (existSize == null) return View("Error");


            return View(existSize);
        }

        [HttpPost]

        public IActionResult Edit(Size size)
        {
            if (!ModelState.IsValid) return View();

            var existSize = _context.Sizes.Find(size.Id);

            if (existSize == null) return View("Error");

            if (existSize.Name != size.Name && _context.Sizes.Any(x => x.Name == size.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            existSize.Name = size.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existSize = _context.Sizes.Include(x => x.ProductSizes).FirstOrDefault(x => x.Id == id);

            if (existSize == null) return View("Error");

            if (existSize.ProductSizes.Count > 0)
            {
                return StatusCode(400);
            }

            _context.Sizes.Remove(existSize);

            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
