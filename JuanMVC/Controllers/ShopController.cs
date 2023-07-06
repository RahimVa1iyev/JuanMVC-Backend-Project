using Microsoft.AspNetCore.Mvc;

namespace JuanMVC.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
