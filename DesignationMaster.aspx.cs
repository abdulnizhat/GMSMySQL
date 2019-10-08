using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DesignationMaster : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stdesig = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                btnAddDesignation.Focus();
                filldepartment();
                bindDesignation();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void filldepartment()
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
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("DesignationMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddDesignation.Visible = true;
                }
                else
                {
                    btnAddDesignation.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdDesignation.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdDesignation.Rows[i].FindControl("btnEditDesination");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdDesignation.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdDesignation.Rows[i].FindControl("btnEditDesination");
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
    private void bindDesignation()
    {
        try
        {
            string sqlquery = q.getdesigDetails();
            DataTable dtdesig = g.ReturnData(sqlquery);
            grdDesignation.DataSource = dtdesig;
            grdDesignation.DataBind();
            // lblcnt.Text = desigdata.Count().ToString();


            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddDesignation_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        ddlDepartment.Focus();
        btnSaveDesignation.Text = "Save";

        try
        {
            DataTable dtmaxId = g.ReturnData("SELECT MAX(designation_id) FROM designation_TB");
            int maxId = Convert.ToInt32(dtmaxId.Rows[0][0].ToString());
            txtDesignationId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {

            txtDesignationId.Text = "1";
        }

    }
    protected void btnSaveDesignation_Click(object sender, EventArgs e)
    {
        try
        {
            stdesig = txtDesignationName.Text.Trim();
            stdesig = Regex.Replace(stdesig, @"\s+", " ");
            if (btnSaveDesignation.Text == "Save")
            {
                DataTable dtexist = g.ReturnData("SELECT *FROM designation_TB where department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + " and designation_name='" + stdesig + "'");

                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Designation already exist.");
                    return;
                }
                else
                {
                    DataTable dtsave = g.ReturnData("Insert into designation_TB(department_id,designation_name,status) values(" + Convert.ToInt32(ddlDepartment.SelectedValue) + ",'" + stdesig + "',True)");

                    g.ShowMessage(this.Page, "Designation is saved successfully.");

                }
            }
            else
            {
                DataTable dtexist = g.ReturnData("SELECT *FROM designation_TB where designation_id=" + Convert.ToInt32(lbldesig.Text) + " and department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + " and designation_name='" + stdesig + "'");

                if (dtexist.Rows.Count > 0)
                {
                    DataTable dtupdate = g.ReturnData("Update designation_TB set department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + ",designation_name='" + stdesig + "' where designation_id=" + Convert.ToInt32(lbldesig.Text) + "");

                    g.ShowMessage(this.Page, "Designation is updated successfully.");
                }
                else
                {
                    DataTable dtexist1 = g.ReturnData("SELECT *FROM designation_TB where designation_id<>" + Convert.ToInt32(lbldesig.Text) + " and department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + " and designation_name='" + stdesig + "'");


                    if (dtexist1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Designation details already exist");
                        return;
                    }
                    else
                    {
                        DataTable dtupdate = g.ReturnData("Update designation_TB set department_id=" + Convert.ToInt32(ddlDepartment.SelectedValue) + ",designation_name='" + stdesig + "' where designation_id=" + Convert.ToInt32(lbldesig.Text) + "");

                        g.ShowMessage(this.Page, "Designation is updated successfully.");
                    }
                }
            }
            cleardesig();
            bindDesignation();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }

    }



    private void cleardesig()
    {
        txtDesignationId.Text = "";
        txtDesignationName.Text = "";
        ddlDepartment.SelectedIndex = 0;
        MultiView1.ActiveViewIndex = 0;
        btnSaveDesignation.Text = "Save";
        btnAddDesignation.Focus();
    }
    protected void btnCloseDesignation_Click(object sender, EventArgs e)
    {
        cleardesig();
    }
    protected void btnEditDesination_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton Lnk = (LinkButton)sender;
            lbldesig.Text = Lnk.CommandArgument;

            string sqlquery = q.getdesigDetails();
            sqlquery = sqlquery + " where  designation_id=" + Convert.ToInt32(lbldesig.Text) + "";
            DataTable dtEdit = g.ReturnData(sqlquery);

            ddlDepartment.SelectedValue = dtEdit.Rows[0]["department_id"].ToString();
            txtDesignationId.Text = dtEdit.Rows[0]["designation_id"].ToString();
            txtDesignationName.Text = dtEdit.Rows[0]["designation_name"].ToString();

            btnSaveDesignation.Text = "Update";
            MultiView1.ActiveViewIndex = 1;
            ddlDepartment.Focus();


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDesignation.PageIndex = e.NewPageIndex;
        bindDesignation();
    }
}