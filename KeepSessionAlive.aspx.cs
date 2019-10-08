using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KeepSessionAlive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null)
        {
            // Refresh this page 60 seconds before session timeout, effectively resetting the session timeout counter.
            MetaRefresh.Attributes["content"] = "300;url=KeepSessionAlive.aspx?q=" + DateTime.Now.Ticks;

            //WindowStatusText = "Last refresh " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }
    }
}