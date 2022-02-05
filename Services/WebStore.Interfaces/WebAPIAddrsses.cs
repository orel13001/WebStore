using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Interfaces
{
    public static class WebAPIAddrsses
    {
        public const string Employees = "api/v1/employees";
        public const string Orders = "api/v1/orders";
        public const string Products = "api/v1/products";
        public const string Values = "api/v1/values";

        public static class Identity
        {
            public const string Users = "api/v1/users";
            public const string Roles = "api/v1/roles";
        }
    }
}
