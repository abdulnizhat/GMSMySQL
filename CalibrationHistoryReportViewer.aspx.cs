using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CalibrationHistoryReportViewer : System.Web.UI.Page
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
                        ds1 = g.ReturnData1("Select cb.calibration_transaction_id,gt.gauge_sr_no, gt.gauge_type, cb.calibration_schedule_id, DATE_FORMAT(cb.calibration_date, '%d/%m/%Y') as calibration_date, cb.calibration_cost, cb.calibration_hours,cb.calibration_status,cb.certification_no,cb.humidity,cb.other,cb.pressure,cb.temprature,cb.tollerance_go,cb.tollerance_no_go,DATE_FORMAT(Case when(cs.frequency_type='MONTH') then DATE_ADD(cb.calibration_date,Interval cs.calibration_frequency MONTH )else DATE_ADD(cb.calibration_date,Interval cs.calibration_frequency YEAR ) End,'%d/%m/%Y') AS next_due_date , (Select SUM(calibration_cost) from calibration_transaction_TB  where gauge_id='" + gaugeId + "') as calibCost, gt.gauge_sr_no  from calibration_transaction_TB as cb Left Outer join calibration_schedule_TB as cs ON cb.calibration_schedule_id=cs.calibration_schedule_id Left Outer join gaugeMaster_TB as gt ON cb.gauge_id=gt.gauge_id where  cb.status=1 and cb.gauge_id='" + gaugeId + "' Order by cb.calibration_transaction_id DESC ");
                        ds2 = g.ReturnData1("Select gt.gauge_id,gt.customer_id,gt.gauge_sr_no, gt.gauge_Manufature_Id, gt.gauge_name, gt.gauge_type,gt.size_range, gt.resolution,gt.go_tollerance_plus,gt.go_tollerance_minus, gt.no_go_tollerance_plus,gt.no_go_tollerance_minus,gt.go_were_limit,gt.least_count, gt.permisable_error1,gt.permisable_error2,gt.store_location, gt.current_location,gt.purchase_cost , ( (Select DATE_FORMAT(cs.next_due_date, '%d/%m/%Y') from calibration_schedule_TB as cs where gauge_id='" + gaugeId + "' order by calibration_schedule_id DESC LIMIT 1)) as next_due_date from gaugeMaster_TB as gt where gt.gauge_id='" + gaugeId + "'");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/CalibrationHistoryReport.rdlc");
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