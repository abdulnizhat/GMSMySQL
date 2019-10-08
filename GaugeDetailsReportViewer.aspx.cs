
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;




public partial class GaugeDetailsReportViewer : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                
                try
                {
                if (Request.QueryString["gaugeId"] != null)
                {
                    string getIds = Request.QueryString["gaugeId"].ToString();
                    string[] str = getIds.Split(',');
                    int gaugeId = Convert.ToInt32(str[0].ToString());
                    int customerId = Convert.ToInt32(str[1].ToString());
                  
                    DataTable dat = new DataTable();
                    string stprocedure = "spGaugeDetailsReport";
                    DataSet ds = q.ProcdureWith3Param(stprocedure, 1, customerId, gaugeId);


                 
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Reset();
                       
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/GaugeDetailsReport.rdlc");
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportDataSource rep = new ReportDataSource("DataSet1", ds.Tables[0]);
                        ReportViewer1.LocalReport.DataSources.Add(rep);
                        ReportViewer1.LocalReport.Refresh();
                    }
                    DataTable dtcust = new DataTable();
                    dtcust = g.GetCustomerDetails(Convert.ToInt32(Session["Customer_ID"]));
                    ReportDataSource repcust = new ReportDataSource("DataSetcust", dtcust);
                    ReportViewer1.LocalReport.DataSources.Add(repcust);
                    ReportViewer1.LocalReport.EnableExternalImages = true;
                  }
                 
                }
                catch (Exception ex)
                {
                    g.ShowMessage(this.Page, ex.Message);
                }
            }
           
        }

        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    public Image byteArrayToImage(byte[] byteArrayIn)
    {
        //MemoryStream ms = new MemoryStream(byteArrayIn);
        //Image returnImage = Image.FromStream(ms);
        //returnImage.Save(Server.MapPath("~/Images/Gauge_Details.png"));
        //return returnImage;
        Image returnImage = null;
        using (MemoryStream ms = new MemoryStream(byteArrayIn))
        {
            returnImage = Image.FromStream(ms, true, true);
        }
        returnImage.Save(Server.MapPath("~/Images/Gauge_Details.png"));
        return returnImage;
    }
}