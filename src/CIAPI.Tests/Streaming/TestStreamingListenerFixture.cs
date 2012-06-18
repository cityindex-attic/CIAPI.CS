using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Serialization;
using CIAPI.Streaming.Testing;
using CIAPI.StreamingClient;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.Tests.Streaming
{
    [TestFixture]
    public class TestStreamingListenerFixture
    {
        /// <summary>
        /// The utility of the 'mocked' test factories will be best leveraged by code that
        /// accepts an CIAPI.Rpc.Client as a constuctor argument.
        /// This will allow clean testing of business logic assemblies. 
        /// 
        /// GUI apps are a different story. Suggestions are welcome.
        /// </summary>
        [Test]
        public void HowToUseTestRpcAndStreamingClient()
        {
            // create and setup a TestRequestFactory for rpc calls
            var requestFactory = new TestRequestFactory
                {
                    PrepareResponse = rq =>
                                          {


                                              if (rq.RequestUri.AbsoluteUri == "http://foo.com/session")
                                              {
                                                  rq.SetResponseStream(
                                                      "{\"AllowedAccountOperator\":false,\"PasswordChangeRequired\":false,\"Session\":\"ecbeff35-e5b7-4c15-bb2e-52232360f575\"}");

                                              }

                                              if (rq.RequestUri.AbsoluteUri == "http://foo.com/session/deleteSession?userName=MyUserName&session=ecbeff35-e5b7-4c15-bb2e-52232360f575")
                                              {
                                                  rq.SetResponseStream("{\"LoggedOut\":true}");

                                              }

                                              if (Regex.IsMatch(rq.RequestUri.AbsoluteUri, "http://foo.com/news/dj/\\d+"))
                                              {
                                                  rq.SetResponseStream("{\"NewsDetail\":{\"Story\":\"<pre></pre><p>\\n  By Anita Greil </p><p>\\n  Of DOW JONES NEWSWIRES </p><pre></pre><p>\\n  ZURICH (Dow Jones)--Switzerland may introduce capital controls to fight a sharp rise in the Swiss franc in the event of a euro-zone collapse, radical measures that the Alpine nation hasn't employed since the seventies. </p><p>\\n  The risk of a potential Greek exit from the euro has increased in recent weeks as the country's political crisis has intensified, heightening worries about knock-on effects on other heavily indebted euro-zone nations. This is strengthening the Swiss franc, traditionally considered a refuge in times of economic and political turbulence. Switzerland-surrounded by, but not a member of, the European Union-is a haven of fiscal and political stability, but turbulence in the currency bloc is a major risk for its economy. The country depends heavily on exports for growth and a strong franc is hindering exports at a time when demand is slumping in the euro zone, Switzerland's biggest trade partner. </p><p>\\n  Because of the threat a euro collapse presents for the country, Berne earlier this year set up a task force to evaluate measures to be taken if this occurred. The task force is led by Swiss central bank president Thomas Jordan, Swiss Finance Minister Evelyne Widmer-Schlumpf and Anne Heritier Lachat, the head of financial-services industry regulator FinMa. </p><p>\\n  \\\"We must be prepared for the worst case, under which the currency union falls apart, even as I don't expect this to happen,\\\" Mr. Jordan said in an interview with Swiss weekly Sonntagszeitung. </p><p>\\n  Worries about the stability of the euro zone have heightened in recent weeks, putting further pressure on the Swiss franc. As result, the euro has traded close to the 1.20 Swiss franc floor that the Swiss National Bank introduced in September when the currency traded at record levels against the euro and the dollar. </p><p>\\n  Mr. Jordan said in the interview that the SNB will continue to defend this level with utmost determination, even under very difficult circumstances. </p><p>\\n  The floor has helped exporters, and contributed to the country avoiding recession. The SNB has so far managed to defend the franc without much visible effort, even though the euro briefly fell below 1.20 francs in April for a few seconds. The drop has been attributed to trades among banks that don't deal directly with the SNB and were selling euros for less than 1.20 francs for reasons that remain unclear. </p><p>\\n  Introducing a minimum rate for the euro against the franc didn't require any involvement from the government, but more extreme measures such as capital controls or negative interest rates would. </p><p>\\n  \\\"The task force focuses on measures that require cooperation between the government and the central bank to fight Swiss franc strength,\\\" Mr. Jordan said in Sonntagszeitung. One such measure would be capital controls, meaning tools, which directly influence the inflow of capital into Switzerland. </p><p>\\n  In the 1970s, Switzerland used extreme measures to curb excessive demand for its currency. The country prohibited foreign investments in Swiss securities and real-estate and introduced negative interest rates on foreign deposits. Both tools failed to stem the Swiss franc's rise, which only halted after the central bank introduced a temporary peg to the deutsche mark, Germany's currency at the time. </p><p>\\n  The SNB's floor on the franc has won the backing of the International Monetary Fund, not usually known for its endorsement of interference with market forces. The fund is more skeptical of other measures, though, warning in its latest report on the Swiss economy that capital-flow measures would be complex to design and costly given the country's role as an international financial center. </p><p>\\n  Mr. Jordan himself recently dismissed negative interest rates on foreign deposits as a tool for curbing haven flows. </p><pre></pre><p>\\n  -By Anita Greil, Dow Jones Newswires, +41 43 443 8044; anita.greil@dowjones.com </p><pre></pre><p>\\n  (END) Dow Jones Newswires</p><p>\\n  May 27, 2012 11:02 ET (15:02 GMT)</p>\",\"StoryId\":1416482,\"Headline\":\"UPDATE: Swiss Mull Capital Control In Case Of Euro Break-Up\",\"PublishDate\":\"\\/Date(1338130920000)\\/\"}}");

                                              }
                                              if (rq.RequestUri.AbsoluteUri == "http://foo.com/news/dj/UK?MaxResults=100")
                                              {
                                                  rq.SetResponseStream("{\"Headlines\":[{\"StoryId\":1416482,\"Headline\":\"UPDATE: Swiss Mull Capital Control In Case Of Euro Break-Up\",\"PublishDate\":\"\\/Date(1338130920000)\\/\"},{\"StoryId\":1416481,\"Headline\":\"Brazil Government To Examine Car Makers' Profitability -Report\",\"PublishDate\":\"\\/Date(1338126180000)\\/\"},{\"StoryId\":1416480,\"Headline\":\"BOE To Host Central Bankers Meeting On Euro Crisis - Report\",\"PublishDate\":\"\\/Date(1338125100000)\\/\"},{\"StoryId\":1416478,\"Headline\":\"SNB Jordan: May Introduce Capital Controls If Euro Zone Collapses\",\"PublishDate\":\"\\/Date(1338110820000)\\/\"},{\"StoryId\":1416467,\"Headline\":\"UK Services Sector Shows Signs Of Stabilizing - CBI\",\"PublishDate\":\"\\/Date(1338073260000)\\/\"},{\"StoryId\":1416466,\"Headline\":\"UK Services Show Signs Of Stabilizing - CBI\",\"PublishDate\":\"\\/Date(1338073260000)\\/\"},{\"StoryId\":1416464,\"Headline\":\"UK Govt Mulls Tighter Immigration Controls If Euro Zone Fractures\",\"PublishDate\":\"\\/Date(1338031140000)\\/\"},{\"StoryId\":1416447,\"Headline\":\"CORRECT: Mexico Peso Closes Weaker Vs Dollar As Europe Remains In Focus\",\"PublishDate\":\"\\/Date(1337984280000)\\/\"},{\"StoryId\":1416446,\"Headline\":\"Brazil Stocks Close Slightly Higher Amid Global Lull, Rate Cut Hopes\",\"PublishDate\":\"\\/Date(1337983380000)\\/\"},{\"StoryId\":1416438,\"Headline\":\"INTERVIEW: Uralkali CEO Doesn't Foresee Further Russian Taxes\",\"PublishDate\":\"\\/Date(1337980980000)\\/\"},{\"StoryId\":1416437,\"Headline\":\"Borje Ekholm Named Interim Chairman Of Nasdaq OMX Board\",\"PublishDate\":\"\\/Date(1337980680000)\\/\"},{\"StoryId\":1416434,\"Headline\":\"Net Bets For Dollar, Vs Euro Largest Since 2007\",\"PublishDate\":\"\\/Date(1337979300000)\\/\"},{\"StoryId\":1416433,\"Headline\":\"Mexican Stocks Close Lower As Euro-Zone Worries Persist\",\"PublishDate\":\"\\/Date(1337979000000)\\/\"},{\"StoryId\":1416430,\"Headline\":\"Mexico Peso Closes Weaker Vs Dollar As Europe Remains In Focus\",\"PublishDate\":\"\\/Date(1337978640000)\\/\"},{\"StoryId\":1416429,\"Headline\":\"UPDATE: Syngenta Settles Atrazine Class-Action Lawsuit For $105M\",\"PublishDate\":\"\\/Date(1337978460000)\\/\"},{\"StoryId\":1416428,\"Headline\":\"INTERVIEW: Uralkali Watches Peers To Determine New Potash Mine Timing -CEO\",\"PublishDate\":\"\\/Date(1337978340000)\\/\"},{\"StoryId\":1416427,\"Headline\":\"Peru's Main Stock Indexes End Higher; Sol Unchanged\",\"PublishDate\":\"\\/Date(1337978220000)\\/\"},{\"StoryId\":1416425,\"Headline\":\"Chile Stocks Close Higher, Post A Slight Weekly Gain\",\"PublishDate\":\"\\/Date(1337978040000)\\/\"},{\"StoryId\":1416423,\"Headline\":\"Uralkali CEO: Can Deliver Strong Results On Higher Pricing\",\"PublishDate\":\"\\/Date(1337977920000)\\/\"},{\"StoryId\":1416417,\"Headline\":\"Speculators Pile Up Largest Net Bets For Dollar, Vs Euro Since 2007\",\"PublishDate\":\"\\/Date(1337977200000)\\/\"},{\"StoryId\":1416415,\"Headline\":\"G7 Political, Economic Calendar - Week Ahead -3-\",\"PublishDate\":\"\\/Date(1337976960000)\\/\"},{\"StoryId\":1416414,\"Headline\":\"G7 Political, Economic Calendar - Week Ahead -2-\",\"PublishDate\":\"\\/Date(1337976960000)\\/\"},{\"StoryId\":1416413,\"Headline\":\"G7 Political, Economic Calendar - Week Ahead\",\"PublishDate\":\"\\/Date(1337976960000)\\/\"},{\"StoryId\":1416409,\"Headline\":\"Payback Time For Tax-Shy Greeks -IMF Chief\",\"PublishDate\":\"\\/Date(1337976840000)\\/\"},{\"StoryId\":1416408,\"Headline\":\"Brazil Real Strengthens To BRL1.9972 On Central Bank Action\",\"PublishDate\":\"\\/Date(1337976780000)\\/\"},{\"StoryId\":1416407,\"Headline\":\"EU Van Rompuy/Barroso: Need Strong, Credible G20 Message On Growth\",\"PublishDate\":\"\\/Date(1337976720000)\\/\"},{\"StoryId\":1416398,\"Headline\":\"Brazil Real Strengthens To BRL1.9972 On Central Bank Action\",\"PublishDate\":\"\\/Date(1337975880000)\\/\"},{\"StoryId\":1416388,\"Headline\":\"US GRAIN AND SOY REVIEW: Wheat Rises On Dryness Concerns\",\"PublishDate\":\"\\/Date(1337974620000)\\/\"},{\"StoryId\":1416387,\"Headline\":\"OIL FUTURES: Crude Ekes Out A Gain Ahead Of Long Weekend\",\"PublishDate\":\"\\/Date(1337974620000)\\/\"},{\"StoryId\":1416373,\"Headline\":\"OIL FUTURES: Crude Ekes Out A Gain Ahead Of Long Weekend\",\"PublishDate\":\"\\/Date(1337973540000)\\/\"},{\"StoryId\":1416372,\"Headline\":\"Peru's Central Bank Intervenes, Sells $2 Million\",\"PublishDate\":\"\\/Date(1337973120000)\\/\"},{\"StoryId\":1416371,\"Headline\":\"Emerging-Market Currencies, Debt Flat, But Investors Are Nervous\",\"PublishDate\":\"\\/Date(1337973060000)\\/\"},{\"StoryId\":1416369,\"Headline\":\"Peru's Ctrl Bk Intervenes In Forex Mkt, Sells $2 Million\",\"PublishDate\":\"\\/Date(1337972580000)\\/\"},{\"StoryId\":1416363,\"Headline\":\"FOREX WEEK AHEAD: Dollar's Fate Linked To Employment Data\",\"PublishDate\":\"\\/Date(1337971440000)\\/\"},{\"StoryId\":1416362,\"Headline\":\"LatAm Political, Economic Calendar - Week Ahead\",\"PublishDate\":\"\\/Date(1337970900000)\\/\"},{\"StoryId\":1416361,\"Headline\":\"BFA-Bankia To Get EUR19B Rescue From Spain Govt\",\"PublishDate\":\"\\/Date(1337970900000)\\/\"},{\"StoryId\":1416358,\"Headline\":\"MARKET TALK: Banorte Sees Strong 2Q Exports On Weak Mexico Peso\",\"PublishDate\":\"\\/Date(1337970480000)\\/\"},{\"StoryId\":1416355,\"Headline\":\"Chile Peso Ends At Fresh Four-Month Low On Euro-Zone Uncertainty\",\"PublishDate\":\"\\/Date(1337969400000)\\/\"},{\"StoryId\":1416354,\"Headline\":\"S&P Removes Tyco From Negative Watch List On Lower Borrowing\",\"PublishDate\":\"\\/Date(1337968920000)\\/\"},{\"StoryId\":1416347,\"Headline\":\"UPDATE: Italy Says Eni Must Sell At Least 25.1% Of Snam To CDP\",\"PublishDate\":\"\\/Date(1337967720000)\\/\"},{\"StoryId\":1416339,\"Headline\":\"UPDATE: Citigroup Completes Sale Of 10.1% Stake In Akbank\",\"PublishDate\":\"\\/Date(1337964960000)\\/\"},{\"StoryId\":1416332,\"Headline\":\"Italy Says Eni Must Sell At Least 25.1% Of Snam To Lender CDP\",\"PublishDate\":\"\\/Date(1337964060000)\\/\"},{\"StoryId\":1416331,\"Headline\":\"MARKET COMMENT: Brussels Stocks Close Slightly Higher\",\"PublishDate\":\"\\/Date(1337964000000)\\/\"},{\"StoryId\":1416330,\"Headline\":\"Greek Banks To Post Losses; Focus On Liquidity, Deposit Flight\",\"PublishDate\":\"\\/Date(1337963940000)\\/\"},{\"StoryId\":1416328,\"Headline\":\"MARKET COMMENT: Amsterdam Stocks Close Slightly Higher\",\"PublishDate\":\"\\/Date(1337963640000)\\/\"},{\"StoryId\":1416327,\"Headline\":\"MARKET COMMENT: Paris Stocks Close Slightly Higher\",\"PublishDate\":\"\\/Date(1337963160000)\\/\"},{\"StoryId\":1416326,\"Headline\":\"MARKET COMMENT: Nordic Stocks Are Mixed By Mkt Close\",\"PublishDate\":\"\\/Date(1337962860000)\\/\"},{\"StoryId\":1416324,\"Headline\":\"MARKET COMMENT: Zurich Stocks Close Marginally Higher\",\"PublishDate\":\"\\/Date(1337962140000)\\/\"},{\"StoryId\":1416323,\"Headline\":\"MARKET COMMENT: Frankfurt Stocks Close Slightly Higher\",\"PublishDate\":\"\\/Date(1337961720000)\\/\"},{\"StoryId\":1416322,\"Headline\":\"UPDATE: US Consumer Sentiment At Highest Level Since 2007\",\"PublishDate\":\"\\/Date(1337961660000)\\/\"},{\"StoryId\":1416321,\"Headline\":\"Tullow Finds Even More Oil At First Kenya Well\",\"PublishDate\":\"\\/Date(1337961600000)\\/\"},{\"StoryId\":1416320,\"Headline\":\"DJ British Bankers Association Libor Rates\",\"PublishDate\":\"\\/Date(1337961600000)\\/\"},{\"StoryId\":1416319,\"Headline\":\"DJ British Bankers Association Libor Rates For Euros\",\"PublishDate\":\"\\/Date(1337961600000)\\/\"},{\"StoryId\":1416318,\"Headline\":\"DJ British Bankers Association Libor Rates For Dollars\",\"PublishDate\":\"\\/Date(1337961600000)\\/\"},{\"StoryId\":1416314,\"Headline\":\"UK Summary: FTSE Closes Flat, Supported By Positive US Data\",\"PublishDate\":\"\\/Date(1337961180000)\\/\"},{\"StoryId\":1416311,\"Headline\":\"UPDATE: US Consumer Sentiment At Highest Level Since 2007\",\"PublishDate\":\"\\/Date(1337960760000)\\/\"},{\"StoryId\":1416310,\"Headline\":\"MARKET COMMENT: London Stocks Close Flat\",\"PublishDate\":\"\\/Date(1337960580000)\\/\"},{\"StoryId\":1416308,\"Headline\":\"Bolivia Expects Rising Energy Investment To Lift Oil, Gas Output\",\"PublishDate\":\"\\/Date(1337960340000)\\/\"},{\"StoryId\":1416307,\"Headline\":\"UPDATE: Canada Fin Min Says European Leaders Must 'Overwhelm' Debt Crisis\",\"PublishDate\":\"\\/Date(1337960340000)\\/\"},{\"StoryId\":1416305,\"Headline\":\"Brazilian Real Strengthens Past BRL2 Per Dollar On Central Bank Action\",\"PublishDate\":\"\\/Date(1337960220000)\\/\"},{\"StoryId\":1416303,\"Headline\":\"Swiss Flows Eyed As Steady Nordic Currencies Draw Focus\",\"PublishDate\":\"\\/Date(1337960100000)\\/\"},{\"StoryId\":1416300,\"Headline\":\"CORRECT: BOE's Weale Says There's Still A Case For More QE\",\"PublishDate\":\"\\/Date(1337959920000)\\/\"},{\"StoryId\":1416298,\"Headline\":\"Manhattan Apartment Owner Places 34 Buildings Into Bankruptcy\",\"PublishDate\":\"\\/Date(1337959860000)\\/\"},{\"StoryId\":1416295,\"Headline\":\"Brazilian Real Strengthens Past BRL2 Per Dollar On Central Bank Action\",\"PublishDate\":\"\\/Date(1337959320000)\\/\"},{\"StoryId\":1416293,\"Headline\":\"Financial News: Flint Hails 'Well-Positioned' HSBC\",\"PublishDate\":\"\\/Date(1337958960000)\\/\"},{\"StoryId\":1416290,\"Headline\":\"BOE's Weale Says There's Still A Case For More QE\",\"PublishDate\":\"\\/Date(1337958840000)\\/\"},{\"StoryId\":1416289,\"Headline\":\"KEY UK DATA: Manufacturing PMI To Shrink For First Time In 2012\",\"PublishDate\":\"\\/Date(1337958660000)\\/\"},{\"StoryId\":1416288,\"Headline\":\"Sterling Index At 1500 GMT\",\"PublishDate\":\"\\/Date(1337958600000)\\/\"},{\"StoryId\":1416286,\"Headline\":\"3rd UPDATE: Greek Euro Exit By Numbers: What -2-\",\"PublishDate\":\"\\/Date(1337958540000)\\/\"},{\"StoryId\":1416285,\"Headline\":\"3rd UPDATE: Greek Euro Exit By Numbers: What Economists Expect\",\"PublishDate\":\"\\/Date(1337958540000)\\/\"},{\"StoryId\":1416283,\"Headline\":\"Late Spot Sterling Rates In London\",\"PublishDate\":\"\\/Date(1337958000000)\\/\"},{\"StoryId\":1416281,\"Headline\":\"Canada Fin Min Says European Leaders Must 'Overwhelm' Debt Crisis\",\"PublishDate\":\"\\/Date(1337958000000)\\/\"},{\"StoryId\":1416272,\"Headline\":\"UPDATE: PAI Partners Pulls Out Of Iglo Auction - Sources\",\"PublishDate\":\"\\/Date(1337957760000)\\/\"},{\"StoryId\":1416265,\"Headline\":\"Academic Study Shows FX Carry Trade Really Does Work\",\"PublishDate\":\"\\/Date(1337956920000)\\/\"},{\"StoryId\":1416264,\"Headline\":\"PAI Partners Pulls Out Of Iglo Auction - Sources\",\"PublishDate\":\"\\/Date(1337956860000)\\/\"},{\"StoryId\":1416261,\"Headline\":\"WSJ BLOG/MarketBeat: Consumer Sentiment Jumps; Stocks Shrug\",\"PublishDate\":\"\\/Date(1337956560000)\\/\"},{\"StoryId\":1416260,\"Headline\":\"MARKET COMMENT: London Stocks Pare Losses After US Data\",\"PublishDate\":\"\\/Date(1337956500000)\\/\"},{\"StoryId\":1416259,\"Headline\":\"US Sentiment Rises To Highest Level Since May 2007\",\"PublishDate\":\"\\/Date(1337956500000)\\/\"},{\"StoryId\":1416257,\"Headline\":\"Arbitrage Spreads on Pending Mergers & Acquisitions\",\"PublishDate\":\"\\/Date(1337956440000)\\/\"},{\"StoryId\":1416256,\"Headline\":\"Brazil Stocks Rebound On Rate Cut Bets, German, US Confidence\",\"PublishDate\":\"\\/Date(1337956440000)\\/\"},{\"StoryId\":1416247,\"Headline\":\"Euro-Zone Stresses Fuel Danish Krone Rise, Put Strain On Peg\",\"PublishDate\":\"\\/Date(1337955960000)\\/\"},{\"StoryId\":1416245,\"Headline\":\"KEY CEE DATA: Hungary Rate, Polish GDP To Set Pace In Region\",\"PublishDate\":\"\\/Date(1337955660000)\\/\"},{\"StoryId\":1416244,\"Headline\":\"Mexico Stocks Mostly Lower As Euro-Zone Crisis Simmers\",\"PublishDate\":\"\\/Date(1337955660000)\\/\"},{\"StoryId\":1416242,\"Headline\":\"End-May Reuters/Univ Mich Sentiment Up To 79.3 From Early-May 77.8\",\"PublishDate\":\"\\/Date(1337955540000)\\/\"},{\"StoryId\":1416241,\"Headline\":\"WSJ BLOG/MarketBeat: Consumer Sentiment Jumps; Stocks Shrug\",\"PublishDate\":\"\\/Date(1337955480000)\\/\"},{\"StoryId\":1416238,\"Headline\":\"UPDATE:Spanish Bond Rally Reverses On Catalonia Worries\",\"PublishDate\":\"\\/Date(1337954760000)\\/\"},{\"StoryId\":1416236,\"Headline\":\"End-May Reuters/Univ Mich Sentiment Up To 79.3 From Early-May 77.8\",\"PublishDate\":\"\\/Date(1337954640000)\\/\"},{\"StoryId\":1416225,\"Headline\":\"Reuters/U Michigan 5-Yr Inflation Forecast +2.7%\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416224,\"Headline\":\"Reuters/U Michigan 12-Mo Inflation Forecast +3.0%\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416223,\"Headline\":\"Reuters/Univ Michigan End-May Expectations 74.3\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416222,\"Headline\":\"UPDATE: Russian Ruble Falls To New Lows\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416221,\"Headline\":\"Reuters/Univ Michigan End-May Current Index 87.2\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416220,\"Headline\":\"Reuters/Univ Michigan End-May Sentiment 79.3\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416218,\"Headline\":\"Brazil's Central Bank Sells $698 Mln In Dollar Swap Contracts\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416217,\"Headline\":\"China Not Named Currency Manipulator\",\"PublishDate\":\"\\/Date(1337954100000)\\/\"},{\"StoryId\":1416216,\"Headline\":\"UK Summary: London Stocks Slip Deeper Into The Red\",\"PublishDate\":\"\\/Date(1337953980000)\\/\"},{\"StoryId\":1416214,\"Headline\":\"ECB WATCH: Weak Data, Rising Tensions Boost Hope For Rate Action\",\"PublishDate\":\"\\/Date(1337953680000)\\/\"},{\"StoryId\":1416206,\"Headline\":\"Brazil's Central Bank Sells 35% Of Dollar Contracts Offered\",\"PublishDate\":\"\\/Date(1337952780000)\\/\"},{\"StoryId\":1416205,\"Headline\":\"Brazil's Central Bank Sells $698 Mln In Dollar Swap Contracts\",\"PublishDate\":\"\\/Date(1337952720000)\\/\"},{\"StoryId\":1416203,\"Headline\":\"Scottish Independence Campaigners Start Task Of Convincing Voters\",\"PublishDate\":\"\\/Date(1337952480000)\\/\"}]}");

                                              }

                                          }
                };


            // setup a streaming factory to deliver the messages we want
            var streamingFactory = new TestStreamingClientFactory();

            streamingFactory.CreatePriceMessage = args =>
            {
                

                if (args.DataAdapter == "CITYINDEXSTREAMINGDEFAULTPRICES" && args.Topic == "PRICES.AC0")
                {
                    args.Data.AuditId = "sbRDBProdFX35536125";
                    args.Data.Bid = 1.55341m;
                    args.Data.Change = 0.00243m;
                    args.Data.Direction = 0;
                    args.Data.High = 1.55487m;
                    args.Data.Low = 1.55025m;
                    args.Data.MarketId = 400494234;
                    args.Data.Offer = 1.55369m;
                    args.Data.Price = 1.55355m;
                    args.Data.StatusSummary = 0;
                    args.Data.TickDate = DateTime.UtcNow;
                }
            };

            // now let us use the rpc client as usual

            var rpcClient = new Client(new Uri("http://foo.com"), new Uri("http://foo.com"), "FOOBAR", new Serializer(), requestFactory, streamingFactory);

            ApiLogOnResponseDTO loginResponse = rpcClient.LogIn("MyUserName", "MyPassword");
            Assert.AreEqual("ecbeff35-e5b7-4c15-bb2e-52232360f575", loginResponse.Session);


            ListNewsHeadlinesResponseDTO headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            Assert.IsTrue(headlines.Headlines.Length > 0, "did not get headlines");

            var detail = rpcClient.News.GetNewsDetail("dj", headlines.Headlines[0].StoryId.ToString());

            Assert.IsNotNullOrEmpty(detail.NewsDetail.Headline, "dumb assertion - if it is getting called it is going to pass");


            // use the rpc client to create a streaming client with which we will subscribe to a stream

            var streamingClient = rpcClient.CreateStreamingClient();

            var listener = streamingClient.BuildDefaultPricesListener(0);

            // the gate is not part of the usage, per se, but rather a common mechanism to block the test 
            // completion while we listen to the stream
            var gate = new AutoResetEvent(false);

            PriceDTO streamingData = null;

            listener.MessageReceived += (s, e) =>
                                            {
                                                streamingData = e.Data;

                                                // let the test complete
                                                gate.Set();
                                            };




            if (!gate.WaitOne(10000))
            {
                Assert.Fail("timed out waiting for streaming event");
            }

            Assert.IsNotNull(streamingData, "no streaming data recieved");

            Assert.AreEqual("sbRDBProdFX35536125", streamingData.AuditId, "did not get the expected event data");

            bool loggedOut = rpcClient.LogOut();
            Assert.IsTrue(loggedOut, "did not log out");

            // clean up
            rpcClient.Dispose();


        }
        [Test]
        public void HowToUseTestStreamingFactory()
        {
            // this is the factory you pass in the ctor of rpc.Client
            // keep an instance reference so you can feed the streams
            var streamingFactory = new TestStreamingClientFactory();

            // before the listener publishes the MessageReceived event
            // it will give you access to the data item and it's descriptors
            streamingFactory.CreateNewsMessage = (args) =>
                                                      {
                                                          // in this handler you can check the args.DataAdapter
                                                          // and args.Topic to inform your code
                                                          // in this case we just send foobar data

                                                          NewsDTO dto = args.Data;
                                                          dto.Headline = "we are listening to " + args.DataAdapter + "." + args.Topic;
                                                      };



            var client = streamingFactory.Create(new Uri("http://foo.com"), "me", "pwd", new CIAPI.Serialization.Serializer());
            var listener = client.BuildNewsHeadlinesListener("YOUR CATEGORY");

            var gate = new AutoResetEvent(false);
            MessageEventArgs<NewsDTO> received = null;
            listener.MessageReceived += (sender, args) =>
                                            {
                                                received = args;
                                                gate.Set();

                                            };



            gate.WaitOne();

            // tear down as usual
            client.TearDownListener(listener);


            Assert.AreEqual("we are listening to CITYINDEXSTREAMING.NEWS.HEADLINES.YOUR CATEGORY", received.Data.Headline);

        }
        [Test]
        public void HowTestStreamingListenerWorks()
        {

            var listener = new TestStreamingListener<ApiLookupDTO>("DATAADAPTER", "TOPIC");

            listener.CreateMessage += (e) =>
            {
                e.Data.Description = "FOO";
            };

            var gate = new AutoResetEvent(false);
            int counter = 0;
            Exception exception = null;
            listener.MessageReceived += (sender, e) =>
            {
                Console.WriteLine("got {0} ", e.Data.Description);
                if (++counter > 10)
                {
                    gate.Set();
                }
                try
                {
                    if (e.Data.Description != "FOO")
                    {
                        throw new Exception("expected description FOO");
                    }
                }
                catch (Exception ex)
                {

                    exception = ex;
                    gate.Set();
                }
            };

            listener.Start(0);

            gate.WaitOne();
            listener.Stop();

            listener.Dispose();

            if (exception != null)
            {
                Assert.Fail(exception.Message);
            }
        }


    }
}