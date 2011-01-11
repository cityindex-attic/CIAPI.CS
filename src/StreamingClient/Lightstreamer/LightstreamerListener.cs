using Lightstreamer.DotNet.Client;

namespace TradingApi.Client.Core.Lightstreamer
{
    public abstract class LightstreamerListener: IHandyTableListener
    {
        private readonly ILightstreamerConnection _connection;
        private SubscribedTableKey _subscribedTableKey;

        protected LightstreamerListener(ILightstreamerConnection connection)
        {
            _connection = connection;
        }

        public ILightstreamerConnection Connection
        {
            get { return _connection; }
        }

        
        public void Subscribe()
        {
            if (!_connection.IsOpen) _connection.Open();
            
            _subscribedTableKey = _connection.LSClient.SubscribeTable(GetTableInfo(), this, false);
        }

        protected internal abstract void OnUpdate(StreamingEventArgs<StreamingUpdate> lightstreamerEventArgs);

        protected abstract SimpleTableInfo GetTableInfo();

        public void Unsubscribe()
        {
            if (_subscribedTableKey!=null)
                _connection.LSClient.UnsubscribeTable(_subscribedTableKey);
        }

        #region Implementation of IHandyTableListener

        public virtual void OnUpdate(int itemPos, string itemName, UpdateInfo update)
        {
            OnUpdate(new StreamingEventArgs<StreamingUpdate> { Item = new StreamingUpdate { ItemName = itemName, ItemPosition = itemPos, Update = UpdateDetails.From(update) } });
        }

        public virtual void OnSnapshotEnd(int itemPos, string itemName)
        {
            _connection.OnStatusChanged(new StatusEventArgs { Status = string.Format("Snapshot ended: {0}:{1}", itemPos,itemName) });
        }

        public virtual void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            _connection.OnStatusChanged(new StatusEventArgs { Status = string.Format("{0} Raw Updates Lost: {1}:{2}", lostUpdates, itemPos, itemName) });
        }

        public virtual void OnUnsubscr(int itemPos, string itemName)
        {
            _connection.OnStatusChanged(new StatusEventArgs { Status = string.Format("Unsubscribed: {0}:{1}", itemPos, itemName) });
        }

        public virtual void OnUnsubscrAll()
        {
            _connection.OnStatusChanged(new StatusEventArgs { Status = "Unsubscribed from all" });
        }
        #endregion
    }
}