using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                displayDueStatus();
                displayMsaDueStatus();
                displayIssueReturnPending();
                
                displayGraph();
                sendMail();
                displayExclusiveReport();
               // displayRestDetailsofExclusiveReport();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void displayRestDetailsofExclusiveReport()
    {
        int customerId = Convert.ToInt32(Session["Customer_ID"]);
        string stprocedure = "spExReportForRestDetails";

        #region  verification due
        DataSet ds1 = new DataSet();
        ds1 = q.ProcdureWithTwoParam(stprocedure, 1, customerId);
        if (ds1 != null)
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                lblVerificationduecountlmonth.Text = ds1.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet ds2 = new DataSet();
        ds2 = q.ProcdureWithTwoParam(stprocedure, 2, customerId);
        if (ds2 != null)
        {
            if (ds2.Tables[0].Rows.Count > 0)
            {
                lblVerificationduecountcmonth.Text = ds2.Tables[0].Rows.Count.ToString();
            }

        }
        #endregion

        #region  Validation due
        DataSet ds3 = new DataSet();
        ds3 = q.ProcdureWithTwoParam(stprocedure, 3, customerId);
        if (ds3 != null)
        {
            if (ds3.Tables[0].Rows.Count > 0)
            {
                lblvalidationDuecountlmonth.Text = ds3.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet ds4 = new DataSet();
        ds4 = q.ProcdureWithTwoParam(stprocedure, 4, customerId);
        if (ds4 != null)
        {
            if (ds4.Tables[0].Rows.Count > 0)
            {
                lblvalidationDuecountcmonth.Text = ds4.Tables[0].Rows.Count.ToString();
            }

        }
        #endregion
        DateTime tdate = DateTime.Now;

        DateTime tdate1 = tdate.AddDays(15);
        string tdate2 = tdate1.ToString("dd/MM/yyyy");

        #region  verification completed
        lblverificationcom.Text = "Total No. of Gauge completed for Calibration Upto " + tdate2;
        DataSet ds5 = new DataSet();
        ds5 = q.ProcdureWithTwoParam(stprocedure, 5, customerId);
        if (ds5 != null)
        {
            if (ds5.Tables[0].Rows.Count > 0)
            {
                lblverificationcomcount.Text = ds5.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet ds6 = new DataSet();
        ds6 = q.ProcdureWithTwoParam(stprocedure, 6, customerId);
        if (ds6 != null)
        {
            if (ds6.Tables[0].Rows.Count > 0)
            {
                lblverificationcomlmonth.Text = ds6.Tables[0].Rows[0][0].ToString();
            }

        }

        DataSet ds7 = new DataSet();
        ds7 = q.ProcdureWithTwoParam(stprocedure, 7, customerId);
        if (ds7 != null)
        {
            if (ds7.Tables[0].Rows.Count > 0)
            {
                lblverificationcomcmonth.Text = ds7.Tables[0].Rows[0][0].ToString();
            }

        }
        #endregion

        #region validation completed
        lblvalidationcom.Text = "Total No. of Gauge completed for MSA Upto " + tdate2;

        DataSet ds8 = new DataSet();
        ds8 = q.ProcdureWithTwoParam(stprocedure, 8, customerId);
        if (ds8 != null)
        {
            if (ds8.Tables[0].Rows.Count > 0)
            {
                lblvalidationcomcount.Text = ds8.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet ds9 = new DataSet();
        ds9 = q.ProcdureWithTwoParam(stprocedure, 9, customerId);
        if (ds9 != null)
        {
            if (ds9.Tables[0].Rows.Count > 0)
            {
                lblvalidationcomcountlmonth.Text = ds9.Tables[0].Rows[0][0].ToString();
            }

        }

        DataSet ds10 = new DataSet();
        ds10 = q.ProcdureWithTwoParam(stprocedure, 10, customerId);
        if (ds10 != null)
        {
            if (ds10.Tables[0].Rows.Count > 0)
            {
                lblvalidationcomcountcmonth.Text = ds10.Tables[0].Rows[0][0].ToString();
            }

        }
        #endregion


        #region purchase cost for returned fixture

        DataSet ds11 = new DataSet();
        ds11 = q.ProcdureWithTwoParam(stprocedure, 11, customerId);
        if (ds11 != null)
        {
            if (ds11.Tables[0].Rows.Count > 0)
            {
                lblRPCostYTD.Text = ds11.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet ds12 = new DataSet();
        ds12 = q.ProcdureWithTwoParam(stprocedure, 12, customerId);
        if (ds12 != null)
        {
            if (ds12.Tables[0].Rows.Count > 0)
            {
                lblRPCostlastMonth.Text = ds12.Tables[0].Rows[0][0].ToString();
            }

        }

        DataSet ds13 = new DataSet();
        ds13 = q.ProcdureWithTwoParam(stprocedure, 13, customerId);
        if (ds13 != null)
        {
            if (ds13.Tables[0].Rows.Count > 0)
            {
                lblRPCostcurrentMonth.Text = ds13.Tables[0].Rows[0][0].ToString();
            }

        }
        #endregion

        
    }
    private void displayExclusiveReport()
    {
        int customerId = Convert.ToInt32(Session["Customer_ID"]);
        string stprocedurePurchaseCost = "spExReportForGauseDetailReport";
        DataSet dspCost = new DataSet();
        //for total Purchase cost
        #region for total Purchase cost
        dspCost = q.ProcdureWithTwoParam(stprocedurePurchaseCost, 1, customerId);
        if (dspCost != null)
        {
            if (dspCost.Tables[0].Rows.Count > 0)
            {
                lblPCostlastMonth.Text = dspCost.Tables[0].Rows[0][0].ToString();
            }

        }
        DataSet dspCost1 = new DataSet();
        dspCost1 = q.ProcdureWithTwoParam(stprocedurePurchaseCost, 4, customerId);
        if (dspCost1 != null)
        {
            if (dspCost1.Tables[0].Rows.Count > 0)
            {
                lblPCostcurrentMonth.Text = dspCost1.Tables[0].Rows[0][0].ToString();
            }
        }
        DataSet dspCost2 = new DataSet();
        dspCost2 = q.ProcdureWithTwoParam(stprocedurePurchaseCost, 3, customerId);
        if (dspCost2 != null)
        {
            if (dspCost2.Tables[0].Rows.Count > 0)
            {
                lblPCostYTD.Text = dspCost2.Tables[0].Rows[0][0].ToString();
            }
        }
        #endregion

        string stprocedure = "spExReportForCalibHistory";
        DataSet ds = new DataSet();
        //for verification cost
        #region  for verification cost
        ds = q.ProcdureWithTwoParam(stprocedure, 1, customerId);
        if (ds != null)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblcalibcostcurrentM.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        DataSet ds1 = new DataSet();
        ds1 = q.ProcdureWithTwoParam(stprocedure, 2, customerId);
        if (ds1 != null)
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                lblcalibcostLastM.Text = ds1.Tables[0].Rows[0][0].ToString();
            }
        }
        DataSet ds2 = new DataSet();
        ds2 = q.ProcdureWithTwoParam(stprocedure, 4, customerId);
        if (ds2 != null)
        {
            if (ds2.Tables[0].Rows.Count > 0)
            {

                lblcalibcostYTD.Text = ds2.Tables[0].Rows[0][0].ToString();
            }
        }
        #endregion

      
    }

    private void sendMail()
    {
        try
        {
            string senderMailId = null;
            string password = null;
            string sendTo = null;
            int port = 0;

            DateTime date = new DateTime();
            date = DateTime.Now;
            stDate = date.ToString("yyyy-MM-dd");
            DataTable dtcheckSendMailOrNotToday = g.ReturnData("Select customer_id from sendMailHistoryTB where customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and DATE_FORMAT(send_Date, '%Y-%m-%d')='" + stDate + "'"); // and user_id = " + Convert.ToInt32(Session["User_ID"]) + "

            if (dtcheckSendMailOrNotToday.Rows.Count > 0)
            {
                return;
            }
            else
            {
                DataTable dtresultgetMailId = g.ReturnData("Select email_id_from, credential, port, specified_emai_to_send from mail_Setting_TB where customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status = True");

                if (dtresultgetMailId.Rows.Count > 0)
                {

                    senderMailId = dtresultgetMailId.Rows[0]["email_id_from"].ToString();
                    password = dtresultgetMailId.Rows[0]["credential"].ToString();
                    port = Convert.ToInt32(dtresultgetMailId.Rows[0]["port"].ToString());
                    sendTo = dtresultgetMailId.Rows[0]["specified_emai_to_send"].ToString();


                    DataTable dtresultCustEmailId = g.ReturnData("Select email from customer_TB where customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + "");

                    string cusmailid = null;

                    cusmailid = dtresultCustEmailId.Rows[0]["email"].ToString();
                    string[] a = sendTo.Split(',');
                    if (!String.IsNullOrEmpty(cusmailid))
                    {
                        if (!String.IsNullOrEmpty(sendTo))
                        {

                            if (!a.Contains(cusmailid))
                            {
                                sendTo = sendTo + ',' + cusmailid;
                            }
                        }
                        else
                        {
                            sendTo = cusmailid;
                        }

                        if (!String.IsNullOrEmpty(senderMailId) && !String.IsNullOrEmpty(password) && port != 0)
                        {
                            lblcustmailid.Text = cusmailid;
                            SendMailFunction(senderMailId, password, port, sendTo, "ALL");
                        }

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(senderMailId) && !String.IsNullOrEmpty(password) && port != 0)
                        {
                            lblcustmailid.Text = cusmailid;
                            SendMailFunction(senderMailId, password, port, sendTo, "ALL");
                        }

                    }



                }
                else
                {

                    return;
                }
            }

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            //  g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void SendMailFunction(string senderMailId, string password, int port, string sendTo, string who)
    {
        try
        {
            if (String.IsNullOrEmpty(sendTo))
            {
                return;
            }
            ViewState["dtRemainingList"] = null;
            ViewState["dtissuedlist"] = null;
            grdsendCustomerGaugeStatus.DataSource = null;
            grdsendCustomerGaugeStatus.DataBind();
            string sendQuery = null;
            if (who == "ALL")
            {
                #region All
                string[] strarryEmailId = sendTo.Split(',');
                string[] strsmtpget = senderMailId.Split('@');
                string a = strsmtpget[1].ToString();
                string b = "smtp.";
                b = b + a;
                
               
               
                    int cnt = 0;

                    DataTable dtRemainingList = new DataTable();
                    DataTable dtissuedlist = new DataTable();

               
                    sendQuery = @"Select distinct cs.calibration_schedule_id, cs.gauge_id, gm.gauge_sr_no,gm.size_range, gm.gauge_name,DATE_FORMAT(cs.next_due_date,'%d/%m/%Y') as next_due_date,
dt.department_name as current_location
from calibration_schedule_TB as cs
                Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id 
           
            left outer join employee_tb em on em.employee_id=gm.created_by_id
            left outer join department_tb dt on dt.department_id=em.department_id
                where cs.status=1 
and cs.next_due_date <= DATE_ADD(now(), INTERVAL 15 DAY) and cs.gauge_id NOT IN (Select ct.gauge_id from calibration_transaction_TB ct   where ct.calibration_schedule_id=cs.calibration_schedule_id )
 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " ";

                    DataTable getnextDueStatus = g.ReturnData(sendQuery);

                    if (getnextDueStatus.Rows.Count > 0)
                    {
                        for (int j = 0; j < getnextDueStatus.Rows.Count; j++)
                        {
                            string stissueexist = @"Select ist.gauge_id,em.employee_id,em.employee_name,em.email from issued_status_tb ist left outer join employee_tb em on em.employee_id=ist.issued_to_employee_id where ist.issued_to_type='Employee' and ist.gauge_id=" + Convert.ToInt32(getnextDueStatus.Rows[j]["gauge_id"].ToString()) + " and issued_status='OPEN' ";
                            DataTable dt = g.ReturnData(stissueexist);
                            if (dt.Rows.Count > 0)
                            {

                                if (ViewState["dtissuedlist"] != null)
                                {
                                    dtissuedlist = (DataTable)ViewState["dtissuedlist"];
                                }
                                else
                                {
                                    DataColumn gauge_id = dtissuedlist.Columns.Add("gauge_id");
                                    DataColumn employee_id = dtissuedlist.Columns.Add("employee_id");
                                    DataColumn email = dtissuedlist.Columns.Add("email");
                                    DataColumn employee_name = dtissuedlist.Columns.Add("employee_name");

                                }
                                DataRow dr = dtissuedlist.NewRow();

                                dr[0] = dt.Rows[0]["gauge_id"].ToString();
                                dr[1] = dt.Rows[0]["employee_id"].ToString();
                                dr[2] = dt.Rows[0]["email"].ToString();
                                dr[3] = dt.Rows[0]["employee_name"].ToString();
                                if (dtissuedlist.Rows.Count > 0)
                                {
                                    for (int f = 0; f < dtissuedlist.Rows.Count; f++)
                                    {
                                        string checkDuplicateEntry = dtissuedlist.Rows[f][1].ToString();
                                        if (checkDuplicateEntry == dt.Rows[0]["employee_id"].ToString())
                                        {
                                            cnt++;
                                        }

                                    }
                                    if (cnt == 0)
                                    {
                                        dtissuedlist.Rows.Add(dr);
                                        ViewState["dtissuedlist"] = dtissuedlist;
                                    }
                                }
                                else
                                {
                                    dtissuedlist.Rows.Add(dr);
                                    ViewState["dtissuedlist"] = dtissuedlist;
                                }


                            }
                            else
                            {
                                if (ViewState["dtRemainingList"] != null)
                                {
                                    dtRemainingList = (DataTable)ViewState["dtRemainingList"];
                                }
                                else
                                {
                                    DataColumn scheduleid = dtRemainingList.Columns.Add("Schedule Id");
                                    DataColumn gaugeid = dtRemainingList.Columns.Add("Gauge Id");
                                    DataColumn srno = dtRemainingList.Columns.Add("Sr. No");
                                    DataColumn size = dtRemainingList.Columns.Add("Size/Range");
                                    DataColumn gaugename = dtRemainingList.Columns.Add("Gauge Name");
                                    DataColumn duedate = dtRemainingList.Columns.Add("Due Date");
                                    DataColumn curlocation = dtRemainingList.Columns.Add("Current Location");
                                }
                                DataRow dr = dtRemainingList.NewRow();

                                dr[0] = getnextDueStatus.Rows[j]["calibration_schedule_id"].ToString();
                                dr[1] = getnextDueStatus.Rows[j]["gauge_id"].ToString();
                                dr[2] = getnextDueStatus.Rows[j]["gauge_sr_no"].ToString();
                                dr[3] = getnextDueStatus.Rows[j]["size_range"].ToString();
                                dr[4] = getnextDueStatus.Rows[j]["gauge_name"].ToString();
                                dr[5] = getnextDueStatus.Rows[j]["next_due_date"].ToString();
                                dr[6] = getnextDueStatus.Rows[j]["current_location"].ToString();
                                dtRemainingList.Rows.Add(dr);
                                ViewState["dtRemainingList"] = dtRemainingList;
                            }



                        }

                        if (dtissuedlist.Rows.Count > 0)
                        {

                            for (int k = 0; k < dtissuedlist.Rows.Count; k++)
                            {


                                sendQuery = @"Select distinct cs.calibration_schedule_id as 'Schedule Id', cs.gauge_id as 'Gauge Id', gm.gauge_sr_no as 'Sr. No',gm.size_range as 'Size/Range', gm.gauge_name as 'Gauge Name',DATE_FORMAT(cs.next_due_date,'%d/%m/%Y')  as 'Due Date',
dt.department_name as 'current location'
from calibration_schedule_TB as cs
                Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id 
            left outer join issued_status_tb ist1 on cs.gauge_id=ist1.gauge_id  
            left outer join employee_tb em on em.employee_id=ist1.issued_to_employee_id
            left outer join department_tb dt on dt.department_id=em.department_id
                where cs.status=1 
and cs.next_due_date <= DATE_ADD(now(), INTERVAL 15 DAY) and cs.gauge_id NOT IN (Select ct.gauge_id from calibration_transaction_TB ct   where ct.calibration_schedule_id=cs.calibration_schedule_id )
and cs.gauge_id in(Select ist.gauge_id from issued_status_tb ist where ist.issued_to_type='Employee' and issued_to_employee_id=" + Convert.ToInt32(dtissuedlist.Rows[k]["employee_id"].ToString()) + ") and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and ist1.issued_to_type='Employee' and ist1.issued_to_employee_id=" + Convert.ToInt32(dtissuedlist.Rows[k]["employee_id"].ToString()) + "";
                                DataTable dtissuedlistemp = new DataTable();
                                dtissuedlistemp = g.ReturnData(sendQuery);
                                Label lb = new Label();
                                lb.Text = dtissuedlist.Rows[k]["employee_name"].ToString();
                                GridView grd = new GridView();
                                grd.DataSource = dtissuedlistemp;
                                grd.DataBind();
                                tablecell.Controls.Add(lb);
                                tablecell.Controls.Add(grd);


                                MailMessage objMail = new MailMessage();
                                string strSendTo = dtissuedlist.Rows[k]["email"].ToString();
                                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                                mail.To.Add(strSendTo);
                                mail.From = new MailAddress(senderMailId);
                                mail.Subject = "Gauge Due Status";
                                mail.Body = "<br><br>Hi Dear ..!!<br><br><br> ";
                                mail.Body += "Your gauge next due status date as below list:";
                                mail.Body += GetTableData(Table1);
                                mail.Body += "<br><br><br><b>Gauge Management System!!</b><br><br><br>";
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.UseDefaultCredentials = false;
                                smtp.Host = b;// "smtp.gmail.com";
                                smtp.Credentials = new System.Net.NetworkCredential(senderMailId, password);
                                smtp.EnableSsl = true;
                                smtp.Port = port;
                                smtp.Send(mail);
                                tablecell.Controls.Clear();

                            }

                        }
                        //for (int i = 0; i < strarryEmailId.Count(); i++)
                        //{
                        if (dtRemainingList.Rows.Count > 0)
                        {
                            if (dtissuedlist.Rows.Count > 0)
                            {

                                for (int k = 0; k < dtissuedlist.Rows.Count; k++)
                                {


                                    sendQuery = @"Select distinct cs.calibration_schedule_id as 'Schedule Id', cs.gauge_id as 'Gauge Id', gm.gauge_sr_no as 'Sr. No',gm.size_range as 'Size/Range', gm.gauge_name as 'Gauge Name',DATE_FORMAT(cs.next_due_date,'%d/%m/%Y') as 'Due Date',
dt.department_name as 'current location'
from calibration_schedule_TB as cs
                Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id 
            left outer join issued_status_tb ist1 on cs.gauge_id=ist1.gauge_id  
            left outer join employee_tb em on em.employee_id=ist1.issued_to_employee_id
            left outer join department_tb dt on dt.department_id=em.department_id
                where cs.status=1 
and cs.next_due_date <= DATE_ADD(now(), INTERVAL 15 DAY) and cs.gauge_id NOT IN (Select ct.gauge_id from calibration_transaction_TB ct   where ct.calibration_schedule_id=cs.calibration_schedule_id )
and cs.gauge_id in(Select ist.gauge_id from issued_status_tb ist where ist.issued_to_type='Employee' and issued_to_employee_id=" + Convert.ToInt32(dtissuedlist.Rows[k]["employee_id"].ToString()) + ") and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and ist1.issued_to_type='Employee' and ist1.issued_to_employee_id=" + Convert.ToInt32(dtissuedlist.Rows[k]["employee_id"].ToString()) + "";
                                    DataTable dtissuedlistemp = new DataTable();
                                    dtissuedlistemp = g.ReturnData(sendQuery);
                                    Label lb = new Label();
                                    lb.Text = dtissuedlist.Rows[k]["employee_name"].ToString();
                                    GridView grd = new GridView();
                                    grd.DataSource = dtissuedlistemp;
                                    grd.DataBind();
                                    tablecell.Controls.Add(lb);
                                    tablecell.Controls.Add(grd);
                                }
                            }

                            Label lb1 = new Label();
                            lb1.Text = "All Remaining Due Gauge List";
                            GridView grd1 = new GridView();
                            grd1.DataSource = dtRemainingList;
                            grd1.DataBind();
                            tablecell.Controls.Add(lb1);
                            tablecell.Controls.Add(grd1);
                            MailMessage objMail = new MailMessage();

                            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                            foreach (var item in strarryEmailId)
                            {
                                // 
                                mail.To.Add(new MailAddress(item));
                            }



                            mail.From = new MailAddress(senderMailId);
                            mail.Subject = "Gauge Due Status";
                            mail.Body = "<br><br>Hi Dear ..!!<br><br><br> ";
                            mail.Body += "Your gauge next due status date as below list:";
                            mail.Body += GetTableData(Table1);
                            //  mail.Body += GetTableData(Table1);
                            mail.Body += "<br><br><br><b>Gauge Management System!!</b><br><br><br>";
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.UseDefaultCredentials = false;
                            smtp.Host = b;// "smtp.gmail.com";
                            smtp.Credentials = new System.Net.NetworkCredential(senderMailId, password);
                            smtp.EnableSsl = true;
                            smtp.Port = port;
                            smtp.Send(mail);
                            tablecell.Controls.Clear();//
                        }
                    }
                    else
                    {
                        if (dtissuedlist.Rows.Count > 0)
                        {

                            for (int k = 0; k < dtissuedlist.Rows.Count; k++)
                            {


                                sendQuery = @"Select distinct cs.calibration_schedule_id as 'Schedule Id', cs.gauge_id as 'Gauge Id', gm.gauge_sr_no as 'Sr. No',gm.size_range as 'Size/Range', gm.gauge_name as 'Gauge Name',DATE_FORMAT(cs.next_due_date,'%d/%m/%Y') as 'Due Date',
dt.department_name as 'current location'
from calibration_schedule_TB as cs
                Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id 
            left outer join issued_status_tb ist1 on cs.gauge_id=ist1.gauge_id  
            left outer join employee_tb em on em.employee_id=ist1.issued_to_employee_id
            left outer join department_tb dt on dt.department_id=em.department_id
                where cs.status=1 
and cs.next_due_date <= DATE_ADD(now(), INTERVAL 15 DAY) and cs.gauge_id NOT IN (Select ct.gauge_id from calibration_transaction_TB ct   where ct.calibration_schedule_id=cs.calibration_schedule_id )
and cs.gauge_id in(Select ist.gauge_id from issued_status_tb ist where ist.issued_to_type='Employee' and issued_to_employee_id=" + Convert.ToInt32(dtissuedlist.Rows[k]["employee_id"].ToString()) + ") and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and ist1.issued_to_type='Employee' and ist1.issued_to_employee_id=" + Convert.ToInt32(dtissuedlist.Rows[k]["employee_id"].ToString()) + "";
                                DataTable dtissuedlistemp = new DataTable();
                                dtissuedlistemp = g.ReturnData(sendQuery);
                                Label lb = new Label();
                                lb.Text = dtissuedlist.Rows[k]["employee_name"].ToString();
                                GridView grd = new GridView();
                                grd.DataSource = dtissuedlistemp;
                                grd.DataBind();
                                tablecell.Controls.Add(lb);
                                tablecell.Controls.Add(grd);



                                MailMessage objMail = new MailMessage();

                                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                                foreach (var item in strarryEmailId)
                                {
                                    // 
                                    mail.To.Add(new MailAddress(item));
                                }



                                mail.From = new MailAddress(senderMailId);
                                mail.Subject = "Gauge Due Status";
                                mail.Body = "<br><br>Hi Dear ..!!<br><br><br> ";
                                mail.Body += "Your gauge next due status date as below list:";
                                mail.Body += GetTableData(Table1);
                                //  mail.Body += GetTableData(Table1);
                                mail.Body += "<br><br><br><b>Gauge Management System!!</b><br><br><br>";
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.UseDefaultCredentials = false;
                                smtp.Host = b;// "smtp.gmail.com";
                                smtp.Credentials = new System.Net.NetworkCredential(senderMailId, password);
                                smtp.EnableSsl = true;
                                smtp.Port = port;
                                smtp.Send(mail);
                                tablecell.Controls.Clear();//
                            }
                        }
                    }
                  


                 

                //}
                #endregion

            }



            DateTime date = new DateTime();
            date = DateTime.Now;
            string strDate = date.ToString("yyyy-MM-dd H:mm:ss");

            DataTable dtsavemailhist = g.ReturnData("Insert into sendMailHistoryTB (customer_id,user_id,send_Date) values(" + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(Session["User_ID"]) + ",'" + strDate + "')");


        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            // g.ShowMessage(this.Page, ex.Message);
        }
    }

    private string GetTableData(Table Table1)
    {
        StringBuilder strBuilder = new StringBuilder();
        StringWriter strWriter = new StringWriter(strBuilder);
        HtmlTextWriter htw = new HtmlTextWriter(strWriter);
        // Table1.AllowPaging = false;
        Table1.RenderControl(htw);
        return strBuilder.ToString();
    }
    //private string GetTableData(Table Table2)
    //{
    //    StringBuilder strBuilder = new StringBuilder();
    //    StringWriter strWriter = new StringWriter(strBuilder);
    //    HtmlTextWriter htw = new HtmlTextWriter(strWriter);
    //    // Table1.AllowPaging = false;
    //    Table2.RenderControl(htw);
    //    return strBuilder.ToString();
    //}
    private void displayIssueReturnPending()
    {
        try
        {
            string query = @"Select st.issued_id, st.issue_type, st.issued_to_type, st.issued_date, st.date_of_return,
            st.gauge_id, gt.gauge_name, gt.gauge_sr_no,gt.size_range, gt.gauge_Manufature_Id, gt.gauge_type,
            case when st.issued_to_type='Employee' then em.employee_name
            else sp.supplier_name end as Name,
            case when st.issued_status='OPEN' then 'PENDING'
            else st.issued_status end as ReturnStatus
            from issued_status_TB as st
            Left Outer Join gaugeMaster_TB as gt
            ON st.gauge_id=gt.gauge_id
            Left Outer Join supplier_TB as sp
            ON st.issued_to_supplier_id=sp.supplier_id
            Left outer Join employee_TB as em
            ON st.issued_to_employee_id=em.employee_id
            where st.status=1 and st.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and st.issued_status='OPEN'";
            DataTable dtPendingIssuedStatus = g.ReturnData(query);
            grdIssue.DataSource = dtPendingIssuedStatus;
            grdIssue.DataBind();

            lbldueforreturn.Text = "Total  Gauges / Instruments Due For Return";
            lbldueforreturncount.Text = dtPendingIssuedStatus.Rows.Count.ToString();
           

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            //g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void displayMsaDueStatus()
    {
        try
        {
            //02/09/2017 By ABdul Due Seprate Table made of this report. DataTable dt = g.ReturnData("Select cs.calibration_schedule_id, cs.gauge_id, gm.gauge_sr_no,gm.gauge_name, cs.next_due_date,(CASE when (cs.frequency_type='YEAR') then DATEADD(YEAR,CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)else DATEADD(MONTH, CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)end) as MSADate from calibration_schedule_TB as cs Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id where cs.status=1 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and (CASE when (cs.frequency_type='YEAR') then DATEADD(YEAR,CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date) else DATEADD(MONTH, CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)end) <= (Getdate()+15)");
            string strQuery = " Select cs.msa_schedule_id, cs.gauge_id, gm.gauge_sr_no,gm.size_range, gm.gauge_name, cs.next_due_date " +
                          " from msa_schedule_TB as cs " +
                           " Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id " +
                            " where cs.status=1 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " " +
                            " and cs.next_due_date <= DATE_ADD(now(), INTERVAL 15 DAY) " +
                " and cs.gauge_id NOT IN (Select mt.gauge_id from msa_transaction_TB mt where mt.calibration_schedule_id=cs.msa_schedule_id)";
            DataTable dt = g.ReturnData(strQuery);
            grdMsaDue.DataSource = dt;
            grdMsaDue.DataBind();


            DateTime tdate = DateTime.Now;

            DateTime tdate1 = tdate.AddDays(15);
            string tdate2 = tdate1.ToString("dd/MM/yyyy");

            lblMSADue.Text = "Total Gauges / Instruments Due for MSA upto " + tdate2;
            lblMSADuecount.Text = dt.Rows.Count.ToString();
            
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    private void displayDueStatus()
    {
        try
        {
            string stprocedure = "spDashBoardQuery";
            DataSet ds = q.ProcdureWithTwoParam(stprocedure, 1, Convert.ToInt32(Session["Customer_ID"]));
            // var result = ds.spDashBoardQuery(Convert.ToInt32(Session["Customer_ID"]), 1).ToList();
            grdDueStatus.DataSource = ds.Tables[0];
            grdDueStatus.DataBind();


            DateTime tdate = DateTime.Now;

            DateTime tdate1 = tdate.AddDays(15);
            string tdate2 = tdate1.ToString("dd/MM/yyyy");

            lblcalibdue.Text = "Total Gauges / Instruments Due for Calibration upto " + tdate2;
            lblcalibduecount.Text = ds.Tables[0].Rows.Count.ToString();



        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    private void displayGraph()
    {
        try
        {
            DateTime tdate1 = DateTime.Now;
            string tdate2 = tdate1.ToString("yyyy-MM-dd");
            DateTime fdate1 = tdate1.AddYears(-1);
            string fdate2 = fdate1.ToString("yyyy-MM-dd");
            DataTable dtdisp = new DataTable();
            int custid = Convert.ToInt32(Session["Customer_ID"]);

            // DataTable dtsch = g.ReturnData("Select (DATENAME(YY, last_calibration_date)+'-'+ Left(DATENAME(MM, last_calibration_date),3) )as Month, (Select  Count(calibration_schedule_id) from calibration_schedule_TB where last_calibration_date >='" + fdate1 + "' and last_calibration_date <='" + tdate1 + "' and DATENAME(MONTH, last_calibration_date)=DATENAME(MONTH, cas.last_calibration_date) and DATENAME(YY, last_calibration_date)=DATENAME(YY,cas.last_calibration_date) and customer_id='" + custid + "')as Scount from calibration_schedule_TB as cas where cas.last_calibration_date >='" + fdate1 + "' and cas.last_calibration_date <='" + tdate1 + "' and cas.customer_id='" + custid + "' order by last_calibration_date");
            DataTable dtsch = g.ReturnData("Select concat_WS('-',EXTRACT(YEAR FROM cas.next_due_date), DATE_FORMAT(cas.next_due_date,'%b') ) as Month, (Select  Count(calibration_schedule_id) from calibration_schedule_TB where DATE_FORMAT(next_due_date, '%Y-%m-%d') >='" + fdate2 + "' and DATE_FORMAT(next_due_date, '%Y-%m-%d') <='" + tdate2 + "' and  MONTHNAME(next_due_date) =MONTHNAME(cas.next_due_date) and EXTRACT(YEAR FROM next_due_date)=EXTRACT(YEAR FROM cas.next_due_date) and  customer_id='" + custid + "' and gauge_id NOT IN (Select ct.gauge_id from aarushquality_.calibration_transaction_TB ct  where ct.calibration_schedule_id=calibration_schedule_id )) as Scount  from calibration_schedule_TB as cas where DATE_FORMAT(cas.next_due_date, '%Y-%m-%d')  >='" + fdate2 + "' and  DATE_FORMAT(cas.next_due_date, '%Y-%m-%d') <='" + tdate2 + "' and  cas.customer_id='" + custid + "'   and cas.gauge_id NOT IN (Select ct.gauge_id from aarushquality_.calibration_transaction_TB ct  where ct.calibration_schedule_id=cas.calibration_schedule_id )  order by next_due_date");

            for (int i = 0; i < dtsch.Rows.Count; i++)
            {
                int cnt = 0;
                if (ViewState["dtdisp"] != null)
                {
                    dtdisp = (DataTable)ViewState["dtdisp"];
                }
                else
                {
                    DataColumn Month = dtdisp.Columns.Add("Month");
                    DataColumn Scount = dtdisp.Columns.Add("Scount");
                    DataColumn Tcount = dtdisp.Columns.Add("Tcount");
                }
                DataRow dr = dtdisp.NewRow();
                dr[0] = dtsch.Rows[i]["Month"].ToString();
                dr[1] = dtsch.Rows[i]["Scount"].ToString();
                dr[2] = 0;
                for (int j = 0; j < dtdisp.Rows.Count; j++)
                {
                    if (dtdisp.Rows[j]["Month"].ToString() == dtsch.Rows[i]["Month"].ToString())
                    {
                        cnt++;
                    }
                }
                if (cnt == 0)
                {
                    dtdisp.Rows.Add(dr);
                }

                ViewState["dtdisp"] = dtdisp;
            }
            // DataTable dttch = g.ReturnData("Select (DATENAME(YY, calibration_date)+'-'+  Left(DATENAME(MM, calibration_date),3) )as Month, (Select  Count(calibration_transaction_id) from calibration_transaction_TB where calibration_date >= '" + fdate1 + "' and calibration_date <='" + tdate1 + "' and DATENAME(MONTH, calibration_date)=DATENAME(MONTH,cts.calibration_date) and DATENAME(YY, calibration_date)=DATENAME(YY,cts.calibration_date) and customer_id='" + custid + "')as Tcount from calibration_transaction_TB cts where  cts.calibration_date >='" + fdate1 + "'  and cts.calibration_date <='" + tdate1 + "' and cts.customer_id='" + custid + "' order by calibration_date");
            DataTable dttch = g.ReturnData("Select concat_WS('-',EXTRACT(YEAR FROM cts.calibration_date), DATE_FORMAT(cts.calibration_date,'%b') ) as Month, (Select  Count(calibration_transaction_id) from calibration_transaction_TB where DATE_FORMAT(calibration_date, '%Y-%m-%d') >= '" + fdate2 + "' and DATE_FORMAT(calibration_date, '%Y-%m-%d') <='" + tdate2 + "' and  MONTHNAME(calibration_date)= MONTHNAME(cts.calibration_date) and EXTRACT(YEAR FROM calibration_date)=EXTRACT(YEAR FROM cts.calibration_date)  and customer_id=" + custid + ") as Tcount  from calibration_transaction_TB cts where DATE_FORMAT(cts.calibration_date, '%Y-%m-%d') >='" + fdate2 + "'  and DATE_FORMAT(cts.calibration_date, '%Y-%m-%d') <='" + tdate2 + "'  and cts.customer_id=" + custid + " order by calibration_date");
            for (int j = 0; j < dttch.Rows.Count; j++)
            {
                int cnt = 0;
                int cnt1 = 0;
                if (ViewState["dtdisp"] != null)
                {
                    dtdisp = (DataTable)ViewState["dtdisp"];
                }

                for (int k = 0; k < dtdisp.Rows.Count; k++)
                {
                    if (dtdisp.Rows[k]["Month"].ToString() == dttch.Rows[j]["Month"].ToString())
                    {
                        dtdisp.Rows[k]["Tcount"] = dttch.Rows[j]["Tcount"];
                        cnt++;
                    }

                }
                if (cnt == 0)
                {

                    if (ViewState["dtdisp"] != null)
                    {
                        dtdisp = (DataTable)ViewState["dtdisp"];
                    }
                    else
                    {
                        DataColumn Month = dtdisp.Columns.Add("Month");
                        DataColumn Scount = dtdisp.Columns.Add("Scount");
                        DataColumn Tcount = dtdisp.Columns.Add("Tcount");
                    }
                    DataRow dr = dtdisp.NewRow();
                    dr[0] = dttch.Rows[j]["Month"].ToString();
                    dr[1] = 0;
                    dr[2] = dttch.Rows[j]["Tcount"].ToString();
                    for (int l = 0; l < dtdisp.Rows.Count; l++)
                    {
                        if (dtdisp.Rows[l]["Month"].ToString() == dttch.Rows[j]["Month"].ToString())
                        {
                            cnt1++;
                        }
                    }
                    if (cnt1 == 0)
                    {
                        dtdisp.Rows.Add(dr);
                    }
                    ViewState["dtdisp"] = dtdisp;
                }
            }
            if (dtdisp.Rows.Count > 0)
            {
                Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                Chart1.Series["Series1"].XValueMember = "Month";
                Chart1.Series["Series1"].YValueMembers = "SCount";
                Chart1.Series["Series2"].XValueMember = "Month";
                Chart1.Series["Series2"].YValueMembers = "TCount";
                Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                //Chart1.Series["Series1"].ToolTip = #VALX;
                Chart1.Series["Series1"].IsValueShownAsLabel = true;

                // Chart1.ChartAreas[0].AxisX.IntervalAutoMode = System.Windows.Charting.IntervalAutoMode.VariableCount;

                Chart1.DataSource = dtdisp;
                Chart1.DataBind();
            }
            Session["dtdisp1"] = dtdisp;
            dtdisp = null;
            ViewState["dtdisp"] = null;

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            //   g.ShowMessage(this.Page, ex.Message);
        }
    }
    // This Method is used to render gridview control
    public string GetGridviewData(GridView gv)
    {
        StringBuilder strBuilder = new StringBuilder();
        StringWriter strWriter = new StringWriter(strBuilder);
        HtmlTextWriter htw = new HtmlTextWriter(strWriter);
        gv.AllowPaging = false;
        gv.RenderControl(htw);
        return strBuilder.ToString();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void grdDueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDueStatus.PageIndex = e.NewPageIndex;
        displayDueStatus();
    }
    protected void grdMsaDue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMsaDue.PageIndex = e.NewPageIndex;
        displayMsaDueStatus();
    }
    protected void grdIssue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdIssue.PageIndex = e.NewPageIndex;
        displayIssueReturnPending();
    }
}