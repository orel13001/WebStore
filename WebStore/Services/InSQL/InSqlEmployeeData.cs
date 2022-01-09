using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class InSqlEmployeeData : IEmployeesData
    {
        private readonly WebStoreDB _db;

        public InSqlEmployeeData(WebStoreDB db) => _db = db;

        public int Add(Employee employee)
        {
            _db.Employees.Add(new Employee
            {
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronomic = employee.Patronomic,
                Age = employee.Age,
            });
            _db.SaveChangesAsync();
            return _db.Employees.Select(e => e.Id).Max();
        }

        public bool Delete(int id)
        {
            var empDel = _db.Employees.FirstOrDefault(e => e.Id == id);
            if (empDel != null)
            {
                _db.Employees.Remove(empDel);
                _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Edit(Employee employee)
        {
            var empEd = _db.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if (empEd != null)
            {
                empEd.LastName = employee.LastName;
                empEd.FirstName = employee.FirstName;
                empEd.Patronomic = employee.Patronomic;
                empEd.Age = employee.Age;
                _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public IEnumerable<Employee> GetAll() => _db.Employees;

        public Employee? GetById(int id) => _db.Employees.FirstOrDefault(x => x.Id == id);
    }
}
