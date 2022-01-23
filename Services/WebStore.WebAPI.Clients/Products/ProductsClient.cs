
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, "api/products")
        {

        }

        public Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand)
        {
            
        }

        public Brand? GetBrandById(int id)
        {
            
        }

        public IEnumerable<Brand> GetBrands()
        {
            
        }

        public Product? GetProductById(int id)
        {
            
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            
        }

        public Section? GetSectionById(int id)
        {
            
        }

        public IEnumerable<Section> GetSections()
        {
            
        }
    }
}
