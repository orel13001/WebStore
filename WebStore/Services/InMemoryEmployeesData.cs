using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ICollection<Employee> _Employees;
        private int _MaxFreeId;

        public InMemoryEmployeesData()
        {
            _Employees = TestData.Employees;
            _MaxFreeId = _Employees.DefaultIfEmpty().Max(o => o?.Id ?? 0) + 1;
        }

        public int Add(Employee employee)
        {
            if(employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee))
                return employee.Id;

            employee.Id = _MaxFreeId++;
            _Employees.Add(employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee == null)
                return false;

            _Employees.Remove(employee);
            return true;
        }

        public bool Edit(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee))
                return true;

            var db_employee = GetById(employee.Id);
            if (db_employee == null)
                return false;

            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronomic = employee.Patronomic;
            db_employee.Age = employee.Age;

            return true;
        }

        public IEnumerable<Employee> GetAll() => _Employees;

        public Employee? GetById(int id) => _Employees.FirstOrDefault(employee => employee.Id == id);
        
    }
}
