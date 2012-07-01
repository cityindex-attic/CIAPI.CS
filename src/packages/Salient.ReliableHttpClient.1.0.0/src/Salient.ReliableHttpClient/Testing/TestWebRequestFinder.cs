using System;
using System.Collections.Generic;
using System.Text;

namespace Salient.ReliableHttpClient.Testing
{
    public class TestWebRequestFinder
    {

        public void PopulateRequest(TestWebRequest target, RequestInfoBase source)
        {

            target.SetResponseStream(source.ResponseText);
            target.ContentType = source.RequestContentType.ToHeaderValue();
            foreach (var h in source.Headers)
            {
                target.Headers[h.Key] = h.Value;
            }

        }


        public List<RequestInfoBase> Reference { get; set; }

        public RequestInfoBase FindMatchBySingleHeader(TestWebRequest webRequest, string headerKey)
        {
            string headerValue = webRequest.Headers[headerKey];
            foreach (RequestInfoBase r in Reference)
            {
                if (r.Headers.ContainsKey(headerKey))
                {
                    if ((string)r.Headers[headerKey] == headerValue)
                    {
                        return r;
                    }
                }
            }
            return null;
        }
        public RequestInfoBase FindMatchExact(TestWebRequest webRequest)
        {

            foreach (RequestInfoBase r in Reference)
            {
                if (string.Compare(r.Method.ToString(), webRequest.Method, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    continue;
                }

                if (string.Compare(r.Uri.AbsoluteUri, webRequest.RequestUri.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    continue;
                }

                // #hack - RequestInfoBase requestbody is getting set null while TestWebRequest.RequestBody is returning empty string
                // have to decide which is approppriate and standardize
                if ((r.RequestBody ?? "") != webRequest.RequestBody)
                {
                    continue;
                }

                if (r.Headers != null)
                {
                    if (webRequest.Headers == null)
                    {
                        continue;
                    }

                    //#TODO: compare headers
                }

                return r;


            }
            return null;
        }
    }
}