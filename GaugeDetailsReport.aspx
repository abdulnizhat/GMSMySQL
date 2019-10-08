<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="GaugeDetailsReport.aspx.cs" Inherits="GaugeDetailsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>Gauge Details Report</h3>
    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="col-md-5" id="divcust" runat="server" visible="false">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlcust_SelectedIndexChanged" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlcust"
                                Display="Dynamic" CssClass="text-danger" ErrorMessage="Customer Name field is required." />
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
                                    <asp:ListItem>Gauge-Wise</asp:ListItem>
                                    <asp:ListItem>Gauge Type-Wise</asp:ListItem>
                                    <asp:ListItem>Size/Range-Wise</asp:ListItem>
                                    <asp:ListItem>Supplier Name-Wise</asp:ListItem>
                                    <asp:ListItem>Part No.-Wise</asp:ListItem>
                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>                                    
                                     <asp:ListItem>Current Location-Wise</asp:ListItem>
                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                    </asp:DropDownList>
                                <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlsortby"
                                    Display="Dynamic" ErrorMessage="Select search option" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>--%>
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
                    <asp:Button runat="server" ID="btnShowGaugeReport" ValidationGroup="a" Text="Show"
                        OnClick="btnShowGaugeReport_Click" CssClass="btn btn-primary" TabIndex="4" />
                    <br />
                </div>
                <div class="col-md-3">
                    <asp:UpdatePanel ID="updexport" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportToExcelGauge" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnExportToExcelGauge" Text="Export To Excel" OnClick="btnExportToExcelGauge_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
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
                                            <asp:Label runat="server"  Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Total Purchase Cost"></asp:Label>
                                        </td>
                                       
                                        <td align="center">
                                            <asp:Label runat="server" ID="lbltotalPCostlastMonth" Style="align-items: center; font-family: 'Times New Roman';" ></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lbltotalPCostLastYear" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lbltotalPCostcycleYTD" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
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
            <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                <asp:GridView ID="grdGaugeDetailsReport" runat="server" Width="100%" AutoGenerateColumns="false"
                    CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true"
                    OnPageIndexChanging="grdGaugeDetailsReport_PageIndexChanging">
                    <HeaderStyle CssClass="header" />
                    <Columns>
                        <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                        <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                        <asp:BoundField DataField="gauge_type" HeaderText="Gauge Type" />
                        <asp:BoundField DataField="part_number" HeaderText="Part Namber" />
                        <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                        <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No" />
                        <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                        <asp:BoundField DataField="purchase_cost" HeaderText="Purchase Cost" />
                        <asp:BoundField DataField="purchase_date" HeaderText="Purchase Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="current_location" HeaderText="Current Location" />
                        <asp:BoundField DataField="service_date" HeaderText="Service Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="last_calibration_date" HeaderText="Last Calib. Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="next_due_date" HeaderText="Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                        
                        <asp:BoundField DataField="employee_name" HeaderText="Created By" />
                        <asp:TemplateField HeaderText="Print">
                            <ItemTemplate>
                                <asp:UpdatePanel ID="updaction" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnPrintGaugeDetailsReport" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:LinkButton ID="btnPrintGaugeDetailsReport" Enabled="false" CommandName='<%# Eval("customer_id")%>'
                                            runat="server" OnClick="btnPrintGaugeDetailsReport_Click"
                                            CommandArgument='<%# Eval("gauge_id")%>' class="btn btn-sm btn-info"
                                            title="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
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



</asp:Content>

