using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore.ViewModels;

namespace WebStore.Services.Interfaces
{
	public interface IOrderService
	{
		Task<IEnumerable<Order>> GetUserOrdersAsync (string UserName, CancellationToken Cancel = default);

		Task<Order?> GetOrderByIdAsync(int id, CancellationToken Cancel = default);

		Task<Order> CreateOrderAsync(string UserName, CartViewModel cartViewModel, OrderViewModel orderViewModel, CancellationToken Cancel = default);
	}
}
