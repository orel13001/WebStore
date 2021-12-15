var builder = WebApplication.CreateBuilder(args);

var servises = builder.Services;
servises.AddControllersWithViews(); // добавление инфраструктуры MVC (контроллеры и представления)



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //позволяет перехватывать все исключения приложения
}

app.UseRouting(); //Добавление пользовательской маршрутизации

//Загрузка инфы из файла конфигурации

//var configuration = app.Configuration;
//var greetings = configuration["CustomGreetings"];



// Стандартная маршрутизация перехватывает все обращения к корню сайта
//app.MapGet("/", () => app.Configuration["CustomGreetings"]);


app.MapGet("/throw", () =>
    {
        throw new ApplicationException("Ошибка в программе"); //генерация исключения для проверки диагностики
    });

app.MapDefaultControllerRoute(); //Добавление обработки входящих подключений к MVC (стандартный маршрут по умолчанию)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //настраиваемый маршрут. 

app.Run();
