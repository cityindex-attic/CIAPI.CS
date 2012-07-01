using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Lightstreamer.DotNet.Client;
using System.Threading;
using System.Diagnostics;

namespace WindowsPhone7Demo
{
    public partial class App : Application
    {

        private const string pushServerHost = "http://push.lightstreamer.com";
        public static string[] items = {"item1", "item2", "item3", "item4", "item5",
            "item6", "item7", "item8", "item9", "item10", "item11", "item12", "item13",
            "item14", "item15"};
        public static string[] fields = { "stock_name", "last_price", "time" };

        private static Object ConnLock = new Object();
        private static int phase = 0;
        private static int lastDelay = 1;

        private static ILightstreamerListener listener = null;

        public static LightstreamerClient LightStreamer = new LightstreamerClient(items, fields);

        public PhoneApplicationFrame RootFrame { get; private set; }

        public App()
        {

            UnhandledException += Application_UnhandledException;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Application.Current.Host.Settings.EnableFrameRateCounter = true;
                //Application.Current.Host.Settings.EnableRedrawRegions = true;
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            InitializeComponent();
            InitializePhoneApplication();

            Debug.WriteLine("Lightstreamer Demo Application Started");
        }

        internal static void SetListener(ILightstreamerListener _listener)
        {
            listener = _listener;
        }

        private static void Start(int ph)
        {

            while (listener == null)
            {
                //or we may use a different listener that will pass the received values to
                //the front-end once the front-end is ready
                Thread.Sleep(500);
            }

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                PauseAndRetry(ph, null);
                return;
            }

            try
            {
                if (!checkPhase(ph))
                {
                    return;
                }

                listener.OnStatusChange(ph, LightstreamerConnectionHandler.CONNECTING, "Connecting to " + pushServerHost);

                LightStreamer.Start(pushServerHost, phase, listener);
                Debug.WriteLine("Lightstreamer Client Started");
                lastDelay = 1;

                if (!checkPhase(ph))
                {
                    return;
                }

                LightStreamer.Subscribe(ph, listener);


            }

            catch (PushConnException pce)
            {
                PauseAndRetry(ph, pce);
            }
            catch (SubscrException se)
            {
                PauseAndRetry(ph, se);
            }
            
        }

        private static void PauseAndRetry(int ph, Exception ee)
        {
            Debug.WriteLine("Lightstreamer Client, unable to start: " + ee);

            lastDelay *= 2;
            // Probably a connection issue, ask myself to respawn
            for (int i = lastDelay; i > 0; i--)
            {

                if (!checkPhase(ph))
                {
                    return;
                }

                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    listener.OnStatusChange(ph, LightstreamerConnectionHandler.CONNECTING, "Network unavailble, next check in " + i + " seconds");
                }
                else
                {
                    listener.OnStatusChange(ph, LightstreamerConnectionHandler.CONNECTING, "Connection failed, retrying in " + i + " seconds");
                }

                Thread.Sleep(1000);
            }

            
            Debug.WriteLine("Trying to respawn Lightstreamer Client");
            listener.OnReconnectRequest(ph);
            return;
        }

        private static void Stop(int ph)
        {
            
            if (!checkPhase(ph))
            {
                return;
            }
           
            LightStreamer.Stop();
            if (listener != null)
            {
                listener.OnStatusChange(ph, LightstreamerConnectionHandler.DISCONNECTED, "Disconnected");
            }
            Debug.WriteLine("Lightstreamer Client Stopped");
            
        }

        public static Boolean checkPhase(int ph)
        {
            lock (ConnLock)
            {
                return ph == phase;
            }
        }

        public static void SpawnLightstreamerClientStart()
        {
            StartStop execute;
            lock (ConnLock)
            {
                phase++;
                execute = new StartStop(phase);
            }

            Debug.WriteLine("About to Start Lightstreamer Client");
            ThreadStart ts = new ThreadStart(execute.DoStart);
            Thread th = new Thread(ts);
            th.Name = "LightStreamerWPThread";
            th.Start();
            
        }

        public static void SpawnLightstreamerClientStop()
        {
            StartStop execute;
            lock (ConnLock)
            {
                phase++;
                execute = new StartStop(phase);
            }

            Debug.WriteLine("About to Stop Lightstreamer Client");
            ThreadStart ts = new ThreadStart(execute.DoStop);
            Thread th = new Thread(ts);
            th.Name = "LightStreamerWPThread";
            th.Start();
        
        }

        public static void UserClick(Boolean wantsConnection)
        {
            lastDelay = 1;
            // This event triggers LightStreamer Client start/stop.
            if (!wantsConnection)
            {
                // stop
                App.SpawnLightstreamerClientStop();
            }
            else
            {
                // start
                App.SpawnLightstreamerClientStart();
            }
        }


        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            Debug.WriteLine("Application_Launching Called");
            SpawnLightstreamerClientStart();
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Debug.WriteLine("Application_Activated Called");
            SpawnLightstreamerClientStart();
            
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Debug.WriteLine("Application_Deactivated Called");
            SpawnLightstreamerClientStop();
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Debug.WriteLine("Application_Closing Called");
            SpawnLightstreamerClientStop();
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("Application_UnhandledException Called");
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        class StartStop
        {
            public int Phase;

            public StartStop(int Ph)
            {
                this.Phase = Ph;
            }

            public void DoStart()
            {
                Start(phase);
            }

            public void DoStop()
            {
                Stop(phase);
            }
        }

        #region Windows Phone Application Initialization

        private bool phoneApplicationInitialized = false;

        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

    }
}