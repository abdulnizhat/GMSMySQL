using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmployeeMaster : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string empName = "";
    string mobileNo = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["dleteempLink"] = "NO";
                MultiView1.ActiveViewIndex = 0;
                txtEmployeeName.Focus();
                bindEmployeeGrid(Convert.ToInt32(Session["Customer_ID"]));
                bindDepartment();
                bindBranch();
                bindCountry();
                btnAddEmployee.Focus();


            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void bindDepartment()
    {

        try
        {
            DataTable dtdept = q.getdepartmentdetails();
            ddlDepartment.DataSource = dtdept;
            ddlDepartment.DataTextField = "department_name";
            ddlDepartment.DataValueField = "department_id";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, "--Select--");

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

            ddlCity.DataSource = dtcity;
            ddlCity.DataTextField = "city_name";
            ddlCity.DataValueField = "city_Id";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, "--Select--");

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
            childId = g.GetChildId("EmployeeMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddEmployee.Visible = true;
                }
                else
                {
                    btnAddEmployee.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdEmployee.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdEmployee.Rows[i].FindControl("btnEditEmployee");
                        LinkButton lnkdlet = (LinkButton)grdEmployee.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = true;
                        lnk.Enabled = true;
                        Session["dleteempLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdEmployee.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdEmployee.Rows[i].FindControl("btnEditEmployee");
                        lnk.Enabled = false;
                        LinkButton lnkdlet = (LinkButton)grdEmployee.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = false;
                        Session["dleteempLink"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindEmployeeGrid(int custId)
    
    {
        try
        {
            string stprocedure = "spEmployeeDetails";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith4Param(stprocedure,1, custId,"","");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Employee Name-Wise")
            {

                DataSet ds = q.ProcdureWith4Param(stprocedure, 2, custId, txtsearchValue.Text, "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Mobile No.-Wise")
            {
                DataSet ds = q.ProcdureWith4Param(stprocedure, 3, custId,"", txtsearchValue.Text);
                dt = ds.Tables[0];
            }


            grdEmployee.DataSource = dt;
            grdEmployee.DataBind();

            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddEmployee_Click(object sender, EventArgs e)
    {
        txtEmployeeName.Focus();
        MultiView1.ActiveViewIndex = 1;

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(employee_id) from employee_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtEmployeeId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {

            txtEmployeeId.Text = "1";
        }

        txtEmployeeName.ReadOnly = false;

        txtMobileNo.ReadOnly = false;
        txtAddress.ReadOnly = false;
        txtEmail.ReadOnly = false;
    }
    protected void btnSaveEmployee_Click(object sender, EventArgs e)
    {
        // For remove white space
        empName = txtEmployeeName.Text.Trim();
        empName = Regex.Replace(empName, @"\s+", " ");
        mobileNo = txtMobileNo.Text;
        if (btnSaveEmployee.Text == "Save")
        {


            DataTable dtcheck = g.ReturnData("Select employee_id from employee_TB where mobile_no='" + mobileNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "");

            if (dtcheck.Rows.Count > 0)
            {
                g.ShowMessage(this.Page, "Mobile number is already exist");
                return;
            }
            else
            {
                bool issupplier = false;
                if (rbtIsSupplier.SelectedIndex == 0)
                {
                    issupplier = false;
                    DataTable dtsave = g.ReturnData("Insert into employee_TB (employee_name,mobile_no,address,branch_id,city_id,state_id,country_id,customer_id,department_id,designation_id,email,status,created_by_id,is_supplier) Values('" + empName + "','" + mobileNo + "','" + txtAddress.Text + "'," + Convert.ToInt32(ddlBranch.SelectedValue) + "," + Convert.ToInt32(ddlCity.SelectedValue) + "," + Convert.ToInt32(ddlState.SelectedValue) + "," + Convert.ToInt32(ddlCountry.SelectedValue) + "," + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(ddlDepartment.SelectedValue) + "," + Convert.ToInt32(ddlDesignation.SelectedValue) + ",'" + txtEmail.Text + "',True," + Convert.ToInt32(Session["User_ID"]) + "," + issupplier + " )");
                }
                else if (rbtIsSupplier.SelectedIndex == 1)
                {
                    issupplier = true;
                    DataTable dtsave = g.ReturnData("Insert into employee_TB (employee_name,mobile_no,address,branch_id,city_id,state_id,country_id,customer_id,department_id,designation_id,email,status,created_by_id,is_supplier,supplier_id) Values('" + empName + "','" + mobileNo + "','" + txtAddress.Text + "'," + Convert.ToInt32(ddlBranch.SelectedValue) + "," + Convert.ToInt32(ddlCity.SelectedValue) + "," + Convert.ToInt32(ddlState.SelectedValue) + "," + Convert.ToInt32(ddlCountry.SelectedValue) + "," + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(ddlDepartment.SelectedValue) + "," + Convert.ToInt32(ddlDesignation.SelectedValue) + ",'" + txtEmail.Text + "',True," + Convert.ToInt32(Session["User_ID"]) + "," + issupplier + "," + Convert.ToInt32(ddlSupplier.SelectedValue) + " )");

                }


                g.ShowMessage(this.Page, "Employee data is saved successfully.");
            }

        }
        else
        {
            int editEmpId = Convert.ToInt32(lblEmpID.Text);


            DataTable dtcheck = g.ReturnData("Select employee_id from employee_TB where mobile_no='" + mobileNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and employee_id=" + editEmpId + "");

            if (dtcheck.Rows.Count > 0)
            {
                updateEmployee(editEmpId);

            }
            else
            {
                DataTable dtcheck1 = g.ReturnData("Select employee_id from employee_TB where mobile_no='" + mobileNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and employee_id<>" + editEmpId + "");

                if (dtcheck1.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Mobile number is already exist");
                    return;

                }
                else
                {

                    updateEmployee(editEmpId);
                }
            }


        }
        bindEmployeeGrid(Convert.ToInt32(Session["Customer_ID"]));
        clearFields();
    }
    private void updateEmployee(int EmpID)
    {
        try
        {
            bool issupplier = false;
            if (rbtIsSupplier.SelectedIndex == 0)
            {
                issupplier = false;
                DataTable dtupdate = g.ReturnData("Update employee_TB set employee_name='" + empName + "',mobile_no='" + mobileNo + "',address='" + txtAddress.Text + "',branch_id=" + Convert.ToInt32(ddlBranch.SelectedValue) + ",city_id=" + Convert.ToInt32(ddlCity.SelectedValue) + ",state_id=" + Convert.ToInt32(ddlState.SelectedValue) + ",country_id=" + Convert.ToInt32(ddlCountry.SelectedValue) + ",department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + ",designation_id=" + Convert.ToInt32(ddlDesignation.SelectedValue) + ",email='" + txtEmail.Text + "',created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",is_supplier=" + issupplier + " Where employee_id=" + EmpID + "");
            }
            else if (rbtIsSupplier.SelectedIndex == 1)
            {
                issupplier = true;
                //em.supplier_id = Convert.ToInt32(ddlSupplier.SelectedValue);
                DataTable dtupdate = g.ReturnData("Update employee_TB set employee_name='" + empName + "',mobile_no='" + mobileNo + "',address='" + txtAddress.Text + "',branch_id=" + Convert.ToInt32(ddlBranch.SelectedValue) + ",city_id=" + Convert.ToInt32(ddlCity.SelectedValue) + ",state_id=" + Convert.ToInt32(ddlState.SelectedValue) + ",country_id=" + Convert.ToInt32(ddlCountry.SelectedValue) + ",department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + ",designation_id=" + Convert.ToInt32(ddlDesignation.SelectedValue) + ",email='" + txtEmail.Text + "',created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",is_supplier=" + issupplier + ",supplier_id=" + Convert.ToInt32(ddlSupplier.SelectedValue) + " Where employee_id=" + EmpID + "");
            }


            g.ShowMessage(this.Page, "Employee data is updated successfully.");

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseEmployee_Click(object sender, EventArgs e)
    {
        clearFields();
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
            ddlCity.Items.Clear();
        }
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
            ddlCity.Items.Clear();
        }
    }
    protected void btnEditEmployee_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnkEmpEdit = (LinkButton)sender;
            lblEmpID.Text = lnkEmpEdit.CommandArgument;
            string sqlquery = q.getEmpDetails();
            sqlquery = sqlquery + " where employee_id=" + Convert.ToInt32(lblEmpID.Text) + "";
            DataTable dtedit = g.ReturnData(sqlquery);

            txtEmployeeId.Text = dtedit.Rows[0]["employee_id"].ToString();
            txtEmployeeName.Text = dtedit.Rows[0]["employee_name"].ToString();
            ddlBranch.SelectedValue = dtedit.Rows[0]["branch_id"].ToString();
            ddlCountry.SelectedValue = dtedit.Rows[0]["country_id"].ToString();
            bindState(Convert.ToInt32(ddlCountry.SelectedValue));

            ddlState.SelectedValue = dtedit.Rows[0]["state_id"].ToString();
            bindCity(Convert.ToInt32(ddlState.SelectedValue));
            ddlCity.SelectedValue = dtedit.Rows[0]["city_id"].ToString();
            txtMobileNo.Text = dtedit.Rows[0]["mobile_no"].ToString();
            txtAddress.Text = dtedit.Rows[0]["address"].ToString();
            txtEmail.Text = dtedit.Rows[0]["email"].ToString();
            ddlDepartment.SelectedValue = dtedit.Rows[0]["department_id"].ToString();
            bindDesignation(Convert.ToInt32(ddlDepartment.SelectedValue));
            ddlDesignation.SelectedValue = dtedit.Rows[0]["designation_id"].ToString();

            string issupplier = dtedit.Rows[0]["is_supplier"].ToString();
            if (issupplier == "True")
            {
                fillsupplier(Convert.ToInt32(Session["Customer_ID"]));
                ddlSupplier.SelectedValue = dtedit.Rows[0]["supplier_id"].ToString();
                rbtIsSupplier.SelectedIndex = 1;
                divSupplier.Visible = true;
                txtEmployeeName.ReadOnly = true;
                txtMobileNo.ReadOnly = true;
                txtAddress.ReadOnly = true;
                txtEmail.ReadOnly = true;
            }
            else
            {

                rbtIsSupplier.SelectedIndex = 0;
                divSupplier.Visible = false;
                txtEmployeeName.ReadOnly = false;
                txtMobileNo.ReadOnly = false;
                txtAddress.ReadOnly = false;
                txtEmail.ReadOnly = false;
            }
            btnSaveEmployee.Text = "Update";
            MultiView1.ActiveViewIndex = 1;

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDepartment.Focus();
        if (ddlDepartment.SelectedIndex > 0)
        {
            bindDesignation(Convert.ToInt32(ddlDepartment.SelectedValue));
        }
    }
    private void bindDesignation(int depId)
    {
        try
        {
            string sqlquery = q.getdesigDetails();
            sqlquery = sqlquery + " where DET.department_id=" + depId + " and DET.status=True";
            DataTable dtdesig = g.ReturnData(sqlquery);

            ddlDesignation.DataSource = dtdesig;
            ddlDesignation.DataTextField = "designation_name";
            ddlDesignation.DataValueField = "designation_id";
            ddlDesignation.DataBind();
            ddlDesignation.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void clearFields()
    {
        txtEmployeeName.Text = "";
        txtAddress.Text = "";
        txtEmail.Text = "";
        txtMobileNo.Text = "";
        ddlDepartment.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        ddlBranch.SelectedIndex = 0;
        ddlDesignation.Items.Clear();
        ddlState.Items.Clear();
        ddlCity.Items.Clear();
        MultiView1.ActiveViewIndex = 0;
        btnSaveEmployee.Text = "Save";

        if (rbtIsSupplier.SelectedIndex == 1)
        {
            ddlSupplier.SelectedIndex = 0;
        }
        rbtIsSupplier.SelectedIndex = 0;

        btnAddEmployee.Focus();
        divSupplier.Visible = false;
    }

    private void clearFields1()
    {
        txtEmployeeName.Text = "";
        txtAddress.Text = "";
        txtEmail.Text = "";
        txtMobileNo.Text = "";
        ddlDepartment.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        ddlBranch.SelectedIndex = 0;
        ddlDesignation.Items.Clear();
        ddlState.Items.Clear();
        ddlCity.Items.Clear();
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string confirmValue = Request.Form["confirm_value"];
        if (confirmValue == "Yes")
        {
            try
            {
                LinkButton lnkEmpEdit = (LinkButton)sender;
                lblEmpID.Text = lnkEmpEdit.CommandArgument;

                DataTable dtupdate = g.ReturnData("Update employee_TB set status=False where employee_id=" + Convert.ToInt32(lblEmpID.Text) + "");

                g.ShowMessage(this.Page, "Employee data is deleted successfully.");

                bindEmployeeGrid(Convert.ToInt32(Session["Customer_ID"]));
            }
            catch (Exception ex)
            {
                g.ShowMessage(this.Page, ex.Message);

            }

        }
    }
    protected void grdEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdEmployee.PageIndex = e.NewPageIndex;
        bindEmployeeGrid(Convert.ToInt32(Session["Customer_ID"]));
    }
    protected void rbtIsSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtIsSupplier.SelectedItem.Text == "No")
        {
            divSupplier.Visible = false;
            txtEmployeeName.ReadOnly = false;

            txtMobileNo.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtEmail.ReadOnly = false;
            clearFields1();

        }
        else if (rbtIsSupplier.SelectedItem.Text == "Yes")
        {
            clearFields1();
            divSupplier.Visible = true;
            fillsupplier(Convert.ToInt32(Session["Customer_ID"]));

        }
    }

    private void fillsupplier(int custId)
    {
        try
        {
            DataTable dtsupplier = q.GetSupNameId(custId);

            ddlSupplier.DataSource = dtsupplier;
            ddlSupplier.DataTextField = "supplierNameAndID";
            ddlSupplier.DataValueField = "supplier_id";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSupplier.SelectedIndex > 0)
        {
            int supId = Convert.ToInt32(ddlSupplier.SelectedValue);
            fillSupplierDetails(supId);
        }
        else
        {
            clearFields1();
        }

    }

    private void fillSupplierDetails(int supplerId)
    {
        try
        {
            string sqlquery = q.getSupllierDetails();
            sqlquery = sqlquery + " and supplier_TB.supplier_id=" + Convert.ToInt32(supplerId) + "";
            DataTable dtsupplier = g.ReturnData(sqlquery);

            txtEmployeeName.Text = dtsupplier.Rows[0]["supplier_name"].ToString();
            txtAddress.Text = dtsupplier.Rows[0]["address1"].ToString();
            txtEmail.Text = dtsupplier.Rows[0]["email_id"].ToString();
            txtMobileNo.Text = dtsupplier.Rows[0]["mobile_no"].ToString();
            ddlBranch.SelectedValue = dtsupplier.Rows[0]["branch_id"].ToString();
            ddlCountry.SelectedValue = dtsupplier.Rows[0]["country_id"].ToString();
            bindState(Convert.ToInt32(ddlCountry.SelectedValue));
            ddlState.SelectedValue = dtsupplier.Rows[0]["state_id"].ToString();
            bindCity(Convert.ToInt32(ddlState.SelectedValue));
            ddlCity.SelectedValue = dtsupplier.Rows[0]["city_id"].ToString();

            txtEmployeeName.ReadOnly = true;
            txtMobileNo.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtEmail.ReadOnly = true;


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
        if (ddlsortby.SelectedItem.Text == "Employee Name-Wise")
        {
            lblName.Text = "Employee Name";
        }
        else if (ddlsortby.SelectedItem.Text == "Mobile No.-Wise")
        {
            lblName.Text = "Mobile No.";
        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindEmployeeGrid(Convert.ToInt32(Session["Customer_ID"]));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindEmployeeGrid(Convert.ToInt32(Session["Customer_ID"]));


        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}