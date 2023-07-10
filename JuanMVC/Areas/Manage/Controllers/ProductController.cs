using JuanMVC.Areas.Manage.ViewModels;
using JuanMVC.DAL;
using JuanMVC.Helpers;
using JuanMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanMVC.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly JuanDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(JuanDbContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page= 1 , string search= null)
        {
            ViewBag.Search = search;

            var query = _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Color)          
                .Include(x => x.Images.Where(x => x.ImageStatus == true))
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size).AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
                

            return View(PaginatedList<Product>.Create(query,page,5));
        }

        public IActionResult Create()
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();

            return View();
        }

        [HttpPost]

        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = _context.Brands.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Sizes = _context.Sizes.ToList();

                return View();

            }

            if (!_context.Brands.Any(x=>x.Id == product.BrandId)) return View("Error");
            if (!_context.Categories.Any(x => x.Id == product.CategoryId)) return View("Error");
            if (!_context.Colors.Any(x => x.Id == product.ColorId)) return View("Error");

          

            foreach (var sId in product.SizeIds)
            {
                if (!_context.Sizes.Any(x => x.Id == sId)) return View("Error");

                product.ProductSizes.Add(new ProductSize { SizeId = sId });
            }





            if(product.ImageFile == null) return View("Error");

            if (product.ImageFiles == null) return View("Error");

            Image posterImg = new Image()
            {
                ImageStatus = true,
                ImageName = FileManager.Save(product.ImageFile, _env.WebRootPath, "manage/assets/uploads/products")
            };

            product.Images.Add(posterImg);

            foreach (var item in product.ImageFiles)
            {

                Image img = new Image()
                {
                    ImageStatus = false,
                    ImageName = FileManager.Save(item, _env.WebRootPath, "manage/assets/uploads/products")
                };
                product.Images.Add(img);
            }
          
            _context.Products.Add(product);

            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var existProduct = _context.Products              
                .Include(x => x.Images)
                .Include(x => x.ProductSizes)
                .FirstOrDefault(x => x.Id == id);
            if (existProduct == null) return View("Error");

             existProduct.SizeIds = existProduct.ProductSizes.Select(x => x.SizeId).ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();



            return View(existProduct);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                var existPr = _context.Products
                .Include(x => x.Images)
                .Include(x => x.ProductSizes)
                .FirstOrDefault(x => x.Id == product.Id);
                if (existPr == null) return View("Error");

                existPr.SizeIds = existPr.ProductSizes.Select(x => x.SizeId).ToList();

                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Brands = _context.Brands.ToList();
                ViewBag.Sizes = _context.Sizes.ToList();



                return View(existPr);
            }

            var existProduct = _context.Products
               .Include(x => x.Images)
               .Include(x => x.ProductSizes)
               .FirstOrDefault(x => x.Id == product.Id);

            if (existProduct == null) return View();

            if (!_context.Brands.Any(x => x.Id == product.BrandId)) return View("Error");
            if (!_context.Categories.Any(x => x.Id == product.CategoryId)) return View("Error");
            if (!_context.Colors.Any(x => x.Id == product.ColorId)) return View("Error");


            existProduct.ProductSizes = new List<ProductSize>();

            foreach (var sId in product.SizeIds)
            {
                if (!_context.Sizes.Any(x => x.Id == sId)) return View("Error");

                existProduct.ProductSizes.Add(new ProductSize { SizeId = sId });
            }

            existProduct.Name = product.Name;
            existProduct.SalePrice = product.SalePrice;
            existProduct.CostPrice = product.CostPrice;
            existProduct.DiscountedPrice = product.DiscountedPrice;
            existProduct.Description = product.Description;
            existProduct.StockStatus = product.StockStatus;
            existProduct.BrandId = product.BrandId;
            existProduct.CategoryId = product.CategoryId;
            existProduct.ColorId = product.ColorId;

            List<string> removableAllImages = new List<string>();

            if(product.ImageFile != null)
            {
                Image poster = existProduct.Images.FirstOrDefault(x => x.ImageStatus == true);
                removableAllImages.Add(poster.ImageName);
                poster.ImageName = FileManager.Save(product.ImageFile, _env.WebRootPath, "manage/assets/uploads/products");
                             
            }

            var removableImages = existProduct.Images.FindAll(x => x.ImageStatus == false && !product.ImageIds.Contains(x.Id));
            _context.Images.RemoveRange(removableImages);

            removableAllImages.AddRange(removableImages.Select(x => x.ImageName).ToList());

          
                foreach (var item in product.ImageFiles)
                {
                    Image img = new Image()
                    {
                        ImageStatus = false,
                        ImageName = FileManager.Save(item, _env.WebRootPath, "manage/assets/uploads/products")
                    };
                    existProduct.Images.Add(img);

                }

            

            _context.SaveChanges();

            FileManager.DeleteAll(_env.WebRootPath, "manage/assets/uploads/products", removableAllImages);



            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {
             var existProduct = _context.Products.FirstOrDefault(x => x.Id == id);
            if (existProduct == null) return View("Error");

            List<string> removableImages = new List<string>();

            var images = existProduct.Images.Select(x=>x.ImageName);

            removableImages.AddRange(images);

            _context.Products.Remove(existProduct);
            _context.SaveChanges();

            FileManager.DeleteAll(_env.WebRootPath, "manage/assets/uploads/products", removableImages);


            return RedirectToAction("index");
        }
    }
}
