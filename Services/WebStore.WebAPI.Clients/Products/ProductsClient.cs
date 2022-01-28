
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIAddrsses.Products)
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

            var product = response.Content.ReadFromJsonAsync<ProductDTO>().Result;
            return product!.FromDTO();
        }

        public Brand? GetBrandById(int id)
        {
            var brand = Get<BrandDTO>($"{Address}/brand/{id}");
            return brand!.FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
            return brands!.FromDTO();
        }

        public Product? GetProductById(int id)
        {
            var product = Get<ProductDTO>($"{Address}/{id}");
            return product!.FromDTO();
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            var response = Post(Address, Filter ?? new());
            var products = response.Content. ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
            return products!.FromDTO();
        }

        public Section? GetSectionById(int id)
        {
            var section = Get<SectionDTO>($"{Address}/sections/{id}");
            return section!.FromDTO();
        }

        public IEnumerable<Section> GetSections()
        {
            var sections = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
            return sections!.FromDTO();
        }
    }
}
