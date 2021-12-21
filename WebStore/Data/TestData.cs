using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new List<Employee>()
        {
            new Employee() { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronomic = "Иванович", Age = 27 },
            new Employee() { Id = 2, LastName = "Петров", FirstName = "Петр", Patronomic = "Петрович", Age = 30 },
            new Employee() { Id = 3, LastName = "Сидоров", FirstName = "Сидр", Patronomic = "Сидорович", Age = 27 }
        };
    }
}
