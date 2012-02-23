using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using log4net;

using Lightstreamer.Interfaces.Data;
using Lightstreamer.Adapters.PortfolioDemo.Feed;


namespace Lightstreamer.Adapters.PortfolioDemo.Data
{
    /// <summary>
    /// This Data Adapter accepts subscriptions to items representing stock
    /// portfolios and inquiries a (simulated) portfolio feed, getting the current
    /// portfolio contents and waiting for update events. The events are then
    /// forwarded to Lightstreamer according to the COMMAND mode protocol.
    /// 
    /// This example demonstrates how a Data Adapter could interoperate with
    /// an external feed; in this example, the feed provides a bean object
    /// for each single portfolio instance.
    /// </summary>
    public class PortfolioAdapter : IDataProvider
    {
        /// <summary>
        /// Private logger; we lean on some logging configuration for log4net
        /// in the Remote Server launcher program.
        /// A specific "Lightstreamer.Adapters.PortfolioDemo" log category
        /// can be defined.
        /// </summary>
        private static ILog _log = LogManager.GetLogger("Lightstreamer.Adapters.PortfolioDemo");

        /// <summary>
        /// The listener of updates provided by Lightstreamer library.
        /// </summary>
        private volatile IItemEventListener listener;

        /// <summary>
        /// Contains the set of item names for the currently subscribed items.
        /// A Hashset could be used for this purpose;
        /// a Hashtable is used instead, because it is already thread safe.
        /// </summary>
        private Hashtable subscriptions = new Hashtable();

        /// <summary>
        /// The feed simulator.
        /// </summary>
        private volatile PortfolioFeedSimulator feed = null;

        /// <summary>
        /// Used to serialize operations on the portfolio structure.
        /// </summary>
        private NotificationQueue executor = new NotificationQueue();

        public PortfolioAdapter() 
        {
            executor.Start();
        }

        public void SetFeed(PortfolioFeedSimulator simulator)
        {
            this.feed = simulator;
        }            

        public void Init(IDictionary map, String file)
        {
            if (feed == null)
            {
                // SetFeed has not been invoked after the object creation.
                // Note that the related feed is not obtained in Init,
                // based on configuration parameters,
                // but must be provided explicitly,
                // through the custom SetFeed method.
                // As a consequence, this Adapter cannot be managed by the
                // basic DotNetServer_N2.exe launcher provided by LS library,
                // but requires a custom launcher.
                throw new DataProviderException("Portfolio feed not configured; currently, portfolio adapters can only be launched by the provided custom launcher");
            }
        }

        public void SetListener(IItemEventListener eventListener)
        {
            // Save the update listener
            listener = eventListener;
        }

