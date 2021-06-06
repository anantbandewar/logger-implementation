using Logger.Constants;
using System;

namespace Logger.Entities
{
    public class TransactionEntry : LogEntryBase, ITransactionEntry
    {
        #region ITransactionEntry Members

        private object _request;

        private object _response;

        public object Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
            }
        }

        public object Response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
            }
        }

        public string ServiceUrl { get; set; }

        public string MethodName { get; set; }

        public string Status
        {
            get { return Enum.GetName(typeof(StatusOptions), this.StatusType); }
        }

        public StatusOptions StatusType { get; set; }

        public double TimeTaken { get; set; }

        public override ILogEntry CopyTo()
        {
            var entry = new TransactionEntry();
            this.CopyTo(entry);
            entry.ServiceUrl = this.ServiceUrl;
            entry.MethodName = this.MethodName;
            if (this.Request != null)
                entry.Request = this.Request;
            if (this.Response != null)
                entry.Response = this.Response;
            entry.TimeTaken = this.TimeTaken;
            entry.Message = this.Message;
            entry.MachineName = entry.GetCurrentMachineName();
            entry.IpAddress = entry.GetIPAddress();
            entry.CallType = entry.CallType;
            return entry;
        }
        #endregion

        #region Constructors

        public TransactionEntry()
        {
            this.InitializeIntrinsicProperties();
            this.SeverityType = SeverityOptions.Information;
            this.Title = this.Severity;
            this.TimeTaken = double.MinValue;
        }
        #endregion

        #region Protected Members
        protected static string Serialize(object val, SerializerType serializerType)
        {
            if (val == null)
                return null;
            var serialized = val as string;
            if (serialized != null)
                return serialized;
            switch (serializerType)
            {
                case SerializerType.DataContractSerializer:
                    return SerializationFactory.DataContractSerialize(val);
                case SerializerType.DataContractJsonSerializer:
                    return SerializationFactory.DataContractJsonSerialize(val);
                case SerializerType.JsonNetSerializer:
                    return SerializationFactory.JsonNetSerialize(val);
                case SerializerType.XmlSerializer:
                default:
                    return SerializationFactory.XmlSerialize(val);
            }
        }
        #endregion
    }
}
