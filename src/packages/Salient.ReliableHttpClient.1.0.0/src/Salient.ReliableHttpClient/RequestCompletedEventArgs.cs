using System;

namespace Salient.ReliableHttpClient
{
    public class RequestCompletedEventArgs : EventArgs
    {
        public RequestCompletedEventArgs(RequestInfoBase info)
        {
            Info = info;
        }
        public RequestInfoBase Info { get; set; }
    }
}