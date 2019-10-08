using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomerMaster : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string custName = "";
    string ownerName = "";
    string mobileNo = "";
    string address = "";
    Byte[] imgByte = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["dletecustLink"] = "NO";
                MultiView1.ActiveViewIndex = 0;
                bindCustomerGrid();
                bindBranch();
                bindCountry();
                btnAddnewCustomer.Focus();
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
            childId = g.GetChildId("CustomerMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddnewCustomer.Visible = true;
                }
                else
                {
                    btnAddnewCustomer.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCustomer.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCustomer.Rows[i].FindControl("btnEditCustomer");
                        lnk.Enabled = true;
                        LinkButton lnkdel = (LinkButton)grdCustomer.Rows[i].FindControl("LnkBtnDelete");
                        lnkdel.Enabled = true;
                        Session["dletecustLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdCustomer.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCustomer.Rows[i].FindControl("btnEditCustomer");
                        lnk.Enabled = false;
                        LinkButton lnkdel = (LinkButton)grdCustomer.Rows[i].FindControl("LnkBtnDelete");
                        lnkdel.Enabled = false;
                        Session["dletecustLink"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindCustomerGrid()
    {
        try
        {
            string stprocedure = "spCustomerDetails";
            
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith4Param(stprocedure, 1, "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Customer Name-Wise")
            {

                DataSet ds = q.ProcdureWith4Param(stprocedure, 2, txtsearchValue.Text,"", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Contact No.-Wise")
            {
                DataSet ds = q.ProcdureWith4Param(stprocedure, 3, "", txtsearchValue.Text,"");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Branch Name-Wise")
            {
                DataSet ds = q.ProcdureWith4Param(stprocedure, 4,"","",  txtsearchValue.Text);
                dt = ds.Tables[0];
            }
            
            grdCustomer.DataSource = dt;
            grdCustomer.DataBind();
            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindBranch()
    {
        try
        {
            DataTable dtbranch = q.getbranchdetails();

            ddlBranch.DataSource = dtbranch;
            ddlBranch.DataTextField = "branch_name";
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindCountry()
    {
        try
        {

            DataTable dtcountry = q.getcountryDetails();
            ddlCountry.DataSource = dtcountry;
            ddlCountry.DataTextField = "country_name";
            ddlCountry.DataValueField = "country_Id";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindState(int countryId)
    {
        try
        {
            string sqlquery = q.getstateDetails();
            sqlquery = sqlquery + " where stateMaster_TB.country_Id=" + countryId + " and stateMaster_TB.status=True";
            DataTable dtstate = g.ReturnData(sqlquery);

            ddlState.DataSource = dtstate;
            ddlState.DataTextField = "state_name";
            ddlState.DataValueField = "stateId";
            ddlState.DataBind();
            ddlState.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindCity(int stateId)
    {
        try
        {
            string sqlquery = q.getcityDetails();
            sqlquery = sqlquery + " and cityMaster_TB.state_Id=" + stateId + "";
            DataTable dtcity = g.ReturnData(sqlquery);

            ddlcity.DataSource = dtcity;
            ddlcity.DataTextField = "city_name";
            ddlcity.DataValueField = "city_Id";
            ddlcity.DataBind();
            ddlcity.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddnewCustomer_Click(object sender, EventArgs e)
    {
        txtcustomerName.Focus();
        MultiView1.ActiveViewIndex = 1;

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(customer_id) from customer_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtcustomerId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {
            txtcustomerId.Text = "1";
        }

    }
    protected void btnSaveCustomer_Click(object sender, EventArgs e)
    {

        custName = txtcustomerName.Text.Trim();
        custName = Regex.Replace(custName, @"\s+", " ");
        mobileNo = txtMobile.Text;
        address = txtAddress1.Text.Trim();
        address = Regex.Replace(address, @"\s+", " ");
        ownerName = txtOwnerName.Text.Trim();
        ownerName = Regex.Replace(ownerName, @"\s+", " ");

        #region File Upload
        FileUpload img = (FileUpload)fileuploadlogo;
        if (img.HasFile && img.PostedFile != null)
        {
            imgByte = null;
            //To create a PostedFile
            HttpPostedFile File = fileuploadlogo.PostedFile;
            lblImageName.Text = File.FileName;
            string filePath = fileuploadlogo.PostedFile.FileName;
            string filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            long fileSize = fileuploadlogo.FileContent.Length;
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
                    //File.InputStream.Read(imgByte, 0, File.ContentLength);
                    // imglogo.ImageUrl = filePath;

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



        if (btnSaveCustomer.Text == "Save")
        {

            try
            {
                DataTable dtexist = g.ReturnData("Select customer_name from customer_TB where mobile_no='" + mobileNo + "'");

                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Contact number already exist");
                    return;
                }
                else
                {
                    string stquery = "Insert into customer_TB (branch_id,country_id,state_id,city_id,created_by_id,status,customer_name,mobile_no,email,phone1,phone2,owner,license_no,websit,fax_no,address1,address2,pin_code,agent,logoname,logodata) Values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21)";
                    g.savewith22param(stquery, Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlCountry.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(Session["User_ID"]),"1", custName, mobileNo, txtEmail.Text, txtPhone1.Text, txtPhone2.Text, ownerName, txtLicenseNo.Text, txtWebsite.Text, txtFaxNumber.Text, address, txtAddress2.Text, txtpin.Text, txtagent.Text, lblImageName.Text, imgByte);
                    //DataTable dtsave = g.ReturnData("Insert into customer_TB (customer_name,branch_id,country_id,state_id,city_id,mobile_no,email,phone1,phone2,owner,license_no,websit,fax_no,address1,address2,created_by_id,logodata,logoname,pin_code,agent,status) Values('" + custName + "'," + Convert.ToInt32(ddlBranch.SelectedValue) + "," + Convert.ToInt32(ddlCountry.SelectedValue) + "," + Convert.ToInt32(ddlState.SelectedValue) + "," + Convert.ToInt32(ddlcity.SelectedValue) + ",'" + mobileNo + "','" + txtEmail.Text + "','" + txtPhone1.Text + "','" + txtPhone2.Text + "','" + ownerName + "','" + txtLicenseNo.Text + "','" + txtWebsite.Text + "','" + txtFaxNumber.Text + "','" + address + "','" + txtAddress2.Text + "'," + Convert.ToInt32(Session["User_ID"]) + ",'" + imgByte + "','" + lblImageName.Text + "','" + txtpin.Text + "','" + txtagent.Text + "',True)");

                   
                    g.ShowMessage(this.Page, "Customer data is saved successfully.");
                }
            }
            catch (Exception ex)
            {
                g.ShowMessage(this.Page, ex.Message);
            }

        }
        else
        {
            int editcustId = Convert.ToInt32(lblCustomerId.Text);

            DataTable dtexist = g.ReturnData("Select customer_name from customer_TB where mobile_no='" + mobileNo + "' and customer_id=" + editcustId + "");
            if (dtexist.Rows.Count > 0)
            {
                updateCustomer(editcustId);

            }
            else
            {
                DataTable dtexist1 = g.ReturnData("Select customer_name from customer_TB where mobile_no='" + mobileNo + "' and customer_id<>" + editcustId + "");

                if (dtexist1.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Contact number already exist");
                    return;

                }
                else
                {
                    updateCustomer(editcustId);
                }
            }


        }
        bindCustomerGrid();
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }

    private void updateCustomer(int custID)
    {
        try
        {
            DataTable dt = g.ReturnData("Select logodata from customer_TB where customer_id= " + Convert.ToInt32(lblCustomerId.Text) + "");
            string stfile = "";
            stfile = dt.Rows[0]["logodata"].ToString();
            if (stfile != "")
            {
                imgByte = (Byte[])dt.Rows[0]["logodata"];
            }
          
           // string stquery = "Insert into customer_TB (branch_id,country_id,state_id,city_id,created_by_id,status,customer_name,mobile_no,email,phone1,phone2,owner,license_no,websit,fax_no,address1,address2,pin_code,agent,logoname,logodata) Values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21)";

            string stquery = "Update customer_TB set branch_id=?param1,country_id=?param2,state_id=?param3,city_id=?param4,created_by_id=?param5,status=?param6,customer_name=?param7,mobile_no=?param8,email=?param9,phone1=?param10,phone2=?param11,owner=?param12,license_no=?param13,websit=?param14,fax_no=?param15,address1=?param16,address2=?param17,pin_code=?param18,agent=?param19,logoname=?param20,logodata=?param21 where customer_id=" + custID + "";
            g.savewith22param(stquery, Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlCountry.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(Session["User_ID"]), "1", custName, mobileNo, txtEmail.Text, txtPhone1.Text, txtPhone2.Text, ownerName, txtLicenseNo.Text, txtWebsite.Text, txtFaxNumber.Text, address, txtAddress2.Text, txtpin.Text, txtagent.Text, lblImageName.Text, imgByte);
                    
            //DataTable dtupd = g.ReturnData("Update customer_TB set customer_name='" + custName + "',mobile_no='" + mobileNo + "',address1='" + address + "',address2='" + txtAddress2.Text + "',fax_no='" + txtFaxNumber.Text + "',license_no='" + txtLicenseNo.Text + "',owner='" + ownerName + "',phone1='" + txtPhone1.Text + "',phone2='" + txtPhone2.Text + "',branch_id=" + Convert.ToInt32(ddlBranch.SelectedValue) + ",city_id=" + Convert.ToInt32(ddlcity.SelectedValue) + ",country_id=" + Convert.ToInt32(ddlCountry.SelectedValue) + ",state_id=" + Convert.ToInt32(ddlState.SelectedValue) + ",email='" + txtEmail.Text + "',websit='" + txtWebsite.Text + "',created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",logodata='" + imgByte + "',logoname='" + lblImageName.Text + "',pin_code='" + txtpin.Text + "',agent='" + txtagent.Text + "' where customer_id=" + custID + "");
            g.ShowMessage(this.Page, "Customer data is updated successfully.");
                      

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseCustomer_Click(object sender, EventArgs e)
    {
        clearFields();
        MultiView1.ActiveViewIndex = 0;
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCountry.Focus();
        if (ddlCountry.SelectedIndex > 0)
        {
            bindState(Convert.ToInt32(ddlCountry.SelectedValue));
        }
        else
        {
            ddlState.Items.Clear();
            ddlcity.Items.Clear();
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlState.Focus();
        if (ddlState.SelectedIndex > 0)
        {
            bindCity(Convert.ToInt32(ddlState.SelectedValue));
        }
        else
        {
            ddlcity.Items.Clear();
        }
    }
    protected void btnEditCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkEmpEdit = (LinkButton)sender;
            lblCustomerId.Text = lnkEmpEdit.CommandArgument;
            DataTable dtcust = g.GetCustomerDetails(Convert.ToInt32(lblCustomerId.Text));
                
           txtcustomerId.Text = dtcust.Rows[0]["customer_id"].ToString();
            txtcustomerName.Text = dtcust.Rows[0]["customer_name"].ToString();
            ddlBranch.SelectedValue = dtcust.Rows[0]["branch_id"].ToString();
            ddlCountry.SelectedValue = dtcust.Rows[0]["country_id"].ToString();
            bindState(Convert.ToInt32(ddlCountry.SelectedValue));
            ddlState.SelectedValue = dtcust.Rows[0]["state_id"].ToString();
            bindCity(Convert.ToInt32(ddlState.SelectedValue));
            ddlcity.SelectedValue = dtcust.Rows[0]["city_id"].ToString();
            txtMobile.Text = dtcust.Rows[0]["mobile_no"].ToString();
            txtAddress1.Text = dtcust.Rows[0]["address1"].ToString();
            txtAddress2.Text = dtcust.Rows[0]["address2"].ToString();
            txtEmail.Text = dtcust.Rows[0]["email"].ToString();
            txtOwnerName.Text = dtcust.Rows[0]["owner"].ToString();
            txtLicenseNo.Text = dtcust.Rows[0]["license_no"].ToString();
            txtPhone1.Text = dtcust.Rows[0]["phone1"].ToString();
            txtPhone2.Text = dtcust.Rows[0]["phone2"].ToString();
            txtWebsite.Text = dtcust.Rows[0]["websit"].ToString();
            txtFaxNumber.Text = dtcust.Rows[0]["fax_no"].ToString();
            lblImageName.Text = dtcust.Rows[0]["logoname"].ToString();
            txtpin.Text = dtcust.Rows[0]["pin_code"].ToString();
            txtagent.Text=dtcust.Rows[0]["agent"].ToString();
           
             
            MultiView1.ActiveViewIndex = 1;
            btnSaveCustomer.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void LnkBtnDelete_Click(object sender, EventArgs e)
    {

        string confirmValue = Request.Form["confirm_value"];
        if (confirmValue == "Yes")
        {
            try
            {
                LinkButton lnkEmpEdit = (LinkButton)sender;
                lblCustomerId.Text = lnkEmpEdit.CommandArgument;
                DataTable dtupd = g.ReturnData("Update customer_TB set status=False where customer_id=" + Convert.ToInt32(lblCustomerId.Text) + "");
                g.ShowMessage(this.Page, "Customer data is deleted successfully.");

                bindCustomerGrid();
            }
            catch (Exception ex)
            {
                g.ShowMessage(this.Page, ex.Message);

            }

        }

    }
    private void clearFields()
    {
        btnAddnewCustomer.Focus();
        txtcustomerName.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtLicenseNo.Text = "";
        txtOwnerName.Text = "";
        txtPhone1.Text = "";
        txtPhone2.Text = "";
        txtWebsite.Text = "";
        txtFaxNumber.Text = "";
        txtEmail.Text = "";
        lblCustomerId.Text = "";
        txtMobile.Text = "";
        lblImageName.Text = "";
        txtpin.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlBranch.SelectedIndex = 0;
        ddlState.Items.Clear();
        ddlcity.Items.Clear();
        btnSaveCustomer.Text = "Save";
    }
    protected void grdCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCustomer.PageIndex = e.NewPageIndex;
        bindCustomerGrid();
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
        if (ddlsortby.SelectedItem.Text == "Customer Name-Wise")
        {
            lblName.Text = "Customer Name";
        }
        else if (ddlsortby.SelectedItem.Text == "Contact No.-Wise")
        {
            lblName.Text = "Contact No.";
        }
        else if (ddlsortby.SelectedItem.Text == "Branch Name-Wise")
        {
            lblName.Text = "Branch Name";
        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindCustomerGrid();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindCustomerGrid();

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}