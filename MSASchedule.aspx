<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MSASchedule.aspx.cs" Inherits="MSASchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

  <%--  <link rel="stylesheet" href="css/jquery-ui.css" type="text/css" media="all" />
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>--%>

     <script type="text/javascript" src="js/searchabledropdown/jquery-1.9.1-jquery.min.js"></script>
     <link href="js/searchabledropdown/3.3.7-css-bootstrap.min.css" rel="stylesheet" />
     <script type="text/javascript" src="js/searchabledropdown/1.12.2-js-select.min.js"></script>
     <link href="js/searchabledropdown/1.12.2-css-select.min.css" rel="stylesheet" />
   
    <%--For Calendar--%>
    <link rel="stylesheet" href="css/jquery-ui.css" type="text/css" media="all" />
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <style>
        .dropdownCustom{
            width:280px !important;
        }
    </style>

    <script type="text/javascript">
        $(function () {

            $("#<%= txtLastCalibrationDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function onlyNos(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            catch (err) {
                alert(err.Description);
            }
        }

        function Confirm() {
            var chk = '<%= Session["dleteCalibrationSchLink"].ToString() %>';
            if (chk == "YES") {
                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("Do you want to delete calibration scheduled record?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            }

        }



    </script>
    <h3><%: Title %>MSA Schedule</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnAddCalibSchedule" />
                                    </Triggers>
                                    <ContentTemplate>
                                          <div class="col-md-1">
                                        <asp:Button runat="server" ID="btnAddCalibSchedule" Text="Add New" OnClick="btnAddCalibSchedule_Click" CssClass="btn btn-primary" />
                                     </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                     <asp:ListItem>Gauge Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>                                                  
                                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Type-Wise</asp:ListItem>
                                                    <asp:ListItem>Frequency Type-Wise</asp:ListItem>
                                                   
                                                    </asp:DropDownList>
                                              
                                            </div>

                                        </div>
                                        <div class="col-md-5" runat="server" id="divSearchBy">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="lblName" Visible="false"></asp:Label>
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="searchBy" Visible="false"></asp:Label>
                                            <div class="col-md-6">
                                                
                                                <div runat="server" id="divtxtsearch" visible="false">
                                                    <asp:TextBox ID="txtsearchValue" runat="server" ValidationGroup="a" TabIndex="3" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtsearchValue"
                                                        CssClass="text-danger" ErrorMessage="Required search value." />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" ValidationGroup="a" Visible="false" CssClass="btn btn-primary" />
                                        </div>
                                              </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12" style="text-align: right">
                                <b>
                                    <asp:Label ID="lbl1" Visible="false" runat="server" Text="No. of Count :">
                                    </asp:Label>
                                    <asp:Label ID="lblcnt" Visible="false" runat="server">
                                    </asp:Label></b>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdCalibrationSchedule"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" runat="server" Width="100%"
                                        AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10"
                                        AllowPaging="true" OnPageIndexChanging="grdCalibrationSchedule_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <%-- <asp:TemplateField HeaderText="Sr .No." HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Container.DataItemIndex + 1 %>' runat="server" ID="srno"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>--%>
                                            <asp:BoundField DataField="msa_schedule_id" HeaderText="Schedule Id" />
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                             <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                            <asp:BoundField DataField="supplier_name" HeaderText="MSA By" ItemStyle-Wrap="false"/>
                                            <asp:BoundField DataField="LasCalibratedBy" HeaderText="Last MSA By" ItemStyle-Wrap="false"/>
                                            <asp:BoundField DataField="cycles" HeaderText="Int.Time Used" />
                                            <asp:BoundField DataField="frequency_type" HeaderText="Freq.Type" />
                                            <asp:BoundField DataField="calibration_frequency" HeaderText="MSA.Freq" />
                                            <asp:BoundField DataField="calibration_hours" HeaderText="MSA.Hours" />
                                            <asp:BoundField DataField="last_calibration_date" HeaderText="Last MSA.Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="next_due_date" HeaderText="MSA Next Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                            <asp:BoundField DataField="employee_name" HeaderText="Created By" />
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updd" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnEditCalibSchedule" />
                                                            <asp:PostBackTrigger ControlID="lnkDelete" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnEditCalibSchedule" Enabled="false" runat="server" OnClick="btnEditCalibSchedule_Click"
                                                                CommandArgument='<%# Eval("msa_schedule_id")%>' class="btn btn-sm btn-info"
                                                                title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                            &nbsp;
                                                    <asp:LinkButton ID="lnkDelete" Enabled="false" Visible="false" OnClientClick="Confirm();" runat="server"
                                                        OnClick="lnkDelete_Click"
                                                        CommandArgument='<%# Eval("msa_schedule_id")%>' class="btn btn-sm btn-info"
                                                        title="Delete record"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>

        <asp:View ID="View2" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <br />
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="FailureText" />
                                </p>
                            </asp:PlaceHolder>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Schedule Id</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtCalibScheduleId" ReadOnly="true" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtCalibScheduleId"
                                        CssClass="text-danger" ErrorMessage="Calibration Schedule Id field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlGaugeID" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlGaugeID" runat="server" OnSelectedIndexChanged="ddlGaugeID_SelectedIndexChanged" AutoPostBack="true" class="selectpicker dropdownCustom" data-live-search-style=""
                                    data-live-search="true">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlGaugeID" Display="Dynamic" ForeColor="Maroon"
                                                ErrorMessage="Select Gauge" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Last MSA By<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlLastCalibratedBy" Enabled="false" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:Label ID="lblInitialValue" runat="server" Visible="false"></asp:Label>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">MSA By<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlCalibratedBy" runat="server" TabIndex="2" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddlCalibratedBy" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Calibratted By" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Initial Time Used<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="3" MaxLength="8" ID="txtInitialTimeUsed" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtInitialTimeUsed"
                                        CssClass="text-danger" SetFocusOnError="true" ValidationGroup="c" Display="Dynamic" ErrorMessage="Initial Time Used field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Hours</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="4" ID="txtCalibrationHours" onkeypress="return onlyNos(event,this);" MaxLength="8" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtCalibrationHours"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Calibration Hours field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Frequency Type<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlFrequencyType" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlFrequencyType" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ddlFrequencyType_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">MONTH</asp:ListItem>
                                                <asp:ListItem Value="2">YEAR</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator2" SetFocusOnError="true" runat="server" ControlToValidate="ddlFrequencyType" Display="Dynamic" ForeColor="Maroon"
                                                ErrorMessage="Select Frequency Type" Operator="NotEqual" ValidationGroup="c" ValueToCompare="0"></asp:CompareValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal1" />
                                </p>
                            </asp:PlaceHolder>

                            <asp:Label ID="lblsheduleID" runat="server" Visible="false"></asp:Label>


                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Frequency<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="txtCalibrationFrequency" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" onkeypress="return onlyNos(event,this);" TabIndex="6" MaxLength="2" OnTextChanged="txtCalibrationFrequency_TextChanged" AutoPostBack="true" ID="txtCalibrationFrequency" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCalibrationFrequency"
                                                CssClass="text-danger" SetFocusOnError="true" ValidationGroup="c" Display="Dynamic" ErrorMessage="MSA Frequency field is required." />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Last MSA Date<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="txtLastCalibrationDate" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" TabIndex="7" OnTextChanged="txtLastCalibrationDate_TextChanged" AutoPostBack="true" ID="txtLastCalibrationDate" placeHolder="DD/MM/YYYY" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" SetFocusOnError="true" Display="Dynamic" runat="server" ControlToValidate="txtLastCalibrationDate"
                                                CssClass="text-danger" ErrorMessage="Last MSA Date field is required." />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <%-- <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Projected Calibration Schedule</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtProjectedCalibrationSchedule" ReadOnly="true" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtProjectedCalibrationSchedule"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Projected Calibration Schedule field is required." />
                                </div>
                            </div>--%>



                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Next MSA Due Date</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtNextDueDate" ReadOnly="true" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtNextDueDate"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Next Due Date field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Bias<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="10" MaxLength="3" ID="txtBias" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBias"
                                        ValidationGroup="c" Display="Dynamic" CssClass="text-danger" ErrorMessage="Bias field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Linearity<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="11" MaxLength="3" ID="txtLinearity" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLinearity"
                                        ValidationGroup="c" Display="Dynamic" CssClass="text-danger" ErrorMessage="Linearity field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Stability<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" MaxLength="3" TabIndex="12" ID="txtSatability" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSatability"
                                        ValidationGroup="c" Display="Dynamic" CssClass="text-danger" ErrorMessage="Stability field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">R&R<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" MaxLength="3" TabIndex="12" ID="txtRR" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRR"
                                        ValidationGroup="c" Display="Dynamic" CssClass="text-danger" ErrorMessage="R&R field is required." />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-offset-5 col-md-6">
                        <div class="col-md-2">
                            <asp:UpdatePanel ID="updd" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSaveCalibSchedule" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button runat="server" ValidationGroup="c" TabIndex="13" ID="btnSaveCalibSchedule" OnClick="btnSaveCalibSchedule_Click" Text="Save" CssClass="btn btn-primary" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" TabIndex="14" CausesValidation="false" ID="btnClloseCalibSchedule" Text="Close" OnClick="btnClloseCalibSchedule_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

