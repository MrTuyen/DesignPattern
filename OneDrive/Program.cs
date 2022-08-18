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

            //Console.WriteLine(fun().ToList());
            //Console.WriteLine(fun().ToList());

            //Console.ReadLine();

            string[] strings =
            {
                "A penny saved is a penny earned.",
                "The early bird catches the worm.",
                "The pen is mightier than the sword."
            };

            // Split the sentence into an array of words
            // and select those whose first letter is a vowel.
            var earlyBirdQuery =
                from sentence in strings
                let words = sentence.Split(' ')
                from word in words
                let w = word.ToLower()
                where w[0] == 'a' || w[0] == 'e'
                    || w[0] == 'i' || w[0] == 'o'
                    || w[0] == 'u'
                select word;

            // Execute the query.
            foreach (var v in earlyBirdQuery)
            {
                Console.WriteLine("\"{0}\" starts with a vowel", v);
            }

            // Keep the console window open in debug model.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
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
