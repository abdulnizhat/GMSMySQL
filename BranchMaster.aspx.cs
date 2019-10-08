using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BranchMaster : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string branchName = "";
    string branchCode = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindBranchGrid();
                btnAddnewBranch.Focus();

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
            childId = g.GetChildId("BranchMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddnewBranch.Visible = true;
                }
                else
                {
                    btnAddnewBranch.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdBranch.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdBranch.Rows[i].FindControl("btnEdit");

                        lnk.Enabled = true;

                    }
                }
                else
                {
                    for (int i = 0; i < grdBranch.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdBranch.Rows[i].FindControl("btnEdit");
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
    private void bindBranchGrid()
    {
        try
        {
            DataTable dtbranch = q.getbranchdetails();

            if (dtbranch.Rows.Count > 0)
            {
                grdBranch.DataSource = dtbranch;
                grdBranch.DataBind();
            }
            else
            {
                grdBranch.DataSource = null;
                grdBranch.DataBind();
            }

            checkAuthority();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddnewBranch_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        txtBranch.Focus();

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(branch_id) from branch_TB");
            int maxid = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtBranchId.Text = (maxid + 1).ToString();
        }
        catch (Exception)
        {

            txtBranchId.Text = "1";
        }



    }
    protected void btnSaveBranch_Click(object sender, EventArgs e)
    {
        // For remove white space
        branchName = txtBranch.Text.Trim();
        branchName = Regex.Replace(branchName, @"\s+", " ");
        branchCode = txtBranchCode.Text.Trim();
        branchCode = Regex.Replace(branchCode, @"\s+", " ");
        try
        {
            if (btnSaveBranch.Text == "Save")
            {

                DataTable dtedit = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_name='" + branchName + "'");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Branch name already exist");
                    return;
                }
                else
                {
                    DataTable dtedit1 = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_code='" + branchCode + "'");

                    if (dtedit1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Branch code  already exist");
                        return;
                    }
                    else
                    {
                        DataTable dtsave = g.ReturnData("Insert into branch_TB (branch_name,branch_code,status) values('" + branchName + "','" + branchCode + "',True)");
                        g.ShowMessage(this.Page, "Branch data saved successfully.");
                    }
                }

            }
            else
            {

                DataTable dtedit = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_id=" + Convert.ToInt32(lblId.Text) + " and branch_name='" + branchName + "' and branch_code='" + branchCode + "'");


                if (dtedit.Rows.Count > 0)
                {
                    updateBranch();

                }
                else
                {
                    DataTable dtedit1 = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_id<>" + Convert.ToInt32(lblId.Text) + " and branch_name='" + branchName + "' and branch_code='" + branchCode + "'");

                    if (dtedit1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Branch name already exist");
                        return;
                    }
                    else
                    {
                        DataTable dtedit2 = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_id<>" + Convert.ToInt32(lblId.Text) + " and branch_code='" + branchCode + "'");

                        if (dtedit2.Rows.Count > 0)
                        {
                            g.ShowMessage(this.Page, "Branch code already exist");
                            return;

                        }
                        else
                        {
                            DataTable dtedit3 = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_id<>" + Convert.ToInt32(lblId.Text) + " and branch_name='" + branchName + "'");

                            if (dtedit3.Rows.Count > 0)
                            {
                                g.ShowMessage(this.Page, "Branch name already exist");
                                return;

                            }
                            else
                            {
                                updateBranch();
                            }
                        }
                    }
                }
            }
            clearFields();
            bindBranchGrid();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }



    }

    private void updateBranch()
    {
        try
        {
            DataTable dtUpdate = g.ReturnData("Update branch_TB set branch_code='" + branchCode + "',branch_name='" + branchName + "' where branch_id=" + Convert.ToInt32(lblId.Text) + "");

            g.ShowMessage(this.Page, "Branch data updated successfully.");
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void clearFields()
    {
        txtBranchId.Text = "";
        txtBranch.Text = "";
        txtBranchCode.Text = "";
        btnSaveBranch.Text = "Save";
        lblId.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddnewBranch.Focus();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        lblId.Text = lnk.CommandArgument;
        DataTable dtedit = g.ReturnData("Select branch_id,branch_code,branch_name from branch_TB where branch_id=" + Convert.ToInt32(lblId.Text) + "");

        txtBranchId.Text = dtedit.Rows[0]["branch_id"].ToString();
        txtBranch.Text = dtedit.Rows[0]["branch_name"].ToString();
        txtBranchCode.Text = dtedit.Rows[0]["branch_code"].ToString();

        MultiView1.ActiveViewIndex = 1;
        btnSaveBranch.Text = "Update";
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        clearFields();
    }
    protected void grdBranch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBranch.PageIndex = e.NewPageIndex;
        bindBranchGrid();
    }
}