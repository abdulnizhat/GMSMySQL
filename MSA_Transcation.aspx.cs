using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MSA_Transcation : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    Byte[] imgByteBias = null;
    Byte[] imgByteLinearity = null;
    Byte[] imgByteStability = null;
    Byte[] imgByteRR = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                fillGauge(Convert.ToInt32(Session["Customer_ID"]));
                bindMSATranscGrd(Convert.ToInt32(Session["Customer_ID"]));
                btnAddMSATransaction.Focus();
                displayMsaDueStatus();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void fillGauge(int custId)
    {
        try
        {
            DataTable dtCalibSchedule = g.ReturnData("SELECT t0.gauge_id,concat_WS(': ID- ', t0.gauge_name, CAST(t0.gauge_id as CHAR(20))) AS gauge_name FROM gaugeMaster_TB AS t0 INNER JOIN msa_schedule_TB AS t1 ON t0.gauge_id = t1.gauge_id WHERE t1.customer_id = " + custId + " AND t1.status = True");


            ddlGauge.DataSource = dtCalibSchedule;
            ddlGauge.DataTextField = "gauge_name";
            ddlGauge.DataValueField = "gauge_id";
            ddlGauge.DataBind();
            ddlGauge.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindMSATranscGrd(int custId)
    {
        try
        {

            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spMSATransactionDt";

            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
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

            
            grdMSATran.DataSource = dt;
            grdMSATran.DataBind();

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
            childId = g.GetChildId("MSA_Transcation.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddMSATransaction.Visible = true;
                }
                else
                {
                    btnAddMSATransaction.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdMSATran.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdMSATran.Rows[i].FindControl("btnEditMSATran");
                        lnk.Enabled = true;
                        LinkButton lnk1 = (LinkButton)grdMSATran.Rows[i].FindControl("LnkDownLoadDocument");
                        lnk1.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdMSATran.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdMSATran.Rows[i].FindControl("btnEditMSATran");
                        lnk.Enabled = false;
                        LinkButton lnk1 = (LinkButton)grdMSATran.Rows[i].FindControl("LnkDownLoadDocument");
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
    protected void btnAddMSATransaction_Click(object sender, EventArgs e)
    {
        ddlGauge.Focus();
        MultiView1.ActiveViewIndex = 1;
        getMSATransactionId();
    }
    private void getMSATransactionId()
    {

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(msa_transaction_id) from msa_transaction_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());

            txtMSATranID.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {
            txtMSATranID.Text = "1";
        }

    }
    protected void ddlGauge_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlGauge.Focus();
        if (ddlGauge.SelectedIndex > 0)
        {
            DataTable dtcalibscheduleid = g.ReturnData("Select msa_schedule_id from msa_schedule_TB where gauge_id=" + Convert.ToInt32(ddlGauge.SelectedValue) + " and status=True");
            fillGaugeDetails(Convert.ToInt32(dtcalibscheduleid.Rows[0]["msa_schedule_id"].ToString()));
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
    private void fillGaugeDetails(int scheduleId)
    {
        try
        {
            divDisplayData1.Visible = true;
            divDisplayData2.Visible = true;
            string stprocedure = "spFetchGaugDtOnMSATransction";
            DataSet ds = q.ProcdureWith3Param(stprocedure, 1, Convert.ToInt32(Session["Customer_ID"]), scheduleId);
          
            DataTable dtgauge = ds.Tables[0];
            txtschduleId.Text = dtgauge.Rows[0]["msa_schedule_id"].ToString();
            txtGaugeId.Text = dtgauge.Rows[0]["gauge_id"].ToString();
            txtGaugeName.Text = dtgauge.Rows[0]["gauge_name"].ToString();
            txtManufactureID.Text = dtgauge.Rows[0]["gauge_Manufature_Id"].ToString();
            txtType.Text = dtgauge.Rows[0]["gauge_type"].ToString();
            if (dtgauge.Rows[0]["gauge_type"].ToString() == "ATTRIBUTE")
            {
                divSize.Visible = true;
                divRange.Visible = false;
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

            //DateTime purchaseDate = Convert.ToDateTime(dtgauge.Rows[0]["Purchasedate"].ToString());
            txtPurchaseDate.Text = dtgauge.Rows[0]["Purchasedate"].ToString();
            // DateTime serviceDate = Convert.ToDateTime(dtgauge.Rows[0]["ServiceDate"].ToString());
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
            //DateTime lastcalibDate = Convert.ToDateTime(dtgauge.Rows[0]["LastCalibrationDate"].ToString());
            txtLastCalibrationDate.Text = dtgauge.Rows[0]["LastCalibrationDate"].ToString();
            // DateTime nextcalibDate = Convert.ToDateTime(dtgauge.Rows[0]["NextDueDate"].ToString());
            txtNextDueDate.Text = dtgauge.Rows[0]["NextDueDate"].ToString();
            txtPermisableError1.Text = dtgauge.Rows[0]["permisable_error1"].ToString();
            txtPermisableError2.Text = dtgauge.Rows[0]["permisable_error2"].ToString();

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void clearFields()
    {
        txtMSATranID.Text = txtMSADate.Text =
        lblMSATranId.Text = txtMSAHours.Text = txtMSAReportNo.Text =
        txtGaugeId.Text = txtHumidity.Text = txtOther.Text = txtPressure.Text =
        txtTemprature.Text = txtSize.Text = txtRange.Text = txtGoTolleranceMinus.Text =
        txtGoTollerancePlus.Text = txtNoGoTolleranceMinus.Text =
        txtNoGoTollerancePlus.Text = txtGoWereLimit.Text = txtLeastCount.Text =
        txtCurrentLocation.Text = txtStoreLocation.Text =
        txtPurchaseDate.Text = txtServiceDate.Text = txtRetairementDate.Text =
        txtInitialTimeUsed.Text = txtResolution.Text = txtLastCalibratedBy.Text =
        txtCalibrattedBy.Text = txtFrequencyType.Text = txtCalibrationFrequency.Text =
        txtCalibrationHours.Text = txtLastCalibrationDate.Text = txtNextDueDate.Text =
        txtPermisableError1.Text =
        txtPermisableError2.Text = txtType.Text = txtGaugeName.Text = txtschduleId.Text =
        txtManufactureID.Text = txtPurchaseCost.Text =
        lblFileBias.Text =
                lblFileLinearity.Text =
                lblFileStability.Text =
                lblFileRR.Text = string.Empty;
        ddlGauge.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        btnSaveMSATransaction.Text = "Save";
        btnAddMSATransaction.Focus();
    }
    protected void btnSaveMSATransaction_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {
            #region File Upload Bias
            FileUpload img = (FileUpload)FileUploadBias;
            if (img.HasFile && img.PostedFile != null)
            {
                imgByteBias = null;
                //To create a PostedFile
                HttpPostedFile File = FileUploadBias.PostedFile;
                lblFileBias.Text = File.FileName;
                string filePath = FileUploadBias.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                long fileSize = FileUploadBias.FileContent.Length;
                if (fileSize <= 1024000) // Check in byte 1024000 Byte =1.024 MB . 1 MB.
                {
                    string contenttype = String.Empty;
                    switch (ext)
                    {
                        case ".xls":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                    }
                    if (contenttype != String.Empty)
                    {
                        Stream stream = File.InputStream;
                        BinaryReader bReader = new BinaryReader(stream);
                        imgByteBias = bReader.ReadBytes((int)stream.Length);
                        //Create byte Array with file len
                        //imgByteBias = new Byte[File.ContentLength];
                        //force the control to load data in array
                        // File.InputStream.Read(imgByteBias, 0, File.ContentLength);
                    }
                    else
                    {
                        g.ShowMessage(this.Page, "File type is not valid.");
                        return;
                    }
                }
                else
                {
                    g.ShowMessage(this.Page, "Selected Bias file size is exceeds the size limit 1 MB only.");
                    return;
                }

            }
            #endregion

            #region File Upload Linearity
            FileUpload imgLinearity = (FileUpload)FileUploadLinearity;
            if (imgLinearity.HasFile && imgLinearity.PostedFile != null)
            {
                imgByteLinearity = null;
                //To create a PostedFile
                HttpPostedFile FileLinearity = FileUploadLinearity.PostedFile;
                lblFileLinearity.Text = FileLinearity.FileName;
                string filePath = FileUploadLinearity.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                long fileSize = FileUploadLinearity.FileContent.Length;
                if (fileSize <= 1024000) // Check in byte 1024000 Byte =1.024 MB . 1 MB.
                {
                    string contenttype = String.Empty;
                    switch (ext)
                    {
                        case ".xls":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                    }
                    if (contenttype != String.Empty)
                    {
                        Stream stream = FileLinearity.InputStream;
                        BinaryReader bReader = new BinaryReader(stream);
                        imgByteLinearity = bReader.ReadBytes((int)stream.Length);
                        //Create byte Array with file len
                        // imgByteLinearity = new Byte[FileLinearity.ContentLength];
                        //force the control to load data in array
                        //FileLinearity.InputStream.Read(imgByteLinearity, 0, FileLinearity.ContentLength);
                    }
                    else
                    {
                        g.ShowMessage(this.Page, "File type is not valid.");
                        return;
                    }
                }
                else
                {
                    g.ShowMessage(this.Page, "Selected Linearity file size is exceeds the size limit 1 MB only.");
                    return;
                }

            }
            #endregion


            #region File Upload Stability
            FileUpload imgStability = (FileUpload)FileUploadStability;
            if (imgStability.HasFile && imgStability.PostedFile != null)
            {
                imgByteStability = null;
                //To create a PostedFile
                HttpPostedFile FileStability = FileUploadStability.PostedFile;
                lblFileStability.Text = FileStability.FileName;
                string filePath = FileUploadStability.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                long fileSize = FileUploadStability.FileContent.Length;
                if (fileSize <= 1024000) // Check in byte 1024000 Byte =1.024 MB . 1 MB.
                {
                    string contenttype = String.Empty;
                    switch (ext)
                    {
                        case ".xls":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                    }
                    if (contenttype != String.Empty)
                    {
                        Stream stream = FileStability.InputStream;
                        BinaryReader bReader = new BinaryReader(stream);
                        imgByteStability = bReader.ReadBytes((int)stream.Length);
                        //Create byte Array with file len
                        // imgByteStability = new Byte[FileStability.ContentLength];
                        //force the control to load data in array
                        // FileStability.InputStream.Read(imgByteStability, 0, FileStability.ContentLength);
                    }
                    else
                    {
                        g.ShowMessage(this.Page, "File type is not valid.");
                        return;
                    }
                }
                else
                {
                    g.ShowMessage(this.Page, "Selected Stability file size is exceeds the size limit 1 MB only.");
                    return;
                }

            }
            #endregion

            #region File Upload R&R
            FileUpload imgRR = (FileUpload)FileUploadRR;
            if (imgRR.HasFile && imgRR.PostedFile != null)
            {
                imgByteRR = null;
                //To create a PostedFile
                HttpPostedFile FileRR = FileUploadRR.PostedFile;
                lblFileRR.Text = FileRR.FileName;
                string filePath = FileUploadRR.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                long fileSize = FileUploadRR.FileContent.Length;
                if (fileSize <= 1024000) // Check in byte 1024000 Byte =1.024 MB . 1 MB.
                {
                    string contenttype = String.Empty;
                    switch (ext)
                    {
                        case ".xls":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contenttype = "application/vnd.ms-excel";
                            break;
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                    }
                    if (contenttype != String.Empty)
                    {
                        Stream stream = FileRR.InputStream;
                        BinaryReader bReader = new BinaryReader(stream);
                        imgByteRR = bReader.ReadBytes((int)stream.Length);
                        //Create byte Array with file len
                        // imgByteRR = new Byte[FileRR.ContentLength];
                        //force the control to load data in array
                        //FileRR.InputStream.Read(imgByteRR, 0, FileRR.ContentLength);
                    }
                    else
                    {
                        g.ShowMessage(this.Page, "File type is not valid.");
                        return;
                    }
                }
                else
                {
                    g.ShowMessage(this.Page, "Selected R&R file size is exceeds the size limit 1 MB only.");
                    return;
                }

            }
            #endregion


            if (btnSaveMSATransaction.Text == "Save")
            {
                DataTable dtexist = g.ReturnData("Select calibration_schedule_id from msa_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and status=True");

                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "MSA transaction is already exist.");
                    return;
                }
                else
                {
                    saveUpdateMSATran(0);
                }
            }
            else
            {
                int editId = Convert.ToInt32(lblMSATranId.Text);
                DataTable dtexist = g.ReturnData("Select calibration_schedule_id from msa_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and msa_transaction_id=" + editId + "");

                if (dtexist.Rows.Count > 0)
                {
                    saveUpdateMSATran(editId);
                }
                else
                {
                    DataTable dtexist1 = g.ReturnData("Select calibration_schedule_id from msa_transaction_TB where calibration_schedule_id=" + Convert.ToInt32(txtschduleId.Text) + " and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and msa_transaction_id<>" + editId + "");

                    if (dtexist1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "MSA transaction is already exist.");
                        return;
                    }
                    else
                    {
                        saveUpdateMSATran(editId);
                    }
                }

            }

            clearFields();
            bindMSATranscGrd(Convert.ToInt32(Session["Customer_ID"]));
            displayMsaDueStatus();
            MultiView1.ActiveViewIndex = 0;
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void saveUpdateMSATran(int editMSATranId)
    {
        try
        {
            DateTime upddate = DateTime.Now;
            string stupdate = upddate.ToString("yyyy-MM-dd H:mm:ss");
            string stfilename = "";
            Byte[] stmsauploaddata = null;

            string stlinitfilename = "";
            Byte[] stmsalinitupload = null;

            string stsstabilitly = "";
            Byte[] ststabilityupload = null;

            string strrfilename = "";
            Byte[] strrupload = null;

            if (imgByteBias != null)
            {
                stfilename = lblFileBias.Text;
                stmsauploaddata = imgByteBias;
            }

            if (imgByteLinearity != null)
            {
                stlinitfilename = lblFileLinearity.Text;
                stmsalinitupload = imgByteLinearity;
            }

            if (imgByteStability != null)
            {
                stsstabilitly = lblFileStability.Text;
                ststabilityupload = imgByteStability;
            }
            if (imgByteRR != null)
            {
                strrfilename = lblFileRR.Text;
                strrupload = imgByteRR;
            }
            DateTime msaDate = DateTime.ParseExact(txtMSADate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string msaDate1 = msaDate.ToString("yyyy-MM-dd H:mm:ss");
            if (btnSaveMSATransaction.Text == "Save")
            {
                string stquery = "Insert into msa_transaction_TB (status,customer_id,msa_date,msa_hours,calibration_schedule_id, msa_status,msa_report_no,created_by_id,gauge_id,humidity,other,pressure,temprature,file_name,msa_report_uploaded_data,liniarity_file_name, msa_liniarity_uploaded_data,msa_stability_file_name,msa_stability_uploaded_data,msa_RR_file_name,msa_RR_uploaded_data)   values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21)";
                g.savemsatranwith22param(stquery, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, txtMSAHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtMSAReportNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload);

                ////save code for msa_transaction_upd_tb
                //DataTable dtmaxid = g.ReturnData("Select Max(msa_transaction_id) from  msa_transaction_TB");
                //int maxId = Convert.ToInt32(dtmaxid.Rows[0][0].ToString());

                //string stquery1 = "Insert into msa_transactio_upd_tb (status,customer_id,msa_date,msa_hours,calibration_schedule_id, msa_status,msa_report_no,created_by_id,gauge_id,humidity,other,pressure,temprature,file_name,msa_report_uploaded_data,liniarity_file_name, msa_liniarity_uploaded_data,msa_stability_file_name,msa_stability_uploaded_data,msa_RR_file_name,msa_RR_uploaded_data,msa_transaction_id,updated_date)   values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23)";
                //g.savemsatranwith24param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, txtMSAHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtMSAReportNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload, maxId, stupdate);

                #region Re-Schedule This Gauge
                DataTable dtmax = g.ReturnData("Select calibration_schedule_id from msa_transaction_TB where msa_transaction_id=(select MAX(msa_transaction_id) from msa_transaction_TB) ");

                DataTable dtupdmsaSched = g.ReturnData("Update msa_schedule_TB set status=False where msa_schedule_id=" + Convert.ToInt32(dtmax.Rows[0]["calibration_schedule_id"].ToString()) + "");


                DataTable dtselect = g.ReturnData("Select gauge_id,calibrate_id,calibration_frequency,frequency_type,last_calibrated_by,  bias,linearity,stability from msa_schedule_TB where msa_schedule_id=" + Convert.ToInt32(dtmax.Rows[0]["calibration_schedule_id"].ToString()) + "");

                int feq = Convert.ToInt32(dtselect.Rows[0]["calibration_frequency"].ToString());
                DateTime dnextd = Convert.ToDateTime(msaDate);
                if (dtselect.Rows[0]["frequency_type"].ToString() == "YEAR")
                {
                    dnextd = dnextd.AddYears(feq);
                }
                else
                {
                    dnextd = dnextd.AddMonths(feq);
                }
                string stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");

                DataTable dtsavesched = g.ReturnData("Insert into msa_schedule_TB (status,customer_id,gauge_id,calibrate_id,calibration_frequency,  calibration_hours,frequency_type,intial_time_used,last_calibrated_by,last_calibration_date,next_due_date,created_by_id, bias,linearity,stability) values(True," + Convert.ToInt32(Session["Customer_ID"]) + ", " + Convert.ToInt32(dtselect.Rows[0]["gauge_id"].ToString()) + ", " + Convert.ToInt32(dtselect.Rows[0]["calibrate_id"].ToString()) + ", '" + dtselect.Rows[0]["calibration_frequency"].ToString() + "','" + txtCalibrationHours.Text + "', '" + dtselect.Rows[0]["frequency_type"].ToString() + "', '" + txtInitialTimeUsed.Text + "'," + Convert.ToInt32(dtselect.Rows[0]["last_calibrated_by"].ToString()) + ",'" + msaDate1 + "', '" + stnextd + "'," + Convert.ToInt32(Session["User_ID"]) + ",'" + dtselect.Rows[0]["bias"].ToString() + "', '" + dtselect.Rows[0]["linearity"].ToString() + "', '" + dtselect.Rows[0]["stability"].ToString() + "')");



                #endregion
                g.ShowMessage(this.Page, "MSA transaction is saved and MSA rescheduled successfully.");
            }
            else
            {
                DataTable dt = g.ReturnData("Select msa_status,file_name,liniarity_file_name,msa_stability_file_name,msa_RR_file_name, msa_report_uploaded_data, msa_liniarity_uploaded_data,msa_stability_uploaded_data,msa_RR_uploaded_data from msa_transaction_TB where msa_transaction_id=" + editMSATranId + "");
                string ststatus = "";
                ststatus = dt.Rows[0]["msa_status"].ToString();
                string stbios = ""; string stliniraty = ""; string ststabi = ""; string strr = "";
                stbios = dt.Rows[0]["msa_report_uploaded_data"].ToString();
                stliniraty = dt.Rows[0]["msa_liniarity_uploaded_data"].ToString();
                ststabi = dt.Rows[0]["msa_stability_uploaded_data"].ToString();
                strr = dt.Rows[0]["msa_RR_uploaded_data"].ToString();

                if (imgByteBias == null)
                {
                    if (stbios != "")
                    {

                        stfilename = dt.Rows[0]["file_name"].ToString();
                        stmsauploaddata = (Byte[])dt.Rows[0]["msa_report_uploaded_data"];
                    }
                }
                if (imgByteLinearity == null)
                {
                    if (stliniraty != "")
                    {

                        stlinitfilename = dt.Rows[0]["liniarity_file_name"].ToString();
                        stmsalinitupload = (Byte[])dt.Rows[0]["msa_liniarity_uploaded_data"];
                    }
                }
                if (imgByteStability == null)
                {
                    if (ststabi != "")
                    {

                        stsstabilitly = dt.Rows[0]["msa_stability_file_name"].ToString();
                        ststabilityupload = (Byte[])dt.Rows[0]["msa_stability_uploaded_data"];
                    }
                }
                if (imgByteRR == null)
                {
                    if (strr != "")
                    {
                        strrfilename = dt.Rows[0]["msa_RR_file_name"].ToString();
                        strrupload = (Byte[])dt.Rows[0]["msa_RR_uploaded_data"];
                    }
                }

                string stquery = "Update msa_transaction_TB set status=?param1,customer_id=?param2,msa_date=?param3,msa_hours=?param4,calibration_schedule_id=?param5,   msa_status=?param6,msa_report_no=?param7,created_by_id=?param8,gauge_id=?param9,   humidity=?param10,other=?param11,pressure=?param12,temprature=?param13,file_name=?param14,msa_report_uploaded_data=?param15,  liniarity_file_name=?param16,msa_liniarity_uploaded_data=?param17,msa_stability_file_name=?param18,msa_stability_uploaded_data=?param19,msa_RR_file_name=?param20,msa_RR_uploaded_data=?param21 where msa_transaction_id=" + editMSATranId + "";
                g.savemsatranwith22param(stquery, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, txtMSAHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtMSAReportNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload);

                #region code for MSA transaction upd tb
                if (ststatus != ddlStatus.SelectedItem.Text)
                {
                    // save code
                    string stquery1 = "Insert into msa_transactio_upd_tb (status,customer_id,msa_date,msa_hours,calibration_schedule_id, msa_status,msa_report_no,created_by_id,gauge_id,humidity,other,pressure,temprature,file_name,msa_report_uploaded_data,liniarity_file_name, msa_liniarity_uploaded_data,msa_stability_file_name,msa_stability_uploaded_data,msa_RR_file_name,msa_RR_uploaded_data,msa_transaction_id,updated_date)   values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23)";
                    g.savemsatranwith24param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, txtMSAHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtMSAReportNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload, editMSATranId, stupdate);

                }
                else
                {
                    //Update Code
                    try
                    {
                        DataTable dtmax = g.ReturnData("SELECT max(msa_transaction_upd_id) FROM msa_transactio_upd_tb where msa_transaction_id=" + editMSATranId + "");
                        int msaupdid = Convert.ToInt32(dtmax.Rows[0][0].ToString());
                        string stquery1 = "Update msa_transactio_upd_tb set status=?param1,customer_id=?param2,msa_date=?param3,msa_hours=?param4,calibration_schedule_id=?param5,   msa_status=?param6,msa_report_no=?param7,created_by_id=?param8,gauge_id=?param9,   humidity=?param10,other=?param11,pressure=?param12,temprature=?param13,file_name=?param14,msa_report_uploaded_data=?param15,  liniarity_file_name=?param16,msa_liniarity_uploaded_data=?param17,msa_stability_file_name=?param18,msa_stability_uploaded_data=?param19,msa_RR_file_name=?param20,msa_RR_uploaded_data=?param21,msa_transaction_id=?param22,updated_date=?param23 where msa_transaction_upd_id=" + msaupdid + "";
                        g.savemsatranwith24param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, txtMSAHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtMSAReportNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload, editMSATranId, stupdate);
                    }
                    catch (Exception)
                    {
                        string stquery1 = "Insert into msa_transactio_upd_tb (status,customer_id,msa_date,msa_hours,calibration_schedule_id, msa_status,msa_report_no,created_by_id,gauge_id,humidity,other,pressure,temprature,file_name,msa_report_uploaded_data,liniarity_file_name, msa_liniarity_uploaded_data,msa_stability_file_name,msa_stability_uploaded_data,msa_RR_file_name,msa_RR_uploaded_data,msa_transaction_id,updated_date)   values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23)";
                        g.savemsatranwith24param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, txtMSAHours.Text, Convert.ToInt32(txtschduleId.Text), ddlStatus.SelectedItem.Text, txtMSAReportNo.Text, Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(txtGaugeId.Text), txtHumidity.Text, txtOther.Text, txtPressure.Text, txtTemprature.Text, stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload, editMSATranId, stupdate);

                    }
                }
                #endregion

                #region Re-Schedule This Gauge

                DataTable dtfreq = g.ReturnData("Select calibration_frequency,frequency_type from msa_schedule_TB where gauge_id=" + Convert.ToInt32(txtGaugeId.Text) + " and status=True");
                int feq = Convert.ToInt32(dtfreq.Rows[0]["calibration_frequency"].ToString());
                DateTime dnextd = Convert.ToDateTime(msaDate);
                if (dtfreq.Rows[0]["frequency_type"].ToString() == "YEAR")
                {
                    dnextd = dnextd.AddYears(feq);
                }
                else
                {
                    dnextd = dnextd.AddMonths(feq);
                }
                string stnextd = dnextd.ToString("yyyy-MM-dd H:mm:ss");

                DataTable dtupdatemssche = g.ReturnData("Update msa_schedule_TB set customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ", last_calibration_date='" + msaDate1 + "', next_due_date='" + stnextd + "', created_by_id=" + Convert.ToInt32(Session["User_ID"]) + "  where gauge_id=" + Convert.ToInt32(txtGaugeId.Text) + " and status=True");

                #endregion

                g.ShowMessage(this.Page, "MSA transaction and MSA resceduled are updated successfully.");
            }



        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseMSATransaction_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }
    protected void btnEditMSATran_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblMSATranId.Text = lnk.CommandArgument;
            DataTable dtedit = g.ReturnData("Select msa_transaction_id,msa_date,msa_hours,msa_report_no,gauge_id, humidity,other,pressure,temprature,file_name,liniarity_file_name,msa_stability_file_name,msa_RR_file_name,msa_report_uploaded_data, msa_liniarity_uploaded_data,msa_stability_uploaded_data,msa_RR_uploaded_data, calibration_schedule_id,msa_status  from msa_transaction_TB where msa_transaction_id=" + Convert.ToInt32(lblMSATranId.Text) + "");


            txtMSATranID.Text = dtedit.Rows[0]["msa_transaction_id"].ToString();
            DateTime dtLastcalidate = Convert.ToDateTime(dtedit.Rows[0]["msa_date"].ToString());
            txtMSADate.Text = dtLastcalidate.ToString("dd/MM/yyyy");
            txtMSAHours.Text = dtedit.Rows[0]["msa_hours"].ToString();
            txtMSAReportNo.Text = dtedit.Rows[0]["msa_report_no"].ToString();
            txtGaugeId.Text = dtedit.Rows[0]["gauge_id"].ToString();
            txtHumidity.Text = dtedit.Rows[0]["humidity"].ToString();
            txtOther.Text = dtedit.Rows[0]["other"].ToString();
            txtPressure.Text = dtedit.Rows[0]["pressure"].ToString();
            txtTemprature.Text = dtedit.Rows[0]["temprature"].ToString();
            lblFileBias.Text = dtedit.Rows[0]["file_name"].ToString();
            lblFileLinearity.Text = dtedit.Rows[0]["liniarity_file_name"].ToString();
            lblFileStability.Text = dtedit.Rows[0]["msa_stability_file_name"].ToString();
            lblFileRR.Text = dtedit.Rows[0]["msa_RR_file_name"].ToString();

            ddlGauge.SelectedValue = dtedit.Rows[0]["gauge_id"].ToString();
            txtschduleId.Text = dtedit.Rows[0]["calibration_schedule_id"].ToString();
            fillGaugeDetails(Convert.ToInt32(dtedit.Rows[0]["calibration_schedule_id"].ToString()));
            if (dtedit.Rows[0]["msa_status"].ToString() == "PASSED")
            {
                ddlStatus.SelectedIndex = 1;
            }
            else if (dtedit.Rows[0]["msa_status"].ToString() == "FAILED")
            {
                ddlStatus.SelectedIndex = 2;
            }
            else if (dtedit.Rows[0]["msa_status"].ToString() == "REPAIR")
            {
                ddlStatus.SelectedIndex = 3;
            }
            else
            {
                ddlStatus.SelectedIndex = 0;
            }

            MultiView1.ActiveViewIndex = 1;
            btnSaveMSATransaction.Text = "Update";

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


    protected void grdMSATran_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMSATran.PageIndex = e.NewPageIndex;
        bindMSATranscGrd(Convert.ToInt32(Session["Customer_ID"]));
    }
    private void displayMsaDueStatus()
    {
        try
        {
            string strQuery = " Select cs.msa_schedule_id, cs.gauge_id, gm.gauge_sr_no, gm.gauge_name, gm.gauge_sr_no,gm.size_range, gm.gauge_Manufature_Id, gm.gauge_type, cs.next_due_date " +
                          " from msa_schedule_TB as cs " +
                           " Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id " +
                            " where cs.status=1 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " " +
                            " and cs.next_due_date <= DATE_ADD(now(), INTERVAL 15 DAY) " +
                " and cs.gauge_id NOT IN (Select mt.gauge_id from msa_transaction_TB mt where mt.calibration_schedule_id=cs.msa_schedule_id)";
            DataTable dt = g.ReturnData(strQuery);
            grdMsaDue.DataSource = dt;
            grdMsaDue.DataBind();

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
    protected void grdMsaDue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMsaDue.PageIndex = e.NewPageIndex;
        displayMsaDueStatus();
    }
    protected void btnReTransactionMSA_Command(object sender, CommandEventArgs e)
    {
        try
        {
            clearFields();
            getMSATransactionId();
            Button lnk = (Button)sender;
            int gaugeId = Convert.ToInt32(e.CommandArgument.ToString());
            DataTable dtschid = g.ReturnData("Select msa_schedule_id from msa_schedule_TB where gauge_id=" + gaugeId + " and status=True");

            fillGaugeDetails(Convert.ToInt32(dtschid.Rows[0]["msa_schedule_id"].ToString()));
            divDisplayData1.Visible = true;
            divDisplayData2.Visible = true;


            ddlGauge.SelectedValue = gaugeId.ToString();
            MultiView1.ActiveViewIndex = 1;
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
            Logger.Error(ex.Message);
        }
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
       
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindMSATranscGrd(Convert.ToInt32(Session["Customer_ID"]));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindMSATranscGrd(Convert.ToInt32(Session["Customer_ID"]));


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
            DataTable dtupd = g.ReturnData("Update msa_transaction_TB set remarks='" + txtremark.Text + "' , is_approved=True,approved_by=" + Convert.ToInt32(Session["User_ID"]) + " where msa_transaction_id=" + tranid + "");


            #region   save code for calibration transaction upd tb
            DataTable dtedit = g.ReturnData("Select msa_transaction_id,msa_date,msa_hours,msa_report_no,gauge_id, humidity,other,pressure,temprature,file_name,liniarity_file_name,msa_stability_file_name,msa_RR_file_name,msa_report_uploaded_data, msa_liniarity_uploaded_data,msa_stability_uploaded_data,msa_RR_uploaded_data, calibration_schedule_id,msa_status  from msa_transaction_TB where msa_transaction_id=" + tranid + "");
            DateTime msaDate = Convert.ToDateTime(dtedit.Rows[0]["msa_date"].ToString());
            string msaDate1 = msaDate.ToString("yyyy-MM-dd H:mm:ss");

            string stfilename = "";
            stfilename = dtedit.Rows[0]["file_name"].ToString();
            Byte[] stmsauploaddata = null;

            string stlinitfilename = "";
            stlinitfilename = dtedit.Rows[0]["liniarity_file_name"].ToString();
            Byte[] stmsalinitupload = null;

            string stsstabilitly = "";
            stsstabilitly = dtedit.Rows[0]["msa_stability_file_name"].ToString();
            Byte[] ststabilityupload = null;

            string strrfilename = "";
            strrfilename = dtedit.Rows[0]["msa_RR_file_name"].ToString();
            Byte[] strrupload = null;

            if (stfilename != "")
            {

                stmsauploaddata = (Byte[])dtedit.Rows[0]["msa_report_uploaded_data"];
            }

            if (stlinitfilename != "")
            {

                stmsalinitupload = (Byte[])dtedit.Rows[0]["msa_liniarity_uploaded_data"];
            }

            if (stsstabilitly != "")
            {

                ststabilityupload = (Byte[])dtedit.Rows[0]["msa_stability_uploaded_data"];
            }
            if (strrfilename != "")
            {

                strrupload = (Byte[])dtedit.Rows[0]["msa_RR_uploaded_data"];
            }
            
           
            // save code
            string stquery1 = "Insert into msa_transactio_upd_tb (status,customer_id,msa_date,msa_hours,calibration_schedule_id, msa_status,msa_report_no,created_by_id,gauge_id,humidity,other,pressure,temprature,file_name,msa_report_uploaded_data,liniarity_file_name, msa_liniarity_uploaded_data,msa_stability_file_name,msa_stability_uploaded_data,msa_RR_file_name,msa_RR_uploaded_data,msa_transaction_id,updated_date,remarks,is_approved,approved_by)   values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23,?param24,?param25,?param26)";
            g.savemsatranwith27param(stquery1, "1", Convert.ToInt32(Session["Customer_ID"]), msaDate1, dtedit.Rows[0]["msa_hours"].ToString(), Convert.ToInt32(dtedit.Rows[0]["calibration_schedule_id"].ToString()), dtedit.Rows[0]["msa_status"].ToString(), dtedit.Rows[0]["msa_report_no"].ToString(), Convert.ToInt32(Session["User_ID"]), Convert.ToInt32(dtedit.Rows[0]["gauge_id"].ToString()), dtedit.Rows[0]["humidity"].ToString(), dtedit.Rows[0]["other"].ToString(), dtedit.Rows[0]["pressure"].ToString(), dtedit.Rows[0]["temprature"].ToString(), stfilename, stmsauploaddata, stlinitfilename, stmsalinitupload, stsstabilitly, ststabilityupload, strrfilename, strrupload, tranid, stupdate, txtremark.Text, "1", Convert.ToInt32(Session["User_ID"]));

            #endregion
            g.ShowMessage(this.Page, "Transaction is Approved successfully.");

            bindMSATranscGrd(Convert.ToInt32(Session["Customer_ID"]));
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