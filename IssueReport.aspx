<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="IssueReport.aspx.cs" Inherits="IssueReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>Issue Report</h3>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-horizontal">
                    <div class="form-group" id="divcust" runat="server" visible="false">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlcust_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" ControlToValidate="ddlcust" 
                                SetFocusOnError="True" Display="Dynamic" CssClass="text-danger" ErrorMessage="Customer Name field is required." />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6" id="divsortby" runat="server">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Sort By<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>Supplier-Wise</asp:ListItem>
                                <asp:ListItem>Employee-Wise</asp:ListItem>
                                <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>
                                <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                            </asp:DropDownList>
                            
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-horizontal">                   
                    <div class="form-group" id="divIssueId" runat="server" visible="false">
                        <asp:Label ID="lblName" runat="server" CssClass="col-md-4 control-label"><b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="txtIssueId" MaxLength="10" TabIndex="3" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" ControlToValidate="txtIssueId" SetFocusOnError="True"
                                Display="Dynamic" CssClass="text-danger" ErrorMessage="Search Value is required." />

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

     <div class="row">
    <div class="col-md-12">
        <div class="col-md-offset-2 col-md-1">
        <asp:Button runat="server" ID="btnShowIssueReport" Text="Show" OnClick="btnShowIssueReport_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
        </div>
        <div class="col-md-1">
         <asp:Button runat="server" ID="btnPrintAll" Text="Print All" Enabled="false" OnClick="btnPrintAll_Click" CssClass="btn btn-primary" TabIndex="5" ValidationGroup="a" />
        <br />
         </div>
        <div class="col-md-2">
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
    
    <div class="row">
    <div class="col-md-12">
        <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
            <asp:GridView ID="grdIssueReport" runat="server" Width="100%"
                AutoGenerateColumns="false" AllowPaging="true"
                OnPageIndexChanging="grdIssueReport_PageIndexChanging"
                CssClass="table table-bordered table-responsive" PageSize="10">
                <HeaderStyle CssClass="header" />
                <Columns>
                    <asp:BoundField DataField="issued_id" HeaderText="Issued Id" />
                    <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                    <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                     <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                    <asp:BoundField DataField="issue_type" HeaderText="Issue For" />
                    <asp:BoundField DataField="issued_to_type" HeaderText="Issued To" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="issued_date" HeaderText="Issued Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="date_of_return" HeaderText="Exp. Return Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="issued_status" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Print">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnPrintIssue" Enabled="false" runat="server" OnClick="btnPrintIssue_Click"
                                CommandArgument='<%# Eval("issued_id")%>' CssClass="btn btn-sm btn-info"
                                title="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No data exist</EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>
    </div>
        </div>
</asp:Content>

