using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace RedisCache
{
    public static class RedisCall
    {
        public static T Get<T>(this IDatabase cache, string key)
        {
            //return Deserialize<T>(cache.StringGet(key));
            try
            {
                var value = cache.StringGet(key);
                if (!value.IsNull)
                    return JsonConvert.DeserializeObject<T>(value);
                else
                {
                    return default(T);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static void Set(this IDatabase cache, string key, object value, TimeSpan experation)
        {
            cache.StringSet(key, JsonConvert.SerializeObject(value), experation);
        }
    }
}
