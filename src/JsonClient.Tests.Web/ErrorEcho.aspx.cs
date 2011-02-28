using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JsonClient.Tests.Web
{
    public partial class ErrorEcho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int errorCode = Convert.ToInt32(Request["code"]);
            if(errorCode!=0)
            {
                Response.StatusCode = errorCode;    
            }

            string errorMessage = Request["message"];
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Response.Write(errorMessage);    
            }
            
        }
    }
}