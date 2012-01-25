using System;
using System.Collections.Generic;
using System.Net;
using AMEEClient.CS.Model;
using AMEEClient.Model;
using CityIndex.JsonClient;

namespace AMEEClient
{
    public class Client : CityIndex.JsonClient.Client
    {
        // #TODO: implement per request specification of content-type as AMEE varies

        public Client(Uri uri, IRequestController requestController, string basicAuthUsername, string basicAuthPassword)
            : base(uri, requestController, basicAuthUsername, basicAuthPassword)
        {
            RequestController.ContentType = ContentType.FORM;
        }

        public Client(Uri uri, string basicAuthUsername, string basicAuthPassword)
            : base(uri, basicAuthUsername, basicAuthPassword)
        {
            RequestController.ContentType = ContentType.FORM;
        }

        protected override void BeforeIssueRequest(System.Net.WebRequest request, string url, string target, string uriTemplate, string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope)
        {
            ((HttpWebRequest)request).Accept = "application/json";
            base.BeforeIssueRequest(request, url, target, uriTemplate, method, parameters, cacheDuration, throttleScope);
        }

        public GetProfilesResponse GetProfiles()
        {
            string uriTemplate = "/";
            return Request<GetProfilesResponse>("profiles", uriTemplate, "GET",
                                        new Dictionary<string, object>
                                            {
                                            }, TimeSpan.FromMilliseconds(0), "default");
        }
        public DeleteProfileResponse DeleteProfile(string uid)
        {
            string uriTemplate = "/{uid}"; // prevent caching
            return Request<DeleteProfileResponse>("profiles", uriTemplate, "DELETE",
                                        new Dictionary<string, object>
                                        {
                                            {"uid",uid}
                                        }, TimeSpan.FromMilliseconds(0), "default");
        }
        public CreateProfileResponse CreateProfile()
        {
            string uriTemplate = "/"; // prevent caching
            return Request<CreateProfileResponse>("profiles", uriTemplate, "POST",
                                        new Dictionary<string, object>
                                        {
                                            {"profile",true}
                                        }, TimeSpan.FromMilliseconds(0), "default");
        }

        public DataItemResponse GetDataItem(string path, string uid)
        {
            string uriTemplate = "/" + path.Trim(' ', '/') + "/{uid}";


            var parameters = new Dictionary<string, object>() { { "uid", uid } };

            return Request<DataItemResponse>("data", uriTemplate, "GET", parameters, TimeSpan.FromMilliseconds(0), "default");
        }
        public DrillDownResponse GetDrillDown(string path, params ValueItem[] selections)
        {

            string uriTemplate = "/" + path.Trim(' ','/') + "/drill";

            var parameters = new Dictionary<string, object>();
            foreach (var item in selections)
            {
                uriTemplate = AppendParameter(uriTemplate, item.Name, item.Value);
                parameters.Add(item.Name, item.Value);
            }

            return Request<DrillDownResponse>("data", uriTemplate, "GET", parameters, TimeSpan.FromMilliseconds(0), "default");
        }
        public CalculateResponse Calculate(string profileUid, string path, params ValueItem[] selections)
        {

            string uriTemplate = "/{profileUid}/" + path.Trim(' ', '/') + "/";

            var parameters = new Dictionary<string, object>
                                 {
                                     {"profileUid", profileUid}
                                 };

            foreach (var item in selections)
            {
                parameters.Add(item.Name, item.Value);
            }

            return Request<CalculateResponse>("profiles", uriTemplate, "POST", parameters, TimeSpan.FromMilliseconds(0), "default");
        }

        // #TODO: move AppendParameter functionality to JSONCLIENT
        /// <summary>
        /// this is a pretty naive implementation that assumes that parameter names and values will not contain an
        /// unencoded '?'
        /// </summary>
        /// <param name="uriTemplate"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
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
        //DrillDownResponse
        //transport/defra/fuel
    }

    public class nullResponse
    {

    }
}