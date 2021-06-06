using Logger.Entities;
using Logger.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logger.Providers
{
    public class Logger : ILogger
    {
        public static IOptions<LoggingConfiguration> _options;

        public static string RedisHostIp = string.Empty;

        public Logger(IOptions<LoggingConfiguration> config)
        {
            _options = config;
            var host = _options.Value.RedisConnectionString;

            if (!IsIpAddress(host))
            {
                if (string.IsNullOrEmpty(Logger.RedisHostIp))
                {
                    var ip = Dns.GetHostEntryAsync(host).GetAwaiter().GetResult();
                    if (ip != null && ip.AddressList != null & ip.AddressList.Length > 0)
                    {
                        ip.AddressList.ToList().ForEach(x =>
                        {
                            var ipaddr = x.ToString();
                            if (IsIpAddress(ipaddr))
                            {
                                Logger.RedisHostIp = ipaddr;
                                host = ipaddr;
                            }
                        });
                    }
                }
                else
                    host = Logger.RedisHostIp;
            }
        }

        #region public methods

        public void LogException(IExceptionEntry entry)
        {
            if (entry != null)
            {
                if (entry.Id == Guid.Empty)
                    entry.Id = Guid.NewGuid();

                var logData = JsonConvert.SerializeObject(entry);
                var gzipData = FormatData(logData);
                var redisConnectionString = String.Format("{0}:6379", _options.Value.RedisConnectionString);
                var manager = new RedisManagerPool(redisConnectionString);
                using (var client = manager.GetClient())
                {
                    client.AddItemToList(_options.Value.RedisListId, gzipData);
                }
            }
        }

        public Task WriteAsync(LogEntry entry)
        {
            WriteAsync(GetContextualLogEntry(entry));
            return Task.CompletedTask;
        }

        #endregion

        #region private methods

        private string FormatData(string data)
        {
            var compressed = ByteArrayProcesser.Compress(Encoding.UTF8.GetBytes(data));
            var retVal = Convert.ToBase64String(compressed);
            return retVal;
        }

        private static bool IsIpAddress(string host)
        {
            return Regex.IsMatch(host, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
        }

        private static ILogEntry GetTransactionEntry(LogEntry entry)
        {
            var apiEntry = (TransactionLogEntry)entry;
            var transactionEntry = new TransactionEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.Now,
                MethodName = apiEntry.MethodName,
                ServiceUrl = apiEntry.ServiceUrl,
                ApplicationName = apiEntry.ApplicationName,
                MachineName = apiEntry.MachineName,
                StatusType = apiEntry.Status,
                ProcessId = apiEntry.ProcessId,
                ProcessName = apiEntry.ProcessName,
                Request = apiEntry.Request,
                Response = apiEntry.Response,
                TimeTaken = apiEntry.TimeTaken,
                CallType = apiEntry.CallType
            };
            if (apiEntry.RequestHeaders != null)
                foreach (var header in apiEntry.RequestHeaders)
                {
                    var key = "headers_" + header.Key.ToLower();
                    if (!transactionEntry.AdditionalInfo.ContainsKey(key))
                        transactionEntry.AdditionalInfo.Add(key, header.Value);
                }
            if (!string.IsNullOrWhiteSpace(apiEntry.Message))
                transactionEntry.AddMessage(apiEntry.Message);
            if (apiEntry.AdditionalData != null)
            {
                foreach (var data in apiEntry.AdditionalData)
                {
                    if (!transactionEntry.AdditionalInfo.ContainsKey(data.Key))
                        transactionEntry.AdditionalInfo.Add(data.Key, data.Value);
                }
            }
            return transactionEntry;
        }

        private static ILogEntry GetContextualLogEntry(LogEntry entry)
        {
            ILogEntry logEntry = null;
            if (entry is TransactionLogEntry)
            {
                logEntry = GetTransactionEntry(entry);
            }
            return logEntry;
        }

        private void WriteAsync(ILogEntry entry)
        {
            try
            {
                if (entry == null)
                    throw new ArgumentNullException("entry");
                var clone = entry.CopyTo();
                Write(clone);
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        private void Write(ILogEntry entry)
        {
            try
            {
                var logger = new RedisSink();
                logger.Write(entry);
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLogFactory.Log(ex);
            }
        }

        #endregion
    }
}
