using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        Section? GetSectionById(int id);

        Brand? GetBrandById(int id);

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter? Filter = null);

        Product? GetProductById(int id);

        Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand);
    }
}
