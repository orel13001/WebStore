using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IEnumerable<Product> products = TestData.Products;
            if (Filter?.SectionId != null)
            {
                products = products.Where(o => o.SectionId == Filter.SectionId);

            }

            if (Filter?.BrandId != null)
            {
                products = products.Where(o => o.BrandId == Filter.BrandId);
            }

            return products;
        }
    }
}
