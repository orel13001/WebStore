using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
	{
		private readonly ICartService _cartService;

		public CartController (ICartService cartService) => _cartService = cartService;

		public IActionResult Index() => View(new CartOrderViewModel { Cart = _cartService.GetViewModel()});

		public IActionResult Add(int id)
		{
			_cartService.Add(id);
			return RedirectToAction("Index", "Cart");
		}

		public IActionResult Decrement(int id)
		{
			_cartService.Decrement(id);
			return RedirectToAction("Index", "Cart");
		}

		public IActionResult Remove(int id)
		{
			_cartService.Remove(id);
			return RedirectToAction("Index", "Cart");
		}

		[HttpPost] 
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CheckOut(OrderViewModel orderModel, [FromServices] IOrderService orderService)
        {
			if (!ModelState.IsValid)
				return View(nameof(Index), new CartOrderViewModel()
				{
					Cart = _cartService.GetViewModel(),
					Order = orderModel
				});

			var order = await orderService.CreateOrderAsync(
				User.Identity?.Name,
				_cartService.GetViewModel(),
				orderModel);

			_cartService.Clear();

			return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
        }


		public IActionResult OrderConfirmed(int Id)
        {
			ViewBag.OrderId = Id;
			return View();
        }

	}
}
