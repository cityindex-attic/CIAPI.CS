using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salient.JsonSchemaUtilities;

namespace WcfRestService1
{
    public partial class MetaData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var emmitter = new ModelGenerator();
            var schema = new JObject();
            var smd = new JObject();
            smd["version"] = 1.0;
            JObject services = new JObject();
            smd["services"] = services;
            foreach (var route in (List<RouteInfo>)Application["routes"])
            {
                var service = new JObject();
                service["target"] = route.RoutePrefix;
                services[route.RoutePrefix] = service;


                foreach (var method in route.ServiceType.GetMethods())
                {
                    bool publishMethod = false;
                    string httpMethod = "";
                    string uriTemplate = "";
                    var get = (WebGetAttribute)method.GetCustomAttributes(typeof(WebGetAttribute), true).FirstOrDefault();
                    if (get != null)
                    {
                        httpMethod = "GET";
                        uriTemplate = get.UriTemplate;
                        publishMethod = true;

                    }
                    else
                    {
                        // #question: would one put both a get and and invoke on the same method
                        var invoke = (WebInvokeAttribute)method.GetCustomAttributes(typeof(WebInvokeAttribute), true).FirstOrDefault();
                        if (invoke != null)
                        {
                            httpMethod = invoke.Method;
                            uriTemplate = invoke.UriTemplate;
                            publishMethod = true;
                        }
                    }

                    if (publishMethod)
                    {

                        var methodObj = new JObject();
                        methodObj["transport"] = httpMethod;
                        methodObj["uriTemplate"] = uriTemplate;
                        if (method.ReturnType != typeof(void))
                        {
                            var rt = new JObject();
                            emmitter.AssignSchemaType(method.ReturnType, ref rt, ref schema, false);

                            methodObj["returns"] = rt;
                        }

                        service[method.Name] = methodObj;
                    }

                }



            }

            Response.Write("<h1>SMD</h1><br/><pre>" + smd.ToString(Formatting.Indented) + "</pre>");
            Response.Write("<h1>SCHEMA</h1><br/><pre>" + schema.ToString(Formatting.Indented) + "</pre>");
        }



    }
}