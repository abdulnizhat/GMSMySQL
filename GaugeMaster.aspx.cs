using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GaugeMaster : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string blob = "";
    string gaugeName = "";
    string gaugeSrNo = "";
    byte[] imgByte = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {

            if (!Page.IsPostBack)
            {
                Session["dleteGage"] = "NO";
                btnAddGauge.Focus();
                MultiView1.ActiveViewIndex = 0;
                bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));

            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("GaugeMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');
                if (staustatus[0].ToString() == "True")
                {
                    btnAddGauge.Visible = true;
                }
                else
                {
                    btnAddGauge.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdGauge.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGauge.Rows[i].FindControl("btnEditGauge");
                        LinkButton lnkdown = (LinkButton)grdGauge.Rows[i].FindControl("LnkDownLoadDocument");
                        lnkdown.Enabled = true;
                        lnk.Enabled = true;
                        LinkButton lnkdlet = (LinkButton)grdGauge.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = true;
                        Session["dleteGage"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdGauge.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGauge.Rows[i].FindControl("btnEditGauge");
                        lnk.Enabled = false;
                        LinkButton lnkdown = (LinkButton)grdGauge.Rows[i].FindControl("LnkDownLoadDocument");
                        lnkdown.Enabled = false;
                        LinkButton lnkdlet = (LinkButton)grdGauge.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = false;
                      
                         Session["dleteGage"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void bindGaugeGrid(int custId)
    {
        try
        {
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spGaugeDetails";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 6, custId,0, "", "", "", "","");
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

                DataSet ds = q.ProcdureWith8Param(stprocedure, 7, custId, 0, searchValue, "", "", "","");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 8, custId, 0, "", searchValue, "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 9, custId,0, "", "", searchValue, "","");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Size/Range-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 10, custId, 0, "", "", "", searchValue, "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 3, custId,0, "", "", "","", searchValue);
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Part Number-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure,11, custId, 0, "", "", "",searchValue, "");
                dt = ds.Tables[0];
            }

            grdGauge.DataSource = dt;            
            grdGauge.DataBind();

            checkAuthority();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddGauge_Click(object sender, EventArgs e)
    {

     

        txtGaugeName.Focus();
        MultiView1.ActiveViewIndex = 1;

        DataTable dtcurloc = g.ReturnData("SELECT department_name FROM employee_tb as em left outer join department_tb dt on em.department_id=dt.department_id where em.employee_id=" + Convert.ToInt32(Session["Customer_ID"]) + " ");
        txtCurrentLocation.Text = dtcurloc.Rows[0]["department_name"].ToString();

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(gauge_id) from gaugeMaster_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());

            txtGaugeId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {

            txtGaugeId.Text = "1";
        }

    }
    protected void btnSaveGauge_Click(object sender, EventArgs e)
    {
        // For remove white space
        gaugeName = txtGaugeName.Text.Trim();
        gaugeName = Regex.Replace(gaugeName, @"\s+", " ");
        gaugeSrNo = txtGaugeSrNo.Text.Trim();
        gaugeSrNo = Regex.Replace(gaugeSrNo, @"\s+", " ");
        MultiView1.ActiveViewIndex = 1;

        try
        {
           
            #region File Upload
            FileUpload img = (FileUpload)UploadDrwainfFile;
            if (img.HasFile && img.PostedFile != null)
            {
                imgByte = null;
                //To create a PostedFile
                HttpPostedFile File = UploadDrwainfFile.PostedFile;
                txtImageName.Text = File.FileName;
                string filePath = UploadDrwainfFile.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                long fileSize = UploadDrwainfFile.FileContent.Length;
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
                       // File.InputStream.Read(imgByte, 0, File.ContentLength);
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

         

          
          
           
            if (btnSaveGauge.Text == "Save")
            {

                DataTable dtexist = g.ReturnData("Select gauge_sr_no from gaugeMaster_TB where gauge_sr_no='" + gaugeSrNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "");
                    
                    if (dtexist.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Gauge serial number is already exist.");
                        return;
                    }
                    else
                    {
                        saveUpdateGauge(0);
                    }
                   
                
            }
            else
            {
                int updateGaugeId = Convert.ToInt32(lblGaugeId.Text);
                DataTable dtexist = g.ReturnData("Select gauge_sr_no from gaugeMaster_TB where gauge_sr_no='" + gaugeSrNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_id=" + updateGaugeId + "");
                    
                    if (dtexist.Rows.Count > 0)
                    {
                        saveUpdateGauge(updateGaugeId);
                    }
                    else
                    {
                        DataTable dtexist1 = g.ReturnData("Select gauge_sr_no from gaugeMaster_TB where gauge_sr_no='" + gaugeSrNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_id<>" + updateGaugeId + "");
                        
                        if (dtexist1.Rows.Count > 0)
                        {
                            g.ShowMessage(this.Page, "Gauge serial number is already exist.");
                            return;
                        }
                        else
                        {
                            saveUpdateGauge(updateGaugeId);
                        }
                    }
                  

                }

          

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
        clearFields();
        MultiView1.ActiveViewIndex = 0;
        bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
    }
    private void saveUpdateGauge(int updateGaugeId)
    {
        try
        {
            string stsize="";
                if (ddlType.SelectedIndex == 1)
                {
                   stsize = txtSize.Text;
                }
                else if (ddlType.SelectedIndex == 2)
                {
                   stsize = txtRange.Text;
                } 
            DateTime purchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string purchaseDate1=purchaseDate.ToString("yyyy-MM-dd H:mm:ss"); 
             DateTime retairmentDate = DateTime.ParseExact(txtRetairementDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string retairmentDate1=retairmentDate.ToString("yyyy-MM-dd H:mm:ss"); 
            DateTime serviceDate = DateTime.ParseExact(txtServiceDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string serviceDate1=serviceDate.ToString("yyyy-MM-dd H:mm:ss"); 
                if (btnSaveGauge.Text == "Save")
                {

                    string stquery = "Insert into gaugeMaster_TB (customer_id,cycles,status,created_by_id,gauge_name,gauge_sr_no,gauge_Manufature_Id,current_location,gauge_type,go_tollerance_plus,go_tollerance_minus,go_were_limit,least_count,no_go_tollerance_plus,no_go_tollerance_minus,permisable_error1,permisable_error2,purchase_cost,purchase_date,retairment_date,service_date,resolution,size_range,store_location,drawing_name,drawing_file,sapcode_no) VALUES(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23,?param24,?param25,?param26,?param27)";
                    //Note :  customer_id= param1,cycles=param2,  status=param3,created_by_id= param4,gauge_name=param5, gauge_sr_no=param6, gauge_Manufature_Id=param7, current_location=param8, gauge_type=param9, go_tollerance_plus=param10,go_tollerance_minus=param11, go_were_limit=param12,least_count=param13, no_go_tollerance_plus=param14, no_go_tollerance_minus=param15,permisable_error1=param16,permisable_error2=param17, purchase_cost=param18, purchase_date=param19,retairment_date=param20, service_date=param21, resolution=param22,size_range= param23, store_location=param24,drawing_name=param25, drawing_file= param26
                    g.savewith27param(stquery, Convert.ToInt32(Session["Customer_ID"]) , 0 ,"1", Convert.ToInt32(Session["User_ID"]) ,gaugeName,gaugeSrNo,txtManufactureId.Text,txtCurrentLocation.Text, ddlType.SelectedItem.Text, txtGoTollerancePlus.Text,txtGoTolleranceMinus.Text,txtGoWereLimit.Text,txtLeastCount.Text,txtNoGoTollerancePlus.Text,txtNoGoTolleranceMinus.Text,txtPermisableError1.Text,txtPermisableError2.Text, Convert.ToDecimal(txtPurchaseCost.Text), purchaseDate1,retairmentDate1,serviceDate1,txtResolution.Text,stsize,txtStoreLocation.Text,txtImageName.Text,imgByte,txtsapcode.Text);
                    
                    //DataTable dtsave = g.ReturnData("Insert into gaugeMaster_TB (customer_id,cycles,status,created_by_id,gauge_name,gauge_sr_no,gauge_Manufature_Id,current_location,gauge_type,go_tollerance_plus,go_tollerance_minus,go_were_limit,least_count,no_go_tollerance_plus,no_go_tollerance_minus,permisable_error1,permisable_error2,purchase_cost,purchase_date,retairment_date,service_date,resolution,size_range,store_location,drawing_name,drawing_file) Values(" + Convert.ToInt32(Session["Customer_ID"]) + "," + 0 + ",True," + Convert.ToInt32(Session["User_ID"]) + ", '"+gaugeName+"', '"+gaugeSrNo+"','"+txtManufactureId.Text+"','"+txtCurrentLocation.Text+"', '"+ddlType.SelectedItem.Text+"','"+ txtGoTollerancePlus.Text+"', '"+txtGoTolleranceMinus.Text+"','"+txtGoWereLimit.Text+"','"+txtLeastCount.Text+"','"+txtNoGoTollerancePlus.Text+"','"+txtNoGoTolleranceMinus.Text+"','"+txtPermisableError1.Text+"','"+txtPermisableError2.Text+"', "+ Convert.ToDecimal(txtPurchaseCost.Text)+",'"+ purchaseDate1+"','"+retairmentDate1+"','"+serviceDate1+"','"+txtResolution.Text+"','"+stsize+"','"+txtStoreLocation.Text+"','"+txtImageName.Text+"','"+imgByte+"')");
                    g.ShowMessage(this.Page, "Gauge data is saved successfully.");
        
                    
                }
                else
                {
                    DataTable dtfetch = g.ReturnData("Select drawing_file from gaugeMaster_TB where gauge_id=" + updateGaugeId + "");
                    string stfile = "";
                    stfile = dtfetch.Rows[0]["drawing_file"].ToString();
                    if (stfile!="")
                    {
                        if (imgByte == null)
                        {
                            imgByte=(Byte[])dtfetch.Rows[0]["drawing_file"];
                        } 
                    }

                    string stquery = "Update gaugeMaster_TB set customer_id=?param1 ,cycles=?param2,status=?param3,created_by_id=?param4,gauge_name=?param5,gauge_sr_no=?param6,gauge_Manufature_Id=?param7,current_location=?param8,gauge_type=?param9,go_tollerance_plus=?param10,go_tollerance_minus=?param11, go_were_limit=?param12,least_count=?param13,no_go_tollerance_plus=?param14, no_go_tollerance_minus=?param15,permisable_error1=?param16,permisable_error2=?param17,purchase_cost=?param18, purchase_date=?param19,retairment_date=?param20,service_date=?param21,  resolution=?param22,size_range=?param23,  store_location=?param24,drawing_name=?param25,drawing_file=?param26,sapcode_no=?param27  where gauge_id=" + updateGaugeId + "";
                    g.savewith27param(stquery, Convert.ToInt32(Session["Customer_ID"]), 0, "1", Convert.ToInt32(Session["User_ID"]), gaugeName, gaugeSrNo, txtManufactureId.Text, txtCurrentLocation.Text, ddlType.SelectedItem.Text, txtGoTollerancePlus.Text, txtGoTolleranceMinus.Text, txtGoWereLimit.Text, txtLeastCount.Text, txtNoGoTollerancePlus.Text, txtNoGoTolleranceMinus.Text, txtPermisableError1.Text, txtPermisableError2.Text, Convert.ToDecimal(txtPurchaseCost.Text), purchaseDate1, retairmentDate1, serviceDate1, txtResolution.Text, stsize, txtStoreLocation.Text, txtImageName.Text, imgByte,txtsapcode.Text);
                   // DataTable dtupdate = g.ReturnData("Update gaugeMaster_TB set customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " ,cycles=" + 0 + ",status=True,created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",gauge_name='" + gaugeName + "',  gauge_sr_no='" + gaugeSrNo + "',gauge_Manufature_Id='" + txtManufactureId.Text + "',current_location='" + txtCurrentLocation.Text + "', gauge_type='" + ddlType.SelectedItem.Text + "',go_tollerance_plus='" + txtGoTollerancePlus.Text + "',go_tollerance_minus='" + txtGoTolleranceMinus.Text + "', go_were_limit='" + txtGoWereLimit.Text + "',least_count='" + txtLeastCount.Text + "',no_go_tollerance_plus='" + txtNoGoTollerancePlus.Text + "', no_go_tollerance_minus='" + txtNoGoTolleranceMinus.Text + "',permisable_error1='" + txtPermisableError1.Text + "',  permisable_error2='" + txtPermisableError2.Text + "',purchase_cost='" + Convert.ToDecimal(txtPurchaseCost.Text) + "', purchase_date='" + purchaseDate1 + "',retairment_date='" + retairmentDate1 + "',service_date='" + serviceDate1 + "',  resolution='" + txtResolution.Text + "',size_range='" + stsize + "',  store_location='" + txtStoreLocation.Text + "',drawing_name='" + txtImageName.Text + "',drawing_file='" + imgByte + "'  where gauge_id=" + updateGaugeId + "");
                    
                    g.ShowMessage(this.Page, "Gauge data is updated successfully.");
                }
            
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnClloseGauge_Click(object sender, EventArgs e)
    {
        clearFields();
        MultiView1.ActiveViewIndex = 0;
    }
    private void clearFields()
    {
        txtGaugeSrNo.Text = "";
        btnAddGauge.Focus();
        txtGaugeId.Text = "";
        txtGaugeName.Text = "";
        txtManufactureId.Text = "";
        ddlType.SelectedIndex = 0;
        txtGoWereLimit.Text = "";
        txtSize.Text = "";
        txtRange.Text = "";
        txtGoTollerancePlus.Text = "";
        txtGoTolleranceMinus.Text = "";
        txtNoGoTollerancePlus.Text = "";
        txtNoGoTolleranceMinus.Text = "";
        txtLeastCount.Text = "";
        txtResolution.Text = "";
        txtStoreLocation.Text = "";
        txtCurrentLocation.Text = "";
        txtPurchaseCost.Text = "";
        txtPurchaseDate.Text = "";
        txtServiceDate.Text = "";
        txtRetairementDate.Text = "";
        txtPermisableError1.Text = "";
        txtPermisableError2.Text = "";
        divSize.Visible = true;
        divRange.Visible = false;
        divLeastCount.Visible = true;
        divResolution.Visible = true;
        divGoandNoGoPlus.Visible = true;
        divGoandNoGoTolleranceminus.Visible = true;
        divPermisable1.Visible = true;
        divPermisable2.Visible = true;
        divGoWereLimit.Visible = true;
        txtImageName.Text = "";
        txtsapcode.Text = "";
        btnSaveGauge.Text = "Save";
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedIndex == 1)
        {
            divSize.Visible = true;
            divRange.Visible = false;
            divLeastCount.Visible = false;
            divResolution.Visible = false;
            divGoandNoGoPlus.Visible = true;
            divGoandNoGoTolleranceminus.Visible = true;
            divPermisable1.Visible = false;
            divPermisable2.Visible = false;
            divGoWereLimit.Visible = true;
            ddlType.Focus();
        }
        else if (ddlType.SelectedIndex == 2)
        {
            divSize.Visible = false;
            divRange.Visible = true;
            divLeastCount.Visible = true;
            divResolution.Visible = true;
            divGoandNoGoPlus.Visible = false;
            divGoandNoGoTolleranceminus.Visible = false;
            divPermisable1.Visible = true;
            divPermisable2.Visible = true;
            divGoWereLimit.Visible = false;
            ddlType.Focus();


        }
        else
        {
            divSize.Visible = true;
            divRange.Visible = false;
            divLeastCount.Visible = true;
            divResolution.Visible = true;
            divGoandNoGoPlus.Visible = true;
            divGoandNoGoTolleranceminus.Visible = true;
            divPermisable1.Visible = true;
            divPermisable2.Visible = true;
            divGoWereLimit.Visible = true;

        }
    }
    protected void btnEditGauge_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblGaugeId.Text = lnk.CommandArgument;
            string stprocedure = "spGaugeDetailsReport";
            DataSet ds = q.ProcdureWith3Param(stprocedure,2,0,Convert.ToInt32(lblGaugeId.Text));
            DataTable dtgauge = ds.Tables[0];
            txtGaugeId.Text = dtgauge.Rows[0]["gauge_id"].ToString();
            txtGaugeName.Text = dtgauge.Rows[0]["gauge_name"].ToString();
            txtGaugeSrNo.Text = dtgauge.Rows[0]["gauge_sr_no"].ToString();
            txtsapcode.Text = dtgauge.Rows[0]["sapcode_no"].ToString();
                txtManufactureId.Text =dtgauge.Rows[0]["gauge_Manufature_Id"].ToString();

                if (dtgauge.Rows[0]["gauge_type"].ToString() == "ATTRIBUTE")
                {
                    ddlType.SelectedIndex = 1;
                    divSize.Visible = true;
                    divRange.Visible = false;
                    divLeastCount.Visible = false;
                    divResolution.Visible = false;
                    divGoandNoGoPlus.Visible = true;
                    divGoandNoGoTolleranceminus.Visible = true;
                    divPermisable1.Visible = false;
                    divPermisable2.Visible = false;
                    divGoWereLimit.Visible = true;
                    txtSize.Text =dtgauge.Rows[0]["size_range"].ToString();
                }
                else if (dtgauge.Rows[0]["gauge_type"].ToString() == "VARIABLE")
                {
                    ddlType.SelectedIndex = 2;
                    divSize.Visible = false;
                    divRange.Visible = true;
                    divLeastCount.Visible = true;
                    divResolution.Visible = true;
                    divGoandNoGoPlus.Visible = false;
                    divGoandNoGoTolleranceminus.Visible = false;
                    divPermisable1.Visible = true;
                    divPermisable2.Visible = true;
                    divGoWereLimit.Visible = false;
                    txtRange.Text = dtgauge.Rows[0]["size_range"].ToString();
                }
                txtGoWereLimit.Text = dtgauge.Rows[0]["go_were_limit"].ToString(); 
                txtGoTollerancePlus.Text = dtgauge.Rows[0]["go_tollerance_plus"].ToString(); 
                txtGoTolleranceMinus.Text = dtgauge.Rows[0]["go_tollerance_minus"].ToString();
                txtNoGoTollerancePlus.Text = dtgauge.Rows[0]["no_go_tollerance_plus"].ToString(); 
                txtNoGoTolleranceMinus.Text = dtgauge.Rows[0]["no_go_tollerance_minus"].ToString();
                txtLeastCount.Text = dtgauge.Rows[0]["least_count"].ToString(); 
                txtResolution.Text = dtgauge.Rows[0]["resolution"].ToString();
                txtStoreLocation.Text = dtgauge.Rows[0]["store_location"].ToString();
                //DataTable dtcurloc = g.ReturnData("SELECT department_name FROM employee_tb as em left outer join department_tb dt on em.department_id=dt.department_id where em.employee_id=" + Convert.ToInt32(Session["Customer_ID"]) + " ");
                //txtCurrentLocation.Text = dtcurloc.Rows[0]["department_name"].ToString();
                txtCurrentLocation.Text = dtgauge.Rows[0]["current_location"].ToString(); 
                txtPurchaseCost.Text = dtgauge.Rows[0]["purchase_cost"].ToString();
                             
                txtPurchaseDate.Text =dtgauge.Rows[0]["purchase_date"].ToString();

              
                txtServiceDate.Text = dtgauge.Rows[0]["service_date"].ToString();

               
                txtRetairementDate.Text = dtgauge.Rows[0]["retairment_date"].ToString();
                txtPermisableError1.Text = dtgauge.Rows[0]["permisable_error1"].ToString();
                txtPermisableError2.Text = dtgauge.Rows[0]["permisable_error2"].ToString(); 
                txtImageName.Text = dtgauge.Rows[0]["drawing_name"].ToString(); 
                MultiView1.ActiveViewIndex = 1;
                btnSaveGauge.Text = "Update";
            
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdGauge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGauge.PageIndex = e.NewPageIndex;
        bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
    }
    protected void LnkDownLoadDocument_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int gaugeId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select drawing_file, drawing_name from gaugeMaster_TB where gauge_id='" + gaugeId + "'");

            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["drawing_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                
                byte[] bytes = (byte[])(dt.Rows[0]["drawing_file"]);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["drawing_name"].ToString());
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
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        searchBy.Text = "";
        txtsearchValue.Text = "";
        if (ddlsortby.SelectedIndex > 0)
        {
            btnSearch.Visible = true;
            lblName.Visible = true;
            txtsearchValue.Visible = true;
        }
        else
        {
            btnSearch.Visible = false;
            txtsearchValue.Visible = false;
            lblName.Text = "";
            txtsearchValue.Text = "";
            lblName.Visible = false;

        }
          if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
        {
            lblName.Text = "Gauge Id";
            searchBy.Text = "gt.gauge_id";
        }
          else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
        {
            lblName.Text = "Gauge Name";
            searchBy.Text = "gt.gauge_name";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
        {
            lblName.Text = "Gauge Sr.No.";
            searchBy.Text = "gt.gauge_sr_no";
        }
       
        else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
        {
            lblName.Text = "Manufacture Id";
            searchBy.Text = "gt.gauge_Manufature_Id";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
        {
            lblName.Text = "Gauge Type";
            searchBy.Text = "gt.gauge_type";
        }
        else if (ddlsortby.SelectedItem.Text == "Size/Range-Wise")
        {
            lblName.Text = "Size/Range";
            searchBy.Text = "gt.size_range";
        }
          else if (ddlsortby.SelectedItem.Text == "Part Number-Wise")
        {
            lblName.Text = "Part Number";
            searchBy.Text = "gt.size_range";
        }
              
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
           
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
           
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, "Some error found. " + ex.Message);
        }
    }



    protected void lnkDelete_Click(object sender, EventArgs e)
    {

        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                LinkButton lnk = (LinkButton)sender;
                string stgaugeid = lnk.CommandArgument;
                DataTable dtdeletegauge = g.ReturnData("delete from gaugeMaster_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");

                DataTable dtdeletegaugesup = g.ReturnData("delete from gauge_supplier_link_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtdeletegaugepart = g.ReturnData("delete from gauge_part_link_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtdeleteSched = g.ReturnData("delete from calibration_schedule_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtdeleteTrans = g.ReturnData("delete from calibration_transaction_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtdeletetranupd = g.ReturnData("delete from calibration_transaction_upd_tb where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
              

                DataTable dtdeletemsaSched = g.ReturnData("delete from msa_schedule_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtdeletemsatran = g.ReturnData("delete from msa_transaction_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtdeletemsatranupd = g.ReturnData("delete from msa_transactio_upd_tb where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                DataTable dtcheckissue = g.ReturnData("Select issued_id from issued_status_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
                if (dtcheckissue.Rows.Count>0)
                {
                    for (int i = 0; i < dtcheckissue.Rows.Count; i++)
                    {

                        DataTable dtdeletereturn = g.ReturnData("delete from returned_status_TB where issued_id=" + Convert.ToInt32(dtcheckissue.Rows[i]["issued_id"].ToString()) + "");
                    }
                }
                DataTable dtdeleteissue = g.ReturnData("delete from issued_status_TB where gauge_id=" + Convert.ToInt32(stgaugeid) + "");
               
                g.ShowMessage(this.Page, "Gauge Details are deleted successfully.");

                bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnMakeSupplierlink_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string strgaugeId = lnk.CommandArgument;
            if (String.IsNullOrEmpty(strgaugeId))
            {
                return;
            }
            else
            {

                Response.Redirect("~/GaugeSupplier.aspx?supplierLink_gauge_id=" + strgaugeId);
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnaddpartlink_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string strgaugeId = lnk.CommandArgument;
            if (String.IsNullOrEmpty(strgaugeId))
            {
                return;
            }
            else
            {

                Response.Redirect("~/GaugePart.aspx?gauge_id=" + strgaugeId);
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnMakeSchedulelink_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string strgaugeId = lnk.CommandArgument;
            if (String.IsNullOrEmpty(strgaugeId))
            {
                return;
            }
            else
            {

                Response.Redirect("~/CalibrationSchedule.aspx?schedule_gauge_id=" + strgaugeId);
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
}