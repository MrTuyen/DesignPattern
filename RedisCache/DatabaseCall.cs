using Polly;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RedisCache
{
    public static class DatabaseCall
    {
        public static async Task<T> DatabaseCallRedis<T>(this IDatabase cache, string key, Func<string, Task<T>> databaseCall, string methodParameter, TimeSpan timeSpan, bool invalidate, bool useCache)
        {
            T returnValue;
            var cachedItem = default(T);
            try
            {
                cachedItem = await cache.GetAsync<T>(key);
            }
            catch (RedisConnectionException ex)
            {
                //create new connection
                HandleRedisConnectionError(ex);
            }
            if (null != cachedItem && !(cachedItem.Equals(default(T))) && useCache && !invalidate)// != cachedItems && useCache && !invalidate)
            {
                returnValue = cachedItem;
            }
            else
            {
                if (invalidate)
                    await cache.KeyDeleteAsync(key);
                returnValue = await databaseCall(methodParameter);
                if (useCache)
                    await cache.SetAsync(key, returnValue, timeSpan);
            }
            return returnValue;
        }

        private static bool attemptingToConnect = false;

        private static void HandleRedisConnectionError(RedisConnectionException ex)
        {
            if (attemptingToConnect) return;
            try
            {
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (exception, timeSpan) =>
                        {
                            Debug.WriteLine("Redis retry attempt" + exception.Message);
                        }
                    )
                    .Execute(() =>
                    {
                        attemptingToConnect = true;
                        RedisConnection.Reconnect();
                    });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                attemptingToConnect = false;
            }
        }
    }
}
