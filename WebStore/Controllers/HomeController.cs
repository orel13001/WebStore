using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            //return Content("Data from new controller");
            return View();
        }

        public string ConfiguredAction(string id)
        {
            return $"Hellow World {id}!";
        }

        public void Throw(string Message) => throw new ApplicationException(Message);  
       
       
    }

    
}
