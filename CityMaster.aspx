<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CityMaster.aspx.cs" Inherits="CityMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h3><%: Title %>City Master</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" ID="btnAddCity" Text="Add New" OnClick="btnAddCity_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                         <div class="col-md-10" style="text-align:right" runat="server" Visible="false">
                          
                                <b>
                                                                <asp:Label ID="lbl1" runat="server" Text="No. of Count :">
                                                                </asp:Label>
                                            <asp:Label ID="lblcnt" runat="server">
                                            </asp:Label></b>
                            
                            </div>
                        <div class="form-group">

                            <div class="col-md-10">
                                 <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdCity" runat="server" Width="100%" AutoGenerateColumns="false" 
                                AllowPaging="true" OnPageIndexChanging="grdCity_PageIndexChanging" 
                                    CssClass="table table-bordered table-responsive" PageSize="10">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="city_Id" HeaderText="City Id"/>
                                        <asp:BoundField DataField="country_name" HeaderText="Country Name" />
                                        <asp:BoundField DataField="state_name" HeaderText="State Name" />
                                        <asp:BoundField DataField="city_name" HeaderText="City Name" />
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditCity" runat="server" OnClick="btnEditCity_Click" CommandArgument='<%# Eval("city_Id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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
                        <%-- <h4>Branch Master</h4>--%>
                        <br />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">City Id</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtCityId" ReadOnly="true" CssClass="form-control" />
                                <asp:Label ID="lblcityid" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCityId" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="City ID field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Country Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" TabIndex="1" CssClass="form-control" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlCountry" SetFocusOnError="true"
                                    ErrorMessage="Select Country" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">State Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlState" runat="server" TabIndex="2" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlState" SetFocusOnError="true"
                                    ErrorMessage="Select State" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                    ValueToCompare="--Select--"></asp:CompareValidator>
                            </div>
                        </div>
                      
                         <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">City Name<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtCity" CssClass="form-control" TabIndex="3"/>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="City Name field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="col-md-offset-2 col-md-6">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveCity" TabIndex="4" OnClick="btnSaveCity_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnCloseCity" TabIndex="5" Text="Close" OnClick="btnCloseCity_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>


            </div>

        </asp:View>
    </asp:MultiView>
</asp:Content>

