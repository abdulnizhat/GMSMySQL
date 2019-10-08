<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="MSADueStatusReport.aspx.cs" Inherits="MSADueStatusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" href="css/jquery-ui.css" type="text/css" media="all" />
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#<%= txtMSADueDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <h3><%: Title %>MSA Due Status</h3>
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
                        <asp:Label runat="server" ID="lblName" CssClass="col-md-4 control-label">MSA Due Date<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" TabIndex="2" placeHolder="DD/MM/YYYY" ID="txtMSADueDate" CssClass="form-control" />
                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtMSADueDate"
                                CssClass="text-danger" ErrorMessage="MSA Due Date field is required." />
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
                        <asp:PostBackTrigger ControlID="btnExportToExcel" />
                         <asp:PostBackTrigger ControlID="BtnPrintAllMSA" />
                            <asp:PostBackTrigger ControlID="btnShowMSADueStatus" />
                    </Triggers>
                    <ContentTemplate>
            <div class="col-md-offset-2 col-md-1">
                <asp:Button runat="server" ID="btnShowMSADueStatus" ValidationGroup="a"
                    OnClick="btnShowMSADueStatus_Click" Text="Show" CssClass="btn btn-primary" />
                <br />
            </div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="BtnPrintAllMSA" OnClick="BtnPrintAllMSA_Click" Text="Print All" Enabled="false" CssClass="btn btn-primary" />

                <br />
            </div>
            <div class="col-md-2">
               
                        <asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel" OnClick="btnExportToExcel_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
                  
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
                            <asp:GridView ID="grdMSADueStatus"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" runat="server" Width="100%"
                                AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10"
                                AllowPaging="true" OnPageIndexChanging="grdMSADueStatus_PageIndexChanging">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:BoundField DataField="msa_schedule_id" HeaderText="Schedule Id" />
                                    <asp:BoundField DataField="last_calibration_date" HeaderText="MSA Scheduled Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="next_due_date" HeaderText="MSA Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                    <asp:BoundField DataField="gauge_name" HeaderText="Gauge" />
                                    <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                    <asp:BoundField DataField="current_location" HeaderText="Current Location" />
                                    <asp:BoundField DataField="Calibrator" HeaderText="MSA By" />
                                    <asp:BoundField DataField="cycles" HeaderText="Intial Time Used" />
                                    <asp:BoundField DataField="frequency_type" HeaderText="Frequency Type" />
                                    <asp:BoundField DataField="bias" HeaderText="Bias" />
                                    <asp:BoundField DataField="linearity" HeaderText="Linearity" />
                                    <asp:BoundField DataField="stability" HeaderText="Stability" />
                                    <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                    <asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnPrintMSADueStatus" Enabled="false" runat="server" OnClick="btnPrintMSADueStatus_Click"
                                                CommandArgument='<%# Eval("msa_schedule_id")%>' CssClass="btn btn-sm btn-info"
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

