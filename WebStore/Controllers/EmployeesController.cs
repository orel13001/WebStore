using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> _Employees = new List<Employee>()
        {
            new Employee() { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronomic = "Иванович", Age = 27 },
            new Employee() { Id = 2, LastName = "Петров", FirstName = "Петр", Patronomic = "Петрович", Age = 30 },
            new Employee() { Id = 3, LastName = "Сидоров", FirstName = "Сидр", Patronomic = "Сидорович", Age = 27 }
        };
        public IActionResult Index()
        {
            return View(_Employees);
        }

        public IActionResult EmployeesInfo(int id)
        {
            var employee = _Employees.FirstOrDefault(o => o.Id == id);

            if (employee == null)
                return NotFound();
            return View(employee);
        }
    }
}
