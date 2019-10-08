using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MSASchedule : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    int supplierlinkId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["dleteCalibrationSchLink"] = "NO";
                MultiView1.ActiveViewIndex = 0;
                fillSupplier(Convert.ToInt32(Session["Customer_ID"]));
                bindCalibrationSchGrd(Convert.ToInt32(Session["Customer_ID"]));
                btnAddCalibSchedule.Focus();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void bindCalibrationSchGrd(int custId)
    {
        try
        {
          
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spMSAScheduleDetails";

            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 1, custId, 0, "", "", "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
            {
                try
                {
                    int gaugeId = Convert.ToInt32(searchValue);
                    DataSet ds = q.ProcdureWith8Param(stprocedure, 2, custId, gaugeId, "", "", "", "", "");
                    dt = ds.Tables[0];
                }
                catch (Exception ex)
                {

                    g.ShowMessage(this.Page, "Gauge Id is accept only numeric value. " + ex.Message);
                }

            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
            {

                DataSet ds = q.ProcdureWith8Param(stprocedure, 3, custId, 0, searchValue, "", "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 4, custId, 0, "", searchValue, "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 5, custId, 0, "", "", searchValue, "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 6, custId, 0, "", "", "", searchValue, "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Frequency Type-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 7, custId, 0, "", "", "", "", searchValue);
                dt = ds.Tables[0];
            }
           
            grdCalibrationSchedule.DataSource =dt;
            grdCalibrationSchedule.DataBind();
          

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
            childId = g.GetChildId("MSASchedule.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddCalibSchedule.Visible = true;
                }
                else
                {
                    btnAddCalibSchedule.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCalibrationSchedule.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibrationSchedule.Rows[i].FindControl("btnEditCalibSchedule");
                        LinkButton lnkdlet = (LinkButton)grdCalibrationSchedule.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = true;
                        lnk.Enabled = true;
                        Session["dleteCalibrationSchLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdCalibrationSchedule.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibrationSchedule.Rows[i].FindControl("btnEditCalibSchedule");
                        lnk.Enabled = false;
                        LinkButton lnkdlet = (LinkButton)grdCalibrationSchedule.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = false;
                        Session["dleteCalibrationSchLink"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void fillSupplier(int custId)
    {
        try
        {
            DataTable dtsup = q.GetSupNameId(custId);            
            ddlCalibratedBy.DataSource = dtsup;
            ddlCalibratedBy.DataTextField = "supplierNameAndID";
            ddlCalibratedBy.DataValueField = "supplier_id";
            ddlCalibratedBy.DataBind();
            ddlCalibratedBy.Items.Insert(0, "--Select--");
            ddlLastCalibratedBy.DataSource = dtsup;
            ddlLastCalibratedBy.DataTextField = "supplierNameAndID";
            ddlLastCalibratedBy.DataValueField = "supplier_id";
            ddlLastCalibratedBy.DataBind();
            ddlLastCalibratedBy.Items.Insert(0, "--Select--");


        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void fillGauge(int custId)
    {
        try
        {

            DataTable dtgauge = g.ReturnData("SELECT t0.gauge_id,  CONCAT(t1.gauge_name , ': ID-',t0.gauge_id,': Sr No-',t1.gauge_sr_no) as gaugeNameAndID FROM  gauge_supplier_link_TB AS t0 INNER JOIN gaugeMaster_TB AS t1 ON t0.gauge_id= t1.gauge_id WHERE t0.gauge_id NOT IN ( SELECT t2.gauge_id  FROM msa_schedule_TB AS t2 WHERE t2.gauge_id = t0.gauge_id ) AND (t0.customer_id = " + custId + ") AND (t0.link_status = 'ISSUED') AND (t0.status =True)");
            ddlGaugeID.DataSource = dtgauge;
            ddlGaugeID.DataTextField = "gaugeNameAndID";
            ddlGaugeID.DataValueField = "gauge_id";
            ddlGaugeID.DataBind();
            ddlGaugeID.Items.Insert(0, "--Select--");
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddCalibSchedule_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        ddlGaugeID.Focus();
        fillGauge(Convert.ToInt32(Session["Customer_ID"]));

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(msa_schedule_id) from msa_schedule_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtCalibScheduleId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {

            txtCalibScheduleId.Text = "1";
        }

    }
    protected void btnSaveCalibSchedule_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {

            if (btnSaveCalibSchedule.Text == "Save")
            {
                DataTable dtexist = g.ReturnData("Select gauge_id from msa_schedule_TB where gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + " and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and status=True");

                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "MSA scheduled is already exist for this gauge.");
                    return;
                }
                else
                {
                    saveUpdateCalibrationSch(0);
                }
            }
            else
            {
                int editId = Convert.ToInt32(lblsheduleID.Text);
                DataTable dtexist = g.ReturnData("Select gauge_id from msa_schedule_TB where gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + " and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and msa_schedule_id=" + editId + "");

                if (dtexist.Rows.Count > 0)
                {
                    saveUpdateCalibrationSch(editId);
                }
                else
                {
                    DataTable dtexist1 = g.ReturnData("Select gauge_id from msa_schedule_TB where gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + " and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and msa_schedule_id<>" + editId + "");

                    if (dtexist1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "MSA scheduled is already exist for this gauge.");
                        return;
                    }
                    else
                    {
                        saveUpdateCalibrationSch(editId);
                    }
                }

            }

            clearFields();

            bindCalibrationSchGrd(Convert.ToInt32(Session["Customer_ID"]));
            MultiView1.ActiveViewIndex = 0;
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void saveUpdateCalibrationSch(int editSchID)
    {
        try
        {
            DateTime lastCalibdate = DateTime.ParseExact(txtLastCalibrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strlastCalibdate = lastCalibdate.ToString("yyyy-MM-dd H:mm:ss");
            DateTime nextDueDate = DateTime.ParseExact(txtNextDueDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strNextdueDtae = nextDueDate.ToString("yyyy-MM-dd H:mm:ss");
            if (btnSaveCalibSchedule.Text == "Save")
            {
                DataTable dtsave = g.ReturnData("Insert into msa_schedule_TB (status,customer_id,gauge_id,calibrate_id,calibration_frequency, calibration_hours,frequency_type,intial_time_used,last_calibrated_by,last_calibration_date,next_due_date,created_by_id, bias,linearity,stability,R_and_R) values(True," + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(ddlGaugeID.SelectedValue) + ", " + Convert.ToInt32(ddlCalibratedBy.SelectedValue) + ",'" + txtCalibrationFrequency.Text + "','" + txtCalibrationHours.Text + "', '" + ddlFrequencyType.SelectedItem.Text + "','" + txtInitialTimeUsed.Text + "'," + Convert.ToInt32(ddlLastCalibratedBy.SelectedValue) + ",'" + strlastCalibdate + "','" + strNextdueDtae + "'," + Convert.ToInt32(Session["User_ID"]) + ",'" + txtBias.Text + "','" + txtLinearity.Text + "','" + txtSatability.Text + "','" + txtRR.Text + "')");
                g.ShowMessage(this.Page, "MSA schedule is saved successfully.");
            }
            else
            {
                DataTable dtupdate = g.ReturnData("Update msa_schedule_TB set status=True,customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ",gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + ", calibrate_id=" + Convert.ToInt32(ddlCalibratedBy.SelectedValue) + ",calibration_frequency='" + txtCalibrationFrequency.Text + "',calibration_hours='" + txtCalibrationHours.Text + "', frequency_type='" + ddlFrequencyType.SelectedItem.Text + "',intial_time_used='" + txtInitialTimeUsed.Text + "',last_calibrated_by=" + Convert.ToInt32(ddlLastCalibratedBy.SelectedValue) + ",last_calibration_date='" + strlastCalibdate + "',next_due_date='" + strNextdueDtae + "',created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",bias='" + txtBias.Text + "',linearity='" + txtLinearity.Text + "',stability='" + txtSatability.Text + "',R_and_R='" + txtRR.Text + "'  where msa_schedule_id=" + editSchID + "");
                g.ShowMessage(this.Page, "MSA schedule is updated successfully.");

            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void updateGaugeCycle(int? gaugeID)
    {
        try
        {
            DataTable dtgauge = g.ReturnData("Select cycles from gaugeMaster_TB where gauge_id=" + gaugeID + "");
            string stcycle = "";

            if (lblInitialValue.Text == "")
            {
                if (dtgauge.Rows[0]["cycles"].ToString() == "0")
                {
                    stcycle = txtInitialTimeUsed.Text;
                }
                else
                {
                    string getInitialValu = dtgauge.Rows[0]["cycles"].ToString();
                    int cycleValue = Convert.ToInt32(getInitialValu);
                    string nextValue = txtInitialTimeUsed.Text;
                    int nextCycleValue = Convert.ToInt32(nextValue);
                    int totalValue = cycleValue + nextCycleValue;
                    stcycle = totalValue.ToString();
                }
            }
            else
            {
                int updatedcycleValue = Convert.ToInt32(txtInitialTimeUsed.Text);
                int beforeValue = Convert.ToInt32(lblInitialValue.Text);
                if (beforeValue == updatedcycleValue)
                {
                    return;
                }
                else if (beforeValue > updatedcycleValue)
                {
                    int ext = beforeValue - updatedcycleValue;
                    int cycleext = Convert.ToInt32(dtgauge.Rows[0]["cycles"].ToString());
                    int total = cycleext - ext;
                    stcycle = total.ToString();
                }
                else
                {
                    int ext1 = updatedcycleValue - beforeValue;
                    int cycleext = Convert.ToInt32(dtgauge.Rows[0]["cycles"].ToString());
                    int total = cycleext + ext1;
                    stcycle = total.ToString();
                }
            }
            DataTable dtupd = g.ReturnData("Update gaugeMaster_TB set cycles='" + stcycle + "' where gauge_id=" + gaugeID + "");


        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnClloseCalibSchedule_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }
    private void clearFields()
    {

        txtCalibScheduleId.Text = txtInitialTimeUsed.Text =
        txtLastCalibrationDate.Text = txtNextDueDate.Text = txtCalibrationHours.Text =
        txtCalibrationFrequency.Text = txtBias.Text = txtLinearity.Text = txtRR.Text =
        txtSatability.Text = string.Empty;
        ddlGaugeID.SelectedIndex = 0;
        ddlCalibratedBy.SelectedIndex = 0;
        ddlLastCalibratedBy.SelectedIndex = 0;
        ddlFrequencyType.SelectedIndex = 0;
        btnSaveCalibSchedule.Text = "Save";
        lblInitialValue.Text = "";
        btnAddCalibSchedule.Focus();
    }
    protected void ddlGaugeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlGaugeID.SelectedIndex > 0)
            {
                DataTable dtsupid = g.ReturnData("Select supplier_id from gauge_supplier_link_TB where gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + "");
                supplierlinkId = Convert.ToInt32(dtsupid.Rows[0]["supplier_id"].ToString());

                ddlLastCalibratedBy.SelectedValue = supplierlinkId.ToString();
                ddlCalibratedBy.Focus();

            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void txtLastCalibrationDate_TextChanged(object sender, EventArgs e)
    {
        txtBias.Focus();
        fillScheduleDate();
    }
    private void fillScheduleDate()
    {
        try
        {
            if (String.IsNullOrEmpty(txtLastCalibrationDate.Text))
            {
               
                txtNextDueDate.Text = string.Empty;
                return;
            }
            int fereq = 0;
            string strdate = txtLastCalibrationDate.Text;
            DateTime dt = DateTime.ParseExact(strdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime revDate = dt;
            if (String.IsNullOrEmpty(txtCalibrationFrequency.Text))
            {
                g.ShowMessage(this.Page, "Enter the value of msa frequency.");
                txtNextDueDate.Text = string.Empty;
                return;
            }
            else
            {
                fereq = Convert.ToInt32(txtCalibrationFrequency.Text);
            }
            if (ddlFrequencyType.SelectedIndex > 0)
            {
                if (ddlFrequencyType.SelectedIndex == 1)
                {
                    DateTime nextDuedate = revDate.AddMonths(fereq);
                    txtNextDueDate.Text = nextDuedate.ToString("dd/MM/yyyy");
                  
                }
                else
                {
                    DateTime nextDuedateYear = revDate.AddYears(fereq);
                    txtNextDueDate.Text = nextDuedateYear.ToString("dd/MM/yyyy");
                    
                }
            }
            else
            {
                g.ShowMessage(this.Page, "Select frequency type.");
                txtNextDueDate.Text = string.Empty;
                return;
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void ddlFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFrequencyType.Focus();
        fillScheduleDate();
       
        txtCalibrationFrequency.Focus();
    }
    protected void txtCalibrationFrequency_TextChanged(object sender, EventArgs e)
    {
        txtLastCalibrationDate.Focus();
        if (String.IsNullOrEmpty(txtCalibrationFrequency.Text))
        {
            txtBias.Text = txtLinearity.Text = txtSatability.Text = string.Empty;
        }
        else
        {
            int freq = 0;
            freq = Convert.ToInt32(txtCalibrationFrequency.Text);
            txtBias.Text = txtLinearity.Text = txtSatability.Text = txtRR.Text = Convert.ToString(freq);
        }
        fillScheduleDate();
    }

    private void fillGaugeOnEdit(int gaugeId)
    {
        try
        {
            //Need for fetch gauge on edit time. 17/09/2017
            DataTable dtgauge = g.ReturnData("Select gauge_id,concat_WS(': ID-',gauge_name , gauge_id) as gaugeNameAndID  from gaugeMaster_TB where gauge_id=" + gaugeId + "");
            ddlGaugeID.DataSource = dtgauge;
            ddlGaugeID.DataTextField = "gaugeNameAndID";
            ddlGaugeID.DataValueField = "gauge_id";
            ddlGaugeID.DataBind();
            ddlGaugeID.Items.Insert(0, "--Select--");


        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnEditCalibSchedule_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblsheduleID.Text = lnk.CommandArgument;
            DataTable dtedit = g.ReturnData("Select msa_schedule_id,calibration_frequency,calibration_hours,intial_time_used,last_calibration_date,next_due_date,calibrate_id,last_calibrated_by,gauge_id,bias,linearity,stability,R_and_R,frequency_type from msa_schedule_TB where msa_schedule_id=" + Convert.ToInt32(lblsheduleID.Text) + "");

            txtCalibScheduleId.Text = dtedit.Rows[0]["msa_schedule_id"].ToString();
            txtCalibrationFrequency.Text = dtedit.Rows[0]["calibration_frequency"].ToString();
            txtCalibrationHours.Text = dtedit.Rows[0]["calibration_hours"].ToString();
            txtInitialTimeUsed.Text = dtedit.Rows[0]["intial_time_used"].ToString();
            lblInitialValue.Text = dtedit.Rows[0]["intial_time_used"].ToString();
            DateTime dtLastcalidate = Convert.ToDateTime(dtedit.Rows[0]["last_calibration_date"].ToString());
            txtLastCalibrationDate.Text = dtLastcalidate.ToString("dd/MM/yyyy");
            DateTime nextDueDate = Convert.ToDateTime(dtedit.Rows[0]["next_due_date"].ToString());
            txtNextDueDate.Text = nextDueDate.ToString("dd/MM/yyyy");

            ddlCalibratedBy.SelectedValue = dtedit.Rows[0]["calibrate_id"].ToString();
            ddlLastCalibratedBy.SelectedValue = dtedit.Rows[0]["last_calibrated_by"].ToString();
            fillGaugeOnEdit(Convert.ToInt32(dtedit.Rows[0]["gauge_id"].ToString()));
            ddlGaugeID.SelectedValue = dtedit.Rows[0]["gauge_id"].ToString();
            txtBias.Text = dtedit.Rows[0]["bias"].ToString();
            txtLinearity.Text = dtedit.Rows[0]["linearity"].ToString();
            txtSatability.Text = dtedit.Rows[0]["stability"].ToString();
            txtRR.Text = dtedit.Rows[0]["R_and_R"].ToString();
            if (dtedit.Rows[0]["frequency_type"].ToString() == "MONTH")
            {
                ddlFrequencyType.SelectedIndex = 1;
            }
            else if (dtedit.Rows[0]["frequency_type"].ToString() == "YEAR")
            {
                ddlFrequencyType.SelectedIndex = 2;
            }
            else
            {
                ddlFrequencyType.SelectedIndex = 0;
            }

            MultiView1.ActiveViewIndex = 1;
            btnSaveCalibSchedule.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        lblsheduleID.Text = lnk.CommandArgument;
        string confirmValue = Request.Form["confirm_value"];
        if (confirmValue == "Yes")
        {
            try
            {

                DataTable dtupd = g.ReturnData("Update msa_schedule_TB set status=False where msa_schedule_id=" + Convert.ToInt32(lblsheduleID.Text) + "");

                g.ShowMessage(this.Page, "MSA scheduled is deleted successfully.");

                bindCalibrationSchGrd(Convert.ToInt32(Session["Customer_ID"]));
            }
            catch (Exception ex)
            {
                g.ShowMessage(this.Page, ex.Message);

            }

        }
    }
    protected void grdCalibrationSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCalibrationSchedule.PageIndex = e.NewPageIndex;
        bindCalibrationSchGrd(Convert.ToInt32(Session["Customer_ID"]));
    }
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        searchBy.Text = "";
        txtsearchValue.Text = "";
        if (ddlsortby.SelectedIndex > 0)
        {
            btnSearch.Visible = true;
            lblName.Visible = true;


            txtsearchValue.Text = "";

            divtxtsearch.Visible = true;

        }
        else
        {
            btnSearch.Visible = false;

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
        else if (ddlsortby.SelectedItem.Text == "Frequency Type-Wise")
        {
            lblName.Text = "Frequency Type";

        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindCalibrationSchGrd(Convert.ToInt32(Session["Customer_ID"]));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindCalibrationSchGrd(Convert.ToInt32(Session["Customer_ID"]));


        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}