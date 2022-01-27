﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient client) : base(client, "api/orders") { }

        public async Task<Order> CreateOrderAsync(string UserName, CartViewModel cartViewModel, OrderViewModel orderViewModel, CancellationToken Cancel = default)
        {
            var model = new CreateOrderDTO
            {
                Items = cartViewModel.ToDTO(),
                Order = orderViewModel,
            };

            var response = await PostAsync($"{Address}/{UserName}", model).ConfigureAwait(false);
            var order = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<OrderDTO>(cancellationToken: Cancel)
                .ConfigureAwait(false);

            return order.FromDTO()!;
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken Cancel = default)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/user/{id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}").ConfigureAwait(false);
            return orders!.FromDTO()!;
        }
    }
}