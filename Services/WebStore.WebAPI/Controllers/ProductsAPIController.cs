using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{

    [ApiController]
    [Route("api/products")] // http://localhost:5001/api/products
    public class ProductsAPIController : Controller
    {
        private readonly IProductData _productData;

        public ProductsAPIController(IProductData productData) => _productData = productData;

        [HttpGet("sections")] // GET ->  http://localhost:5001/api/products/sections
        public IActionResult GetSections()
        {
            var sections = _productData.GetSections();
            return Ok(sections.ToDTO());
        }

        [HttpGet("sections/{Id}")]
        public IActionResult GetSectionById(int Id)
        {
            var section = _productData.GetSectionById(Id);
            if(section is null)
                return NotFound();
            return Ok(section.ToDTO());
        }


        [HttpGet("brands")] // GET ->  http://localhost:5001/api/products/brands
        public IActionResult GetBrands()
        {
            var brands = _productData.GetBrands();
            return Ok(brands.ToDTO());
        }

        [HttpGet("brsnds/{Id}")]
        public IActionResult GetBrandById(int Id)
        {
            var brend = _productData.GetBrandById(Id);
            if (brend is null)
                return NotFound();
            return Ok(brend.ToDTO());
        }


        [HttpPost] // Post-запрос, т.к. в качестве параметра передаётся объект, размер которого может быть большим.
        public IActionResult GetProducts(ProductFilter? filter = null)
        {
            var products = _productData.GetProducts(filter);
            return Ok(products.ToDTO());
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null)
                return NotFound();

            return Ok(product.ToDTO());
        }

        [HttpPost("new/{Name}")]
        public IActionResult CreateProduct(CreateProductDTO Model)
        {
            var product = _productData.CreateProduct(Model.Name, Model.Order, Model.Price, Model.ImageUrl, Model.Section, Model.Brand);

            return CreatedAtAction(nameof(GetProductById), new {product.Id}, product.ToDTO());
        }
    }
}
