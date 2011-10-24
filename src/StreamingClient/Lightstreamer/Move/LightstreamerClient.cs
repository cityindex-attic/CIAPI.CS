using System;
using System.Collections.Generic;

namespace StreamingClient.Lightstreamer
{

    public class LightstreamerClient : IDisposable
    {
        private readonly string _sessionId;
        private readonly string _userName;
        private readonly string _streamingUri;
        private readonly Dictionary<string, FaultTolerantLsClientAdapter> _adapters ;
        public LightstreamerClient(Uri streamingUri, string userName, string sessionId)
        {
            _adapters = new Dictionary<string, FaultTolerantLsClientAdapter>();
            _streamingUri = streamingUri.ToString();
            _sessionId = sessionId;
            _userName = userName;
        }

 
        public event EventHandler<ConnectionStatusEventArgs> StatusUpdate;

        private void OnStatusUpdate(object sender, ConnectionStatusEventArgs e)
        {
            EventHandler<ConnectionStatusEventArgs> handler = StatusUpdate;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Allows consumer to stop and remove a listener from this client.
        /// </summary>
        public void TearDownListener(IStreamingListener  listener) 
        {
            lock (_adapters)
            {
                if (_adapters.ContainsKey(listener.DataAdapter))
                {
                    var adapter = _adapters[listener.DataAdapter];
                    adapter.TearDownListener(listener);
                }
            }
            
        }
        public IStreamingListener<TDto> BuildListener<TDto>(string dataAdapter, string topic)
                where TDto : class, new()
        {
            lock (_adapters)
            {
                if (!_adapters.ContainsKey(dataAdapter))
                {
                    var adp = new FaultTolerantLsClientAdapter(_streamingUri, _userName, _sessionId, dataAdapter);
                    adp.StatusUpdate += OnStatusUpdate;
                    _adapters.Add(dataAdapter, adp);
                    adp.Start();
                }
                var adapter = _adapters[dataAdapter];
                IStreamingListener<TDto> listener = adapter.BuildListener<TDto>(topic);
                return listener;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (KeyValuePair<string, FaultTolerantLsClientAdapter> kvp in _adapters)
                {
                    kvp.Value.Dispose();
                }
            }
        }
 
    }
}