<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateUserLogin.aspx.cs" Inherits="CreateUserLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>User Information</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" ID="btnAddUser" Text="Add New" OnClick="btnAddUser_Click" CssClass="btn btn-primary" TabIndex="1" />
                            </div>
                        </div>
                        <div class="form-group">

                            <div class="col-md-10">
                                 <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdUser" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdUser_PageIndexChanging">

                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="user_master_id" HeaderText="User Id" />
                                        <asp:BoundField DataField="employee_name" HeaderText="Employee Name" />
                                        <asp:BoundField DataField="customer_name" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="user_name" HeaderText="User Name" />
                                        <%-- <asp:BoundField DataField="password" HeaderText="Password" />--%>
                                        <asp:BoundField DataField="role" HeaderText="User Type" />
                                        <asp:BoundField DataField="status" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditUser" Enabled="false" runat="server" OnClick="btnEditUser_Click" CommandName="Upd" CommandArgument='<%# Eval("user_master_id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No data exist</EmptyDataTemplate>
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
                <div class="col-md-offset-2 col-md-12">
                    <div class="form-horizontal">
                        <%-- <h4>User Login Form</h4>--%>
                        <br />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                            <asp:Label ID="lbluserid" runat="server" Visible="False"></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlcustomer" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlcustomer_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddlcustomer" SetFocusOnError="true"
                                    ErrorMessage="Select Customer" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Employee Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlemployee" runat="server" CssClass="form-control" TabIndex="2"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlemployee" SetFocusOnError="true"
                                    ErrorMessage="Select Employee" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">User Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtusername" CssClass="form-control" TabIndex="3" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtusername" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="User Name field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Password<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtpassword" CssClass="form-control" TextMode="Password" MaxLength="10" TabIndex="4" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtpassword" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Password field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">User Type<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddluserType" runat="server" CssClass="form-control" TabIndex="5"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddluserType" SetFocusOnError="true"
                                    ErrorMessage="Select User Type" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Status<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-3">
                                <asp:RadioButtonList ID="rd_status" runat="server" RepeatDirection="Horizontal" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Selected="True" Text="Active" Value="0">
                                    </asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="1">
                                    </asp:ListItem>
                                </asp:RadioButtonList>

                            </div>
                        </div>

                        <div class="col-md-offset-2 col-md-6">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveUser" OnClick="btnSaveUser_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="7" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnCloseUser" Text="Close" OnClick="btnCloseUser_Click" CssClass="btn btn-primary" TabIndex="8" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

