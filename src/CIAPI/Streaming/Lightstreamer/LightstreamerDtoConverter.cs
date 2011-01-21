using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lightstreamer.DotNet.Client;
using Newtonsoft.Json;

namespace CIAPI.Streaming.Lightstreamer
{
    public abstract class LightstreamerDtoConverter<TDto> : IMessageConverter<TDto>
    {
        public abstract TDto Convert(object data);

        public string GetFieldList()
        {
            return string.Join(" ", DtoPropertyNames);
        }

        public int GetFieldIndex(PropertyInfo fieldPropertyInfo)
        {
            return DtoPropertyNames.FindIndex(item => item == fieldPropertyInfo.Name) + 1;
        }

        private List<string> _dtoPropertyNames;
        public List<string> DtoPropertyNames
        {
            get
            {
                if (null == _dtoPropertyNames)
                {
                    _dtoPropertyNames = (from propertyInfo in typeof(TDto).GetProperties()
                                         select propertyInfo.Name).ToList();
                }
                return _dtoPropertyNames;
            }
        }

        private static string GetCurrentValue(UpdateInfo updateInfo, int pos)
        {
            return updateInfo.IsValueChanged(pos)
                       ? updateInfo.GetNewValue(pos)
                       : updateInfo.GetOldValue(pos);
        }

        protected static DateTime GetAsJSONDateTimeUtc(UpdateInfo updateInfo, int pos)
        {
            var currentValue = GetCurrentValue(updateInfo, pos);
            return JsonConvert.DeserializeObject<DateTimeOffset>("\"" + currentValue + "\"").DateTime;
        }

        protected static string GetAsString(UpdateInfo updateInfo, int pos)
        {
            return GetCurrentValue(updateInfo, pos);
        }

        protected int GetAsInt(UpdateInfo updateInfo, int pos)
        {
            return System.Convert.ToInt32((string) GetCurrentValue(updateInfo, pos));
        }

        protected decimal GetAsDecimal(UpdateInfo updateInfo, int pos)
        {
            return System.Convert.ToDecimal(GetCurrentValue(updateInfo, pos));
        }
    }
}