using System;
using System.Collections;
using System.Threading;

using Lightstreamer.Interfaces.Data;
using Lightstreamer.Adapters.StockListDemo.Feed;

namespace Lightstreamer.Adapters.StockListDemo.Data {

	/// <summary>
	/// This Data Adapter accepts a limited set of item names (the names starting
	/// with "item") and listens to a (simulated) stock quotes feed, waiting for
	/// update events. The events pertaining to the currently subscribed items
	/// are then forwarded to Lightstreamer.
	/// This example demonstrates how a Data Adapter could interoperate with
	/// a broadcast feed, which sends data for all items. Many other types of feeds
	/// may exist, with very different behaviours.
	/// </summary>
	public class StockListDemoAdapter : IDataProvider, IExternalFeedListener {

		private IDictionary _subscribedItems;
		private ExternalFeedSimulator _myFeed;

		private IItemEventListener _listener;

		public StockListDemoAdapter() {
			_subscribedItems= new Hashtable();
			
			_myFeed= new ExternalFeedSimulator();
		}
		
		// ////////////////////////////////////////////////////////////////////////
		// IDataProvider methos

		public void Init(IDictionary parameters, string configFile) {
			_myFeed.SetFeedListener(this);
			_myFeed.Start();
		}

		public void SetListener(IItemEventListener eventListener) {
			_listener= eventListener;
		}

		public void Subscribe(string itemName) {
			if (!itemName.StartsWith("item")) 
				throw new SubscriptionException("Unexpected item: " + itemName);

			lock (_subscribedItems) {
				if (_subscribedItems.Contains(itemName)) return;

				_subscribedItems[itemName]= false;
			}
			_myFeed.SendCurrentValues(itemName);
		}
		
		public void Unsubscribe(string itemName) {
			if (!itemName.StartsWith("item")) 
				throw new SubscriptionException("Unexpected item: " + itemName);

			lock (_subscribedItems) {
				_subscribedItems.Remove(itemName);
			}
		}

		public bool IsSnapshotAvailable(string itemName) {
			if (!itemName.StartsWith("item")) 
				throw new SubscriptionException("Unexpected item: " + itemName);

			return true;
		}
	
		// ////////////////////////////////////////////////////////////////////////
		// IExternalFeedListener methods

		public void OnEvent(string itemName, 
			IDictionary currentValues,
			bool isSnapshot) {
			lock (_subscribedItems) {
				if (!_subscribedItems.Contains(itemName)) return;

				bool started = (bool) _subscribedItems[itemName];
				if (!started) {
					if (!isSnapshot) 
						return;
					
					_subscribedItems[itemName]= true;
				} 
				else {
					if (isSnapshot) {
						isSnapshot = false;
					}
				}

                // We have to ensure that Update cannot be called after
                // Unsubscribe, so we need to hold the _subscribedItems lock;
                // however, Update is nonblocking; moreover, it only takes locks
                // to first order mutexes; so, it can safely be called here

                // Note that, in case a rapid Subscribe-Unsubscribe-Subscribe
                // sequence has just been issued for this item,
                // we may still be receiving and forwarding the snapshot
                // related with the first Subscribe call;
                // this case still leads to a perfectly consistent update flow,
                // in this scenario, so no checks are inserted to detect the case

                _listener.Update(itemName, currentValues, isSnapshot);
			}
		}
	}
	
}