        public bool IsSnapshotAvailable(string portfolioId)
        {
            // We have always the snapshot available from our feed
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Subscribe(string portfolioId)
        {

            if (!PortfolioFeedSimulator.CheckPortfolio(portfolioId))
            {
                throw new SubscriptionException("No such portfolio: "
                        + portfolioId);
            }
            
            // Complete the subscription operation asynchronously
            executor.Add(delegate()
            {
                Debug.Assert(!subscriptions.ContainsKey(portfolioId));

                Portfolio portfolio = feed.GetPortfolio(portfolioId);
                if (portfolio == null)
                {
                    _log.Error("No such portfolio: " + portfolioId);
                    Debug.Assert(false);
                    return;
                }
                // Add the new item to the list of subscribed items;
                // the "true" value is a placeholder, as we use a Hashtable.
                subscriptions.Add(portfolioId, true);

                // Create a new listener for the portfolio
                MyPortfolioListener listener = new MyPortfolioListener(portfolioId, this);
                // Set the listener on the feed
                portfolio.SetListener(listener);

                _log.Info(portfolioId + " subscribed");
            });

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Unsubscribe(string portfolioId) {
            // Perform the unsubscription operation asynchronously
            executor.Add(delegate()
            {
                Debug.Assert(subscriptions.ContainsKey(portfolioId));

                Portfolio portfolio = feed.GetPortfolio(portfolioId);
                if (portfolio != null) {
                    // Remove the listener from the feed to not receive new
                    // updates
                    portfolio.RemoveListener();
                }
                // Remove the item from the list of subscribed items
                subscriptions.Remove(portfolioId);

                _log.Info(portfolioId + " unsubscribed");
            });
        }

        private bool IsSubscribed(string portfolioId) {
            // Just check if a given item is in the map of subscribed items
            return subscriptions.Contains(portfolioId);
        }

        internal void OnUpdate(string portfolioId, string key, int qty) {
            // An update was received from the feed
            // Check for late calls
            if (IsSubscribed(portfolioId))
            {
                // Create a new Hashtable instance that will represent the update
                IDictionary update = new Hashtable();

                // We have to set the key
                update.Add("key", key);
                // The UPDATE command
                update.Add("command", "UPDATE");
                // And the new quantity value
                update.Add("qty", Convert.ToString(qty));

                // Pass everything to Lightstreamer library
                listener.Update(portfolioId, update, false);
            }
        }

        internal void OnDelete(string portfolioId, string key) {
            // An update was received from the feed
            // Check for late calls
            if (IsSubscribed(portfolioId))
            {
                // Create a new Hashtable instance that will represent the update
                IDictionary update = new Hashtable();

                // We just need the key
                update.Add("key", key);
                // And the DELETE command
                update.Add("command", "DELETE");

                // Pass everything to Lightstreamer library
                listener.Update(portfolioId, update, false);
            }
        }

        internal void OnAdd(string portfolioId, string key, int qty, bool snapshot) {
            // An update for a new stock was received from the feed or the snapshot was read
            // Check for late calls
            if (IsSubscribed(portfolioId))
            {
                // Create a new Hashtable instance that will represent the update
                IDictionary update = new Hashtable();

                // We have to set the key
                update.Add("key", key);
                // The ADD command
                update.Add("command", "ADD");
                // And the initial quantity
                update.Add("qty", Convert.ToString(qty));


                // Pass everything to Lightstreamer library
                listener.Update(portfolioId, update, snapshot);
            }

        }

        internal void OnEndSnapshot(string portfolioId)
        {
            // The snapshot has just been read.
            // Pass the notice to Lightstreamer library
            listener.EndOfSnapshot(portfolioId);
        }
    }


    /// <summary>
    /// Inner class that listens to a single Portfolio.
    /// </summary>
    class MyPortfolioListener : PortfolioListener
    {
        private static ILog _log = LogManager.GetLogger("Lightstreamer.Adapters.PortfolioDemo");

        private string portfolioId;
        private PortfolioAdapter pda;

        public MyPortfolioListener(string portfolioId, PortfolioAdapter pda) {
            this.portfolioId = portfolioId;
            this.pda = pda;
        }

        public void Update(string stock, int qty, int oldQty) {
            // An update was received from the feed
            if (qty <= 0) {
                // If qty is 0 or less we have to delete the "row"
                pda.OnDelete(this.portfolioId, stock);
                _log.Debug(this.portfolioId + ": deleted " + stock);

            } else if (oldQty == 0) {
                // If oldQty value is 0 then this is a new stock
                // in the portfolio so that we have to add a "row"
                pda.OnAdd(this.portfolioId, stock, qty, false);
                _log.Debug(this.portfolioId + ": added " + stock);

            } else {
                // A simple update
                pda.OnUpdate(this.portfolioId, stock, qty);
                _log.Debug(this.portfolioId + ": updated " + stock);
            }
        }

        public void OnActualStatus(IDictionary currentStatus) {
            // Iterates through the Hash representing the actual status
            // to send the snapshot to Lightstreamer
            IEnumerator keys = currentStatus.Keys.GetEnumerator();
            while(keys.MoveNext()) {
                string key = (string) keys.Current;
                pda.OnAdd(this.portfolioId, key, (int) currentStatus[key], true);
            }

            // Notify the end of snapshot to Lightstreamer
            pda.OnEndSnapshot(this.portfolioId);

            _log.Info(this.portfolioId + ": snapshot sent");
        }
    }
}
