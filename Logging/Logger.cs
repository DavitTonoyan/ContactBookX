using System;
using System.IO;
using System.Threading.Tasks;

namespace ContactBookX.Logging
{
    sealed class Logger : ILogger
    {
        private static ILogger _instatnce;
        private string _folderPath;

        private Logger()
        {
        }

        public static ILogger CreateInstance()
        {
            if (_instatnce == null)
            {
                _instatnce = new Logger();
            }
            return _instatnce;
        }

        public async Task Error(string error)
        {
            string msg = $"ERROR: {DateTime.Now}  {error}  \n";
            await AppendToFileAsync(msg);
        }

        public async Task Information(string info)
        {
            string msg = $"Information: {DateTime.Now}  {info}  \n";
            await AppendToFileAsync(msg);
        }

        public async Task Warning(string warning)
        {
            string msg = $"Warning: {DateTime.Now}  {warning}  \n";
            await AppendToFileAsync(msg);
        }



        private async Task AppendToFileAsync(string massage)
        {
            CreateFolder();

            string fileName = @$"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Minute}.txt";
            string path = "LoggerFolder\\" + fileName;

            await File.AppendAllTextAsync(path, massage);
        }

        private void CreateFolder()
        {
            if (!Directory.Exists("LoggerFolder"))
            {
                Directory.CreateDirectory("LoggerFolder");
            }
        }

    }
}

