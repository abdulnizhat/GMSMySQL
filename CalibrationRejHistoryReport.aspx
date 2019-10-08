<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CalibrationRejHistoryReport.aspx.cs" Inherits="CalibrationRejHistoryReport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>Calibration Rejected Or Not Inused History</h3>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-5" id="divcust" runat="server" visible="false">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcust_SelectedIndexChanged" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>
            <div class="col-md-5" runat="server" id="divsortby">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label2" CssClass="col-md-4 control-label">Sort By</asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddtRejeOrNotInUsed" runat="server" CssClass="form-control" AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="ddtRejeOrNotInUsed_SelectedIndexChanged">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">NOT INUSE</asp:ListItem>
                                <asp:ListItem Value="2">REJECTED</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-md-12">
             <div class="col-md-5" id="divsearchby" runat="server">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Gauge-Wise</asp:ListItem>
                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>    
                                    <asp:ListItem>Size/Range-Wise</asp:ListItem>                                
                                   
                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                    </asp:DropDownList>
                                
                            </div>
                        </div>
                    </div>
                </div>

            <div class="col-md-5" id="divgauge" runat="server">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblName" CssClass="col-md-4 control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtsearch" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                             <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtsearch"
                                                    CssClass="text-danger" ErrorMessage="Required search value." />
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>



    <div class="row">
        <div class="col-md-10">
            <div class="col-md-offset-2 col-md-1">
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
                        <asp:Button runat="server" ID="btnExportToExcel" Enabled="false" Text="Export To Excel" OnClick="btnExportToExcel_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
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
                                    <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
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
                                                    <asp:PostBackTrigger ControlID="btnPrintCalibTranHistory" />
                                                    <asp:PostBackTrigger ControlID="LnkDownLoadDocument" />
                                                </Triggers>
                                                <ContentTemplate>

                                                    <asp:LinkButton ID="btnPrintCalibTranHistory" runat="server" OnClick="btnPrintCalibTranHistory_Click"
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
</asp:Content>

