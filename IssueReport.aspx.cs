using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IssueReport : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    DataTable dtPrint = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["AllPrintData"] = null;
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                // Check super Admin condition
                if (Status == true)
                {
                    divcust.Visible = true;
                    divsortby.Visible = false;
                   
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                  
                    divsortby.Visible = true;
                    ddlsortby.Focus();
                    divcust.Visible = false;
                    divIssueId.Visible = false;
                   
                }
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
   
    private void fillCustomer()
    {
        try
        {
            DataTable dtcust = q.GetCustomerNameId();
            ddlcust.DataSource = dtcust;
            ddlcust.DataTextField = "customer_name";
            ddlcust.DataValueField = "customer_id";
            ddlcust.DataBind();
            ddlcust.Items.Insert(0, "All");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
  
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsortby.SelectedIndex == 0)
        {
            
            divIssueId.Visible = false;
            
            txtIssueId.Text = "";
            lblName.Visible = false;
            ddlsortby.Focus();
        }
        else if (ddlsortby.SelectedIndex == 1)
        {

            divIssueId.Visible = true;
            lblName.Visible = true;
            lblName.Text = "Supplier";
            txtIssueId.Text = "";
            
        }
        else if (ddlsortby.SelectedIndex == 2)
        {

            divIssueId.Visible = true;
            lblName.Visible = true;
            lblName.Text = "Employee";
            txtIssueId.Text = "";
           
        }
        else if (ddlsortby.SelectedIndex == 3)
        {
            lblName.Visible = true;
            lblName.Text = "Gauge";
            txtIssueId.Text = "";
            divIssueId.Visible = true;
           
           
        }
        else if (ddlsortby.SelectedIndex == 4)
        {

            lblName.Visible = true;
            lblName.Text = "Gauge Sr.No.";
            txtIssueId.Text = "";
            divIssueId.Visible = true;
            ddlsortby.Focus();
        }
        else if (ddlsortby.SelectedIndex == 5)
        {
            lblName.Visible = true;
            lblName.Text = "Manufacture Id";
            txtIssueId.Text = "";
            divIssueId.Visible = true;
           
           
        }
       

    }
    protected void ddlcust_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcust.SelectedIndex > 0)
        {
            divsortby.Visible = true;
            ddlsortby.SelectedIndex = 0;
           
            divIssueId.Visible = false;
            
            txtIssueId.Text = "";
            ddlsortby.Focus();
        }
        else
        {
            divsortby.Visible = false;
           
            divIssueId.Visible = false;
           
            txtIssueId.Text = "";
            ddlcust.Focus();
        }
    }
    protected void btnShowIssueReport_Click(object sender, EventArgs e)
    {
        bindIssueReportGrd();
        btnShowIssueReport.Focus();
    }

    private void bindIssueReportGrd()
    {
        try
        {
            Session["AllPrintData"] = null;
            dtPrint = null;
          

                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                string stprocedure = "spIssueDetails";
                // Check super Admin condition
                #region Supper admin
                if (Status == true)
                {
                    if (ddlcust.SelectedIndex == 0)
                    {
                        DataSet ds = q.ProcdureWith6Param(stprocedure, 2, 0, "", "", "","");
                        grdIssueReport.DataSource = ds.Tables[0];
                       
                        grdIssueReport.DataBind();
                       
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["AllPrintData"] = ds.Tables[0];
                        }
                        else
                        {
                            Session["AllPrintData"] = null;
                        }

                    }
                    else if (ddlcust.SelectedIndex > 0)
                    {
                        fillSortWiseData(Convert.ToInt32(ddlcust.SelectedValue));
                    }
                }
                #endregion
                #region Employee wise
                else
                {
                    fillSortWiseData(Convert.ToInt32(Session["Customer_ID"]));
                }
                #endregion
            
            checkAuthority();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("IssueReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnPrintAll.Enabled = true;
                    for (int i = 0; i < grdIssueReport.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdIssueReport.Rows[i].FindControl("btnPrintIssue");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    btnPrintAll.Enabled = false;
                    for (int i = 0; i < grdIssueReport.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdIssueReport.Rows[i].FindControl("btnPrintIssue");
                        lnk.Enabled = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void fillSortWiseData(int custId)
    {
        try
        {
            string searchValue = txtIssueId.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spIssueDetails";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedIndex == 0)
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 1, custId, "", "", "", "");
                grdIssueReport.DataSource = ds.Tables[0];
                grdIssueReport.DataBind();
                 if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["AllPrintData"] = ds.Tables[0];
                }
                else
                {
                    Session["AllPrintData"] = null;
                }
            }
            if (ddlsortby.SelectedIndex == 1)
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 3, custId, "", searchValue,"", "");
                grdIssueReport.DataSource = ds.Tables[0];
                grdIssueReport.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["AllPrintData"] = ds.Tables[0];
                }
                else
                {
                    Session["AllPrintData"] = null;
                }               
                
            }
            else if (ddlsortby.SelectedIndex == 2)
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 4, custId, "", "", searchValue,"");
                grdIssueReport.DataSource = ds.Tables[0];
                grdIssueReport.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["AllPrintData"] = ds.Tables[0];
                }
                else
                {
                    Session["AllPrintData"] = null;
                }

            }
            else if (ddlsortby.SelectedIndex == 3)
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 6, custId, searchValue, "", "","");
                grdIssueReport.DataSource = ds.Tables[0];
                grdIssueReport.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["AllPrintData"] = ds.Tables[0];
                }
                else
                {
                    Session["AllPrintData"] = null;
                }

                
            }
            else if (ddlsortby.SelectedIndex == 4)
            {
                
                            DataSet ds = q.ProcdureWith6Param(stprocedure, 5, custId, "", "", "", searchValue);
                            grdIssueReport.DataSource = ds.Tables[0];
                            grdIssueReport.DataBind();
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["AllPrintData"] = ds.Tables[0];
                            }
                            else
                            {
                                Session["AllPrintData"] = null;
                            }
                    
               

            }
            else if (ddlsortby.SelectedIndex == 5)
            {
                
                    DataSet ds = q.ProcdureWith6Param(stprocedure, 9, custId, "","", "", searchValue);
                    grdIssueReport.DataSource = ds.Tables[0];
                    grdIssueReport.DataBind();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["AllPrintData"] = ds.Tables[0];
                    }
                    else
                    {
                        Session["AllPrintData"] = null;
                    }
               

            }
           
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdIssueReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdIssueReport.PageIndex = e.NewPageIndex;
        bindIssueReportGrd();
    }
    protected void btnPrintIssue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            //Response.Redirect("IssueStatusReportViewer.aspx?issueId=" + lnk.CommandArgument);
            string ss = "window.open('IssueStatusReportViewer.aspx?issueId=" + lnk.CommandArgument + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            string script = "<script language='javascript'>" + ss + "</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    protected void btnPrintAll_Click(object sender, EventArgs e)
    {
        if (Session["AllPrintData"] != null)
        {
            //Response.Redirect("IssueStatusReportViewer.aspx");
            string ss = "window.open('IssueStatusReportViewer.aspx?Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            string script = "<script language='javascript'>" + ss + "</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
        }
        else
        {
            g.ShowMessage(this.Page, "There is no data.");
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }

    protected void ExportToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=IssueReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grdIssueReport.AllowPaging = false;
                grdIssueReport.Columns[9].Visible = false;
                this.bindIssueReportGrd();

                grdIssueReport.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdIssueReport.HeaderRow.Cells)
                {
                    cell.BackColor = grdIssueReport.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdIssueReport.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdIssueReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdIssueReport.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grdIssueReport.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}