

using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BrandsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke(string BrandId)
        {

            ViewBag.BrandId = (int.TryParse(BrandId, out var id) ? id : (int?)null)!;

            return View(GetBrands());
        }

        private IEnumerable<BrandViewModel> GetBrands() =>
            _productData.GetBrands()
            .OrderBy(b => b.Order)
            .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
            });
    }
}
