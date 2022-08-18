using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    public class Program
    {
        static void Main(string[] args)
        {
            
        }

        public static void ReadData()
        {
            var cache = RedisConnection.Connection.GetDatabase();
            var devicesCount = 10000;
            for (int i = 0; i < devicesCount; i++)
            {
                var value = cache.StringGet($"Device_Status:{i}");
                Console.WriteLine($"Valor={value}");
            }
        }

        public static void SaveBigData()
        {
            var devicesCount = 10000;
            var rnd = new Random();
            var cache = RedisConnection.Connection.GetDatabase();

            for (int i = 1; i < devicesCount; i++)
            {
                var value = rnd.Next(0, 10000);
                cache.StringSet($"Device_Status:{i}", value);
            }
        }
    }


    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
