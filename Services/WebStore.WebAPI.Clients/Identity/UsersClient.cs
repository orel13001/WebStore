using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient
    {
        public UsersClient(HttpClient client) : base(client, WebAPIAddrsses.Identity.Users)
        {
        }
    }
}
