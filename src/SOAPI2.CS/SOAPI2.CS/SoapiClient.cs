using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using CityIndex.JsonClient;
using SOAPI2.Model;


namespace SOAPI2
{

    public partial class SoapiClient : Client
    {

        // https://stackexchange.com/oauth/dialog?client_id=24&scope=no_expiry,read_inbox&redirect_uri=https://stackexchange.com/oauth/login_success

        // #TODO implement 'backoff' logic  ?   ????
        // if backoff is set we need to bump the cache item expiration by that number of seconds, if necessary
        // to do so we need to expose a means of getting the cache item for this specific request
        // we also need to know if this response is coming from the cache already to prevent
        // creating a never expiring cache item
        // i think we are way too far separated here and the layers of abstraction are going to prevent
        // a clean seam. 

        // unique requests should already be cached for at least 1 minute by default so this may not be an issue

        private SoapiClient _client;
        private static string _versionNumber;
        private readonly string _apiKey;
        private readonly string _appId;


        private static string GetVersionNumber()
        {
            if (string.IsNullOrEmpty(_versionNumber))
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                string[] parts = asm.FullName.Split(',');
                _versionNumber = parts[1].Split('=')[1];
            }
            return _versionNumber;
        }


        // #TODO: move AppendParameter functionality to JSONCLIENT
        private static string AppendParameter(string uriTemplate, string parameterName, string parameterValue)
        {
            uriTemplate += "";

            if (parameterValue != null)
            {
                if (uriTemplate == "/")
                {
                    uriTemplate = "";
                }
                uriTemplate += (uriTemplate.Contains("?") ? "&" : "?");
                uriTemplate += parameterName + "=" + parameterValue;
            }
            return uriTemplate;
        }

        private string AppendApiKey(string uriTemplate)
        {
            return AppendParameter(uriTemplate, "key", _apiKey);
        }

        #region network wide methods

        public ResponseWrapperClass<SiteClass> GetSites(int page, int pagesize)
        {
            string uriTemplate = AppendApiKey("?pagesize={pagesize}&page={page}");
            var response = Request<ResponseWrapperClass<SiteClass>>("sites", uriTemplate, "GET",
                                                      new Dictionary<string, object>
                                                          {
                                                              {"page", page},
                                                              {"pagesize", pagesize}
                                                          }, TimeSpan.FromMilliseconds(60000*60), "default");
            
            return response;
        }

        #endregion

        #region site specific methods


        public ResponseWrapperClass<ErrorClass> GetErrors(string site, int page, int pagesize)
        {
            string uriTemplate = AppendApiKey("?site={site}&pagesize={pagesize}&page={page}");
            return Request<ResponseWrapperClass<ErrorClass>>("errors", uriTemplate, "GET",
                                           new Dictionary<string, object>
                                               {
                                                   {"site", site},
                                                   {"page", page},
                                                   {"pagesize", pagesize}
                                               }, TimeSpan.FromMilliseconds(60000), "default");
        }

        public ResponseWrapperClass<InfoClass> GetInfo(string site)
        {
            string uriTemplate = AppendApiKey("?site={site}");
            return Request<ResponseWrapperClass<InfoClass>>("info", uriTemplate, "GET",
                                         new Dictionary<string, object>
                                             {
                                                 {"site", site}
                                             }, TimeSpan.FromMilliseconds(60000), "default");
        }
        #endregion

 
        // #TODO: implement logic in JSONCLient to for decompression, automatic or otherwise
        protected override void BeforeIssueRequest(WebRequest request, string url, string target, string uriTemplate,
                                                   string method, Dictionary<string, object> parameters,
                                                   TimeSpan cacheDuration, string throttleScope)
        {
            ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.GZip;
        }
    }
}