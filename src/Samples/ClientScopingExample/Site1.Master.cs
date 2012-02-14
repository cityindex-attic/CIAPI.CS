using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientScopingExample
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var headlines = Global.GlobalRpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 10);

            global_headlines.Text = string.Join("<br/>", headlines.Headlines.Select(h => h.Headline).ToArray());
        }
    }
}