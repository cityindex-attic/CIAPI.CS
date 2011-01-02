using System;
using CityIndex.JsonClient;

namespace SOAPI.CS2
{
    public static class Throttle
    {
        static Throttle()
        {
            Instance = new ThrottedRequestQueue(TimeSpan.FromSeconds(6), 25, 10);
        }

        public static IThrottedRequestQueue Instance { get; set; }
    }
}