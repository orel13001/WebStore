

using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    public class BreadCrumsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
