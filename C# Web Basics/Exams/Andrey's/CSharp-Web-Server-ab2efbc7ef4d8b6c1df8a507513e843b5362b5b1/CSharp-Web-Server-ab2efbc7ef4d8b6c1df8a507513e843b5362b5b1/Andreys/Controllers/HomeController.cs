using Andreys.Data;
using Andreys.Services;
using Andreys.ViewModels.Products;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andreys.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly AppDbContext data;
        public HomeController(AppDbContext data)
        {
           this.data = data;
        }
                              
        public HttpResponse Index()
        {        
            if (this.User.IsAuthenticated) 
            {
                var products = this.data.Products.Select(x => new AllProductsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price

                }).ToList();

                return this.View(products, "Home");
            }

            return this.View();
        }
    }
}
