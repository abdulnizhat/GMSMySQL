using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GaugeSupplier : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {

            if (!Page.IsPostBack)
            {
               
                Session["dleteGageSuppLink"] = "NO";
                btnAddGaugeSupplier.Focus();
                MultiView1.ActiveViewIndex = 0;
                fillGauge(Convert.ToInt32(Session["Customer_ID"]));
                fillSupplier(Convert.ToInt32(Session["Customer_ID"]));
                bindGaugeSupplierGrd(Convert.ToInt32(Session["Customer_ID"]));

                if (Request.QueryString["supplierLink_gauge_id"] != null)
                {
                    string getquerystringid = Request.QueryString["supplierLink_gauge_id"].ToString();
                    int gaugeid = Convert.ToInt32(getquerystringid);
                    ddlGauge.SelectedValue = getquerystringid;
                    GetSupplierlinkId();
                    fillGaugedata(gaugeid);
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
            childId = g.GetChildId("GaugeSupplier.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddGaugeSupplier.Visible = true;
                }
                else
                {
                    btnAddGaugeSupplier.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdGaugeSupplier.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGaugeSupplier.Rows[i].FindControl("btnEditPart");
                        LinkButton lnkdlet = (LinkButton)grdGaugeSupplier.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = true;
                        lnk.Enabled = true;
                        Session["dleteGageSuppLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdGaugeSupplier.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGaugeSupplier.Rows[i].FindControl("btnEditPart");
                        lnk.Enabled = false;
                        LinkButton lnkdlet = (LinkButton)grdGaugeSupplier.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = false;
                        Session["dleteGageSuppLink"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindGaugeSupplierGrd(int custId)
    {
        try
        {
            string stprocedure = "spGaugeSupplier";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 1, custId,"","", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 2, custId, txtsearchValue.Text,"", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 4, custId, "", txtsearchValue.Text,"", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Supplier Name-Wise")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 5, custId, "", "", txtsearchValue.Text,"");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 6, custId, "", "", "", txtsearchValue.Text);
                dt = ds.Tables[0];
            }
           
            grdGaugeSupplier.DataSource = dt;
            grdGaugeSupplier.DataBind();

            checkAuthority();
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
    private void fillGauge(int custId)
    {
        try
        {
            string stgauge = q.GetGaugeIdname(custId);
            DataTable dtgauge = g.ReturnData(stgauge);

            ddlGauge.DataSource = dtgauge;
            ddlGauge.DataTextField = "gaugeNameAndID";
            ddlGauge.DataValueField = "gauge_id";
            ddlGauge.DataBind();
            ddlGauge.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddGaugeSupplier_Click(object sender, EventArgs e)
    {
        GetSupplierlinkId();
    }

    private void GetSupplierlinkId()
    {
        MultiView1.ActiveViewIndex = 1;
        ddlGauge.Focus();

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(gauge_supplier_link_id) from gauge_supplier_link_tb");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtGaugeSupLinkID.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {
            txtGaugeSupLinkID.Text = "1";
        }
    }
    protected void btnSaveGaugeSupplier_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {
            if (btnSaveGaugeSupplier.Text == "Save")
            {

                DataTable dtcheck = g.ReturnData("Select gauge_id from gauge_supplier_link_TB where link_status='ISSUED' and status=True and gauge_id=" + Convert.ToInt32(ddlGauge.SelectedValue) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + "");

                if (dtcheck.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "This Gauge is already issued.");
                        return;
                    }
                    else
                    {
                        saveUpdateGaugeSupplier(0);
                    }
                
            }
            else
            {
                int editId = Convert.ToInt32(lblGaugeSupplierID.Text);
                DataTable dtcheck = g.ReturnData("Select gauge_id from gauge_supplier_link_TB where link_status='ISSUED' and status=True and gauge_id=" + Convert.ToInt32(ddlGauge.SelectedValue) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_supplier_link_id ="+editId+"");

                if (dtcheck.Rows.Count> 0)
                    {
                        saveUpdateGaugeSupplier(editId);

                    }
                    else
                    {
                        DataTable dtcheck1 = g.ReturnData("Select gauge_id from gauge_supplier_link_TB where link_status='ISSUED' and status=True and gauge_id=" + Convert.ToInt32(ddlGauge.SelectedValue) + " and customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_supplier_link_id <>" + editId + "");

                        if (dtcheck1.Rows.Count > 0)
                        {
                            g.ShowMessage(this.Page, "This Gauge is already issued.");
                            return;
                        }
                        else
                        {
                            saveUpdateGaugeSupplier(editId);
                        }
                    }
                }
            
            bindGaugeSupplierGrd(Convert.ToInt32(Session["Customer_ID"]));
            MultiView1.ActiveViewIndex = 0;
            clearFields();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void saveUpdateGaugeSupplier(int gauge_Supplier_Id)
    {
        try
        {
            DateTime dtlinkDate = DateTime.Now;
            string dtlinkDate1=dtlinkDate.ToString("yyyy-MM-dd H:mm:ss"); 
          
              
               
                if (btnSaveGaugeSupplier.Text == "Save")
                {
                     DataTable dtsave=g.ReturnData("Insert into gauge_supplier_link_TB (customer_id,status,supplier_id, linked_date,link_status,gauge_id,created_by_id) Values("+Convert.ToInt32(Session["Customer_ID"])+",True,"+Convert.ToInt32(ddlSuplier.SelectedValue)+",'"+dtlinkDate1+"','"+ddlStatus.SelectedItem.Text+"',"+Convert.ToInt32(ddlGauge.SelectedValue)+", "+Convert.ToInt32(Session["User_ID"])+")");
                   g.ShowMessage(this.Page, "Gauge supplier linked is saved successfully");
                }
                else
                {
                    DataTable dtupdate = g.ReturnData("Update gauge_supplier_link_TB set customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ", status=True,supplier_id=" + Convert.ToInt32(ddlSuplier.SelectedValue) + ",linked_date='" + dtlinkDate1 + "',link_status='" + ddlStatus.SelectedItem.Text + "',gauge_id=" + Convert.ToInt32(ddlGauge.SelectedValue) + ",   created_by_id=" + Convert.ToInt32(Session["User_ID"]) + " where gauge_supplier_link_id ="+ gauge_Supplier_Id+"");
                    
               
                    g.ShowMessage(this.Page, "Gauge supplier linked is updated successfully");
                }

          
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
            throw;
        }
    }
    protected void btnClloseGaugeSupplier_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }
    protected void ddlGauge_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGauge.SelectedIndex > 0)
        {
            fillGaugedata(Convert.ToInt32(ddlGauge.SelectedValue));

        }
        else
        {
            clearFields();
        }
        ddlSuplier.Focus();
    }
    private void fillGaugedata(int gaugeId)
    {
        try
        {
            string sqlquery = q.getGaugeDetails();
            sqlquery = sqlquery + " and gt.gauge_id=" + gaugeId + "";
            DataTable dtgauge = g.ReturnData(sqlquery);
            if (dtgauge.Rows.Count > 0)
            {
                txtCurrentLocation.Text = dtgauge.Rows[0]["current_location"].ToString();
               
                txtPurchaseDate.Text =  dtgauge.Rows[0]["purchase_date"].ToString();
                txtPurchaseCost.Text = dtgauge.Rows[0]["purchase_cost"].ToString();
                if (dtgauge.Rows[0]["gauge_type"].ToString() == "ATTRIBUTE")
                {
                    divSize.Visible = true;
                    divRange.Visible = false;
                    txtSize.Text = dtgauge.Rows[0]["size_range"].ToString();
                    divPemisablerror1.Visible = false;
                    divPemisablerror2.Visible = false;
                }
                else
                {
                    divSize.Visible = false;
                    divRange.Visible = true;
                    txtRange.Text = dtgauge.Rows[0]["size_range"].ToString();
                    divPemisablerror1.Visible = true;
                    divPemisablerror2.Visible = true;
                    txtPermisableError1.Text = dtgauge.Rows[0]["permisable_error1"].ToString();
                    txtPermisableError2.Text = dtgauge.Rows[0]["permisable_error2"].ToString();
                }
               
                txtServiceDate.Text = dtgauge.Rows[0]["service_date"].ToString();
                txtStoreLocation.Text = dtgauge.Rows[0]["store_location"].ToString();
                txtType.Text = dtgauge.Rows[0]["gauge_type"].ToString();
                
                txtRetairementDate.Text =dtgauge.Rows[0]["retairment_date"].ToString();
                txtManufactureID.Text = dtgauge.Rows[0]["gauge_Manufature_Id"].ToString();
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void clearFields()
    {
        ddlGauge.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlSuplier.SelectedIndex = 0;
        txtCurrentLocation.Text = txtPermisableError1.Text = txtPermisableError2.Text =
        txtPurchaseCost.Text = txtPurchaseDate.Text = txtRange.Text = txtServiceDate.Text =
        txtSize.Text = txtStoreLocation.Text = txtType.Text = txtRetairementDate.Text =
        txtManufactureID.Text = string.Empty;
        btnSaveGaugeSupplier.Text = "Save";
        btnAddGaugeSupplier.Focus();

        if (Request.QueryString["supplierLink_gauge_id"] != null)
        {
            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove("supplierLink_gauge_id");
            txtGaugeSupLinkID.Text = String.Empty;
            Response.Redirect("~/GaugeMaster.aspx");
        }
    }
    protected void btnEditPart_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblGaugeSupplierID.Text = lnk.CommandArgument;
            DataTable dtgaugesup = g.ReturnData("Select gauge_id,supplier_id,link_status from gauge_supplier_link_TB where gauge_supplier_link_id=" + Convert.ToInt32(lblGaugeSupplierID.Text) + "");
               
                int id = Convert.ToInt32(dtgaugesup.Rows[0]["gauge_id"].ToString());
                txtGaugeSupLinkID.Text = lblGaugeSupplierID.Text;
                ddlGauge.SelectedValue =dtgaugesup.Rows[0]["gauge_id"].ToString();
                ddlSuplier.SelectedValue =dtgaugesup.Rows[0]["supplier_id"].ToString();
                if (dtgaugesup.Rows[0]["link_status"].ToString() == "ISSUED")
                {
                    ddlStatus.SelectedValue = "1";
                }

                fillGaugedata(id);
         
            MultiView1.ActiveViewIndex = 1;
            btnSaveGaugeSupplier.Text = "Update";
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
                lblGaugeSupplierID.Text = lnk.CommandArgument;
                DataTable dtupd = g.ReturnData("Update gauge_supplier_link_TB set status=False where gauge_supplier_link_id=" + Convert.ToInt32(lblGaugeSupplierID.Text) + "");

                g.ShowMessage(this.Page, "Gauge supplier linked deleted successfully");

                bindGaugeSupplierGrd(Convert.ToInt32(Session["Customer_ID"]));
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdGaugeSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGaugeSupplier.PageIndex = e.NewPageIndex;
        bindGaugeSupplierGrd(Convert.ToInt32(Session["Customer_ID"]));
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindGaugeSupplierGrd(Convert.ToInt32(Session["Customer_ID"]));

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
        
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
        if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
        {
            lblName.Text = "Gauge Name";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
        {
            lblName.Text = "Gauge Sr.No.";
        }
        else if (ddlsortby.SelectedItem.Text == "Supplier Name-Wise")
        {
            lblName.Text = "Supplier Name";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
        {
            lblName.Text = "Gauge Type";
        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindGaugeSupplierGrd(Convert.ToInt32(Session["Customer_ID"]));
        }
    }
}