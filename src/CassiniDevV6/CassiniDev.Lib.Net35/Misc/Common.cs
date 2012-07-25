//  **********************************************************************************
//  CassiniDev - http://cassinidev.codeplex.com
// 
//  Copyright (c) 2010 Sky Sanders. All rights reserved.
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  
//  This source code is subject to terms and conditions of the Microsoft Public
//  License (Ms-PL). A copy of the license can be found in the license.txt file
//  included in this distribution.
//  
//  You must not remove this notice, or any other, from this software.
//  
//  **********************************************************************************

#region

using System;
using System.Collections.Generic;
//using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;


#endregion

namespace CassiniDev
{
    public static class CommonExtensions
    {

        

        public static string ConvertToHexView(this byte[] value, int numBytesPerRow)
        {
            if (value == null) return null;

            List<string> hexSplit = BitConverter.ToString(value)
                .Replace('-', ' ')
                .Trim()
                .SplitIntoChunks(numBytesPerRow*3)
                .ToList();

            int byteAddress = 0;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexSplit.Count; i++)
            {
                sb.AppendLine(byteAddress.ToString("X4") + ":\t" + hexSplit[i]);
                byteAddress += numBytesPerRow;
            }

            return sb.ToString();
        }

        public static string GetAspVersion()
        {
            string version = null;
            try
            {
                Type type = typeof (Page);
                Assembly assembly = Assembly.GetAssembly(type);

                object[] customAttributes = assembly.GetCustomAttributes(typeof (AssemblyFileVersionAttribute), true);
                if ((customAttributes != null) && (customAttributes.GetLength(0) > 0))
                {
                    version = ((AssemblyFileVersionAttribute) customAttributes[0]).Version;
                }
                else
                {
                    version = assembly.GetName().Version.ToString();
                }
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
            }
            return version;
        }

        

        //public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName)
        //{
        //    int ordinal = row.GetOrdinal(fieldName);
        //    return row.GetValueOrDefault<T>(ordinal);
        //}

        //public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal)
        //{
        //    return (T) (row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal));
        //}

        public static byte[] StreamToBytes(this Stream input)
        {
            int capacity = input.CanSeek ? (int) input.Length : 0;
            using (MemoryStream output = new MemoryStream(capacity))
            {
                int readLength;
                byte[] buffer = new byte[4096];

                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, readLength);
                } while (readLength != 0);

                return output.ToArray();
            }
        }



        private static IList<string> SplitIntoChunks(this string text, int chunkSize)
        {
            List<string> chunks = new List<string>();
            int offset = 0;
            while (offset < text.Length)
            {
                int size = Math.Min(chunkSize, text.Length - offset);
                chunks.Add(text.Substring(offset, size));
                offset += size;
            }
            return chunks;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RunState
    {
        /// <summary>
        /// 
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 
        /// </summary>
        Running
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PortMode
    {
        /// <summary>
        /// 
        /// </summary>
        FirstAvailable = 0,
        /// <summary>
        /// 
        /// </summary>
        Specific
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ErrorField
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        ApplicationPath,
        /// <summary>
        /// 
        /// </summary>
        VirtualPath,
        /// <summary>
        /// 
        /// </summary>
        HostName,
        /// <summary>
        /// 
        /// </summary>
        IsAddHost,
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable InconsistentNaming
        IPAddress,
// ReSharper restore InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable InconsistentNaming
        IPAddressAny,
// ReSharper restore InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable InconsistentNaming
        IPAddressLoopBack,
// ReSharper restore InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        Port,
        /// <summary>
        /// 
        /// </summary>
        PortRangeStart,
        /// <summary>
        /// 
        /// </summary>
        PortRangeEnd,
        /// <summary>
        /// 
        /// </summary>
        PortRange
    }

    /// <summary>
    /// 
    /// </summary>
// ReSharper disable InconsistentNaming
    public enum IPMode
// ReSharper restore InconsistentNaming
    {
        /// <summary>
        /// 
        /// </summary>
        Loopback = 0,
        /// <summary>
        /// 
        /// </summary>
        Any,
        /// <summary>
        /// 
        /// </summary>
        Specific
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// 
        /// </summary>
        Server,
        /// <summary>
        /// 
        /// </summary>
        Hostsfile
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal class CassiniException : Exception
    {
        public CassiniException(string message, ErrorField field, Exception innerException)
            : base(message, innerException)
        {
            Field = field;
        }

        public CassiniException(string message, ErrorField field)
            : this(message, field, null)
        {
        }

        public ErrorField Field { get; set; }
    }
}