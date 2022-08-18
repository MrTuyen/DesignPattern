using StackExchange.Redis;
using System;

namespace RedisCache
{
    public static class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static void Reconnect()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));
        }
    }
}
