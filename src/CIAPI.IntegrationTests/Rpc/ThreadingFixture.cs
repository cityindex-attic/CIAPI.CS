using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using NUnit.Framework;
using Timer = System.Timers.Timer;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class ThreadingFixture : RpcFixtureBase
    {

        /// <summary>
        /// https://github.com/cityindex/CIAPI.CS/issues/147
        /// This test simulates the code from https://github.com/cityindex/CIAPI.CS.Excel/blob/master/src/CityIndex.Excel/DataLayer/Data.cs#L240
        /// </summary>
        [Test, Ignore("Takes ages to run, and only fails intermittently.  Grrr")]
        public void WorksCorrectlyWhenCalledFromBackgroundThread()
        {
            var client = BuildRpcClient();
            var accounts = client.AccountInformation.GetClientAndTradingAccount();
            var gate = new ManualResetEvent(false);
            var successfulResponses = 0;

            var totalRequestsToMake = 10;
            for (var i = 0; i < totalRequestsToMake; i++)
            {
                
                Background.Run(
                    () =>
                        {
                            var res = new List<ApiTradeHistoryDTO>();
                            foreach (var tradingAccount in accounts.TradingAccounts)
                            {
                                var resp = client.TradesAndOrders.ListTradeHistory(tradingAccount.TradingAccountId, 10);
                                res.AddRange(resp.TradeHistory);
                            }
                            return res.ToArray();
                        },
                    (success) =>
                        {
                            Interlocked.Increment(ref successfulResponses);
                            if (successfulResponses>=totalRequestsToMake) {
                                gate.Set();
                            }
                        }, 
                    (failureException) =>
                               {
                                   Console.WriteLine(failureException);
                                   gate.Set();
                               });
            }

            gate.WaitOne(TimeSpan.FromSeconds(totalRequestsToMake));

            Assert.AreEqual(totalRequestsToMake, successfulResponses,"One of the requests failed");

            /* Example error output:
             
            2012-04-16 20:04:21Z [INFO]  Salient.ReliableHttpClient.RequestController - Dispatched #12 : https://ciapi.cityindex.com/tradingapi/order/tradehistory?TradingAccountId=400237564&MaxResults=10 
            System.InvalidOperationException: Collection was modified; enumeration operation may not execute.
               at System.ThrowHelper.ThrowInvalidOperationException(ExceptionResource resource)
               at System.Collections.Generic.Dictionary`2.Enumerator.MoveNext()
               at Salient.ReliableHttpClient.RequestInfoBase.CreateRequest(RequestInfoBase info, IRequestFactory requestFactory) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\RequestInfoBase.cs:line 103
               at Salient.ReliableHttpClient.RequestInfo.BuildRequest(IRequestFactory requestFactory) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\RequestInfo.cs:line 33
               at Salient.ReliableHttpClient.RequestController.CreateRequest(Uri uri, RequestMethod method, String body, Dictionary`2 headers, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, Int32 timeout, String target, String uriTemplate, Int32 retryCount, Dictionary`2 parameters, ApiAsyncCallback callback, Object state) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\RequestController.cs:line 339
               at Salient.ReliableHttpClient.RequestController.BeginRequest(Uri uri, RequestMethod method, String body, Dictionary`2 headers, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, Int32 timeout, String target, String uriTemplate, Int32 retryCount, Dictionary`2 parameters, ApiAsyncCallback callback, Object state) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\RequestController.cs:line 369
               at Salient.ReliableHttpClient.ClientBase.BeginRequest(RequestMethod method, String target, String uriTemplate, Dictionary`2 parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, Int32 timeout, Int32 retryCount, ApiAsyncCallback callback, Object state) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\ClientBase.cs:line 263
               at CIAPI.Rpc.Client.BeginRequest(RequestMethod method, String target, String uriTemplate, Dictionary`2 parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, Int32 timeout, Int32 retryCount, ApiAsyncCallback callback, Object state) in C:\Dev\CIAPI.CS\src\CIAPI\Rpc\ApiClient.cs:line 18
               at Salient.ReliableHttpClient.ClientBase.Request(RequestMethod method, String target, String uriTemplate, Dictionary`2 parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, Int32 timeout, Int32 retryCount) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\ClientBase.cs:line 166
               at Salient.ReliableHttpClient.ClientBase.Request[T](RequestMethod method, String target, String uriTemplate, Dictionary`2 parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, Int32 timeout, Int32 retryCount) in C:\Dev\CIAPI.CS\src\ReliableHttpClient\Salient.ReliableHttpClient\ClientBase.cs:line 295
               at CIAPI.Rpc.Client._TradesAndOrders.ListTradeHistory(Int32 tradingAccountId, Int32 maxResults) in C:\Dev\CIAPI.CS\src\CIAPI\Generated\Routes.cs:line 1459
               at CIAPI.IntegrationTests.Rpc.ThreadingFixture.<>c__DisplayClass6.<WorksCorrectlyWhenCalledFromBackgroundThread>b__0() in C:\Dev\CIAPI.CS\src\CIAPI.IntegrationTests\Rpc\ThreadingFixture.cs:line 40
               at CIAPI.IntegrationTests.Rpc.Background.<>c__DisplayClass2`1.<Run>b__0(Object s, DoWorkEventArgs args) in C:\Dev\CIAPI.CS\src\CIAPI.IntegrationTests\Rpc\ThreadingFixture.cs:line 117
               at System.ComponentModel.BackgroundWorker.WorkerThreadStart(Object argument)
            2012-04-16 20:04:21Z [INFO]  Salient.ReliableHttpClient.RequestController - Dispatched #13 : https://ciapi.cityindex.com/tradingapi/order/tradehistory?TradingAccountId=400237564&MaxResults=10 

            */

            client.LogOut();
        }
    }

    
