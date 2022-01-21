using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole.Data;

namespace TestConsole.Servises.Interfaces
{
    public interface IDataManager
    {
        void ProcessData(IEnumerable<DataValue> values);
    }

    public interface IDataProcessor
    {
        void Process(DataValue value);
    }
}
