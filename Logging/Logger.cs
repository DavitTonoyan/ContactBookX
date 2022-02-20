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
            string msg = $"Information: {DateTime.Now}  {warning}  \n";
            await AppendToFileAsync(msg);
        }



        private async Task AppendToFileAsync(string massage)
        {
            if (_folderPath == null)
            {
                _folderPath = GetFolderName();
            }

            string fileName = @$"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Minute}.txt";
            string path = _folderPath + "\\" + fileName;

            await File.AppendAllTextAsync(path, massage);
        }
        private string GetFolderName()
        {
            string directory = Directory.GetCurrentDirectory();

            int index = directory.IndexOf("\\bin");
            string folderPath = directory.Substring(0, index) + "\\LoggerFolder";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
    }
}

