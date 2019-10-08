<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IssueStatus.aspx.cs" Inherits="IssueStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
   
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
            $("#<%= txtIssueDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtReturnDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function Confirm() {
            var chk = '<%=  Session["dleteIssuehLink"].ToString() %>';
            if (chk == "YES") {
                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("Do you want to delete Issue record?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            }

        }
    </script>
    <h3><%: Title %>Issue Status</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnAddIssue" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="col-md-1">
                                            <asp:Button runat="server" ID="btnAddIssue" Text="Add New" OnClick="btnAddIssue_Click" CssClass="btn btn-primary" />

                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Supplier Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Employee Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                  <%--  <asp:ListItem>Issue Id-Wise</asp:ListItem>--%>
                                                     <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>
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
                            <div class="col-md-12">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdIssueStatus" runat="server" Width="100%" AutoGenerateColumns="false"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdIssueStatus_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>

                                            <asp:BoundField DataField="issued_id" HeaderText="Issued Id" />
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                             <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                            <asp:BoundField DataField="issue_type" HeaderText="Issue For" />
                                            <asp:BoundField DataField="issued_to_type" HeaderText="Issued To" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" />
                                            <asp:BoundField DataField="issued_date" HeaderText="Issued Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="date_of_return" HeaderText="Exp. Return Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="issued_status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updd" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnEditIssueStatus" />
                                                            <asp:PostBackTrigger ControlID="lnkDelete" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnEditIssueStatus" runat="server" OnClick="btnEditIssueStatus_Click"
                                                                CommandArgument='<%# Eval("issued_id")%>' class="btn btn-sm btn-info"
                                                                title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                            &nbsp;
                                                <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="Confirm();" OnClick="lnkDelete_Click"
                                                    CommandArgument='<%# Eval("issued_id")%>' class="btn btn-sm btn-info"
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
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Issue Id</asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" ID="txtIssueId" ReadOnly="true" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtIssueId"
                                        Display="Dynamic" ValidationGroup="c" SetFocusOnError="true" CssClass="text-danger" ErrorMessage="Issue ID field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Gauge<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlGaugeName" TabIndex="1" runat="server" class="selectpicker dropdownCustom" data-live-search-style=""
                                    data-live-search="true">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator5" runat="server"
                                        ControlToValidate="ddlGaugeName" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Gauge" Operator="NotEqual" SetFocusOnError="true"
                                        ValidationGroup="c" ValueToCompare="--Select--">
                                    </asp:CompareValidator>
                                </div>
                            </div>
                              
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Issue Date<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" TabIndex="2" ID="txtIssueDate" CssClass="form-control" MaxLength="15" placeHolder="DD/MM/YYYY" />
                                    <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="c" runat="server" ControlToValidate="txtIssueDate"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Issue Date field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Issue Time<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" TabIndex="3" ID="txtIssueTime" TextMode="Time" CssClass="form-control" />
                                    <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="c" runat="server" ControlToValidate="txtIssueTime"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Issue Time field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Issue Type<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlIssueType" TabIndex="4" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">For CALIBRATION</asp:ListItem>
                                        <asp:ListItem Value="2">For REPAIR</asp:ListItem>
                                        <asp:ListItem Value="3">For USE</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddlIssueType" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Issue Type" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="0"></asp:CompareValidator>
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


                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Status<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlStatus" TabIndex="5" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">OPEN</asp:ListItem>
                                        <asp:ListItem Value="2">PENDING</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator6" runat="server"
                                        ControlToValidate="ddlStatus" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Status" Operator="NotEqual" SetFocusOnError="true"
                                        ValidationGroup="c" ValueToCompare="0">
                                    </asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Issued To<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlIssueTo" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlIssueTo" TabIndex="6" runat="server" OnSelectedIndexChanged="ddlIssueTo_SelectedIndexChanged"
                                                AutoPostBack="true" CssClass="form-control">
                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Supplier</asp:ListItem>
                                                <asp:ListItem Value="2">Employee</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator7" runat="server"
                                                ControlToValidate="ddlIssueTo" Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                                ErrorMessage="Select Issue To" Operator="NotEqual"
                                                ValidationGroup="c" ValueToCompare="0">
                                            </asp:CompareValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="form-group" id="divSupplier" runat="server" visible="false">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Supplier Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlSuplier" TabIndex="8" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlSuplier" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Supplier" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>

                            <div class="form-group" id="divEmployee" runat="server" visible="false">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Employee Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlEmployee" TabIndex="8" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlEmployee" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Employee" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group" id="divDepartment" runat="server" visible="false">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Department<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlDepartment" TabIndex="9" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="ddlDepartment" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Department" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Exp. Return Date<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <%-- <asp:UpdatePanel ID="updtxtdate" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="txtReturnDate" />
                                        </Triggers>
                                        <ContentTemplate>--%>
                                    <asp:TextBox runat="server" TabIndex="9" ID="txtReturnDate" CssClass="form-control" MaxLength="15" placeHolder="DD/MM/YYYY" />
                                    <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="c" runat="server" ControlToValidate="txtReturnDate"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Return Date field is required." />
                                    <%--  </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Label ID="lblIssuedId" runat="server" Visible="false"></asp:Label>
                    <div class="col-md-offset-5 col-md-6">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveIssue" TabIndex="10" ValidationGroup="c" OnClick="btnSaveIssue_Click" Text="Save" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnClloseIssue" TabIndex="11" CausesValidation="false" Text="Close" OnClick="btnClloseIssue_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

