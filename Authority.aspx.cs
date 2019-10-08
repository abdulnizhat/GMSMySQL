using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Authority : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    DataTable dtauthority = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {

            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    protected void rbtnuserandoperation_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlUserAuthority.Items.Clear();
        lblAuthorityName.Visible = true;
        ddlUserAuthority.Visible = true;
        if (rbtnuserandoperation.SelectedItem.Text == "User Wise")
        {
            lblAuthorityName.Text = "User Name";
            btnAuthority.Visible = true;
            fillemployee();
        }
        else
        {
            lblAuthorityName.Text = "Operation Name";
            btnAuthority.Visible = true;
            fillOperation();
        }
    }

    private void fillOperation()
    {
        try
        {
            DataTable dtoperation = g.ReturnData("Select childid,childnode from treeviewchild_TB");

            ddlUserAuthority.DataSource = dtoperation;
            ddlUserAuthority.DataTextField = "childnode";
            ddlUserAuthority.DataValueField = "childid";
            ddlUserAuthority.DataBind();
            ddlUserAuthority.Items.Insert(0, "--Select--");



        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void binduserAuthoriy()
    {
        try
        {

            ViewState["dtauthority"] = null;
            grduserandoperationwise.DataSource = null;
            grduserandoperationwise.DataBind();
            dtauthority = new DataTable();

            if (rbtnuserandoperation.SelectedIndex == 0)
            {
                #region userwise gridshow
                grduserandoperationwise.Columns[0].Visible = true;
                grduserandoperationwise.Columns[1].Visible = false;

                DataTable dtoperation = g.ReturnData("SELECT treeviewchild_TB.childid, treeviewchild_TB.childnode FROM treeviewchild_TB ORDER BY treeviewchild_TB.childid ASC");
                if (dtoperation.Rows.Count > 0)
                {
                    for (int i = 0; i < dtoperation.Rows.Count; i++)
                    {
                        if (ViewState["dtauthority"] != null)
                        {
                            dtauthority = (DataTable)ViewState["dtauthority"];
                        }
                        else
                        {
                            DataColumn childnode = dtauthority.Columns.Add("childnode");
                            DataColumn username = dtauthority.Columns.Add("username");
                            DataColumn allowcreate = dtauthority.Columns.Add("allowcreate");
                            DataColumn allowmodify = dtauthority.Columns.Add("allowmodify");
                            DataColumn allowview = dtauthority.Columns.Add("allowview");
                            DataColumn employee_id = dtauthority.Columns.Add("employee_id");
                            DataColumn CustId = dtauthority.Columns.Add("CustId");
                            DataColumn child_id = dtauthority.Columns.Add("child_id");
                        }
                        DataRow dr = dtauthority.NewRow();
                        dr[0] = dtoperation.Rows[i]["childnode"].ToString();
                        dr[1] = ddlUserAuthority.SelectedItem;
                        dr[2] = "";
                        dr[3] = "";
                        dr[4] = "";
                        dr[5] = ddlUserAuthority.SelectedValue;
                        DataTable dtcust = g.ReturnData("Select customer_id from employee_TB where employee_id=" + Convert.ToInt32(ddlUserAuthority.SelectedValue) + "");
                        if (dtcust.Rows.Count > 0)
                        {
                            dr[6] = dtcust.Rows[0]["customer_id"].ToString();
                        }

                        dr[7] = dtoperation.Rows[i]["childid"].ToString();

                        dtauthority.Rows.Add(dr);
                        ViewState["dtauthority"] = dtauthority;

                    }
                }
                grduserandoperationwise.DataSource = dtauthority;
                grduserandoperationwise.DataBind();
                for (int i = 0; i < grduserandoperationwise.Rows.Count; i++)
                {
                    CheckBox CreateValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowcreate");
                    CheckBox ModifyValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowmodify");
                    CheckBox viewvalue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowview");
                    Label lblchildid = (Label)grduserandoperationwise.Rows[i].FindControl("lblchildId");
                    Label lblCustid = (Label)grduserandoperationwise.Rows[i].FindControl("lblCustId");
                    Label lblempid = (Label)grduserandoperationwise.Rows[i].FindControl("lblempId");

                    DataTable dtcheckvalue = g.ReturnData("Select allowcreate,allowmodify,allowview from authority_TB where customer_id=" + Convert.ToInt32(lblCustid.Text) + " and employee_id=" + Convert.ToInt32(lblempid.Text) + " and child_id=" + Convert.ToInt32(lblchildid.Text) + "");
                    if (dtcheckvalue.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dtcheckvalue.Rows[0]["allowcreate"].ToString()) == true)
                        {
                            CreateValue.Checked = true;
                        }
                        else
                        {
                            CreateValue.Checked = false;
                        }
                        if (Convert.ToBoolean(dtcheckvalue.Rows[0]["allowmodify"].ToString()) == true)
                        {
                            ModifyValue.Checked = true;
                        }
                        else
                        {
                            ModifyValue.Checked = false;
                        }
                        if (Convert.ToBoolean(dtcheckvalue.Rows[0]["allowview"].ToString()) == true)
                        {
                            viewvalue.Checked = true;
                        }
                        else
                        {
                            viewvalue.Checked = false;
                        }
                    }

                }

                #endregion
            }

            else
            {
                #region Operationwise gridshow
                grduserandoperationwise.Columns[0].Visible = false;
                grduserandoperationwise.Columns[1].Visible = true;

                DataTable dtemp = g.ReturnData("SELECT employee_TB.employee_id, employee_TB.customer_id,concat_WS(': ID-',employee_TB.employee_name,employee_TB.employee_id) AS username FROM employee_TB where employee_TB.status=True");

                for (int i = 0; i < dtemp.Rows.Count; i++)
                {

                    if (ViewState["dtauthority"] != null)
                    {
                        dtauthority = (DataTable)ViewState["dtauthority"];
                    }
                    else
                    {
                        DataColumn childnode = dtauthority.Columns.Add("childnode");
                        DataColumn username = dtauthority.Columns.Add("username");
                        DataColumn allowcreate = dtauthority.Columns.Add("allowcreate");
                        DataColumn allowmodify = dtauthority.Columns.Add("allowmodify");
                        DataColumn allowview = dtauthority.Columns.Add("allowview");
                        DataColumn employee_id = dtauthority.Columns.Add("employee_id");
                        DataColumn CustId = dtauthority.Columns.Add("CustId");
                        DataColumn child_id = dtauthority.Columns.Add("child_id");
                    }
                    DataRow dr = dtauthority.NewRow();
                    dr[0] = ddlUserAuthority.SelectedItem;
                    dr[1] = dtemp.Rows[i]["username"].ToString();
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = "";
                    dr[5] = dtemp.Rows[i]["employee_id"].ToString();
                    dr[6] = dtemp.Rows[i]["customer_id"].ToString();
                    dr[7] = ddlUserAuthority.SelectedValue;

                    dtauthority.Rows.Add(dr);
                    ViewState["dtauthority"] = dtauthority;

                }
                grduserandoperationwise.DataSource = dtauthority;
                grduserandoperationwise.DataBind();
                for (int i = 0; i < grduserandoperationwise.Rows.Count; i++)
                {
                    CheckBox CreateValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowcreate");
                    CheckBox ModifyValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowmodify");
                    CheckBox viewvalue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowview");
                    Label lblchildid = (Label)grduserandoperationwise.Rows[i].FindControl("lblchildId");
                    Label lblCustid = (Label)grduserandoperationwise.Rows[i].FindControl("lblCustId");
                    Label lblempid = (Label)grduserandoperationwise.Rows[i].FindControl("lblempId");

                    DataTable dtcheckvalue = g.ReturnData("Select allowcreate,allowmodify,allowview from authority_TB where customer_id=" + Convert.ToInt32(lblCustid.Text) + " and employee_id=" + Convert.ToInt32(lblempid.Text) + " and child_id=" + Convert.ToInt32(lblchildid.Text) + "");

                    if (dtcheckvalue.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dtcheckvalue.Rows[0]["allowcreate"].ToString()) == true)
                        {
                            CreateValue.Checked = true;
                        }
                        else
                        {
                            CreateValue.Checked = false;
                        }
                        if (Convert.ToBoolean(dtcheckvalue.Rows[0]["allowmodify"].ToString()) == true)
                        {
                            ModifyValue.Checked = true;
                        }
                        else
                        {
                            ModifyValue.Checked = false;
                        }
                        if (Convert.ToBoolean(dtcheckvalue.Rows[0]["allowview"].ToString()) == true)
                        {
                            viewvalue.Checked = true;
                        }
                        else
                        {
                            viewvalue.Checked = false;
                        }
                    }

                }
                #endregion
            }

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
            childId = g.GetChildId("Authority.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnupdate.Enabled = true;
                }
                else
                {
                    btnupdate.Enabled = false;
                }

            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fillemployee()
    {
        try
        {
            DataTable dtemp = g.ReturnData("SELECT employee_TB.employee_id, employee_TB.customer_id,concat_WS(': ID-',employee_TB.employee_name,employee_TB.employee_id) AS username FROM employee_TB where employee_TB.status=True");

            ddlUserAuthority.DataSource = dtemp;
            ddlUserAuthority.DataTextField = "username";
            ddlUserAuthority.DataValueField = "employee_id";
            ddlUserAuthority.DataBind();
            ddlUserAuthority.Items.Insert(0, "--Select--");


        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }

    protected void btnAuthority_Click(object sender, EventArgs e)
    {
        dtauthority = null;
        ViewState["dtauthority"] = null;
        grduserandoperationwise.DataSource = null;
        grduserandoperationwise.DataBind();
        chkselectall.Visible = true;
        btnupdate.Visible = true;
        binduserAuthoriy();
    }
    protected void chkselectall_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < grduserandoperationwise.Rows.Count; i++)
        {


            CheckBox CreateValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowcreate");
            CheckBox ModifyValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowmodify");
            CheckBox viewvalue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowview");


            if (chkselectall.SelectedItem.Text == "Check All")
            {

                CreateValue.Checked = true;
                ModifyValue.Checked = true;
                viewvalue.Checked = true;
            }
            else if (chkselectall.SelectedItem.Text == "Uncheck All")
            {
                CreateValue.Checked = false;
                ModifyValue.Checked = false;
                viewvalue.Checked = false;
            }



        }
    }


    protected void btnclear_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            
                for (int i = 0; i < grduserandoperationwise.Rows.Count; i++)
                {
                    CheckBox CreateValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowcreate");
                    CheckBox ModifyValue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowmodify");
                    CheckBox viewvalue = (CheckBox)grduserandoperationwise.Rows[i].FindControl("allowview");
                    Label lblchildid = (Label)grduserandoperationwise.Rows[i].FindControl("lblchildId");
                    Label lblCustid = (Label)grduserandoperationwise.Rows[i].FindControl("lblCustId");
                    Label lblempid = (Label)grduserandoperationwise.Rows[i].FindControl("lblempId");

                    DataTable dtcheckvalue = g.ReturnData("Select authority_id from authority_TB where customer_id=" + Convert.ToInt32(lblCustid.Text) + " and employee_id=" + Convert.ToInt32(lblempid.Text) + " and child_id=" + Convert.ToInt32(lblchildid.Text) + "");

                    if (dtcheckvalue.Rows.Count > 0)
                    {
                        #region code for update

                         bool acreate = false;
                            bool amodify = false;
                            bool aview = false;
                            if (CreateValue.Checked == true)
                            {
                                acreate = true;
                            }
                            else
                            {
                                acreate = false;
                            }
                            if (ModifyValue.Checked == true)
                            {
                                amodify = true;
                            }
                            else
                            {
                                amodify = false;
                            }
                            if (viewvalue.Checked == true)
                            {
                                aview = true;
                            }
                            else
                            {
                                aview= false;
                            }
                            DataTable dtupdate = g.ReturnData("Update authority_TB set allowcreate=" + acreate + ",allowmodify=" + amodify + ",allowview=" + aview + " where authority_id=" + Convert.ToInt32(dtcheckvalue.Rows[0]["authority_id"].ToString()) + "");
                            
                           

                       
                        #endregion
                    }
                    else
                    {
                        if (CreateValue.Checked == true || ModifyValue.Checked == true || viewvalue.Checked == true)
                        {

                            bool acreate = false;
                            bool amodify = false;
                            bool aview = false;
                            if (CreateValue.Checked == true)
                            {
                                acreate = true;
                            }
                            else
                            {
                                acreate = false;
                            }
                            if (ModifyValue.Checked == true)
                            {
                                amodify = true;
                            }
                            else
                            {
                                amodify = false;
                            }
                            if (viewvalue.Checked == true)
                            {
                                aview = true;
                            }
                            else
                            {
                                aview = false;
                            }
                            DateTime dtdate = DateTime.Now;
                            string strnownDate = dtdate.ToString("yyyy-MM-dd H:mm:ss");
                            DataTable dtsave = g.ReturnData("Insert into authority_TB (employee_id,customer_id,child_id,allowcreate,allowmodify,allowview,allocated_by_id,created_datetime) values(" + Convert.ToInt32(lblempid.Text) + "," + Convert.ToInt32(lblCustid.Text) + "," + Convert.ToInt32(lblchildid.Text) + "," + acreate + "," + amodify + "," + aview + "," + Convert.ToInt32(Session["User_ID"]) + ",'" + strnownDate + "')");
                           
                        }
                    }

                }
                g.ShowMessage(this.Page, "Authority Updated successfully.");
                binduserAuthoriy();
          

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }


    }
}