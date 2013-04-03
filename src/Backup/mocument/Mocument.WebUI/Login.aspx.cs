using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace Mocument.WebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var rp = new OpenIdRelyingParty();
            var response = rp.GetResponse();
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var fetch = response.GetExtension<FetchResponse>();
                        string email;

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
                   
                    case AuthenticationStatus.Failed:
                        throw new Exception("auth failed");
                  
                }
            }

        }

        protected void BtnGoogleLoginClick(object sender, CommandEventArgs e)
        {
            string discoveryUri = e.CommandArgument.ToString();
            var openid = new OpenIdRelyingParty();
            var urIbuilder = new UriBuilder(Request.Url) { Query = "" };
            var req = openid.CreateRequest(discoveryUri, urIbuilder.Uri, urIbuilder.Uri);
            var fetch = new FetchRequest();
            fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);


            req.AddExtension(fetch);
            req.RedirectToProvider();
        }
    }
}