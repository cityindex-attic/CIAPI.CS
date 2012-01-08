using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIAPI.DTO;

namespace ApplicationScopingExample
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void ListLogEvents()
        {
            ListBox2.Items.Clear();
            foreach (var evt in Global.adapter.LoggerEvents)
            {
                ListBox2.Items.Add(evt.RenderedMessage);
            }
        }
        private void ListNewsHeadlines()
        {
            ListBox1.Items.Clear();
            ListNewsHeadlinesResponseDTO results = Global.RpcClient.News.ListNewsHeadlinesWithSource("DJ", "UK", 100);
            foreach (var headline in results.Headlines)
            {
                ListBox1.Items.Add(headline.Headline);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ListNewsHeadlines();

            ListLogEvents();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
                       
            Global.adapter.Clear();
            ListLogEvents();
        }
    }
}