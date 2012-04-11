using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIAPI.Rpc
{
    public class AppMetricsPlugin
    {
        private Client _client;
        public AppMetricsPlugin(Client client)
        {
            _client = client;
        }

        public void Start()
        {
            var items = _client.GetRecording();
            
        }
        public void Stop()
        {
            
        }
    }
}
