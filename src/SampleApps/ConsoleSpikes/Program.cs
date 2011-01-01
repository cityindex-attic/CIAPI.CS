using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Core;

namespace ConsoleSpikes
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new ApiContext(new Uri("http://ec2-174-129-8-69.compute-1.amazonaws.com/RESTWebServices"));
            var response = ctx.ListNewsHeadlines("UK", 10);
            foreach (var headline in response.Headlines)
            {
                Console.WriteLine("{0} {1} {2}", headline.StoryId, headline.Headline, headline.PublishDate);
            }

            WaitForEnter("");
        }



        static void WaitForEnter(string message)
        {
            Console.WriteLine("\r\n{0}\r\n", message);
            Console.WriteLine("\r\nPress enter to continue\r\n");
            Console.ReadLine();
        }
    }
}
