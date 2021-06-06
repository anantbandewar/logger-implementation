using Logger.Constants;

namespace Logger.Entities
{
    public interface IEventEntry : ILogEntry
    {
        /// <summary>
        /// Gets the xml request set in the event.
        /// </summary>
        string Request { get; }

        /// <summary>
        /// Gets the xml response set in the event.
        /// </summary>
        string Response { get; }

        /// <summary>
        /// Sets the request object to be serialized in a deferred manner.
        /// </summary>
        object RequestObject { get; set; }

        /// <summary>
        /// Sets the response object to be serialized in a deferred manner.
        /// </summary>
        object ResponseObject { get; set; }

        /// <summary>
        /// Sets the serializer type to be used for serialization of the request and response objects.
        /// </summary>
        SerializerType ReqResSerializerType { get; set; }

        /// <summary>
        /// Gets or sets the provider id.
        /// </summary>
        int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        string Status { get; }

        /// <summary>
        /// Gets or sets the <see cref="StatusOptions"/>.
        /// </summary>
        StatusOptions StatusType { get; set; }

        /// <summary>
        /// Gets or sets the time taken for the operation.
        /// </summary>
        double TimeTaken { get; set; }

        /// <summary>
        /// Sets the <seealso cref="XmlRequest"/> value.
        /// </summary>
        /// <param name="request">The value to be set.</param>
        void SetRequestString(string request);

        /// <summary>
        /// Sets the <seealso cref="XmlResponse"/> value.
        /// </summary>
        /// <param name="response">The value to be set.</param>
        void SetResponseString(string response);
    }
}
