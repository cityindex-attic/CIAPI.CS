using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace Mocument.WebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenIdRelyingParty rp = new OpenIdRelyingParty();
            var response = rp.GetResponse();
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var fetch = response.GetExtension<FetchResponse>();
                        string email = null;

                        if (fetch != null)
                        {
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);


                        }
                        else
                        {
                            throw new Exception("could not get email from google");
                                                    
                        }
                        if (string.IsNullOrEmpty(email))
                        {
                            throw new Exception("could not get email from google");
                            
                        }
                         
                        FormsAuthentication.RedirectFromLoginPage(email, true);

                        break;
                    case AuthenticationStatus.Canceled:
                        throw new Exception("auth cancelled");
                        break;
                    case AuthenticationStatus.Failed:
                        throw new Exception("auth failed");
                        break;
                }
            }

        }

        protected void btnGoogleLogin_Click(object sender, CommandEventArgs e)
        {
            string discoveryUri = e.CommandArgument.ToString();
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            var URIbuilder = new UriBuilder(Request.Url) { Query = "" };
            var req = openid.CreateRequest(discoveryUri, URIbuilder.Uri, URIbuilder.Uri);
            var fetch = new FetchRequest();
            fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);


            req.AddExtension(fetch);
            req.RedirectToProvider();
        }
    }
}