using System.Threading.Tasks;

namespace ContactBookX.Logging
{
    internal interface ILogger
    {
        Task Information(string info);
        Task Warning(string warning);
        Task Error(string error);
    }
}
