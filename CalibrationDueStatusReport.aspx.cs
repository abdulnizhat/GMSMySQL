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

public partial class CalibrationDueStatusReport : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string dueDate = null;
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                Session["PrintduestatusData"] = null;
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
    protected void grdCalibrationDueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCalibrationDueStatus.PageIndex = e.NewPageIndex;
        bindCalibDueStatusGrd();
    }
    protected void btnShowCalibDueStatus_Click(object sender, EventArgs e)
    {
        bindCalibDueStatusGrd();
    }
    private void bindCalibDueStatusGrd()
    {
        try
        {
            dt = null;
            Session["PrintduestatusData"] = null;
            dueDate = txtNextDueDate.Text;
            if (String.IsNullOrEmpty(dueDate))
            {
                g.ShowMessage(this.Page, "Select Next Due Date.");
                return;
            }
            else
            {
                DateTime dtime = DateTime.ParseExact(dueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dueDate = dtime.ToString("yyyy-MM-dd");
            }
           
                grdCalibrationDueStatus.DataSource = null;
                grdCalibrationDueStatus.DataBind();
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                string stprocedure = "spCalibrationDueStatusReport";
                // Check super Admin condition
                #region supper Admin wise
                if (Status == true)
                {
                    if (ddlcust.SelectedIndex == 0)
                    {// For all customer
                         DataSet ds = new DataSet();
                         if (txtcurrentlocation.Text == "")
                         {
                             ds = q.ProcdureWith4Param(stprocedure, 3, 0, dueDate, "");
                         }
                         else if (txtcurrentlocation.Text != "")
                         {
                             ds = q.ProcdureWith4Param(stprocedure, 5, 0, dueDate, txtcurrentlocation.Text);
                         }
                       // DataSet ds = q.ProcdureWith4Param(stprocedure, 3, 0, dueDate,"");
                        grdCalibrationDueStatus.DataSource = ds.Tables[0];
                       
                        grdCalibrationDueStatus.DataBind();
                       
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["PrintduestatusData"] = ds.Tables[0];
                        }
                        else
                        {
                            Session["PrintduestatusData"] = null;
                        }

                    }
                    else if (ddlcust.SelectedIndex > 0)
                    {
                        DataSet ds = new DataSet();
                        // Customer wise
                        if (txtcurrentlocation.Text=="")
                        {
                             ds = q.ProcdureWith4Param(stprocedure, 2, Convert.ToInt32(ddlcust.SelectedValue), dueDate,"");
                        }
                        else if (txtcurrentlocation.Text != "")
                        {
                            ds = q.ProcdureWith4Param(stprocedure, 4, Convert.ToInt32(ddlcust.SelectedValue), dueDate, txtcurrentlocation.Text);
                        }
                       
                        
                        
                        grdCalibrationDueStatus.DataSource = ds.Tables[0];                       

                        grdCalibrationDueStatus.DataBind();
                      
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["PrintduestatusData"] = ds.Tables[0];
                        }
                        else
                        {
                            Session["PrintduestatusData"] = null;
                        }
                    }
                    else
                    {
                        grdCalibrationDueStatus.DataSource = null;
                        grdCalibrationDueStatus.DataBind();

                        Session["PrintduestatusData"] = null;

                    }
                }
                #endregion
                #region Employee wise
                else
                {
                    // Employee logedin customerwise
                    DataSet ds = new DataSet();
                    // Customer wise
                    if (txtcurrentlocation.Text == "")
                    {
                        ds = q.ProcdureWith4Param(stprocedure, 2, Convert.ToInt32(Session["Customer_ID"]), dueDate, "");
                    }
                    else if (txtcurrentlocation.Text != "")
                    {
                        ds = q.ProcdureWith4Param(stprocedure, 4, Convert.ToInt32(Session["Customer_ID"]), dueDate, txtcurrentlocation.Text);
                    }
                   
                    
                    
                    grdCalibrationDueStatus.DataSource = ds.Tables[0];                   
                    grdCalibrationDueStatus.DataBind();
                    
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["PrintduestatusData"] = ds.Tables[0];
                    }
                    else
                    {
                        Session["PrintduestatusData"] = null;
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
            childId = g.GetChildId("CalibrationDueStatusReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    BtnPrintAll.Enabled = true;
                    for (int i = 0; i < grdCalibrationDueStatus.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibrationDueStatus.Rows[i].FindControl("btnPrintCalibDueStatus");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    BtnPrintAll.Enabled = false;
                    for (int i = 0; i < grdCalibrationDueStatus.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibrationDueStatus.Rows[i].FindControl("btnPrintCalibDueStatus");
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
    protected void btnPrintCalibDueStatus_Click(object sender, EventArgs e)
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
                //Response.Redirect("CalibrationDueStatusReportViewer.aspx?calibId=" + calibId);
                string ss = "window.open('CalibrationDueStatusReportViewer.aspx?calibId=" + calibId + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
                string script = "<script language='javascript'>" + ss + "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void BtnPrintAll_Click(object sender, EventArgs e)
    {
        if (Session["PrintduestatusData"] != null)
        {
            //Response.Redirect("CalibrationDueStatusReportViewer.aspx");
            string ss = "window.open('CalibrationDueStatusReportViewer.aspx?Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
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
            dueDate = txtNextDueDate.Text;
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
            ExportToExcel();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
            return;
        }

    }
    protected void ExportToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=CalibrationDueStatusReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            grdCalibrationDueStatus.AllowPaging = false;
            grdCalibrationDueStatus.Columns[11].Visible = false;
            this.bindCalibDueStatusGrd();
            grdCalibrationDueStatus.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in grdCalibrationDueStatus.HeaderRow.Cells)
            {
                cell.BackColor = grdCalibrationDueStatus.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grdCalibrationDueStatus.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grdCalibrationDueStatus.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grdCalibrationDueStatus.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grdCalibrationDueStatus.RenderControl(hw);
            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}