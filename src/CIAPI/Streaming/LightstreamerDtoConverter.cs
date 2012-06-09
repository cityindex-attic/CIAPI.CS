using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CIAPI.Streaming;
using Lightstreamer.DotNet.Client;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization;


namespace StreamingClient.Lightstreamer
{
    public class LightstreamerDtoConverter<TDto> : IMessageConverter<TDto> where TDto : new()
    {
        private IJsonSerializer _serializer;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LightstreamerDtoConverter<TDto>));
        public LightstreamerDtoConverter(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }
        public virtual TDto Convert(object data)
        {
            var updateInfo = (IUpdateInfo)data;

            var dto = new TDto();
            foreach (var property in typeof(TDto).GetProperties())
            {
                int fieldIndex = GetFieldIndex(property);
                string value = GetCurrentValue(updateInfo, fieldIndex);
                PopulateProperty(dto, property.Name, value);
            }

            return dto;
        }

        public string GetFieldList()
        {
            return string.Join(" ", DtoPropertyNames.ToArray());
        }

        public int GetFieldIndex(PropertyInfo fieldPropertyInfo)
        {
            for (var i = 0; i < DtoPropertyNames.Count; i++)
            {
                if (DtoPropertyNames[i] == fieldPropertyInfo.Name)
                    return i + 1;
            }
            throw new ArgumentException(string.Format("Not able to find a property with name {0}", fieldPropertyInfo.Name));
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

        private static string GetCurrentValue(IUpdateInfo updateInfo, int pos)
        {
            string value;

            if (updateInfo.IsValueChanged(pos))
            {
                value = updateInfo.GetNewValue(pos);

            }
            else
            {
                value = updateInfo.GetOldValue(pos);
            }
            return value;
        }


        /// <summary>
        /// public for testing - too lazy to show internals
        /// </summary>
        /// <param name="type"></param>
        /// <param name="underlyingType"></param>
        /// <returns></returns>
        public static bool IsTypeNullable(Type type, out Type underlyingType)
        {
            // TODO: a static dictionary could act as a cache and improve performance
            underlyingType = type;
            Type uType = Nullable.GetUnderlyingType(type);
            if (uType != null)
            {
                underlyingType = uType;
                return true;
            }
            return false;
        }


        /// <summary>
        /// public for testing - too lazy to show internals
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertPropertyValue(Type pType, string propertyName, string value)
        {
            object convertedValue;
            Type propertyType;

            bool isNullable = IsTypeNullable(pType, out propertyType);
            if (isNullable && string.IsNullOrEmpty(value))
            {
                convertedValue = null;
            }
            else
            {
                switch (Type.GetTypeCode(propertyType))
                {
                    case TypeCode.String:
                        convertedValue = value;
                        break;
                    case TypeCode.DateTime:

                        // Why are we converting from DateTimeOffset to DateTime?
                        // and should we be checking for DateTimeOffset as a property type?
                        if (string.IsNullOrEmpty(value))
                        {
                            // not nullable but json is null - cannot throw because lightstreamer likes to send null updates
                            // and DTO members are not alwasys nullable
                            convertedValue = DateTimeOffset.MinValue.DateTime;

                        }
                        else
                        {

                            convertedValue = _serializer.DeserializeObject<DateTimeOffset>("\"" + value + "\"").DateTime;
                        }
                        break;

                    case TypeCode.Boolean:
                    case TypeCode.Byte:
                    case TypeCode.Char:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:

                        if (string.IsNullOrEmpty(value))
                        {
                            // get a default value
                            convertedValue = Activator.CreateInstance(propertyType);
                        }
                        else
                        {
                            try
                            {
                                convertedValue = System.Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture);
                            }
                            catch (FormatException formatException)
                            {
                                Log.Error(formatException);
                                // get a default value
                                convertedValue = Activator.CreateInstance(propertyType);
                            }
                        }
                        break;

                    case TypeCode.Empty:
                    case TypeCode.Object:
                    case TypeCode.DBNull:
                    default:
                        throw new NotImplementedException(string.Format("Cannot populate fields of type {0} such as {1} on type {2}",
                                                            propertyType.FullName, propertyName, typeof(TDto).FullName));

                }


            }

            return convertedValue;
        }
        public void PopulateProperty(TDto dto, string propertyName, string value)
        {

            try
            {
                var propertyInfo = typeof(TDto).GetProperty(propertyName);
                object convertedValue = ConvertPropertyValue(propertyInfo.PropertyType, propertyInfo.Name, value);
                propertyInfo.SetValue(dto, convertedValue, null);
            }
            catch (Exception ex)
            {

                var ex2 = new Exception(string.Format("Error populating property {0} with {1}", propertyName, value), ex);
                throw ex2;
            }
        }
    }
}