using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Diagnostics;

namespace WebStore.Infrastructure.Conventions
{
    public class TestConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            Debug.WriteLine(controller.ControllerName);

            //controller.Actions. // Список действий контроллера. Его можно редактировать или модифицировать действия.

            //controller.RouteValues. // Список маршрутов контроллера. Его можно редактировать (добавлять новые) или изменять существующие маршруты
        }
    }
}
 