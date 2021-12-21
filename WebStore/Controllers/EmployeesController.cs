using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Data;

namespace WebStore.Controllers
{ 
    //[Route("empl/[action]/{id?}")]            // Маршрутизация для отдельного контроллера
    //[Route("Staff/{action=Index}/{Id?}")]     // Маршрутов может быть несколько
    public class EmployeesController : Controller
    {
        private ICollection<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }
        public IActionResult Index()
        {
            return View(_Employees);
        }

        //[Route("~/employees/info-{id}")] // Маршрутизация для отдельного метода
        public IActionResult EmployeesInfo(int id)
        {
            var employee = _Employees.FirstOrDefault(o => o.Id == id);

            if (employee == null)
                return NotFound();
            return View(employee);
        }
    }
}
