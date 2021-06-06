using Logger.Constants;
using Logger.Entities;
using Logger.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace Logger.Infra
{
    public class LoggingHelper
    {
        private ILogger _logger = null;

        private readonly AppSettings _appSettings = null;

        public LoggingHelper(ILogger logger, IOptions<AppSettings> appSettingsOptions)
        {
            _logger = logger;
            _appSettings = appSettingsOptions.Value;
        }

        public void LogAsync(string methodName, object request, object response, StatusOptions statusOption, double timeTaken, string callType)
        {
            try
            {
                if (_appSettings.DisableLogs)
                    return;

                var transactionEntry = new TransactionLogEntry()
                {
                    MethodName = methodName,
                    Request = request,
                    Response = response,
                    Status = statusOption,
                    TimeTaken = timeTaken,
                    CallType = callType,
                    ApplicationName = Constants.ApplicationName,
                };

                _logger.WriteAsync(transactionEntry);
            }
            catch (Exception ex)
            {
                LogAsync(methodName, ex, callType);
            }
        }

        public void LogAsync(string methodName, Exception ex, string callType)
        {
            try
            {
                if (_appSettings.DisableExceptions)
                    return;

                var expEntry = new ExceptionEntry()
                {
                    MethodName = methodName,
                    ApplicationName = Constants.ApplicationName,
                    Source = ex.Source,
                    StackTrace = ex.StackTrace,
                    MachineName = Environment.MachineName,
                    Message = ex.Message,
                    Type = callType
                };

                _logger.LogException(expEntry);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
