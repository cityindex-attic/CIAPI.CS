using System;
using Lightstreamer.DotNet.Client;
using Newtonsoft.Json;

namespace CIAPI.Streaming
{
    public class LightstreamerDtoConverterBase
    {
        private static string GetCurrentValue(UpdateInfo updateInfo, int pos)
        {
            return updateInfo.IsValueChanged(pos)
                       ? updateInfo.GetNewValue(pos)
                       : updateInfo.GetOldValue(pos);
        }

        protected static DateTime GetAsJSONDateTimeUtc(UpdateInfo updateInfo, int pos)
        {
            var currentValue = GetCurrentValue(updateInfo, pos);
            return JsonConvert.DeserializeObject<DateTimeOffset>("\"" + currentValue + "\"").Date;
        }

        protected static string GetAsString(UpdateInfo updateInfo, int pos)
        {
            return GetCurrentValue(updateInfo, pos);
        }

        protected static int GetAsInt(UpdateInfo updateInfo, int pos)
        {
            return Convert.ToInt32((string) GetCurrentValue(updateInfo, pos));
        }

        protected decimal GetAsDecimal(UpdateInfo updateInfo, int pos)
        {
            return Convert.ToDecimal(GetCurrentValue(updateInfo, pos));
        }
    }
}