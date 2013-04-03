using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mocument.Model;
using Mocument.WebUI.Code;

namespace Mocument.WebUI.Tapes
{
    public partial class MyTapes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/tapes/AddTape.aspx");
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = GridView1.SelectedDataKey.Value as string;

            var ds = new Code.ContextDataSource();
            var entries = ds.ListEntries(id);

            var recordurl = ProxySettings.GetProxyUrl() + "record/" + ProxySettings.MungTapeId(id);
            var playurl = ProxySettings.GetProxyUrl() + "play/" + ProxySettings.MungTapeId(id);
            RecordLabel.Text = "RECORDING URL: " + recordurl;
            PlayLabel.Text = "PLAYBACK URL: " + playurl;

            if (entries.Count==0)
            {
                Panel1.Controls.Add(new LiteralControl("<hr/><p><b>Tape is empty</b></p>"));
            }
            else
            {
                foreach (var entry in entries)
                {
                    Panel1.Controls.Add(new LiteralControl("<hr/>"));
                    var entryTable = Code.EntryRenderer.BuildEntryTable(entry);
                    Panel1.Controls.Add(entryTable);
                }    
            }
            
        }

        protected void ObjectDataSource1_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            Tape t = (Tape)e.InputParameters["tape"];
            t.log = null; // round trip from asp.net does not deserialize so it is new. let's null it so DAL doesn't update it
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var name = e.CommandName;
            var arg = e.CommandArgument;
            switch (name.ToLower())
            {
                case "export":


                    string requested = ProxySettings.GetProxyUrl() + "export/" + ProxySettings.MungTapeId(arg);
                    Response.Redirect(requested);
                    break;
            }

        }


    }
}