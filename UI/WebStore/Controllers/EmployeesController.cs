using Microsoft.AspNetCore.Mvc;
//using WebStore.Models;
using WebStore.Data;
using WebStore.Services.Interfaces;
using WebStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers
{
    //[Route("empl/[action]/{id?}")]            // Маршрутизация для отдельного контроллера
    //[Route("Staff/{action=Index}/{Id?}")]     // Маршрутов может быть несколько
    [Authorize]
    public class EmployeesController : Controller
    {
        //private ICollection<Employee> _Employees;
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;
        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {
            _Logger = Logger;
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
        [Authorize(Roles =Role.Administrotors)]
        public IActionResult Edit(int? id)
        {
            if(id == null)
                return View(new EmployeeEditViewModel());

            var employee = _EmployeesData.GetById((int)id);

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
        [Authorize(Roles = Role.Administrotors)]
        public IActionResult Edit(EmployeeEditViewModel Model)
        {
            //Обработка модели...

            //Собственная валидация в пределах контроллеро
            if (Model.FirstName=="Усама" && Model.LastName=="Бен" && Model.Patronymic=="Ладен")
            {
                ModelState.AddModelError("", "Террористов на работу не принемаем!");
            }

            //Обработка ошибок валидации
            if (!ModelState.IsValid)
            {
                return View(Model);
            }

            var employee = new Employee()
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Age = Model.Age,
                Patronomic = Model.Patronymic,
            };

            if(Model.Id == 0)
                _EmployeesData.Add(employee);
            else if(!_EmployeesData.Edit(employee))
            {
                _Logger.LogWarning("Попытка редактирования отсутствующего сотрудника с id {0}", employee.Id); // при вызове логгера не используем интерполяцию
                return NotFound();
            }

            _Logger.LogWarning("Редактирование сотрудника с id {0}", employee.Id); // при вызове логгера не используем интерполяцию
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrotors)]
        public IActionResult Delete(int id)
        {
            if(id<0)
                return BadRequest();

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

        [HttpPost]
        [Authorize(Roles = Role.Administrotors)]
        public IActionResult DeleteConfirmed(int id)
        {
            if(!_EmployeesData.Delete(id))
            {
                _Logger.LogWarning("Попытка удаления отсутствующего сотрудника с id {0}", id); // при вызове логгера не используем интерполяцию
                return NotFound();
            }

            _Logger.LogWarning("Сотрудник с id {0} удалён", id); // при вызове логгера не используем интерполяцию
            return RedirectToAction("Index");
        }
    }
}
