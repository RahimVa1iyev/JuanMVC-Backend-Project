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
    public class BrandController : Controller
    {
        private readonly JuanDbContext _context;

        public BrandController(JuanDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page =1 , string search = null)
        {
            ViewBag.Search = search;
            var query = _context.Brands.Include(x => x.Products).AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }


            return View(PaginatedList<Brand>.Create(query,page,5));
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            if (_context.Brands.Any(x=>x.Name == brand.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var existBrand = _context.Brands.Find(id);

            if (existBrand == null) return View("Error");


            return View(existBrand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            var existBrand = _context.Brands.Find(brand.Id);

            if(existBrand == null) return View("Error");

            if (existBrand.Name != brand.Name && _context.Brands.Any(x=>x.Name==brand.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            existBrand.Name = brand.Name;

            _context.SaveChanges();

          
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existBrand = _context.Brands.Include(x=>x.Products).FirstOrDefault(x=>x.Id==id);

            if(existBrand==null) return View("Error");

            if (existBrand.Products.Count>0)
            {
                return StatusCode(400);
            }

            _context.Brands.Remove(existBrand);
            _context.SaveChanges();

            return RedirectToAction("index");

        }

       
    }
}
