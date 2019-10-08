using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmployeeReportViewer : System.Web.UI.Page
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
                    if (Request.QueryString["empid"] != null)
                    {
                        string empid = Request.QueryString["empid"].ToString();
                        string[] str = empid.Split(',');
                        int empId = Convert.ToInt32(str[0].ToString());
                        DataSet ds = g.ReturnData1(" Select em.employee_id,cm.customer_name,em.employee_name,dpt.department_name,dg.designation_name,bt.branch_name, em.mobile_no, em.email,em.address  +', '+ cn.country_name + ', '+ st.state_name+', '+ ct.city_name as address, CASE When em.status=1 then 'Active' Else 'Inactive' End as status from employee_TB as em Left Outer Join customer_TB as cm ON em.customer_id=cm.customer_id Left outer join countryMaster_TB as cn ON em.country_id=cn.country_Id Left Outer Join stateMaster_TB as st ON em.state_id=st.stateId Left Outer Join cityMaster_TB as ct ON em.city_id=ct.city_Id Left Outer Join branch_TB as bt ON em.branch_id=bt.branch_id Left Outer Join department_TB as dpt ON em.department_id=dpt.department_id Left Outer Join designation_TB as dg ON em.designation_id=dg.designation_id where em.employee_id='" + empId + "' ");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/EmployeeReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);
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