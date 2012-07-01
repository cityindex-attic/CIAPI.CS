// 
// System.Web.HttpUtility (subset of)
//
// Authors:
//   Patrik Torstensson (Patrik.Torstensson@labs2.com)
//   Wictor Wilén (decode/encode functions) (wictor@ibizkit.se)
//   Tim Coleman (tim@timcoleman.com)
//   Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// Copyright (C) 2005-2010 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;

namespace Salient.ReliableHttpClient
{
    public sealed class HttpUtility
    {
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, Encoding.UTF8);
        }

        public static string UrlEncode(string s, Encoding Enc)
        {
            if (s == null)
                return null;

            if (s == String.Empty)
                return String.Empty;

            bool needEncode = false;
            int len = s.Length;
            for (int i = 0; i < len; i++)
            {
                char c = s[i];
                if ((c < '0') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || (c > 'z'))
                {
                    if (HttpEncoder.NotEncoded(c))
                        continue;

                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
                return s;

            // avoided GetByteCount call
            byte[] bytes = new byte[Enc.GetMaxByteCount(s.Length)];
            int realLen = Enc.GetBytes(s, 0, s.Length, bytes, 0);
            byte[] t2 = UrlEncodeToBytes(bytes, 0, realLen);
            return Encoding.UTF8.GetString(t2, 0, t2.Length);
        }

        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null)
                return null;
            return HttpEncoder.UrlEncodeToBytes(bytes, offset, count);
        }

        private class HttpEncoder
        {
            static char[] hexChars = "0123456789abcdef".ToCharArray();

            internal static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
            {
                if (bytes == null)
                    throw new ArgumentNullException("bytes");

                int blen = bytes.Length;
                if (blen == 0)
                    return new byte[0];

                if (offset < 0 || offset >= blen)
                    throw new ArgumentOutOfRangeException("offset");

                if (count < 0 || count > blen - offset)
                    throw new ArgumentOutOfRangeException("count");

                byte[] returnValue;
                using (var result = new MemoryStream(count))
                {
                    int end = offset + count;
                    for (int i = offset; i < end; i++)
                    {
                        UrlEncodeChar((char)bytes[i], result, false);
                    }

                    returnValue = result.ToArray();
                }

                return returnValue;
            }

            internal static bool NotEncoded(char c)
            {
                return (c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_'
#if !NET_4_0
 || c == '\''
#endif
);
            }

            internal static void UrlEncodeChar(char c, Stream result, bool isUnicode)
            {
                if (c > 255)
                {
                    //FIXME: what happens when there is an internal error?
                    //if (!isUnicode)
                    //	throw new ArgumentOutOfRangeException ("c", c, "c must be less than 256");
                    int idx;
                    int i = (int)c;

                    result.WriteByte((byte)'%');
                    result.WriteByte((byte)'u');
                    idx = i >> 12;
                    result.WriteByte((byte)hexChars[idx]);
                    idx = (i >> 8) & 0x0F;
                    result.WriteByte((byte)hexChars[idx]);
                    idx = (i >> 4) & 0x0F;
                    result.WriteByte((byte)hexChars[idx]);
                    idx = i & 0x0F;
                    result.WriteByte((byte)hexChars[idx]);
                    return;
                }

                if (c > ' ' && NotEncoded(c))
                {
                    result.WriteByte((byte)c);
                    return;
                }
                if (c == ' ')
                {
                    result.WriteByte((byte)'+');
                    return;
                }
                if ((c < '0') ||
                    (c < 'A' && c > '9') ||
                    (c > 'Z' && c < 'a') ||
                    (c > 'z'))
                {
                    if (isUnicode && c > 127)
                    {
                        result.WriteByte((byte)'%');
                        result.WriteByte((byte)'u');
                        result.WriteByte((byte)'0');
                        result.WriteByte((byte)'0');
                    }
                    else
                        result.WriteByte((byte)'%');

                    int idx = ((int)c) >> 4;
                    result.WriteByte((byte)hexChars[idx]);
                    idx = ((int)c) & 0x0F;
                    result.WriteByte((byte)hexChars[idx]);
                }
                else
                    result.WriteByte((byte)c);
            }
        }
    }


}