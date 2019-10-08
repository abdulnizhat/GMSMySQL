using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            fillCustomer();
            txtusername.Attributes["value"] = "";
            txtpassword.Attributes["value"] = "";
        }

    }

    private void fillCustomer()
    {
        try
        {
          
            DataTable custdt = q.GetCustomerNameId();
            ddlCustomer.DataSource = custdt;
            ddlCustomer.DataTextField = "customer_name";
            ddlCustomer.DataValueField = "customer_id";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, "--Select--");
            ddlCustomer.Focus();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnlogin_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var username = ConfigurationManager.AppSettings["UserName"];
            var password = ConfigurationManager.AppSettings["password"];
            if (ddlCustomer.SelectedIndex == 0)
            {
                lblerror.Text = "Select Customer";
                lblerror.Visible = true;
                return;
            }
            else if (txtusername.Text == "")
            {
                lblerror.Text = "Enter User Name";
                lblerror.Visible = true;
                return;
            }
            else if (txtpassword.Text == "")
            {
                lblerror.Text = "Enter Password";
                lblerror.Visible = true;
                return;
            }
            else
            {
                lblerror.Visible = false;
                lblerror.Text = "";

              

                DataTable dtLoginData = g.ReturnData("SELECT userMaster_TB.customer_id, userMaster_TB.employee_id, roleMaster_TB.role FROM  userMaster_TB INNER JOIN  roleMaster_TB ON userMaster_TB.role_id = roleMaster_TB.role_id	 where userMaster_TB.user_name='"+txtusername.Text+"' and userMaster_TB.password='"+txtpassword.Text+"' and userMaster_TB.status=1");
                    if (dtLoginData.Rows.Count > 0)
                    {

                        Session["User_ID"] = dtLoginData.Rows[0]["employee_id"].ToString();
                            bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                            // Check super Admin condition
                            if (Status == true)
                            {
                                Session["Customer_ID"] = ddlCustomer.SelectedValue;
                                Session["Role"] = dtLoginData.Rows[0]["role"].ToString();

                            }
                            else
                            {
                                DataTable dtLoginData1 = g.ReturnData("SELECT userMaster_TB.customer_id, userMaster_TB.employee_id, roleMaster_TB.role FROM  userMaster_TB INNER JOIN  roleMaster_TB ON userMaster_TB.role_id = roleMaster_TB.role_id	 where userMaster_TB.user_name='" + txtusername.Text + "' and userMaster_TB.password='" + txtpassword.Text + "' and userMaster_TB.status=1 and userMaster_TB.customer_id ="+ Convert.ToInt32(ddlCustomer.SelectedValue)+"");

                                if (dtLoginData1.Rows.Count > 0)
                                {


                                    Session["User_ID"] = dtLoginData1.Rows[0]["employee_id"].ToString();
                                    Session["Customer_ID"] = dtLoginData1.Rows[0]["customer_id"].ToString(); 
                                    Session["Role"] = dtLoginData1.Rows[0]["role"].ToString();
                 
                                }
                                else
                                {
                                    g.ShowMessage(this.Page, "Enter Correct Credentials");
                                    return;
                                }
                            }
                        }

                    
                    else
                    {

                        g.ShowMessage(this.Page, "Enter Correct Credentials");
                        return;

                    }
               
                Logger.Info("LoggedIn User is " + ddlCustomer.SelectedItem.Text + ". ");
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }


    }

}