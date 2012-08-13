using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using CIAPI.Streaming.Testing;
using CIAPI.StreamingClient;




namespace CIAPI.Streaming.Testing
{
    /// <summary>
    /// this class will cycle through matching messages and deliver them sequentially
    /// </summary>
    public class SequentialSerializedPriceDTOMessageGenerator
    {
        public static void Populate(PriceDTO target, PriceDTO source)
        {
            target.AuditId = source.AuditId;
            target.Bid = source.Bid;
            target.Change = source.Change;
            target.Direction = source.Direction;
            target.High = source.High;
            target.Low = source.Low;
            target.MarketId = source.MarketId;
            target.Offer = source.Offer;
            target.Price = source.Price;
            target.StatusSummary = source.StatusSummary;
            target.TickDate = source.TickDate;
        }

        public SequentialSerializedPriceDTOMessageGenerator(List<MessageEventArgs<PriceDTO>> messages)
        {
            _messages = messages;
            _indexes = new Dictionary<string, int>();
        }
        private List<MessageEventArgs<PriceDTO>> _messages;
        private Dictionary<string, int> _indexes;
        public MessageEventArgs<PriceDTO> GetNextMessage(string adapter, string topic)
        {

            //#TODO: need to propigate dataadapter, channel and topic in the event


            var messages = _messages.Where(m =>
                (string.Compare(m.DataAdapter, adapter, StringComparison.InvariantCultureIgnoreCase) == 0) && 
                (string.Compare(m.Topic, topic, StringComparison.InvariantCultureIgnoreCase) == 0)
                ).ToArray();

            string address = String.Format("{0}.{1}", adapter.ToUpper(), topic.ToUpper());
            if (messages.Count() == 0)
            {
                throw new Exception(string.Format("no matching messages for {0}", address));
            }

            int index;
            if (_indexes.ContainsKey(address))
            {
                index = _indexes[address];
            }
            else
            {
                index = 0;
            }
            index++;

            if (index >= messages.Count())
            {
                index = 0;
            }

            _indexes[address] = index;

            return messages[index];
        }


    }
}