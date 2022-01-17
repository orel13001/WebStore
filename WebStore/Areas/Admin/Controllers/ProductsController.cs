using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrotors)]
    public class ProductsController : Controller
    {

        IProductData _productData;
        ILogger<ProductsController> _logger;
        public ProductsController(IProductData productData, ILogger<ProductsController> logger)
		{
            _productData = productData;
            _logger = logger;
		}
        public IActionResult Index()
		{
            var products = _productData.GetProducts();
            return View(products);
		}
    }
}
