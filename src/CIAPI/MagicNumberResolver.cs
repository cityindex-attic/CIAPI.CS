using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;

namespace CIAPI
{

    public class MagicNumberResolver
    {
        private bool _preloaded;
        private static ILog _logger = LogManager.GetLogger(typeof (MagicNumberResolver));

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
#if SILVERLIGHT
                    if(!_preloaded )
                    {
                        throw new Exception("Messages must be preloaded in silverlight/phone");
                    }
#endif

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

        public void PreloadMagicNumbersAsync()
        {
            var gates = new AutoResetEvent[6];
            var g1 = new AutoResetEvent(false);
            gates[0] = g1;
            _client.Messaging.BeginGetSystemLookup(MagicNumberKeys.ApiOrderResponseDTO_StatusReason, 69, a =>
                {

                    try
                    {
                        MagicNumbers[MagicNumberKeys.ApiOrderResponseDTO_StatusReason] = _client.EndRequest<ApiLookupResponseDTO>(a);
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        g1.Set();    
                    }
                    
                }, null);

            var g2 = new AutoResetEvent(false);
            gates[1] = g2;
            _client.Messaging.BeginGetSystemLookup(MagicNumberKeys.ApiOrderResponseDTO_Status, 69, a =>
                {
                    try
                    {
                        MagicNumbers[MagicNumberKeys.ApiOrderResponseDTO_Status] = _client.EndRequest<ApiLookupResponseDTO>(a);
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        g2.Set();
                    }
                }, null);

            var g3 = new AutoResetEvent(false);
            gates[2] = g3;
            _client.Messaging.BeginGetSystemLookup(MagicNumberKeys.ApiTradeOrderResponseDTO_StatusReason, 69, a =>
                {
                    try
                    {
                        MagicNumbers[MagicNumberKeys.ApiTradeOrderResponseDTO_StatusReason] = _client.EndRequest<ApiLookupResponseDTO>(a);

                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        g3.Set();
                    }
                }, null);

            var g4 = new AutoResetEvent(false);
            gates[3] = g4;
            _client.Messaging.BeginGetSystemLookup(MagicNumberKeys.ApiTradeOrderResponseDTO_Status, 69, a =>
                {
                    try
                    {
                        MagicNumbers[MagicNumberKeys.ApiTradeOrderResponseDTO_Status] = _client.EndRequest<ApiLookupResponseDTO>(a);
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        g4.Set();
                    }
                }, null);

            var g5 = new AutoResetEvent(false);
            gates[4] = g5;
            _client.Messaging.BeginGetSystemLookup(MagicNumberKeys.ApiActiveStopLimitOrderDTO_Applicability, 69, a =>
                {
                    try
                    {
                        MagicNumbers[MagicNumberKeys.ApiActiveStopLimitOrderDTO_Applicability] = _client.EndRequest<ApiLookupResponseDTO>(a);
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        g5.Set();
                    }
                }, null);

            var g6 = new AutoResetEvent(false);
            gates[5] = g6;


            _client.Messaging.BeginGetSystemLookup(MagicNumberKeys.ApiOpenPositionDTO_Status, 69, a =>
                {
                    try
                    {
                        MagicNumbers[MagicNumberKeys.ApiOpenPositionDTO_Status] = _client.EndRequest<ApiLookupResponseDTO>(a);
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        g6.Set();
                    }
                }, null);

            // System.Threading.WaitHandle.WaitAll API is not supported is silverlight/phone
            // WaitHandle.WaitAll(gates);

            g1.WaitOne();
            g2.WaitOne();
            g3.WaitOne();
            g4.WaitOne();
            g5.WaitOne();
            g6.WaitOne();
            _preloaded = true;
            _logger.Debug("PreloadMagicNumbersAsync complete");
        }
        public void PreloadMagicNumbers()
        {
            MagicNumbers[MagicNumberKeys.ApiOrderResponseDTO_StatusReason] = _client.Messaging.GetSystemLookup(MagicNumberKeys.ApiOrderResponseDTO_StatusReason, 69);
            MagicNumbers[MagicNumberKeys.ApiOrderResponseDTO_Status] = _client.Messaging.GetSystemLookup(MagicNumberKeys.ApiOrderResponseDTO_Status, 69);
            MagicNumbers[MagicNumberKeys.ApiTradeOrderResponseDTO_StatusReason] = _client.Messaging.GetSystemLookup(MagicNumberKeys.ApiTradeOrderResponseDTO_StatusReason, 69);
            MagicNumbers[MagicNumberKeys.ApiTradeOrderResponseDTO_Status] = _client.Messaging.GetSystemLookup(MagicNumberKeys.ApiTradeOrderResponseDTO_Status, 69);
            MagicNumbers[MagicNumberKeys.ApiActiveStopLimitOrderDTO_Applicability] = _client.Messaging.GetSystemLookup(MagicNumberKeys.ApiActiveStopLimitOrderDTO_Applicability, 69);
            MagicNumbers[MagicNumberKeys.ApiOpenPositionDTO_Status] = _client.Messaging.GetSystemLookup(MagicNumberKeys.ApiOpenPositionDTO_Status, 69);
            _preloaded = true;
            _logger.Debug("PreloadMagicNumbersAsync complete");
        }
    }
}