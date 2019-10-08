<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="EmployeeReport.aspx.cs" Inherits="Report_EmployeeReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
    <h3><%: Title %>Employee Report</h3>
    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="col-md-5" id="divcust" runat="server" visible="false">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlcust_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="ddlcust"
                                Display="Dynamic" ErrorMessage="Select Customer" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                ValueToCompare="--Select--"></asp:CompareValidator>
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
                                    <asp:ListItem>Employee-Wise</asp:ListItem>
                                    <asp:ListItem>Mobile No.-Wise</asp:ListItem>
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
            <div class="col-md-5" runat="server" id="divDisplaySorting" visible="false">
                <div class="form-horizontal">
                    <div class="form-group" id="divemp" runat="server" visible="false">
                        <asp:Label ID="Label1" runat="server" CssClass="col-md-4 control-label">Employee Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtemp" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ValidationGroup="a" ControlToValidate="txtemp" SetFocusOnError="True"
                                Display="Dynamic" CssClass="text-danger" ErrorMessage="Employee Name field is required." />
                           
                             </div>
                    </div>
                    <div class="form-group" id="divMobile" runat="server" visible="false">
                        <asp:Label ID="Label2" runat="server" CssClass="col-md-4 control-label">Mobile No.<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="txtMobileNo" MaxLength="10" TabIndex="2" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" ControlToValidate="txtMobileNo" SetFocusOnError="True"
                                Display="Dynamic" CssClass="text-danger" ErrorMessage="Mobile Number field is required." />
                           
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-5">
                <div class="col-md-offset-4 col-md-2">
                   <asp:Button runat="server" ID="btnSearch" Text="Show" OnClick="btnSearch_Click" CssClass="btn btn-primary" TabIndex="3" ValidationGroup="a" />
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
    <div class="row">
        <div class="col-md-12">
            <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                <asp:GridView ID="grdemp" runat="server" Width="100%" AutoGenerateColumns="false"
                    CssClass="table table-bordered table-responsive" PageSize="10" OnPageIndexChanging="grdemp_PageIndexChanging" AllowPaging="true">
                    <HeaderStyle CssClass="header" />
                    <Columns>
                        <asp:BoundField DataField="employee_id" HeaderText="Emp Id" />
                        <asp:BoundField DataField="employee_name" HeaderText="Employee" />
                        <asp:BoundField DataField="mobile_no" HeaderText="Mobile No" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Address">
                            <ItemTemplate>
                                <asp:TextBox ID="txtadd" BackColor="#ffffad" runat="server" ReadOnly="true" TextMode="MultiLine" Text='<%# Bind("Address") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="department_name" HeaderText="Department" />
                        <asp:BoundField DataField="designation_name" HeaderText="Designation" />
                        <asp:BoundField DataField="branch_name" HeaderText="Branch" />
                        <asp:BoundField DataField="Customer_name" HeaderText="Customer" />
                        <asp:TemplateField HeaderText="Print">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnprintEmployee" Enabled="false" runat="server" OnClick="btnprintEmployee_Click"
                                    CommandArgument='<%# Eval("employee_id")%>' CssClass="btn btn-sm btn-info"
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

