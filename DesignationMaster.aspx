<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DesignationMaster.aspx.cs" Inherits="DesignationMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <h3><%: Title %>Designation Master</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" ID="btnAddDesignation" Text="Add New" OnClick="btnAddDesignation_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-10" style="text-align: right" runat="server" visible="false">

                                <b>
                                    <asp:Label ID="lbl1" Visible="false" runat="server" Text="No. of Count :">
                                    </asp:Label>
                                    <asp:Label ID="lblcnt" Visible="false" runat="server">
                                    </asp:Label></b>

                            </div>

                            <div class="col-md-10">
                                 <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdDesignation" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdDesignation_PageIndexChanging" CssClass="table table-bordered table-responsive" PageSize="10">
                                  <HeaderStyle CssClass="header" />
                                       <Columns>
                                        <asp:BoundField DataField="designation_id" HeaderText="Designation Id" />
                                        <asp:BoundField DataField="designation_name" HeaderText="Designation Name" />
                                        <asp:BoundField DataField="department_name" HeaderText="Department Name" />
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditDesination" runat="server" CommandName="Upd" OnClick="btnEditDesination_Click"
                                                    CommandArgument='<%# Eval("designation_id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Designation Id</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtDesignationId" ReadOnly="true" CssClass="form-control" />
                                <asp:Label ID="lbldesig" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDesignationId"
                                    CssClass="text-danger" ErrorMessage="Designation ID field is required." />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Department Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDepartment" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Department Name field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Designation Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtDesignationName" CssClass="form-control" TabIndex="2"/>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDesignationName" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Designation Name field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="col-md-offset-2 col-md-6">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveDesignation" TabIndex="3" OnClick="btnSaveDesignation_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnCloseDesignation" TabIndex="4" Text="Close" OnClick="btnCloseDesignation_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

