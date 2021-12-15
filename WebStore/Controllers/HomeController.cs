using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> _Employees = new List<Employee>()
        {
            new Employee() { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronomic = "иванович", Age = 27 },
            new Employee() { Id = 2, LastName = "Петров", FirstName = "Петр", Patronomic = "Петрович", Age = 30 },
            new Employee() { Id = 3, LastName = "Сидоров", FirstName = "Сидр", Patronomic = "Сидорович", Age = 27 }
        };

        public IActionResult Index()
        {
            //return Content("Data from new controller");
            return View();
        }

        public string ConfiguredAction(string id)
        {
            return $"Hellow World {id}!";
        }


        public IActionResult Employees()
        {
            return View(_Employees);
        }
    }

    
}
