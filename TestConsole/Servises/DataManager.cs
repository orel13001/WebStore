using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole.Data;
using TestConsole.Servises.Interfaces;

namespace TestConsole.Servises
{
    public class DataManager : IDataManager
    {

        private IDataProcessor _processor;

        public DataManager(IDataProcessor processor)
        {
            _processor = processor;
        }
        public void ProcessData(IEnumerable<DataValue> values)
        {
            foreach (var value in values)
            {
                _processor.Process(value);
            }
        }
    }
}
