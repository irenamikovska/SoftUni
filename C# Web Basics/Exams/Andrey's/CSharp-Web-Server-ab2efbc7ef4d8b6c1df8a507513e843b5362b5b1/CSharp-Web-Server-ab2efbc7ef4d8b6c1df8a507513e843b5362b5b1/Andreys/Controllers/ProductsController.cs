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
    public class ProductsController : Controller
    {
        private readonly IValidator validator;       
        private readonly AppDbContext data;
        
        public ProductsController(IValidator validator, AppDbContext data)
        {
            this.validator = validator;           
            this.data = data;
        }

        [Authorize]
        public HttpResponse Add()
        {
           return this.View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddProductFormModel model)
        {
            var modelErrors = this.validator.ValidateProduct(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }
                        
            var product = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                Category = Enum.Parse<Category>(model.Category),
                Gender = Enum.Parse <Gender>(model.Gender)
            };

            if (product == null) 
            {
                return Redirect("/Products/Add");
            }

            this.data.Products.Add(product);
            this.data.SaveChanges();            
            return Redirect("/");
        }

        [Authorize]
        public HttpResponse Details(int id)
        {
            var product = this.data.Products.FirstOrDefault(x => x.Id == id);
            return this.View(product);
        }

        [Authorize]
        public HttpResponse Delete(int id)
        {
            var product = this.data.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return BadRequest();
            }

            this.data.Products.Remove(product);

            this.data.SaveChanges();

            return Redirect("/");
        }

    }
}
