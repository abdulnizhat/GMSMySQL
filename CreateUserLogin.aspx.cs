using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateUserLogin : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string strusername = "";
    string strpassword = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                fillCustomer();
                fillusertype();
                bindUserLoginDetails();
                btnAddUser.Focus();
                if (!(String.IsNullOrEmpty(txtpassword.Text.Trim())))
                {
                    txtpassword.Attributes["value"] = txtpassword.Text;
                }
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
            childId = g.GetChildId("CreateUserLogin.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddUser.Visible = true;
                }
                else
                {
                    btnAddUser.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdUser.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdUser.Rows[i].FindControl("btnEditUser");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdUser.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdUser.Rows[i].FindControl("btnEditUser");
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
    private void bindUserLoginDetails()
    {
        try
        {
            DataTable dtuser = g.ReturnData("SELECT userMaster_TB.user_master_id, userMaster_TB.user_name, userMaster_TB.password,case when (userMaster_TB.status=True) then 'Active' else 'InActive' End AS status,employee_TB.employee_name,customer_TB.customer_name, roleMaster_TB.role FROM userMaster_TB INNER JOIN roleMaster_TB ON userMaster_TB.role_id = roleMaster_TB.role_id INNER JOIN customer_TB ON userMaster_TB.customer_id = customer_TB.customer_id INNER JOIN employee_TB ON userMaster_TB.employee_id = employee_TB.employee_id");
            grdUser.DataSource = dtuser;
            grdUser.DataBind();

            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }

    }

    private void fillCustomer()
    {
        try
        {
            DataTable dtcust = q.GetCustomerNameId();
            ddlcustomer.DataSource = dtcust;
            ddlcustomer.DataTextField = "customer_name";
            ddlcustomer.DataValueField = "customer_id";
            ddlcustomer.DataBind();
            ddlcustomer.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }

    }
    protected void ddlcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcustomer.Focus();
        try
        {
            if (ddlcustomer.SelectedIndex > 0)
            {
                fillemployee(Convert.ToInt32(ddlcustomer.SelectedValue));
            }
            else
            {
                ddlemployee.Items.Clear();
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }

    }

    private void fillemployee(int custId)
    {
        try
        {
            if (ddlcustomer.SelectedIndex > 0)
            {

                DataTable dtemp = q.GetEmpNameId(custId);
                ddlemployee.DataSource = dtemp;
                ddlemployee.DataTextField = "employee_name";
                ddlemployee.DataValueField = "employee_id";
                ddlemployee.DataBind();

            }
            else
            {
                ddlemployee.DataSource = null;
                ddlemployee.DataBind();
            }
            ddlemployee.Items.Insert(0, "--Select--");
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }


    private void fillusertype()
    {
        try
        {

            DataTable dtuser = g.ReturnData("Select role_id,role from roleMaster_TB where status=True");
            ddluserType.DataSource = dtuser;
            ddluserType.DataTextField = "role";
            ddluserType.DataValueField = "role_id";
            ddluserType.DataBind();
            ddluserType.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }

    }


    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        txtpassword.ReadOnly = false;
        if ((String.IsNullOrEmpty(txtpassword.Text.Trim())))
        {
            txtpassword.Attributes["value"] = "";
        }

        ddlcustomer.Focus();
    }
    protected void btnEditUser_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lbluserid.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            DataTable dtuser = g.ReturnData("SELECT user_master_id,user_name,customer_id,employee_id, password,role_id,status FROM userMaster_TB where user_master_id =" + Convert.ToInt32(lbluserid.Text) + " ");

            ddlcustomer.SelectedValue = dtuser.Rows[0]["customer_id"].ToString();
            fillemployee(Convert.ToInt32(ddlcustomer.SelectedValue));
            ddlemployee.SelectedValue = dtuser.Rows[0]["employee_id"].ToString();
            txtusername.Text = dtuser.Rows[0]["user_name"].ToString();
            if ((String.IsNullOrEmpty(txtpassword.Text.Trim())))
            {
                txtpassword.Attributes["value"] = dtuser.Rows[0]["password"].ToString();
            }
            txtpassword.ReadOnly = true;
            // txtpassword.Text =;
            ddluserType.SelectedValue = dtuser.Rows[0]["role_id"].ToString();
            if (Convert.ToBoolean(dtuser.Rows[0]["status"].ToString()) == true)
            {
                rd_status.SelectedIndex = 0;
            }
            else
            {
                rd_status.SelectedIndex = 1;
            }
            btnSaveUser.Text = "Update";

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }


    }
    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        try
        {
            string dtdate;
           
            DateTime date = DateTime.Now;

            dtdate = date.ToString("yyyy-MM-dd H:mm:ss");  
            strusername = txtusername.Text.Trim();
            strusername = Regex.Replace(strusername, @"\s+", " ");
            bool statusb = false;
            if (rd_status.SelectedIndex == 0)
            {
                statusb = true;
            }
            else
            {
                statusb = false;
            }
            if (btnSaveUser.Text == "Save")
            {
                DataTable dtexist = g.ReturnData("Select customer_id,employee_id,user_name,password,role_id from userMaster_TB where customer_id=" + Convert.ToInt32(ddlcustomer.SelectedValue) + " and employee_id =" + Convert.ToInt32(ddlemployee.SelectedValue) + "");
                if (dtexist.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "User name already exist");
                        return;
                    }
                  
                    else
                    {

                         DataTable dtexistcust = g.ReturnData("Select customer_id,employee_id,user_name,password,role_id from userMaster_TB where user_name='" + strusername + "'");
                        
                        if (dtexistcust.Rows.Count > 0)
                        {
                            g.ShowMessage(this.Page, "User name already exist");
                            return;
                        }
                        else
                        {
                          
                          DataTable dtsave = g.ReturnData("Insert into userMaster_TB (customer_id,employee_id,user_name,password,role_id,created_by_id,updated_date,status) values (" + Convert.ToInt32(ddlcustomer.SelectedValue) + "," + Convert.ToInt32(ddlemployee.SelectedValue) + ",'" + strusername + "','" + txtpassword.Text + "'," + Convert.ToInt32(ddluserType.SelectedValue) + "," + Convert.ToInt32(Session["User_ID"]) + ", '" + dtdate + "'," + statusb + ")");
                            
                            g.ShowMessage(this.Page, "User Login data saved successfully.");
                        }
                    }
                
            }
            else
            {
                DataTable dtexist = g.ReturnData("Select customer_id,employee_id,user_name,password,role_id from userMaster_TB where customer_id=" + Convert.ToInt32(ddlcustomer.SelectedValue) + " and employee_id =" + Convert.ToInt32(ddlemployee.SelectedValue) + " and user_master_id<>"+Convert.ToInt32(lbluserid.Text)+" ");
                
                    if (dtexist.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "User name already exist");
                        return;
                    }
                    else
                    {
                         DataTable dtexistcust = g.ReturnData("Select customer_id,employee_id,user_name,password,role_id from userMaster_TB where user_name='" + strusername + "' and user_master_id<>"+Convert.ToInt32(lbluserid.Text)+"");
                       

                        if (dtexistcust.Rows.Count > 0)
                        {
                            g.ShowMessage(this.Page, "User name already exist");
                            return;
                        }
                        else
                        {
                            DataTable dtupdate = g.ReturnData("Update userMaster_TB set customer_id=" + Convert.ToInt32(ddlcustomer.SelectedValue) + ",employee_id=" + Convert.ToInt32(ddlemployee.SelectedValue) + ",user_name='" + strusername + "',role_id=" + Convert.ToInt32(ddluserType.SelectedValue) + ",created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",updated_date='" + dtdate + "',status=" + statusb + " where user_master_id=" + Convert.ToInt32(lbluserid.Text) + "");
                            
                            g.ShowMessage(this.Page, "User Login data Updated successfully.");
                        }
                       
                    }
               
            }
            clearallfields();
            bindUserLoginDetails();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseUser_Click(object sender, EventArgs e)
    {
        clearallfields();
    }

    private void clearallfields()
    {
        ddlcustomer.SelectedIndex = 0;
        ddlemployee.Items.Clear();
        txtusername.Text = "";
        if ((String.IsNullOrEmpty(txtpassword.Text.Trim())))
        {
            txtpassword.Attributes["value"] = "";
        }
        ddluserType.SelectedIndex = 0;
        rd_status.SelectedIndex = 0;
        btnSaveUser.Text = "Save";
        MultiView1.ActiveViewIndex = 0;
        btnAddUser.Focus();
    }


    protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
        bindUserLoginDetails();
    }
}