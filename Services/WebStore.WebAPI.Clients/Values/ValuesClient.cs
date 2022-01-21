
using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        private HttpClient _Client;

        public ValuesClient(HttpClient client) : base(client, "api/values")
        {

        }

        public void Add(string value)
        {
            
        }

        public int Count()
        {
            
        }

        public void Delete(int Id)
        {
            
        }

        public void Edit(int Id, string Value)
        {
            
        }

        public string GetById(int Id)
        {
            
        }

        public IEnumerable<string> GetValues()
        {
            
        }
    }
}
