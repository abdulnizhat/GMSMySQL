using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GaugeDetailsReport : System.Web.UI.Page
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
                    divsortby.Visible = false;
                    divgauge.Visible = false;
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                    divgauge.Visible = false;
                    divsortby.Visible = true;
                    ddlsortby.Focus();
                    divcust.Visible = false;
                    //fillGauge(Convert.ToInt32(Session["Customer_ID"]));
                }
                fillExcusiveReport();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void fillExcusiveReport()
    {
        int customerId = 0;
        bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));

        // Check super Admin condition
        if (Status == true)
        {
            if (ddlcust.SelectedIndex > 0)
            {
                customerId = Convert.ToInt32(ddlcust.SelectedValue);
            }
            else
            {
                customerId = Convert.ToInt32(Session["Customer_ID"]);
            }

        }
        else
        {
            customerId = Convert.ToInt32(Session["Customer_ID"]);
        }

        string stprocedure = "spExReportForGauseDetailReport";
        DataSet ds = new DataSet();
        //for total Purchase cost
        ds = q.ProcdureWithTwoParam(stprocedure, 1, customerId);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbltotalPCostlastMonth.Text = ds.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet ds1 = new DataSet();
        ds1 = q.ProcdureWithTwoParam(stprocedure, 2, customerId);
        if (ds1 != null)
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                lbltotalPCostLastYear.Text = ds1.Tables[0].Rows[0][0].ToString();
            }
        }
        DataSet ds2 = new DataSet();
        ds2 = q.ProcdureWithTwoParam(stprocedure, 3, customerId);
        if (ds2 != null)
        {
            if (ds2.Tables[0].Rows.Count > 0)
            {
                lbltotalPCostcycleYTD.Text = ds2.Tables[0].Rows[0][0].ToString();
            }
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
    protected void btnShowGaugeReport_Click(object sender, EventArgs e)
    {
        bindGaugeDetailsReport();
        btnShowGaugeReport.Focus();

    }
    private void bindGaugeDetailsReport()
    {
        try
        {
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            grdGaugeDetailsReport.DataSource = null;
            grdGaugeDetailsReport.DataBind();
            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
            DataTable dtFetchResult = new DataTable();
            string strQuery = q.getGaugeReportDetails();
            // Check super Admin condition
            #region supper Admin wise

            if (Status == true)
            {
                if (ddlcust.SelectedIndex == 0)
                {
                    dtFetchResult = g.ReturnData(strQuery);
                   
                }
                if (ddlcust.SelectedIndex > 0)
                {
                    if (ddlsortby.SelectedIndex == 0)
                    {
                        strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + " ";
                        dtFetchResult = g.ReturnData(strQuery);
                    }
                    if (ddlsortby.SelectedIndex == 1)
                    {


                        strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "   and gt.gauge_name like  '%" + searchValue + "%' ";
                        dtFetchResult = g.ReturnData(strQuery);

                    }
                    if (ddlsortby.SelectedIndex == 2)
                    {
                        
                        strQuery = strQuery + " " + "and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "  and gt.gauge_type like  '%" + searchValue + "%' ";
                        dtFetchResult = g.ReturnData(strQuery);
                       
                    }

                    if (ddlsortby.SelectedIndex == 3)
                    {
                        strQuery = strQuery + " " + "and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "  and gt.size_range like  '%" + searchValue + "%' ";
                        dtFetchResult = g.ReturnData(strQuery);
                    }
                    if (ddlsortby.SelectedIndex == 4)
                    {

                        strQuery = "Select gt.gauge_id,gt.gauge_sr_no,gt.customer_id, gt.gauge_Manufature_Id, gt.gauge_name, gt.gauge_type,gt.size_range, gt.resolution,gt.go_tollerance_plus,gt.go_tollerance_minus, gt.no_go_tollerance_plus,gt.no_go_tollerance_minus,gt.go_were_limit,gt.least_count, gt.permisable_error1,gt.permisable_error2,gt.store_location, gt.current_location,gt.purchase_cost, DATE_FORMAT(gt.purchase_date, '%d/%m/%Y') as purchase_date,DATE_FORMAT(gt.service_date, '%d/%m/%Y') as service_date,DATE_FORMAT(gt.retairment_date, '%d/%m/%Y') as retairment_date,gt.cycles,gt.created_by_id, gt.status, ct.customer_name,em.employee_name, (Select DATE_FORMAT(cs.last_calibration_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id ) as last_calibration_date , (Select DATE_FORMAT(cs.next_due_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id) as next_due_date  from gauge_supplier_link_TB gts Left Outer Join  gaugeMaster_TB as gt ON gts.gauge_id=gt.gauge_id Left Outer Join customer_TB as ct ON gt.customer_id=ct.customer_id Left Outer Join employee_TB as em ON gt.created_by_id=em.employee_id Left Outer Join supplier_TB as sp ON gts.supplier_id=sp.supplier_id  where  gt.status=1 And gt.gauge_id not in (Select clbt.gauge_id from calibration_transaction_TB as clbt where clbt.gauge_id=gt.gauge_id AND (clbt.calibration_status='REJECTED' OR clbt.calibration_status='NOT INUSE' )) and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + " and sp.supplier_name like '%" + searchValue + "%'";
                        dtFetchResult = g.ReturnData(strQuery);
                       

                    }
                    if (ddlsortby.SelectedIndex == 5)
                    {

                        strQuery = "Select gt.gauge_id,gt.gauge_sr_no,gt.customer_id, gt.gauge_Manufature_Id, gt.gauge_name, gt.gauge_type,gt.size_range, gt.resolution,gt.go_tollerance_plus,gt.go_tollerance_minus, gt.no_go_tollerance_plus,gt.no_go_tollerance_minus,gt.go_were_limit,gt.least_count, gt.permisable_error1,gt.permisable_error2,gt.store_location, gt.current_location,gt.purchase_cost,DATE_FORMAT(gt.purchase_date, '%d/%m/%Y') as purchase_date,DATE_FORMAT(gt.service_date, '%d/%m/%Y') as service_date,DATE_FORMAT(gt.retairment_date, '%d/%m/%Y') as retairment_date,gt.cycles,gt.created_by_id, gt.status, ct.customer_name,em.employee_name, (Select DATE_FORMAT(cs.last_calibration_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id ) as last_calibration_date , (Select DATE_FORMAT(cs.next_due_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id) as next_due_date   from gauge_part_link_TB gpt Left Outer Join  gaugeMaster_TB as gt ON gpt.gauge_id=gt.gauge_id Left Outer Join customer_TB as ct ON gt.customer_id=ct.customer_id Left Outer Join employee_TB as em ON gt.created_by_id=em.employee_id Left  Join partMaster_TB as pt ON gpt.part_id=pt.part_id  where  gt.status=1 And gt.gauge_id not in (Select clbt.gauge_id from calibration_transaction_TB as clbt where clbt.gauge_id=gt.gauge_id AND (clbt.calibration_status='REJECTED' OR clbt.calibration_status='NOT INUSE' )) and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "  and pt.part_number like '%" + searchValue + "%'";
                        dtFetchResult = g.ReturnData(strQuery);
                        
                    }
                    if (ddlsortby.SelectedIndex == 6)
                    {
                        
                        strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "   and gt.gauge_sr_no like  '%" + searchValue + "%' ";
                        dtFetchResult = g.ReturnData(strQuery);
                    }
                   
                    if (ddlsortby.SelectedIndex == 7)
                    {

                        strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "   and gt.current_location like  '%" + searchValue + "%' ";
                        dtFetchResult = g.ReturnData(strQuery);
                    }
                    if (ddlsortby.SelectedIndex == 8)
                    {

                        strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(ddlcust.SelectedValue) + "   and gt.gauge_Manufature_Id like  '%" + searchValue + "%' ";
                        dtFetchResult = g.ReturnData(strQuery);
                    }
                }
                grdGaugeDetailsReport.DataSource = dtFetchResult;
                grdGaugeDetailsReport.DataBind();
            }
            #endregion
            #region employee wise

            else
            {
                if (ddlsortby.SelectedIndex == 0)
                {
                    strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " ";
                    dtFetchResult = g.ReturnData(strQuery);
                }
                if (ddlsortby.SelectedIndex == 1)
                {
                   
                    strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "   and gt.gauge_name like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                   
                }
                if (ddlsortby.SelectedIndex == 2)
                {
                    
                    strQuery = strQuery + " " + "and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "  and gt.gauge_type like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                   

                }
                if (ddlsortby.SelectedIndex == 3)
                {
                    strQuery = strQuery + " " + "and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "  and gt.size_range like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                }
                if (ddlsortby.SelectedIndex == 4)
                {
                    strQuery = "Select gt.gauge_id,gt.gauge_sr_no,gt.customer_id, gt.gauge_Manufature_Id, gt.gauge_name, gt.gauge_type,gt.size_range, gt.resolution,gt.go_tollerance_plus,gt.go_tollerance_minus, gt.no_go_tollerance_plus,gt.no_go_tollerance_minus,gt.go_were_limit,gt.least_count, gt.permisable_error1,gt.permisable_error2,gt.store_location, gt.current_location,gt.purchase_cost,DATE_FORMAT(gt.purchase_date, '%d/%m/%Y') as purchase_date,DATE_FORMAT(gt.service_date, '%d/%m/%Y') as service_date,DATE_FORMAT(gt.retairment_date, '%d/%m/%Y') as retairment_date,gt.cycles,gt.created_by_id, gt.status, ct.customer_name,em.employee_name, (Select DATE_FORMAT(cs.last_calibration_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id ) as last_calibration_date,(Select DATE_FORMAT(cs.next_due_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id) as next_due_date   from gauge_supplier_link_TB gts Left Outer Join  gaugeMaster_TB as gt ON gts.gauge_id=gt.gauge_id Left Outer Join customer_TB as ct ON gt.customer_id=ct.customer_id Left Outer Join employee_TB as em ON gt.created_by_id=em.employee_id Left Outer Join supplier_TB as sp ON gts.supplier_id=sp.supplier_id  where  gt.status=1 And gt.gauge_id not in (Select clbt.gauge_id from calibration_transaction_TB as clbt where clbt.gauge_id=gt.gauge_id AND (clbt.calibration_status='REJECTED' OR clbt.calibration_status='NOT INUSE' )) and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and  sp.supplier_name like '%" + searchValue + "%'";
                    dtFetchResult = g.ReturnData(strQuery);
                    
                }
                if (ddlsortby.SelectedIndex == 5)
                {

                    strQuery = "Select gt.gauge_id,gt.gauge_sr_no,gt.customer_id, gt.gauge_Manufature_Id, gt.gauge_name, gt.gauge_type,gt.size_range, gt.resolution,gt.go_tollerance_plus,gt.go_tollerance_minus, gt.no_go_tollerance_plus,gt.no_go_tollerance_minus,gt.go_were_limit,gt.least_count, gt.permisable_error1,gt.permisable_error2,gt.store_location, gt.current_location,gt.purchase_cost, DATE_FORMAT(gt.purchase_date, '%d/%m/%Y') as purchase_date,DATE_FORMAT(gt.service_date, '%d/%m/%Y') as service_date,DATE_FORMAT(gt.retairment_date, '%d/%m/%Y') as retairment_date,gt.cycles,gt.created_by_id, gt.status, ct.customer_name,em.employee_name,  (Select DATE_FORMAT(cs.last_calibration_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id ) as last_calibration_date,(Select DATE_FORMAT(cs.next_due_date, '%d/%m/%Y') from calibration_schedule_TB cs where cs.status=1 and cs.gauge_id=gt.gauge_id) as next_due_date   from gauge_part_link_TB gpt Left Outer Join  gaugeMaster_TB as gt ON gpt.gauge_id=gt.gauge_id Left Outer Join customer_TB as ct ON gt.customer_id=ct.customer_id Left Outer Join employee_TB as em ON gt.created_by_id=em.employee_id Left  Join partMaster_TB as pt ON gpt.part_id=pt.part_id  where  gt.status=1 And gt.gauge_id not in (Select clbt.gauge_id from calibration_transaction_TB as clbt where clbt.gauge_id=gt.gauge_id AND (clbt.calibration_status='REJECTED' OR clbt.calibration_status='NOT INUSE' )) and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "  and pt.part_number like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                    
                }
                if (ddlsortby.SelectedIndex == 6)
                {
                   
                    strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "   and gt.gauge_sr_no like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                }
               
                if (ddlsortby.SelectedIndex == 7)
                {

                    strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "   and gt.current_location like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                }
                if (ddlsortby.SelectedIndex == 8)
                {

                    strQuery = strQuery + " " + " and gt.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "   and gt.gauge_Manufature_Id like  '%" + searchValue + "%' ";
                    dtFetchResult = g.ReturnData(strQuery);
                }
                grdGaugeDetailsReport.DataSource = dtFetchResult;
                grdGaugeDetailsReport.DataBind();
               
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
            childId = g.GetChildId("GaugeDetailsReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    for (int i = 0; i < grdGaugeDetailsReport.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGaugeDetailsReport.Rows[i].FindControl("btnPrintGaugeDetailsReport");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdGaugeDetailsReport.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGaugeDetailsReport.Rows[i].FindControl("btnPrintGaugeDetailsReport");
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
            divgauge.Visible = false;
            //ddlGaugeWise.Items.Clear();
            // ddlGaugeTypeWise.SelectedIndex = 0;
            ddlsortby.Focus();
        }
        else
        {
            divsortby.Visible = false;
            divgauge.Visible = false;
            //ddlGaugeWise.Items.Clear();
            // ddlGaugeTypeWise.SelectedIndex = 0;
            ddlcust.Focus();
        }
    }
    protected void grdGaugeDetailsReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGaugeDetailsReport.PageIndex = e.NewPageIndex;
        bindGaugeDetailsReport();
    }
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
        if (ddlsortby.SelectedIndex == 0)
        {
            divgauge.Visible = false;
            ddlsortby.Focus();

        }
        else if (ddlsortby.SelectedIndex == 1)
        {
            divgauge.Visible = true;

            lblName.Text = "Gauge Name";
            txtsearchValue.Text = "";



        }
        else if (ddlsortby.SelectedIndex == 2)
        {
            divgauge.Visible = true;

            lblName.Text = "Gauge Type";
            txtsearchValue.Text = "";
        }
        else if (ddlsortby.SelectedIndex == 3)
        {
            divgauge.Visible = true;

            lblName.Text = "Size/Range";
            txtsearchValue.Text = "";
        }

        else if (ddlsortby.SelectedIndex == 4)
        {
            divgauge.Visible = true;

            lblName.Text = "Supplier Name";
            txtsearchValue.Text = "";

        }
        else if (ddlsortby.SelectedIndex == 5)
        {
            divgauge.Visible = true;

            lblName.Text = "Part No.";
            txtsearchValue.Text = "";


        }
        else if (ddlsortby.SelectedIndex == 6)
        {
            divgauge.Visible = true;

            lblName.Text = "Gauge Sr.No.";
            txtsearchValue.Text = "";
        }
       
        else if (ddlsortby.SelectedIndex == 7)
        {
            divgauge.Visible = true;

            lblName.Text = "Current Location";
            txtsearchValue.Text = "";
        }
        else if (ddlsortby.SelectedIndex == 8)
        {
            divgauge.Visible = true;

            lblName.Text = "Manufacture Id";
            txtsearchValue.Text = "";
        }
    }

    //private void fillSupplier(int customer)
    //{
    //    try
    //    {
    //            DataTable suplierdata = q.GetSupNameId(customer);
    //            ddlSupllierWise.DataSource = suplierdata;
    //            ddlSupllierWise.DataTextField = "supplierNameAndID";
    //            ddlSupllierWise.DataValueField = "supplier_id";
    //            ddlSupllierWise.DataBind();
    //            ddlSupllierWise.Items.Insert(0, "All");

    //    }
    //    catch (Exception ex)
    //    {
    //        g.ShowMessage(this.Page, ex.Message);
    //    }
    //}

    //private void fillPart(int customer)
    //{
    //    try
    //    {
    //            DataTable partdata = q.GetPartIdname(customer);
    //            ddlPartWise.DataSource = partdata;
    //            ddlPartWise.DataTextField = "part_name";
    //            ddlPartWise.DataValueField = "part_id";
    //            ddlPartWise.DataBind();
    //            ddlPartWise.Items.Insert(0, "All");

    //    }
    //    catch (Exception ex)
    //    {
    //        g.ShowMessage(this.Page, ex.Message);
    //    }
    //}
    protected void btnPrintGaugeDetailsReport_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string gaugeId = lnk.CommandArgument;
            string customerId = lnk.CommandName;

            if (String.IsNullOrEmpty(gaugeId) || String.IsNullOrEmpty(customerId))
            {
                return;
            }
            else
            {
                //Response.Redirect("~/GaugeDetailsReportViewer.aspx?gaugeId=" + gaugeId + ","+ customerId);'"+~/+"'
                //string st = ConfigurationManager.AppSettings["ReportViewerURL"].ToString();
                //st = st + "GaugeDetailsReportViewer.aspx?gaugeId=" + gaugeId + "," + customerId;
                //string ss = "window.open('" + st + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
                string ss = "window.open('GaugeDetailsReportViewer.aspx?gaugeId=" + gaugeId + "," + customerId + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
                string script = "<script language='javascript'>" + ss + "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnExportToExcelGauge_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }
    protected void ExportToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=GaugeDetailReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            grdGaugeDetailsReport.AllowPaging = false;
            grdGaugeDetailsReport.Columns[9].Visible = false;
            this.bindGaugeDetailsReport();

            grdGaugeDetailsReport.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in grdGaugeDetailsReport.HeaderRow.Cells)
            {
                cell.BackColor = grdGaugeDetailsReport.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in grdGaugeDetailsReport.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = grdGaugeDetailsReport.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = grdGaugeDetailsReport.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            grdGaugeDetailsReport.RenderControl(hw);
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