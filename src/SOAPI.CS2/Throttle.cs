using System;
using CityIndex.JsonClient;

namespace SOAPI.CS2
{
    public static class Throttle
    {
        static Throttle()
        {
            Instance = new ThrottledRequestQueue(TimeSpan.FromSeconds(6), 25, 10,"");
        }

        public static IThrottledRequestQueue Instance { get; set; }
    }
}