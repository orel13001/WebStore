using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.ViewModels;
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

        //public IActionResult Create() => View();

        /// <summary>
        /// Отправка формы для редактирования сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            var employee = _Employees.FirstOrDefault(o => id == o.Id);

            if (employee == null)
                return NotFound();

            var model = new EmployeeEditViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                Patronymic = employee.Patronomic,
                
            };

            return View(model);
        }

        /// <summary>
        /// Приём результата изменения в форме и его обработка
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public IActionResult Edit(EmployeeEditViewModel Model)
        {
            //Обработка модели...

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) => View();
    }
}
