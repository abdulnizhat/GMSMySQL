<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DepartmentMaster.aspx.cs" Inherits="DepartmentMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <h3><%: Title %>Department Master</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" ID="btnAddDepartment" Text="Add New" OnClick="btnAddDepartment_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-10" style="text-align: right" runat="server" visible="false">

                                <b>
                                    <asp:Label ID="lbl1" runat="server" Text="No. of Count :">
                                    </asp:Label>
                                    <asp:Label ID="lblcnt" runat="server">
                                    </asp:Label></b>

                            </div>
                        </div>
                        <div class="form-group">

                            <div class="col-md-10">
 <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdDepartment" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdDepartment_PageIndexChanging" CssClass="table table-bordered table-responsive" PageSize="10">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="department_id" HeaderText="Department Id" />
                                        <asp:BoundField DataField="department_name" HeaderText="Department Name" />
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditDepartment" runat="server" OnClick="btnEditDepartment_Click" CommandName="Upd" CommandArgument='<%# Eval("department_id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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
                <div class="col-md-offset-2 col-md-12">
                    <div class="form-horizontal">
                        <%-- <h4>Branch Master</h4>--%>
                         <br />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Department Id</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtDepatmentId" ReadOnly="true" CssClass="form-control" />
                                <asp:Label ID="lbldeptid" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDepatmentId" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Department ID field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Department Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtDepatmentName" CssClass="form-control" TabIndex="1" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDepatmentName" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Department Name field is required." ValidationGroup="a" />
                            </div>
                        </div>

                        <div class="col-md-offset-2 col-md-6">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveDepartment" OnClick="btnSaveDepartment_Click" Text="Save" CssClass="btn btn-primary" TabIndex="2" ValidationGroup="a" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnCloseDept" Text="Close" OnClick="btnCloseDept_Click" CssClass="btn btn-primary" TabIndex="3" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

