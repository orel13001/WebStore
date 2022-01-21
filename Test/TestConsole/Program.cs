using Microsoft.Extensions.DependencyInjection;
using TestConsole.Data;
using TestConsole.Servises;
using TestConsole.Servises.Interfaces;

var service_collection = new ServiceCollection();

service_collection.AddSingleton<IDataManager, DataManager>();
service_collection.AddSingleton<IDataProcessor, ConsolePrintProcessor>();
//service_collection.AddSingleton<IDataProcessor, WriteToFilePrintProcessor>();

var provider = service_collection.BuildServiceProvider();

var service = provider.GetRequiredService<IDataManager>();

var data = Enumerable.Range(1, 100).Select(o => new DataValue
{
    Id = o,
    Value = $"Value-{o}",
    Time = DateTime.Now,
}); 

service.ProcessData(data);

Console.ReadKey();