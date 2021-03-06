using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Loggin;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.Services.Services.InSQL;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();
builder.Host.UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
   .MinimumLevel.Debug()
   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
   .Enrich.FromLogContext()
   .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
   .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
   .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
   .WriteTo.Seq("http://localhost:5341/")
   );

#region Настройка построителя приложения - определение содержимого (определяется набор сервисов приложения и его бизнесс-логика)
var servises = builder.Services;
servises.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //Добавление соглашений
}); // добавление инфраструктуры MVC с контроллерами и представлениими
//servises.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); // Singleton, потому что InMemory
//servises.AddSingleton<IProductData, InMemoryProductData>(); // Singleton, потому что InMemory
//servises.AddScoped<IProductData, InSqlProductData>();
//servises.AddScoped<IEmployeesData, InSqlEmployeeData>();
servises.AddScoped<ICartService, CartService>();
servises.AddScoped<ICartStore, InCookiesCartStore>();

//servises.AddScoped<IOrderService, InSqlOrderService>();

var configuration = builder.Configuration;
servises.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
servises.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
servises.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
servises.AddHttpClient<IOrderService, OrdersClient>(client => client.BaseAddress = new(configuration["WebAPI"]));

servises.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
servises.AddTransient<IDbInitializer, DbInitializer>();

servises.AddIdentity<User, Role>()              //Добавление сервиса идентификации
    //.AddEntityFrameworkStores<WebStoreDB>()     //Указание источника данных
    .AddDefaultTokenProviders();                //указание токена по умолчанию


servises.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IUserStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
    .AddTypedClient<IRoleStore<Role>, RolesClient>();


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


servises.AddAutoMapper(Assembly.GetEntryAssembly());


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

app.UseMiddleware<ExceptionHandlingMiddleware>();

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

//app.MapDefaultControllerRoute(); //Добавление обработки входящих подключений к MVC (стандартный маршрут по умолчанию "{controller=Home}/{action=Index}/{id?}")

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
