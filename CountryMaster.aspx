<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CountryMaster.aspx.cs" Inherits="CountryMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>Country Master</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
           
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" ID="btnAddCountry" Text="Add New" OnClick="btnAddCountry_Click" CssClass="btn btn-primary" Visible="false" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-10" style="text-align: right" runat="server" Visible="false">

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
                                <asp:GridView ID="grdCountry" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive" PageSize="10" OnPageIndexChanging="grdCountry_PageIndexChanging">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="Country_id" HeaderText="Country Id" />
                                        <asp:BoundField DataField="Country_name" HeaderText="Country Name" />
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditCountry" runat="server" OnClick="btnEditCountry_Click" 
                                                    CommandArgument='<%# Eval("country_Id")%>' class="btn btn-sm btn-info" title="Update" Enabled="false"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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
            
        </asp:View>

        <asp:View ID="View2" runat="server">
            
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
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Country Id</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtCountryId" ReadOnly="true" CssClass="form-control" TabIndex="1"/>
                                <asp:Label ID="lblcountId" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCountryId" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Country ID field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Country Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtCountryname" CssClass="form-control" TabIndex="2"/>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCountryname" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Country Name field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="col-md-offset-2 col-md-6">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveCountry" OnClick="btnSaveCountry_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="3"/>
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnCloseCountry" Text="Close" OnClick="btnCloseCountry_Click" CssClass="btn btn-primary" TabIndex="4"/>
                            </div>
                        </div>
                    </div>
                </div>
          
        </asp:View>
    </asp:MultiView>

</asp:Content>

