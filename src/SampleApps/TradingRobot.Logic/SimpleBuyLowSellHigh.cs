using System;
using CIAPI.DTO;
using CIAPI.Rpc;

namespace TradingRobot.Logic
{
    public class SimpleBuyLowSellHigh : Trader
    {
        
        // no logic yet for rollover

        public SimpleBuyLowSellHigh(Client client)
            : base(client)
        {
        }

        public bool PositionIsOpen { get; set; }

        public Decimal BidPrice { get; set; }
        public Decimal LastBidPrice { get; set; }
        public bool BidPending { get; set; }

        public Decimal OfferPrice { get; set; }
        public Decimal LastOfferPrice { get; set; }
        public bool OfferPending { get; set; }

        public override void ProcessTick(PriceDTO price)
        {
            if (PositionIsOpen)
            {
                // check to see if we should start thinking about selling
                if (BidPending)
                {
                    // we are now waiting for the price to stop rising so we can sell.
                    // hopefully it does not plummet past the sell trigger
                    if (price.Bid >= BidPrice)
                    {
                        if (price.Bid >= LastBidPrice)
                        {
                            // still rising, lets wait...
                            LastBidPrice = price.Bid;
                        }
                        else
                        {
                            // price is no longer rising but is still above our trigger so 
                            // SELL SELL SELL
                            ClosePostion(price);
                            BidPending = false;
                            LastBidPrice = 0;
                            PositionIsOpen = false;
                        }
                    }
                    else
                    {
                        // it has fallen and we need to stand down
                        BidPending = false;
                        LastBidPrice = 0;
                    }
                }
                else
                {
                    if (price.Bid >= BidPrice)
                    {
                        // get ready to sell
                        LastBidPrice = price.Bid;
                        BidPending = true;
                    }
                }
            }
            else
            {
                // check to see if we should start thinking about buying

                if (OfferPending)
                {
                    // we saw the price equal or less than to our trigger price at some time
                    // in the past and we are going to be greedy and wait until it stops falling
                    if (price.Offer <= OfferPrice)
                    {
                        // is the current price still falling? this is where some fluctuation can be tolerated by complex logic
                        if (price.Offer < LastOfferPrice)
                        {
                            // yes, just let it fall
                            LastOfferPrice = price.Offer;
                        }
                        else
                        {
                            // no, it is rising, time to buy
                            OpenPosition(price);
                            OfferPending = false;
                            PositionIsOpen = true;
                            LastOfferPrice = 0;
                        }
                    }
                    else
                    {
                        // the price has risen above our trigger price since last tick, cancel pending buy
                        OfferPending = false;
                        LastOfferPrice = 0;
                    }
                }
                else
                {
                    // if the price falls below our offer then go on alert and wait for it to stop falling and then buy
                    if (price.Offer <= OfferPrice)
                    {
                        LastOfferPrice = price.Offer;
                        OfferPending = true;
                    }
                }
            }
        }
    }
}