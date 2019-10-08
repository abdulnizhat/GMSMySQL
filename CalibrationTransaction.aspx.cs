using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CalibrationTransaction : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    Byte[] imgByte = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {

            if (!Page.IsPostBack)
            {

                Session["SupplierId"] = "0";
                int SupplierId = g.CheckIsSupplier(Convert.ToInt32(Session["User_ID"]));
                if (SupplierId != 0)
                {
                    Session["SupplierId"] = SupplierId.ToString();
                }
                Session["dleteCalibrationTranLink"] = "NO";
                MultiView1.ActiveViewIndex = 0;
                fillCalibrationScheduleID(Convert.ToInt32(Session["Customer_ID"]));
                bindCalibrationTranscGrd(Convert.ToInt32(Session["Customer_ID"]));
                displayDueStatus();
                btnAddCalibTransaction.Focus();
                if (Request.QueryString["TransactionId"] != null)
                {

                    string getquerystring = Request.QueryString["TransactionId"].ToString();
                    string[] getquerystring1 = getquerystring.Split(',');
                    string getIds = getquerystring1[0].ToString();
                    int trasactionId = Convert.ToInt32(getIds);
                    lblcabTranId.Text = trasactionId.ToString();
                    getValueForEdit(trasactionId);
                }



            }

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void fillCalibrationScheduleID(int custId)
    {
        try
        {

            if (Convert.ToInt32(Session["SupplierId"]) == 0)
            {

                DataTable dtCalibSchedule = g.ReturnData("SELECT t0.gauge_id,concat_WS(': ID- ', t0.gauge_name, CAST(t0.gauge_id as CHAR(20))) AS gauge_name FROM gaugeMaster_TB AS t0 INNER JOIN calibration_schedule_TB AS t1 ON t0.gauge_id = t1.gauge_id WHERE t1.customer_id = " + custId + " AND (t1.status = True)");
                ddlScheduleID.DataSource = dtCalibSchedule;
                ddlScheduleID.DataTextField = "gauge_name";
                ddlScheduleID.DataValueField = "gauge_id";
                ddlScheduleID.DataBind();
                ddlScheduleID.Items.Insert(0, "--Select--");

            }
            else if (Convert.ToInt32(Session["SupplierId"]) != 0)
            {
                DataTable dtfetchCalibScheduledId = g.ReturnData("SELECT t0.gauge_id, concat_WS(': ID- ', t0.gauge_name, CAST(t0.gauge_id as CHAR(20))) AS gauge_name FROM gaugeMaster_TB AS t0 INNER JOIN calibration_schedule_TB AS t1 ON t0.gauge_id = t1.gauge_id INNER JOIN gauge_supplier_link_TB AS t2 ON t0.gauge_id = t2.gauge_id WHERE (t1.customer_id = " + custId + ") AND (t1.status = True) AND (t2.supplier_id = " + Convert.ToInt32(Session["SupplierId"]) + ") ");
                ddlScheduleID.DataSource = dtfetchCalibScheduledId;
                ddlScheduleID.DataTextField = "gauge_name";
                ddlScheduleID.DataValueField = "gauge_id";
                ddlScheduleID.DataBind();
                ddlScheduleID.Items.Insert(0, "--Select--");

            }


        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    private void bindCalibrationTranscGrd(int custId)
    {
        try
        {
            string SqlQuery = "";
            if (Convert.ToInt32(Session["SupplierId"]) == 0)
            {
                string stprocedure = "spCalibTransactionDt";
                DataSet ds = q.ProcdureWith3Param(stprocedure, 1, custId, 0);
                grdCalibTran.DataSource = ds.Tables[0];

                grdCalibTran.DataBind();

            }
            else if (Convert.ToInt32(Session["SupplierId"]) != 0)
            {
                SqlQuery = @"Select cb.calibration_transaction_id, cb.calibration_schedule_id, cb.calibration_date,
                cb.calibration_cost, cb.calibration_hours,cb.calibration_status,
                cb.certification_no,cb.gauge_id,gt.gauge_name,gt.gauge_sr_no, gt.gauge_Manufature_Id, cb.customer_id, ct.customer_name,
                cb.humidity,cb.other,cb.pressure,cb.temprature,cb.tollerance_go, cb.remarks,
                cb.tollerance_no_go, Case when(cs.frequency_type='MONTH') then DATE_ADD(cb.calibration_date,Interval cs.calibration_frequency MONTH )
                else DATE_ADD(cb.calibration_date,Interval cs.calibration_frequency YEAR ) End AS next_due_date 
                 from calibration_transaction_TB as cb
                 Left Outer join calibration_schedule_TB as cs
                ON cb.calibration_schedule_id=cs.calibration_schedule_id
                Left Outer join gaugeMaster_TB as gt
                ON cb.gauge_id=gt.gauge_id
                Left Outer join customer_TB as ct
                ON cb.customer_id=ct.customer_id
                Left outer Join gauge_supplier_link_TB as gs ON cb.gauge_id=gs.gauge_id
                where cb.customer_id=" + custId + " " +
                    " and (cb.calibration_status='PASSED' or cb.calibration_status='FAILED' or cb.calibration_status='REPAIR') " +
                    " And cb.calibration_schedule_Id=(Select Max(tt.calibration_schedule_Id)  from calibration_transaction_TB as tt where tt.gauge_id=cb.gauge_Id) and gs.supplier_id=" + Convert.ToInt32(Session["SupplierId"]) + " " +
                    " and (cb.is_approved='False' OR cb.is_approved IS NULL) " +
                    " Order by cb.calibration_transaction_id DESC ";
                grdCalibTran.DataSource = g.ReturnData(SqlQuery);
                grdCalibTran.DataBind();
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
            childId = g.GetChildId("CalibrationTransaction.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddCalibTransaction.Visible = true;
                }
                else
                {
                    btnAddCalibTransaction.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCalibTran.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibTran.Rows[i].FindControl("btnEditCalibTran");
                        lnk.Enabled = true;
                        LinkButton lnk1 = (LinkButton)grdCalibTran.Rows[i].FindControl("LnkDownLoadDocument");
                        lnk1.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdCalibTran.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibTran.Rows[i].FindControl("btnEditCalibTran");
                        lnk.Enabled = false;
                        LinkButton lnk1 = (LinkButton)grdCalibTran.Rows[i].FindControl("LnkDownLoadDocument");
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
    protected void btnAddCalibTransaction_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        ddlScheduleID.Focus();
        getNewTransactionId();
    }
    private void getNewTransactionId()
    {

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(calibration_transaction_id) from calibration_transaction_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());


            txtCalibrationID.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {
            txtCalibrationID.Text = "1";
        }

    }
    protected void ddlScheduleID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlScheduleID.Focus();
        if (ddlScheduleID.SelectedIndex > 0)
        {
            DataTable dtcalibscheduleid = g.ReturnData("Select customer_id,calibration_schedule_id from calibration_schedule_TB where gauge_id=" + Convert.ToInt32(ddlScheduleID.SelectedValue) + " and status=True");

            fillGaugeDetails(Convert.ToInt32(dtcalibscheduleid.Rows[0]["calibration_schedule_id"].ToString()), Convert.ToInt32(dtcalibscheduleid.Rows[0]["customer_id"].ToString()));
            divDisplayData1.Visible = true;
            divDisplayData2.Visible = true;

        }
        else
        {
            clearFields();
            divDisplayData1.Visible = false;
            divDisplayData2.Visible = false;
        }
    }
    private void fillGaugeDetails(int scheduleId, int custid)
    {
        try
        {
            divDisplayData1.Visible = true;
            divDisplayData2.Visible = true;
            string stprocedure = "spFetchGaugDtOnCalibTransction";
            DataSet ds = q.ProcdureWith3Param(stprocedure, 1, custid, scheduleId);
            DataTable dtgauge = ds.Tables[0];

            txtschduleId.Text = dtgauge.Rows[0]["calibration_schedule_id"].ToString();
            txtGaugeId.Text = dtgauge.Rows[0]["gauge_id"].ToString();
            txtGaugeName.Text = dtgauge.Rows[0]["gauge_name"].ToString();
            txtManufactureID.Text = dtgauge.Rows[0]["gauge_Manufature_Id"].ToString();
            txtType.Text = dtgauge.Rows[0]["gauge_type"].ToString();
            if (dtgauge.Rows[0]["gauge_type"].ToString() == "ATTRIBUTE")
            {
                divSize.Visible = true;
                divRange.Visible = false;
                divTolGo.Visible = true;
                divTolNoGo.Visible = true;
                divPerError1.Visible = false;
                divPerError2.Visible = false;
                divPermissableError1.Visible = false;
                divPermissableError2.Visible = false;
                divNoGoPlusminus.Visible = true;
                divGoWearLimit.Visible = true;
                divGoTolPulus.Visible = true;
                divLeastCount.Visible = false;
                divResolution.Visible = false;

            }
            else
            {
                divSize.Visible = false;
                divRange.Visible = true;
                divTolGo.Visible = false;
                divTolNoGo.Visible = false;
                divPerError1.Visible = true;
                divPerError2.Visible = true;
                divPermissableError1.Visible = true;
                divPermissableError2.Visible = true;
                divNoGoPlusminus.Visible = false;
                divGoWearLimit.Visible = false;
                divGoTolPulus.Visible = false;
                divLeastCount.Visible = true;
                divResolution.Visible = true;
            }
            txtSize.Text = dtgauge.Rows[0]["size_range"].ToString();
            txtRange.Text = dtgauge.Rows[0]["size_range"].ToString();
            txtGoTolleranceMinus.Text = dtgauge.Rows[0]["go_tollerance_minus"].ToString();
            txtGoTollerancePlus.Text = dtgauge.Rows[0]["go_tollerance_plus"].ToString();
            txtNoGoTolleranceMinus.Text = dtgauge.Rows[0]["no_go_tollerance_minus"].ToString();
            txtNoGoTollerancePlus.Text = dtgauge.Rows[0]["no_go_tollerance_plus"].ToString();
            txtGoWereLimit.Text = dtgauge.Rows[0]["go_were_limit"].ToString();
            txtLeastCount.Text = dtgauge.Rows[0]["least_count"].ToString();
            txtPurchaseCost.Text = dtgauge.Rows[0]["purchase_cost"].ToString();
            txtCurrentLocation.Text = dtgauge.Rows[0]["current_location"].ToString();
            txtStoreLocation.Text = dtgauge.Rows[0]["store_location"].ToString();
            // DateTime purchaseDate = Convert.ToDateTime(dtgauge.Rows[0]["Purchasedate"].ToString());
            txtPurchaseDate.Text = dtgauge.Rows[0]["Purchasedate"].ToString();
            //  DateTime serviceDate = Convert.ToDateTime(dtgauge.Rows[0]["ServiceDate"].ToString());
            txtServiceDate.Text = dtgauge.Rows[0]["ServiceDate"].ToString();
            // DateTime retairementDate = Convert.ToDateTime(dtgauge.Rows[0]["RetairmentDate"].ToString());
            txtRetairementDate.Text = dtgauge.Rows[0]["RetairmentDate"].ToString();
            txtInitialTimeUsed.Text = dtgauge.Rows[0]["cycles"].ToString();
            txtResolution.Text = dtgauge.Rows[0]["resolution"].ToString();
            txtLastCalibratedBy.Text = dtgauge.Rows[0]["LasCalibratedBy"].ToString();
            txtCalibrattedBy.Text = dtgauge.Rows[0]["CalibratedBy"].ToString();
            txtFrequencyType.Text = dtgauge.Rows[0]["frequency_type"].ToString();
            txtCalibrationFrequency.Text = dtgauge.Rows[0]["calibration_frequency"].ToString();
            txtCalibrationHours.Text = dtgauge.Rows[0]["calibration_hours"].ToString();
            //  DateTime lastcalibDate = Convert.ToDateTime(dtgauge.Rows[0]["LastCalibrationDate"].ToString());
            txtLastCalibrationDate.Text = dtgauge.Rows[0]["LastCalibrationDate"].ToString();
            // DateTime nextcalibDate = Convert.ToDateTime(dtgauge.Rows[0]["NextDueDate"].ToString());
            txtNextDueDate.Text = dtgauge.Rows[0]["NextDueDate"].ToString();
            // DateTime projectedcalibDate = Convert.ToDateTime(dtgauge.Rows[0]["ProjectedSchDate"].ToString());
            txtProjectedCalibrationSchedule.Text = dtgauge.Rows[0]["ProjectedSchDate"].ToString();
            txtPermisableError1.Text = dtgauge.Rows[0]["permisable_error1"].ToString();
            txtPermisableError2.Text = dtgauge.Rows[0]["permisable_error2"].ToString();

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveCalibTransaction_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {
            #region File Upload
            FileUpload img = (FileUpload)CertificationUpload;
            if (img.HasFile && img.PostedFile != null)
            {
                imgByte = null;
                //To create a PostedFile
                HttpPostedFile File = CertificationUpload.PostedFile;
                txtImageName.Text = File.FileName;
                string filePath = CertificationUpload.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                long fileSize = CertificationUpload.FileContent.Length;
                if (fileSize <= 1024000) // Check in byte 1024000 Byte =1.024 MB . 1 MB.
                {
                    string contenttype = String.Empty;
                    switch (ext)
                    {
                        case ".jpeg":
                            contenttype = "image/jpeg";
                            break;
                        case ".jpg":
                            contenttype = "image/jpg";
                            break;
                        case ".png":
                            contenttype = "image/png";
                            break;
                        case ".gif":
                            contenttype = "image/gif";
                            break;
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                        case ".bmp":
                            contenttype = "image/bmp";
                            break;
                    }
                    if (contenttype != String.Empty)
                    {
                        Stream stream = File.InputStream;
                        BinaryReader bReader = new BinaryReader(stream);
                        imgByte = bReader.ReadBytes((int)stream.Length);
                        //Create byte Array with file len
                        // imgByte = new Byte[File.ContentLength];
                        //force the control to load data in array
                        //  File.InputStream.Read(imgByte, 0, File.ContentLength);
                    }
                    else
                    {
                        g.ShowMessage(this.Page, "File type is not valid.");
                        return;
                    }
                }
                else
                {
                    g.ShowMessage(this.Page, "Selected file size is exceeds the size limit 1 MB only.");
                    return;
                }

            }
            #endregion

            if (btnSaveCalibTransaction.Text == "Save")
            {
                DataTable dtcheck = g.ReturnData("Select calibration_schedule_id from calibration_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True");

                if (dtcheck.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Calibration transaction is already exist.");
                    return;
                }
                else
                {
                    saveUpdateCalibTran(0);
                }

            }
            else
            {
                int editId = Convert.ToInt32(lblcabTranId.Text);
                DataTable dtcheck = g.ReturnData("Select calibration_schedule_id from calibration_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and calibration_transaction_id=" + editId + "");

                if (dtcheck.Rows.Count > 0)
                {
                    saveUpdateCalibTran(editId);
                }
                else
                {
                    DataTable dtcheck1 = g.ReturnData("Select calibration_schedule_id from calibration_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and calibration_transaction_id<>" + editId + "");

                    if (dtcheck1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Calibration transaction is already exist.");
                        return;
                    }
                    else
                    {
                        saveUpdateCalibTran(editId);
                    }
                }

            }

            clearFields();
            bindCalibrationTranscGrd(Convert.ToInt32(Session["Customer_ID"]));
            displayDueStatus();
            MultiView1.ActiveViewIndex = 0;
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void clearFields()
    {
        txtCalibrationID.Text = txtCalibCost.Text = txtCalibrationDate.Text =
        lblcabTranId.Text = txtCalibHours.Text = txtCertificationNo.Text =
        txtGaugeId.Text = txtHumidity.Text = txtOther.Text = txtPressure.Text =
        txtTemprature.Text = txtTolleranceGo.Text = txtTolleranceNoGo.Text =
        txtSize.Text = txtRange.Text = txtGoTolleranceMinus.Text =
        txtGoTollerancePlus.Text = txtNoGoTolleranceMinus.Text =
        txtNoGoTollerancePlus.Text = txtGoWereLimit.Text = txtLeastCount.Text =
        txtCurrentLocation.Text = txtStoreLocation.Text =
        txtPurchaseDate.Text = txtServiceDate.Text = txtRetairementDate.Text =
        txtInitialTimeUsed.Text = txtResolution.Text = txtLastCalibratedBy.Text =
        txtCalibrattedBy.Text = txtFrequencyType.Text = txtCalibrationFrequency.Text =
        txtCalibrationHours.Text = txtLastCalibrationDate.Text = txtNextDueDate.Text =
        txtProjectedCalibrationSchedule.Text = txtPermisableError1.Text =
        txtPermisableError2.Text = txtType.Text = txtGaugeName.Text = txtschduleId.Text =
        txtManufactureID.Text = txtPurchaseCost.Text = txtPermisableError1Entry.Text = txtPermisableError2Entry.Text =
        txtImageName.Text = "";
        ddlScheduleID.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        btnSaveCalibTransaction.Text = "Save";
        btnAddCalibTransaction.Focus();
    }
    private void saveUpdateCalibTran(int editCaliTranId)
    {
        try
        {
            DateTime upddate = DateTime.Now;
            string stupdate = upddate.ToString("yyyy-MM-dd H:mm:ss");
            string sttolarancego = "";
            string sttolarancenogo = "";
            DateTime caliDate = DateTime.ParseExact(txtCalibrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strcaliDate = caliDate.ToString("yyyy-MM-dd H:mm:ss");
            if (txtType.Text == "ATTRIBUTE")
            {
                sttolarancego = txtTolleranceGo.Text;
                sttolarancenogo = txtTolleranceNoGo.Text;
            }
            else
            {
                // Insert into tollerance go column in DataBase due project is 95% completed so changes.
                sttolarancego = txtPermisableError1Entry.Text;
                sttolarancenogo = txtPermisableError2Entry.Text;
            }


            if (btnSaveCalibTransaction.Text == "Save")
            {

                string stquery = "Insert into calibration_transaction_TB (status,customer_id,calibration_cost, calibration_date,calibration_hours,calibration_schedule_id,calibration_status,certification_no,created_by_id,gauge_id,  humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_certification_upload)  values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18)";

                g.savetranwith19param(stquery, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(txtCalibCost.Text), strcaliDate, txtCalibHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtCertificationNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, sttolarancego, sttolarancenogo, txtImageName.Text, imgByte);

                ////save code for calibration_transaction_upd_tb
                //DataTable dtmaxid = g.ReturnData("Select Max(calibration_transaction_id) from  calibration_transaction_TB");
                //int maxId = Convert.ToInt32(dtmaxid.Rows[0][0].ToString());


                //string stquery1 = "Insert into calibration_transaction_upd_tb (status,customer_id,calibration_cost, calibration_date,calibration_hours,calibration_schedule_id,calibration_status,certification_no,created_by_id,gauge_id,  humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_certification_upload,calibration_transaction_id,updated_date)  values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20)";
                //g.savetranwith21param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(txtCalibCost.Text), strcaliDate, txtCalibHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtCertificationNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, sttolarancego, sttolarancenogo, txtImageName.Text, imgByte, maxId, stupdate);

               
                #region Re-Schedule This Gauge
                gaugeReschedule();
             

                #endregion
                g.ShowMessage(this.Page, "Calibration transaction is saved and calibration rescheduled.");
            }

            else
            {
                int editId = Convert.ToInt32(lblcabTranId.Text);
                DataTable dtcheck = g.ReturnData("Select is_approved,calibration_status,approved_by,remarks,image_name,calibration_certification_upload from calibration_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and calibration_transaction_id=" + editId + "");
                string ststatus = "";
                ststatus = dtcheck.Rows[0]["calibration_status"].ToString();
                string stfile = "";
                stfile = dtcheck.Rows[0]["calibration_certification_upload"].ToString();
                if (stfile != "")
                {
                    if (imgByte == null)
                    {
                        imgByte = (Byte[])dtcheck.Rows[0]["calibration_certification_upload"];
                    }
                }




                string stquery = "Update calibration_transaction_TB set status=?param1,customer_id=?param2,calibration_cost=?param3,calibration_date=?param4,  calibration_hours=?param5,calibration_schedule_id=?param6,calibration_status=?param7,certification_no=?param8,  created_by_id=?param9,gauge_id=?param10,humidity=?param11,other=?param12, pressure=?param13,temprature=?param14,tollerance_go=?param15,tollerance_no_go=?param16,image_name=?param17,calibration_certification_upload=?param18 where calibration_transaction_id = " + editCaliTranId + " ";

                g.savetranwith19param(stquery, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(txtCalibCost.Text), strcaliDate, txtCalibHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtCertificationNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, sttolarancego, sttolarancenogo, txtImageName.Text, imgByte);

                #region code for calibration transaction upd tb
                if (ststatus != ddlStatus.SelectedItem.Text)
                {
                    // save code

                    string stquery1 = "Insert into calibration_transaction_upd_tb (status,customer_id,calibration_cost, calibration_date,calibration_hours,calibration_schedule_id,calibration_status,certification_no,created_by_id,gauge_id,  humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_certification_upload,calibration_transaction_id,updated_date)  values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20)";
                    g.savetranwith21param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(txtCalibCost.Text), strcaliDate, txtCalibHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtCertificationNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, sttolarancego, sttolarancenogo, txtImageName.Text, imgByte, editCaliTranId, stupdate);
                }
                else
                {
                    //Update Code

                    try
                    {
                        DataTable dtmax = g.ReturnData("SELECT max(calibration_transaction_upd_id) FROM calibration_transaction_upd_tb where calibration_transaction_id=" + editCaliTranId + "");
                        int clibupdid = Convert.ToInt32(dtmax.Rows[0][0].ToString());
                        string stquery1 = "Update calibration_transaction_upd_tb set status=?param1,customer_id=?param2,calibration_cost=?param3,calibration_date=?param4,  calibration_hours=?param5,calibration_schedule_id=?param6,calibration_status=?param7,certification_no=?param8,  created_by_id=?param9,gauge_id=?param10,humidity=?param11,other=?param12, pressure=?param13,temprature=?param14,tollerance_go=?param15,tollerance_no_go=?param16,image_name=?param17,calibration_certification_upload=?param18,calibration_transaction_id=?param19,updated_date=?param20 where calibration_transaction_upd_id = " + clibupdid + " ";
                        g.savetranwith21param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(txtCalibCost.Text), strcaliDate, txtCalibHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtCertificationNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, sttolarancego, sttolarancenogo, txtImageName.Text, imgByte, editCaliTranId, stupdate);
                    }
                    catch (Exception)
                    {
                        string stquery1 = "Insert into calibration_transaction_upd_tb (status,customer_id,calibration_cost, calibration_date,calibration_hours,calibration_schedule_id,calibration_status,certification_no,created_by_id,gauge_id,  humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_certification_upload,calibration_transaction_id,updated_date)  values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20)";
                        g.savetranwith21param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(txtCalibCost.Text), strcaliDate, txtCalibHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtCertificationNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, sttolarancego, sttolarancenogo, txtImageName.Text, imgByte, editCaliTranId, stupdate);
                    }




                }
                #endregion
                //}
                if (Request.QueryString["TransactionId"] != null)
                {
                    string getquerystring = Request.QueryString["TransactionId"].ToString();
                    string[] getquerystring1 = getquerystring.Split(',');
                    string getpagename = getquerystring1[1].ToString();

                    if (getpagename == "RejHis")
                    {
                        gaugeReschedule();
                    }
                    else
                    {
                        #region Re-Schedule This Gauge
                        DataTable dtselect = g.ReturnData("Select status,customer_id,gauge_id,calibrate_id,calibration_frequency, calibration_hours,frequency_type,intial_time_used,last_calibrated_by from calibration_schedule_TB where gauge_id=" + Convert.ToInt32(txtGaugeId.Text) + " and status=True");
                        string stnextd = "";
                        string stprojectd = "";
                        int feq = Convert.ToInt32(dtselect.Rows[0]["calibration_frequency"].ToString());
                        DateTime dnextd = Convert.ToDateTime(caliDate);
                        DateTime dprod = Convert.ToDateTime(caliDate);
                        if (dtselect.Rows[0]["frequency_type"].ToString() == "YEAR")
                        {
                            dnextd = dnextd.AddYears(feq);
                            stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");
                            dprod = dprod.AddYears(feq + feq);
                            stprojectd = dprod.ToString("yyyy-MM-dd H:mm:ss");
                        }
                        else
                        {
                            dnextd = dnextd.AddMonths(feq);
                            stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");
                            dprod = dprod.AddMonths(feq + feq);
                            stprojectd = dprod.ToString("yyyy-MM-dd H:mm:ss");

                        }
                        DataTable dtupdschedule = g.ReturnData("Update calibration_schedule_TB set status=True,customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ",last_calibration_date='" + strcaliDate + "', next_due_date='" + stnextd + "',projected_calib_schedule='" + stprojectd + "',created_by_id = " + Convert.ToInt32(Session["User_ID"]) + " where gauge_id=" + Convert.ToInt32(txtGaugeId.Text) + " and status=True");


                        #endregion
                    }
                }
                else
                {
                    #region Re-Schedule This Gauge
                    DataTable dtselect = g.ReturnData("Select status,customer_id,gauge_id,calibrate_id,calibration_frequency, calibration_hours,frequency_type,intial_time_used,last_calibrated_by from calibration_schedule_TB where gauge_id=" + Convert.ToInt32(txtGaugeId.Text) + " and status=True");
                    string stnextd = "";
                    string stprojectd = "";
                    int feq = Convert.ToInt32(dtselect.Rows[0]["calibration_frequency"].ToString());
                    DateTime dnextd = Convert.ToDateTime(caliDate);
                    DateTime dprod = Convert.ToDateTime(caliDate);
                    if (dtselect.Rows[0]["frequency_type"].ToString() == "YEAR")
                    {
                        dnextd = dnextd.AddYears(feq);
                        stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");
                        dprod = dprod.AddYears(feq + feq);
                        stprojectd = dprod.ToString("yyyy-MM-dd H:mm:ss");
                    }
                    else
                    {
                        dnextd = dnextd.AddMonths(feq);
                        stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");
                        dprod = dprod.AddMonths(feq + feq);
                        stprojectd = dprod.ToString("yyyy-MM-dd H:mm:ss");

                    }
                    DataTable dtupdschedule = g.ReturnData("Update calibration_schedule_TB set status=True,customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ",last_calibration_date='" + strcaliDate + "', next_due_date='" + stnextd + "',projected_calib_schedule='" + stprojectd + "',created_by_id = " + Convert.ToInt32(Session["User_ID"]) + " where gauge_id=" + Convert.ToInt32(txtGaugeId.Text) + " and status=True");


                    #endregion
                }

              
                g.ShowMessage(this.Page, "Calibration transaction and calibration reschedule are updated successfully.");
                if (Request.QueryString["TransactionId"] != null)
                {
                    string getquerystring = Request.QueryString["TransactionId"].ToString();
                    string[] getquerystring1 = getquerystring.Split(',');
                    string getpagename = getquerystring1[1].ToString();
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    // make collection editable
                    isreadonly.SetValue(this.Request.QueryString, false, null);

                    // remove
                    this.Request.QueryString.Remove("TransactionId");
                    lblcabTranId.Text = String.Empty;
                    if (getpagename == "CalHis")
                    {
                        Response.Redirect("~/CalibrationHistoryReport.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/CalibrationRejHistoryReport.aspx");
                    }

                }

            }


        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void gaugeReschedule()
    {
        #region Re-Schedule This Gauge

        DateTime upddate = DateTime.Now;
        string stupdate = upddate.ToString("yyyy-MM-dd H:mm:ss");
       
        DateTime caliDate = DateTime.ParseExact(txtCalibrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        string strcaliDate = caliDate.ToString("yyyy-MM-dd H:mm:ss");
       


        DataTable dtupdschedule = g.ReturnData("Update calibration_schedule_TB set status=False where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + "");
        DataTable dtselect = g.ReturnData("Select status,customer_id,gauge_id,calibrate_id,calibration_frequency, calibration_hours,frequency_type,intial_time_used,last_calibrated_by,created_by_id,bias,linearity,stability from calibration_schedule_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + "");
        if (ddlStatus.SelectedItem.Text == "PASSED")
        {

            string stnextd = "";
            string stprojectd = "";
            int feq = Convert.ToInt32(dtselect.Rows[0]["calibration_frequency"].ToString());
            DateTime dnextd = Convert.ToDateTime(caliDate);
            DateTime dprod = Convert.ToDateTime(caliDate);
            if (dtselect.Rows[0]["frequency_type"].ToString() == "YEAR")
            {
                dnextd = dnextd.AddYears(feq);
                stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");
                dprod = dprod.AddYears(feq + feq);
                stprojectd = dprod.ToString("yyyy-MM-dd H:mm:ss");
            }
            else
            {
                dnextd = dnextd.AddMonths(feq);
                stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");
                dprod = dprod.AddMonths(feq + feq);
                stprojectd = dprod.ToString("yyyy-MM-dd H:mm:ss");

            }
            DataTable dtsave = g.ReturnData("Insert into calibration_schedule_TB (status,customer_id,gauge_id,calibrate_id, calibration_frequency,calibration_hours,frequency_type,intial_time_used,last_calibrated_by,last_calibration_date, next_due_date,projected_calib_schedule,created_by_id,bias,linearity,stability)  values(True," + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(dtselect.Rows[0]["gauge_id"].ToString()) + ", " + Convert.ToInt32(dtselect.Rows[0]["calibrate_id"].ToString()) + ", '" + dtselect.Rows[0]["calibration_frequency"].ToString() + "', '" + txtCalibrationHours.Text + "','" + dtselect.Rows[0]["frequency_type"].ToString() + "','" + txtInitialTimeUsed.Text + "', " + Convert.ToInt32(dtselect.Rows[0]["last_calibrated_by"].ToString()) + ",'" + strcaliDate + "','" + stnextd + "', '" + stprojectd + "', " + Convert.ToInt32(Session["User_ID"]) + ",'" + dtselect.Rows[0]["bias"].ToString() + "','" + dtselect.Rows[0]["linearity"].ToString() + "','" + dtselect.Rows[0]["stability"].ToString() + "')");
        }

        #endregion
    }
    protected void btnClloseCalibTransaction_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }
    protected void btnEditCalibTran_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblcabTranId.Text = lnk.CommandArgument;
            getValueForEdit(Convert.ToInt32(lblcabTranId.Text));

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void getValueForEdit(int transactionId)
    {
        try
        {
            DataTable dtedit = g.ReturnData("Select customer_id,calibration_transaction_id,calibration_cost,calibration_date,calibration_hours, certification_no,gauge_id,humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_schedule_id, calibration_status from calibration_transaction_TB where calibration_transaction_id=" + transactionId + "");
            // calibration_transaction_TB cb = ds.calibration_transaction_TBs.Where(s => s.calibration_transaction_id == transactionId).FirstOrDefault();
            txtCalibrationID.Text = dtedit.Rows[0]["calibration_transaction_id"].ToString();
            txtCalibCost.Text = dtedit.Rows[0]["calibration_cost"].ToString();
            DateTime dtLastcalidate = Convert.ToDateTime(dtedit.Rows[0]["calibration_date"].ToString());
            txtCalibrationDate.Text = dtLastcalidate.ToString("dd/MM/yyyy");
            txtCalibHours.Text = dtedit.Rows[0]["calibration_hours"].ToString();
            txtCertificationNo.Text = dtedit.Rows[0]["certification_no"].ToString();
            txtGaugeId.Text = dtedit.Rows[0]["gauge_id"].ToString();
            txtHumidity.Text = dtedit.Rows[0]["humidity"].ToString();
            txtOther.Text = dtedit.Rows[0]["other"].ToString();
            txtPressure.Text = dtedit.Rows[0]["pressure"].ToString();
            txtTemprature.Text = dtedit.Rows[0]["temprature"].ToString();

            txtTolleranceGo.Text = dtedit.Rows[0]["tollerance_go"].ToString();// Here the value of Go tollerance
            txtTolleranceNoGo.Text = dtedit.Rows[0]["tollerance_no_go"].ToString(); // Here the value of No Go tollerance
            txtPermisableError1Entry.Text = dtedit.Rows[0]["tollerance_go"].ToString();// Here the value of Permissable error1 but same column name in both
            txtPermisableError2Entry.Text = dtedit.Rows[0]["tollerance_no_go"].ToString();// Here the value of Permissable error2
            txtImageName.Text = dtedit.Rows[0]["image_name"].ToString();
            if (Request.QueryString["TransactionId"] != null)
            {
                string getquerystring = Request.QueryString["TransactionId"].ToString();
                string[] getquerystring1 = getquerystring.Split(',');
                string getpagename = getquerystring1[1].ToString();

                if (getpagename == "RejHis")
                {
                    DataTable dtCalibSchedule = g.ReturnData("SELECT t0.gauge_id,concat_WS(': ID- ', t0.gauge_name, CAST(t0.gauge_id as CHAR(20))) AS gauge_name FROM gaugeMaster_TB AS t0 INNER JOIN calibration_schedule_TB AS t1 ON t0.gauge_id = t1.gauge_id WHERE t1.customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and t1.gauge_id=" + Convert.ToInt32(dtedit.Rows[0]["gauge_id"].ToString()) + " order by calibration_schedule_id desc limit 1");
                    ddlScheduleID.DataSource = dtCalibSchedule;
                    ddlScheduleID.DataTextField = "gauge_name";
                    ddlScheduleID.DataValueField = "gauge_id";
                    ddlScheduleID.DataBind();
                    ddlScheduleID.Items.Insert(0, "--Select--");

                }
            }

            ddlScheduleID.SelectedValue = dtedit.Rows[0]["gauge_id"].ToString();
            txtschduleId.Text = dtedit.Rows[0]["calibration_schedule_id"].ToString();
            fillGaugeDetails(Convert.ToInt32(dtedit.Rows[0]["calibration_schedule_id"].ToString()), Convert.ToInt32(dtedit.Rows[0]["customer_id"].ToString()));
            if (dtedit.Rows[0]["calibration_status"].ToString() == "PASSED")
            {
                ddlStatus.SelectedIndex = 1;
            }

            else if (dtedit.Rows[0]["calibration_status"].ToString() == "NOT INUSE")
            {
                ddlStatus.SelectedIndex = 2;
            }
            else if (dtedit.Rows[0]["calibration_status"].ToString() == "REJECTED")
            {
                ddlStatus.SelectedIndex = 3;
            }
            else
            {
                ddlStatus.SelectedIndex = 0;
            }

            MultiView1.ActiveViewIndex = 1;
            btnSaveCalibTransaction.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdCalibTran_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCalibTran.PageIndex = e.NewPageIndex;
        bindCalibrationTranscGrd(Convert.ToInt32(Session["Customer_ID"]));
    }
    protected void LnkDownLoadDocument_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int tranId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select calibration_certification_upload, image_name from calibration_transaction_TB where calibration_transaction_id='" + tranId + "'");

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
    private void displayDueStatus()
    {
        try
        {
            DataTable dt = new DataTable();
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spCalibrationDuelist";


            int custId = Convert.ToInt32(Convert.ToInt32(Session["Customer_ID"]));
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith10Param(stprocedure, 1, custId, 0, "", "", "", "", "", "", 0);
                dt = ds.Tables[0];

                if (Convert.ToInt32(Session["SupplierId"]) != 0)
                {
                    DataSet dss = q.ProcdureWith10Param(stprocedure, 2, custId, 0, "", "", "", "", "", "", Convert.ToInt32(Session["SupplierId"]));
                    dt = dss.Tables[0];

                }


            }
            else
            {
                //string sqlQuery = DeuStatusQuery();
                //string searchValue = txtsearchValue.Text;

                if (Convert.ToInt32(Session["SupplierId"]) != 0)
                {
                    int supid = Convert.ToInt32(Session["SupplierId"]);
                    if (ddlsortby.SelectedIndex == 1)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 3, custId, 0, searchValue, "", "", "", "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 2)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 4, custId, 0, "", searchValue, "", "", "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 3)
                    {
                        try
                        {
                            int gaugeId = Convert.ToInt32(searchValue);
                            DataSet dss = q.ProcdureWith10Param(stprocedure, 5, custId, gaugeId, "", "", "", "", "", "", 0);
                            dt = dss.Tables[0];
                        }
                        catch (Exception ex)
                        {
                            g.ShowMessage(this.Page, "Gauge Id is accept only numeric value. " + ex.Message);
                        }

                    }
                    else if (ddlsortby.SelectedIndex == 4)
                    {

                        DataSet dss = q.ProcdureWith10Param(stprocedure, 6, custId, 0, "", "", searchValue, "", "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 5)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 7, custId, 0, "", "", "", searchValue, "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 6)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 9, custId, 0, "", "", "", "", "", searchValue, 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 7)
                    {

                        DataSet dss = q.ProcdureWith10Param(stprocedure, 8, custId, 0, "", "", "", "", searchValue, "", 0);
                        dt = dss.Tables[0];
                    }

                }
                else
                {
                    if (ddlsortby.SelectedIndex == 1)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 3, custId, 0, searchValue, "", "", "", "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 2)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 4, custId, 0, "", searchValue, "", "", "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 3)
                    {
                        try
                        {
                            int gaugeId = Convert.ToInt32(searchValue);
                            DataSet dss = q.ProcdureWith10Param(stprocedure, 5, custId, gaugeId, "", "", "", "", "", "", 0);
                            dt = dss.Tables[0];
                        }
                        catch (Exception ex)
                        {
                            g.ShowMessage(this.Page, "Gauge Id is accept only numeric value. " + ex.Message);
                        }

                    }
                    else if (ddlsortby.SelectedIndex == 4)
                    {

                        DataSet dss = q.ProcdureWith10Param(stprocedure, 6, custId, 0, "", "", searchValue, "", "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 5)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 7, custId, 0, "", "", "", searchValue, "", "", 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 6)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 9, custId, 0, "", "", "", "", "", searchValue, 0);
                        dt = dss.Tables[0];
                    }
                    else if (ddlsortby.SelectedIndex == 7)
                    {
                        DataSet dss = q.ProcdureWith10Param(stprocedure, 8, custId, 0, "", "", "", "", searchValue, "", 0);
                        dt = dss.Tables[0];
                    }

                }


            }

            grdCalibDueListStatus.DataSource = dt;
            grdCalibDueListStatus.DataBind();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    protected void grdCalibDueListStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCalibDueListStatus.PageIndex = e.NewPageIndex;
        displayDueStatus();
    }
    protected void btnReTransactionCalib_Command(object sender, CommandEventArgs e)
    {
        try
        {
            clearFields();
            getNewTransactionId();
            Button lnk = (Button)sender;
            int gaugeId = Convert.ToInt32(e.CommandArgument.ToString());
            DataTable dtcalibscheduleid = g.ReturnData("Select customer_id,calibration_schedule_id from calibration_schedule_TB where gauge_id=" + gaugeId + " and status=True");

            fillGaugeDetails(Convert.ToInt32(dtcalibscheduleid.Rows[0]["calibration_schedule_id"].ToString()), Convert.ToInt32(dtcalibscheduleid.Rows[0]["customer_id"].ToString()));
            divDisplayData1.Visible = true;
            divDisplayData2.Visible = true;

            ddlScheduleID.SelectedValue = gaugeId.ToString();
            MultiView1.ActiveViewIndex = 1;
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
            btnSearch.Visible = true;
            lblName.Visible = true;
            if (ddlsortby.SelectedItem.Text == "Supplier-Wise")
            {

                divtxtsearch.Visible = true;
                txtsearchValue.Text = "";
            }
            else
            {
                divtxtsearch.Visible = true;

            }
        }
        else
        {
            btnSearch.Visible = false;

            lblName.Text = "";
            txtsearchValue.Text = "";
            lblName.Visible = false;

            divtxtsearch.Visible = false;

        }
        if (ddlsortby.SelectedItem.Text == "Supplier-Wise")
        {

            lblName.Text = "Supplier";
            searchBy.Text = "gs.supplier_id";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
        {
            lblName.Text = "Gauge Name";
            searchBy.Text = "gm.gauge_name";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
        {
            lblName.Text = "Gauge Sr.No.";
            searchBy.Text = "gm.gauge_sr_no";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
        {
            lblName.Text = "Gauge Id";
            searchBy.Text = "gm.gauge_id";
        }
        else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
        {
            lblName.Text = "Manufacture Id";
            searchBy.Text = "gm.gauge_Manufature_Id";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
        {
            lblName.Text = "Gauge Type";
            searchBy.Text = "gm.gauge_type";
        }

        else if (ddlsortby.SelectedItem.Text == "Part-Wise")
        {
            lblName.Text = "Part Name";
            searchBy.Text = "pt.part_name";
        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            displayDueStatus();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            displayDueStatus();


        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    protected void LnkApproved_Command(object sender, CommandEventArgs e)
    {
        lblTranId.Text = "";
        Button Lnk = (Button)sender;
        int transactionId = Convert.ToInt32(e.CommandArgument.ToString());
        lblTranId.Text = Convert.ToString(transactionId);
        tblPaymentDetails.Visible = true;
        ModalPopupExtender1.Show();
    }
    protected void btnSaveRemak_Click(object sender, EventArgs e)
    {
        DateTime upddate = DateTime.Now;
        string stupdate = upddate.ToString("yyyy-MM-dd H:mm:ss");
        lblmsg.Visible = false;
        if (txtremark.Text != "")
        {
            lblmsg.Visible = false;
            int tranid = Convert.ToInt32(lblTranId.Text);
            DataTable dtupd = g.ReturnData("Update calibration_transaction_TB set remarks='" + txtremark.Text + "' , is_approved=True,approved_by=" + Convert.ToInt32(Session["User_ID"]) + " where calibration_transaction_id=" + tranid + "");


            //#region   save code for calibration transaction upd tb
            //DataTable dtedit = g.ReturnData("Select customer_id,calibration_certification_upload,calibration_transaction_id,calibration_cost,calibration_date,calibration_hours, certification_no,gauge_id,humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_schedule_id, calibration_status from calibration_transaction_TB where calibration_transaction_id=" + tranid + "");
            //DateTime caliDate = Convert.ToDateTime(dtedit.Rows[0]["calibration_date"].ToString());
            //    string strcaliDate = caliDate.ToString("yyyy-MM-dd H:mm:ss");
            // string stfile = "";
            //    stfile = dtedit.Rows[0]["calibration_certification_upload"].ToString();
            //    if (stfile!="")
            //    {

            //            imgByte = (Byte[])dtedit.Rows[0]["calibration_certification_upload"];

            //    }
            //    string stquery1 = "Insert into calibration_transaction_upd_tb (status,customer_id,calibration_cost, calibration_date,calibration_hours,calibration_schedule_id,calibration_status,certification_no,created_by_id,gauge_id,  humidity,other,pressure,temprature,tollerance_go,tollerance_no_go,image_name,calibration_certification_upload,calibration_transaction_id,remarks,is_approved,approved_by,updated_date)  values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23)";
            //g.savetranwith24param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), Convert.ToDecimal(dtedit.Rows[0]["calibration_cost"].ToString()), strcaliDate, dtedit.Rows[0]["calibration_hours"].ToString(), Convert.ToInt32(dtedit.Rows[0]["calibration_schedule_id"].ToString()), dtedit.Rows[0]["calibration_status"].ToString(), dtedit.Rows[0]["certification_no"].ToString(), Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(dtedit.Rows[0]["gauge_id"].ToString()), dtedit.Rows[0]["humidity"].ToString(), dtedit.Rows[0]["other"].ToString(), dtedit.Rows[0]["pressure"].ToString(), dtedit.Rows[0]["temprature"].ToString(), dtedit.Rows[0]["tollerance_go"].ToString(), dtedit.Rows[0]["tollerance_no_go"].ToString(), dtedit.Rows[0]["image_name"].ToString(), imgByte, tranid, txtremark.Text, "1", Convert.ToInt32(Session["User_ID"]), stupdate);
            //#endregion
            g.ShowMessage(this.Page, "Transaction is Approved successfully.");

            bindCalibrationTranscGrd(Convert.ToInt32(Session["Customer_ID"]));
            tblPaymentDetails.Visible = false;
            ModalPopupExtender1.Hide();
        }
        else
        {
            lblmsg.Visible = true;
            tblPaymentDetails.Visible = true;
            ModalPopupExtender1.Show();
        }

    }
}