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

public partial class ClibrationTransactionStatusReport : System.Web.UI.Page
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
    protected void ddlcust_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcust.SelectedIndex > 0)
        {
            divgauge.Visible = true;           

        }
        else
        {
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
            string stprocedure = "spCalibTransactionStatus";
            DataSet ds = new DataSet();

            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
            // Check super Admin condition
            #region supper Admin wise
            if (Status == true)
            {
                if (ddlcust.SelectedIndex == 0)
                {
                    ds = q.ProcdureWith3Param(stprocedure, 3, 0, "");
                    grdCalibTran.DataSource = ds.Tables[0];
                    grdCalibTran.DataBind();
                }
                if (ddlcust.SelectedIndex > 0)
                {
                    int customerId = Convert.ToInt32(ddlcust.SelectedValue);
                    if (txtsearch.Text == "")
                    {
                        ds = q.ProcdureWith3Param(stprocedure, 1, customerId, "");
                        grdCalibTran.DataSource = ds.Tables[0];

                        grdCalibTran.DataBind();
                    }
                    else if (txtsearch.Text != "")
                    {
                        ds = q.ProcdureWith3Param(stprocedure, 2, customerId, searchValue);
                        grdCalibTran.DataSource = ds.Tables[0];


                        grdCalibTran.DataBind();
                    }
                    else
                    {
                        grdCalibTran.DataSource = null;
                        grdCalibTran.DataBind();
                    }
                }
            }
            #endregion
            #region employee wise

            else
            {
                int customerID = Convert.ToInt32(Session["Customer_ID"]);
                if (txtsearch.Text == "")
                {
                    ds = q.ProcdureWith3Param(stprocedure, 1, customerID, "");
                    grdCalibTran.DataSource = ds.Tables[0];

                    grdCalibTran.DataBind();
                }
                else if (txtsearch.Text != "")
                {
                    ds = q.ProcdureWith3Param(stprocedure, 2, customerID, searchValue);
                    grdCalibTran.DataSource = ds.Tables[0];

                    grdCalibTran.DataBind();
                }
                else
                {
                    grdCalibTran.DataSource = null;
                    grdCalibTran.DataBind();
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
            childId = g.GetChildId("ClibrationTransactionStatusReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    
                }
                else
                {
                    
                }


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
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=CalibrationTransactionStatusReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            grdCalibTran.AllowPaging = false;
           // grdCalibTran.Columns[10].Visible = false;
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}