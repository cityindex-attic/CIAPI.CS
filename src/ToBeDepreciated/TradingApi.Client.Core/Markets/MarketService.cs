using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using TradingApi.CoreDTO;
using Microsoft.Http;
using TradingApi.Client.Core.Exceptions;

namespace TradingApi.Client.Core.Markets
{

    public class MarketService
    {

        private readonly Connection Conn;

        public MarketService(Connection conn)
        {
            this.Conn = conn;
        }

        public MarketDTO[] GetMarkets(string marketQuery)
        {

            throw new NotImplementedException("needs to be rewritten to handle new format");
            
        }



    }
}
