using Logger.Constants;
using Logger.Entities;
using Logger.Providers;
using System;
using System.Collections.Generic;

namespace Logger
{
    /// <summary>
    /// Contains extension utility methods for the logging framework, this can be used outside of the framework as well.
    /// </summary>
    public static class Helpers
    {
        static Helpers()
        {
            // Logger.EnsureConfigurationLoad();
        }

        /// <summary>
        /// Converts the <see cref="Exception"/> into <see cref="IExceptionEntry"/>.
        /// </summary>
        /// <param name="exception">The exception to be converted.</param>
        /// <returns>Returns the exception entry filled with the values of <param name="exception"></param></returns>
        public static IExceptionEntry ToEntry(this Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");
            return FormattingFactory.CurrentFormatter.FormatException(exception);
        }

        public static string Compress(this string dataToCompress)
        {
            return CompressionProviderFactory.CurrentProvider.Compress(dataToCompress ?? string.Empty);
        }

        public static string DeCompress(this string dataToDecompress)
        {
            return CompressionProviderFactory.CurrentProvider.Decompress(dataToDecompress);
        }

        public static TransactionLogEntry GetTransactionEntry(object request, object response, string methodName,
            string serviceUrl, StatusOptions status, string applicationName, string environment, double timeTaken = 0, string title = null,
            Dictionary<string, string> additionalInfo = null)
        {
            return new TransactionLogEntry
            {
                MethodName = methodName,
                ServiceUrl = serviceUrl,
                Request = request,
                Response = response,
                Status = status,
                TimeTaken = timeTaken,
                AdditionalData = additionalInfo,
                ApplicationName = applicationName
            };
        }

        public static class Constants
        {
            public static class Logging
            {
                public static class Exceptions
                {
                    public static readonly string Category = "RedisLogger";
                    public static readonly string Title = "Error";
                }
                public static readonly string Category = "RedisLogger";
                public static readonly string Title = "Transaction";
            }
        }
    }
}
