using Logger.Entities;
using Logger.Interfaces;
using Logger.Properties;
using Logger.Providers;
using System;

namespace Logger
{
    public class FailSafeLogFactory
    {
        #region Fields & Properties
        private static bool ReThrowExceptionsDisabled { get; set; }

        private static readonly IFailSafeLogger FailSafeLogger;
        #endregion

        #region Constructors
        static FailSafeLogFactory()
        {
            FailSafeLogger = new EventViewerLogger();
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Logs the exception using the <see cref="IFailSafeLogger"/> implementation.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="overrideRethrowSettingToSuppress">When set to true, overrides the setting to supress the exception and not re-throw the same.</param>
        /// <exception cref="LogException"></exception>
        public static void Log(Exception ex, bool overrideRethrowSettingToSuppress = false)
        {
            FailSafeLogger.Log(ex);
            if (!overrideRethrowSettingToSuppress && !ReThrowExceptionsDisabled)
                throw new LogException(LogResources.GenericLogExceptionMessage, ex);
        }

        internal static void SetFailOverLoggingBehaviour(bool reThrowExceptions)
        {
            ReThrowExceptionsDisabled = !reThrowExceptions;
        }
        #endregion
    }
}
