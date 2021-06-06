using Logger.Constants;

namespace Logger.Entities
{
    public interface ITransactionEntry : ILogEntry
    {
        string ServiceUrl { get; set; }

        string MethodName { get; set; }

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
    }
}
