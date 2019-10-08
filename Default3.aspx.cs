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

public partial class Default3 : System.Web.UI.Page
{
    Genreal g = new Genreal();
    protected void Page_Load(object sender, EventArgs e)
    {


       
        for (int i = 0; i < 4; i++)
        {
            grdGauge.DataSource = null;
            grdGauge.DataBind();


            Label lb = new Label();
            lb.Text= "Ram" + '_' + i.ToString();
            DataTable dt = g.ReturnData("Select employee_name,mobile_no from employee_tb");
            GridView grd = new GridView();
            grd.DataSource = dt;
            grd.DataBind();
            grdGauge.DataSource = dt;
            grdGauge.DataBind();
           // tablecell.Text=st.ToString();
            tablecell.Controls.Add(lb);
            tablecell.Controls.Add(grd);
            


        }


        //string[] strarryEmailId = sendTo.Split(',');
        //string[] strsmtpget = senderMailId.Split('@');
        //string a = strsmtpget[1].ToString();
        //string b = "smtp.";
        //b = b + a;
        //for (int i = 0; i < strarryEmailId.Count(); i++)
        //{

        MailMessage objMail = new MailMessage();

        string strSendTo = "nizhatparveen@gmail.com";
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.To.Add(strSendTo);
        mail.From = new MailAddress("aarushiqualitynfogms@gmail.com");
        mail.Subject = "Gauge Due Status";
        mail.Body = "<br><br>Hi Dear ..!!<br><br><br> ";
        mail.Body += "Your gauge next due status date as below list:";
        mail.Body += GetGridviewData(Table1);
        mail.Body += "<br><br><br><b>Gauge Management System!!</b><br><br><br>";
        mail.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient();
        smtp.UseDefaultCredentials = false;
        smtp.Host = "smtp.gmail.com";// "smtp.gmail.com";
        smtp.Credentials = new System.Net.NetworkCredential("aarushiqualitynfogms@gmail.com", "aarush@1234");
        smtp.EnableSsl = true;
        smtp.Port = Convert.ToInt32("587");
        smtp.Send(mail);
    }


    private string GetGridviewData(Table Table1)
    {
        StringBuilder strBuilder = new StringBuilder();
        StringWriter strWriter = new StringWriter(strBuilder);
        HtmlTextWriter htw = new HtmlTextWriter(strWriter);
       // Table1.AllowPaging = false;
        Table1.RenderControl(htw);
        return strBuilder.ToString();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}