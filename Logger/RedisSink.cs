using Logger.Entities;
using Logger.Providers;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace Logger
{
    public sealed class RedisSink : SinkBase
    {
        #region Static Members
        private static string _redisHost;

        private static int _redisPort = 6379;

        private static string _listId;

        static RedisSink()
        {
            ResolveConfigurations();
        }

        private static void ResolveConfigurations()
        {
            var connString = Logger.Providers.Logger._options.Value.RedisConnectionString;
            if (string.IsNullOrEmpty(connString))
            {
                throw new LogException("Redis connection string is not in correct format :" + connString);
            }
            var portedConn = connString.Split(':');
            var port = 6379;
            if (portedConn.Length > 1 && !int.TryParse(portedConn[1], out port))
            {
                throw new LogException("Redis connection string is not in correct format :" + connString);
            }
            _redisHost = portedConn[0];
            _redisPort = port;
            _listId = Logger.Providers.Logger._options.Value.RedisListId;
        }
        #endregion

        #region Members
        private static void PushData(string data)
        {
            if (_listId == null)
                throw new LogException(string.Format("The key {0} must be defined in the settings. This is the name of the queue where another component (e.g. redis-logstash) will pick up things for further processing.", Logger.Providers.Logger._options.Value.RedisListId));
            var retry = 0;
            for (var i = 0; i < 2; i++)
            {
                using (var client = RedisClientManager.GetClient(_redisHost, _redisPort))
                {
                    {
                        try
                        {
                            client.AddItemToList(_listId, data);
                        }
                        catch (Exception)
                        {
                            if (retry == 0) //two tries, as the DNS resolution to get ips might have expired.
                            {
                                retry++;
                                continue;
                            }
                            throw;
                        }
                        break;
                    }
                }
            }
        }

        private static string FormatData(string data)
        {
            var compressed = ByteArrayProcesser.Compress(Encoding.UTF8.GetBytes(data));
            var retVal = Convert.ToBase64String(compressed);
            return retVal;
        }
        #endregion

        protected override void WriteTransaction(ITransactionEntry transactionEntry)
        {
            if (transactionEntry != null)
            {
                if (transactionEntry.Id == Guid.Empty)
                    transactionEntry.Id = Guid.NewGuid();
                //var str = Newtonsoft.Json.JsonConvert.SerializeObject(transactionEntry);
                var logData = JsonConvert.SerializeObject(transactionEntry);
                var gzipData = FormatData(logData);
                PushData(gzipData);
            }
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            if (eventEntry != null)
            {
                if (eventEntry.Id == Guid.Empty)
                    eventEntry.Id = Guid.NewGuid();
                //var str = Newtonsoft.Json.JsonConvert.SerializeObject(eventEntry);
                //PushData(str);
                var logData = JsonConvert.SerializeObject(eventEntry);
                var gzipData = FormatData(logData);
                PushData(gzipData);
            }
        }

        protected override void WriteEvent(IEventEntry transactionEntry)
        {
            if (transactionEntry != null)
            {
                if (transactionEntry.Id == Guid.Empty)
                    transactionEntry.Id = Guid.NewGuid();
                //var str = Newtonsoft.Json.JsonConvert.SerializeObject(transactionEntry);
                //PushData(str);
                var logData = JsonConvert.SerializeObject(transactionEntry);
                var gzipData = FormatData(logData);
                PushData(gzipData);
            }
        }
    }

    public static class RedisClientManager
    {
        private static ConcurrentDictionary<string, RedisManagerPool> _redisManagerPoolStore =
            new ConcurrentDictionary<string, RedisManagerPool>(StringComparer.OrdinalIgnoreCase);
        static int _maxPoolSize = 5;
        static RedisClientManager()
        {
            int maxPoolSize;
            var mpz = "5"; //Logger._logConfig?.GetAppSettingByKey("RedisMaxPoolSize");
            _maxPoolSize = int.TryParse(mpz, out maxPoolSize) ? maxPoolSize : 5;
        }
        public static IRedisClient GetClient(string hostName, int port = 6379)
        {
            return _redisManagerPoolStore.GetOrAdd(string.Join(":", hostName, port.ToString(CultureInfo.InvariantCulture)),
                                                   f => new RedisManagerPool(new String[] { f }, new RedisPoolConfig() { MaxPoolSize = _maxPoolSize })).GetClient();
        }
    }
}
