using System;
using Lightstreamer.DotNet.Client;

namespace TradingApi.Client.Core.Lightstreamer
{
    public class UpdateDetails
    {
        private UpdateInfo _updateInfo;
        
        public static UpdateDetails From(UpdateInfo updateInfo)
        {
            return new UpdateDetails {_updateInfo = updateInfo};
        }

        public virtual bool IsNull()
        {
            return (_updateInfo.GetOldValue(1) == null && _updateInfo.GetNewValue(1)==null);
        }

        public virtual bool IsValueChanged(int index)
        {
            return _updateInfo.IsValueChanged(index);
        }

        public virtual decimal GetAsDecimal(int index)
        {
            return _updateInfo.IsValueChanged(index) ? Decimal.Parse(_updateInfo.GetNewValue(index)) : Decimal.Parse(_updateInfo.GetOldValue(index));
        }

        public virtual long GetAsLong(int index)
        {
            return _updateInfo.IsValueChanged(index) ? Int64.Parse(_updateInfo.GetNewValue(index)) : Int64.Parse(_updateInfo.GetOldValue(index));
        }

        public virtual int GetAsInt(int index)
        {
            return _updateInfo.IsValueChanged(index) ? Int32.Parse(_updateInfo.GetNewValue(index)) : Int32.Parse(_updateInfo.GetOldValue(index));
        }

        public virtual DateTime GetAsDateTime(int index)
        {
            return _updateInfo.IsValueChanged(index) ? DateTime.Parse(_updateInfo.GetNewValue(index)) : DateTime.Parse(_updateInfo.GetOldValue(index));
        }

        public virtual bool GetAsBool(int index)
        {
            return _updateInfo.IsValueChanged(index) ? bool.Parse(_updateInfo.GetNewValue(index)) : bool.Parse(_updateInfo.GetOldValue(index));
        }

        public virtual string GetAsString(int index)
        {
            return _updateInfo.IsValueChanged(index) ? _updateInfo.GetNewValue(index) : _updateInfo.GetOldValue(index);
        }

        public DateTime GetAsJSONDateTimeUtc(int index)
        {
            return _updateInfo.IsValueChanged(index) ? JSONParser.ParseJSONDateToUtc(_updateInfo.GetNewValue(index)) : JSONParser.ParseJSONDateToUtc(_updateInfo.GetOldValue(index));
        }
    }
}
