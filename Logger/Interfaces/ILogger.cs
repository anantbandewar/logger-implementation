using Logger.Entities;
using System;
using System.Threading.Tasks;

namespace Logger.Interfaces
{
    public interface ILogger
    {
        void LogException(IExceptionEntry entry);
        
        Task WriteAsync(LogEntry entry);
    }
}
