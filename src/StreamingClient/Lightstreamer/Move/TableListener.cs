using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
 
using Lightstreamer.DotNet.Client;
using Salient.ReflectiveLoggingAdapter;
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

        public TableListener(string groupOrItemName, int phase)
        {
            _phase = phase;
            _groupOrItemName = groupOrItemName;
        }

        /// <summary>
        /// It seems some streams have a 'spin-up' process that can return an all null update
        /// until the data starts streaming. We were not catching this and null updates were
        /// sometimes throwing exceptions that were logged and then swallowed by LS. I think
        /// a better way is to determine if the update is emtpy (null) and simply not fire if so.
        /// 
        /// Follows is a very simple check
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        private static bool IsUpdateNull(IUpdateInfo update)
        {
            for (int i = 1; i < update.NumFields + 1; i++)
            {
                object value = update.IsValueChanged(i) ? update.GetNewValue(i) : update.GetOldValue(i);
                if (value != null)
                {
                    return false;
                }
            }
            return true;
        }
        void IHandyTableListener.OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            // if all null values simply return.
            if (IsUpdateNull(update))
            {

                return;
            }

            try
            {
                if (MessageReceived == null) return;

                TDto value = _messageConverter.Convert(update);
                var args = new MessageEventArgs<TDto>(_groupOrItemName, value, _phase);
                MessageReceived(this, args);
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
            Logger.Debug(string.Format("OnRawUpdatesLost fired -> itemPos: {0} ietmName: {1} lostUpdates:{2}", itemPos, itemName, lostUpdates));
            /* do nothing */
        }

        void IHandyTableListener.OnSnapshotEnd(int itemPos, string itemName)
        {
            Logger.Debug(string.Format("OnSnapshotEnd fired -> itemPos: {0} ietmName: {1}", itemPos, itemName));
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscr(int itemPos, string itemName)
        {
            Logger.Debug(string.Format("OnUnsubscr fired -> itemPos: {0} ietmName: {1}", itemPos, itemName));
            /* do nothing */
        }

        void IHandyTableListener.OnUnsubscrAll()
        {
            Logger.Debug("OnUnsubscrAll fired");
            /* do nothing */
        }
    }
}
