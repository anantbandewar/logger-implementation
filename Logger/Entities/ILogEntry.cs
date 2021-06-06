using Logger.Constants;
using System;
using System.Collections.Generic;

namespace Logger.Entities
{
    public interface ILogEntry
    {
        /// <summary>
        /// Uniquely identifies an entry
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the UTC time stamp.
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the machine name in which the event occurred.
        /// </summary>
        string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the process id in which the application is running.
        /// </summary>
        string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the process name in which the application is running.
        /// </summary>
        string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the additional info, consider using <seealso cref="AddAdditionalInfo"/>
        /// </summary>
        IDictionary<string, string> AdditionalInfo { get; }

        /// <summary>
        /// Gets the message added via <seealso cref="AddMessage"/> function.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the severity string converted from the set value of <seealso cref="SeverityType"/>
        /// </summary>
        string Severity { get; }

        /// <summary>
        /// Gets or sets the <see cref="SeverityOptions"/>.
        /// </summary>
        SeverityOptions SeverityType { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Adds a message into the log entry incrementally.
        /// </summary>
        /// <param name="message">The message to be added in the entry.</param>
        void AddMessage(string message);

        /// <summary>
        /// Adds an additional info
        /// </summary>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The value against the key.</param>
        void AddAdditionalInfo(string key, string value);

        /// <summary>
        /// Clones the log entry.
        /// </summary>
        ILogEntry CopyTo();

        /// <summary>
        /// Gets or sets the title of the log.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the call type.
        /// </summary>
        string CallType { get; set; }
    }
}
