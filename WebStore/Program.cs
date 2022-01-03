using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Настройка построителя приложения - определение содержимого (определяется набор сервисов приложения и его бизнесс-логика)
var servises = builder.Services;
servises.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //Добавление соглашений
}); // добавление инфраструктуры MVC с контроллерами и представлениими
servises.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton, потому что InMemory
//servises.AddSingleton<IProductData, InMemoryProductData>(); // Singleton, потому что InMemory
servises.AddScoped<IProductData, InSqlProductData>();

servises.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
servises.AddTransient<IDbInitializer, DbInitializer>();

//servises.AddMvc(); // базовая инфраструктура MVC
//servises.AddControllers(); // Добавление только контроллеров (обычно для WebAPI )


#endregion


var app = builder.Build();

#region Определение конвейера обработки входящих подключений из блоков промежуточного ПО

await using(var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: true);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //позволяет перехватывать все исключения приложения
}

app.Map("/testpath", async context => await context.Response.WriteAsync("TestMiddleWare")); //Простое самописное промежуточное ПО ("/testpath" - адрес по которому оно вызывается. Далее выполняемый метод)

//Добавляем в конвейер  обработки входного подключения промежуточного ПО, которое будет обнаруживать запрос к файлу в wwwroot
//(по сути добавление файл-сервера для стандартных статических ресурсов)
app.UseStaticFiles();

app.UseRouting(); //Добавление пользовательской маршрутизации

app.UseMiddleware<TestMiddleware>();

app.UseWelcomePage("/welcome"); //Добавление ПО встроенной странички приветствия

//Загрузка инфы из файла конфигурации

//var configuration = app.Configuration;
//var greetings = configuration["CustomGreetings"];



// Стандартная маршрутизация перехватывает все обращения к корню сайта
//app.MapGet("/", () => app.Configuration["CustomGreetings"]);


app.MapGet("/throw", () =>
    {
        throw new ApplicationException("Ошибка в программе"); //генерация исключения для проверки диагностики
    });

app.MapDefaultControllerRoute(); //Добавление обработки входящих подключений к MVC (стандартный маршрут по умолчанию "{controller=Home}/{action=Index}/{id?}")
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //настраиваемый маршрут. 

#endregion
app.Run();
