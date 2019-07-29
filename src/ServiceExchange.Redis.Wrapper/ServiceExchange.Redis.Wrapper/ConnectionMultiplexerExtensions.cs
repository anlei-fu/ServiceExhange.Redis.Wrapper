using StackExchange.Redis;
using System.IO;

namespace StackExchange.Redis.Wrapper
{
    public static  class ConnectionMultiplexerExtensions
    {
        public static ConnectionMultiplexer WithAutoReconnect(this ConnectionMultiplexer connector ,int reconnectInterval,ConfigurationOptions config,TextWriter logger=null)
        {
            return connector;
        }

        public static ConnectionMultiplexer WithAutoReconnect(this ConnectionMultiplexer connector, int reconnectInterval, string config, TextWriter logger = null)
        {
            return connector;
        }
    }
}
