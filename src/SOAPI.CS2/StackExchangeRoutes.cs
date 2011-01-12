using System;
using System.Collections.Generic;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Converters;
using SOAPI.CS2.Domain;


namespace SOAPI.CS2
{
    public partial class StackExchangeClient
    {
        //users/{id}

        // id
        // page (optional)
        // pagesize (optional)
        // fromdate (optional)
        // todate (optional)
        // max (optional)
        // min (optional)
        // sort (optional)
        // order (optional)

        private static string ConvertDateTimeOffsetToUnixTimestamp(DateTimeOffset? date)
        {
            return date.HasValue
                ? date.Value.ToUnixTime(true).ToString()
                : null;
        }

        public UsersResponse GetUserById(int id, int? page, int? pagesize, DateTimeOffset? fromdate, DateTimeOffset? todate,
                                     string min = "", string max = "", string sort = "", string order = "")
        {
            return Request<UsersResponse>("users", "/{id}?page={page}&pagesize={pagesize}&fromdate={fromdate}&todate={todate}&max={max}&min={min}&sort={sort}&order={order}&key={key}", "GET", new Dictionary<string, object>
                {
                    {"id", id},
                    {"page", page},
                    {"pagesize", pagesize},
                    {"fromdate", ConvertDateTimeOffsetToUnixTimestamp(fromdate)},
                    {"todate", ConvertDateTimeOffsetToUnixTimestamp(todate)},
                    {"max", max},
                    {"min", min},
                    {"sort", sort},
                    {"order", order}
                }, TimeSpan.FromMinutes(1), "");
        }

        public void BeginGetUserById(ApiAsyncCallback<UsersResponse> callback, object state, int id, int? page,
                                 int? pagesize, DateTimeOffset? fromdate, DateTimeOffset? todate, string min = "",
                                 string max = "", string sort = "", string order = "")
        {
            BeginRequest(callback, state, "users", "/{id}?page={page}&pagesize={pagesize}&fromdate={fromdate}&todate={todate}&max={max}&min={min}&sort={sort}&order={order}&key={key}", "GET", new Dictionary<string, object>
                {
                    {"id", id},
                    {"page", page},
                    {"fromdate", ConvertDateTimeOffsetToUnixTimestamp(fromdate)},
                    {"todate", ConvertDateTimeOffsetToUnixTimestamp(todate)},
                    {"todate", todate},
                    {"max", max},
                    {"min", min},
                    {"sort", sort},
                    {"order", order}
                },
                         TimeSpan.FromMinutes(1), "");
        }

        public UsersResponse EndGetUserById(ApiAsyncResult<UsersResponse> asyncResult)
        {
            return EndRequest(asyncResult);
        }
    }
}