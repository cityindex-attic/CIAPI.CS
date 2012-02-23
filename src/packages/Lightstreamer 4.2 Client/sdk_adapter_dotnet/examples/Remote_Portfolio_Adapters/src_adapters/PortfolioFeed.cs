using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using log4net;

namespace Lightstreamer.Adapters.PortfolioDemo.Feed
{
    /// <summary>
    /// Used to receive data from the simulated portfolio feed in an
    /// asynchronous way.
    /// Upon listener submission, a single call to onActualStatus is issued
    /// in short time, then multiple calls to "update" can be issued.
    /// </summary>
    public interface PortfolioListener
    {
        /// <summary>
        /// Called at first to send the actual portfolio contents.
        /// The IDictionary associates stock ids with quantities.
        /// Only stocks with positive quantities are included.
        /// </summary>
        void OnActualStatus(IDictionary currentStatus);

        /// <summary>
        /// Called on each new update on the state of the portfolio.
        /// If oldQty is 0 means that the stock wasn't on the portfolio before;
        /// if qty is 0 means that the stock was completely sold from the portfolio.
        /// </summary>
        void Update(string stock, int qty, int oldQty);
    }

    /// <summary>
    /// Simulates an external data feed that provides the contents of multiple
    /// stock portfolios. Only 5 portfolios of names "portfolio1" to "portfolio5"
    /// are currently accepted.
    /// The managed portfolios are initialized with random content. Each portfolio
    /// is initialized only when needed, then it is kept permanently.
    /// The feed provides the clients with a single bean for each managed portfolio,
    /// which can be used either to listen to the contents
    /// or to notify buy/sell orders.
    /// </summary>
    public class PortfolioFeedSimulator
    {
        /// <summary>
        /// Private logger; we lean on some logging configuration for log4net
        /// in the Remote Server launcher program.
        /// A specific "Lightstreamer.Adapters.PortfolioDemo" log category
        /// can be defined.
        /// </summary>
        private static ILog _log = LogManager.GetLogger("Lightstreamer.Adapters.PortfolioDemo");

        /// <summary>
        /// Map of portfolios.
        /// </summary>
        private IDictionary portfolios = new Hashtable(); 

        public PortfolioFeedSimulator() 
        {
        }

        public Portfolio GetPortfolio(string portfolioId) 
        {
            //Check the portfolioId to see if it's a valid one
            if (!CheckPortfolio(portfolioId)) 
            {
                _log.Warn("Wrong portfolio ID: " + portfolioId);
                return null;
            }

            //Get the portfolio by id from the portfolios map
            Portfolio portfolio = (Portfolio)portfolios[portfolioId];
            if (portfolio != null) 
            {
                //If the portfolio is already available return it
                return portfolio;
            } 
            else 
            {
                //If the portfolio is not yet available we will create it
                //We have to synchronize to avoid conflict with other thread that
                //need to create the same portfolio
                lock (this) 
                {
                    //Check again if the portfolio is available in case another thread created it
                    //while we were waiting for the lock
                    portfolio = (Portfolio)portfolios[portfolioId];
                    if (portfolio == null) 
                    {
                        //If no such portfolio exists we create a new portfolio
                        portfolio = new Portfolio(portfolioId);

                        //We need to generate an actual status of the portfolio to avoid starting with
                        //an empty one. Some random quantity will do the trick.
                        AddRandomQuantities(portfolio);

                        //Add the new portfolio to the list of available portfolios
                        portfolios.Add(portfolioId, portfolio);

                        _log.Info(portfolioId + " created");
                    }
                    //Return the portfolio
                    return portfolio;
                }
            }
        }

        /// <summary>
        /// Creates a random initial status for the portfolio.
        /// </summary>
        private static void AddRandomQuantities(Portfolio portfolio) 
        {
            Random generator = new Random();

            bool[] used = new bool[30];
            for (int i = 0; i < 30; i++) 
            {
                used[i] = false;
            }

            //we start with 6-8 stocks
            int stocks = 6 + generator.Next(3);

            for (int i = 1; i <= stocks; i++) {

                int stockN;
                do 
                {
                    //We need a stock number between 0 and 29
                    stockN = generator.Next(30);
                } 
                while (used[stockN]); //We need a stockId that's not been already used for this portfolio

                //Sign that we've used this stock number
                used[stockN] = true;

                //The stock id will be itemN where N is a number between 1 and 30
                string item = "item" + (stockN + 1);

                //The initial quantity will be between 100 and 2500
                int qty = generator.Next(25) + 1;
                qty *= 100;

                //Use the buy method to initialize the status
                portfolio.Buy(item, qty);
            }
        }

        /// <summary>
        /// Performs a simple hard-coded portfolio id validation;
        /// we accept portfolioN where N is a number between 1 and 5.
        /// </summary>
        public static bool CheckPortfolio(string portfolio)
        {
            if (portfolio.IndexOf("portfolio") != 0) 
            {
                return false;
            }
            int stNum;
            try
            {
                stNum = Convert.ToInt32(portfolio.Substring(9));
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }

            if (stNum <= 0 || stNum > 5) 
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Performs a simple hard-coded stock id validation;
        /// We accept itemN where N is a number between 1 and 30.
        /// NOTE that also the Portfolio class is aware about
        /// the way the stock id are composed.
        /// </summary>
        public static bool CheckStock(string stock)
        {
            
            if (stock.IndexOf("item") != 0)
            {
                return false;
            }
            int stNum;
            try
            {
                stNum = Convert.ToUInt16(stock.Substring(4));
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }

            if (stNum <= 0 || stNum > 30)
            {
                return false;
            }

            return true;
        }

    
    }

