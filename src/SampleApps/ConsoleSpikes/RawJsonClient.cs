using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using CityIndex.JsonClient;
using Newtonsoft.Json;

namespace ConsoleSpikes
{
    public static class RawJsonClient
    {
        /// <summary>
        /// Demonstrates the simplest means of retrieving strongly typed data from an HTTP JSON API
        /// </summary>
        public static void SimpleRequest()
        {

            // creates a JSON client with nominal values for throttling and caching
            var client = new Client(new Uri("http://stackauth.com/1.0"));

            SitesResponse response = client.Request<SitesResponse>("sites", "GET");

            foreach (Site site in response.Sites)
            {
                Console.WriteLine("{0}\r\n\t{1}\r\n", site.Name, site.SiteUrl);
            }
        }

        /// <summary>
        /// Demonstrates retrieving parameterized query using uri templates
        /// 
        /// </summary>
        public static void ParameterizedRequest()
        {
            // our target is http://api.stackoverflow.com/1.0/users?filter=sky&page=1&pagesize=10

            // creates a JSON client with nominal values for throttling and caching
            var client = new Client(new Uri("http://api.stackoverflow.com/1.0"));

            string method = "GET";

            string target = "users";


            string uriTemplate = "?filter={filter}&page={page}&pagesize={pagesize}";

            // each entry in the parameters dictionary will replace the template value
            // of the same name. (template value = '{foo}')

            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "filter", "sky" },
        { "page", 1 },
        { "pagesize", 10 },
    };

            UsersResponse response = client.Request<UsersResponse>(target, uriTemplate, method, parameters);

            foreach (User user in response.Users)
            {
                Console.WriteLine("{0} - {1}", user.DisplayName, user.Reputation);
            }

        }


        /// <summary>
        /// A simple partial DTO that resembles the collection element returned from http://api.stackauth.com/1.0/sites
        /// </summary>
        private class Site
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("site_url")]
            public string SiteUrl { get; set; }

            [JsonProperty("api_endpoint")]
            public string ApiEndpoint { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

        }

        /// <summary>
        /// Simple collection wrapper that resembles the root json returned from  http://api.stackauth.com/1.0/sites
        /// </summary>
        private class SitesResponse
        {
            [JsonProperty("api_sites")]
            public List<Site> Sites { get; set; }
        }

        ///<summary>
        /// A simple partial DTO that resembles the collection element returned from http://api.stackoverflow.com/1.0/users?filter=sky&page=1&pagesize=10
        ///</summary>
        private class User
        {
            [JsonProperty("user_id")]
            public int UserId { get; set; }

            [JsonProperty("reputation")]
            public int Reputation { get; set; }

            [JsonProperty("display_name")]
            public string DisplayName { get; set; }

            [JsonProperty("about_me")]
            public string AboutMe { get; set; }

            [JsonProperty("user_type")]
            public string UserType { get; set; }

        }


        /// <summary>
        /// Simple collection wrapper that resembles the root json returned from  http://api.stackoverflow.com/1.0/users?filter=sky&page=1&pagesize=10
        /// </summary>
        private class UsersResponse
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("pagesize")]
            public int PageSize { get; set; }

            [JsonProperty("users")]
            public List<User> Users { get; set; }
        }
    }
}
