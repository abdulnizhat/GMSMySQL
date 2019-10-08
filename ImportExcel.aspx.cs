using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;  //2.0
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
public partial class ImportExcel : System.Web.UI.Page
{
    Genreal g = new Genreal();
    OleDbConnection Econ;
    MySqlConnection con;
    string constr, Query = null;
    string formName = null;
    string[] addColumns = null;
    string tableName = null;
    int customerId = 0;
    public string filePathData = null;
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                bool Status = g.CheckSuperAdmin(Convert.ToInt32(Session["User_ID"]));
                //Check super Admin condition
                if (Status == true)
                {
                    divcust.Visible = true;
                    fillCustomer();
                    ddlcust.Focus();
                }
                else
                {
                    customerId = Convert.ToInt32(Session["Customer_ID"]);
                    divcust.Visible = false;
                }
            }
        }
        else
        {
            Logger.Error("Session expired.");
            Response.Redirect("Login.aspx");
        }
    }
    private void fillCustomer()
    {
        try
        {

            DataTable dtcust = q.GetCustomerNameId();
            ddlcust.DataSource = dtcust;
            ddlcust.DataTextField = "customer_name";
            ddlcust.DataValueField = "customer_id";
            ddlcust.DataBind();
            ddlcust.Items.Insert(0, "--Select--");

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    private void InsertExcelRecords(DataSet dataset)
    {
        try
        {
            con = new MySqlConnection(g.DBgetConnectionString());
            if (dataset.Tables[0] == null)
            {
                g.ShowMessage(this.Page, "Data is not found.");
                Logger.Error("Data is not found. ");
                return;
            }
            DataTable Exceldt = dataset.Tables[0];
            int cntdata = 0;
            //string stdatemsg = checkdate(Exceldt);
            string stdatemsg = "OK";
            if (stdatemsg != "OK")
            {
                g.ShowMessage(this.Page, stdatemsg);
                Logger.Error("Data is not found. ");
                return;
            }
            else
            {
                if (addColumns != null)
                {
                    #region Customer Master
                    if (formName == "Customer")
                    {
                        try
                        {
                            string stquery = "Insert into customer_TB (branch_id,country_id,state_id,city_id,created_by_id,status,customer_name,mobile_no,email,phone1,phone2,owner,license_no,websit,fax_no,address1,address2,pin_code,agent,logoname,logodata) Values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21)";
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                Byte[] imgByte = null;
                                g.savewith22param(stquery, Convert.ToInt32(Exceldt.Rows[i]["branch_id"].ToString()), Convert.ToInt32(Exceldt.Rows[i]["country_id"].ToString()), Convert.ToInt32(Exceldt.Rows[i]["state_id"].ToString()), Convert.ToInt32(Exceldt.Rows[i]["city_id"].ToString()), Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()), "1", Exceldt.Rows[i]["customer_name"].ToString(), Exceldt.Rows[i]["mobile_no"].ToString(), Exceldt.Rows[i]["email"].ToString(), Exceldt.Rows[i]["phone1"].ToString(), Exceldt.Rows[i]["phone2"].ToString(), Exceldt.Rows[i]["owner"].ToString(), Exceldt.Rows[i]["license_no"].ToString(), Exceldt.Rows[i]["websit"].ToString(), "", Exceldt.Rows[i]["address1"].ToString(), Exceldt.Rows[i]["address2"].ToString(), Exceldt.Rows[i]["pin_code"].ToString(), "", "", imgByte);
                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }
                    }
                    #endregion

                    #region Employee Master
                    if (formName == "Employee")
                    {
                        try
                        {
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                DataTable dtsave = g.ReturnData("Insert into employee_TB (customer_id,employee_name, department_id,designation_id,branch_id,mobile_no,email, address, country_id, state_id, city_id, created_by_id, status) Values( " + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + " ,'" + Exceldt.Rows[i]["employee_name"].ToString() + "', " + Convert.ToInt32(Exceldt.Rows[i]["department_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["designation_id"].ToString()) + " , " + Convert.ToInt32(Exceldt.Rows[i]["branch_id"].ToString()) + ", '" + Exceldt.Rows[i]["mobile_no"].ToString() + "', '" + Exceldt.Rows[i]["email"].ToString() + "', '" + Exceldt.Rows[i]["address"].ToString() + "', " + Convert.ToInt32(Exceldt.Rows[i]["country_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["state_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["city_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ",'1' )");
                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }
                    }



                    #endregion

                    #region Supplier
                    if (formName == "Supplier")
                    {
                        try
                        {
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                DataTable dtsave = g.ReturnData("Insert into supplier_TB (customer_id,supplier_name,supplier_type,address1,address2,certificate_details,contact_person,email_id,fax_no,mobile_no,phone1,phone2,product_portfolio,website,created_by_id,branch_id,country_id,state_id,city_id,status) values(" + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + ",'" + Exceldt.Rows[i]["supplier_name"].ToString() + "','" + Exceldt.Rows[i]["supplier_type"].ToString() + "','" + Exceldt.Rows[i]["address1"].ToString() + "','" + Exceldt.Rows[i]["address2"].ToString() + "','" + Exceldt.Rows[i]["certificate_details"].ToString() + "','" + Exceldt.Rows[i]["contact_person"].ToString() + "','" + Exceldt.Rows[i]["email_id"].ToString() + "','" + Exceldt.Rows[i]["fax_no"].ToString() + "','" + Exceldt.Rows[i]["mobile_no"].ToString() + "','" + Exceldt.Rows[i]["phone1"].ToString() + "','" + Exceldt.Rows[i]["phone2"].ToString() + "','" + Exceldt.Rows[i]["product_portfolio"].ToString() + "','" + Exceldt.Rows[i]["website"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["branch_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["country_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["state_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["city_id"].ToString()) + ",'1')");
                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }
                    }
                    #endregion
                    #region Part Master
                    if (formName == "Part Master")
                    {
                        try
                        {
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                DataTable dtsave = g.ReturnData("Insert into partmaster_tb (part_number, part_name, operation, drawing_revision_number, drawing_number, drawing_revision_date,customer_id,created_by_id,status) values('" + Exceldt.Rows[i]["part_number"].ToString() + "','" + Exceldt.Rows[i]["part_name"].ToString() + "','" + Exceldt.Rows[i]["operation"].ToString() + "','" + Exceldt.Rows[i]["drawing_revision_number"].ToString() + "','" + Exceldt.Rows[i]["drawing_number"].ToString() + "','" + Exceldt.Rows[i]["drawing_revision_date"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + "," + Convert.ToInt32(Session["created_by_id"]) + ",'1')");

                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }
                    }
                    #endregion
                    string gagId = "";
                    #region Gauge Master
                    if (formName == "Gauge Master")
                    {
                        try
                        {

                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {

                                //DataTable dtsave = g.ReturnData("Insert into gaugeMaster_TB (customer_id,gauge_sr_no,gauge_Manufature_Id,gauge_name,gauge_type,size_range,resolution,go_tollerance_plus,go_tollerance_minus,no_go_tollerance_plus,no_go_tollerance_minus,go_were_limit,least_count,permisable_error1,permisable_error2,store_location,current_location,purchase_cost,purchase_date,service_date,retairment_date,cycles,created_by_id,status,sapcode_no) Values( " + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + ",'" + Exceldt.Rows[i]["gauge_sr_no"].ToString() + "', '" + Exceldt.Rows[i]["gauge_Manufature_Id"].ToString() + "', '" + Exceldt.Rows[i]["gauge_name"].ToString() + "', '" + Exceldt.Rows[i]["gauge_type"].ToString() + "','" + Exceldt.Rows[i]["size_range"].ToString() + "','" + Exceldt.Rows[i]["resolution"].ToString() + "', '" + Exceldt.Rows[i]["go_tollerance_plus"].ToString() + "','" + Exceldt.Rows[i]["go_tollerance_minus"].ToString() + "', '" + Exceldt.Rows[i]["no_go_tollerance_plus"].ToString() + "','" + Exceldt.Rows[i]["no_go_tollerance_minus"].ToString() + "','" + Exceldt.Rows[i]["go_were_limit"].ToString() + "','" + Exceldt.Rows[i]["least_count"].ToString() + "','" + Exceldt.Rows[i]["permisable_error1"].ToString() + "','" + Exceldt.Rows[i]["permisable_error2"].ToString() + "','" + Exceldt.Rows[i]["store_location"].ToString() + "', '" + Exceldt.Rows[i]["current_location"].ToString() + "','" + Convert.ToDecimal(Exceldt.Rows[i]["purchase_cost"].ToString()) + "','" + Exceldt.Rows[i]["purchase_date"].ToString() + "','" + Exceldt.Rows[i]["service_date"].ToString() + "','" + Exceldt.Rows[i]["retairment_date"].ToString() + "', '" + Exceldt.Rows[i]["cycles"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ",'1','" + Exceldt.Rows[i]["sapcode_no"].ToString() + "')");
                               // DataTable dtsave = g.ReturnData("Update gaugeMaster_TB set  gauge_sr_no='" + Exceldt.Rows[i]["gauge_sr_no"].ToString() + "'   where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "");
                               
                                
                                //DataTable dtsave = g.ReturnData("Update gaugeMaster_TB set gauge_name='" + Exceldt.Rows[i]["gauge_name"].ToString() + "',  gauge_sr_no='" + Exceldt.Rows[i]["gauge_sr_no"].ToString() + "', gauge_Manufature_Id='" + Exceldt.Rows[i]["gauge_Manufature_Id"].ToString() + "' ,gauge_type='" + Exceldt.Rows[i]["gauge_type"].ToString() + "',size_range='" + Exceldt.Rows[i]["size_range"].ToString() + "',resolution='" + Exceldt.Rows[i]["resolution"].ToString() + "',go_tollerance_plus='" + Exceldt.Rows[i]["go_tollerance_plus"].ToString() + "',go_tollerance_minus='" + Exceldt.Rows[i]["go_tollerance_minus"].ToString() + "' ,no_go_tollerance_plus='" + Exceldt.Rows[i]["no_go_tollerance_plus"].ToString() + "' ,no_go_tollerance_minus='" + Exceldt.Rows[i]["no_go_tollerance_minus"].ToString() + "',go_were_limit='" + Exceldt.Rows[i]["go_were_limit"].ToString() + "',least_count='" + Exceldt.Rows[i]["least_count"].ToString() + "' ,permisable_error1='" + Exceldt.Rows[i]["permisable_error1"].ToString() + "',permisable_error2='" + Exceldt.Rows[i]["permisable_error2"].ToString() + "',store_location='" + Exceldt.Rows[i]["store_location"].ToString() + "',current_location='" + Exceldt.Rows[i]["current_location"].ToString() + "' ,purchase_cost='" +  Convert.ToDecimal(Exceldt.Rows[i]["purchase_cost"].ToString())+ "',purchase_date='" + Exceldt.Rows[i]["purchase_date"].ToString() + "',service_date='" + Exceldt.Rows[i]["service_date"].ToString() + "',retairment_date='" + Exceldt.Rows[i]["retairment_date"].ToString() + "',cycles='" + Exceldt.Rows[i]["cycles"].ToString() + "',sapcode_no='" + Exceldt.Rows[i]["sapcode_no"].ToString() + "' where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "");

                                DataTable dtsave = g.ReturnData("Update gaugeMaster_TB set gauge_type='" + Exceldt.Rows[i]["gauge_type"].ToString() + "',go_tollerance_plus='" + Exceldt.Rows[i]["go_tollerance_plus"].ToString() + "',go_tollerance_minus='" + Exceldt.Rows[i]["go_tollerance_minus"].ToString() + "' ,no_go_tollerance_plus='" + Exceldt.Rows[i]["no_go_tollerance_plus"].ToString() + "' ,no_go_tollerance_minus='" + Exceldt.Rows[i]["no_go_tollerance_minus"].ToString() + "',go_were_limit='" + Exceldt.Rows[i]["go_were_limit"].ToString() + "',permisable_error1='" + Exceldt.Rows[i]["permisable_error1"].ToString() + "',permisable_error2='" + Exceldt.Rows[i]["permisable_error2"].ToString() + "' where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "");

                                //DataTable dtNotExist = g.ReturnData("Select gauge_id from gaugeMaster_TB where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + " ");
                                //if (dtNotExist.Rows.Count > 0)
                                //{

                                //}
                                //else
                                //{
                                //    gagId = gagId + "," + Exceldt.Rows[i]["gauge_id"].ToString();
                                //    cntdata++;
                                //}
                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }
                    }
                    #endregion

                    #region Gauge Supplier
                    if (formName == "Gauge Supplier")
                    {
                        try
                        {
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                               DataTable dtsave = g.ReturnData("Insert into gauge_supplier_link_TB (customer_id,gauge_id,supplier_id,link_status,status,linked_date,created_by_id) Values(" + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["supplier_id"].ToString()) + ",'" + Exceldt.Rows[i]["link_status"].ToString() + "','1',  '" + Exceldt.Rows[i]["linked_date"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ")");
                                //DataTable dtsave = g.ReturnData("Update gauge_supplier_link_TB set supplier_id=" + Convert.ToInt32(Exceldt.Rows[i]["supplier_id"].ToString()) + ",  linked_date='" + Exceldt.Rows[i]["linked_date"].ToString() + "', created_by_id='" + Exceldt.Rows[i]["created_by_id"].ToString() + "'  where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + " and customer_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "");
                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }

                    }
                    #endregion

                    #region Gauge Part
                    if (formName == "Gauge Part")
                    {
                        try
                        {
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                
                                DataTable dtsave = g.ReturnData("Insert into gauge_part_link_tb (customer_id,gauge_id,part_id,part_number,linked_date,created_by_id,part_linked_status,status)  values(" + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["part_id"].ToString()) + ", '" + Exceldt.Rows[i]["part_number"].ToString() + "', '" + Exceldt.Rows[i]["linked_date"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + " , '" + Exceldt.Rows[i]["part_linked_status"].ToString() + "','1')");
                               // DataTable dtsave = g.ReturnData("Update gauge_part_link_tb set part_id=" + Convert.ToInt32(Exceldt.Rows[i]["part_id"].ToString()) + ",  linked_date='" + Exceldt.Rows[i]["linked_date"].ToString() + "', created_by_id='" + Exceldt.Rows[i]["created_by_id"].ToString() + "'  where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "");
                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }

                    }
                    #endregion

                    #region Issue Status
                    if (formName == "Issue Status")
                    {
                        try
                        {
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                DataTable dtsave = g.ReturnData("Insert into issued_status_TB (status,customer_id,created_by_id,issued_to_supplier_id, department_id,issued_to_employee_id,gauge_id,issued_status,issue_type,issued_date,date_of_return,issued_time,issued_to_type)   values('1'," + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["issued_to_supplier_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["department_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["issued_to_employee_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + ",'" + Exceldt.Rows[i]["issued_status"].ToString() + "','" + Exceldt.Rows[i]["issue_type"].ToString() + "', '" + Exceldt.Rows[i]["issued_date"].ToString() + "','" + Exceldt.Rows[i]["date_of_return"].ToString() + "','" + Exceldt.Rows[i]["issued_time"].ToString() + "','" + Exceldt.Rows[i]["issued_to_type"].ToString() + "')");


                                cntdata++;
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }

                    }
                    #endregion

                    #region  Return Issue code for update gauge master
                    if (formName == "Return Issue")
                    {

                        for (int i = 0; i < Exceldt.Rows.Count; i++)
                        {

                            DataTable dtexist = g.ReturnData("Select gauge_id from issued_status_TB where issued_id=" + Convert.ToInt32(Exceldt.Rows[i]["issued_id"].ToString()) + "");
                            DataTable checkgaugeid = g.ReturnData("Select cycles from gaugeMaster_TB where gauge_id=" + Convert.ToInt32(dtexist.Rows[0]["gauge_id"].ToString()) + "");
                            //var gaugeiddata = from dt in tts.issued_status_tbs
                            //                      where dt.issued_id == convert.toint32(exceldt.rows[i]["issued_id"].tostring())
                            //                      select new { dt.gauge_id };
                            //    foreach (var item in gaugeiddata)
                            //    {
                            //        gaugemaster_tb gtb = tts.gaugemaster_tbs.where(d => d.gauge_id == convert.toint32(item.gauge_id)).first();
                            if (checkgaugeid.Rows[0]["cycles"].ToString() == "0" || checkgaugeid.Rows[0]["cycles"].ToString() == null)
                            {
                                DataTable dtupdate = g.ReturnData("Update gaugeMaster_TB set cycles='" + Exceldt.Rows[i]["cycles"].ToString() + "' where gauge_id=" + Convert.ToInt32(dtexist.Rows[0]["gauge_id"].ToString()) + "");
                                //gtb.cycles = exceldt.rows[i]["cycles"].tostring();
                                //tts.submitchanges();
                                Logger.Info("gauge cycle updated.");
                            }
                            else
                            {
                                DataTable dtupdate = g.ReturnData("Update gaugeMaster_TB set cycles=" + (Convert.ToInt32(checkgaugeid.Rows[0]["cycles"].ToString()) + Convert.ToInt32(Exceldt.Rows[i]["cycles"].ToString())) + "' where gauge_id=" + Convert.ToInt32(dtexist.Rows[0]["gauge_id"].ToString()) + "");
                                //gtb.cycles = (convert.toint32(gtb.cycles) + convert.toint32(exceldt.rows[i]["cycles"].tostring())).tostring();
                                //tts.submitchanges();
                                Logger.Info("gauge cycle updated.");
                            }
                        }
                        // }


                    }
                    #endregion

                    #region Calibration Schedule
                    if (formName == "Calibration Schedule")
                    {

                        for (int i = 0; i < Exceldt.Rows.Count; i++)
                        {
                            try
                            {

                                string strFerqType = Exceldt.Rows[i]["frequency_type"].ToString();

                                int fereq = Convert.ToInt32(Exceldt.Rows[i]["calibration_frequency"].ToString());

                                DateTime dt = Convert.ToDateTime(Exceldt.Rows[i]["last_calibration_date"].ToString());

                                DateTime revDate = dt;

                                string nextDueDate = "";
                                string projectedDate = "";
                                if (strFerqType == "MONTH")
                                {
                                    // for Month
                                    DateTime nextDuedate = revDate.AddMonths(fereq);
                                    nextDueDate = nextDuedate.ToString("yyyy-MM-dd");
                                    DateTime projectedCalibSche = nextDuedate.AddMonths(fereq);
                                    projectedDate = projectedCalibSche.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    // for Year
                                    DateTime nextDuedateYear = revDate.AddYears(fereq);
                                    nextDueDate = nextDuedateYear.ToString("yyyy-MM-dd");
                                    DateTime projectedCalibScheYear = nextDuedateYear.AddYears(fereq);
                                    projectedDate = projectedCalibScheYear.ToString("yyyy-MM-dd");
                                }
                                try
                                {
                                    //DataTable dtselect = g.ReturnData("select calibration_schedule_id from calibration_schedule_tb where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + " limit 1");
                                    //DataTable dtsave = g.ReturnData("Update calibration_schedule_tb set status='0' where calibration_schedule_id=" + Convert.ToInt32(dtselect.Rows[0]["calibration_schedule_id"].ToString()) + " ");
                                    // Update by specific column
                                    //'" + Exceldt.Rows[i]["last_calibration_date"].ToString() + "','" + nextDueDate + "','" + projectedDate + "',
                                    //DataTable dtsave = g.ReturnData("Update calibration_schedule_tb set status='0' where calibration_schedule_id=" + Convert.ToInt32(Exceldt.Rows[i]["calibration_schedule_id"].ToString()) + " ");
                                    //With Calibration_schedule_id
                                    //DataTable dtsave = g.ReturnData("Insert into calibration_schedule_tb (calibration_schedule_id,customer_id,gauge_id,calibrate_id,last_calibrated_by,intial_time_used,frequency_type,calibration_frequency,calibration_hours,last_calibration_date,next_due_date,projected_calib_schedule,created_by_id,bias,linearity,stability,status)  values(" + Convert.ToInt32(Exceldt.Rows[i]["calibration_schedule_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["calibrate_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["last_calibrated_by"].ToString()) + ", '" + Exceldt.Rows[i]["intial_time_used"].ToString() + "','" + Exceldt.Rows[i]["frequency_type"].ToString() + "','" + Exceldt.Rows[i]["calibration_frequency"].ToString() + "', '" + Exceldt.Rows[i]["calibration_hours"].ToString() + "', '" + Exceldt.Rows[i]["last_calibration_date"].ToString() + "','" + nextDueDate + "','" + projectedDate + "', " + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ",'" + Exceldt.Rows[i]["bias"].ToString() + "','" + Exceldt.Rows[i]["linearity"].ToString() + "','" + Exceldt.Rows[i]["stability"].ToString() + "','1')");
                                    //Without Calibration_schedule_id
                                //  DataTable dtsave = g.ReturnData("Insert into calibration_schedule_tb (customer_id,gauge_id,calibrate_id,last_calibrated_by,intial_time_used,frequency_type,calibration_frequency,calibration_hours,last_calibration_date,next_due_date,projected_calib_schedule,created_by_id,bias,linearity,stability,status)  values(" + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["calibrate_id"].ToString()) + ", " + Convert.ToInt32(Exceldt.Rows[i]["last_calibrated_by"].ToString()) + ", '" + Exceldt.Rows[i]["intial_time_used"].ToString() + "','" + Exceldt.Rows[i]["frequency_type"].ToString() + "','" + Exceldt.Rows[i]["calibration_frequency"].ToString() + "', '" + Exceldt.Rows[i]["calibration_hours"].ToString() + "', '" + Exceldt.Rows[i]["last_calibration_date"].ToString() + "','" + nextDueDate + "','" + projectedDate + "', " + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ",'" + Exceldt.Rows[i]["bias"].ToString() + "','" + Exceldt.Rows[i]["linearity"].ToString() + "','" + Exceldt.Rows[i]["stability"].ToString() + "','1')");

                                    DataTable dtsave = g.ReturnData("Update calibration_schedule_TB set last_calibration_date='" + Exceldt.Rows[i]["last_calibration_date"].ToString() + "',next_due_date='" + nextDueDate + "',projected_calib_schedule='" + projectedDate + "' where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + ""); cntdata++;
                                }
                                catch (Exception ex)
                                {
                                    stdatemsg = ex.Message;
                                    //g.ShowMessage(this.Page, ex.Message);
                                }

                            }
                            catch (Exception ex)
                            {
                                stdatemsg = ex.Message;
                                //g.ShowMessage(this.Page, ex.Message);
                            }
                        }
                    }
                    #endregion
                    #region Calibration Transaction
                    int updCount = 0;
                    if (formName == "Calibration Transaction")
                    {
                        try
                        {
                            //DataTable dtIexistgauId = g.ReturnData("Select gauge_id from calibration_transaction_tb");
                            //for (int j = 0; j < dtIexistgauId.Rows.Count; j++)
                            //{
                            //    string id = dtIexistgauId.Rows[j]["gauge_id"].ToString();
                            //    if (Exceldt.Rows.Equals(id))
                            //    {
                                    
                            //    }
                          
                            //}s
                            for (int i = 0; i < Exceldt.Rows.Count; i++)
                            {
                                //DataTable dtIexistgauId = g.ReturnData("Select gauge_id from calibration_transaction_tb where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "");
                                //if (dtIexistgauId.Rows.Count > 0)
                                //{
                                //    DataTable dtupd = g.ReturnData("Update calibration_transaction_tb set calibration_date='" + Exceldt.Rows[i]["calibration_date"].ToString() + "',  calibration_status='" + Exceldt.Rows[i]["calibration_status"].ToString() + "', calibration_schedule_id=" +  Convert.ToInt32(Exceldt.Rows[i]["calibration_schedule_id"].ToString()) + "  where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + " ");
                                //    updCount++;
                                //}
                                //else
                                //{
                                //    DataTable dtsave = g.ReturnData("Insert into calibration_transaction_tb (customer_id, calibration_date,calibration_status,calibration_cost,calibration_hours,tollerance_go,tollerance_no_go,temprature,  humidity,pressure,certification_no,calibration_schedule_id,gauge_id,created_by_id,status)  Values(" + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + ",  '" + Exceldt.Rows[i]["calibration_date"].ToString() + "', '" + Exceldt.Rows[i]["calibration_status"].ToString() + "'," + Convert.ToDecimal(Exceldt.Rows[i]["calibration_cost"].ToString()) + ",'" + Exceldt.Rows[i]["calibration_hours"].ToString() + "','" + Exceldt.Rows[i]["tollerance_go"].ToString() + "', '" + Exceldt.Rows[i]["tollerance_no_go"].ToString() + "', '" + Exceldt.Rows[i]["temprature"].ToString() + "', '" + Exceldt.Rows[i]["humidity"].ToString() + "', '" + Exceldt.Rows[i]["pressure"].ToString() + "', '" + Exceldt.Rows[i]["certification_no"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["calibration_schedule_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ", '1')");
                                //   cntdata++;
                                //}
                                //Delete 
                                // DataTable dtdelete = g.ReturnData("delete  from calibration_transaction_tb where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + " ");
                                DataTable dtsave = g.ReturnData("Insert into calibration_transaction_tb (customer_id, calibration_date,calibration_status,calibration_cost,calibration_hours,tollerance_go,tollerance_no_go,temprature,  humidity,pressure,certification_no,calibration_schedule_id,gauge_id,created_by_id,status)  Values(" + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + ",  '" + Exceldt.Rows[i]["calibration_date"].ToString() + "', '" + Exceldt.Rows[i]["calibration_status"].ToString() + "'," + Convert.ToDecimal(Exceldt.Rows[i]["calibration_cost"].ToString()) + ",'" + Exceldt.Rows[i]["calibration_hours"].ToString() + "','" + Exceldt.Rows[i]["tollerance_go"].ToString() + "', '" + Exceldt.Rows[i]["tollerance_no_go"].ToString() + "', '" + Exceldt.Rows[i]["temprature"].ToString() + "', '" + Exceldt.Rows[i]["humidity"].ToString() + "', '" + Exceldt.Rows[i]["pressure"].ToString() + "', '" + Exceldt.Rows[i]["certification_no"].ToString() + "'," + Convert.ToInt32(Exceldt.Rows[i]["calibration_schedule_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + "," + Convert.ToInt32(Exceldt.Rows[i]["created_by_id"].ToString()) + ", '1')");
                                cntdata++;
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            stdatemsg = ex.Message;
                        }

                    }
                    #endregion

                    if (stdatemsg == "OK")
                    {
                        g.ShowMessage(this.Page,  cntdata + " Rows Imported Succefully.");
                    }
                    else
                    {
                        g.ShowMessage(this.Page, stdatemsg);
                    }
                   // g.ShowMessage(this.Page, gagId);
                }
                else
                {
                    g.ShowMessage(this.Page, "Column is not correct or not found. ");
                    //Logger.Info("SQL server connection is closed. ");
                }
                
                addColumns = null;
                tableName = null;
                Query = null;
                customerId = 0;
                formName = null;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
        finally { con.Dispose(); }

    }
    private string checkdate(DataTable Exceldt)
    {
        string stmsg = "";
        if (formName == "Part Master")
        {
            #region code for Part Master check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["drawing_revision_date"].ToString()))
                {
                    return stmsg = "Drawing revision date is empty.Date format should be (YYYY-MM-DD) on row postion " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["drawing_revision_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["drawing_revision_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Drawing revision date is not in correct format.Date format should be (YYYY-MM-DD) on row postion " + i;
                    }
                }
            }
            #endregion
        }
        else if (formName == "Gauge Master")
        {
            #region code for Gauge Master check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["purchase_date"].ToString()))
                {
                    return stmsg = "Purchase date is empty.Date format should be (YYYY-MM-DD) on row postion " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["purchase_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["purchase_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Purchase date is not in correct format.Date format should be (YYYY-MM-DD) on row postion " + i;
                    }
                }

                if (String.IsNullOrEmpty(Exceldt.Rows[i]["service_date"].ToString()))
                {
                    return stmsg = "Service date is empty.Date formate should be (YYYY-MM-DD) on row postion " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["service_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["service_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Service date is not in correct formate.Date formate should be (YYYY-MM-DD) on row postion " + i;
                    }
                }
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["retairment_date"].ToString()))
                {
                    return stmsg = "Retairment date is empty.Date formate should be (YYYY-MM-DD) on row postion " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["retairment_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["retairment_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Retairment date is not in correct formate.Date formate should be (YYYY-MM-DD) on row postion " + i;
                    }
                }
            }
            #endregion
        }
        else if (formName == "Gauge Supplier")
        {
            #region code for Gauge Supplier check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["linked_date"].ToString()))
                {
                    return stmsg = "Linked date is empty.Date format should be (YYYY-MM-DD) on row position " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["linked_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["linked_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Linked date is not in correct format.Date format should be (YYYY-MM-DD) on row position " + i;
                    }
                }
            }
            //string st = "";
            //for (int i = 0; i < Exceldt.Rows.Count; i++)
            //{
            //    try
            //    {
            //        DataTable dt = g.ReturnData("Select gauge_id from gaugeMaster_TB where gauge_id=" + Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString()) + " and customer_id= " + Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString()) + "  ");
            //        if (dt.Rows.Count == 0)
            //        {
            //            st = st + "," + Exceldt.Rows[i]["gauge_id"].ToString();

            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        return stmsg = ex.Message.ToString();
            //    }

            //}
            //if (st == "")
            //{
            //    stmsg = "OK";
            //}
            //else
            //{
            //    return stmsg = st;
            //    g.ShowMessage(this.Page, st);
            //}

            #endregion
        }
        else if (formName == "Gauge Part")
        {
            #region code for Gauge Part check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["linked_date"].ToString()))
                {
                    return stmsg = "Linked date is empty.Date format should be (YYYY-MM-DD) on row position " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["linked_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["linked_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Linked date is not in correct format.Date format should be (YYYY-MM-DD) on row position " + i;
                    }
                }
            }
            //string st = "";
            //for (int i = 0; i < Exceldt.Rows.Count; i++)
            //{
            //    try
            //    {
            //        DataTable dt = g.ReturnData("Select gauge_id from gaugemaster_tb where gauge_id='" + Exceldt.Rows[i]["gauge_id"].ToString() + "' and customer_id=22");
            //        if (dt.Rows.Count == 0)
            //        {
            //            st = st + "," + Exceldt.Rows[i]["gauge_id"].ToString();
            //            g.ShowMessage(this.Page, st);
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        return stmsg = ex.Message.ToString();
            //    }

            //}
            //if (st == "")
            //{
            //    stmsg = "OK";
            //}
            //else
            //{
            //    return stmsg = st;
            //    g.ShowMessage(this.Page, st);
            //}

            #endregion
        }

        else if (formName == "Issue Status")
        {
            #region code for Issue Status check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["issued_date"].ToString()))
                {
                    return stmsg = "Issued date is empty.Date format should be (YYYY-MM-DD) on row position " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["issued_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["issued_date"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Issued date is not in correct format.Date format should be (YYYY-MM-DD) on row position " + i;
                    }
                }
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["date_of_return"].ToString()))
                {
                    return stmsg = "Excepted Return date is empty.Date formate should be (YYYY-MM-DD) on row position " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["date_of_return"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["date_of_return"].ToString());
                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Excepted Return date is not in correct formate.Date formate should be (YYYY-MM-DD) on row position " + i;
                    }
                }

            }
            #endregion
        }
        else if (formName == "Return Issue")
        {
            #region code for Return Issue check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["return_date"].ToString()))
                {
                    return stmsg = "Returned date is empty.Date format should be (YYYY-MM-DD) on row position " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["return_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["return_date"].ToString());

                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Returned date is not in correct format.Date format should be (YYYY-MM-DD) on row position " + i;
                    }
                }
                if (!String.IsNullOrEmpty(Exceldt.Rows[i]["cycles"].ToString()))
                {
                    try
                    {
                        int dtcyc = Convert.ToInt32(Exceldt.Rows[i]["cycles"].ToString());

                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "Cycles should be integer value.On row position " + i;
                    }
                }
            }
            #endregion
        }
        else if (formName == "Customer")
        {
            stmsg = "OK";
        }
        else if (formName == "Employee")
        {
            stmsg = "OK";
        }
        else if (formName == "Supplier")
        {
            stmsg = "OK";
        }
        else if (formName == "Calibration Schedule")
        {
            #region code for Return Issue check date
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(Exceldt.Rows[i]["last_calibration_date"].ToString()))
                {
                    return stmsg = "last_calibration_date is empty.Date format should be (YYYY-MM-DD) on row position " + i;
                }
                else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["last_calibration_date"].ToString()))
                {
                    try
                    {
                        DateTime dtdate = Convert.ToDateTime(Exceldt.Rows[i]["last_calibration_date"].ToString());

                        stmsg = "OK";
                    }
                    catch (Exception)
                    {
                        return stmsg = "last_calibration_date is not in correct format.Date format should be (YYYY-MM-DD) on row position " + i;
                    }
                }
            }

            


            //for (int i = 0; i < Exceldt.Rows.Count; i++)
            //{
            //    int cid = 0;
            //    if (String.IsNullOrEmpty(Exceldt.Rows[i]["customer_id"].ToString()))
            //    {
            //        return stmsg = "customer_id is empty on row position " + i;
            //    }
            //    else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["customer_id"].ToString()))
            //    {
            //        try
            //        {
            //            cid = Convert.ToInt32(Exceldt.Rows[i]["customer_id"].ToString());
            //            stmsg = "OK";
            //        }
            //        catch (Exception)
            //        {
            //            return stmsg = "customer_id is not in correct format(Only numeric value put here) on row position " + i;
            //        }
            //    }

            //    else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["customer_id"].ToString()))
            //    {
            //        try
            //        {
            //            DataTable dtcust = g.ReturnData("Select customer_id from customer_TB where customer_id=" + cid + "");
            //            if (dtcust.Rows.Count > 0)
            //            {
            //                stmsg = "OK";
            //            }
            //            else
            //            {
            //                return stmsg = "customer_id is not in the database on row position " + i;
            //            }

            //        }
            //        catch (Exception)
            //        {
            //            return stmsg = "customer_id is not in the database on row position " + i;
            //        }
            //    }
            //}

            //for (int i = 0; i < Exceldt.Rows.Count; i++)
            //{
            //    int gid = 0;
            //    if (String.IsNullOrEmpty(Exceldt.Rows[i]["gauge_id"].ToString()))
            //    {
            //        return stmsg = "gauge_id is empty on row position " + i;
            //    }
            //    else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["gauge_id"].ToString()))
            //    {
            //        try
            //        {
            //            gid = Convert.ToInt32(Exceldt.Rows[i]["gauge_id"].ToString());
            //            stmsg = "OK";
            //        }
            //        catch (Exception)
            //        {
            //            return stmsg = "gauge_id  is not in correct format(Only numeric value put here) on row position " + i;
            //        }
            //    }

            //    else if (!String.IsNullOrEmpty(Exceldt.Rows[i]["gauge_id"].ToString()))
            //    {
            //        try
            //        {
            //            DataTable dtcust = g.ReturnData("Select gauge_id from gaugeMaster_TB where gauge_id=" + gid + "");
            //            if (dtcust.Rows.Count > 0)
            //            {
            //                stmsg = "OK";
            //            }
            //            else
            //            {
            //                return stmsg = "gauge_id is not in the database on row position " + i;
            //            }

            //        }
            //        catch (Exception)
            //        {
            //            return stmsg = "gauge_id is not in the database on row position " + i;
            //        }
            //    }
            //}
            #endregion
        }
        return stmsg = "OK";
    }
    protected void btnImportExcelFile_Click(object sender, EventArgs e)
    {

        string currentFilePath = "";
        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        string s = selectedFile();
        string[] st = s.Split(new[] { '/', '\\' }, StringSplitOptions.None);
        string stpath = "";
        for (int i = 0; i < st.Count() - 2; i++)
        {
            if (stpath == "")
            {
                stpath = st[i].ToString();
            }
            else
            {
                stpath = stpath + "/" + st[i].ToString();
            }
        }
        stpath = stpath + "/" + "ExcelFile" + "/" + st[st.Count() - 1].ToString();
        currentFilePath = st[st.Count() - 1].ToString();
        string filePath1 = Server.MapPath("~/ExcelFile/" + currentFilePath);
        filePathData = filePath1;
        Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath1);
        Microsoft.Office.Interop.Excel.Range oRng = null;
        try
        {

            DataSet ds = new DataSet();
            string columnname = "";
            int cnt = 0;
            foreach (Microsoft.Office.Interop.Excel.Worksheet oSheet in xlWorkbook.Sheets)
            {
                cnt++;
                if (cnt == 1)
                {

                    DataTable dt = new DataTable(oSheet.Name);

                    ds.Tables.Add(dt);

                    DataRow dr;
                    StringBuilder sb = new StringBuilder();
                    int jValue = oSheet.UsedRange.Cells.Columns.Count;
                    int iValue = oSheet.UsedRange.Cells.Rows.Count;

                    for (int j = 1; j <= jValue; j++)
                    {
                        oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[1, j];
                        string strValue = oRng.Text.ToString();

                        dt.Columns.Add(strValue);
                        if (columnname == "")
                        {
                            columnname = strValue;
                        }
                        else
                        {
                            if (strValue != " " || strValue != null)
                            {
                                columnname = columnname + "," + strValue;
                            }

                        }

                    }

                    for (int i = 2; i <= iValue; i++)
                    {
                        dr = ds.Tables[oSheet.Name].NewRow();
                        for (int j = 1; j <= jValue; j++)
                        {
                            oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[i, j];
                            string strValue = oRng.Text.ToString();
                            //for column
                            oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[1, j];
                            string columnValue = oRng.Text.ToString();
                            dr[columnValue] = strValue;
                        }
                        ds.Tables[oSheet.Name].Rows.Add(dr);
                    }
                }
            }
            addColumns = columnname.Split(',');
            // xlWorkbook.Close();


            if (currentFilePath == null)
            {
                Logger.Error("File is not found. ");
                g.ShowMessage(this.Page, "File is not found. ");
                return;
            }
            if (ddlcust.SelectedIndex > 0)
            {
                customerId = Convert.ToInt32(ddlcust.SelectedValue);
            }

            #region Import to Query
            if (ddlSelectForm.SelectedIndex > 0)
            {
                formName = ddlSelectForm.SelectedItem.Text;
                if (ddlSelectForm.SelectedIndex == 1)
                {
                    tableName = "customer_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 2)
                {
                    tableName = "employee_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 3)
                {
                    tableName = "supplier_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 4)
                {
                    tableName = "partMaster_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 5)
                {
                    tableName = "gaugeMaster_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 6)
                {
                    tableName = "gauge_supplier_link_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 7)
                {
                    tableName = "gauge_part_link_TB";
                }

                else if (ddlSelectForm.SelectedIndex == 8)
                {
                    tableName = "issued_status_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 9)
                {
                    tableName = "returned_status_TB";
                }

                else if (ddlSelectForm.SelectedIndex == 10)
                {
                    tableName = "calibration_schedule_TB";
                }
                else if (ddlSelectForm.SelectedIndex == 11)
                {
                    tableName = "calibration_transaction_TB";
                }
            }
            else
            {
                formName = null;

                g.ShowMessage(this.Page, "Please select option.");
                return;
            }
            #endregion
            bool isExistImportedFile = checkExistImportedFile(customerId, formName);
            if (isExistImportedFile == false)
            {
                string filePath = Server.MapPath("~/ExcelFile/" + currentFilePath);
                if (String.IsNullOrEmpty(filePath))
                {
                    Logger.Info("File is not found.");
                    g.ShowMessage(this.Page, "File is not found.");
                    return;
                }
                Logger.Info("Import excel file path is " + filePath + " .");
                InsertExcelRecords(ds);
            }
            else
            {
                Logger.Info("This data is already imported.");
                g.ShowMessage(this.Page, "This data is already imported. You may only one time import every form.");
            }
            xlWorkbook.Close();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
        finally
        {
            xlWorkbook = null;
            xlApp.Quit();
            xlApp = null;
        }
    }
    private bool checkExistImportedFile(int customer_Id, string form_Name)
    {
        try
        {
            DataTable dtexist = g.ReturnData("Select forms_name from import_history_TB where customer_id=" + customer_Id + " and forms_name='" + form_Name + "' and status=True");

            if (dtexist.Rows.Count > 0)
            {
                Logger.Info("Already imported." + " ");
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            return false;
        }
    }
    private string selectedFile()
    {
        #region File Upload
        string filename = null;
        string stfile = null;
        FileUpload excelfile = (FileUpload)ExcelFileUpload;
        if (excelfile.HasFile && excelfile.PostedFile != null)
        {
            stfile = Server.MapPath(ExcelFileUpload.FileName);

            //To create a PostedFile
            HttpPostedFile File = ExcelFileUpload.PostedFile;
            string filePath = ExcelFileUpload.PostedFile.FileName;
            filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            long fileSize = ExcelFileUpload.FileContent.Length;
            string contenttype = String.Empty;
            switch (ext)
            {
                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    filename = "ExcelFileXLS.xls";
                    Logger.Info("File type is .xls ");
                    break;
                case ".xlsx":
                    contenttype = "application/vnd.ms-excel";
                    filename = "ExcelFIleXLSX.xlsx";
                    Logger.Info("File type is .xlsx ");
                    break;
                case ".csv":
                    contenttype = "application/vnd.ms-excel";
                    filename = "CSVFILE.csv";
                    Logger.Info("File type is .csv ");
                    break;
            }
            if (contenttype == String.Empty)
            {
                Logger.Error("File type is not valid.");
                g.ShowMessage(this.Page, "File type is not valid. ");
                return filePath;
            }
            else
            {
                string subPath = "ExcelFile"; // your code goes here

                bool exists = Directory.Exists(Server.MapPath(subPath));
                if (!exists)
                    Directory.CreateDirectory(Server.MapPath(subPath));
                ExcelFileUpload.SaveAs(Server.MapPath("~/ExcelFile/" + filename));
            }
            // Logger.Info(filename +" ");
        }
        return stfile + "\\" + filename;
        #endregion
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

        // Confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.

    }
}
