using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : System.Web.UI.Page
{
    Genreal g = new Genreal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                
                txtcurrentpassword.Focus();
                fetchEmppassword();
                if (!(String.IsNullOrEmpty(txtcurrentpassword.Text.Trim())))
                {
                    txtcurrentpassword.Attributes["value"] = txtcurrentpassword.Text;
                }

            }

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void fetchEmppassword()
    {
        try
        {
            DataTable dtpassw = g.ReturnData("SELECT userMaster_TB.password FROM userMaster_TB where employee_id=" + Convert.ToInt32(Session["User_ID"]) + "");

            if (dtpassw.Rows.Count > 0)
            {

                txtcurrentpassword.Text = dtpassw.Rows[0]["password"].ToString();

            }
            else
            {
                txtcurrentpassword.Text = "";
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
             try
        {
            if (txtnewpassword.Text == txtconfpassword.Text)
            {

                DataTable existpass = g.ReturnData("SELECT userMaster_TB.password,userMaster_TB.user_master_id FROM userMaster_TB where employee_id=" + Convert.ToInt32(Session["User_ID"]) + "");


                if (existpass.Rows.Count > 0)
                {
                    DataTable dtupdate = g.ReturnData("Update userMaster_TB set userMaster_TB.password='" + txtnewpassword.Text + "' where userMaster_TB.employee_id =" + Convert.ToInt32(Session["User_ID"]) + " and userMaster_TB.user_master_id=" + Convert.ToInt32(existpass.Rows[0]["user_master_id"].ToString()) + " ");

                    if ((String.IsNullOrEmpty(txtcurrentpassword.Text.Trim())))
                    {
                        txtcurrentpassword.Attributes["value"] = "";
                    }
                    if ((String.IsNullOrEmpty(txtnewpassword.Text.Trim())))
                    {
                        txtnewpassword.Attributes["value"] = "";
                    }


                    g.ShowMessage(this.Page, "Login details updated successfully.");
                    fetchEmppassword();
                }
                else
                {
                    g.ShowMessage(this.Page, "Please Enter Correct Password");

                }
            }
            else
            {
                g.ShowMessage(this.Page, "Please Enter Correct Confirm Password");
                if (!(String.IsNullOrEmpty(txtnewpassword.Text.Trim())))
                {
                    txtnewpassword.Attributes["value"] = txtnewpassword.Text;
                }
                txtconfpassword.Focus();
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
      
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        if ((!String.IsNullOrEmpty(txtcurrentpassword.Text.Trim())))
        {
            txtcurrentpassword.Attributes["value"] = "";
        }
        if ((!String.IsNullOrEmpty(txtnewpassword.Text.Trim())))
        {
            txtnewpassword.Attributes["value"] = "";
        }
        if ((!String.IsNullOrEmpty(txtconfpassword.Text.Trim())))
        {
            txtnewpassword.Attributes["value"] = "";
        }
    }
}