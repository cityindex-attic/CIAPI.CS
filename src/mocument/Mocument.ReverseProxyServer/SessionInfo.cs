using System;
using Fiddler;

namespace Mocument.ReverseProxyServer
{
    public enum StateAction
    {
        Reset,
        Position
    }
    public class SessionInfo
    {
        public StateAction StateAction { get; set; }
        
        public SessionInfo(Session oS)
        {
            if (oS.PathAndQuery.StartsWith("/state/", StringComparison.OrdinalIgnoreCase))
            {
                Type = SessionType.State;
                string[] pathSegments = oS.PathAndQuery.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                UserId = pathSegments[1];
                TapeId = pathSegments[2];
                StateAction = (StateAction) Enum.Parse(typeof(StateAction), pathSegments[3],true);
                return;
            }
            if (oS.PathAndQuery.StartsWith("/record/", StringComparison.OrdinalIgnoreCase))
            {
                Type = SessionType.Record;
            }
            if (oS.PathAndQuery.StartsWith("/play/", StringComparison.OrdinalIgnoreCase))
            {
                Type = SessionType.Playback;
            }
      
            if (oS.PathAndQuery.StartsWith("/export/", StringComparison.OrdinalIgnoreCase))
            {
                Type = SessionType.Export;
                string[] pathSegments = oS.PathAndQuery.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                UserId = pathSegments[1];
                TapeId = pathSegments[2];
                       
                return;
            }

            if (Type != SessionType.None)
            {
                string path = oS.PathAndQuery;

                switch (path)
                {
                    //#TODO: remove this
                    case "/blank.html":
                        //do nothing
                        break;
                    default:
                        string[] pathSegments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        UserId = pathSegments[1];
                        TapeId = pathSegments[2];
                        Host = pathSegments[3];
                        PathAndQuery = "/" + string.Join("/", pathSegments, 4, pathSegments.Length - 4);
                        break;
                }
            }
        }
        public SessionType Type;
        public string UserId;
        public string TapeId;
        public string Host;
        public string PathAndQuery;
    }
}