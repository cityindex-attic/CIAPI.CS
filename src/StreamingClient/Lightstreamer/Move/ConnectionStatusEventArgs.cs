using System;

namespace StreamingClient
{
    public class ConnectionStatusEventArgs:EventArgs
    {
        public ConnectionStatusEventArgs(string message,ConnectionStatus status)
        {
            Message = message;
            Status = status;
        }
        public string Message { get; set; }
        public ConnectionStatus Status { get; set; }
        public override string ToString()
        {
            return string.Format("{0}: {1}", Status, Message);
        }
    }
}