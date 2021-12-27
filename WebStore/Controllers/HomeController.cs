using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index([FromServices]IProductData ProductData)
        {
            var product = ProductData.GetProducts()
                .OrderBy(o => o.Order)
                .Take(6)
                .Select(o => new ProductViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    Price = o.Price,
                    ImageUrl = o.ImageUrl,
                });
            ViewBag.ProductData = product;
            //return Content("Data from new controller");
            return View();
        }

        public string ConfiguredAction(string id)
        {
            return $"Hellow World {id}!";
        }

        public void Throw(string Message) => throw new ApplicationException(Message);  

        public IActionResult Error404() => View();
       
        
    }

    
}
