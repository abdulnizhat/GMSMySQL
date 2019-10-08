using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MSAReportViewer : System.Web.UI.Page
{
    Genreal g = new Genreal();
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
                        DataTable dt2 = new DataTable();
                        DataTable dt1 = new DataTable();
                        DataSet ds1 = new DataSet();
                        DataSet ds2 = new DataSet();
                        string strQuery = @"  Select ms.msa_transaction_id, ms.calibration_schedule_id, 
                                      DATE_FORMAT(ms.msa_date,'%d/%m/%Y') as msa_date, 
                                      ms.msa_hours,ms.msa_status,ms.gauge_id, ms.msa_report_no,
                                      ms.humidity,ms.other,ms.pressure,ms.temprature, 
                                      cs.bias,cs.linearity, cs.stability from msa_transaction_TB 
                                      as ms Left Outer Join msa_schedule_TB cs 
                                      ON ms.calibration_schedule_id=cs.msa_schedule_id
                                       where  ms.status=1 and ms.gauge_id=" + gaugeId + "";
                        ds1 = g.ReturnData1(strQuery);
                        //ds1 = g.ReturnData1("Select ms.msa_transaction_id, ms.calibration_schedule_id, CONVERT(nvarchar, ms.msa_date,103) as msa_date, ms.msa_hours,ms.msa_status,ms.gauge_id, ms.msa_report_no,ms.humidity,ms.other,ms.pressure,ms.temprature, cs.bias,cs.linearity, cs.stability from msa_transaction_TB as ms Left Outer Join calibration_schedule_TB cs ON ms.calibration_schedule_id=cs.calibration_schedule_id where  ms.status=1 and ms.gauge_id='" + gaugeId + "' ");
                        ds2 = g.ReturnData1("Select gt.gauge_id,gt.customer_id, gt.gauge_Manufature_Id, gt.gauge_name, gt.gauge_type,gt.size_range, gt.resolution,gt.go_tollerance_plus,gt.go_tollerance_minus, gt.no_go_tollerance_plus,gt.no_go_tollerance_minus,gt.go_were_limit,gt.least_count, gt.permisable_error1,gt.permisable_error2,gt.store_location, gt.current_location,gt.purchase_cost from gaugeMaster_TB as gt where gt.gauge_id='" + gaugeId + "'");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/MSAReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds1.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);

                        }
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            ReportDataSource src2 = new ReportDataSource("DataSet2", ds2.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(src2);
                        }
                        DataTable dtcust = new DataTable();
                        dtcust = g.GetCustomerDetails(Convert.ToInt32(Session["Customer_ID"]));
                        ReportDataSource repcust = new ReportDataSource("DataSetcust", dtcust);
                        ReportViewer1.LocalReport.DataSources.Add(repcust);
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
}