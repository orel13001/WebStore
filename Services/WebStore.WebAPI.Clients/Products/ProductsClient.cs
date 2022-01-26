
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
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
            var query = string.Join("&",
                $"{nameof(Order)}={Order}",
                $"{nameof(Price)}={Price}",
                $"{nameof(ImageUrl)}={ImageUrl}",
                $"{nameof(Section)}={Section}",
                $"{nameof(Brand)}={Brand}");
            var response = Post($"{Address}/new/", new CreateProductDTO
            {
                Name = Name,
                Order = Order,
                Price = Price,
                ImageUrl = ImageUrl,
                Section = Section,
                Brand = Brand,
            });

            var product = response.Content.ReadFromJsonAsync<Product>().Result;
            return product!;
        }

        public Brand? GetBrandById(int id)
        {
            var brand = Get<Brand>($"{Address}/brand/{id}");
            return brand!;
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = Get<IEnumerable<Brand>>($"{Address}/brands");
            return brands!;
        }

        public Product? GetProductById(int id)
        {
            var product = Get<Product>($"{Address}/{id}");
            return product!;
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            var response = Post(Address, Filter ?? new());
            var products = response.Content. ReadFromJsonAsync<IEnumerable<Product>>().Result;
            return products!;
        }

        public Section? GetSectionById(int id)
        {
            var section = Get<Section>($"{Address}/sections/{id}");
            return section!;
        }

        public IEnumerable<Section> GetSections()
        {
            var sections = Get<IEnumerable<Section>>($"{Address}/sections");
            return sections!;
        }
    }
}
