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

public partial class MSAHistoryReport : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                // Check super Admin condition
                if (Status == true)
                {
                    divcust.Visible = true;
                    divgauge.Visible = false;
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                    divgauge.Visible = true;
                    divcust.Visible = false;
                    ddlsortby.Focus();
                   
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
    protected void ddlcust_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcust.SelectedIndex > 0)
        {
            divgauge.Visible = true;
           
            ddlsortby.Focus();
        }
        else
        {
            divgauge.Visible = false;
          
            ddlcust.Focus();
        }

    }

    protected void btnPrintMSATran_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string gaugeId = lnk.CommandArgument;
            if (String.IsNullOrEmpty(gaugeId))
            {
                return;
            }
            else
            {
                //Response.Redirect("MSAReportViewer.aspx?gaugeId=" + gaugeId);
                string ss = "window.open('MSAReportViewer.aspx?gaugeId=" + gaugeId + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
                string script = "<script language='javascript'>" + ss + "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdMSATranReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMSATranReport.PageIndex = e.NewPageIndex;
        bindMSATranHistoryGrd();
    }

    private void bindMSATranHistoryGrd()
    {
        try
        {
            grdMSATranReport.DataSource = null;
            grdMSATranReport.DataBind();
            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
            // Check super Admin condition
            #region supper Admin wise
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spMSATransactionReport";
            DataTable dt = new DataTable();

            if (Status == true)
            {
                if (ddlcust.SelectedIndex == 0)
                {
                    DataSet ds = q.ProcdureWith7Param(stprocedure, 1, 0, 0, "", "", "", "");
                    dt = ds.Tables[0];
                }
                if (ddlcust.SelectedIndex > 0)
                {
                    int custId = Convert.ToInt32(ddlcust.SelectedValue);

                    if (ddlsortby.SelectedItem.Text == "All")
                    {
                        DataSet ds = q.ProcdureWith7Param(stprocedure, 2, custId, 0, "", "", "", "");
                        dt = ds.Tables[0];
                    }
                    else if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
                    {
                        try
                        {
                            int gaugeId = Convert.ToInt32(searchValue);
                            DataSet ds = q.ProcdureWith7Param(stprocedure, 3, custId, gaugeId, "", "", "", "");
                            dt = ds.Tables[0];
                        }
                        catch (Exception ex)
                        {

                            g.ShowMessage(this.Page, "Gauge Id is accept only numeric value. " + ex.Message);
                        }

                    }
                    else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
                    {

                        DataSet ds = q.ProcdureWith7Param(stprocedure, 4, custId, 0, searchValue, "", "", "");
                        dt = ds.Tables[0];
                    }
                    else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
                    {
                        DataSet ds = q.ProcdureWith7Param(stprocedure, 5, custId, 0, "", searchValue, "", "");
                        dt = ds.Tables[0];
                    }
                    else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
                    {
                        DataSet ds = q.ProcdureWith7Param(stprocedure, 6, custId, 0, "", "", searchValue, "");
                        dt = ds.Tables[0];
                    }
                    else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
                    {
                        DataSet ds = q.ProcdureWith7Param(stprocedure, 7, custId, 0, "", "", "", searchValue);
                        dt = ds.Tables[0];
                    }
                   
                   
                }
                grdMSATranReport.DataSource = dt;
                grdMSATranReport.DataBind();
            }
            #endregion
            #region employee wise

            else
            {
                int custId = Convert.ToInt32(Session["Customer_ID"]);

                if (ddlsortby.SelectedItem.Text == "All")
                {
                    DataSet ds = q.ProcdureWith7Param(stprocedure, 2, custId, 0, "", "", "", "");
                    dt = ds.Tables[0];
                }
                else if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
                {
                    try
                    {
                        int gaugeId = Convert.ToInt32(searchValue);
                        DataSet ds = q.ProcdureWith7Param(stprocedure, 3, custId, gaugeId, "", "", "", "");
                        dt = ds.Tables[0];
                    }
                    catch (Exception ex)
                    {

                        g.ShowMessage(this.Page, "Gauge Id is accept only numeric value. " + ex.Message);
                    }

                }
                else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
                {

                    DataSet ds = q.ProcdureWith7Param(stprocedure, 4, custId, 0, searchValue, "", "", "");
                    dt = ds.Tables[0];
                }
                else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
                {
                    DataSet ds = q.ProcdureWith7Param(stprocedure, 5, custId, 0, "", searchValue, "", "");
                    dt = ds.Tables[0];
                }
                else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
                {
                    DataSet ds = q.ProcdureWith7Param(stprocedure, 6, custId, 0, "", "", searchValue, "");
                    dt = ds.Tables[0];
                }
                else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
                {
                    DataSet ds = q.ProcdureWith7Param(stprocedure, 7, custId, 0, "", "", "", searchValue);
                    dt = ds.Tables[0];
                }
                grdMSATranReport.DataSource = dt;
                grdMSATranReport.DataBind();
               
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
            childId = g.GetChildId("CalibrationHistoryReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    for (int i = 0; i < grdMSATranReport.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdMSATranReport.Rows[i].FindControl("btnPrintMSATran");
                        lnk.Enabled = true;
                        LinkButton lnk1 = (LinkButton)grdMSATranReport.Rows[i].FindControl("LnkDownLoadDocument");
                        lnk1.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdMSATranReport.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdMSATranReport.Rows[i].FindControl("btnPrintMSATran");
                        lnk.Enabled = false;
                        LinkButton lnk1 = (LinkButton)grdMSATranReport.Rows[i].FindControl("LnkDownLoadDocument");
                        lnk1.Enabled = false;
                    }
                }


            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void LnkDownLoadDocument_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int msatranId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select msa_report_uploaded_data, file_name from msa_transaction_TB where msa_transaction_id='" + msatranId + "'");
            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["file_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                Byte[] bytes = (Byte[])dt.Rows[0]["msa_report_uploaded_data"];
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["file_name"].ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.SuppressContent = true;
                //Response.End();
            }
            else
            {
                g.ShowMessage(this.Page, "There is no file.");
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    protected void btnShowMSAHistory_Click(object sender, EventArgs e)
    {
        bindMSATranHistoryGrd();
        btnShowMSAHistory.Focus();
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
            Response.AddHeader("content-disposition", "attachment;filename=MSAHistortReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                //To Export all pages
                grdMSATranReport.AllowPaging = false;
                grdMSATranReport.Columns[12].Visible = false;
                grdMSATranReport.Columns[13].Visible = false;
                grdMSATranReport.Columns[14].Visible = false;
                grdMSATranReport.Columns[15].Visible = false;
                grdMSATranReport.Columns[16].Visible = false;
                this.bindMSATranHistoryGrd();
                grdMSATranReport.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdMSATranReport.HeaderRow.Cells)
                {
                    cell.BackColor = grdMSATranReport.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdMSATranReport.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdMSATranReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdMSATranReport.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grdMSATranReport.RenderControl(hw);
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
    protected void LnkDownLoadLinearity_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int msatranId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select msa_liniarity_uploaded_data, liniarity_file_name from msa_transaction_TB where msa_transaction_id='" + msatranId + "'");
            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["liniarity_file_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                Byte[] bytes = (Byte[])dt.Rows[0]["msa_liniarity_uploaded_data"];
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["liniarity_file_name"].ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.SuppressContent = true;
                //Response.End();
            }
            else
            {
                g.ShowMessage(this.Page, "There is no file.");
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    protected void LnkDownLoadStability_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int msatranId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select msa_stability_uploaded_data, msa_stability_file_name from msa_transaction_TB where msa_transaction_id='" + msatranId + "'");
            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["msa_stability_file_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                Byte[] bytes = (Byte[])dt.Rows[0]["msa_stability_uploaded_data"];
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["msa_stability_file_name"].ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.SuppressContent = true;
                //Response.End();
            }
            else
            {
                g.ShowMessage(this.Page, "There is no file.");
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void LnkDownLoadRR_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int msatranId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select msa_RR_uploaded_data, msa_RR_file_name from msa_transaction_TB where msa_transaction_id='" + msatranId + "'");
            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["msa_RR_file_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                Byte[] bytes = (Byte[])dt.Rows[0]["msa_RR_uploaded_data"];
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["msa_RR_file_name"].ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.SuppressContent = true;
                //Response.End();
            }
            else
            {
                g.ShowMessage(this.Page, "There is no file.");
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        searchBy.Text = "";
        txtsearchValue.Text = "";
        if (ddlsortby.SelectedIndex > 0)
        {
           
            lblName.Visible = true;


            txtsearchValue.Text = "";

            divtxtsearch.Visible = true;

        }
        else
        {
            

            lblName.Text = "";
            txtsearchValue.Text = "";
            lblName.Visible = false;

            divtxtsearch.Visible = false;

        }

        if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
        {
            lblName.Text = "Gauge Name";

        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
        {
            lblName.Text = "Gauge Sr.No.";

        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
        {
            lblName.Text = "Gauge Id";

        }
        else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
        {
            lblName.Text = "Manufacture Id";

        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
        {
            lblName.Text = "Gauge Type";

        }

        else if (ddlsortby.SelectedItem.Text == "All")
        {
            bindMSATranHistoryGrd();
        }
    }
}