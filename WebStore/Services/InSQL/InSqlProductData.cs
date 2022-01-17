using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class InSqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public InSqlProductData(WebStoreDB db) => _db = db;

		public Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null)
		{
            var section = _db.Sections.FirstOrDefault(s => s.Name == Section) ?? new Section {Name = Section };
            var brand = Brand is { Length: > 0}
                ? _db.Brands.FirstOrDefault(s => s.Name == Brand) ?? new Brand {Name = Brand }
                : null;


            var product = new Product
            {
                Name = Name,
                Order = Order,
                Price = Price,
                ImageUrl = ImageUrl,
                Section = section,
                Brand = brand,
            };

			_db.Products.Add(product);
            _db.SaveChanges();

            return product;
		}

		public Brand? GetBrandById(int id) => _db.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
		public Section? GetSectionById(int id) => _db.Sections.Include(s => s.Products).FirstOrDefault(s => s.Id == id);

		public IEnumerable<Brand> GetBrands() => _db.Brands;

        public Product? GetProductById(int id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == id);

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.Ids?.Length > 0)
            {
                query = query.Where(p => Filter.Ids.Contains(p.Id));
            }
            else
            {
                if (Filter?.SectionId != null)
                {
                    query = query.Where(e => e.SectionId == Filter.SectionId);
                } 
                if (Filter?.BrandId != null)
                {
                    query = query.Where(e => e.BrandId == Filter.BrandId);
                }
            }

            return query;
        }


		public IEnumerable<Section> GetSections() => _db.Sections;
    }
}
