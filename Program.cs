using ContactBookX.Logging;
using ContactBookX.Tables;
using ContactBookX.UI;
using System.Threading.Tasks;

namespace ContactBookX
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Please before start the project  GOTO -> Tables -> CBook.mdf
            // and update tables to your Database

            ILogger logger = Logger.CreateInstance();

            var ui = new WorkUI(new DbConnection(), logger);
            await ui.Start();
        }
    }
}
