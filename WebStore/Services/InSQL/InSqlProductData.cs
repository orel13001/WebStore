﻿using System;
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

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (Filter?.SectionId != null)
            {
                query = query.Where(e => e.SectionId == Filter.SectionId);
            }
            if (Filter?.BrandId != null)
            {
                query = query.Where(e => e.BrandId == Filter.BrandId);
            }

            return query;
        }

        public IEnumerable<Section> GetSections() => _db.Sections;
    }
}
