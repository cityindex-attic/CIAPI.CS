using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIAPI.Rpc;

namespace ScopingSample
{
    public partial class Default : System.Web.UI.Page
    {
        // this client is specific to this asp.net session and is useful for client specific service calls
        private Client SessionClient { get { return (Client)Session["sessionClient"]; } }

        private void RenderSessionInfo()
        {
            if (SessionClient != null)
            {
                lblUserSession.Text = SessionClient.Session;
                LoginPanel.Visible = false;
            }
            else
            {
                LoginPanel.Visible = true;
            }
            lblGlobalSession.Text = Global.RpcClient.Session;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RenderSessionInfo();
            var globalHeadlines = Global.RpcClient.News.ListNewsHeadlinesWithSource("DJ", "UK", 100);
            foreach (var headline in globalHeadlines.Headlines)
            {
                lstGlobalHeadlines.Items.Add(headline.Headline);
            }

        }

        private void LoginSessionClient(string username, string password)
        {
            var sessionClient = new Client(new Uri(WebConfigurationManager.AppSettings["apiRpcUrl"]));
            sessionClient.LogIn(username, password);
            Session["sessionClient"] = sessionClient;

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                LoginSessionClient(txtUserName.Text, txtPassword.Text);
                lblLoginError.Text = "";
                RenderSessionInfo();
            }
            catch (Exception ex)
            {

                lblLoginError.Text = ex.Message;
            }
        }
    }
}