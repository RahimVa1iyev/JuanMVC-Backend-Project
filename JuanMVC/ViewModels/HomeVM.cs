using JuanMVC.Models;

namespace JuanMVC.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }

        public List<Service> Services { get; set; }

        public List<Product> OurProducts { get; set; }

        public List<Campany> Campanies { get; set; }

        public List<Product> NewProducts { get; set; }

        public List<Sponsor> Sponsors { get; set; }
    }
}
