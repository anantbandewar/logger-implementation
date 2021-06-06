using Logger.Entities;
using Logger.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Logger.Providers
{
    /// <summary>
    /// A <see cref="IFailSafeLogger"/> implementation, logs in the event viewer.
    /// </summary>
    public sealed class EventViewerLogger : IFailSafeLogger
    {
        private const string Source = "Tavisca.Frameworks.Logging";

        private const string LogCategory = "Application";

        public void Log(Exception ex)
        {
            try
            {
                IExceptionEntry e;
                try
                {
                    e = ex.ToEntry();
                }
                catch (Exception)
                {
                    var formatter = new DefaultFormatter();
                    e = formatter.FormatException(ex, new ExceptionEntry()); //in case the DI/custom formatter is failing.
                }
                var sb = new StringBuilder()
                    .Append("Application Name: ")
                    .AppendLine(string.IsNullOrWhiteSpace(e.ApplicationName) ? "<was blank>" : e.ApplicationName)
                    .Append("Exception Type: ")
                    .AppendLine(e.Type)
                    .AppendLine("---------Message-------------")
                    .AppendLine(e.Message)
                    .AppendLine(string.Empty).AppendLine("-----------Stack Trace-----------------")
                    .AppendLine(e.StackTrace)
                    .AppendLine("---------Process Name-------------")
                    .AppendLine(e.ProcessName)
                    .AppendLine("---------Entry Assembly Name-------------")
                    .AppendLine(GetEntryAssemblyName())
                    .AppendLine("---------Inner Exceptions-------------")
                    .AppendLine(e.InnerExceptions);
                ILoggerFactory loggerFactory = new LoggerFactory();
                loggerFactory.CreateLogger(Source).LogError(sb.ToString(), null);
            }
            catch { }
        }

        private string GetEntryAssemblyName()
        {
            try
            {
                var entryAsembly = System.Reflection.Assembly.GetEntryAssembly();
                return entryAsembly != null ? entryAsembly.GetName().Name : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
