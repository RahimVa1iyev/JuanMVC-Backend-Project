﻿using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

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

        public IActionResult Edit(int id)
        {
            var existCategory = _context.Categories.Find(id);

            if (existCategory == null) return View("Error");

            return View(existCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid) return View();

            var existCategory = _context.Categories.Find(category.Id);

            if (existCategory == null) return View("Error");

            if (existCategory.Name != category.Name && _context.Categories.Any(x=>x.Name==category.Name))
            {
                ModelState.AddModelError("Name", "This name also already exists");
                return View();
            }

            existCategory.Name = category.Name;

            _context.SaveChanges();
                      
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existCategory = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            if (existCategory == null) return View("Error");

            if (existCategory.Products.Count>0)
            {
                return StatusCode(400);
            }
            _context.Categories.Remove(existCategory);
            _context.SaveChanges();
          
            return RedirectToAction("index");
        }

    }
}
