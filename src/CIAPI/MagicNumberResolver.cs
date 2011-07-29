using System.Collections.Generic;
using System.Linq;
using CIAPI.DTO;
using CIAPI.Rpc;

namespace CIAPI
{
    public class MagicNumberResolver
    {
        private static readonly Dictionary<string, ApiLookupResponseDTO> MagicNumbers =
            new Dictionary<string, ApiLookupResponseDTO>();

        private readonly Client _client;

        public MagicNumberResolver(Client client)
        {
            _client = client;
        }

        public GetOpenPositionResponseDTOResolved ResolveGetOpenPositionResponseDTO(GetOpenPositionResponseDTO source)
        {
            var result = new GetOpenPositionResponseDTOResolved(source);
            result.OpenPosition.StatusReason = ResolveMagicNumber(MagicNumberKeys.OrderStatusReason,
                                                                  result.OpenPosition.Status);
            return result;
        }

        public ListOpenPositionsResponseDTOResolved ResolveListOpenPositionsResponseDTO(
            ListOpenPositionsResponseDTO source)
        {
            var result = new ListOpenPositionsResponseDTOResolved(source);
            foreach (ApiOpenPositionDTOResolved openPosition in result.OpenPositions)
            {
                openPosition.StatusReason = ResolveMagicNumber(MagicNumberKeys.OrderStatusReason, openPosition.Status);
            }
            return result;
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