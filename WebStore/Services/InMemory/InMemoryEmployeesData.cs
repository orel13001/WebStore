using WebStore.Data;
using WebStore.Models;

using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _Logger;
        private readonly ICollection<Employee> _Employees;
        private int _MaxFreeId;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger) //ILogger- интерфейс. InMemoryEmployeesData - заголовок, который пишется в лог
        {
            _Logger = Logger;
            //_Employees = TestData.Employees;
            _MaxFreeId = _Employees.DefaultIfEmpty().Max(o => o?.Id ?? 0) + 1;
        }

        public int Add(Employee employee)
        {
            if (employee == null)
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
            {
                _Logger.LogWarning("Попытка удаления отсутствующего сотрудника с id {0}", id); // при вызове логгера не используем интерполяцию
                return false;
            }

            _Employees.Remove(employee);
            _Logger.LogWarning("Сотрудник с id {0} удалён", id); // при вызове логгера не используем интерполяцию

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
            {
                _Logger.LogWarning("Попытка редактирования отсутствующего сотрудника с id {0}", employee.Id); // при вызове логгера не используем интерполяцию
                return false;
            }

            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronomic = employee.Patronomic;
            db_employee.Age = employee.Age;

            _Logger.LogWarning("Редактирование сотрудника с id {0}", employee.Id); // при вызове логгера не используем интерполяцию

            return true;
        }

        public IEnumerable<Employee> GetAll() => _Employees;

        public Employee? GetById(int id) => _Employees.FirstOrDefault(employee => employee.Id == id);

    }
}
