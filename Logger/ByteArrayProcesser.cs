using Logger.Constants;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;

namespace Logger
{
    public class ByteArrayProcesser
    {
        private static readonly ConcurrentDictionary<DataCompressionLevel, CompressionLevel> CompressionLevelsMapping;

        static ByteArrayProcesser()
        {
            ByteArrayProcesser.CompressionLevelsMapping = new ConcurrentDictionary<DataCompressionLevel, CompressionLevel>();
            ByteArrayProcesser.CompressionLevelsMapping.AddOrUpdate(DataCompressionLevel.Optimal, CompressionLevel.Optimal, (DataCompressionLevel level, CompressionLevel compressionLevel) => CompressionLevel.Optimal);
            ByteArrayProcesser.CompressionLevelsMapping.AddOrUpdate(DataCompressionLevel.Fastest, CompressionLevel.Fastest, (DataCompressionLevel level, CompressionLevel compressionLevel) => CompressionLevel.Fastest);
            ByteArrayProcesser.CompressionLevelsMapping.AddOrUpdate(DataCompressionLevel.NoCompression, CompressionLevel.NoCompression, (DataCompressionLevel level, CompressionLevel compressionLevel) => CompressionLevel.NoCompression);
        }

        public static byte[] Compress(byte[] data, DataCompressionLevel level = DataCompressionLevel.Fastest)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, ByteArrayProcesser.CompressionLevelsMapping[level], true))
                {
                    gZipStream.Write(data, 0, data.Length);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static byte[] Decompress(byte[] byteData)
        {
            if (byteData == null)
            {
                throw new ArgumentNullException("byteData", "inputData must be non-null");
            }

            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream(byteData))
            {
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    using (BufferedStream bufferedStream = new BufferedStream(new GZipStream(memoryStream, CompressionMode.Decompress), 4096))
                    {
                        bufferedStream.CopyTo(memoryStream2);
                    }
                    result = memoryStream2.ToArray();
                }
            }
            return result;
        }
    }
}
