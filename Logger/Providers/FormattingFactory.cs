using Logger.Interfaces;
using System;

namespace Logger.Providers
{
    /// <summary>
    /// The factory responsible for maintaining the formatter instance and selection.
    /// </summary>
    public class FormattingFactory
    {
        private static ILogEntryFormatter _logEntryFormatter;

        /// <summary>
        /// The current formatter set in the system. If nothing is set, this returns the 
        /// <see cref="DefaultFormatter"/> class instance.
        /// </summary>
        public static ILogEntryFormatter CurrentFormatter
        {
            get { return _logEntryFormatter ?? (_logEntryFormatter = new DefaultFormatter()); }
        }

        /// <summary>
        /// Sets the formatter which can then be retrieved from the <seealso cref="CurrentFormatter"/> property.
        /// </summary>
        /// <param name="logEntryFormatter">The formatter instance to be set.</param>
        public static void SetFormatter(ILogEntryFormatter logEntryFormatter)
        {
            _logEntryFormatter = logEntryFormatter ?? throw new ArgumentNullException("logEntryFormatter");
        }
    }
}
