using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[employees]")] // http://localhost:5001/api/employees
    public class EmployeesAPIController : ControllerBase
    {

        private readonly IEmployeesData _employeesData;
        public EmployeesAPIController(IEmployeesData employeesData) => _employeesData = employeesData;

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeesData.GetAll();
            return Ok(employees);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var employee = _employeesData.GetById(id);
            if(employee is null)
                return NotFound();

            return Ok(employee);
        } 

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            var id = _employeesData.Add(employee);
            return CreatedAtAction(nameof(GetById), new{ id = id}, employee);
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            _employeesData.Edit(employee);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _employeesData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
