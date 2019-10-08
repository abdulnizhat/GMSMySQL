using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MSADueStatusReportViewer : System.Web.UI.Page
{
    Genreal g = new Genreal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                DataSet ds1 = new DataSet();
                try
                {


                    if (Request.QueryString["calibId"] != null)
                    {
                        string strmsaScheduleId = Request.QueryString["calibId"].ToString();
                        string[] str = strmsaScheduleId.Split(',');
                        int msaId = Convert.ToInt32(str[0].ToString());
                        string strQuery = @"Select cs.msa_schedule_id, cs.calibrate_id, sp.supplier_name as Calibrator,
    cs.last_calibrated_by, sp1.supplier_name as LasCalibratedBy , gt.cycles, cs.calibration_frequency, cs.calibration_hours,
     cs.frequency_type, DATE_FORMAT(cs.last_calibration_date,'%d/%m/%Y') as last_calibration_date, DATE_FORMAT(cs.next_due_date, '%d/%m/%Y') as next_due_date, 
     cs.customer_id, ct.customer_name, cs.created_by_id, em.employee_name, gt.gauge_name,gt.size_range, cs.gauge_id, cs.bias,cs.linearity,cs.stability 
       from msa_schedule_TB as cs Left Outer Join customer_TB as ct ON cs.customer_id=ct.customer_id Left Outer Join supplier_TB as sp 
         ON cs.calibrate_id=sp.supplier_id Left Outer Join supplier_TB as sp1 ON cs.last_calibrated_by=sp1.supplier_id Left Outer Join gaugeMaster_TB as gt
          ON cs.gauge_id=gt.gauge_id Left Outer Join employee_TB as em ON cs.created_by_id=em.employee_id Where cs.msa_schedule_id=" + msaId + "";
                        ds1 = g.ReturnData1(strQuery);
                        //ds1 = g.ReturnData1("Select cs.calibration_schedule_id, cs.calibrate_id, sp.supplier_name as Calibrator, cs.last_calibrated_by, sp1.supplier_name as LasCalibratedBy , gt.cycles, cs.calibration_frequency, cs.calibration_hours, cs.frequency_type, CONVERT(nvarchar,cs.last_calibration_date,103) as last_calibration_date, CONVERT(nvarchar,cs.next_due_date,103) as next_due_date, CONVERT(nvarchar,cs.projected_calib_schedule,103) as projected_calib_schedule, cs.customer_id, ct.customer_name, cs.created_by_id, em.employee_name, gt.gauge_name, cs.gauge_id, cs.bias,cs.linearity,cs.stability, Convert(nvarchar, (CASE when (cs.frequency_type='YEAR') then DATEADD(YEAR,CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)  else DATEADD(MONTH, CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)   end ),103) as MSADate from calibration_schedule_TB as cs Left Outer Join customer_TB as ct ON cs.customer_id=ct.customer_id Left Outer Join supplier_TB as sp ON cs.calibrate_id=sp.supplier_id Left Outer Join supplier_TB as sp1 ON cs.last_calibrated_by=sp1.supplier_id Left Outer Join gaugeMaster_TB as gt ON cs.gauge_id=gt.gauge_id Left Outer Join employee_TB as em ON cs.created_by_id=em.employee_id Where cs.calibration_schedule_id='" + calibId + "'");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();

                            ReportViewer1.LocalReport.ReportPath = MapPath("~/MSADueStatusReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds1.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);
                        }


                    }
                    else if (Session["PrintMSAduestatusData"] != null)
                    {
                        DataTable dt2;
                        dt2 = (DataTable)(Session["PrintMSAduestatusData"]);
                        if (dt2.Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();

                            ReportViewer1.LocalReport.ReportPath = MapPath("~/MSADueStatusReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", dt2);
                            ReportViewer1.LocalReport.DataSources.Add(rep);
                        }
                    }
                    DataTable dtcust = new DataTable();
                    dtcust = g.GetCustomerDetails(Convert.ToInt32(Session["Customer_ID"]));
                    ReportDataSource repcust = new ReportDataSource("DataSetcust", dtcust);
                    ReportViewer1.LocalReport.DataSources.Add(repcust);
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