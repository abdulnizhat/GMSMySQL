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

public partial class CalibrationRejHistoryReport : System.Web.UI.Page
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
                    divsearchby.Visible = false;
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                    divgauge.Visible = false;
                    divcust.Visible = false;
                    divsearchby.Visible = false;
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
            divsortby.Visible = true;
            txtsearch.Text = "";
            divgauge.Visible = false;
            //txtsearch.Text = "";

        }
        else
        {
            divsortby.Visible = false;
            divgauge.Visible = false;
            txtsearch.Text = "";
            ddlcust.Focus();
        }

    }
    protected void btnShowCalibHistory_Click(object sender, EventArgs e)
    {
        bindTranHistoryGrd();
        btnShowCalibHistory.Focus();
    }
    protected void btnPrintCalibTranHistory_Click(object sender, EventArgs e)
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
                //Response.Redirect("CalibrationHistoryReportViewer.aspx?gaugeId=" + gaugeId);
                string ss = "window.open('CalibrationHistoryReportViewer.aspx?gaugeId=" + gaugeId + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
                string script = "<script language='javascript'>" + ss + "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdCalibTran_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCalibTran.PageIndex = e.NewPageIndex;
        bindTranHistoryGrd();
    }
    private void bindTranHistoryGrd()
    {
        try
        {
            grdCalibTran.DataSource = null;
            grdCalibTran.DataBind();
            string searchValue = txtsearch.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
            string stprocedure = "spCalibTransactionDt";
            // Check super Admin condition
            #region supper Admin wise
            if (Status == true)
            {
                int customerId;
               
                if (ddlcust.SelectedIndex == 0)
                {

                    if (ddtRejeOrNotInUsed.SelectedIndex == 0)
                    {
                        DataSet ds = q.ProcdureWith3Param(stprocedure, 9, 0, "");
                        grdCalibTran.DataSource = ds.Tables[0];
                        grdCalibTran.DataBind();
                    }
                    else if (ddtRejeOrNotInUsed.SelectedIndex == 1)
                    {
                        if (ddlsortby.SelectedIndex == 0)
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 10, 0, "");
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 1 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 16, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 2 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 18, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 3 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 19, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 4 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 20, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }

                    }
                    else if (ddtRejeOrNotInUsed.SelectedIndex == 2)
                    {
                        if (ddlsortby.SelectedIndex == 0)
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 11, 0, "");
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 1 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 17, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 2 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 21, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 3 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 22, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 4 && txtsearch.Text != "")
                        {
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 23, 0, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }

                    }

                }
                if (ddlcust.SelectedIndex > 0)
                {
                    if (ddtRejeOrNotInUsed.SelectedIndex == 0)
                    {
                        customerId = Convert.ToInt32(ddlcust.SelectedValue);
                        DataSet ds = q.ProcdureWith3Param(stprocedure, 7, customerId, "");
                        grdCalibTran.DataSource = ds.Tables[0];
                        grdCalibTran.DataBind();
                    }
                    if (ddtRejeOrNotInUsed.SelectedIndex == 1)
                    {

                        if (ddlsortby.SelectedIndex == 0)
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 12, customerId, "");
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 1 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 14, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 2 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 24, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 3 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 25, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 4 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 26, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }

                    }

                    if (ddtRejeOrNotInUsed.SelectedIndex == 2)
                    {

                        if (ddlsortby.SelectedIndex == 0)
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 13, customerId, "");
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 1 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 15, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 2 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 27, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 3 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 28, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 4 && txtsearch.Text != "")
                        {
                            customerId = Convert.ToInt32(ddlcust.SelectedValue);
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 29, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }

                    }


                   

                }

            }
            #endregion
            #region employee wise
            else
            {
                int customerId = Convert.ToInt32(Session["Customer_ID"]);
               if (ddtRejeOrNotInUsed.SelectedIndex == 0)
                    {
                        
                        DataSet ds = q.ProcdureWith3Param(stprocedure, 7, customerId, "");
                        grdCalibTran.DataSource = ds.Tables[0];
                        grdCalibTran.DataBind();
                    }
                    if (ddtRejeOrNotInUsed.SelectedIndex == 1)
                    {

                        if (ddlsortby.SelectedIndex == 0)
                        {
                         
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 12, customerId, "");
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 1 && txtsearch.Text != "")
                        {
                            
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 14, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 2 && txtsearch.Text != "")
                        {
                            
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 24, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 3 && txtsearch.Text != "")
                        {
                            
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 25, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 4 && txtsearch.Text != "")
                        {
                           
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 26, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }

                    }

                    if (ddtRejeOrNotInUsed.SelectedIndex == 2)
                    {

                        if (ddlsortby.SelectedIndex == 0)
                        {
                            
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 13, customerId, "");
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 1 && txtsearch.Text != "")
                        {
                            
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 15, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 2 && txtsearch.Text != "")
                        {
                           
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 27, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 3 && txtsearch.Text != "")
                        {
                           
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 28, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }
                        else if (ddlsortby.SelectedIndex == 4 && txtsearch.Text != "")
                        {
                            
                            DataSet ds = q.ProcdureWith3Param(stprocedure, 29, customerId, searchValue);
                            grdCalibTran.DataSource = ds.Tables[0];
                            grdCalibTran.DataBind();
                        }

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
            childId = g.GetChildId("CalibrationRejHistoryReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnExportToExcel.Enabled = true;
                    for (int i = 0; i < grdCalibTran.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibTran.Rows[i].FindControl("btnPrintCalibTranHistory");
                        lnk.Enabled = true;
                        LinkButton lnkdown = (LinkButton)grdCalibTran.Rows[i].FindControl("LnkDownLoadDocument");
                        lnkdown.Enabled = true;
                        LinkButton lnkedit = (LinkButton)grdCalibTran.Rows[i].FindControl("btnEditCalibTran");
                        lnkedit.Enabled = true;
                    }
                }
                else
                {
                    btnExportToExcel.Enabled = false;
                    for (int i = 0; i < grdCalibTran.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibTran.Rows[i].FindControl("btnPrintCalibTranHistory");
                        lnk.Enabled = false;
                        LinkButton lnkdown = (LinkButton)grdCalibTran.Rows[i].FindControl("LnkDownLoadDocument");
                        lnkdown.Enabled = false;
                        LinkButton lnkedit = (LinkButton)grdCalibTran.Rows[i].FindControl("btnEditCalibTran");
                        lnkedit.Enabled = false;
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
            int calibtranId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select calibration_certification_upload, image_name from calibration_transaction_TB where calibration_transaction_id='" + calibtranId + "'");
            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["image_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                Byte[] bytes = (Byte[])dt.Rows[0]["calibration_certification_upload"];
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["image_name"].ToString());
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
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }
    protected void ExportToExcel()
    {
        if (grdCalibTran.Rows.Count > 0)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CalibrationHistoryReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grdCalibTran.AllowPaging = false;
                grdCalibTran.Columns[10].Visible = false;
                this.bindTranHistoryGrd();

                grdCalibTran.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdCalibTran.HeaderRow.Cells)
                {
                    cell.BackColor = grdCalibTran.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdCalibTran.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdCalibTran.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdCalibTran.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grdCalibTran.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        else
        {
            g.ShowMessage(this.Page, "No data available.");
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void btnEditCalibTran_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string strTransactionId = lnk.CommandArgument;
            if (String.IsNullOrEmpty(strTransactionId))
            {
                return;
            }
            else
            {
                strTransactionId = strTransactionId + "," + "RejHis";
                Response.Redirect("~/CalibrationTransaction.aspx?TransactionId=" + strTransactionId);
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
            throw;
        }
    }
    protected void ddtRejeOrNotInUsed_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddtRejeOrNotInUsed.SelectedIndex == 0)
        {
            divsearchby.Visible = false;
            txtsearch.Text = "";
        }
        else
        {
            divsearchby.Visible = true;
            txtsearch.Text = "";
        }
    }
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsortby.SelectedIndex == 0)
        {
            divgauge.Visible = false;
            ddlsortby.Focus();

        }
        else if (ddlsortby.SelectedIndex == 1)
        {
            divgauge.Visible = true;

            lblName.Text = "Gauge Name";
            txtsearch.Text = "";

        }
        else if (ddlsortby.SelectedIndex == 2)
        {
            divgauge.Visible = true;

            lblName.Text = "Gauge Sr.No.";
            txtsearch.Text = "";
        }

        else if (ddlsortby.SelectedIndex == 3)
        {
            divgauge.Visible = true;

            lblName.Text = "Size/Range";
            txtsearch.Text = "";
        }

        else if (ddlsortby.SelectedIndex == 4)
        {
            divgauge.Visible = true;

            lblName.Text = "Manufacture Id";
            txtsearch.Text = "";
        }
    }
}