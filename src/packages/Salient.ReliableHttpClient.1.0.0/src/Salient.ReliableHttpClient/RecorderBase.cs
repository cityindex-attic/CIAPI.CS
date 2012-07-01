using System;

namespace Salient.ReliableHttpClient
{
    public abstract class RecorderBase : IDisposable
    {
        private bool _disposed;
        protected RecorderBase(ClientBase client)
        {
            Paused = true;
            Client = client;
            Client.RequestCompleted += OnRequestCompleted;
        }

        private void OnRequestCompleted(object sender, RequestCompletedEventArgs e)
        {
            AddRequest(e.Info);
        }
        protected ClientBase Client { get; private set; }

        protected bool IsDisposed()
        {
            return _disposed;
        }
        public void Dispose()
        {
            _disposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client.RequestCompleted -= OnRequestCompleted;
            }
            
        }

        public bool Paused { get; protected set; }

        public abstract void Start();
        public abstract void Stop();
        protected abstract void AddRequest(RequestInfoBase info);

    }
}