using WebStore.Domain.Entities;
using Newtonsoft.Json;
using WebStore.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;

        private readonly string _cartName;

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context?.Response.Cookies;

                var cart_cookie = context.Request.Cookies[_cartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaceCart(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCart(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cart);
        }

        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _httpContextAccessor = httpContextAccessor;
            _productData = productData;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStoreLSA.Cart{user_name}";
        }

        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == id);
            if (item == null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == id);
            if (item == null)
                return;
            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);
            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var cart = Cart;

            var products = _productData.GetProducts(new()
            {
                Ids = Cart.Items.Select(p => p.ProductId).ToArray()
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
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == id);
            if (item == null)
                return;

            cart.Items.Remove(item);

            Cart = cart;
        }
    }
}
