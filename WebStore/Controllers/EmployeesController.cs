using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.ViewModels;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{ 
    //[Route("empl/[action]/{id?}")]            // Маршрутизация для отдельного контроллера
    //[Route("Staff/{action=Index}/{Id?}")]     // Маршрутов может быть несколько
    public class EmployeesController : Controller
    {
        //private ICollection<Employee> _Employees;
        private readonly IEmployeesData _EmployeesData;
        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }
        public IActionResult Index()
        {
            var result = _EmployeesData.GetAll();
            return View(result);
        }

        //[Route("~/employees/info-{id}")] // Маршрутизация для отдельного метода
        public IActionResult EmployeesInfo(int id)
        {
            var employee = _EmployeesData.GetById(id);

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
            var employee = _EmployeesData.GetById(id);

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
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel Model)
        {
            //Обработка модели...
            var employee = new Employee()
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Age = Model.Age,
                Patronomic = Model.Patronymic,
            };

            if(!_EmployeesData.Edit(employee))
                return NotFound();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) => View();
    }
}
