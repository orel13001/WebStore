using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    public class Role : IdentityRole
    {
        public const string Administrotors = "Administrotors";
        public const string Users = "Users";
    }
}
