using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CityMaster : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stcityname = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                btnAddCity.Focus();
                fillCountry();
                bindCityDetails();
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
            childId = g.GetChildId("CityMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddCity.Visible = true;
                }
                else
                {
                    btnAddCity.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCity.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCity.Rows[i].FindControl("btnEditCity");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdCity.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCity.Rows[i].FindControl("btnEditCity");
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
    private void bindCityDetails()
    {
        try
        {
            string sqlquery = q.getcityDetails();
            DataTable dtcity = g.ReturnData(sqlquery);
            grdCity.DataSource = dtcity;
            grdCity.DataBind();
            //lblcnt.Text = resultCity.Count().ToString();

            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fillCountry()
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
    protected void btnAddCity_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        ddlCountry.Focus();

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(city_Id) from cityMaster_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtCityId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {

            txtCityId.Text = "1";
        }

    }
    protected void btnEditCity_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblcityid.Text = Lnk.CommandArgument;
            string sqlquery = q.getcityDetails();
            sqlquery = sqlquery + " and cityMaster_TB.city_Id=" + Convert.ToInt32(lblcityid.Text) + "";
            DataTable dtedit = g.ReturnData(sqlquery);

            txtCityId.Text = dtedit.Rows[0]["city_Id"].ToString();
            ddlCountry.SelectedValue = dtedit.Rows[0]["country_Id"].ToString();
            bindState(Convert.ToInt32(ddlCountry.SelectedValue));
            ddlState.SelectedValue = dtedit.Rows[0]["stateId"].ToString();
            txtCity.Text = dtedit.Rows[0]["city_name"].ToString();

            MultiView1.ActiveViewIndex = 1;
            btnSaveCity.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveCity_Click(object sender, EventArgs e)
    {
        try
        {
            stcityname = txtCity.Text.Trim();
            stcityname = Regex.Replace(stcityname, @"\s+", " ");


            if (btnSaveCity.Text == "Save")
            {
                #region Save code
                DataTable dtcheck = g.ReturnData("Select city_Id from cityMaster_TB where  city_name='" + stcityname + "' and state_Id=" + Convert.ToInt32(ddlState.SelectedValue) + "");

                if (dtcheck.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "City data is already exist.");
                    return;
                }
                else
                {
                    DataTable dtsave = g.ReturnData("Insert into cityMaster_TB (city_name,country_Id,state_Id,status) values ('" + stcityname + "'," + Convert.ToInt32(ddlCountry.SelectedValue) + "," + Convert.ToInt32(ddlState.SelectedValue) + ",True)");

                    g.ShowMessage(this.Page, "City data is saved successfully.");

                }
                #endregion
            }
            else
            {
                DataTable dtcheck = g.ReturnData("Select city_Id from cityMaster_TB where city_Id=" + Convert.ToInt32(lblcityid.Text) + " and city_name='" + stcityname + "' and state_Id=" + Convert.ToInt32(ddlState.SelectedValue) + "");

                if (dtcheck.Rows.Count > 0)
                {
                    updatecode();
                }
                else
                {
                    DataTable dtcheck1 = g.ReturnData("Select city_Id from cityMaster_TB where city_Id<>" + Convert.ToInt32(lblcityid.Text) + " and city_name='" + stcityname + "' and state_Id=" + Convert.ToInt32(ddlState.SelectedValue) + "");

                    if (dtcheck1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "City data is already exist.");
                        return;
                    }
                    else
                    {
                        updatecode();
                    }
                }
            }


            clearFields();
            MultiView1.ActiveViewIndex = 0;
            bindCityDetails();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void updatecode()
    {
        try
        {
            DataTable dtupdate = g.ReturnData("Update cityMaster_TB set city_name='" + stcityname + "',state_Id=" + Convert.ToInt32(ddlState.SelectedValue) + ",country_Id=" + Convert.ToInt32(ddlCountry.SelectedValue) + " where city_Id=" + Convert.ToInt32(lblcityid.Text) + "");
            g.ShowMessage(this.Page, "City data is updated successfully.");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }


    private void clearFields()
    {
        ddlCountry.SelectedIndex = 0;
        ddlState.Items.Clear();
        txtCity.Text = txtCityId.Text = lblcityid.Text = string.Empty;
        btnSaveCity.Text = "Save";
        btnAddCity.Focus();
    }
    protected void btnCloseCity_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }

    protected void grdCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCity.PageIndex = e.NewPageIndex;
        bindCityDetails();
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCountry.SelectedIndex > 0)
        {
            bindState(Convert.ToInt32(ddlCountry.SelectedValue));
        }
        else
        {
            ddlState.Items.Clear();
        }
    }
    private void bindState(int countryId)
    {
        try
        {
            string sqlquery = q.getstateDetails();
            sqlquery = sqlquery + " where stateMaster_TB.country_Id =" + countryId + " and stateMaster_TB.status=True";
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


}