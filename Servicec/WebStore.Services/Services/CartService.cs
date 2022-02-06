using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _CartStore;
        private readonly IProductData _productData;

        private readonly string _cartName;

        

        public CartService(ICartStore CartStore, IProductData productData)
        {
            _CartStore = CartStore;
            _productData = productData;

        }

        public void Add(int id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == id);
            if (item == null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;

            _CartStore.Cart = cart;
        }

        public void Clear()
        {
            var cart = _CartStore.Cart;

            cart.Items.Clear();

            _CartStore.Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == id);
            if (item == null)
                return;
            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);
            _CartStore.Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var cart = _CartStore.Cart;

            var products = _productData.GetProducts(new()
            {
                Ids = _CartStore.Cart.Items.Select(p => p.ProductId).ToArray()
            });

            var product_views = products.ToView().ToDictionary(i => i.Id);
            return new CartViewModel()
            {
                Items = cart.Items.Where(item => product_views.ContainsKey(item.ProductId))
                .Select(item => (product_views[item.ProductId], item.Quantity))!
            };

        }

        public void Remove(int id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == id);
            if (item == null)
                return;

            cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }
    }
}
