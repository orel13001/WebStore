using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient
    {

        public EmployeesClient(HttpClient client) : base(client, "api/employees")
        {

        }

    }
}
