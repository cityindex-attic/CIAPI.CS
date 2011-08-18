using System.Collections.Generic;
using System.Linq;
using CIAPI.DTO;
using CIAPI.Rpc;

namespace CIAPI
{
    
    public class MagicNumberResolver
    {

        public void ResolveMagicNumbers(ListOpenPositionsResponseDTO value)
        {
            if (value.OpenPositions != null)
            {
                foreach (ApiOpenPositionDTO dto in value.OpenPositions)
                {
                    this.ResolveMagicNumbers(dto);
                }    
            }
            
        }

        public void ResolveMagicNumbers(ApiOpenPositionDTO value)
        {
            value.Status_Resolved = this.ResolveMagicNumber(MagicNumberKeys.ApiOpenPositionDTO_Status, value.Status);
        }

        public void ResolveMagicNumbers(ApiActiveStopLimitOrderDTO value)
        {
            value.Applicability_Resolved = this.ResolveMagicNumber(MagicNumberKeys.ApiActiveStopLimitOrderDTO_Applicability, value.Applicability);
        }

        public void ResolveMagicNumbers(ApiTradeOrderResponseDTO value)
        {
            value.StatusReason_Resolved = this.ResolveMagicNumber(MagicNumberKeys.ApiTradeOrderResponseDTO_StatusReason, value.StatusReason);
            value.Status_Resolved = this.ResolveMagicNumber(MagicNumberKeys.ApiTradeOrderResponseDTO_Status, value.Status);

            if (value.Orders != null)
            {
                foreach (ApiOrderResponseDTO order in value.Orders)
                {
                    this.ResolveMagicNumbers(order);
                }    
            }
            
        }

        public void ResolveMagicNumbers(ApiOrderResponseDTO value)
        {
            value.StatusReason_Resolved = this.ResolveMagicNumber(MagicNumberKeys.ApiOrderResponseDTO_StatusReason, value.StatusReason);
            value.Status_Resolved = this.ResolveMagicNumber(MagicNumberKeys.ApiOrderResponseDTO_Status, value.Status);
            if (value.OCO != null)
            {
                this.ResolveMagicNumbers(value.OCO);
            }



            if (value.IfDone != null)
            {
                foreach (ApiIfDoneResponseDTO apiIfDoneResponseDTO in value.IfDone)
                {
                    if (apiIfDoneResponseDTO.Limit != null)
                    {

                        this.ResolveMagicNumbers(apiIfDoneResponseDTO.Limit);
                    }

                    if (apiIfDoneResponseDTO.Stop != null)
                    {
                        this.ResolveMagicNumbers(apiIfDoneResponseDTO.Stop);    
                    }
                    
                }    
            }
            
        }
        
        public void ResolveMagicNumbers(GetOpenPositionResponseDTO value)
        {
            if (value.OpenPosition != null)
            {
                this.ResolveMagicNumbers(value.OpenPosition);
            }
            
        }
        private static readonly Dictionary<string, ApiLookupResponseDTO> MagicNumbers =
            new Dictionary<string, ApiLookupResponseDTO>();

        private readonly Client _client;

        public MagicNumberResolver(Client client)
        {
            _client = client;
        }
         

        public string ResolveMagicNumber(string type, int code)
        {
            lock (MagicNumbers)
            {
                ApiLookupResponseDTO lookup;

                if (!MagicNumbers.TryGetValue(type, out lookup))
                {
                    lookup = _client.Messaging.GetSystemLookup(type, 69);
                    MagicNumbers[type] = lookup;
                }

                ApiLookupDTO item = lookup.ApiLookupDTOList.FirstOrDefault(p => p.Id == code);

                if (item != null)
                {
                    return item.TranslationText;
                }

                // log unresolved code
                return code.ToString();
            }
        }
    }
}