#region Background classes pulled from https://github.com/cityindex/CIAPI.CS.Excel/blob/master/src/CityIndex.Excel/Utils/Background.cs
    public static class Background
    {
        public static void Run<T>(Func<T> func, Action<T> onSuccess, Action<Exception> onException)
        {
            var worker = new BackgroundWorker();
            worker.DoWork +=
                (s, args) =>
                {
                    args.Result = func();
                };
            worker.RunWorkerCompleted +=
                (s, args) =>
                {
                    if (args.Error == null)
                    {
                        try
                        {
                            onSuccess.TryCall((T)(args.Result));
                        }
                        catch (Exception exc)
                        {
                            onException.TryCall(exc);
                        }
                    }
                    else
                        onException.TryCall(args.Error);

                    worker.Dispose();
                };
            worker.RunWorkerAsync();
        }

        public static void TryCall(this Action action)
        {
            if (action != null)
                action();
        }

        public static void TryCall<T>(this Action<T> action, T val)
        {
            if (action != null)
                action(val);
        }

        public static void Run(Action command, Action onSuccess, Action<Exception> onException)
        {
            Run<object>(
                () =>
                {
                    command();
                    return null;
                },
                res => onSuccess.TryCall(), onException);
        }

        public static Thread RunLong(Action func, Action onFinish, Action<Exception> onException)
        {
            var thread = new Thread(
                () =>
                {
                    try
                    {
                        func();
                        try
                        {
                            onFinish.TryCall();
                        }
                        catch (Exception exc)
                        {
                            onException.TryCall(exc);
                        }
                    }
                    catch (Exception exc)
                    {
                        onException.TryCall(exc);
                    }
                });
            thread.Start();
            return thread;
        }

        public static System.Timers.Timer RunTimer(TimeSpan period, bool needThreadSync, Action<TimerEventArgs> onTick)
        {
            var callSync = needThreadSync ? new ThreadSync() : null;

            var timer = new Timer { Interval = period.TotalMilliseconds };
            var tickArgs = new TimerEventArgs();

            timer.Elapsed +=
                (state, args) =>
                {
                    try
                    {
                        timer.Stop();

                        if (callSync == null)
                            onTick(tickArgs);
                        else
                        {
                            callSync.Invoke(() => onTick(tickArgs));
                            Thread.Sleep(1); // give the main thread some time to pump window messages
                        }

                        if (tickArgs.Cancel)
                            timer.Dispose();
                        else
                            timer.Start();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    catch (Exception exc)
                    {
                        Trace.WriteLine(exc);
                    }
                };

            timer.Start();
            return timer;
        }
    }

    public class TimerEventArgs
    {
        public bool Cancel { get; set; }
    }

    public class ThreadSync : IDisposable
    {
        public ThreadSync()
        {
            _form.Handle.ToString(); // ensure handle is created
        }

        public void Dispose()
        {
            _form.Dispose();
        }

        public void BeginInvoke(Action action)
        {
            _form.BeginInvoke(action);
        }

        public void Invoke(Action action)
        {
            _form.Invoke(action);
        }

        public bool InvokeRequired
        {
            get { return _form.InvokeRequired; }
        }

        public void VerifyIsMainThread()
        {
            if (InvokeRequired)
                throw new ApplicationException("Calling thread is not equal to the creator thread");
        }

        private readonly Form _form = new Form();
    }
#endregion 

}