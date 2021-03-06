using Logger.Entities;
using Logger.Interfaces;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Logger.Providers
{
    /// <summary>
    /// The default formatter for formatting exceptions, 
    /// this class can be inherited for ease of translating exceptions into entries.
    /// </summary>
    public class DefaultFormatter : ILogEntryFormatter
    {
        #region ILogEntryFormatter Members

        /// <summary>
        /// Default implementation, returns the object "as is", this method should be overridden in 
        /// case of any customizations required e.g. masking capability addition.
        /// Also think of overriding <seealso cref="FormatEvent"/>
        /// </summary>
        /// <param name="exceptionEntry">The exception entry to be formatted.</param>
        /// <returns>Formatted exception.</returns>
        public virtual IExceptionEntry FormatException(IExceptionEntry exceptionEntry)
        {
            return exceptionEntry;
        }

        /// <summary>
        /// Responsible for converting exceptions into <seealso cref="IExceptionEntry"/> object, 
        /// gets the entry empty object and passes to <seealso cref="FormatException(System.Exception, IExceptionEntry)"/>
        /// The method calls <seealso cref="FormatException(IExceptionEntry)"/> at the end for
        /// final formatting, override this method only to change how an exception is converted to an entry.
        /// </summary>
        /// <param name="exception">The exception entry to be converted.</param>
        /// <returns>An <see cref="IExceptionEntry"/> object.</returns>
        public virtual IExceptionEntry FormatException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");
            IExceptionEntry entry = new ExceptionEntry();
            entry = FormatException(exception, entry);
            return FormatException(entry);
        }

        /// <summary>
        /// Default implementation, returns the object with the request and response compressed, 
        /// this method should be overridden in case of any customizations 
        /// required e.g. masking capability addition. To keep compression, the base method should be called 
        /// at the end.
        /// Also think of overriding <seealso cref="FormatException(IExceptionEntry)"/>
        /// </summary>
        /// <param name="eventEntry">The event entry to be formatted.</param>
        /// <returns>Formatted event.</returns>
        public virtual IEventEntry FormatEvent(IEventEntry eventEntry)
        {
            var request = eventEntry.Request;
            if (!string.IsNullOrWhiteSpace(request))
            {
                request = request.Compress();
                eventEntry.SetRequestString(request);
            }
            var response = eventEntry.Response;
            if (!string.IsNullOrWhiteSpace(response))
            {
                response = response.Compress();
                eventEntry.SetResponseString(response);
            }
            return eventEntry;
        }
        #endregion

        #region Other Methods
        /// <summary>
        /// Formats and translates the exception into an <see cref="IExceptionEntry"/> object.
        /// </summary>
        /// <param name="exception">The exception to format.</param>
        /// <param name="entry">The <see cref="IExceptionEntry"/> object to format.</param>
        /// <returns>The entry object provided with excpetion information added.</returns>
        protected internal virtual IExceptionEntry FormatException(Exception exception, IExceptionEntry entry)
        {
            if (exception.InnerException != null)
            {
                entry.AddInnerExceptionMessage(exception.InnerException.ToString());
                var builder = new StringBuilder();
                var innerException = exception.InnerException;
                builder.AppendLine("----Flattened Exception Data[BEGIN]----");
                while (innerException != null)
                {
                    foreach (var pair in innerException.Data.Cast<DictionaryEntry>())
                    {
                        builder.Append(pair.Key).Append(":").Append(pair.Value).AppendLine("---");
                    }
                    innerException = innerException.InnerException;
                }
                builder.AppendLine("----Flattened Exception Data[END]----");
                entry.AddInnerExceptionMessage(builder.ToString());
            }
            entry.AddMessage(exception.Message);
            entry.Source = exception.Source;
            entry.Type = exception.GetType().Name;
            entry.StackTrace = exception.StackTrace;
            //entry.TargetSite = exception.TargetSite == null ? string.Empty : exception.TargetSite.Name;
            if (exception.Data.Count > 0)
            {
                entry.AddMessage("---Main Exception Data---");
                try
                {
                    foreach (var pair in exception.Data.Cast<DictionaryEntry>())
                    {
                        entry.AddMessage(pair.Key + ": " + pair.Value);
                    }
                }
                catch { }
            }
            entry.ThreadIdentity = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);
            return entry;
        }
        #endregion
    }
}
