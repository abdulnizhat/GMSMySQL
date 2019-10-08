using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CountryMaster : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stcountry = "";

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindCountrygrid();
                btnAddCountry.Focus();


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
            childId = g.GetChildId("CountryMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddCountry.Visible = true;
                }
                else
                {
                    btnAddCountry.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCountry.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCountry.Rows[i].FindControl("btnEditCountry");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdCountry.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCountry.Rows[i].FindControl("btnEditCountry");
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
    private void bindCountrygrid()
    {
        try
        {
            DataTable dtcountry = q.getcountryDetails();

            grdCountry.DataSource = dtcountry;
            grdCountry.DataBind();
            //lblcnt.Text = coundata.Count().ToString();

            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddCountry_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveCountry.Text = "Save";
        txtCountryname.Focus();
        
            try
            {
                DataTable dtmax = g.ReturnData("Select MAX(country_Id) from countryMaster_TB");
                int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
                txtCountryId.Text = (maxId + 1).ToString();
            }
            catch (Exception)
            {

                txtCountryId.Text = "1";
            }
       
    }

    protected void btnEditCountry_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblcountId.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtCountryname.Focus();
            DataTable dtedit = g.ReturnData("Select country_Id,country_name from countryMaster_TB where country_Id=" + Convert.ToInt32(lblcountId.Text) + "");

            txtCountryname.Text = dtedit.Rows[0]["country_name"].ToString();
            txtCountryId.Text = dtedit.Rows[0]["country_Id"].ToString();
            btnSaveCountry.Text = "Update";
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseCountry_Click(object sender, EventArgs e)
    {
        txtCountryId.Text = "";
        txtCountryname.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddCountry.Focus();
    }
    protected void btnSaveCountry_Click(object sender, EventArgs e)
    {
        try
        {
            stcountry = txtCountryname.Text.Trim();
            stcountry = Regex.Replace(stcountry, @"\s+", " ");

           if (btnSaveCountry.Text == "Save")
                {
                    DataTable dtedit = g.ReturnData("Select country_Id,country_name from countryMaster_TB where country_name='" + stcountry + "'");

                    if (dtedit.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Country name is already exist.");
                        return;
                    }
                    else
                    {
                        DataTable dtsave = g.ReturnData("Insert into countryMaster_TB (country_name,status) values('" + stcountry + "',True)");
                        
                        g.ShowMessage(this.Page, "Country is saved successfully.");
                    }
                }
                else
                {
                    DataTable dtedit = g.ReturnData("Select country_Id,country_name from countryMaster_TB where country_name='" + stcountry + "' and country_Id<>" + Convert.ToInt32(lblcountId.Text) + "");

                    if (dtedit.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Country name is already exist.");
                       
                        return;
                    }
                    else
                    {
                        DataTable dtupdate = g.ReturnData("Update countryMaster_TB set country_name='" + stcountry + "' where country_Id=" + Convert.ToInt32(lblcountId.Text) + "");
                       
                        g.ShowMessage(this.Page, "Country is updated successfully.");
                    }
                }

                bindCountrygrid();
                MultiView1.ActiveViewIndex = 0;
                txtCountryId.Text = "";
                txtCountryname.Text = "";
                btnAddCountry.Focus();
            
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdCountry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCountry.PageIndex = e.NewPageIndex;
        bindCountrygrid();
    }
}