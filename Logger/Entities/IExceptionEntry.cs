namespace Logger.Entities
{
    public interface IExceptionEntry : ILogEntry
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// Gets or sets the target site.
        /// </summary>
        string TargetSite { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the thread identity.
        /// </summary>
        string ThreadIdentity { get; set; }

        /// <summary>
        /// Gets or sets the inner exception details.
        /// </summary>
        string InnerExceptions { get; }

        /// <summary>
        /// Sets the inner exception details incrementally.
        /// </summary>
        /// <param name="message"></param>
        void AddInnerExceptionMessage(string message);
    }
}
