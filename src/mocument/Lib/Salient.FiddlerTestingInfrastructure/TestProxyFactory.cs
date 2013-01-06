using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Fiddler;

namespace Salient.FiddlerTestingInfrastructure
{
    public static class TestProxyFactory
    {
        private static readonly AutoResetEvent StartSignal;
        public static readonly int Port;

        static TestProxyFactory()
        {
            Port = GetAvailablePort(32000, 33000, IPAddress.Loopback, false);
            StartSignal = new AutoResetEvent(true);
        }


        public static TestProxy Create()
        {
            StartSignal.WaitOne();
            var p = new TestProxy();
            return p;
        }


        /// <summary>
        /// From http://cassinidev.codeplex.com/SourceControl/changeset/view/94877#2280469
        /// 
        /// Returns first available port on the specified IP address. 
        /// The port scan excludes ports that are open on ANY loopback adapter. 
        /// 
        /// If the address upon which a port is requested is an 'ANY' address all 
        /// ports that are open on ANY IP are excluded.
        /// </summary>
        /// <param name="rangeStart"></param>
        /// <param name="rangeEnd"></param>
        /// <param name="ip">The IP address upon which to search for available port.</param>
        /// <param name="includeIdlePorts">If true includes ports in TIME_WAIT state in results. 
        /// TIME_WAIT state is typically cool down period for recently released ports.</param>
        /// <returns></returns>
        public static int GetAvailablePort(int rangeStart, int rangeEnd, IPAddress ip, bool includeIdlePorts)
        {
            IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();

            // if the ip we want a port on is an 'any' or loopback port we need to exclude all ports that are active on any IP
            Func<IPAddress, bool> isIpAnyOrLoopBack = i => IPAddress.Any.Equals(i) ||
                                                           IPAddress.IPv6Any.Equals(i) ||
                                                           IPAddress.Loopback.Equals(i) ||
                                                           IPAddress.IPv6Loopback.
                                                               Equals(i);
            // get all active ports on specified IP. 
            var excludedPorts = new List<ushort>();

            // if a port is open on an 'any' or 'loopback' interface then include it in the excludedPorts
            excludedPorts.AddRange(from n in ipProps.GetActiveTcpConnections()
                                   where
                                       n.LocalEndPoint.Port >= rangeStart &&
                                       n.LocalEndPoint.Port <= rangeEnd && (
                                                                               isIpAnyOrLoopBack(ip) ||
                                                                               n.LocalEndPoint.Address.Equals(ip) ||
                                                                               isIpAnyOrLoopBack(n.LocalEndPoint.Address)) &&
                                       (!includeIdlePorts || n.State != TcpState.TimeWait)
                                   select (ushort) n.LocalEndPoint.Port);

            excludedPorts.AddRange(from n in ipProps.GetActiveTcpListeners()
                                   where n.Port >= rangeStart && n.Port <= rangeEnd && (
                                                                                           isIpAnyOrLoopBack(ip) ||
                                                                                           n.Address.Equals(ip) ||
                                                                                           isIpAnyOrLoopBack(n.Address))
                                   select (ushort) n.Port);

            excludedPorts.AddRange(from n in ipProps.GetActiveUdpListeners()
                                   where n.Port >= rangeStart && n.Port <= rangeEnd && (
                                                                                           isIpAnyOrLoopBack(ip) ||
                                                                                           n.Address.Equals(ip) ||
                                                                                           isIpAnyOrLoopBack(n.Address))
                                   select (ushort) n.Port);

            excludedPorts.Sort();

            for (int port = rangeStart; port <= rangeEnd; port++)
            {
                if (!excludedPorts.Contains((ushort) port))
                {
                    return port;
                }
            }

            return 0;
        }

        #region Nested type: TestProxy

        public class TestProxy
        {
            public DateTime StartTime { get; internal set; }
            public DateTime StopTime { get; internal set; }

            public void Start()
            {
                StartSignal.Reset();
                StartTime = DateTime.Now;
                CONFIG.IgnoreServerCertErrors = true;
                FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.ForgetStreamedData", false);
                FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);
                Connect();


                FiddlerApplication.Startup(Port, false, true, true);
            }

            public void Stop()
            {
                Disconnect();
                FiddlerApplication.Shutdown();
                Thread.Sleep(500);

                StopTime = DateTime.Now;
                StartSignal.Set();
            }

            internal void Connect()
            {
                FiddlerApplication.AfterSessionComplete += OnAfterSessionComplete;
                FiddlerApplication.BeforeRequest += OnBeforeRequest;
                FiddlerApplication.BeforeResponse += OnBeforeResponse;
                FiddlerApplication.BeforeReturningError += OnBeforeReturningError;
                FiddlerApplication.FiddlerAttach += OnFiddlerAttach;
                FiddlerApplication.FiddlerDetach += OnFiddlerDetach;
                FiddlerApplication.OnClearCache += OnOnClearCache;
                FiddlerApplication.OnNotification += OnOnNotification;
                FiddlerApplication.OnReadRequestBuffer += OnOnReadRequestBuffer;
                FiddlerApplication.OnReadResponseBuffer += OnOnReadResponseBuffer;
                FiddlerApplication.OnValidateServerCertificate += OnOnValidateServerCertificate;
                FiddlerApplication.RequestHeadersAvailable += OnRequestHeadersAvailable;
                FiddlerApplication.ResponseHeadersAvailable += OnResponseHeadersAvailable;
            }

