using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GaugePart : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    ArrayList arraylist1 = new ArrayList();
    ArrayList arraylist2 = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {

                Session["dleteGagePartLink"] = "NO";
                btnAddGaugePart.Focus();
                MultiView1.ActiveViewIndex = 0;
                fillGauge(Convert.ToInt32(Session["Customer_ID"]));
                fillPart(Convert.ToInt32(Session["Customer_ID"]));
                bindGaugePartGrd(Convert.ToInt32(Session["Customer_ID"]));

                if (Request.QueryString["gauge_id"] != null)
                {
                    string getquerystringid = Request.QueryString["gauge_id"].ToString();

                    int gaugeid = Convert.ToInt32(getquerystringid);
                    ddlGaugeID.SelectedValue = getquerystringid;
                    addnewid();
                    fillGaugeData(gaugeid);
                }
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void fillPart(int custId)
    {
        try
        {
            DataTable partdata = g.ReturnData("Select part_id, part_number  from partMaster_TB where status=True and customer_id=" + custId + "");

            ddlPartNumber.DataSource = partdata;
            ddlPartNumber.DataTextField = "part_number";
            ddlPartNumber.DataValueField = "part_id";
            ddlPartNumber.DataBind();
            //ddlPartNumber.Items.Insert(0, "--Select--");
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
            childId = g.GetChildId("GaugePart.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddGaugePart.Visible = true;
                }
                else
                {
                    btnAddGaugePart.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdGaugePart.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGaugePart.Rows[i].FindControl("btnEditGaugePart");
                        LinkButton lnkdlet = (LinkButton)grdGaugePart.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = true;
                        lnk.Enabled = true;
                        Session["dleteGagePartLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdGaugePart.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGaugePart.Rows[i].FindControl("btnEditGaugePart");
                        lnk.Enabled = false;
                        LinkButton lnkdlet = (LinkButton)grdGaugePart.Rows[i].FindControl("lnkDelete");
                        lnkdlet.Enabled = false;
                        Session["dleteGagePartLink"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindGaugePartGrd(int custId)
    {
        try
        {
            string stprocedure = "spGaugePartLinkDetails";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 1, custId, "", "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
            {

                DataSet ds = q.ProcdureWith6Param(stprocedure, 2, custId, txtsearchValue.Text, "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 3, custId, "", txtsearchValue.Text, "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Part No.-Wise")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 4, custId, "", "", txtsearchValue.Text, "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Part Name-Wise")
            {
                DataSet ds = q.ProcdureWith6Param(stprocedure, 5, custId, "", "", "", txtsearchValue.Text);
                dt = ds.Tables[0];
            }

            grdGaugePart.DataSource = dt;
            grdGaugePart.DataBind();

            checkAuthority();
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
            DataTable dtgauge = g.ReturnData("SELECT gauge_supplier_link_TB.gauge_id,CONCAT(gaugeMaster_TB.gauge_name , ': ID-',gauge_supplier_link_TB.gauge_id,': Sr No-',gaugeMaster_TB.gauge_sr_no) as gaugeNameAndID FROM gauge_supplier_link_TB INNER JOIN   gaugeMaster_TB ON gauge_supplier_link_TB.gauge_id = gaugeMaster_TB.gauge_id where gauge_supplier_link_TB.status=True and gauge_supplier_link_TB.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_supplier_link_TB.link_status='ISSUED'");

            ddlGaugeID.DataSource = dtgauge;
            ddlGaugeID.DataTextField = "gaugeNameAndID";
            ddlGaugeID.DataValueField = "gauge_id";
            ddlGaugeID.DataBind();
            ddlGaugeID.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddGaugePart_Click(object sender, EventArgs e)
    {
        addnewid();
    }

    private void addnewid()
    {
        MultiView1.ActiveViewIndex = 1;
        ddlGaugeID.Focus();

        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(gauge_part_link_id) from gauge_part_link_tb");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());
            txtLinkID.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {

            txtLinkID.Text = "1";
        }
    }
    protected void btnSaveGaugePart_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {
            string stpartid = "";
            string usedstpartid = "";
            int cnt = 0;
            DateTime dtDate = DateTime.Now;
            string dtDate1 = dtDate.ToString("yyyy-MM-dd H:mm:ss");

            if (btnSaveGaugePart.Text == "Save")
            {
                #region code for save
                for (int i = 0; i < listSelectedPart.Items.Count; i++)
                {

                    DataTable dtexistpart = g.ReturnData("Select part_id from gauge_part_link_TB where customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "  and part_id=" + Convert.ToInt32(listSelectedPart.Items[i].Value) + " and gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + " and status=True");
                    if (dtexistpart.Rows.Count > 0)
                    {
                        cnt++;
                        if (usedstpartid == "")
                        {
                            usedstpartid = listSelectedPart.Items[i].Text;
                        }
                        else
                        {
                            usedstpartid = usedstpartid + "," + listSelectedPart.Items[i].Text;

                        }
                    }

                    else
                    {
                        if (stpartid == "")
                        {
                            stpartid = listSelectedPart.Items[i].Value;
                        }
                        else
                        {
                            stpartid = stpartid + "," + listSelectedPart.Items[i].Value;

                        }
                    }

                }
                if (cnt > 0)
                {
                    g.ShowMessage(this.Page, usedstpartid + " Part number is already linked");
                    return;

                }
                else
                {
                    string[] stpartid1 = stpartid.Split(',');
                    for (int i = 0; i < stpartid1.Length; i++)
                    {

                        DataTable dtsave = g.ReturnData("Insert into gauge_part_link_TB (status,customer_id,linked_date,part_id, part_linked_status,gauge_id,created_by_id)  values(True," + Convert.ToInt32(Session["Customer_ID"]) + ",'" + dtDate1 + "'," + Convert.ToInt32(stpartid1[i].ToString()) + ", '" + txtStatus.Text + "'," + Convert.ToInt32(ddlGaugeID.SelectedValue) + "," + Convert.ToInt32(Session["User_ID"]) + ")");
                        g.ShowMessage(this.Page, " Gauge part linked is saved successfully.");
                    }


                }
                #endregion
            }

            else
            {
                #region code for Update
                for (int i = 0; i < listSelectedPart.Items.Count; i++)
                {

                    if (listSelectedPart.Items[i].Selected == true)
                    {
                        cnt++;
                    }

                }
                if (cnt > 1)
                {
                    g.ShowMessage(this.Page, "You can not select multiple part number on update.");
                    return;
                }
                else
                {


                    int editId = Convert.ToInt32(lblgaugePartLinkId.Text);
                    DataTable dtexistpart = g.ReturnData("Select part_id from gauge_part_link_TB where customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "  and part_id=" + Convert.ToInt32(listSelectedPart.SelectedValue) + " and gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + " and status=True and gauge_part_link_id =" + editId + "");

                    if (dtexistpart.Rows.Count > 0)
                    {
                        DataTable dtupdate = g.ReturnData("Update gauge_part_link_TB set status=True,customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ", linked_date='" + dtDate1 + "',part_id=" + Convert.ToInt32(listSelectedPart.SelectedValue) + ", part_linked_status='" + txtStatus.Text + "',gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + ",created_by_id=" + Convert.ToInt32(Session["User_ID"]) + " where gauge_part_link_id=" + editId + " ");
                        g.ShowMessage(this.Page, "Gauge part inked is updated successfully.");

                    }
                    else
                    {
                        DataTable dtexistpart1 = g.ReturnData("Select part_id from gauge_part_link_TB where customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "  and part_id=" + Convert.ToInt32(listSelectedPart.SelectedValue) + " and gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + " and status=True and gauge_part_link_id <>" + editId + "");

                        if (dtexistpart1.Rows.Count > 0)
                        {
                            g.ShowMessage(this.Page, "Part number is already linked");
                            return;
                        }
                        else
                        {
                            DataTable dtupdate = g.ReturnData("Update gauge_part_link_TB set status=True,customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ", linked_date='" + dtDate1 + "',part_id=" + Convert.ToInt32(listSelectedPart.SelectedValue) + ", part_linked_status='" + txtStatus.Text + "',gauge_id=" + Convert.ToInt32(ddlGaugeID.SelectedValue) + ",created_by_id=" + Convert.ToInt32(Session["User_ID"]) + " where gauge_part_link_id=" + editId + " ");
                            g.ShowMessage(this.Page, "Gauge part inked is updated successfully.");

                        }
                    }
                }
                #endregion

            }

            clearFields();
            MultiView1.ActiveViewIndex = 0;
            bindGaugePartGrd(Convert.ToInt32(Session["Customer_ID"]));
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    protected void btnClloseGaugePart_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }

    protected void ddlGaugeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlGaugeID.Focus();
        try
        {
            if (ddlGaugeID.SelectedIndex > 0)
            {
                fillGaugeData(Convert.ToInt32(ddlGaugeID.SelectedValue));
            }
            else
            {
                txtmanufactureID.Text = txtType.Text = txtStatus.Text = string.Empty;
            }

        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fillGaugeData(int gaugeId)
    {
        try
        {
            DataTable dtgauge = g.ReturnData("SELECT gauge_supplier_link_TB.link_status,gaugeMaster_TB.gauge_id,gaugeMaster_TB.gauge_Manufature_Id,gaugeMaster_TB.gauge_type FROM gauge_supplier_link_TB INNER JOIN   gaugeMaster_TB ON gauge_supplier_link_TB.gauge_id = gaugeMaster_TB.gauge_id where gaugeMaster_TB.gauge_id=" + gaugeId + "");

            if (dtgauge.Rows.Count > 0)
            {

                txtmanufactureID.Text = dtgauge.Rows[0]["gauge_Manufature_Id"].ToString();
                txtType.Text = dtgauge.Rows[0]["gauge_type"].ToString();
                txtStatus.Text = dtgauge.Rows[0]["link_status"].ToString();

            }
            else
            {
                txtmanufactureID.Text = txtType.Text = txtStatus.Text = string.Empty;
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void ddlPartNumber_SelectedIndexChanged(object sender, EventArgs e)
    {

        for (int i = 0; i < ddlPartNumber.Items.Count; i++)
        {
            if (ddlPartNumber.Items[i].Selected == true)
            {
                string text = ddlPartNumber.Items[i].Text;
                string value = ddlPartNumber.Items[i].Value;
            }

        }

        ddlPartNumber.Focus();
        try
        {
            if (ddlPartNumber.SelectedIndex > 0)
            {
                fillPartData(Convert.ToInt32(ddlPartNumber.SelectedValue));
            }
            else
            {
                txtPartId.Text = txtPartName.Text = txtOperation.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }

    private void fillPartData(int partId)
    {
        DataTable dtpart = g.ReturnData("Select part_name,part_id,operation from partMaster_TB where part_id=" + Convert.ToInt32(partId) + "");


        if (dtpart.Rows.Count > 0)
        {

            txtPartId.Text = dtpart.Rows[0]["part_id"].ToString();
            txtPartName.Text = dtpart.Rows[0]["part_name"].ToString();
            txtOperation.Text = dtpart.Rows[0]["operation"].ToString();

        }
        else
        {
            txtPartId.Text = txtPartName.Text = txtOperation.Text = string.Empty;
        }


    }
    private void clearFields()
    {
        listSelectedPart.Items.Clear();
        ddlGaugeID.SelectedIndex = 0;
        // ddlPartNumber.SelectedIndex = 0;
        txtmanufactureID.Text = txtType.Text = txtStatus.Text =
        txtPartId.Text = txtPartName.Text = txtOperation.Text = string.Empty;
        btnSaveGaugePart.Text = "Save";
        btnAddGaugePart.Focus();
        fillPart(Convert.ToInt32(Session["Customer_ID"]));

        if (Request.QueryString["gauge_id"] != null)
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove("gauge_id");
            txtLinkID.Text = String.Empty;
            Response.Redirect("~/GaugeMaster.aspx");
        }
    }
    protected void btnEditGaugePart_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblgaugePartLinkId.Text = lnk.CommandArgument;
            DataTable dtedit = g.ReturnData("Select gauge_part_link_id,gauge_id,part_id from gauge_part_link_TB where gauge_part_link_id=" + Convert.ToInt32(lblgaugePartLinkId.Text) + "");

            txtLinkID.Text = dtedit.Rows[0]["gauge_part_link_id"].ToString();
            ddlGaugeID.SelectedValue = dtedit.Rows[0]["gauge_id"].ToString();
            ddlPartNumber.SelectedValue = dtedit.Rows[0]["part_id"].ToString();

            int selectedindex = ddlPartNumber.SelectedIndex;
            listSelectedPart.Items.Add(((ListItem)ddlPartNumber.Items[selectedindex]));

            listSelectedPart.SelectedValue = ddlPartNumber.SelectedValue;
            ddlPartNumber.Items.Remove(((ListItem)ddlPartNumber.Items[selectedindex]));

            fillGaugeData(Convert.ToInt32(ddlGaugeID.SelectedValue));
            //fillPartData(Convert.ToInt32(ddlPartNumber.SelectedValue));

            MultiView1.ActiveViewIndex = 1;
            btnSaveGaugePart.Text = "Update";
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
                lblgaugePartLinkId.Text = lnk.CommandArgument;
                DataTable dtupd = g.ReturnData("Update gauge_part_link_TB set status=False where gauge_part_link_id=" + Convert.ToInt32(lblgaugePartLinkId.Text) + "");
                g.ShowMessage(this.Page, "Gauge part linked is Deleted successfully.");
                bindGaugePartGrd(Convert.ToInt32(Session["Customer_ID"]));
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdGaugePart_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGaugePart.PageIndex = e.NewPageIndex;
        bindGaugePartGrd(Convert.ToInt32(Session["Customer_ID"]));
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
        else if (ddlsortby.SelectedItem.Text == "Part No.-Wise")
        {
            lblName.Text = "Part No.";
        }
        else if (ddlsortby.SelectedItem.Text == "Part Name-Wise")
        {
            lblName.Text = "Part Name";
        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindGaugePartGrd(Convert.ToInt32(Session["Customer_ID"]));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        try
        {
            bindGaugePartGrd(Convert.ToInt32(Session["Customer_ID"]));

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
    protected void lnkallAddPartNumber_Click(object sender, EventArgs e)
    {
        string partId = "";
        if (ddlPartNumber.SelectedIndex >= 0)
        {
            for (int i = 0; i < ddlPartNumber.Items.Count; i++)
            {
                if (ddlPartNumber.Items[i].Selected)
                {
                    if (!arraylist1.Contains(ddlPartNumber.Items[i]))
                    {
                        arraylist1.Add(ddlPartNumber.Items[i]);
                        if (partId == "")
                        {
                            partId = ddlPartNumber.Items[i].Value.ToString();
                        }
                        else
                        {
                            partId = partId + "," + ddlPartNumber.Items[i].Value.ToString();
                        }
                    }
                }
            }
            if (HiddenFieldPartId.Value == "")
            {
                HiddenFieldPartId.Value = partId;
            }
            else
            {
                HiddenFieldPartId.Value = HiddenFieldPartId.Value + "," + partId;
            }
            for (int i = 0; i < arraylist1.Count; i++)
            {
                if (!listSelectedPart.Items.Contains(((ListItem)arraylist1[i])))
                {
                    listSelectedPart.Items.Add(((ListItem)arraylist1[i]));

                }
                ddlPartNumber.Items.Remove(((ListItem)arraylist1[i]));
            }
        }
    }
    protected void lnkRemovePart_Click(object sender, EventArgs e)
    {
        string partId = "";
        if (listSelectedPart.SelectedIndex >= 0)
        {
            for (int i = 0; i < listSelectedPart.Items.Count; i++)
            {
                if (listSelectedPart.Items[i].Selected)
                {
                    if (!arraylist2.Contains(listSelectedPart.Items[i]))
                    {
                        arraylist2.Add(listSelectedPart.Items[i]);
                        if (partId == "")
                        {
                            partId = listSelectedPart.Items[i].Value.ToString();
                        }
                        else
                        {
                            partId = partId + "," + listSelectedPart.Items[i].Value.ToString();

                        }

                    }
                }
            }
            for (int i = 0; i < arraylist2.Count; i++)
            {
                if (!ddlPartNumber.Items.Contains(((ListItem)arraylist2[i])))
                {
                    ddlPartNumber.Items.Add(((ListItem)arraylist2[i]));
                }
                listSelectedPart.Items.Remove(((ListItem)arraylist2[i]));
            }

        }
    }
}