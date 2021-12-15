var builder = WebApplication.CreateBuilder(args);

var servises = builder.Services;
servises.AddControllersWithViews(); // добавление инфраструктуры MVC (контроллеры и представления)



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //позволяет перехватывать все исключения приложения
}


//Загрузка инфы из файла конфигурации

//var configuration = app.Configuration;
//var greetings = configuration["CustomGreetings"];




app.MapGet("/", () => app.Configuration["CustomGreetings"]);
app.MapGet("/throw", () =>
    {
        throw new ApplicationException("Ошибка в программе"); //генерация исключения для проверки диагностики
    });

app.Run();
