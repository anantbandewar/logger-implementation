using Logger.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace Logger.Entities
{
    /// <summary>
    /// Default implementation of <see cref="ILogEntry"/>.
    /// </summary>
    public abstract class LogEntryBase : ILogEntry
    {
        protected readonly StringBuilder MessageBuilder;

        #region ILogEntry Members

        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the log.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the timestamp associated with the log.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the machine name in which the entry was created.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the process id in which the logger was functioning.
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the process name in which the logger was functioning.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets any additional info associated with the log.
        /// </summary>
        public IDictionary<string, string> AdditionalInfo { get; protected set; }

        /// <summary>
        /// Gets or sets the message associated with the log, preferably use <seealso cref="AddMessage"/>
        /// </summary>
        public virtual string Message
        {
            get { return MessageBuilder.ToString(); }
            set
            {
                MessageBuilder.Length = 0;
                MessageBuilder.Append(value);
            }
        }

        /// <summary>
        /// Gets the severity string representation.
        /// </summary>
        public string Severity
        {
            get { return Enum.GetName(typeof(SeverityOptions), this.SeverityType); }
        }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        public SeverityOptions SeverityType { get; set; }

        /// <summary>
        /// Gets or sets the ip address associated with the request.
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the application name in which the logger is functioning.
        /// </summary>
        public string ApplicationName { get; set; }

        public string CallType { get; set; }

        public void AddMessage(string message)
        {
            MessageBuilder.AppendLine(message);
        }

        public void AddAdditionalInfo(string key, string value)
        {
            AdditionalInfo[key] = value;
        }

        public abstract ILogEntry CopyTo();
        #endregion

        #region Constructors

        protected LogEntryBase()
        {
            MessageBuilder = new StringBuilder();
            AdditionalInfo = new Dictionary<string, string>();
        }

        #endregion

        #region Protected Methods

        protected void InitializeIntrinsicProperties()
        {
            this.Timestamp = GetCurrentTimeStamp();
            this.MachineName = GetCurrentMachineName();
            this.ApplicationName = GetCurrentApplicationName();
            var processInfo = GetCurrentProcessInfo();
            this.ProcessId = processInfo.Item1.ToString(CultureInfo.InvariantCulture);
            this.ProcessName = processInfo.Item2;
            this.IpAddress = GetIPAddress();
        }

        protected void CopyTo<T>(T target) where T : ILogEntry
        {
            foreach (var pair in this.AdditionalInfo)
            {
                target.AddAdditionalInfo(pair.Key, pair.Value);
            }
            target.Id = this.Id;
            if (DateTime.MinValue != this.Timestamp)
                target.Timestamp = this.Timestamp;
            if (!string.IsNullOrEmpty(this.ApplicationName))
                target.ApplicationName = this.ApplicationName;
            if (!string.IsNullOrEmpty(this.IpAddress))
                target.IpAddress = this.IpAddress;
            if (!string.IsNullOrEmpty(this.MachineName))
                target.MachineName = this.MachineName;
            if (!string.IsNullOrEmpty(this.Message))
                target.AddMessage(this.Message);
            if (!string.IsNullOrEmpty(this.ProcessId))
                target.ProcessId = this.ProcessId;
            if (!string.IsNullOrEmpty(this.ProcessName))
                target.ProcessName = this.ProcessName;
            if (!string.IsNullOrEmpty(this.Title))
                target.Title = this.Title;
            if (!string.IsNullOrEmpty(this.CallType))
                target.CallType = this.CallType;
        }

        protected virtual DateTime GetCurrentTimeStamp()
        {
            return DateTime.UtcNow;
        }

        private static string _machineName;

        protected virtual string GetCurrentMachineName()
        {
            try
            {
                return _machineName ?? (_machineName = Environment.MachineName);
            }
            catch
            {
                return null;
            }
        }

        private static string _applicationName;

        protected virtual string GetCurrentApplicationName()
        {
            return _applicationName;
        }

        private static Tuple<int, string> _currentProcessInfo;

        protected virtual Tuple<int, string> GetCurrentProcessInfo()
        {
            if (_currentProcessInfo != null)
                return _currentProcessInfo;
            try
            {
                var process = System.Diagnostics.Process.GetCurrentProcess();
                return (_currentProcessInfo = new Tuple<int, string>(process.Id, process.ProcessName));
            }
            catch
            {
                return null;
            }
        }

        private static string _ipAdresses = null;

        protected virtual string GetIPAddress()
        {
            try
            {
                if (_ipAdresses == null)
                {
                    var sb = new StringBuilder();
                    var ipEntry = Dns.GetHostEntryAsync(Dns.GetHostName()).GetAwaiter().GetResult();
                    var addrList = ipEntry.AddressList;
                    if (addrList != null)
                    {
                        foreach (IPAddress addr in addrList)
                        {
                            sb.AppendFormat("{0},", addr);
                        }
                    }
                    if (sb.Length > 0)
                        sb.Remove(sb.Length - 1, 1);
                    _ipAdresses = sb.ToString();
                }
            }
            catch
            {
                _ipAdresses = string.Empty;
            }
            return _ipAdresses;
        }

        #endregion
    }
}
