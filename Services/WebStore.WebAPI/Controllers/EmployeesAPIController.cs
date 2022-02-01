using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(WebAPIAddrsses.Employees)] // http://localhost:5001/api/employees
    public class EmployeesAPIController : ControllerBase
    {

        private readonly IEmployeesData _employeesData;
        public EmployeesAPIController(IEmployeesData employeesData) => _employeesData = employeesData;


        /// <summary>
        /// Получение всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeesData.GetAll();
            return Ok(employees);
        }


        /// <summary>
        /// Получение сотрудника по id
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var employee = _employeesData.GetById(id);
            if(employee is null)
                return NotFound();

            return Ok(employee);
        } 


        /// <summary>
        /// Добавление нового сотрудника
        /// </summary>
        /// <param name="employee">Добавляемый сотрудник</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Employee))]
        public IActionResult Add(Employee employee)
        {
            var id = _employeesData.Add(employee);
            return CreatedAtAction(nameof(GetById), new{ id = id}, employee);
        }


        /// <summary>
        /// Изменение данных сотрудника
        /// </summary>
        /// <param name="employee">Структура с инфоормацией о сотруднике</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public IActionResult Update(Employee employee)
        {
            var success = _employeesData.Edit(employee);
            return Ok(success);
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public IActionResult Delete(int id)
        {
            var result = _employeesData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
