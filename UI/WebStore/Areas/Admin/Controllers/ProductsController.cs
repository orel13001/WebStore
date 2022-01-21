using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Areas.Admin.ViewModel;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

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

        public IActionResult Edit(int id)
		{
            var product = _productData.GetProductById(id);

			if (product is null)
			{
                return NotFound();
			}

            return View(new EditProductViewModel()
			{
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                SectionId = product.SectionId,
                Section = product.Section.Name,
                Brand = product.Brand?.Name,
                BrandId = product.Brand?.Id,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
			});
		}

        [HttpPost]
        public IActionResult Edit(EditProductViewModel viewModel)
		{
            if(!ModelState.IsValid)
                return View(viewModel);

   //         var product = _productData.GetProductById(viewModel.Id);
			//if (product is null)
			//{
   //             return NotFound();
			//}

   //         product.Name = viewModel.Name;
   //         product.Order = viewModel.Order;
   //         product.Price = viewModel.Price;
   //         product.ImageUrl = viewModel.ImageUrl;

   //         var brand = _productData.GetBrandById(viewModel.BrandId ?? -1);
   //         var section = _productData.GetSectionById(viewModel.SectionId);

   //         product.Brand = brand;
   //         product.Section = section;
   //   обновить, используя сервис _productData
   //         //_productData.Update(product);

            return RedirectToAction(nameof(Index));
		}

        public IActionResult Delete (int id)
		{
            var product = _productData.GetProductById(id);

            if (product is null)
            {
                return NotFound();
            }

            return View(new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                SectionId = product.SectionId,
                Section = product.Section.Name,
                Brand = product.Brand?.Name,
                BrandId = product.Brand?.Id,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)
		{
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();

            //   удалить, используя сервис _productData
            //_productData.Delete(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
