using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index() => View();
    }
}
