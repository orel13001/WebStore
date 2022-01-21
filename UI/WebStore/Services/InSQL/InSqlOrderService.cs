using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class InSqlOrderService : IOrderService
    {
        WebStoreDB _db;
        UserManager<User> _userManager;
        public InSqlOrderService(WebStoreDB db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        public async Task<Order> CreateOrderAsync(
        string UserName,
        CartViewModel cartViewModel,
        OrderViewModel OrderModel,
        CancellationToken Cancel = default)
        {
            var user = await _userManager.FindByNameAsync(UserName).ConfigureAwait(false);

            if (user is null)
                throw new InvalidOperationException($"Пользователь с именем {UserName} не найден в БД");

            await using var transaction = await _db.Database.BeginTransactionAsync(Cancel).ConfigureAwait(false);

            var order = new Order
            {
                User = user,
                Address = OrderModel.Address,
                Phone = OrderModel.Phone,
                Description = OrderModel.Description,
            };

            var products_ids = cartViewModel.Items.Select(item => item.Product.Id).ToArray();

            var cart_products = await _db.Products
               .Where(p => products_ids.Contains(p.Id))
               .ToArrayAsync(Cancel)
               .ConfigureAwait(false);

            order.Items = cartViewModel.Items.Join(
                cart_products,
                cart_item => cart_item.Product.Id,
                cart_product => cart_product.Id,
                (cart_item, cart_product) => new OrderItem
                {
                    Order = order,
                    Product = cart_product,
                    Price = cart_product.Price, // Здесь может быть применена скидка к стоимости товара
                Quantity = cart_item.Quantity,
                }).ToArray();

            await _db.Orders.AddAsync(order, Cancel).ConfigureAwait(false);

            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);

            await transaction.CommitAsync(Cancel).ConfigureAwait(false);

            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken Cancel = default)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(o => o.Id == id, Cancel)
                .ConfigureAwait(false);

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(item => item.Product)
                .Where(o => o.User.UserName == UserName)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return order;

        }
    }
}
