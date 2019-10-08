<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="CalibrationHistoryReport.aspx.cs" Inherits="CalibrationHistoryReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
     <style type="text/css">
         .panel_popup
{
    border-style: double;
    border-width: 2px;
    border-radius: 15px;
    -moz-border-radius: 15px;
    padding-left: 20px;
    background-color: #DDDDDD;
    padding-right: 20px;
    box-shadow: 0px 1px 15px #092137;
}
    </style>
  
     <script type="text/javascript">
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
         </script>
    <h3><%: Title %>Calibration History</h3>
    <div class="row">
        <div class="col-md-12">
             <div class="form-horizontal">
            <div class="col-md-5" id="divcust" runat="server" visible="false">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcust_SelectedIndexChanged" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlcust"
                                Display="Dynamic" ValidationGroup="a" CssClass="text-danger" ErrorMessage="Customer Name field is required." />
                        </div>
                    </div>

                </div>
            </div>
              <div class="col-md-5" id="divsortby" runat="server">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-4 control-label">Sort By<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>                                  
                                    <asp:ListItem>Size/Range-Wise</asp:ListItem>                                  
                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>                                
                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                    </asp:DropDownList>                               
                            </div>
                        </div>
                    </div>
                </div>
              </div>
        </div>
    </div>
     
       
    <div class="row">
        <div class="col-md-12">
              <div class="col-md-5" id="divgauge" runat="server" visible="false">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblName" CssClass="col-md-4 control-label"></asp:Label>
                        <div class="col-md-8">
                           
                                                     <asp:TextBox ID="txtsearchValue" runat="server" ValidationGroup="a" TabIndex="3" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtsearchValue"
                                                    CssClass="text-danger" ErrorMessage="Required search value." />
                                                </div>
                       
                    </div>
                </div>
            </div>
             <div class="col-md-5">
            <div class="col-md-offset-4 col-md-2">
                <asp:Button runat="server" ID="btnShowCalibHistory" ValidationGroup="a"
                    OnClick="btnShowCalibHistory_Click" Text="Show" CssClass="btn btn-primary" TabIndex="3" />
                <br />
            </div>
            <div class="col-md-3">
                <asp:UpdatePanel ID="updexport" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExportToExcel" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel" OnClick="btnExportToExcel_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </div>
                 </div>
        </div>
    </div>
       <div class="form-horizontal">
                 <div class="col-md-6">
                   
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel9" runat="server" Width="100%" ScrollBars="Auto">
                                <table style="width: 100%; margin: 0px;" class="table table-bordered table-responsive">

                                    <tr class="header cssForExcluiveReportHeader">
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman'; font-weight:bold" Text="Details for"></asp:Label>
                                        </td>
                                       
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Last Month"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Last Year"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Year Till Date"></asp:Label>
                                        </td>

                                    </tr>
                                     
                                     <tr class="cssForRow2Exv">
                                        <td align="center">
                                          
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Total Calibration Cost"></asp:Label>
                                        </td>
                                       
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblcalibCostLastMonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblcalibCostLastYear" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblcalibCostYTD" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>

                                    </tr>
                                    
                                    
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="grdCalibTran" runat="server" Width="100%" AutoGenerateColumns="false"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" AllowPaging="true" OnPageIndexChanging="grdCalibTran_PageIndexChanging"
                                CssClass="table table-bordered table-responsive" PageSize="10">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:BoundField DataField="calibration_transaction_id" HeaderText="Tran. Id" />
                                    <asp:BoundField DataField="calibration_schedule_id" HeaderText="Scheduled Id" />
                                    <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />                                 
                                     <asp:TemplateField HeaderText="Gauge Name" >
                                                                 
                                                                 <ItemTemplate>
                                                                     <asp:LinkButton ID="lnkgaugename" runat="server" CausesValidation="False" ForeColor="Blue" Text='<%#Eval("gauge_name")%>'
                                                                          CommandArgument='<%#Eval("gauge_id")+ "~" + Eval("customer_id")%>' OnClick="lnkgaugename_Click"></asp:LinkButton>
                                                                     
                                                                 </ItemTemplate>
                                                                 
                                                             </asp:TemplateField>
                                    <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                     <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                    <asp:BoundField DataField="calibration_date" HeaderText="Calibration Date" DataFormatString="{0:dd/MM/yyyy}" />
                                     <asp:BoundField DataField="calibration_cost" HeaderText="Calibration Cost" />
                                    <asp:BoundField DataField="next_due_date" HeaderText="Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="calibration_cost" HeaderText="Calibrate Cost" />
                                    <asp:BoundField DataField="calibration_hours" HeaderText="Calibration Hrs" />
                                    <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                    <asp:BoundField DataField="calibration_status" HeaderText="Calibration Status" />
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="updLink" runat="server">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnPrintCalibTranHistory" />
                                                    <asp:PostBackTrigger ControlID="LnkDownLoadDocument" />
                                                </Triggers>
                                                <ContentTemplate>

                                                    <asp:LinkButton ID="btnPrintCalibTranHistory" Enabled="false" runat="server" OnClick="btnPrintCalibTranHistory_Click"
                                                        CommandArgument='<%# Eval("gauge_id")%>' class="btn btn-sm btn-info"
                                                        ToolTip="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="LnkDownLoadDocument" runat="server" OnClick="LnkDownLoadDocument_Click"
                                                        CommandArgument='<%# Eval("calibration_transaction_id")%>' class="btn btn-sm btn-info"
                                                        title="Download Calibration Document"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                      &nbsp;
                                                   <asp:LinkButton ID="btnEditCalibTran" runat="server" OnClick="btnEditCalibTran_Click"
                                                       CommandArgument='<%# Eval("calibration_transaction_id")%>' class="btn btn-sm btn-info"
                                                       title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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

     <table id="tblleaddetails" runat="server" style="display:none" Height="490px"
                                Width="900px" >
                    <tr>
                        <td>
                            <asp:Panel ID="Panel3" runat="server" CssClass="panel_popup" Height="490px"
                                Width="900px" ScrollBars="Both">
                               
                               
                                   
                                    <table style="width: 100%">
                                        <tr style="background-color: #FFFFFF">
                                            <td style="color: #800000; font-size: 18px; font-weight: bold; background-color: #FFFFFF;"
                                                align="center">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Calibration History
                                            </td>
                                            <td align="right" style="float: right; background-color: #FFFFFF;">
                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/Close.jpg"
                                                    Width="23px" />
                                            </td>
                                        </tr>
                                    </table>
                                   <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-12">
                        </div>
                    </div>
                </div>
            </div>
                              <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="grdhistory" runat="server" Width="100%" AutoGenerateColumns="false"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" AllowPaging="true" 
                                CssClass="table table-bordered table-responsive" PageSize="10">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:BoundField DataField="calibration_transaction_id" HeaderText="Tran. Id" />
                                    <asp:BoundField DataField="calibration_schedule_id" HeaderText="Scheduled Id" />
                                    <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                    <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                   
                                    <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                     <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                    <asp:BoundField DataField="calibration_date" HeaderText="Calibration Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="next_due_date" HeaderText="Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="calibration_cost" HeaderText="Calibrate Cost" />
                                    <asp:BoundField DataField="calibration_hours" HeaderText="Calibration Hrs" />
                                    <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                    <asp:BoundField DataField="calibration_status" HeaderText="Calibration Status" />
                                    <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="updLink" runat="server">
                                                <Triggers>
                                                    
                                                    <asp:PostBackTrigger ControlID="LnkDownLoadpopupdoc" />
                                                </Triggers>
                                                <ContentTemplate>

                                                    
                                                    <asp:LinkButton ID="LnkDownLoadpopupdoc" runat="server" OnClick="LnkDownLoadpopupdoc_Click"
                                                        CommandArgument='<%# Eval("calibration_transaction_id")%>' class="btn btn-sm btn-info"
                                                        title="Download Calibration Document"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                 
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
                                  
                                     </asp:Panel>
                </td>
                 </tr>
                 </table>
               
         
                <asp:Label ID="Labelpanb" runat="server" Text=""></asp:Label>
                <asp:ModalPopupExtender ID="ModalPopupExtenderCalibration" runat="server" TargetControlID="Labelpanb"
                    PopupControlID="tblleaddetails" CancelControlID="ImageButton4">
                </asp:ModalPopupExtender>
</asp:Content>

