using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    Genreal g = new Genreal();
    DataTable Dtparent = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_ID"] != null)
        {
            if (!IsPostBack)
            {
                BindMenu();
                DataTable dtAgentName = g.GetCustomerDetails(Convert.ToInt32(Session["Customer_ID"]));
                if (dtAgentName.Rows.Count > 0)
                {
                    string strAgent = dtAgentName.Rows[0]["agent"].ToString();
                    if (strAgent != "")
                    {
                        lt2.Text = "Today : " + DateTime.Now.ToString("dddd, dd MMMM yyyy") + ".  Marketed By: " + dtAgentName.Rows[0]["agent"].ToString();
                    }
                    else
                    {
                        lt2.Text = "Today : " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                    }

                }
                else
                {
                   lt2.Text = "Today : " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                }

            }
        }
        else
        {
            Response.Redirect("~/SessionExpired.aspx");
        }
    }
    private void BindMenu()
    {
        try
        {


            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
            // Check super Admin condition
            if (Status == true)
            {
                #region for super admin
                DataTable dtquery = g.ReturnData("select at.child_id, tct.parentid,tpt.parentnode from authority_TB at left outer join treeviewchild_TB tct on at.child_id=tct.childid left outer join treeviewparent_TB tpt on tct.parentid=tpt.parentid where at.employee_id=" + Convert.ToInt32(Session["User_ID"]) + " and (at.allowcreate=1 or at.allowmodify=1 or at.allowview=1)");

                for (int i = 0; i < dtquery.Rows.Count; i++)
                {
                    int cnt = 0;
                    if (ViewState["Dtparent"] != null)
                    {
                        Dtparent = (DataTable)ViewState["Dtparent"];
                    }
                    else
                    {
                        DataColumn parentid = Dtparent.Columns.Add("parentid");
                        DataColumn parentnode = Dtparent.Columns.Add("parentnode");
                    }
                    DataRow dr = Dtparent.NewRow();
                    dr[0] = dtquery.Rows[i]["parentid"];
                    dr[1] = dtquery.Rows[i]["parentnode"];
                    if (Dtparent.Rows.Count > 0)
                    {
                        for (int f = 0; f < Dtparent.Rows.Count; f++)
                        {
                            if (Dtparent.Rows[f][0].ToString() == dtquery.Rows[i]["parentid"].ToString())
                            {
                                cnt++;
                            }

                        }
                        if (cnt == 0)
                        {
                            Dtparent.Rows.Add(dr);
                            ViewState["Dtparent"] = Dtparent;
                        }
                    }
                    else
                    {
                        Dtparent.Rows.Add(dr);
                        ViewState["Dtparent"] = Dtparent;
                    }
                }

                litMenu.Text = "<ul class='nav navbar-nav'>";
                for (int s = 0; s < Dtparent.Rows.Count; s++)
                {
                    litMenu.Text = litMenu.Text + "<li class='dropdown'><a href='' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><span>" + Dtparent.Rows[s]["parentnode"].ToString() + "</span><i class='caret'></i></a>   <ul  class='dropdown-menu'>";
                    
                    DataTable dtsubdata = g.ReturnData("SELECT ct.parentid, ct.childnode, ct.navigateurl FROM  treeviewchild_TB ct INNER JOIN  authority_TB at ON ct.childid = at.child_id where ct.parentid=" + Convert.ToInt32(Dtparent.Rows[s]["parentid"]) + " and at.employee_id=" + Convert.ToInt32(Session["User_ID"]) + " and (at.allowcreate=1 or at.allowmodify=1 or at.allowview=1)");


                    for (int i = 0; i < dtsubdata.Rows.Count; i++)
                    {


                        litMenu.Text = litMenu.Text + "<li ><a href='" + dtsubdata.Rows[i]["navigateurl"].ToString() + "'><span>" + dtsubdata.Rows[i]["childnode"].ToString() + "</span></a></li>";

                    }
                    litMenu.Text = litMenu.Text + "</ul></li>";
                }
                litMenu.Text = litMenu.Text + "</ul></li>";
                #endregion
            }
            else
            {
                #region
                DataTable dtquery = g.ReturnData("select at.child_id, tct.parentid,tpt.parentnode from authority_TB at left outer join treeviewchild_TB tct on at.child_id=tct.childid left outer join treeviewparent_TB tpt on tct.parentid=tpt.parentid where at.employee_id=" + Convert.ToInt32(Session["User_ID"]) + " and at.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and (at.allowcreate=1 or at.allowmodify=1 or at.allowview=1)");

                for (int i = 0; i < dtquery.Rows.Count; i++)
                {
                    int cnt = 0;
                    if (ViewState["Dtparent"] != null)
                    {
                        Dtparent = (DataTable)ViewState["Dtparent"];
                    }
                    else
                    {
                        DataColumn parentid = Dtparent.Columns.Add("parentid");
                        DataColumn parentnode = Dtparent.Columns.Add("parentnode");
                    }
                    DataRow dr = Dtparent.NewRow();
                    dr[0] = dtquery.Rows[i]["parentid"];
                    dr[1] = dtquery.Rows[i]["parentnode"];
                    if (Dtparent.Rows.Count > 0)
                    {
                        for (int f = 0; f < Dtparent.Rows.Count; f++)
                        {
                            if (Dtparent.Rows[f][0].ToString() == dtquery.Rows[i]["parentid"].ToString())
                            {
                                cnt++;
                            }

                        }
                        if (cnt == 0)
                        {
                            Dtparent.Rows.Add(dr);
                            ViewState["Dtparent"] = Dtparent;
                        }
                    }
                    else
                    {
                        Dtparent.Rows.Add(dr);
                        ViewState["Dtparent"] = Dtparent;
                    }
                }

                litMenu.Text = "<ul class='nav navbar-nav'>";
                for (int s = 0; s < Dtparent.Rows.Count; s++)
                {
                    litMenu.Text = litMenu.Text + "<li class='dropdown'><a href='' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><span>" + Dtparent.Rows[s]["parentnode"].ToString() + "</span><i class='caret'></i></a>   <ul  class='dropdown-menu'>";
                    DataTable dtsubdata = g.ReturnData("SELECT ct.parentid, ct.childnode, ct.navigateurl FROM  treeviewchild_TB ct INNER JOIN  authority_TB at ON ct.childid = at.child_id where ct.parentid=" + Convert.ToInt32(Dtparent.Rows[s]["parentid"]) + " and at.customer_id ="+ Convert.ToInt32(Session["Customer_ID"])+" and at.employee_id=" + Convert.ToInt32(Session["User_ID"]) + " and (at.allowcreate=1 or at.allowmodify=1 or at.allowview=1)");
                    for (int i = 0; i < dtsubdata.Rows.Count; i++)
                    {
                   

                        litMenu.Text = litMenu.Text + "<li ><a href='" +dtsubdata.Rows[i]["navigateurl"].ToString() + "'><span>" +dtsubdata.Rows[i]["childnode"].ToString() + "</span></a></li>";

                    }
                    litMenu.Text = litMenu.Text + "</ul></li>";
                }
                litMenu.Text = litMenu.Text + "</ul></li>";
                #endregion
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    protected void linkLogout_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session.Abandon();//this will cause the Session_End to fire ==> true only when state session mode is InProc
        Session.Clear();
        Response.Redirect("~/Login.aspx");

    }
    protected void LinkDBBackup_Click(object sender, EventArgs e)
    {
        string strIsDone = g.BackUpDB();
        if (strIsDone == "OK")
        {
            string destdir = ConfigurationManager.AppSettings["dataBaseBackupPath"].ToString();
            string strPath = HttpContext.Current.Server.MapPath(destdir);
            g.ShowMessage(this.Page, "Backup is stored on server folder: " + strPath + ".");
        }
        else
        {
            g.ShowMessage(this.Page, "Backup is not stored.");
        }
    }
}