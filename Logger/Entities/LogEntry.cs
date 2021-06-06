namespace Logger.Entities
{
    public class LogEntry
    {
        public string Message { get; set; }

        public string ApplicationName { get; set; }

        public string CallType { get; set; }

        public string MachineName { get; set; }

        public string ProcessId { get; set; }

        public string ProcessName { get; set; }
    }
}