            internal void Disconnect()
            {
                FiddlerApplication.AfterSessionComplete -= OnAfterSessionComplete;
                FiddlerApplication.BeforeRequest -= OnBeforeRequest;
                FiddlerApplication.BeforeResponse -= OnBeforeResponse;
                FiddlerApplication.BeforeReturningError -= OnBeforeReturningError;
                FiddlerApplication.FiddlerAttach -= OnFiddlerAttach;
                FiddlerApplication.FiddlerDetach -= OnFiddlerDetach;
                FiddlerApplication.OnClearCache -= OnOnClearCache;
                FiddlerApplication.OnNotification -= OnOnNotification;
                FiddlerApplication.OnReadRequestBuffer -= OnOnReadRequestBuffer;
                FiddlerApplication.OnReadResponseBuffer -= OnOnReadResponseBuffer;
                FiddlerApplication.OnValidateServerCertificate -= OnOnValidateServerCertificate;
                FiddlerApplication.RequestHeadersAvailable -= OnRequestHeadersAvailable;
                FiddlerApplication.ResponseHeadersAvailable -= OnResponseHeadersAvailable;
            }

            #region Events

            public event SessionStateHandler AfterSessionComplete;

            internal void OnAfterSessionComplete(Session osession)
            {
                SessionStateHandler handler = AfterSessionComplete;
                if (handler != null) handler(osession);
            }

            public event SessionStateHandler BeforeRequest;

            internal void OnBeforeRequest(Session osession)
            {
                SessionStateHandler handler = BeforeRequest;
                if (handler != null) handler(osession);
            }

            public event SessionStateHandler BeforeResponse;

            internal void OnBeforeResponse(Session osession)
            {
                SessionStateHandler handler = BeforeResponse;
                if (handler != null) handler(osession);
            }

            public event SessionStateHandler BeforeReturningError;

            internal void OnBeforeReturningError(Session osession)
            {
                SessionStateHandler handler = BeforeReturningError;
                if (handler != null) handler(osession);
            }

            public event SimpleEventHandler FiddlerAttach;

            internal void OnFiddlerAttach()
            {
                SimpleEventHandler handler = FiddlerAttach;
                if (handler != null) handler();
            }

            public event SimpleEventHandler FiddlerDetach;

            internal void OnFiddlerDetach()
            {
                SimpleEventHandler handler = FiddlerDetach;
                if (handler != null) handler();
            }

            public event EventHandler<CacheClearEventArgs> OnClearCache;

            internal void OnOnClearCache(object sender, CacheClearEventArgs e)
            {
                EventHandler<CacheClearEventArgs> handler = OnClearCache;
                if (handler != null) handler(this, e);
            }

            public event EventHandler<NotificationEventArgs> OnNotification;

            internal void OnOnNotification(object sender, NotificationEventArgs e)
            {
                EventHandler<NotificationEventArgs> handler = OnNotification;
                if (handler != null) handler(this, e);
            }

            public event EventHandler<RawReadEventArgs> OnReadRequestBuffer;

            internal void OnOnReadRequestBuffer(object sender, RawReadEventArgs e)
            {
                EventHandler<RawReadEventArgs> handler = OnReadRequestBuffer;
                if (handler != null) handler(this, e);
            }

            public event EventHandler<RawReadEventArgs> OnReadResponseBuffer;

            internal void OnOnReadResponseBuffer(object sender, RawReadEventArgs e)
            {
                EventHandler<RawReadEventArgs> handler = OnReadResponseBuffer;
                if (handler != null) handler(this, e);
            }

            public event EventHandler<ValidateServerCertificateEventArgs> OnValidateServerCertificate;

            internal void OnOnValidateServerCertificate(object sender, ValidateServerCertificateEventArgs e)
            {
                EventHandler<ValidateServerCertificateEventArgs> handler = OnValidateServerCertificate;
                if (handler != null) handler(this, e);
            }

            public event SessionStateHandler RequestHeadersAvailable;

            internal void OnRequestHeadersAvailable(Session osession)
            {
                SessionStateHandler handler = RequestHeadersAvailable;
                if (handler != null) handler(osession);
            }

            public event SessionStateHandler ResponseHeadersAvailable;

            internal void OnResponseHeadersAvailable(Session osession)
            {
                SessionStateHandler handler = ResponseHeadersAvailable;
                if (handler != null) handler(osession);
            }

            #endregion
        }

        #endregion
    }
}