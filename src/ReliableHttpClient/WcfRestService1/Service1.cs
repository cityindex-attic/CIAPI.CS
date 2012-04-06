using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcfRestService1
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Service1
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances

        
        [WebGet(UriTemplate = "")]
        public List<SampleItem> GetCollection()
        {
            // TODO: Replace the current implementation to return a collection of SampleItem instances
            return new List<SampleItem>() { new SampleItem() { Id = 1, StringValue = "Hello" } };
        }

        [WebInvoke(UriTemplate = "", Method = "POST")]
        public SampleItem Create(SampleItem instance)
        {

            instance.Id = 1;
            return instance;
        }

        [WebGet(UriTemplate = "{id}")]
        public SampleItem Get(string id)
        {
            return new SampleItem() { Id = 1, StringValue = "Hello" };
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public SampleItem Update(string id, SampleItem instance)
        {
            return instance;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public void Delete(string id)
        {

        }

    }
}
