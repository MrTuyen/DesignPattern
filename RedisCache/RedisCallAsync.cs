using Newtonsoft.Json;
using Polly;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RedisCache
{
    public static class RedisCallAsync
    {
        public static async Task SetAsync(this IDatabaseAsync cache, string key, object value, TimeSpan experation)
        {
            await Policy.Handle<Exception>()
                       .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                       (exception, timeSpan) =>
                       {
                           Debug.WriteLine("Redis retry attempt" + exception.Message);
                       }
                       )
                       .ExecuteAsync(() => cache.StringSetAsync(key, JsonConvert.SerializeObject(value), experation));
        }

        public static async Task<T> GetAsync<T>(this IDatabaseAsync cache, string key)
        {
            var result = await Policy.Handle<RedisConnectionException>()
                                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                    (exception, timeSpan) =>
                                    {
                                        Debug.WriteLine("Redis retry attempt" + exception.Message);
                                    }
                                    )
                                    .ExecuteAsync(async () =>
                                    {
                                        var value = await cache.StringGetAsync(key);
                                        if (!value.IsNull)
                                            return JsonConvert.DeserializeObject<T>(value);
                                        else
                                        {
                                            return default(T);
                                        }
                                    });

            return result;
        }
    }
}
