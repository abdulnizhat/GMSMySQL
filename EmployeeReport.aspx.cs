using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_EmployeeReport : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                checkAuthority();
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                // Check super Admin condition
                if (Status == true)
                {
                    divcust.Visible = true;
                    divsortby.Visible = false;
                    divemp.Visible = false;
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                    divemp.Visible = false;
                    divsortby.Visible = true;
                    ddlsortby.Focus();
                    divcust.Visible = false;
                    //bindemp(Convert.ToInt32(Session["Customer_ID"]));
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindEmpReportGrd();

    }

    private void bindEmpReportGrd()
    {
        try
        {

            
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                string stprocedure = "spEmployeeDetails";
                DataTable dt = new DataTable();
                // Check super Admin condition
                if (Status == true)
                {
                    if (ddlcust.SelectedIndex == 0)
                    {

                        DataSet ds = q.ProcdureWith4Param(stprocedure, 4,0, "", "");
                        dt = ds.Tables[0];
                       
                    }
                    else if (ddlcust.SelectedIndex > 0)
                    {
                        if (ddlsortby.SelectedIndex == 0)
                        {
                            DataSet ds = q.ProcdureWith4Param(stprocedure, 1, Convert.ToInt32(ddlcust.SelectedValue), "", "");
                            dt = ds.Tables[0];
                        }
                        if (ddlsortby.SelectedIndex == 1)
                        {

                            DataSet ds = q.ProcdureWith4Param(stprocedure, 2, Convert.ToInt32(ddlcust.SelectedValue), txtemp.Text, "");
                                dt = ds.Tables[0];
                           
                        }
                        else
                        {
                            DataSet ds = q.ProcdureWith4Param(stprocedure, 3, Convert.ToInt32(ddlcust.SelectedValue), "", txtMobileNo.Text);
                            dt = ds.Tables[0];

                        }
                    }
                    grdemp.DataSource =dt;
                    grdemp.DataBind();
                }
                else
                {
                    if (ddlsortby.SelectedIndex == 1)
                    {
                        DataSet ds = q.ProcdureWith4Param(stprocedure, 1, Convert.ToInt32(Session["Customer_ID"]), "", "");
                        dt = ds.Tables[0];
                    }
                    if (ddlsortby.SelectedIndex == 1)
                    {
                       
                            DataSet ds = q.ProcdureWith4Param(stprocedure, 2, Convert.ToInt32(Session["Customer_ID"]), txtemp.Text, "");
                            dt = ds.Tables[0];
                       
                    }
                    else
                    {
                        DataSet ds = q.ProcdureWith4Param(stprocedure, 3, Convert.ToInt32(Session["Customer_ID"]), "", txtMobileNo.Text);
                        dt = ds.Tables[0];
                    }
                        grdemp.DataSource =dt;
                    grdemp.DataBind();
                }
           
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
            childId = g.GetChildId("EmployeeReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    for (int i = 0; i < grdemp.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdemp.Rows[i].FindControl("btnprintEmployee");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdemp.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdemp.Rows[i].FindControl("btnprintEmployee");
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
    protected void ddlcust_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcust.SelectedIndex > 0)
        {
            divsortby.Visible = true;
            ddlsortby.SelectedIndex = 0;
            divemp.Visible = false;
            divMobile.Visible = false;
            txtemp.Text="";
            txtMobileNo.Text = "";
        }
        else
        {
            divsortby.Visible = false;
            divemp.Visible = false;
            divMobile.Visible = false;
            txtemp.Text="";
            txtMobileNo.Text = "";
        }
      
    }

   
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsortby.SelectedIndex == 0)
        {
            divemp.Visible = false;
            divMobile.Visible = false;
            txtemp.Text="";
            txtMobileNo.Text = "";
            divDisplaySorting.Visible = false;
        }
        else if (ddlsortby.SelectedIndex == 1)
        {
            divemp.Visible = true;
            txtemp.Visible = true;
            divMobile.Visible = false;
            divDisplaySorting.Visible = true;
            
        }
        else
        {
            divemp.Visible = false;
            txtemp.Text="";
            txtMobileNo.Text = "";
            divMobile.Visible = true;
            divDisplaySorting.Visible = true;
        }


    }

    protected void btnprintEmployee_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        //Response.Redirect("~/EmployeeReportViewer.aspx?empid=" +  lnk.CommandArgument);
        string ss = "window.open('EmployeeReportViewer.aspx?empid=" + lnk.CommandArgument + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
        string script = "<script language='javascript'>" + ss + "</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
    }

    protected void grdemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdemp.PageIndex = e.NewPageIndex;
        bindEmpReportGrd();
    }

    protected void ExportToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=EmployeeReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            grdemp.AllowPaging = false;
            grdemp.Columns[9].Visible = false;
            this.bindEmpReportGrd();

            grdemp.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in grdemp.HeaderRow.Cells)
            {
                cell.BackColor = grdemp.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grdemp.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grdemp.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grdemp.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grdemp.RenderControl(hw);

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

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }
}