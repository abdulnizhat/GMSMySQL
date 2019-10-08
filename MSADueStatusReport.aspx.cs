using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MSADueStatusReport : System.Web.UI.Page
{
    Genreal g = new Genreal();
    string dueDate = null;
    DataTable dt = new DataTable();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                Session["PrintMSAduestatusData"] = null;
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                // Check super Admin condition
                if (Status == true)
                {
                    divcust.Visible = true;
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                    divcust.Visible = false;
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
            DataTable dtcustdata = q.GetCustomerNameId();
            ddlcust.DataSource = dtcustdata;
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
    protected void btnShowMSADueStatus_Click(object sender, EventArgs e)
    {
        try
        {
            dueDate = txtMSADueDate.Text;
            if (String.IsNullOrEmpty(dueDate))
            {
                g.ShowMessage(this.Page, "Select Next Due Date.");
                return;
            }
            else
            {
                DateTime dt = DateTime.ParseExact(dueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dueDate = dt.ToString("yyyy-MM-dd");
            }
            bindMSADueStatusGrd();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
            return;
        }
    }
    private void bindMSADueStatusGrd()
    {
        try
        {
            dt = null;
            Session["PrintMSAduestatusData"] = null;
            grdMSADueStatus.DataSource = null;
            grdMSADueStatus.DataBind();
            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
            string stprocedure = "spMSADueStatusReport";
            // Check super Admin condition
            #region supper Admin wise
            if (Status == true)
            {
                if (ddlcust.SelectedIndex == 0)
                {// For all customer
                    DataSet ds = new DataSet();
                         if (txtcurrentlocation.Text == "")
                         {
                             ds = q.ProcdureWith4Param(stprocedure, 1, 0, dueDate, "");
                         }
                         else if (txtcurrentlocation.Text != "")
                         {
                             ds = q.ProcdureWith4Param(stprocedure, 3, 0, dueDate, txtcurrentlocation.Text);
                         }
                   // DataSet ds = q.ProcdureWith3Param(stprocedure, 1, 0, dueDate);
                    grdMSADueStatus.DataSource = ds.Tables[0];
                    grdMSADueStatus.DataBind();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["PrintMSAduestatusData"] = ds.Tables[0];
                    }
                    else
                    {
                        Session["PrintMSAduestatusData"] = null;
                    }

                }
                else if (ddlcust.SelectedIndex > 0)
                {
                    DataSet ds = new DataSet();
                    // Customer wise
                    if (txtcurrentlocation.Text == "")
                    {
                        ds = q.ProcdureWith4Param(stprocedure, 2, Convert.ToInt32(ddlcust.SelectedValue), dueDate, "");
                    }
                    else if (txtcurrentlocation.Text != "")
                    {
                        ds = q.ProcdureWith4Param(stprocedure, 4, Convert.ToInt32(ddlcust.SelectedValue), dueDate, txtcurrentlocation.Text);
                    }
                   // DataSet ds = q.ProcdureWith3Param(stprocedure, 2, Convert.ToInt32(ddlcust.SelectedValue), dueDate);
                    grdMSADueStatus.DataSource = ds.Tables[0];
                    grdMSADueStatus.DataBind();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["PrintMSAduestatusData"] = ds.Tables[0];
                    }
                    else
                    {
                        Session["PrintMSAduestatusData"] = null;
                    }
                }
                else
                {
                    grdMSADueStatus.DataSource = null;
                    grdMSADueStatus.DataBind();

                    Session["PrintMSAduestatusData"] = null;

                }
            }
            #endregion
            #region Employee wise
            else
            {
                // Employee logedin customerwise
                DataSet ds = new DataSet();
                
                if (txtcurrentlocation.Text == "")
                {
                    ds = q.ProcdureWith4Param(stprocedure, 2, Convert.ToInt32(Session["Customer_ID"]), dueDate, "");
                }
                else if (txtcurrentlocation.Text != "")
                {
                    ds = q.ProcdureWith4Param(stprocedure, 4, Convert.ToInt32(Session["Customer_ID"]), dueDate, txtcurrentlocation.Text);
                }
               // DataSet ds = q.ProcdureWith3Param(stprocedure, 2, Convert.ToInt32(Session["Customer_ID"]), dueDate);
                grdMSADueStatus.DataSource = ds.Tables[0];
                grdMSADueStatus.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["PrintMSAduestatusData"] = ds.Tables[0];
                }
                else
                {
                    Session["PrintMSAduestatusData"] = null;
                }
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
            childId = g.GetChildId("MSADueStatusReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    BtnPrintAllMSA.Enabled = true;
                    for (int i = 0; i < grdMSADueStatus.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdMSADueStatus.Rows[i].FindControl("btnPrintMSADueStatus");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    BtnPrintAllMSA.Enabled = false;
                    for (int i = 0; i < grdMSADueStatus.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdMSADueStatus.Rows[i].FindControl("btnPrintMSADueStatus");
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
    protected void btnPrintMSADueStatus_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string calibId = lnk.CommandArgument;
            if (String.IsNullOrEmpty(calibId))
            {
                return;
            }
            else
            {
                //Response.Redirect("MSADueStatusReportViewer.aspx?calibId=" + calibId);
                string ss = "window.open('MSADueStatusReportViewer.aspx?calibId=" + calibId + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
                string script = "<script language='javascript'>" + ss + "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdMSADueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMSADueStatus.PageIndex = e.NewPageIndex;
        bindMSADueStatusGrd();
    }
    protected void BtnPrintAllMSA_Click(object sender, EventArgs e)
    {
        if (Session["PrintMSAduestatusData"] != null)
        {
            //Response.Redirect("MSADueStatusReportViewer.aspx");
            string ss = "window.open('MSADueStatusReportViewer.aspx?Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            string script = "<script language='javascript'>" + ss + "</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
        }
        else
        {
            g.ShowMessage(this.Page, "There is no data");
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            dueDate = txtMSADueDate.Text;
            if (String.IsNullOrEmpty(dueDate))
            {
                g.ShowMessage(this.Page, "Select Next Due Date.");
                return;
            }
            else
            {
                DateTime dt = DateTime.ParseExact(dueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dueDate = dt.ToString("yyyy-MM-dd");
                ExportToExcel();
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
            return;
        }

    }
    protected void ExportToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=MSADueStatusReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                //To Export all pages
                grdMSADueStatus.AllowPaging = false;
                grdMSADueStatus.Columns[12].Visible = false;
                this.bindMSADueStatusGrd();
                grdMSADueStatus.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdMSADueStatus.HeaderRow.Cells)
                {
                    cell.BackColor = grdMSADueStatus.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdMSADueStatus.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdMSADueStatus.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdMSADueStatus.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grdMSADueStatus.RenderControl(hw);
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