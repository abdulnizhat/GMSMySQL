<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Authority.aspx.cs" Inherits="Authority" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>Authority Details</h3>
    <style>
        .space-left {
            margin-left: 15px;
        }
    </style>

    <div class="col-md-12">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-12">
                    <div class="col-md-3">
                        <asp:RadioButtonList ID="rbtnuserandoperation" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="rbtnuserandoperation_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem>User Wise</asp:ListItem>
                            <asp:ListItem class="space-left">Operation Name</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblAuthorityName" runat="server" Font-Bold="True"
                            Visible="false"></asp:Label>
                    </div>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlUserAuthority" runat="server" AutoPostBack="True"
                            CssClass="form-control"
                            Visible="false">                           
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddlUserAuthority" SetFocusOnError="true"
                                    ErrorMessage="Select User Name/ Operation Name" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>
                    </div>

                </div>

                <div class="form-group">
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <asp:Button ID="btnAuthority" runat="server" CssClass="btn btn-primary" ValidationGroup="a"
                                OnClick="btnAuthority_Click" Text="View Authority"
                                Visible="false" />
                        </div>
                        <div class="col-md-10">
                            <asp:RadioButtonList ID="chkselectall" runat="server" AutoPostBack="True"
                                RepeatDirection="Horizontal" CellSpacing="10"
                                OnSelectedIndexChanged="chkselectall_SelectedIndexChanged" Visible="false">
                                <asp:ListItem>Check All</asp:ListItem>
                                <asp:ListItem class="space-left">Uncheck All</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:Panel ID="pnlgag" runat="server" Width="100%" Height="350px" ScrollBars="Auto">
                            <asp:GridView ID="grduserandoperationwise" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="table table-bordered table-responsive">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Operation Wise">
                                        <ItemTemplate>
                                            <asp:Label ID="lblchildnode" runat="server" Text='<%# Bind("childnode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="User Name" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblusername" runat="server" Text='<%# Bind("username") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Create">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="allowcreate" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Modify">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="allowmodify" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="allowview" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--  <asp:TemplateField HeaderText="Lock">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="allowlock" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Employee Id" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempId" runat="server" Text='<%# Bind("employee_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Customer Id" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustId" runat="server" Text='<%# Bind("CustId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="User Name" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblchildId" runat="server" Text='<%# Bind("child_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="col-md-offset-4 col-md-6" style="padding-top: 10px">
                        <div class="col-md-3">
                            <asp:Button ID="btnupdate" Enabled="false" runat="server" CssClass="btn btn-primary" Text="Update Authority"
                                OnClick="btnupdate_Click" Visible="False" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnclear" runat="server" CssClass="btn btn-primary" Text="Close"
                                OnClick="btnclear_Click" />
                        </div>
                    </div>
                </div>


            </div>



        </div>

    </div>


</asp:Content>

