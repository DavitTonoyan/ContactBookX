using System.IO;

namespace ContactBookX.Tables
{
    class DbConnection :IDbConnection
    {
        public string GetConnectionString()
        {

            string directory = Directory.GetCurrentDirectory();

            int index = directory.IndexOf("\\bin");
            string dataBaseName = directory.Substring(0, index) + @"\Tables\CBook.mdf";

            var connectionString = $"Server =(localdb)\\MSSQLLocalDB; Database = {dataBaseName}; Trusted_Connection = True;";

            return connectionString;
        }
    }
}
