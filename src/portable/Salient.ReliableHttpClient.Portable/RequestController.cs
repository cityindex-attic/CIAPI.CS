using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Salient.HTTPArchiveModel;

namespace Salient.ReliableHttpClient
{
    internal class RequestController
    {
        public void BeginRequest(RequestData data, HttpAsyncCallback callback, object state)
        {
            var request = WebRequest.CreateHttp(data.Request.Url);
            request.Method = data.Request.Method;
            //data.Request.BodySize;
            //data.Request.Comment;

            //data.Request.Cookies;
            //data.Request.Headers;
            //data.Request.HeadersSize;
            //data.Request.HttpVersion;
            //data.Request.PostData;
            //data.Request.QueryString;

            var gate = new AutoResetEvent(false);
            if (!string.IsNullOrEmpty(data.Request.PostData.Text))
            {

                request.BeginGetRequestStream(ar =>
                {
                    var postStream = request.EndGetRequestStream(ar);
                    byte[] byteArray = Encoding.UTF8.GetBytes(data.Request.PostData.Text);
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Flush();

                    gate.Set();

                }, state);
            }
            else
            {
                gate.Set();
            }

            gate.WaitOne();

            var result = request.BeginGetResponse(ar =>
                {
                    var r = request.EndGetResponse(ar);
                    var s = r.GetResponseStream();
                    var d = ReadFully(s);
                    var txt = Encoding.UTF8.GetString(d, 0, d.Length);
                    data.Response = new Response()
                        {
                            Content = new Content()
                                {
                                    Text = txt
                                }
                        };
                    gate.Set();
                }, state);

            gate.WaitOne();

        }

        public void EndRequest(HttpAsyncResult result)
        {
            
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
