using System;
using System.Collections;
using System.Text;

using log4net;

using Lightstreamer.Interfaces.Metadata;
using Lightstreamer.Adapters.Metadata;
using Lightstreamer.Adapters.PortfolioDemo.Feed;

namespace Lightstreamer.Adapters.PortfolioDemo.Metadata
{
    /// <summary>
    /// Implements a simple Metadata Adapter suitable for managing client
    /// requests to both the sample Quote Data Adapter and the sample Portfolio
    /// Data Adapter.
    /// It inherits from the LiteralBasedProvider, which is enough for all
    /// demo clients. In addition, it implements the NotifyUserMessage method,
    /// in order to handle "sendMessage" requests from the Portfolio Demo
    /// client. This allows the Portfolio Demo client to use "sendMessage"
    /// in order to submit buy/sell orders to the (simulated) portfolio feed
    /// used by the Portfolio Data Adapter.
    /// 
    /// This is only in order to simplify deployment; in this way, client
    /// order notifications are handled by Lightstreamer Server, without the
    /// need for an additional server to provide the return channel.
    /// Please, consider that it is not recommended to use "sendMessage"
    /// as a return channel in a real case scenario.
    /// </summary>
    class PortfolioMetadataAdapter : LiteralBasedProvider
    {
        /// <summary>
        /// Private logger; we lean on some logging configuration for log4net
        /// in the Remote Server launcher program.
        /// A specific "Lightstreamer.Adapters.PortfolioDemo" log category
        /// can be defined.
        /// </summary>
        private static ILog _log = LogManager.GetLogger("Lightstreamer.Adapters.PortfolioDemo");

        /// <summary>
        /// The associated feed to which buy and sell operations will be forwarded.
        /// </summary>
        private volatile PortfolioFeedSimulator portfolioFeed = null;

        private char[] splitters = new char[1] {'|'};

        public PortfolioMetadataAdapter() 
        {
        }

        public void SetFeed(PortfolioFeedSimulator simulator)
        {
            this.portfolioFeed = simulator;
        }

        public override void Init(IDictionary map, String file)
        {
            if (portfolioFeed == null)
            {
                // SetFeed has not been invoked after the object creation.
                // Note that the related feed is not obtained in Init,
                // based on configuration parameters,
                // but must be provided explicitly,
                // through the custom SetFeed method.
                // As a consequence, this Adapter cannot be managed by the
                // basic DotNetServer_N2.exe launcher provided by LS library,
                // but requires a custom launcher.
                throw new MetadataProviderException("Portfolio feed not configured; currently, portfolio adapters can only be launched by the provided custom launcher");
            }
            base.Init(map, file);
        }

        /// <summary>
        /// Triggered by a client "sendMessage" call.
        /// The message encodes an order entry request by the client.
        /// In this basic implementation, the user is ignored,
        /// we accept messages from any user to modify any portfolio;
        /// session information is ignored too.
        /// </summary>
        public override void NotifyUserMessage(string user, string session, string message) 
        {

            if (message == null) 
            {
                _log.Warn("Null message received");
                throw new NotificationException("Null message received");
            }

            
            string[] pieces = message.Split(splitters);

            this.HandlePortfolioMessage(pieces,message);
        }


        private void HandlePortfolioMessage(string[] operation, string message) 
        {
            if (operation.Length != 4) {
                _log.Warn("Wrong message received: " + message);
                throw new NotificationException("Wrong message received");
            }

            int qty;
            try 
            {
                // Parse the received quantity to be an integer
                qty = Convert.ToInt32(operation[3]);
            }
            catch (FormatException)
            {
                _log.Warn("Wrong message received (quantity must be an integer number): "
                                + message);
                throw new NotificationException("Wrong message received");
            }
            catch (OverflowException)
            {
                throw new NotificationException("Wrong message received");
            }
            
            if (qty <= 0) 
            {
                // Quantity can't be a negative number or 0; just ignore
                _log.Warn("Wrong message received (quantity must be greater than 0): "
                                + message);
                return;
            }

            // get the needed portfolio
            Portfolio portfolio = this.portfolioFeed.GetPortfolio(operation[1]);
            if (portfolio == null) 
            {
                // since the feed creates a new portfolio if no one is available for
                // an id, this will never occur
                _log.Error("No such portfolio: " + operation[1]);
                throw new CreditsException(0, "Portfolio not available", "Portfolio not available");
            }

            if (operation[0] == "BUY") 
            {
                // Call the buy operation on the selected portfolio
                portfolio.Buy(operation[2], qty);
            }
            else if (operation[0] == "SELL")
            {
                // Call the sell operation on the selected portfolio
                portfolio.Sell(operation[2], qty);
            }
            else
            {
                throw new NotificationException("Wrong message received");
            }
        }
    }
}
