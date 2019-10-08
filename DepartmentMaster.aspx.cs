using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepartmentMaster : System.Web.UI.Page
{

    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stdepartment = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {

                MultiView1.ActiveViewIndex = 0;
                bindDeptGrid();
                btnAddDepartment.Focus();

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
            childId = g.GetChildId("DepartmentMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddDepartment.Visible = true;
                }
                else
                {
                    btnAddDepartment.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdDepartment.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdDepartment.Rows[i].FindControl("btnEditDepartment");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdDepartment.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdDepartment.Rows[i].FindControl("btnEditDepartment");
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
    private void bindDeptGrid()
    {
        try
        {
            DataTable dtdepart = q.getdepartmentdetails();
            grdDepartment.DataSource = dtdepart;
            grdDepartment.DataBind();


            //  lblcnt.Text = result.Count().ToString();

            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveDepartment_Click(object sender, EventArgs e)
    {
        try
        {
            stdepartment = txtDepatmentName.Text.Trim();
            stdepartment = Regex.Replace(stdepartment, @"\s+", " ");

            if (btnSaveDepartment.Text == "Save")
            {
                DataTable dtexist = g.ReturnData("SELECT department_id,department_name FROM department_TB where department_name='" + stdepartment + "'");

                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Department name is already exist.");
                    return;
                }
                else
                {
                    DataTable dtsave = g.ReturnData("Insert into department_TB(department_name,status) values('" + stdepartment + "',True)");

                    g.ShowMessage(this.Page, "Department is saved successfully.");
                }
            }
            else
            {
                DataTable dtexist = g.ReturnData("SELECT department_id,department_name FROM department_TB where department_name='" + stdepartment + "' and department_id<>" + Convert.ToInt32(lbldeptid.Text) + "");

                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Department name is already exist.");
                    return;
                }
                else
                {
                    DataTable dtupdate = g.ReturnData("Update department_TB set department_name='" + stdepartment + "'  where department_id=" + Convert.ToInt32(lbldeptid.Text) + "");

                    g.ShowMessage(this.Page, "Department is updated successfully.");
                }
            }

            bindDeptGrid();
            MultiView1.ActiveViewIndex = 0;
            txtDepatmentId.Text = "";
            txtDepatmentName.Text = "";
            btnAddDepartment.Focus();


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }

    }
    protected void btnCloseDept_Click(object sender, EventArgs e)
    {
        txtDepatmentId.Text = "";
        txtDepatmentName.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddDepartment.Focus();
    }
    protected void btnAddDepartment_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveDepartment.Text = "Save";
        txtDepatmentName.Focus();
        try
        {
            DataTable dtmaxid = g.ReturnData("SELECT Max(department_id) FROM department_TB");
            int maxid = Convert.ToInt32(dtmaxid.Rows[0][0].ToString());
            txtDepatmentId.Text = (maxid + 1).ToString();
        }
        catch (Exception)
        {

            txtDepatmentId.Text = "1";
        }

    }
    protected void btnEditDepartment_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lbldeptid.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtDepatmentName.Focus();

            DataTable dtedit = g.ReturnData("SELECT department_name,department_id FROM department_TB where department_id=" + Convert.ToInt32(lbldeptid.Text) + "");
            txtDepatmentName.Text = dtedit.Rows[0]["department_name"].ToString();
            txtDepatmentId.Text = dtedit.Rows[0]["department_id"].ToString();   
            btnSaveDepartment.Text = "Update";
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDepartment.PageIndex = e.NewPageIndex;
        bindDeptGrid();
    }
}