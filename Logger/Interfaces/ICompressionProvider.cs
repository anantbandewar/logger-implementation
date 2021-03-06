namespace Logger.Interfaces
{
    /// <summary>
    /// Compression provider to compress string
    /// </summary>
    public interface ICompressionProvider
    {
        /// <summary>
        /// Compress string
        /// </summary>
        /// <param name="dataToCompress">string that needs to be compressed</param>
        /// <returns>Compressed string</returns>
        string Compress(string dataToCompress);

        /// <summary>
        /// Decompress string
        /// </summary>
        /// <param name="dataToDecompress">string that needs to be decompressed</param>
        /// <returns>Decompressed string</returns>
        string Decompress(string dataToDecompress);
    }
}
