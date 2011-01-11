using System;
using System.Threading;
using CIAPI;
using CIAPI.DTO;

namespace ConsoleSpikes
{
    class Example
    {
        public void FetchNews_sync()
        {
            var ctx = new ApiClient(new Uri("https://ciapipreprod.cityindextest9.co.uk/tradingapi"));
            ctx.LogIn("0x234", "password");
            
            ListNewsHeadlinesResponseDTO news = ctx.ListNewsHeadlines("UK", 10);
            
            //do something with the news

            ctx.LogOut();
        }

        public void FetchNews_async()
        {
            var ctx = new ApiClient(new Uri("https://ciapipreprod.cityindextest9.co.uk/tradingapi"));
            var gate = new AutoResetEvent(false);
            ctx.BeginLogIn("0x234", "password", a =>
            {
                ctx.EndLogIn(a);

                ctx.BeginListNewsHeadlines("UK", 10, newsResult =>
                {
                    
                    ListNewsHeadlinesResponseDTO news = ctx.EndListNewsHeadlines(newsResult);

                    //do something with the news

                    ctx.BeginLogOut(result=>
                        {
                            gate.Set();
                        
                        }, null);
                }, null);
            }, null);

            gate.WaitOne();
        }
    }
}
