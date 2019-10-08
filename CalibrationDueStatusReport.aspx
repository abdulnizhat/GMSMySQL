<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="CalibrationDueStatusReport.aspx.cs" Inherits="CalibrationDueStatusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" href="css/jquery-ui.css" type="text/css" media="all" />
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#<%= txtNextDueDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <h3><%: Title %>Calibration Due Status</h3>
     <div class="row">
    <div class="col-md-12">
        <div class="col-md-5" id="divcust" runat="server" visible="false">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlcust" runat="server" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ControlToValidate="ddlcust"
                            ValidationGroup="a" CssClass="text-danger" ErrorMessage="Customer Name field is required." />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="col-md-5">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblName" CssClass="col-md-4 control-label">Next Due Date<b style="color: Red">*</b></asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" TabIndex="2" placeHolder="DD/MM/YYYY" ID="txtNextDueDate" CssClass="form-control" />
                        <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtNextDueDate"
                            CssClass="text-danger" ErrorMessage="Next Due Date field is required." />
                    </div>
                </div>

            </div>
        </div>
         <div class="col-md-5">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblclocation" CssClass="col-md-4 control-label">Current Location</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" TabIndex="2" ID="txtcurrentlocation" CssClass="form-control" />
                       
                    </div>
                </div>

            </div>
        </div>
        </div>
    </div>
     <div class="row">
        <div class="col-md-10">
             <asp:UpdatePanel ID="updexport" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnShowCalibDueStatus" />
                            <asp:PostBackTrigger ControlID="BtnPrintAll" />
                            <asp:PostBackTrigger ControlID="btnExportToExcel" />
                        </Triggers>
                        <ContentTemplate>
            <div class="col-md-offset-2 col-md-1">
                <asp:Button runat="server" ID="btnShowCalibDueStatus" ValidationGroup="a"
                    OnClick="btnShowCalibDueStatus_Click" Text="Show" CssClass="btn btn-primary" />
                </div>
                <div class="col-md-1">
                 <asp:Button runat="server" ID="BtnPrintAll" OnClick="BtnPrintAll_Click" Text="Print All" Enabled="false" CssClass="btn btn-primary" />
                </div>
               <div class="col-md-3">
                
                         &nbsp;    <asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel" OnClick="btnExportToExcel_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
                       
                <br />
                </div>
                             </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
            
        </div>
     <br />
      <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="grdCalibrationDueStatus"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" runat="server" Width="100%"
                                AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10"
                                AllowPaging="true" OnPageIndexChanging="grdCalibrationDueStatus_PageIndexChanging">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:BoundField DataField="calibration_schedule_id" HeaderText="Schedule Id" />
                                    <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                    <asp:BoundField DataField="gauge_name" HeaderText="Gauge" />
                                     <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                     <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                    <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacturing Id" />                                   
                                    <asp:BoundField DataField="part_number" HeaderText="Part Number" />
                                    <asp:BoundField DataField="current_location" HeaderText="Current Location" />
                                    <asp:BoundField DataField="Calibrator" HeaderText="Calibrator" />
                                    <asp:BoundField DataField="LasCalibratedBy" HeaderText="Last Calibrated By" />
                                    <asp:BoundField DataField="cycles" HeaderText="Intial Time Used" />
                                    <asp:BoundField DataField="frequency_type" HeaderText="Frequency Type" />
                                    <asp:BoundField DataField="calibration_frequency" HeaderText="Calibration Frequency" />
                                    
                                    <asp:BoundField DataField="next_due_date" HeaderText="Next Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                    <asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnPrintCalibDueStatus" Enabled="false" runat="server" OnClick="btnPrintCalibDueStatus_Click"
                                                CommandArgument='<%# Eval("calibration_schedule_id")%>' CssClass="btn btn-sm btn-info"
                                                ToolTip="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>

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
</asp:Content>

