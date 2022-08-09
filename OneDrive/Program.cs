using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OneDrive
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var c = 1;
            //string a = $"{c}";
            //Console.WriteLine(a);
            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        Console.WriteLine("IP Address = " + ip.ToString());
            //    }
            //}

            Console.WriteLine(fun().ToList());
            Console.WriteLine(fun().ToList());

            Console.ReadLine();
        }

        public static IEnumerable<int> fun()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i;
            }
        }
    }
}
