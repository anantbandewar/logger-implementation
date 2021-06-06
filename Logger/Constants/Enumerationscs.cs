namespace Logger.Constants
{
    /// <summary>
    /// Defines the severity level of events.
    /// </summary>
    public enum SeverityOptions : int
    {
        Undefined = 0,
        Critical = 1,
        Error = 2,
        Warning = 4,
        Information = 8,
        Verbose = 16
    }

    /// <summary>
    /// The supported serializers in the framework. See <see cref="SerializationFactory"/> for details.
    /// </summary>
    public enum SerializerType
    {
        DataContractSerializer = 0,
        XmlSerializer = 1,
        DataContractJsonSerializer = 2,
        JsonNetSerializer = 3
    }

    public enum StatusOptions : int
    {
        Success = 0,
        Failure = 1
    }

    public enum PriorityOptions : int
    {
        Undefined = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum DataCompressionLevel
    {
        Optimal,
        Fastest,
        NoCompression
    }
}
