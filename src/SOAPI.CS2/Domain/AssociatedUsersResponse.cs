using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    public class AssociatedUsersResponse:CollectionResponse<User>
    {
        [JsonProperty("associated_users")]
        public override List<User> Items { get; set; }
    }
}
