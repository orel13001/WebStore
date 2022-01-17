using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InCookies;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Настройка построителя приложения - определение содержимого (определяется набор сервисов приложения и его бизнесс-логика)
var servises = builder.Services;
servises.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //Добавление соглашений
}); // добавление инфраструктуры MVC с контроллерами и представлениими
//servises.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton, потому что InMemory
//servises.AddSingleton<IProductData, InMemoryProductData>(); // Singleton, потому что InMemory
servises.AddScoped<IProductData, InSqlProductData>();
servises.AddScoped<IEmployeesData, InSqlEmployeeData>();
servises.AddScoped<ICartService, InCookiesCartService>();

servises.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
servises.AddTransient<IDbInitializer, DbInitializer>();

servises.AddIdentity<User, Role>()              //Добавление сервиса идентификации
    .AddEntityFrameworkStores<WebStoreDB>()     //Указание источника данных
    .AddDefaultTokenProviders();                //указание токена по умолчанию

//конфигурация сервиса идентификации
servises.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

//настройка cookis
servises.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStore_LSA";
    opt.Cookie.HttpOnly = true;

    //opt.Cookie.Expiration = TimeSpan.FromDays(10); //устарело
    opt.ExpireTimeSpan = TimeSpan.FromDays(10);


    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";

    opt.SlidingExpiration = true;

});

//servises.AddMvc(); // базовая инфраструктура MVC
//servises.AddControllers(); // Добавление только контроллеров (обычно для WebAPI )


#endregion


var app = builder.Build();

#region Определение конвейера обработки входящих подключений из блоков промежуточного ПО

await using(var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: false).ConfigureAwait(true);
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

app.UseAuthentication();
app.UseAuthorization();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}" // Маршрут для отдельных областей сайта.
    );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //настраиваемый маршрут по умолчанию. Перехватывает обращения, которые не были обработаны иными маршрутами. Идёт последним. 

});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"); //настраиваемый маршрут. 

#endregion
app.Run();
