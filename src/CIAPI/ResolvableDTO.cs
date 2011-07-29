using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIAPI.DTO
{
    [Serializable]
    public class ApiTradeOrderResponseDTOResolved:ApiTradeOrderResponseDTO
    {
        
    }
    [Serializable]
    public class ApiOpenPositionDTOResolved : ApiOpenPositionDTO
    {
        public ApiOpenPositionDTOResolved(ApiOpenPositionDTO source)
        {
            this.Currency = source.Currency;
            this.Direction = source.Direction;
            this.LastChangedDateTimeUTC = source.LastChangedDateTimeUTC;
            this.LimitOrder = source.LimitOrder;
            this.MarketId = source.MarketId;
            this.MarketName = source.MarketName;
            this.OrderId = source.OrderId;
            this.Price = source.Price;
            this.Quantity = source.Quantity;
            this.Status = source.Status;
            this.StopOrder = source.StopOrder;
            this.TradingAccountId = source.TradingAccountId;
        }

        public string StatusReason { get; set; }
    }

    /// <summary>
    /// Response containing the open position.
    /// </summary>
    [Serializable]
    public class GetOpenPositionResponseDTOResolved
    {
        public GetOpenPositionResponseDTOResolved(GetOpenPositionResponseDTO source)
        {
            this.OpenPosition = new ApiOpenPositionDTOResolved(source.OpenPosition);
        }

        public ApiOpenPositionDTOResolved OpenPosition { get; set; }
    }

    /// <summary>
    /// Contains the result of a ListOpenPositions query
    /// </summary>
    [Serializable]
    public class ListOpenPositionsResponseDTOResolved
    {
        public ListOpenPositionsResponseDTOResolved(ListOpenPositionsResponseDTO source)
        {
            this.OpenPositions = source.OpenPositions.Select(p => new ApiOpenPositionDTOResolved(p)).ToArray();
        }

        public ApiOpenPositionDTOResolved[] OpenPositions { get; set; }
    }
}
