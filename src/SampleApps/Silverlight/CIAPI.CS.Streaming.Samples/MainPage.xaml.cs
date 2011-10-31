using System;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using Common.Logging;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;


namespace CIAPI.CS.Streaming.Samples
{
    public partial class MainPage : UserControl
    {
        private ILog _logger = LogManager.GetLogger(typeof (MainPage));

        private Client _rcpClient;
        private IStreamingClient _streamingClient;
        private IStreamingListener<NewsDTO> _newsListener;

        public MainPage()
        {
            _logger.Info("init");
            InitializeComponent();
        }

        private void ToggleSubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleSubscribeButtonlabel.Text == "Subscribe")
            {
                _rcpClient = new Rpc.Client(new Uri(RpcUriTextbox.Text));

                var userName = UserNameTextbox.Text;
                var streamingUri = new Uri(StreamingUriTextbox.Text);
                var topic = TopicTextbox.Text;
                Log("Creating session...");
                
                _rcpClient.BeginLogIn(userName, PasswordTextbox.Text, loginResult =>
                {
                    try
                    {
                        _rcpClient.EndLogIn(loginResult);

                        _logger.DebugFormat("Session is: {0}", _rcpClient.Session);
                        Log("Creating streaming client...");
                        _streamingClient = StreamingClientFactory.CreateStreamingClient(streamingUri, userName, _rcpClient.Session);
                        _streamingClient.StatusChanged += (s, message) 
                                                          => Log(string.Format("Status update: {0}", message.Status));
                        

new Thread(()=>
               {

                   try
                   {
                       Log("Creating listener...");
                       _newsListener = _streamingClient.BuildNewsHeadlinesListener(topic);


                       Log("Listening to news stream...");
                       _newsListener.MessageReceived += (s, message) =>
                       {
                           try
                           {
                               NewsDTO receivedNewsHeadline = message.Data;
                               Log(
                                   string.Format(
                                       "Received: NewsDTO: StoryId {0}, Headline {1}, PublishDate = {2}",
                                       receivedNewsHeadline.StoryId,
                                       receivedNewsHeadline.Headline,
                                   //receivedNewsHeadline.PublishDate.ToString("u")));
                                   // dates are currently strings
                                       receivedNewsHeadline.PublishDate));
                           }
                           catch (Exception exception)
                           {
                               _logger.Error("Exception occured:", exception);
                           }
                       };
                   }
                   catch (Exception ex)
                   {

                       _logger.Error("Exception occured:", ex);
                   }

               }).Start();
                    }
                    catch (Exception exception)
                    {
                        _logger.Error("Exception occured:", exception);
                    }

                }, null);

                ToggleSubscribeButtonlabel.Text = "Stop";
            }
            else
            {
                try
                {
                    Log("Stopping listening to news stream...");

                    // D: abbreviating conditionals makes it hard to step 
                    // and is also a good way to get bugs. you may notice that I always use
                    // blocks. 

                    if (_newsListener != null)
                    {
                        _streamingClient.TearDownListener(_newsListener);
                        
                    }


                    

                    Log("Deleting session...");

                    if (_rcpClient != null )
                    {
                        // REMOVEME: i commented this out and still getting the ObjectDisposed exception
                        // so it is not in the RpcClient

                        _rcpClient.BeginLogOut(logoutResult =>
                                                   {
                                                       // FIXME: id/session invalid - getting back LoggedOut: false

                                                        /*do nothing*/
                                                       var breakTarget = 0;
                                                   }, null);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error("Exception occured:", exception);
                }

                ToggleSubscribeButtonlabel.Text = "Subscribe";
            }
            
        }

        private void Log(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => LogOnUIThread(message));
        }

        private void LogOnUIThread(string message)
        {
            ResultsTextbox.Text += message + "\n";
            ResultsTextboxScrollViewer.ScrollToVerticalOffset(ResultsTextbox.ActualHeight);
        }

        private void UserNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
