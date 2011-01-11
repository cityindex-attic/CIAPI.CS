using System;
using System.Configuration;

namespace CIAPI.Silverlight.Tests
{
    /// <summary>
    /// TODO: devise some way to read this from an external file
    /// </summary>
    public static class TestConfig
    {
        public static string ApiUrl
        {
            get
            {
                return "http://ec2-174-129-8-69.compute-1.amazonaws.com/RESTWebServices/";
            }
        }
        public static string BasicAuthUsername
        {
            get
            {
                return "api";
            }
        }
        public static string BasicAuthPassword
        {
            get
            {
                return "cityindexapi";
            }
        }
        public static string ApiUsername
        {
            get
            {
                return "CC735158";
            }
        }
        public static string ApiPassword
        {
            get
            {
                return "password";
            }
        }
        public static string ApiTestSessionId
        {
            get
            {
                return "D2FF3E4D-01EA-4741-86F0-437C919B5559";
            }
        }


        
    }
}