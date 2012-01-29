// 
// Project: SOAPI
// http://soapics.codeplex.com
// http://stackapps.com/questions/386
// 
// Copyright 2010, Sky Sanders
// Licensed under the GPL Version 2 license.
// http://soapics.codeplex.com/license
// 
// Date: June 26 2010 
// API ver 1.0 rev 2010.0709.04
// 
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SOAPI2.Converters
{
    /// <summary>
    ///   Useful when serializing/deserializing json for use with the Stack Overflow API, which produces and consumes Unix Timestamp dates
    /// </summary>
    /// <remarks>
    ///   swiped from lfoust and fixed for latest json.net with some tweaks for handling out-of-range dates
    /// </remarks>
    [CLSCompliant(false)]
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        //public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        //{
        //    if (reader.TokenType != JsonToken.Integer)
        //        throw new Exception("Wrong Token Type");

        //    long ticks = (long)reader.Value;
        //    return ticks.FromUnixTime();
        //}

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long val;
            if (value is DateTime)
            {
                val = ((DateTime)value).ToUnixTime();
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(val);
        }

        /// <summary>
        ///   Reads the JSON representation of the object.
        /// </summary>
        /// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
        /// <param name = "objectType">Type of the object.</param>
        /// <param name = "existingValue">The existing value of object being read.</param>
        /// <param name = "serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
                throw new Exception("Wrong Token Type");

            long ticks = (long)reader.Value;
            return UnixDateTimeHelper.FromUnixTime(ticks);
        }
    }




    /// <summary>
    ///   Useful when serializing/deserializing json for use with the Stack Overflow API, which produces and consumes Unix Timestamp dates
    /// </summary>
    /// <remarks>
    ///   swiped from lfoust and fixed for latest json.net with some tweaks for handling out-of-range dates
    /// </remarks>
    [CLSCompliant(false)]
    public class UnixDateTimeOffsetConverter : DateTimeConverterBase
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long val;
            // TODO: perhaps a bit of leniency is in order here. Can convert a date to datetime offset
            if (value is DateTimeOffset)
            {
                val = ((DateTimeOffset)value).ToUnixTime();
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(val);
        }

        /// <summary>
        ///   Reads the JSON representation of the object.
        /// </summary>
        /// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
        /// <param name = "objectType">Type of the object.</param>
        /// <param name = "existingValue">The existing value of object being read.</param>
        /// <param name = "serializer">The calling serializer.</param>
        /// <exception cref="Exception"></exception>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception("Wrong Token Type");
            }

            var ticks = (long)reader.Value;

            return UnixDateTimeOffsetHelper.FromUnixTime(ticks);
        }
    }

}