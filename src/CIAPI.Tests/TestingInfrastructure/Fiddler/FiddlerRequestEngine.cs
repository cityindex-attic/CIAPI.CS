using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiddlerSessionParser;

namespace CIAPI.Tests.TestingInfrastructure.Fiddler
{

    public class FiddlerRequestEngine
    {
        private List<SessionInfo> _sessions;
        public FiddlerRequestEngine(List<SessionInfo> sessions)
        {
            _sessions = sessions;

        }

        public SessionInfo FindSession(string requestMethod, string requestUrl)
        {
            return _sessions.FirstOrDefault(s => s.Request.Method == requestMethod && s.Request.Url == requestUrl);
        }
    }
}
