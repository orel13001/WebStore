using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();
    }
}
