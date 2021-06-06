using Logger.Constants;
using Logger.Interfaces;
using System;
using System.Text;

namespace Logger.Providers
{
    public class GZipCompressionProvider : ICompressionProvider
    {
        public string Compress(string dataToCompress)
        {
            var compressed = ByteArrayProcesser.Compress(Encoding.UTF8.GetBytes(dataToCompress), DataCompressionLevel.Optimal);
            return Convert.ToBase64String(compressed);
        }

        public string Decompress(string dataToDecompress)
        {
            var decompressed = ByteArrayProcesser.Decompress(Convert.FromBase64String(dataToDecompress));
            return Encoding.UTF8.GetString(decompressed);
        }
    }
}
