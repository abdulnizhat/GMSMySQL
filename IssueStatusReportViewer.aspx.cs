using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IssueStatusReportViewer : System.Web.UI.Page
{
    Genreal g = new Genreal();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                try
                {

                    if (Request.QueryString["issueId"] != null)
                    {
                        string strissueId = Request.QueryString["issueId"].ToString();
                        string[] str = strissueId.Split(',');
                        int issueId = Convert.ToInt32(str[0].ToString());
                        DataSet ds = g.ReturnData1("Select st.issued_id, st.issue_type,  DATE_FORMAT(st.issued_date, '%d/%m/%Y') as issued_date, DATE_FORMAT(st.date_of_return, '%d/%m/%Y') as date_of_return, st.gauge_id, gt.gauge_name,gt.size_range, case when st.issued_status='OPEN' then 'PENDING' else st.issued_status end as issued_status, st.issued_to_type, case when st.issued_to_type='Employee' then em.employee_name else sp.supplier_name end as Name, dt.department_name from issued_status_TB as st Left Outer Join gaugeMaster_TB as gt ON st.gauge_id=gt.gauge_id Left Outer Join supplier_TB as sp ON st.issued_to_supplier_id=sp.supplier_id Left outer Join employee_TB as em ON st.issued_to_employee_id=em.employee_id Left Outer Join department_TB as dt ON st.department_id=dt.department_id where st.status=1 and st.issued_status <> 'RETURNED' and st.issued_id='" + issueId + "'");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/IssueStatusReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);
                        }
                    }
                    else if (Session["AllPrintData"] != null)
                    {
                        DataTable dt;
                        dt = (DataTable)Session["AllPrintData"];
                        if (dt.Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/IssueStatusReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", dt);
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