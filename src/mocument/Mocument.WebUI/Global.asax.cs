using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Configuration;
using Fiddler;
using Mocument.WebUI.Code;

namespace Mocument.WebUI
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
    




        }

        private void Application_End(object sender, EventArgs e)
        {
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        private void Session_Start(object sender, EventArgs e)
        {
    
        }

        private void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}