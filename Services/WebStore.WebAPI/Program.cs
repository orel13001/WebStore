using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt =>
{
    const string WebStore_Domain_xml = "WebStore.Domain.xml";
    const string WebStore_WebAPI_xml = "WebStore.WebAPI.xml";
    const string debug_path = "bin/Debug/net6.0";

    //opt.IncludeXmlComments("WebStore.Domain.xml");
    //opt.IncludeXmlComments("WebStore.WebAPI.xml");

    if (File.Exists(WebStore_WebAPI_xml))
        opt.IncludeXmlComments(WebStore_WebAPI_xml);
    else if (File.Exists(Path.Combine(debug_path, WebStore_WebAPI_xml)))
        opt.IncludeXmlComments(Path.Combine(debug_path, WebStore_WebAPI_xml));

    if (File.Exists(WebStore_Domain_xml))
        opt.IncludeXmlComments(WebStore_Domain_xml);
    else if (File.Exists(Path.Combine(debug_path, WebStore_Domain_xml)))
        opt.IncludeXmlComments(Path.Combine(debug_path, WebStore_Domain_xml));
});

var configuration = builder.Configuration;

services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
services.AddTransient<IDbInitializer, DbInitializer>();

services.AddIdentity<User, Role>()              //Добавление сервиса идентификации
    .AddEntityFrameworkStores<WebStoreDB>()     //Указание источника данных
    .AddDefaultTokenProviders();                //указание токена по умолчанию

//конфигурация сервиса идентификации
services.Configure<IdentityOptions>(opt =>
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

services.AddScoped<IProductData, InSqlProductData>();
services.AddScoped<IEmployeesData, InSqlEmployeeData>();
//servises.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IOrderService, InSqlOrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
