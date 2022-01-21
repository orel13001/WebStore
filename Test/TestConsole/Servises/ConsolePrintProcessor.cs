using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole.Data;
using TestConsole.Servises.Interfaces;

namespace TestConsole.Servises
{
    public class ConsolePrintProcessor : IDataProcessor
    {
        public void Process(DataValue value)
        {
            Console.WriteLine($"[{value.Id}]({value.Time}):{value.Value}");
        }
    }


    public class WriteToFilePrintProcessor : IDataProcessor
    {

        public string FileName { get; set; } = "data.txt";

        public void Process(DataValue value)
        {
            using var writer = File.AppendText(FileName);

            writer.WriteLine($"[{value.Id}]({value.Time}):{value.Value}");
        }
    }
}
