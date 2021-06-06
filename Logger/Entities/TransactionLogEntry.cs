using Logger.Constants;
using System.Collections.Generic;

namespace Logger.Entities
{
    public class TransactionLogEntry : LogEntry
    {
        public string MethodName { get; set; }

        public string ServiceUrl { get; set; }

        public string SessionId { get; set; }

        public object Request { get; set; }

        public object Response { get; set; }

        public StatusOptions Status { get; set; }

        public double TimeTaken { get; set; }

        public List<KeyValuePair<string, string>> RequestHeaders { get; } = new List<KeyValuePair<string, string>>();

        public Dictionary<string, string> AdditionalData { get; set; }
    }
}
