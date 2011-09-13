using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lightstreamer.DotNet.Client;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone7Demo;

namespace WP7StockListDemo1
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

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
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

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            Debug.WriteLine("Application_Launching Called");
            SpawnLightstreamerClientStart();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Debug.WriteLine("Application_Activated Called");
            SpawnLightstreamerClientStart();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Debug.WriteLine("Application_Deactivated Called");
            SpawnLightstreamerClientStop();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Debug.WriteLine("Application_Closing Called");
            SpawnLightstreamerClientStop();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
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
        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}