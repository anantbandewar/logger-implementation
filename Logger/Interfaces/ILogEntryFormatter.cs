using Logger.Entities;
using System;

namespace Logger.Interfaces
{
    public interface ILogEntryFormatter
    {
        /// <summary>
        /// Formats the event, adds any information required into it.
        /// </summary>
        /// <param name="eventEntry">The entry to be formatted.</param>
        /// <returns>Formatted entry.</returns>
        IEventEntry FormatEvent(IEventEntry eventEntry);

        /// <summary>
        /// Formats the exception, adds any information required into it.
        /// </summary>
        /// <param name="exceptionEntry">The exception to format.</param>
        /// <returns>Formatted exception.</returns>
        IExceptionEntry FormatException(IExceptionEntry exceptionEntry);

        /// <summary>
        /// Translates the exception into an entry and adds any required information into it.
        /// </summary>
        /// <param name="exception">The excpetion to format.</param>
        /// <returns>Formatted exception.</returns>
        IExceptionEntry FormatException(Exception exception);
    }
}
