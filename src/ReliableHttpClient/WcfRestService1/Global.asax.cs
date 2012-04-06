using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace WcfRestService1
{
    public class Global : HttpApplication
    {
        private List<RouteInfo> Routes { get; set; }

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }


        private void RegisterRoutes()
        {
            Routes = new List<RouteInfo>();
            AddRoute("Service1", typeof(Service1), new WebServiceHostFactory());
            Application["routes"] = Routes;
        }

        /// <summary>
        /// abstract the adding of routes to let us register them for metadata generation
        /// </summary>
        /// <param name="routePrefix"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceHostFactory"></param>
        private void AddRoute(string routePrefix, Type serviceType, WebServiceHostFactory serviceHostFactory)
        {
            Routes.Add(new RouteInfo { RoutePrefix = routePrefix, ServiceHostFactory = serviceHostFactory, ServiceType = serviceType });
            RouteTable.Routes.Add(new ServiceRoute(routePrefix, serviceHostFactory, serviceType));
        }

    }

    public class RouteInfo
    {
        public string RoutePrefix { get; set; }
        public WebServiceHostFactory ServiceHostFactory { get; set; }
        public Type ServiceType { get; set; }
    }
}
