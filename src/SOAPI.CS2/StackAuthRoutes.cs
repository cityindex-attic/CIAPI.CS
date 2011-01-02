using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityIndex.JsonClient;
using SOAPI.CS2.Domain;

namespace SOAPI.CS2
{
    public partial class StackAuthClient
    {

        public SitesResponse GetSites()
        {

            return Request<SitesResponse>("sites", "/", "GET", new Dictionary<string, object> { }, TimeSpan.FromHours(1), "");
        }

        public void BeginGetSites(ApiAsyncCallback<SitesResponse> callback, object state)
        {
            BeginRequest(callback, state, "sites", "/", "GET", new Dictionary<string, object> { }, TimeSpan.FromHours(1), "");
        }

        public SitesResponse EndGetSites(ApiAsyncResult<SitesResponse> asyncResult)
        {
            return EndRequest(asyncResult);
        }


        public AssociatedUsersResponse GetAssociatedUsers(Guid associationId)
        {

            return Request<AssociatedUsersResponse>("users", "/{id}/associated", "GET", new Dictionary<string, object> { { "id", associationId.ToString() } }, TimeSpan.FromMinutes(1), "");
        }

        public void BeginGetAssociatedUsers(ApiAsyncCallback<AssociatedUsersResponse> callback, object state, Guid associationId)
        {
            BeginRequest(callback, state, "users", "/{id}/associated", "GET", new Dictionary<string, object> { { "id", associationId.ToString() } }, TimeSpan.FromMinutes(1), "");
        }

        public AssociatedUsersResponse EndGetAssociatedUsers(ApiAsyncResult<AssociatedUsersResponse> asyncResult)
        {
            return EndRequest(asyncResult);
        }
    }
}
