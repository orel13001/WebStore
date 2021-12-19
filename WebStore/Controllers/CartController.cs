using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult DoAction()
		{
			return View();
		}
	}
}
