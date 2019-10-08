using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class IssueStatus : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["dleteIssuehLink"] = "NO";
                MultiView1.ActiveViewIndex = 0;
                bindIssueGrd(Convert.ToInt32(Session["Customer_ID"]));
                fillGauge(Convert.ToInt32(Session["Customer_ID"]));
                fillEmployee(Convert.ToInt32(Session["Customer_ID"]));
                fillSupplier(Convert.ToInt32(Session["Customer_ID"]));
                fillDepartment();
                btnAddIssue.Focus();
               
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
  
    private void fillDepartment()
    {
        try
        {
            DataTable dtdepart = q.getdepartmentdetails();
            ddlDepartment.DataSource = dtdepart;

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

    private void fillSupplier(int custId)
    {
        try
        {
            DataTable dtsup = q.GetSupNameId(custId);

            ddlSuplier.DataSource = dtsup;
            ddlSuplier.DataTextField = "supplierNameAndID";
            ddlSuplier.DataValueField = "supplier_id";
            ddlSuplier.DataBind();
            ddlSuplier.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fillEmployee(int custId)
    {
        try
        {

            DataTable empdt = g.ReturnData("SELECT employee_id,concat_WS(': ID-', employee_name , employee_id) AS empnameAndID from employee_TB  where status=True and customer_id=" + custId + "");

            ddlEmployee.DataSource = empdt;
            ddlEmployee.DataTextField = "empnameAndID";
            ddlEmployee.DataValueField = "employee_id";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void bindIssueGrd(int custId)
    {
        try
        {
            grdIssueStatus.DataSource = null;
            grdIssueStatus.DataBind();
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spIssueDetails";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedIndex == 0)
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 1, custId, "", "", "", "");
                dt = ds.Tables[0];
               
            }
            if (ddlsortby.SelectedIndex == 1)
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 3, custId, "", searchValue,"", "");
                dt = ds.Tables[0];
               

            }
            else if (ddlsortby.SelectedIndex == 2)
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 4, custId, "", "", searchValue,"");
                dt = ds.Tables[0];

            }
            else if (ddlsortby.SelectedIndex == 3)
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 6, custId, searchValue, "", "","");
                dt = ds.Tables[0];

            }
            else if (ddlsortby.SelectedIndex == 4)
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 5, custId, "", "", "", searchValue);
                    dt = ds.Tables[0];
               

            }



            grdIssueStatus.DataSource = dt;
            grdIssueStatus.DataBind();

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
            childId = g.GetChildId("IssueStatus.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddIssue.Visible = true;
                }
                else
                {
                    btnAddIssue.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdIssueStatus.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdIssueStatus.Rows[i].FindControl("btnEditIssueStatus");
                        lnk.Enabled = true;
                        LinkButton lnkdlet = (LinkButton)grdIssueStatus.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = true;
                        Session["dleteIssuehLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdIssueStatus.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdIssueStatus.Rows[i].FindControl("btnEditIssueStatus");
                        lnk.Enabled = false;
                        LinkButton lnkdlet = (LinkButton)grdIssueStatus.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = false;
                        Session["dleteIssuehLink"] = "NO";
                    }
                }
            }


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
            string sqlquery = q.GetGaugeIdname(custId);
            DataTable fetchGauge = g.ReturnData(sqlquery);

            ddlGaugeName.DataSource = fetchGauge;
            ddlGaugeName.DataTextField = "gaugeNameAndID";
            ddlGaugeName.DataValueField = "gauge_id";
            ddlGaugeName.DataBind();
            ddlGaugeName.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddIssue_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        ddlGaugeName.Focus();

        try
        {
            DateTime d = new DateTime();
            d = DateTime.Now;

            txtIssueDate.Text = d.ToString("dd/MM/yyyy");
            txtIssueTime.Text = d.ToString("HH:mm");

            DataTable dtmax = g.ReturnData("Select MAX(issued_id) from issued_status_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());

            txtIssueId.Text = (maxId + 1).ToString();
           
        }
        catch (Exception)
        {
            txtIssueId.Text = "1";
           
        }

    }

    protected void btnSaveIssue_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {
            if (txtIssueDate.Text != "")
            {
                DateTime issueDate = DateTime.ParseExact(txtIssueDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime compdate = DateTime.Now;
                string strcompDate = compdate.ToString("dd/MM/yyyy");
                DateTime dt = DateTime.ParseExact(strcompDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (dt < issueDate)
                {
                    g.ShowMessage(this.Page, "Issue date is greater than current date.");
                    return;
                }
            }
            if (txtIssueDate.Text != "" && txtReturnDate.Text != "")
            {
                DateTime issueDt = DateTime.ParseExact(txtIssueDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime expReturnDt = DateTime.ParseExact(txtReturnDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (issueDt > expReturnDt)
                {
                    g.ShowMessage(this.Page, "Expected return date is less than issue date.");
                    return;
                }
            }

            if (btnSaveIssue.Text == "Save")
            {
                DataTable dtcheck = g.ReturnData("Select gauge_id from issued_status_TB where gauge_id=" + Convert.ToInt32(ddlGaugeName.SelectedValue) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and issued_status <> 'RETURNED'");

                if (dtcheck.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "This Gauge is already issued.");
                    return;

                }
                else
                {
                    saveUpdateIssue(0);
                }
            }
            else
            {
                int editId = Convert.ToInt32(lblIssuedId.Text);
                DataTable dtcheck = g.ReturnData("Select gauge_id from issued_status_TB where gauge_id=" + Convert.ToInt32(ddlGaugeName.SelectedValue) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and issued_status <> 'RETURNED' and issued_id="+editId+"");

                if (dtcheck.Rows.Count > 0)
                {
                    saveUpdateIssue(editId);
                }
                else
                {
                    DataTable dtcheck1 = g.ReturnData("Select gauge_id from issued_status_TB where gauge_id=" + Convert.ToInt32(ddlGaugeName.SelectedValue) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status=True and issued_status <> 'RETURNED' and issued_id <> " + editId + "");

                    if (dtcheck1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "This Gauge is already issued.");
                        return;

                    }
                    else
                    {
                        saveUpdateIssue(editId);
                    }
                }
            }


            clearFields();
            bindIssueGrd(Convert.ToInt32(Session["Customer_ID"]));
            MultiView1.ActiveViewIndex = 0;
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void saveUpdateIssue(int editId)
    {
        try
        {
            int supid =0;
            int empid = 0;
            int depid = 0;
            if (ddlIssueTo.SelectedIndex == 1)
	        {
		      supid= Convert.ToInt32(ddlSuplier.SelectedValue);
	        }
            else
	        {
                depid= Convert.ToInt32(ddlDepartment.SelectedValue);
                empid = Convert.ToInt32(ddlEmployee.SelectedValue);
	        }
              DateTime issueDate = DateTime.ParseExact(txtIssueDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string issueDate1=issueDate.ToString("yyyy-MM-dd H:mm:ss"); 
             DateTime returnDate = DateTime.ParseExact(txtReturnDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string returnDate1=returnDate.ToString("yyyy-MM-dd H:mm:ss"); 
            if (btnSaveIssue.Text == "Save")
            {
                DataTable dtsave = g.ReturnData("Insert into issued_status_TB (status,customer_id,created_by_id,issued_to_supplier_id, department_id,issued_to_employee_id,gauge_id,issued_status,issue_type,issued_date,date_of_return,issued_time,issued_to_type)    values(True,"+ Convert.ToInt32(Session["Customer_ID"]) +","+Convert.ToInt32(Session["User_ID"])+","+supid+","+depid+","+empid+", "+Convert.ToInt32(ddlGaugeName.SelectedValue)+",'"+ddlStatus.SelectedItem.Text+"','"+ddlIssueType.SelectedItem.Text+"', '"+issueDate1+"','"+returnDate1+"','"+txtIssueTime.Text+"','"+ddlIssueTo.SelectedItem.Text+"')");
                g.ShowMessage(this.Page, "Issue status is saved successfully.");
            }
            else
            {
                DataTable dtupd = g.ReturnData("Update issued_status_TB set status=True,customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ",created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ", issued_to_supplier_id=" + supid + ",department_id=" + depid + ",issued_to_employee_id=" + empid + ", gauge_id=" + Convert.ToInt32(ddlGaugeName.SelectedValue) + ",issued_status='" + ddlStatus.SelectedItem.Text + "',issue_type='" + ddlIssueType.SelectedItem.Text + "', issued_date='" + issueDate1 + "',date_of_return='" + returnDate1 + "',issued_time='" + txtIssueTime.Text + "',issued_to_type='" + ddlIssueTo.SelectedItem.Text + "' where issued_id="+editId+"");
                g.ShowMessage(this.Page, "Issue status is updated successfully.");
            }
               
           
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnClloseIssue_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }
    protected void ddlIssueTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIssueTo.SelectedIndex == 1)
        {
            divSupplier.Visible = true;
            //divSupplierId.Visible = true;
            //divEmpId.Visible = false;
            divDepartment.Visible = false;
            divEmployee.Visible = false;
            ddlSuplier.Focus();
        }
        else if (ddlIssueTo.SelectedIndex == 2)
        {
            divEmployee.Visible = true;
            // divEmpId.Visible = true;
            divDepartment.Visible = true;
            //divSupplierId.Visible = false;
            divSupplier.Visible = false;
            ddlEmployee.Focus();
        }
        else
        {
            // divEmpId.Visible = false;
            divDepartment.Visible = false;
            divEmployee.Visible = false;
            // divSupplierId.Visible = false;
            divSupplier.Visible = false;
            ddlIssueTo.Focus();

        }

    }
    private void clearFields()
    {
        txtIssueTime.Text =
            txtIssueDate.Text = txtIssueId.Text =
            txtReturnDate.Text = lblIssuedId.Text = string.Empty;

        ddlIssueType.SelectedIndex = 0;
        ddlIssueTo.SelectedIndex = 0;
        ddlGaugeName.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 1;
        ddlEmployee.SelectedIndex = 0;
        ddlSuplier.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        divDepartment.Visible = false;
        divEmployee.Visible = false;

        divSupplier.Visible = false;
        btnSaveIssue.Text = "Save";
        btnAddIssue.Focus();
    }
    protected void btnEditIssueStatus_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblIssuedId.Text = lnk.CommandArgument;
            DataTable dtedit = g.ReturnData("Select issued_time,issued_date,issued_id,date_of_return,issue_type,issued_to_type,issued_to_supplier_id,issued_to_employee_id,department_id, gauge_id,issued_status    from issued_status_TB where issued_id=" + Convert.ToInt32(lblIssuedId.Text) + "");

            txtIssueTime.Text = dtedit.Rows[0]["issued_time"].ToString();
            DateTime dt = Convert.ToDateTime(dtedit.Rows[0]["issued_date"].ToString());
            txtIssueDate.Text = dt.ToString("dd/MM/yyyy");
            txtIssueId.Text = dtedit.Rows[0]["issued_id"].ToString();
            DateTime dt1 = Convert.ToDateTime(dtedit.Rows[0]["date_of_return"].ToString());
            txtReturnDate.Text = dt1.ToString("dd/MM/yyyy");
            if (dtedit.Rows[0]["issue_type"].ToString() == "For CALIBRATION")
            {
                ddlIssueType.SelectedIndex = 1;
            }
            else if (dtedit.Rows[0]["issue_type"].ToString() == "For REPAIR")
            {
                ddlIssueType.SelectedIndex = 2;
            }
            else if (dtedit.Rows[0]["issue_type"].ToString() == "For USED")
            {
                ddlIssueType.SelectedIndex = 3;
            }
            else
            {
                ddlIssueType.SelectedIndex = 0;
            }
            if (dtedit.Rows[0]["issued_to_type"].ToString() == "Supplier")
            {
                ddlIssueTo.SelectedIndex = 1;
                string st = dtedit.Rows[0]["issued_to_supplier_id"].ToString();
                ddlSuplier.SelectedValue =dtedit.Rows[0]["issued_to_supplier_id"].ToString();
                divSupplier.Visible = true;
                divDepartment.Visible = false;
                divEmployee.Visible = false;
            }
            else if (dtedit.Rows[0]["issued_to_type"].ToString() == "Employee")
            {
                ddlIssueTo.SelectedIndex = 2;
                ddlEmployee.SelectedValue = dtedit.Rows[0]["issued_to_employee_id"].ToString();
                ddlDepartment.SelectedValue = dtedit.Rows[0]["department_id"].ToString();
                divSupplier.Visible = false;
                divDepartment.Visible = true;
                divEmployee.Visible = true;
            }
            else
            {
                ddlIssueTo.SelectedIndex = 0;
            }
            ddlGaugeName.SelectedValue = dtedit.Rows[0]["gauge_id"].ToString();
            if (dtedit.Rows[0]["issued_status"].ToString() == "OPEN")
            {
                ddlStatus.SelectedIndex = 1;
            }
            else if (dtedit.Rows[0]["issued_status"].ToString() == "PENDING")
            {
                ddlStatus.SelectedIndex = 2;
            }
            else
            {
                ddlStatus.SelectedIndex = 0;
            }

            MultiView1.ActiveViewIndex = 1;
            btnSaveIssue.Text = "Update";
            ddlGaugeName.Focus();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
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
                lblIssuedId.Text = lnk.CommandArgument;
                DataTable dtupd = g.ReturnData("Update issued_status_TB set status = False where issued_id=" + Convert.ToInt32(lblIssuedId.Text) + "");

                g.ShowMessage(this.Page, "Issued record is deleted successfully.");

                bindIssueGrd(Convert.ToInt32(Session["Customer_ID"]));
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdIssueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdIssueStatus.PageIndex = e.NewPageIndex;
        bindIssueGrd(Convert.ToInt32(Session["Customer_ID"]));
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

        if (ddlsortby.SelectedItem.Text == "Supplier Name-Wise")
        {
            lblName.Text = "Supplier Name";

        }
        else if (ddlsortby.SelectedItem.Text == "Employee Name-Wise")
        {
            lblName.Text = "Employee Name";

        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
        {
            lblName.Text = "Gauge Name";

        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
        {
            lblName.Text = "Gauge Sr.No.";

        }
       
        else if (ddlsortby.SelectedItem.Text == "All")
        {
            bindIssueGrd(Convert.ToInt32(Session["Customer_ID"]));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindIssueGrd(Convert.ToInt32(Session["Customer_ID"]));


        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}