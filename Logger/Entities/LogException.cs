using System;

namespace Logger.Entities
{
    /// <summary>
    /// The exception thrown by the framework in case re-throwing of exceptions is enabled, the actual exception
    /// is usually contained in the inner exception.
    /// This class should not be used outside of the framework apart from catching the same.
    /// </summary>
    public class LogException : Exception
    {
        #region Constructors
        public LogException() : base() { }

        public LogException(string message) : base(message) { }

        public LogException(string message, Exception innerException) : base(message, innerException) { }
        #endregion
    }
}