    /// <summary>
    /// Manages the contents for a single portfolio.
    /// The contents can be changed through "buy" and "sell" methods
    /// and can be inquired through a listener; upon setting of a new listener,
    /// the current contents are notified, followed by the notifications
    /// of subsequent content changes.
    /// To make it simple, a single listener is allowed at each time.
    /// All methods are synchronized, but none can be blocking. The calls
    /// to the listener are enqueued and send from a local thread; they may
    /// occur just after "removeListener" has been issued.
    /// </summary>
    public class Portfolio
    {
        private static ILog _log = LogManager.GetLogger("Lightstreamer.Adapters.PortfolioDemo");

        /// <summary>
        /// Single listener for the contents.
        /// </summary>
        private PortfolioListener listener;

        private string id;

        /// <summary>
        /// The portfolio contents; associates stock ids with quantities;
        /// only stocks with positive quantities are included.
        /// </summary>
        private Hashtable quantities = new Hashtable();

        /// <summary>
        /// Used to enqueue the calls to the listener.
        /// </summary>
        private NotificationQueue executor = new NotificationQueue();

        public Portfolio(string id)
        {
            this.id = id;
            executor.Start();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Buy(string stock, int qty)
        {
            if (qty <= 0)
            {
                //We can't buy 0 or less...
                _log.Warn("Cannot buy " + qty + " " + stock + " for " + this.id + " use an integer greater than 0");
                return;
            }

            if (!PortfolioFeedSimulator.CheckStock(stock))
            {
                //this stock does not exist
                _log.Warn("Not valid stock to buy: " + stock);
                return;
            }

            _log.Debug("Buying " + qty + " " + stock + " for " + this.id);
            //Pass the quantity to add to the ChangeQty method
            this.ChangeQty(stock, qty);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Sell(string stock, int qty)
        {
            if (qty <= 0)
            {
                //We can't sell 0 or less...
                _log.Warn("Cannot sell " + qty + " " + stock + " for " + this.id + " use an integer greater than 0");
                return;
            }

            if (!PortfolioFeedSimulator.CheckStock(stock))
            {
                //this stock does not exist
                _log.Warn("Not valid stock to sell: " + stock);
                return;
            }

            _log.Debug("Selling " + qty + " " + stock + " for " + this.id);
            //Change the quantity sign and pass it to the changeQty method
            this.ChangeQty(stock, -qty);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ChangeQty(string stock, int qty)
        {
            //Get the old quantity for the stock
            Object oldQtyObject = quantities[stock];

            int newQty;
            int oldQty;
            if (oldQtyObject == null)
            {
                //If oldQtyObject is null it means that we don't have that stock on our portfolio
                if (qty <= 0)
                {
                    //We can't sell something we don't have, warn and return.
                    _log.Warn(this.id + "|No stock to sell: " + stock);
                    return;
                }
                //Set oldQty to 0 to let the listener know that we previously didn't have such stock
                oldQty = 0;
                //The new quantity is equal to the bought value
                newQty = qty;

            }
            else
            {
                oldQty = Convert.ToInt32(quantities[stock]);
                Debug.Assert(oldQty > 0);

                //The new quantity will be the value of the old quantity plus the qty value.
                //If qty is a negative number than we are selling, in the other case we're buying
                newQty = oldQty + qty;

                // overflow check; just in case
                if (qty > 0 && newQty <= qty)
                {
                    newQty = oldQty;
                    _log.Warn(this.id + "|Quantity overflow; order ignored: " + stock);
                    return;
                }
            }

            if (newQty < 0)
            {
                //We sold more than we had
                _log.Warn(this.id + "|Not enough stock to sell: " + stock);
                //We interpret this as "sell everything"
                newQty = 0;
            }

            if (newQty == 0)
            {
                //If we sold everything we remove the stock from the internal structure
                quantities.Remove(stock);
            }
            else
            {
                //Save the actual quantity in internal structure
                quantities[stock] = newQty;
            }

            if (this.listener != null)
            {
                //copy the actual listener to a constant that will be used
                //by the asynchronous notification
                PortfolioListener localListener = this.listener;
                //copy the values to constant to be used
                //by the asynchronous notification
                int newVal = newQty;
                int oldVal = oldQty;
                string stockId = stock;

                //If we have a listener, create a task to asynchronously
                //notify the listener of the actual status
                executor.Add(delegate()
                {
                    // call the update on the listener;
                    // in case the listener has just been detached,
                    // the listener should detect the case
                    localListener.Update(stockId, newVal, oldVal);
                });
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetListener(PortfolioListener newListener)
        {
            if (newListener == null)
            {
                //we don't accept a null parameter. to delete the actual listener
                //the RemoveListener method must be used
                return;
            }
            //Set the listener
            this.listener = newListener;

            _log.Debug("Listener set on " + this.id);

            //copy the actual listener to a constant that will be used
            //by the asynchronous notification
            PortfolioListener localListener = newListener;

            //Clone the actual status of the portfolio, for use
            //by the asynchronous notification
            IDictionary currentStatus = (Hashtable)quantities.Clone();

            //Create a task to asynchronously
            //notify the listener of the actual status
            executor.Add(delegate()
            {
                // call the onActualStatus on the listener;
                // in case the listener has just been detached,
                // the listener should detect the case
                localListener.OnActualStatus(currentStatus);
            });

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveListener()
        {
            //remove the listener
            this.listener = null;
        }
    }

}
