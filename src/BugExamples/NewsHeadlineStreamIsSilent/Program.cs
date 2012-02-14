using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CIAPI.Rpc;
using CIAPI.Streaming;
using CityIndex.ReflectiveLoggingAdapter;

namespace NewsHeadlineStreamIsSilent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
              => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

                var client = new Client(new Uri("https://ciapi.cityindex.com/tradingapi"));
                var loginResponse = client.LogIn("DM715257", "password");
                if (loginResponse.PasswordChangeRequired)
                {
                    throw new Exception("must change password");
                }
                var streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri("https://push.cityindex.com"), client.UserName,
                                                                                   client.Session);
                var newsStream = streamingClient.BuildNewsHeadlinesListener("UK");
                newsStream.MessageReceived += new EventHandler<StreamingClient.MessageEventArgs<CIAPI.DTO.NewsDTO>>(newsStream_MessageReceived);

                Console.WriteLine("listening for 30 seconds");
                new AutoResetEvent(false).WaitOne(30000);
                
                Console.WriteLine("press enter to exit");
                Console.ReadLine();
                streamingClient.TearDownListener(newsStream);
                streamingClient.Dispose();
                client.LogOut();
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("press enter to exit");
                Console.ReadLine();
                throw;
            }


        }

        static void newsStream_MessageReceived(object sender, StreamingClient.MessageEventArgs<CIAPI.DTO.NewsDTO> e)
        {
            Console.WriteLine(e.Data.Headline);
        }
    }
}
