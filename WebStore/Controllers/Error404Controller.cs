using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
	public class Error404Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
