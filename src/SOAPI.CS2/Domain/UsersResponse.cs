using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    public class UsersResponse : PagedResponse<User>
    {
        [JsonProperty("users")]
        public override List<User> Items { get; set; }
    }
}
