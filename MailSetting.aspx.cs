using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MailSetting : System.Web.UI.Page
{
    Genreal g = new Genreal();
    string stEmailid = "";
    string stMailIdFrom = "";
    DataTable dtAddEmailId = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindMailSettingGrid();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void bindMailSettingGrid()
    {
        try
        {
            DataTable dtmaildata = g.ReturnData("SELECT mail_Setting_TB.customer_id, mail_Setting_TB.status, mail_Setting_TB.mail_setting_id, mail_Setting_TB.email_id_from, mail_Setting_TB.credential, mail_Setting_TB.port, mail_Setting_TB.specified_emai_to_send FROM mail_Setting_TB WHERE mail_Setting_TB.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " AND mail_Setting_TB.status=True");

            grdmailSetting.DataSource = dtmaildata;
            grdmailSetting.DataBind();

            checkAuthority();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveMailSetting_Click(object sender, EventArgs e)
    {
        try
        {
            stMailIdFrom = txtEmail.Text.Trim();
            stMailIdFrom = Regex.Replace(stMailIdFrom, @"\s+", " ");
            saveAndUpdateCode();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void saveAndUpdateCode()
    {
        try
        {
            //Email Id add To send information
            string emailId = null;
            int cnt = 0;
            DataTable dt = (DataTable)ViewState["dtAddEmailId"];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (cnt == 0)
                    {
                        emailId = dt.Rows[i]["mailid"].ToString();
                        cnt++;
                    }
                    else
                    {
                        emailId = emailId + "," + dt.Rows[i]["mailid"].ToString();
                    }
                }
            }
            bool statusb = false;
            if (rd_status.SelectedIndex == 0)
            {
                statusb = true;
            }
            else
            {
                statusb = false;
            }
                if (btnSaveMailSetting.Text == "Save")
                {
                    DataTable dtexist = g.ReturnData("Select email_id_from from mail_Setting_TB where email_id_from='" + stMailIdFrom + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "");

                    if (dtexist.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "This email id is already exist.");
                        return;
                    }
                    else
                    {
                        DataTable dtsave = g.ReturnData("Insert into mail_Setting_TB (email_id_from,credential, port,specified_emai_to_send,customer_id,created_by_id,status) values('" + stMailIdFrom + "','" + txtpassword.Text.Trim() + "','" + txtPort.Text.Trim() + "','" + emailId + "'," + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(Session["User_ID"]) + "," + statusb + ")");
                        
                        g.ShowMessage(this.Page, "Mail setting is saved successfully.");
                    }
                }
                else
                {
                    DataTable dtexist = g.ReturnData("Select email_id_from from mail_Setting_TB where email_id_from='" + stMailIdFrom + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and mail_setting_id<>" + Convert.ToInt32(lblMailSettingId.Text) + "");

                    if (dtexist.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "This email id is already exist.");
                        return;
                    }
                    else
                    {
                        DataTable dtupdate = g.ReturnData("Update mail_Setting_TB set email_id_from='" + stMailIdFrom + "',credential='" + txtpassword.Text.Trim() + "',port='" + txtPort.Text.Trim() + "',specified_emai_to_send='" + emailId + "',customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + ",created_by_id=" + Convert.ToInt32(Session["User_ID"]) + ",status=" + statusb + " where mail_setting_id =" + Convert.ToInt32(lblMailSettingId.Text) + "");
                       
                        g.ShowMessage(this.Page, "Mail setting is updated successfully.");
                    }
                }
                bindMailSettingGrid();
                MultiView1.ActiveViewIndex = 0;
                txtEmail.Text = txtSpecifyMailId.Text = txtpassword.Text = txtPort.Text = string.Empty;
                dtAddEmailId = null;
                btnAddMailSetting.Focus();
                btnSaveMailSetting.Text = "Save";
           
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseSaveMailSetting_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        txtEmail.Text = txtSpecifyMailId.Text = string.Empty;
        dtAddEmailId = null;
        btnAddMailSetting.Focus();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int cnt = 0;
            if (ViewState["dtAddEmailId"] != null)
            {
                dtAddEmailId = (DataTable)ViewState["dtAddEmailId"];
            }
            else
            {
                DataColumn emailidTo = dtAddEmailId.Columns.Add("mailid");
            }
            DataRow dr = dtAddEmailId.NewRow();
            stEmailid = txtSpecifyMailId.Text.Trim();
            stEmailid = Regex.Replace(stEmailid, @"\s+", " ");
            dr[0] = stEmailid;
            if (dtAddEmailId.Rows.Count > 0)
            {
                for (int f = 0; f < dtAddEmailId.Rows.Count; f++)
                {
                    string checkDuplicateEntry = dtAddEmailId.Rows[f][0].ToString();
                    if (checkDuplicateEntry == stEmailid)
                    {
                        cnt++;
                    }
                    else
                    {

                    }
                }
                if (cnt > 0)
                {
                    g.ShowMessage(this.Page, "Already exist");

                }
                else
                {
                    dtAddEmailId.Rows.Add(dr);
                    txtSpecifyMailId.Text = string.Empty;
                }
            }
            else
            {
                dtAddEmailId.Rows.Add(dr);
                txtSpecifyMailId.Text = string.Empty;
            }
            ViewState["dtAddEmailId"] = dtAddEmailId;
            txtSpecifyMailId.Text = string.Empty;
            grdAddEmailIdToSend.DataSource = dtAddEmailId;
            grdAddEmailIdToSend.DataBind();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdmailSetting_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdmailSetting.PageIndex = e.NewPageIndex;
        bindMailSettingGrid();
    }
    protected void btnEditMailSetting_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblMailSettingId.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtEmail.Focus();
           
               DataTable dtmaildata = g.ReturnData("SELECT mail_Setting_TB.customer_id, mail_Setting_TB.status, mail_Setting_TB.mail_setting_id, mail_Setting_TB.email_id_from, mail_Setting_TB.credential, mail_Setting_TB.port, mail_Setting_TB.specified_emai_to_send FROM mail_Setting_TB where mail_setting_id ="+ Convert.ToInt32(lblMailSettingId.Text)+"");

            txtEmail.Text = dtmaildata.Rows[0]["email_id_from"].ToString();
                if (Convert.ToBoolean( dtmaildata.Rows[0]["status"].ToString())== true)
                {
                    rd_status.SelectedIndex = 0;
                }
                else
                {
                    rd_status.SelectedIndex = 1;
                }
                txtpassword.Text = dtmaildata.Rows[0]["credential"].ToString();
                txtPort.Text = dtmaildata.Rows[0]["port"].ToString();
                string emailspecified = dtmaildata.Rows[0]["specified_emai_to_send"].ToString();
                string[] strarray = emailspecified.Split(',');
                ViewState["dtAddEmailId"] = null;
                for (int i = 0; i < strarray.Count(); i++)
                {
                    if (ViewState["dtAddEmailId"] == null)
                    {
                        dtAddEmailId = new DataTable();
                        DataColumn emailidTo = dtAddEmailId.Columns.Add("mailid");
                    }
                    else
                    {
                        dtAddEmailId = (DataTable)ViewState["dtAddEmailId"];
                    }
                    DataRow dr = dtAddEmailId.NewRow();
                    dr[0] = strarray[i].ToString();
                    dtAddEmailId.Rows.Add(dr);
                    ViewState["dtAddEmailId"] = dtAddEmailId;
                }
                grdAddEmailIdToSend.DataSource = dtAddEmailId;
                grdAddEmailIdToSend.DataBind();
           
            btnSaveMailSetting.Text = "Update";
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddMailSetting_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        txtEmail.Focus();

    }
    protected void btnDeleteEmailId_Click(object sender, EventArgs e)
    {
        LinkButton imgdelete = (LinkButton)sender;
        string mailid = imgdelete.CommandArgument;
        dtAddEmailId = (DataTable)ViewState["dtAddEmailId"];
        foreach (DataRow d in dtAddEmailId.Rows)
        {
            if (d[0].ToString() == mailid)
            {
                d.Delete();
                dtAddEmailId.AcceptChanges();
                break;
            }
        }
        grdAddEmailIdToSend.DataSource = dtAddEmailId;
        grdAddEmailIdToSend.DataBind();
        txtSpecifyMailId.Text = string.Empty;
    }
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("MailSetting.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddMailSetting.Visible = true;
                }
                else
                {
                    btnAddMailSetting.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    grdmailSetting.Columns[5].Visible = true;
                    //for (int i = 0; i < grdmailSetting.Rows.Count; i++)
                    //{
                    //    LinkButton lnk = (LinkButton)grdmailSetting.Rows[i].FindControl("btnEditMailSetting");
                    //    lnk.Enabled = true;
                    //}
                }
                else
                {
                    grdmailSetting.Columns[5].Visible = false;
                    //for (int i = 0; i < grdmailSetting.Rows.Count; i++)
                    //{
                    //    LinkButton lnk = (LinkButton)grdmailSetting.Rows[i].FindControl("btnEditMailSetting");
                    //    lnk.Enabled = false;
                    //}
                }
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
}