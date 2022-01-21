using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrdersAsync(User.Identity!.Name!);

            return View(orders.Select(ord => new UserOrderViewModel()
            {
                Id = ord.Id,
                Address = ord.Address,
                Date = ord.Date,
                Description = ord.Description,
                Phone = ord.Phone,
                TotalPrice = ord.TotalPrice,
            }));
        }
    }
}
