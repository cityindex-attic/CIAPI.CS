using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Lightstreamer.DotNet.Client;
using StreamingClient.Lightstreamer;

namespace StreamingClient
{
    internal class TableListener<TDto> : IHandyTableListener where TDto : class,new()
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TableListener<TDto>));
        public event EventHandler<MessageEventArgs<TDto>> MessageReceived;
        private readonly LightstreamerDtoConverter<TDto> _messageConverter = new LightstreamerDtoConverter<TDto>();
        private readonly string _groupOrItemName;
        private readonly int _phase;

        public TableListener(string groupOrItemName,int phase)
        {
            _phase = phase;
            _groupOrItemName = groupOrItemName;
        }

        void IHandyTableListener.OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            try
            {
                if (MessageReceived == null) return;

                MessageReceived(this, new MessageEventArgs<TDto>(_groupOrItemName, _messageConverter.Convert(update), _phase));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                // TODO: lightstreamer swallows errors thrown here - live with it or fix lightstreamer client code
                throw;
            }
        }

        void IHandyTableListener.OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
            Logger.DebugFormat("OnRawUpdatesLost fired -> itemPos: {0} ietmName: {1} lostUpdates:{2}", itemPos, itemName, lostUpdates);
            /* do nothing */
        }

        void IHandyTableListener.OnSnapshotEnd(int itemPos, string itemName)
        {
            Logger.DebugFormat("OnSnapshotEnd fired -> itemPos: {0} ietmName: {1}", itemPos, itemName);
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscr(int itemPos, string itemName)
        {
            Logger.DebugFormat("OnUnsubscr fired -> itemPos: {0} ietmName: {1}", itemPos, itemName);
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscrAll()
        {
            Logger.DebugFormat("OnUnsubscrAll fired");
            /* do nothing */
        }
    }
}
