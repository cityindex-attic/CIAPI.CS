using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lightstreamer.DotNet.Client;
using Newtonsoft.Json;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightstreamerDtoConverter<TDto> : IMessageConverter<TDto> where TDto : new()
    {
        public virtual TDto Convert(object data)
        {
            var updateInfo = (UpdateInfo) data;
            var dto = new TDto();
            foreach (var property in typeof(TDto).GetProperties())
            {
                PopulateProperty(dto, property.Name, GetCurrentValue(updateInfo, GetFieldIndex(property)));
            }

            return dto;
        }

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
                    _dtoPropertyNames.Sort();
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

        public void PopulateProperty(TDto dto, string propertyName, string value)
        {
            var propertyInfo = typeof(TDto).GetProperty(propertyName);
            object convertedValue = null;
            switch (propertyInfo.PropertyType.FullName)
            {
                case "System.String":
                    convertedValue = value;
                    break;
                case "System.Int32":
                    convertedValue =  System.Convert.ToInt32(value);
                    break;
                case "System.Decimal":
                    convertedValue = System.Convert.ToDecimal(value);
                    break;
                case "System.DateTime":
                    convertedValue = JsonConvert.DeserializeObject<DateTimeOffset>("\"" + value + "\"").DateTime;
                    break;
                default:
                    throw new NotImplementedException(string.Format("Cannot populate fields of type {0} such as {1} on type {2}",
                                                                propertyInfo.PropertyType.FullName, propertyName, typeof(TDto).FullName));

            }

            propertyInfo.SetValue(dto, convertedValue, index: null);
        }
    }
}